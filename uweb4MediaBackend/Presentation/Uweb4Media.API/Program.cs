using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using MediatR;
using uweb4Media.Application;
using uweb4Media.Application.Services.PaymentService;
using uweb4Media.Application.Services;
using uweb4Media.Application.Tools;
using uweb4Media.Application.Interfaces;
using uweb4Media.Application.Interfaces.AppUserInterfaces;
using uweb4Media.Application.Interfaces.AppRoleInterfaces;
using Uweb4Media.Persistence.Context;
using Uweb4Media.Persistence.Repositories;
using Uweb4Media.Persistence.Repositories.AppUserRepositories;
using Uweb4Media.Persistence.Repositories.AppRoleRepositories;
// CQRS
using uweb4Media.Application.Features.CQRS.Handlers.User;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Company; 
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Camping; 
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Video; 
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Sector; 
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Channel; 
using uweb4Media.Application.Features.CQRS.Handlers.Comments;
using uweb4Media.Application.Features.CQRS.Handlers.Like; 
using uweb4Media.Application.Features.CQRS.Handlers.Media; 
using uweb4Media.Application.Features.CQRS.Handlers.Subscription; 
using uweb4Media.Application.Features.CQRS.Handlers.Plans; 
//using uweb4Media.Application.Features.CQRS.Handlers.Notification;

namespace Uweb4Media.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddHttpClient();

            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            builder.Services.Configure<CookiePolicyOptions>(options =>
            {
                options.MinimumSameSitePolicy = SameSiteMode.None;
                options.OnAppendCookie = cookieContext =>
                {
                    if (cookieContext.CookieName.Contains(".AspNetCore.Correlation.") || cookieContext.CookieName.Contains(".AspNetCore.Cookies"))
                    {
                        cookieContext.CookieOptions.SameSite = SameSiteMode.None;
                        cookieContext.CookieOptions.Secure = true;
                    }
                };
                options.OnDeleteCookie = cookieContext =>
                {
                    if (cookieContext.CookieName.Contains(".AspNetCore.Correlation.") || cookieContext.CookieName.Contains(".AspNetCore.Cookies"))
                    {
                        cookieContext.CookieOptions.SameSite = SameSiteMode.None;
                        cookieContext.CookieOptions.Secure = true;
                    }
                };
            });

            // ✅ CORS Policy - Frontend portları eklendi
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFrontend", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:5173",   // Vite default port
                        "https://localhost:5173",
                        "http://localhost:5174",   // ✅ Mevcut frontend port
                        "https://localhost:5174",
                        "http://localhost:3000",   // React default port
                        "https://localhost:3000"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
                });

                // ✅ Development için daha geniş CORS policy
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            // ✅ Database Context ve Repository'ler
            builder.Services.AddScoped<Uweb4MediaContext>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<IAppUserRepository, AppUserRepository>();
            builder.Services.AddScoped<IAppRoleRepository, AppRoleRepository>();

            // ✅ CQRS Handlers - User
            builder.Services.AddScoped<GetUserQueryHandler>();
            builder.Services.AddScoped<UpdateUserCommandHandler>();
            builder.Services.AddScoped<RemoveUserCommandHandler>();
            builder.Services.AddScoped<GetUserByIdQueryHandler>();
            
            // Company
            builder.Services.AddScoped<GetCompanyQueryHandler>();
            builder.Services.AddScoped<GetCompanyByIdQueryHandler>();
            builder.Services.AddScoped<CreateCompanyCommandHandler>();
            builder.Services.AddScoped<UpdateCompanyCommandHandler>();
            builder.Services.AddScoped<RemoveCompanyCommandHandler>();
            
            // Campaign 
            builder.Services.AddScoped<GetCampaignQueryHandler>();
            builder.Services.AddScoped<GetCampaignByIdQueryHandler>();
            builder.Services.AddScoped<CreateCampaignCommandHandler>();
            builder.Services.AddScoped<UpdateCampaignCommandHandler>();
            builder.Services.AddScoped<RemoveCampaignCommandHandler>();

            // Video  
            builder.Services.AddScoped<GetVideoQueryHandler>();
            builder.Services.AddScoped<GetVideoByIdQueryHandler>();
            builder.Services.AddScoped<CreateVideoCommandHandler>();
            builder.Services.AddScoped<UpdateVideoCommandHandler>();
            builder.Services.AddScoped<RemoveVideoCommandHandler>();

            // Sector
            builder.Services.AddScoped<GetSectorQueryHandler>();
            builder.Services.AddScoped<GetSectorByIdQueryHandler>();
            builder.Services.AddScoped<CreateSectorCommandHandler>();
            builder.Services.AddScoped<UpdateSectorCommandHandler>();
            builder.Services.AddScoped<RemoveSectorCommandHandler>();

            // Channel
            builder.Services.AddScoped<GetChannelQueryHandler>();
            builder.Services.AddScoped<GetChannelByIdQueryHandler>();
            builder.Services.AddScoped<CreateChannelCommandHandler>();
            builder.Services.AddScoped<UpdateChannelCommandHandler>();
            builder.Services.AddScoped<RemoveChannelCommandHandler>();

            // Comment
            builder.Services.AddScoped<GetCommentQueryHandler>();
            builder.Services.AddScoped<GetCommentByIdQueryHandler>();
            builder.Services.AddScoped<CreateCommentCommandHandler>();
            builder.Services.AddScoped<RemoveCommentCommandHandler>();
            builder.Services.AddScoped<GetCommentsByMediaContentIdQueryHandler>();

            // Like
            builder.Services.AddScoped<GetLikeQueryHandler>();
            builder.Services.AddScoped<GetLikeByIdQueryHandler>();
            builder.Services.AddScoped<CreateLikeCommandHandler>();
            builder.Services.AddScoped<RemoveLikeCommandHandler>();

            // ✅ Media - 500 HATASINI ÇÖZEN BÖLÜM!
            builder.Services.AddScoped<GetMediaContentQueryHandler>();
            builder.Services.AddScoped<GetMediaContentByIdQueryHandler>();
            builder.Services.AddScoped<CreateMediaContentCommandHandler>();
            builder.Services.AddScoped<UpdateMediaContentCommandHandler>();
            builder.Services.AddScoped<RemoveMediaContentCommandHandler>();

            // Subscription
            builder.Services.AddScoped<GetSubscriptionQueryHandler>();
            builder.Services.AddScoped<GetSubscriptionByIdQueryHandler>();
            builder.Services.AddScoped<CreateSubscriptionCommandHandler>();
            builder.Services.AddScoped<RemoveSubscriptionCommandHandler>();

            // Plans
            builder.Services.AddScoped<GetPlansQueryHandler>();
            builder.Services.AddScoped<GetPlansByIdQueryHandler>();
            builder.Services.AddScoped<CreatePlansCommandHandler>();
            builder.Services.AddScoped<UpdatePlansCommandHandler>();
            builder.Services.AddScoped<RemovePlansCommandHandler>();

            // Notification
            builder.Services.AddScoped<GetNotificationQueryHandler>();
            builder.Services.AddScoped<GetNotificationByIdQueryHandler>();
            builder.Services.AddScoped<CreateNotificationCommandHandler>();
            builder.Services.AddScoped<RemoveNotificationCommandHandler>();
            
            // ✅ Payment Services
            builder.Services.AddScoped<IPaymentService, IyzicoPaymentService>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

            // ✅ AUTHENTICATION & AUTHORIZATION
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest; // ✅ HTTP için güncellendi
                options.Cookie.IsEssential = true;
                options.LoginPath = "/api/auth/login";
                options.LogoutPath = "/api/auth/logout";
                options.AccessDeniedPath = "/api/auth/access-denied";
            })
            .AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false; // ✅ Development için false
                opt.SaveToken = true;
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = builder.Configuration["Jwt:ValidAudience"] ?? "uweb4-audience",
                    ValidIssuer = builder.Configuration["Jwt:ValidIssuer"] ?? "uweb4-issuer",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                        builder.Configuration["Jwt:Key"] ?? "your-super-secret-key-for-jwt-token-generation-minimum-32-characters"
                    )),
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ClockSkew = TimeSpan.Zero
                };

                // ✅ JWT Token'ı request header'dan al
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                        if (!string.IsNullOrEmpty(token))
                        {
                            context.Token = token;
                        }
                        return Task.CompletedTask;
                    },
                    OnAuthenticationFailed = context =>
                    {
                        Console.WriteLine($"🔴 JWT Authentication failed: {context.Exception.Message}");
                        return Task.CompletedTask;
                    },
                    OnTokenValidated = context =>
                    {
                        Console.WriteLine($"🟢 JWT Token validated for user: {context.Principal?.Identity?.Name}");
                        return Task.CompletedTask;
                    }
                };
            })
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
                googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
                googleOptions.CallbackPath = "/api/auth/google-callback";
                googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.profile");
                googleOptions.Scope.Add("https://www.googleapis.com/auth/userinfo.email");
                googleOptions.Events.OnCreatingTicket = ctx =>
                {
                    var nameClaim = ctx.Identity.FindFirst(ClaimTypes.Name);
                    if (nameClaim != null)
                    {
                        var parts = nameClaim.Value.Split(' ', 2);
                        if (parts.Length > 0)
                            ctx.Identity.AddClaim(new Claim(ClaimTypes.GivenName, parts[0]));
                        if (parts.Length > 1)
                            ctx.Identity.AddClaim(new Claim(ClaimTypes.Surname, parts[1]));
                    }
                    var pictureClaim = ctx.Identity.FindFirst("picture");
                    if (pictureClaim != null)
                    {
                        ctx.Identity.AddClaim(new Claim("profile_picture", pictureClaim.Value));
                    }
                    return Task.CompletedTask;
                };
            });

            builder.Services.AddAuthorization();
            builder.Services.AddApplicationService(builder.Configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // ✅ Development ortamı ayarları
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
                
                // Development'da daha geniş CORS
                app.UseCors("AllowAll");
                Console.WriteLine("🔧 Development mode: Using AllowAll CORS policy");
            }
            else
            {
                // Production'da sıkı CORS
                app.UseCors("AllowFrontend");
                Console.WriteLine("🔒 Production mode: Using AllowFrontend CORS policy");
            }

            // ✅ HTTPS Redirect'i development'da kapatıldı
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            // ✅ Middleware sırası çok önemli!
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();
            
            // ✅ Controller mapping
            app.MapControllers();

            // ✅ Health check endpoint
            app.MapGet("/api/health", () => new { 
                status = "healthy", 
                timestamp = DateTime.UtcNow,
                environment = app.Environment.EnvironmentName 
            });

            // ✅ CORS test endpoint
            app.MapGet("/api/cors-test", () => new { 
                message = "CORS is working!", 
                timestamp = DateTime.UtcNow 
            });
            
            Console.WriteLine("🚀 uWeb4 Media API Server starting...");
            Console.WriteLine($"🌐 Environment: {app.Environment.EnvironmentName}");
            Console.WriteLine("📡 Server URL: http://localhost:5285");
            Console.WriteLine("🔗 Health Check: http://localhost:5285/api/health");
            Console.WriteLine("🧪 CORS Test: http://localhost:5285/api/cors-test");
            Console.WriteLine("📚 Swagger UI: http://localhost:5285/swagger");
            Console.WriteLine("✅ Ready to accept requests!");
            
            app.Run();
        }
    }
}