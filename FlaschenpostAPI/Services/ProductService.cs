using FlaschenpostAPI.Contracts.Responses.AllQuestions;
using FlaschenpostAPI.Contracts.Responses.ExpensiveAndCheapest;
using FlaschenpostAPI.Contracts.Responses.General;
using FlaschenpostAPI.Data.Models;
using FlaschenpostAPI.Data.Repos;
using FlaschenpostAPI.Helper;

namespace FlaschenpostAPI.Services
{
    public class ProductService : IProductService
    {

        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<MostExpensiveAndCheapestProductResponse> SelectMostExpensiveAndCheapestProduct(string url)
        {
            var products = await _productRepository.GetProductsFromGivenUrlAsync(url);

            return SelectMostExpensiveAndCheapestProduct(products);
        }

        public MostExpensiveAndCheapestProductResponse SelectMostExpensiveAndCheapestProduct(List<Product> products)
        {
            products = TransformData(products);

            // using lists, because multiple products could have the exact same litre price
            var expensiveList = new List<ProductResponse>();
            var cheapestList = new List<ProductResponse>();


            var expensiveTempPrice = double.MinValue;
            var cheapestTempPrice = double.MaxValue;

            foreach (var product in products)
            {
                if (product.Articles == null) continue;

                foreach (var article in product.Articles)
                {
                    if (article.PricePerLitre > 0)
                    {
                        if (article.PricePerLitre >= expensiveTempPrice)
                        {
                            if (article.PricePerLitre > expensiveTempPrice)
                            {
                                expensiveTempPrice = article.PricePerLitre;
                                //expensiveList.Clear();
                                expensiveList = new List<ProductResponse>();
                            }

                            expensiveList.Add(new ProductResponse()
                            {
                                Id = product.Id,
                                BrandName = product.BrandName,
                                Name = product.Name,
                                DescriptionText = product.DescriptionText,
                                Article = article
                            });
                        }

                        if (article.PricePerLitre <= cheapestTempPrice)
                        {
                            if (article.PricePerLitre < cheapestTempPrice)
                            {
                                cheapestTempPrice = article.PricePerLitre;
                                //cheapestList.Clear();
                                cheapestList = new List<ProductResponse>();
                            }

                            cheapestList.Add(new ProductResponse()
                            {
                                Id = product.Id,
                                BrandName = product.BrandName,
                                Name = product.Name,
                                DescriptionText = product.DescriptionText,
                                Article = article
                            });
                        }
                    }
                }
            }

            var response = new MostExpensiveAndCheapestProductResponse()
            {
                MostExpensiveProducts = expensiveList,
                CheapestProducts = cheapestList
            };

            return response;
        }

        public async Task<List<ProductResponse>> SelectProductsForSpecificPrice(string url, double price)
        {
            var products = await _productRepository.GetProductsFromGivenUrlAsync(url);

            return SelectProductsForSpecificPrice(products, price);
        }

        public List<ProductResponse> SelectProductsForSpecificPrice(List<Product> products, double price)
        {

            products = TransformData(products);

            var response = new List<ProductResponse>();

            foreach (var product in products)
            {
                if (product.Articles == null) continue;

                foreach (var article in product.Articles)
                {
                    if (article.Price == price)
                    {
                        response.Add(new ProductResponse()
                        {
                            Id = product.Id,
                            BrandName = product.BrandName,
                            Name = product.Name,
                            DescriptionText = product.DescriptionText,
                            Article = article
                        });
                    }
                }
            }
            // sort by price per litre 
            return response.OrderBy(r => r.Article?.PricePerLitre).ToList();
        }


        public async Task<List<ProductResponse>> SelectProductsWithTheMostBottles(string url)
        {
            var products = await _productRepository.GetProductsFromGivenUrlAsync(url);
            return SelectProductsWithTheMostBottles(products);
        }

        public List<ProductResponse> SelectProductsWithTheMostBottles(List<Product> products)
        {
            var response = new List<ProductResponse>();
            var mostBottles = 0;

            products = TransformData(products);


            foreach (var product in products)
            {
                if (product.Articles == null) continue;

                foreach (var article in product.Articles)
                {

                    if (article.Bottles > 0)
                    {
                        // if bottles greater than the temp mostBottles, create new List of Products
                        if (article.Bottles > mostBottles)
                        {
                            mostBottles = article.Bottles;

                            response = new List<ProductResponse>
                            {
                                new ProductResponse()
                                {
                                    Id = product.Id,
                                    BrandName = product.BrandName,
                                    Name = product.Name,
                                    DescriptionText = product.DescriptionText,
                                    Article = article
                                }
                            };
                        }
                        // if bottles the same amount as mostBottles, then add to List 
                        // --> It may be possible, that there are more than one Product with the same max amount of bottles
                        else if (article.Bottles == mostBottles)
                        {
                            response.Add(new ProductResponse()
                            {
                                Id = product.Id,
                                BrandName = product.BrandName,
                                Name = product.Name,
                                DescriptionText = product.DescriptionText,
                                Article = article
                            });
                        }

                    }
                }

            }
            return response;
        }

        public async Task<AllQuestionsResponse> SelectProductsWithAllQuestions(string url, double price)
        {
            var products = await _productRepository.GetProductsFromGivenUrlAsync(url);

            return SelectProductsWithAllQuestions(products, price);
        }

        public AllQuestionsResponse SelectProductsWithAllQuestions(List<Product> products, double price)
        {

            products = TransformData(products);

            var expensiveAndCheapestProduct = SelectMostExpensiveAndCheapestProduct(products);

            var productsforPrice = SelectProductsForSpecificPrice(products, price);

            var productsWithTheMostBottles = SelectProductsWithTheMostBottles(products);

            var response = new AllQuestionsResponse()
            {
                MostExpensiveAndCheapestProducts = expensiveAndCheapestProduct,
                ProductsForSpecificPrice = productsforPrice,
                ProductsWithMostBottles = productsWithTheMostBottles
            };

            return response;
        }


        /// <summary>
        /// Transforming the Model with additional Informations, so it can be used for further selection
        /// </summary>
        /// <param name="products"></param>
        /// <returns>list of products with transformed / added informations</returns>
        private List<Product> TransformData(List<Product> products)
        {
            foreach (var product in products)
            {
                if (product.Articles != null)
                {
                    foreach (var article in product.Articles)
                    {

                        article.PricePerLitre = ProductHelper.GetPricePerLitre(article.PricePerUnitText);
                        article.Bottles = ProductHelper.GetBottles(article.ShortDescription);
                    }
                }

            }

            return products;
        }

    }
}
