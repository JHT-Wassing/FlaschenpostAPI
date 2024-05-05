using FlaschenpostAPI.Contracts.Responses.AllQuestions;
using FlaschenpostAPI.Contracts.Responses.ExpensiveAndCheapest;
using FlaschenpostAPI.Contracts.Responses.General;
using FlaschenpostAPI.Data.Models;

namespace FlaschenpostAPI.Services
{
    public interface IProductService
    {
        Task<MostExpensiveAndCheapestProductResponse> SelectMostExpensiveAndCheapestProduct(string url);
        MostExpensiveAndCheapestProductResponse SelectMostExpensiveAndCheapestProduct(List<Product> products);

        Task<List<ProductResponse>> SelectProductsForSpecificPrice(string url, double price);
        List<ProductResponse> SelectProductsForSpecificPrice(List<Product> products, double price);

        Task<List<ProductResponse>> SelectProductsWithTheMostBottles(string url);
        List<ProductResponse> SelectProductsWithTheMostBottles(List<Product> products);

        Task<AllQuestionsResponse> SelectProductsWithAllQuestions(string url, double price);
        AllQuestionsResponse SelectProductsWithAllQuestions(List<Product> products, double price);

    }
}
