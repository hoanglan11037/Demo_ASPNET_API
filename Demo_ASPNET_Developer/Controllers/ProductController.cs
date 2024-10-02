namespace Demo_ASPNET_Developer.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/v1/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _productService.GetProductsAsync();
            return Ok(products);
        }

        [HttpPost("AddProduct")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AddProduct([FromBody]Product item)
        {
            await _productService.AddProductsAsync(item);
            return Ok("Success");
        }

        [HttpPut("UpdateProduct")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product item)
        {
            await _productService.UpdateProductsAsync(id, item);
            return Ok("Success");
        }

        [HttpDelete("RemoveProduct")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            await _productService.RemoveProductsAsync(id);
            return Ok("Success");
        }
    }
}
