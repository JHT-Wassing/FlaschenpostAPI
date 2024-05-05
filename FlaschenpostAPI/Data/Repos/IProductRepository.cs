using FlaschenpostAPI.Data.Models;

namespace FlaschenpostAPI.Data.Repos
{
    public interface IProductRepository
    {

        public Task<List<Product>> GetProductsFromGivenUrlAsync(string url);
    }
}
