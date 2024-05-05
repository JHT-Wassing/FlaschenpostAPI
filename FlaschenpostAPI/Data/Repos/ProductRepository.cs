using FlaschenpostAPI.Data.Models;
using Newtonsoft.Json;

namespace FlaschenpostAPI.Data.Repos
{
    public class ProductRepository : IProductRepository
    {

        private readonly HttpClient _httpClient = new();

        public async Task<List<Product>> GetProductsFromGivenUrlAsync(string url)
        {

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var jsonMessage = await response.Content.ReadAsStringAsync();
                var products = JsonConvert.DeserializeObject<List<Product>>(jsonMessage);

                if (products != null && products.Count > 0) return products;
            }

            return new List<Product>();
        }
    }
}
