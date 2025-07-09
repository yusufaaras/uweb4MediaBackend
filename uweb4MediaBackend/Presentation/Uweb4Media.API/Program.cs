using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Uweb4Media.Application.Features.MediaContents.Commands;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Application.Services;
using Uweb4Media.Domain.Repositories;
using Uweb4Media.Persistence;
using Uweb4Media.Persistence.Repositories;

namespace Uweb4Media.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // ✅ Add services to the container.
            builder.Services.AddControllers();

            // ✅ Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ✅ Connection String (appsettings.json içindeki adı)
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // ✅ DI kayıtları
            builder.Services.AddScoped<IMediaContentRepository, MediaContentRepository>();
            builder.Services.AddScoped<MediaContentService>();

            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            // ✅ JSON Options
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            // ✅ MediatR
            builder.Services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(CreateMediaContentCommand).Assembly));

            // ✅ CORS policy ekle
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") // senin Vite dev server adresin
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            var app = builder.Build();

            // ✅ Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // ✅ CORS'u Authorization'dan önce çağır!
            app.UseCors("AllowFrontend");

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
