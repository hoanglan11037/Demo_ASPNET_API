using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;

namespace Demo_ASPNET_Developer.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly DemoAppContext _dbContext;
        private readonly IConfiguration _configuration;

        public AccountController(DemoAppContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody]UserModel model)
        {
            var user = _dbContext.Users.FirstOrDefault(x => x.Username == model.Username);
            if (user != null)
            {
                if (!BCrypt.Net.BCrypt.Verify(model.Password, user.Password)) return BadRequest("Wrong password");

                var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("Jwt:Key")));
                var signingCredential = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.UserType)
                };

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    expires: DateTime.Now.AddMinutes(10),
                    signingCredentials: signingCredential,
                    claims: claims
                    );

                var tokenHandle = new JwtSecurityTokenHandler().WriteToken(token);

                return new JsonResult(new { username = model.Username, token = tokenHandle });
            }
            
            return BadRequest("Error");
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]User item)
        {
            item.Password = BCrypt.Net.BCrypt.HashPassword(item.Password);

            _dbContext.Users.Add(item);
            await _dbContext.SaveChangesAsync();

            return Ok("Success");
        }

        [Authorize(Roles = "admin")]
        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers()
        {
            var products = await _dbContext.Users.ToListAsync();
            return Ok(products);
        }

        [Authorize(Roles = "admin")]
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody]UserModel item)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user != null)
            {
                _dbContext.Users.Entry(user).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }

            return Ok("Success");
        }

        [Authorize(Roles = "admin")]
        [HttpDelete("RemoveUser")]
        public async Task<IActionResult> RemoveUser(string id)
        {
            var product = await _dbContext.Users.FindAsync(id);
            if (product != null)
            {
                _dbContext.Remove(product);
                await _dbContext.SaveChangesAsync();
            }

            return Ok("Success");
        }
    }
}
