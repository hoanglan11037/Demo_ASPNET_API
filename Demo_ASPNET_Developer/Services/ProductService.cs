namespace Demo_ASPNET_Developer.Services
{
    public class ProductService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly DemoAppContext _dbContext;

        public ProductService(IMemoryCache memoryCache, DemoAppContext dbContext)
        {
            _memoryCache = memoryCache;
            _dbContext = dbContext;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            string key = "productsKey";

            if (!_memoryCache.TryGetValue(key, out List<Product> product))
            {
                product = await _dbContext.Products.ToListAsync();

                _memoryCache.Set(key, product);
            }

            return product;
        }

        public async Task<bool> AddProductsAsync(Product item)
        {
            _dbContext.Products.Add(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UpdateProductsAsync(int id, Product item)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if(product != null)
            {
                //_dbContext.Products.Entry(item).State = EntityState.Modified;
                _dbContext.Entry(product).CurrentValues.SetValues(item);
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }

        public async Task<bool> RemoveProductsAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);
            if (product != null)
            {
                _dbContext.Remove(product);
                await _dbContext.SaveChangesAsync();
            }

            return true;
        }
    }
}
