using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : IProductRepository
    {
        public Product CurrentProduct { get; private set; }

        public async Task AddProduct(Product product) => await ProductDAO.Instance.AddProduct(product);

        public async Task<bool> DeleteProduct(Product product)
        {
            bool result = false;
            await ProductDAO.Instance.RemoveProduct(product);
            result = true;
            return result;
        }

        public async Task<List<Product>> GetAll()
        {
            var result = await ProductDAO.Instance.GetProducts();
            return result;
        }

        public async Task<Product> GetProductById(int productID) {
            var result = await ProductDAO.Instance.GetProductById(productID);
            CurrentProduct = result;
            var data = CurrentProduct.ProductId.ToString();
            return result;

        } 

        public async Task<IEnumerable<Product>> GetProductByPriceRange(decimal minPrice, decimal maxPrice)
        {
            var results = await ProductDAO.Instance.GetProductByPriceRange(minPrice, maxPrice);
            return results;
        }
        public async Task<IEnumerable<Product>> GetProductByName(string productName)
        {
            var results = await ProductDAO.Instance.GetProductByName(productName);
            return results;
        }
        public async Task<IEnumerable<Product>> GetProductByUnitStock(int unitInStock)
        {
            var results = await ProductDAO.Instance.GetProductByUnitStock(unitInStock);
            return results;
        }

        public async Task<bool> UpdateProduct(Product product)
        {
            bool result = false;
            await ProductDAO.Instance.UpdateProduct(product);
            result = true;
            return result;
        }
    }
}
