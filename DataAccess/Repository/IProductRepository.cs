using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public interface IProductRepository
    {
        // CRUD and Search
        Product CurrentProduct { get; }
        public Task<List<Product>> GetAll();
        public Task AddProduct(Product product);
        public Task<bool> DeleteProduct(Product product);
        public Task<bool> UpdateProduct(Product product);
        public Task<Product> GetProductById(int productID);
        public Task<IEnumerable<Product>> GetProductByUnitStock(int unitInStock);
        public Task<IEnumerable<Product>> GetProductByPriceRange(decimal minPrice, decimal maxPrice);
        public Task<IEnumerable<Product>> GetProductByName(string productName);

    }
}
