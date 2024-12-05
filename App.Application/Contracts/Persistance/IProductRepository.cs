using App.Repositories.Products;

namespace App.Application.Contracts.Persistance
{
    public interface IProductRepository : IGenericRepository<Product, int>
    {
        public Task<List<Product>> GetTopPriceProductAsync(int count);

    }
}
