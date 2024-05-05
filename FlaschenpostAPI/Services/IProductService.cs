using FlaschenpostAPI.Contracts.Responses;
using FlaschenpostAPI.Models;

namespace FlaschenpostAPI.Services
{
    public interface IProductService
    {

        Task<List<Product>> ReadProductsFromGivenUrlAsync(string url);

        List<ProductResponse> SelectMostExpensiveAndCheapestProduct(List<Product> products);

        List<ProductResponse> SelectProductsForSpecificPrice(List<Product> products, double price);

        List<ProductResponse> SelectProductsWithTheMostBottles(List<Product> products);

        List<ProductResponse> SelectProductsWithAllQuestions(List<Product> products, double price);

    }
}
