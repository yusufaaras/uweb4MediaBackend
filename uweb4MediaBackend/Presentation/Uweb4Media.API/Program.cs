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
using Microsoft.AspNetCore.HttpOverrides;
using uweb4Media.Application;
using uweb4Media.Application.Features.CQRS.Handlers.Invoice;
using uweb4Media.Application.Features.CQRS.Handlers.Payment;
using uweb4Media.Application.Features.Mediator.Handlers.GitHub;
using uweb4Media.Application.Interfaces.Email;
using uweb4Media.Application.Interfaces.Payment;
using uweb4Media.Application.Services.Email;
using uweb4Media.Application.Services.PaymentService;
using uweb4Media.Application.Services.User;
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
    
            // CORS Policy tanımları
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowFirebase", policy =>
                    policy.WithOrigins(
                            "https://primeweb4-9c444.firebaseapp.com",
                            "https://adminprimeweb4.web.app",
                            "https://primeui2.web.app",
                            "https://prime.uweb4.com",
                            "http://prime.uweb4.com",
                            "http://admin.uweb4.com",
                            "https://admin.uweb4.com"
                        )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials()
                );
                options.AddPolicy("AllowLocalhost",
                    policy => policy
                        .WithOrigins(
                            "http://localhost:5173",
                            "https://localhost:5173",
                            "http://localhost:5174",
                            "https://localhost:5174"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials());
            });

            // Tüm servis ve handler ekleme işlemlerin burada kalacak
            builder.Services.AddScoped<Uweb4MediaContext>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped(typeof(IAppUserRepository), typeof(AppUserRepository));
            builder.Services.AddScoped(typeof(IAppRoleRepository), typeof(AppRoleRepository));
            builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
            builder.Services.AddScoped<GetUserQueryHandler>();
            builder.Services.AddScoped<GetUserByIdQueryHandler>();
            builder.Services.AddScoped<UpdateUserCommandHandler>();
            builder.Services.AddScoped<RemoveUserCommandHandler>();
            builder.Services.AddScoped<GetMediaContentQueryHandler>();
            builder.Services.AddScoped<GetMediaContentByIdQueryHandler>();
            builder.Services.AddScoped<CreateMediaContentCommandHandler>();
            builder.Services.AddScoped<UpdateMediaContentCommandHandler>();
            builder.Services.AddScoped<RemoveMediaContentCommandHandler>();
            builder.Services.AddScoped<GetLikeQueryHandler>();
            builder.Services.AddScoped<GetLikeByIdQueryHandler>();
            builder.Services.AddScoped<CreateLikeCommandHandler>();
            builder.Services.AddScoped<RemoveLikeCommandHandler>();
            builder.Services.AddScoped<GetCommentQueryHandler>();
            builder.Services.AddScoped<GetCommentByIdQueryHandler>();
            builder.Services.AddScoped<CreateCommentCommandHandler>();
            builder.Services.AddScoped<RemoveCommentCommandHandler>();
            builder.Services.AddScoped<GetCommentsByVideoIdQueryHandler>();
            builder.Services.AddScoped<GetNotificationQueryHandler>();
            builder.Services.AddScoped<GetNotificationByIdQueryHandler>();
            builder.Services.AddScoped<CreateNotificationCommandHandler>();
            builder.Services.AddScoped<RemoveNotificationCommandHandler>();
            builder.Services.AddScoped<GetPlansQueryHandler>();
            builder.Services.AddScoped<GetPlansByIdQueryHandler>();
            builder.Services.AddScoped<CreatePlansCommandHandler>();
            builder.Services.AddScoped<UpdatePlansCommandHandler>();
            builder.Services.AddScoped<RemovePlansCommandHandler>();
            builder.Services.AddScoped<UpdateSubscribeUserCommandHandler>();
            builder.Services.AddScoped<GetSubscribeUserByIdQueryHandler>();
            builder.Services.AddScoped<GetSubscriptionQueryHandler>();
            builder.Services.AddScoped<GetSubscriptionByIdQueryHandler>();
            builder.Services.AddScoped<CreateSubscriptionCommandHandler>();
            builder.Services.AddScoped<RemoveSubscriptionCommandHandler>();
            builder.Services.AddScoped<GetCampaignQueryHandler>();
            builder.Services.AddScoped<GetCampaignByIdQueryHandler>();
            builder.Services.AddScoped<CreateCampaignCommandHandler>();
            builder.Services.AddScoped<RemoveCampaignCommandHandler>();
            builder.Services.AddScoped<UpdateCampaignCommandHandler>();
            builder.Services.AddScoped<GetChannelQueryHandler>();
            builder.Services.AddScoped<GetChannelByIdQueryHandler>();
            builder.Services.AddScoped<CreateChannelCommandHandler>();
            builder.Services.AddScoped<RemoveChannelCommandHandler>();
            builder.Services.AddScoped<UpdateChannelCommandHandler>();
            builder.Services.AddScoped<GetCompanyQueryHandler>();
            builder.Services.AddScoped<GetCompanyByIdQueryHandler>();
            builder.Services.AddScoped<CreateCompanyCommandHandler>();
            builder.Services.AddScoped<RemoveCompanyCommandHandler>();
            builder.Services.AddScoped<UpdateCompanyCommandHandler>();
            builder.Services.AddScoped<GetSectorQueryHandler>();
            builder.Services.AddScoped<GetSectorByIdQueryHandler>();
            builder.Services.AddScoped<CreateSectorCommandHandler>();
            builder.Services.AddScoped<RemoveSectorCommandHandler>();
            builder.Services.AddScoped<UpdateSectorCommandHandler>();
            builder.Services.AddScoped<GetVideoQueryHandler>();
            builder.Services.AddScoped<GetVideoByIdQueryHandler>();
            builder.Services.AddScoped<CreateVideoCommandHandler>();
            builder.Services.AddScoped<RemoveVideoCommandHandler>();
            builder.Services.AddScoped<UpdateVideoCommandHandler>();
            builder.Services.AddScoped<GetInvoicesQueryHandler>();
            builder.Services.AddScoped<CreateGithubAppUserCommandHandler>();
            builder.Services.AddScoped<IPaymentService, IyzicoPaymentService>();
            builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            builder.Services.AddScoped<GetPaymentsByUserIdQueryHandler>();
            builder.Services.AddScoped<IStripePaymentService, StripePaymentService>();
            builder.Services.AddScoped<IStripeConnectService, StripeConnectService>();
            builder.Services.AddScoped<IEmailService, SmtpEmailService>();
            builder.Services.AddScoped<UserService>();  
            builder.Services.AddMemoryCache();
    
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddCookie()
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, opt =>
                {
                    var audiences = builder.Configuration.GetSection("Jwt:ValidAudiences").Get<string[]>();
                    var issuers = builder.Configuration.GetSection("Jwt:ValidIssuer").Get<string[]>();
                    
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudiences = audiences, // dizi olmalı!
                        ValidIssuers = issuers,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuer = true, 
                        ClockSkew = TimeSpan.Zero,
                        RoleClaimType = ClaimTypes.Role 
                    };
                })
            .AddGoogle(googleOptions =>
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
            })
            .AddGitHub(options =>
            {
                options.ClientId = builder.Configuration["Authentication:GitHub:ClientId"];
                options.ClientSecret = builder.Configuration["Authentication:GitHub:ClientSecret"];
                options.CallbackPath = "/api/auth/github-callback";
                options.Scope.Add("user:email");
                options.SaveTokens = true;
                options.Events.OnCreatingTicket = ctx =>
                {
                    var id = ctx.User.GetProperty("id").GetInt32().ToString();
                    ctx.Identity.AddClaim(new Claim("GithubId", id));
                    var login = ctx.User.GetProperty("login").GetString();
                    ctx.Identity.AddClaim(new Claim("GithubLogin", login));
                    var avatar = ctx.User.GetProperty("avatar_url").GetString();
                    ctx.Identity.AddClaim(new Claim("AvatarUrl", avatar));
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
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseCors("AllowLocalhost");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            else
            {
                app.UseCors("AllowFirebase");
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
            } 
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();
            app.Run();
        }
    }
}