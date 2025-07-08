using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Application.Services;
using Uweb4Media.Domain.Repositories; // Doğru namespace
using Uweb4Media.Persistence;               // DbContext için
using Uweb4Media.Persistence.Repositories;  // Repository için

namespace Uweb4Media.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ✅ Connection String (appsettings.json içindeki adı)
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
            builder.Services.AddScoped<IMediaContentRepository, MediaContentRepository>();
            builder.Services.AddScoped<MediaContentService>();

          
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                 
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // ✅ DI kayıtları
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}