using FlaschenpostAPI.Contracts.Responses.ExpensiveAndCheapest;
using FlaschenpostAPI.Contracts.Responses.General;

namespace FlaschenpostAPI.Contracts.Responses.AllQuestions
{
    public class AllQuestionsResponse
    {
        public MostExpensiveAndCheapestProductResponse? MostExpensiveAndCheapestProducts { get; set; }

        public List<ProductResponse>? ProductsForSpecificPrice { get; set; }

        public List<ProductResponse>? ProductsWithMostBottles { get; set; }
    }
}
