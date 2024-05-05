using FlaschenpostAPI.Contracts.Responses;
using FlaschenpostAPI.Helper;
using FlaschenpostAPI.Models;
using Newtonsoft.Json;

namespace FlaschenpostAPI.Services
{
    public class ProductService : IProductService
    {
        private HttpClient _httpClient = new HttpClient();

        public async Task<List<Product>> ReadProductsFromGivenUrlAsync(string url)
        {

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonMessage = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(jsonMessage);
                
                if (products!=null && products.Count > 0)
                {
                    products = TransformData(products);
                    return products;
                }
            }

            return new List<Product>();
        }

        public List<ProductResponse> SelectMostExpensiveAndCheapestProduct(List<Product> products)
        {
            // using lists, because multiple products could have the exact same litre price
            var expensiveList = new List<ProductResponse>();
            var cheapestList = new List<ProductResponse>();


            var expensiveTempPrice = double.MinValue;
            var cheapestTempPrice = double.MaxValue;

            foreach (var product in products)
            {
                if(product.Articles!=null)
                {
                    foreach (var article in product.Articles)
                    {
                        if (article.PricePerLitre > 0)
                        {

                            if (article.PricePerLitre >= expensiveTempPrice)
                            {
                                if(article.PricePerLitre > expensiveTempPrice)
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
                
            }

            var response = new List<ProductResponse>();
            response.AddRange(expensiveList);
            response.AddRange(cheapestList);

            return response;
        }

        public List<ProductResponse> SelectProductsForSpecificPrice(List<Product> products, double price)
        {
            var response = new List<ProductResponse>();

            foreach (var product in products)
            {
                if (product.Articles != null)
                {
                    foreach (var article in product.Articles)
                    {
                        if(article.Price==price)
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

            response.OrderBy(r => r.Article?.PricePerLitre);


            return response;
        }

        public List<ProductResponse> SelectProductsWithTheMostBottles(List<Product> products)
        {
            var response = new List<ProductResponse>();
            var mostBottles = 0;

            foreach (var product in products)
            {
                if (product.Articles != null)
                {
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
                            else if(article.Bottles == mostBottles)
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
            }
            return response;
        }

        public List<ProductResponse> SelectProductsWithAllQuestions(List<Product> products, double price)
        {
            var expensiveAndCheapestProduct = SelectMostExpensiveAndCheapestProduct(products);

            var productsforPrice = SelectProductsForSpecificPrice(products, price);

            var productsWithTheMostBottles = SelectProductsWithTheMostBottles(products);
            
            

            var response = new List<ProductResponse>();

            response.AddRange(expensiveAndCheapestProduct);
            response.AddRange(productsforPrice);
            response.AddRange(productsWithTheMostBottles);

            // remove duplicates?

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
