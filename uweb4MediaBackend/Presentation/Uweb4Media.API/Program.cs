using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Camping;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Channel;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Company;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Sector;
using uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;
using uweb4Media.Application.Features.CQRS.Handlers.Comments; 
using uweb4Media.Application.Features.CQRS.Handlers.Like;
using uweb4Media.Application.Features.CQRS.Handlers.Media;
using uweb4Media.Application.Features.CQRS.Handlers.Plans;
using uweb4Media.Application.Features.CQRS.Handlers.Subscription;
using uweb4Media.Application.Features.CQRS.Handlers.User;
using uweb4Media.Application.Interfaces.AppRoleInterfaces;
using uweb4Media.Application.Interfaces.AppUserInterfaces;
using uweb4Media.Application.Interfaces;
using uweb4Media.Application.Services;
using uweb4Media.Application.Tools;
using Uweb4Media.Persistence.Context;
using Uweb4Media.Persistence.Repositories.AppRoleRepositories;
using Uweb4Media.Persistence.Repositories.AppUserRepositories;
using Uweb4Media.Persistence.Repositories; 
using System.Security.Claims;        
using Microsoft.AspNetCore.Authentication.Google; 
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Diagnostics;
using uweb4Media.Application;
using uweb4Media.Application.Features.CQRS.Handlers.Invoice;
using uweb4Media.Application.Features.CQRS.Handlers.Payment;
using uweb4Media.Application.Interfaces.Email; 
using uweb4Media.Application.Interfaces.Payment;
using uweb4Media.Application.Services.Email; 
using uweb4Media.Application.Services.PaymentService;
using Uweb4Media.Domain.Entities;
using Uweb4Media.Persistence.Repositories.Payment;
using uweb4Media.Persistence.Services.PaymentService;

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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost5173",
                    policy => policy
                        .WithOrigins("http://localhost:5173", "https://localhost:5173")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });

            //Repos
            builder.Services.AddScoped<Uweb4MediaContext>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); 
            builder.Services.AddScoped(typeof(IAppUserRepository), typeof(AppUserRepository));
            builder.Services.AddScoped(typeof(IAppRoleRepository), typeof(AppRoleRepository));
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

            //User
            builder.Services.AddScoped<GetUserQueryHandler>();
            builder.Services.AddScoped<GetUserByIdQueryHandler>();
            builder.Services.AddScoped<UpdateUserCommandHandler>();
            builder.Services.AddScoped<RemoveUserCommandHandler>();
            
            //MediaContent
            builder.Services.AddScoped<GetMediaContentQueryHandler>();
            builder.Services.AddScoped<GetMediaContentByIdQueryHandler>();
            builder.Services.AddScoped<CreateMediaContentCommandHandler>();
            builder.Services.AddScoped<UpdateMediaContentCommandHandler>();
            builder.Services.AddScoped<RemoveMediaContentCommandHandler>();
            
            //Like
            builder.Services.AddScoped<GetLikeQueryHandler>();
            builder.Services.AddScoped<GetLikeByIdQueryHandler>(); 
            builder.Services.AddScoped<CreateLikeCommandHandler>(); 
            builder.Services.AddScoped<RemoveLikeCommandHandler>();
            
            //Comment
            builder.Services.AddScoped<GetCommentQueryHandler>(); 
            builder.Services.AddScoped<GetCommentByIdQueryHandler>(); 
            builder.Services.AddScoped<CreateCommentCommandHandler>(); 
            builder.Services.AddScoped<RemoveCommentCommandHandler>();
            builder.Services.AddScoped<GetCommentsByVideoIdQueryHandler>();
            
            //Notification
            builder.Services.AddScoped<GetNotificationQueryHandler>(); 
            builder.Services.AddScoped<GetNotificationByIdQueryHandler>(); 
            builder.Services.AddScoped<CreateNotificationCommandHandler>(); 
            builder.Services.AddScoped<RemoveNotificationCommandHandler>();
            
            //Plans
            builder.Services.AddScoped<GetPlansQueryHandler>();
            builder.Services.AddScoped<GetPlansByIdQueryHandler>();
            builder.Services.AddScoped<CreatePlansCommandHandler>();
            builder.Services.AddScoped<UpdatePlansCommandHandler>();
            builder.Services.AddScoped<RemovePlansCommandHandler>();
            
            //Subscription 
            builder.Services.AddScoped<UpdateSubscribeUserCommandHandler>();
            builder.Services.AddScoped<GetSubscribeUserByIdQueryHandler>();
            
            //UserSubscribe
            builder.Services.AddScoped<GetSubscriptionQueryHandler>();
            builder.Services.AddScoped<GetSubscriptionByIdQueryHandler>(); 
            builder.Services.AddScoped<CreateSubscriptionCommandHandler>(); 
            builder.Services.AddScoped<RemoveSubscriptionCommandHandler>();
            
            //Admin/Campaign
            builder.Services.AddScoped<GetCampaignQueryHandler>();
            builder.Services.AddScoped<GetCampaignByIdQueryHandler>(); 
            builder.Services.AddScoped<CreateCampaignCommandHandler>(); 
            builder.Services.AddScoped<RemoveCampaignCommandHandler>();
            builder.Services.AddScoped<UpdateCampaignCommandHandler>();
            
            //Admin/Channel
            builder.Services.AddScoped<GetChannelQueryHandler>();
            builder.Services.AddScoped<GetChannelByIdQueryHandler>(); 
            builder.Services.AddScoped<CreateChannelCommandHandler>(); 
            builder.Services.AddScoped<RemoveChannelCommandHandler>();
            builder.Services.AddScoped<UpdateChannelCommandHandler>();
            
            //Admin/Company
            builder.Services.AddScoped<GetCompanyQueryHandler>();
            builder.Services.AddScoped<GetCompanyByIdQueryHandler>(); 
            builder.Services.AddScoped<CreateCompanyCommandHandler>(); 
            builder.Services.AddScoped<RemoveCompanyCommandHandler>();
            builder.Services.AddScoped<UpdateCompanyCommandHandler>();
            
            //Admin/Sector
            builder.Services.AddScoped<GetSectorQueryHandler>();
            builder.Services.AddScoped<GetSectorByIdQueryHandler>(); 
            builder.Services.AddScoped<CreateSectorCommandHandler>(); 
            builder.Services.AddScoped<RemoveSectorCommandHandler>();
            builder.Services.AddScoped<UpdateSectorCommandHandler>();
            
            //Admin/Video
            builder.Services.AddScoped<GetVideoQueryHandler>();
            builder.Services.AddScoped<GetVideoByIdQueryHandler>(); 
            builder.Services.AddScoped<CreateVideoCommandHandler>(); 
            builder.Services.AddScoped<RemoveVideoCommandHandler>();
            builder.Services.AddScoped<UpdateVideoCommandHandler>();
            
            //Invoice 
            builder.Services.AddScoped<GetInvoicesQueryHandler>();  

            
            //Payment
            builder.Services.AddScoped<IPaymentService, IyzicoPaymentService>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            builder.Services.AddScoped<GetPaymentsByUserIdQueryHandler>(); 
             
            //sritpePayment
            builder.Services.AddScoped<IStripePaymentService, StripePaymentService>();   
            builder.Services.AddScoped<IStripeConnectService, StripeConnectService>();   
            //Email
            builder.Services.AddScoped<IEmailService, SmtpEmailService>(); 
            builder.Services.AddMemoryCache();
        
            builder.Services.AddAuthentication(options =>
        {
            // Varsayılan kimlik doğrulama şemaları
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme; // Google için varsayılan zorlama şeması
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // API istekleri için varsayılan kimlik doğrulama şeması
            
        })
        .AddCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            options.Cookie.IsEssential = true;
        })
        .AddJwtBearer(opt => // JWT Bearer ayarları
        {
            opt.RequireHttpsMetadata = true; // HTTPS kullandığımız için TRUE olmalı (geliştirme ortamında false da olabilir ama iyi pratik true'dur)
            opt.TokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = builder.Configuration["Jwt:ValidAudience"], // appsettings.json'dan oku
                ValidIssuer = builder.Configuration["Jwt:ValidIssuer"],     // appsettings.json'dan oku
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])), 
                ValidateIssuerSigningKey = true,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ClockSkew = TimeSpan.Zero 
            };
        })
        .AddGoogle(googleOptions => // Google ayarları
        {
            googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
            googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
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
            
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireClaim("AppRole", "1"));
            });

            builder.Services.AddApplicationService(builder.Configuration);
            builder.Services.AddControllers(); 
            builder.Services.AddEndpointsApiExplorer(); 
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
 
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage(); 
                app.UseSwagger();
                app.UseSwaggerUI();
            } 
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
                    if (exception is UnauthorizedAccessException)
                    {
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        await context.Response.WriteAsync(exception.Message);
                    } 
                });
            });
            app.UseCors("AllowLocalhost5173");
            app.UseHttpsRedirection(); 
            app.UseCookiePolicy(); 
            
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}