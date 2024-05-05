using Microsoft.AspNetCore.Mvc;
using FlaschenpostAPI.Services;

namespace FlaschenpostAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Read Data from given URL and desirialize
        /// Select most expensive and cheapest product (beer per litre).
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        /// Json List of products with 2 elements. Most cheapest first
        /// </returns>
        [HttpGet(template: "GetMostExpensiveAndCheapestProduct")]
        public async Task<IActionResult> GetMostExpensiveAndCheapestProduct(string url)
        {
            var products = await _productService.ReadProductsFromGivenUrlAsync(url);

            var response = _productService.SelectMostExpensiveAndCheapestProduct(products);

            return Ok(response);

        }

        /// <summary>
        /// Read Data from given URL and desirialize
        /// Select product with most Bottles in articles
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        /// One Product
        /// </returns>
        [HttpGet("GetProductsWithTheMostBottles")]
        public async Task<IActionResult> GetProductsWithMostBottles(string url)
        {
            var products = await _productService.ReadProductsFromGivenUrlAsync(url);

            var response = _productService.SelectProductsWithTheMostBottles(products);

            return Ok(response);

        }

        /// <summary>
        /// Read Data from given URL and desirialize
        /// Select Products with exact price of 17,99€
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        /// List of Products with the Price of 17,99€
        /// </returns>
        [HttpGet("GetProductsForSpecificPrice")]
        public async Task<IActionResult> GetProductsFor1799(string url, double price)
        {
            var products = await _productService.ReadProductsFromGivenUrlAsync(url);

            var response = _productService.SelectProductsForSpecificPrice(products, price);

            return Ok(response);

        }

        /// <summary>
        /// Read Data from given URL and desirialize
        /// Answer all questions from above
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        /// List of Products:
        /// - most expensive and cheapest product
        /// - most bottles in articles
        /// - products with the price of 17,99€
        /// </returns>
        [HttpGet("GetProductsOfAllQuestions")]
        public async Task<IActionResult> GetProductsOfAllQuestions(string url, double price)
        {
            var products = await _productService.ReadProductsFromGivenUrlAsync(url);

            var response = _productService.SelectProductsWithAllQuestions(products, price);

            return Ok(response);

        }

    }
}
