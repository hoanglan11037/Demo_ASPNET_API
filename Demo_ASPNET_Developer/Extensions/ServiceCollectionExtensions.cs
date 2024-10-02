namespace Demo_ASPNET_Developer.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection ServiceCollection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJWTAuthen(configuration)
                    .AddMemoryCache()
                    .AddScoped()
                    .AddDbContext<DemoAppContext>(options =>
                    {
                        options.UseSqlServer(configuration.GetConnectionString("DemoConnection"));
                    });

            return services;
        }

        private static IServiceCollection AddJWTAuthen(this IServiceCollection services, IConfiguration configuration)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                        ValidateAudience = true,
                        ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
                        RequireExpirationTime = true,
                        ValidateLifetime = true,
                        IssuerSigningKey = signingKey
                    };
                });



            return services;
        }

        private static IServiceCollection AddScoped(this IServiceCollection services)
        {
            services.AddScoped<ProductService>();

            return services;
        }
    }
}
