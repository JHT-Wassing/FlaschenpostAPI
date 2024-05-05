using FlaschenpostAPI.Contracts.Responses.General;

namespace FlaschenpostAPI.Contracts.Responses.ExpensiveAndCheapest
{
    public class MostExpensiveAndCheapestProductResponse
    {
        public List<ProductResponse>? MostExpensiveProducts { get; set; }
        public List<ProductResponse>? CheapestProducts { get; set; }
    }
}
