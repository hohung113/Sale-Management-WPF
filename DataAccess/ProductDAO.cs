using DataAccess.Entity;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DataAccess
{
    public class ProductDAO
    {
        private static ProductDAO instance = null;
        private static readonly object instanceLock = new object();
        private ProductDAO() { }
        public static ProductDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductDAO();
                    }
                    return instance;
                }
            }
        }
        FstoreContext _context = new FstoreContext();
        // Implement search product by ID, Name , Key word name, UnitPrice, UnitStock, CRUD Product

        public async Task<List<Product>> GetProducts()
        {
            try
            {
                var result = await _context.Products.AsNoTracking().ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Occur An Error While Fetch Data", ex);
            }
        }

        public async Task RemoveProduct(Product product)
        {
            try
            {
                var existingsProduct = await GetProductById(product.ProductId);
                if (existingsProduct != null)
                {
                    _context.Products.Remove(existingsProduct);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        
        public async Task UpdateProduct(Product product)
        {
            ArgumentNullException.ThrowIfNull(product, nameof(product));
            try
            {
                var existingsProduct = await GetProductById(product.ProductId);
                if (existingsProduct != null)
                {
                    // _context.Entry<Product>(product).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.Products.Update(product);
                    await _context.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task AddProduct(Product product)
        {
            try
            {
                await _context.Products.AddAsync(product);
                 _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<Product> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products.AsNoTracking().SingleOrDefaultAsync(p => p.ProductId == id);
                return product;
            }
            catch (Exception ex)
            {
                throw new Exception("Occur An Error While Fecth Data");
            }
        }

        public async Task<IEnumerable<Product>> GetProductByName(string name)
        {
            try
            {
                var result = _context.Products
                    .Where(p => p.ProductName.Contains(name))
                    .OrderBy(p => p.ProductId);

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Occur An Error While Fetch Data", ex);
            }
        }

        public async Task<IEnumerable<Product>> GetProductByUnitStock(int unitInStock)
        {
            try
            {
                var result = await _context.Products
                    .Where(p => p.UnitInStock.Equals(unitInStock))
                    .OrderBy(p => p.ProductId)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Occur An Error While Fetch Data", ex);
            }
        }

        // Search products by price range
        public async Task<IEnumerable<Product>> GetProductByPriceRange(decimal minPrice, decimal maxPrice)
        {
            try
            {
                var result = await _context.Products
                    .Where(p => p.UnitPrice >= minPrice && p.UnitPrice <= maxPrice)
                    .OrderBy(p => p.ProductId)
                    .ToListAsync();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Occur An Error While Fetch Data", ex);
            }
        }

    }
}
