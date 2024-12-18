using Microsoft.OpenApi.Models;

namespace CleanApp.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerGenExt(this IServiceCollection services) 
        { 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1",new() { Title = "CleanApp.API",Version = "v1"});

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Bearer token kullanımı için. Örnek: 'Bearer YOUR_TOKEN'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerExt(this IApplicationBuilder app) 
        { 
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}
