using sqlapp.Models;

namespace sqlapp.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductList();
        List<Product> GetProducts();
        Task<bool> IsBeta();
    }
}