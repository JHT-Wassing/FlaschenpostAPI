using FlaschenpostAPI.Services;
using Microsoft.AspNetCore.Mvc;

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
        /// Read Data from given URL and deserialize
        /// Select most expensive and cheapest product (beer per litre).
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        /// Status 200
        /// Json List of products with 2 elements. Most cheapest first
        /// </returns>
        [HttpGet("GetMostExpensiveAndCheapestProduct")]
        public async Task<IActionResult> GetMostExpensiveAndCheapestProduct(string url)
        {
            var response = await _productService.SelectMostExpensiveAndCheapestProduct(url);

            return Ok(response);

        }

        /// <summary>
        /// Read Data from given URL and deserialize
        /// Select Products with exact given price
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        /// Status 200
        /// List of Products with the given price (for example 17,99€)
        /// </returns>
        [HttpGet("GetProductsForSpecificPrice")]
        public async Task<IActionResult> GetProductsForSpecificPrice(string url, double price)
        {

            var response = await _productService.SelectProductsForSpecificPrice(url, price);

            return Ok(response);

        }

        /// <summary>
        /// Read Data from given URL and deserialize
        /// Select product with most Bottles in articles
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        /// Status 200
        /// One Product with the most bottles in article
        /// </returns>
        [HttpGet("GetProductsWithTheMostBottles")]
        public async Task<IActionResult> GetProductsWithMostBottles(string url)
        {

            var response = await _productService.SelectProductsWithTheMostBottles(url);

            return Ok(response);

        }

        /// <summary>
        /// Read Data from given URL and deserialize
        /// Answer all questions from above
        /// Important: The response is a concatinated list of 3 questions. Not combined!
        /// </summary>
        /// <param name="url"></param>
        /// <returns>
        /// Status 200
        /// List of Products:
        /// - most expensive and cheapest product
        /// + most bottles in articles
        /// + products with the given price
        /// </returns>
        [HttpGet("GetProductsOfAllQuestions")]
        public async Task<IActionResult> GetProductsOfAllQuestions(string url, double price)
        {

            var response = await _productService.SelectProductsWithAllQuestions(url, price);

            return Ok(response);

        }

    }
}
