using Demo_ASPNET_Developer.Controllers;
using Demo_ASPNET_Developer.Data;
using Demo_ASPNET_Developer.Data.Entities;
using Demo_ASPNET_Developer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;

namespace MyControllerTests
{
    public class ProductControllerTests
    {
        private readonly ProductController _controller;
        private readonly DemoAppContext _dbContext;
        private readonly ProductService _mockProductService;

        public ProductControllerTests()
        {
            var options = new DbContextOptionsBuilder<DemoAppContext>()
                //.UseSqlServer("Data Source=DESKTOP-LDKNLOS\\SQLEXPRESS;Initial Catalog=DemoDatabase;Integrated Security=True;Encrypt=False;Trust Server Certificate=True")
            .UseInMemoryDatabase("DemoDatabase")
            .Options;
            

            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.BuildServiceProvider();
            var memoryCache = serviceProvider.GetService<IMemoryCache>();

            _dbContext = new DemoAppContext(options);
            _mockProductService = new ProductService(memoryCache, _dbContext);
            _controller = new ProductController(_mockProductService);
        }

        [Fact]
        public async Task PostProduct_AddProduct()
        {
            var newproduct = new Product { ID = 1, Name = "test product", Price = 100, Quantity = 1, Desc = "test" };

            var result = await _controller.AddProduct(newproduct);
            var product = _dbContext.Products.Find(newproduct.ID);

            Assert.Equal("test product", product.Name);
            Assert.Equal(100, product.Price);
        }

        [Fact]
        public async Task Get_ReturnsResult_ListProducts()
        {
            var expectedListProducts = new List<Product>
            {
                new Product {ID = 1, Name = "test product", Price = 1000, Quantity = 1, Desc = "test"}
            };

            //_mockProductService.Setup(x => x.GetProductsAsync()).ReturnsAsync(expectedListProducts);
            var rs = await _mockProductService.GetProductsAsync();

            var result = await _controller.GetProducts() as OkObjectResult;

            var returnEntities = Assert.IsType<List<Product>>(result.Value);

            Assert.Equal(expectedListProducts.Count, returnEntities.Count);
        }

        [Fact]
        public async Task PutProduct_UpdateProduct()
        {
            var existingProduct = new Product {ID = 1, Name = "exist product", Price = 200, Quantity = 1, Desc = "existing product" };
            if (!_dbContext.Products.Any(x => x.ID == existingProduct.ID))
            {
                _dbContext.Products.Add(existingProduct);
                await _dbContext.SaveChangesAsync();
            }

            var updatedProduct = new Product {ID = 1, Name = "updated product", Price = 300, Quantity = 2, Desc = "updated product" };

            var result = await _controller.UpdateProduct(updatedProduct.ID, updatedProduct);

            var product = await _dbContext.Products.FindAsync(updatedProduct.ID);

            var objJsonUpdatedProduct = JsonConvert.SerializeObject(updatedProduct);
            var objJsonProduct = JsonConvert.SerializeObject(product);

            Assert.Equal(objJsonUpdatedProduct, objJsonProduct);
        }

        [Fact]
        public async Task DeleteProduct_RemoveProduct()
        {
            var existingProduct = new Product {ID = 1, Name = "exist product", Price = 200, Quantity = 1, Desc = "existing product" };
            if (!_dbContext.Products.Any(x => x.ID == existingProduct.ID))
            {
                _dbContext.Products.Add(existingProduct);
                await _dbContext.SaveChangesAsync();
            } 

            var result = await _controller.RemoveProduct(existingProduct.ID);
            var product = await _dbContext.Products.FindAsync(existingProduct.ID);
            Assert.Null(product);
        }
    }
}
