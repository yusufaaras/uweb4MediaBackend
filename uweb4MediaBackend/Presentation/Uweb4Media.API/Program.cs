
using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using uweb4Media.Application.Features.CQRS.Handlers.Comments;
using uweb4Media.Application.Features.CQRS.Handlers.Firm;
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


            builder.Services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder =>
                {
                    builder.AllowAnyHeader()
                    .AllowAnyMethod()
                    .SetIsOriginAllowed((host) => true)
                    .AllowCredentials();
                });
            });

            //Repos
            builder.Services.AddScoped<Uweb4MediaContext>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>)); 
            builder.Services.AddScoped(typeof(IAppUserRepository), typeof(AppUserRepository));
            builder.Services.AddScoped(typeof(IAppRoleRepository), typeof(AppRoleRepository));

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
            builder.Services.AddScoped<GetCommentsByMediaContentIdQueryHandler>();
            
            //Notification
            builder.Services.AddScoped<GetNotificationQueryHandler>(); 
            builder.Services.AddScoped<GetNotificationByIdQueryHandler>(); 
            builder.Services.AddScoped<CreateNotificationCommandHandler>(); 
            builder.Services.AddScoped<RemoveNotificationCommandHandler>();
            
            //Firm
            builder.Services.AddScoped<GetFirmQueryHandler>();
            builder.Services.AddScoped<GetFirmByIdQueryHandler>();
            builder.Services.AddScoped<CreateFirmCommandHandler>();
            builder.Services.AddScoped<UpdateFirmCommandHandler>();
            builder.Services.AddScoped<RemoveFirmCommandHandler>();
            
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
            
            // JWT Kimlik Doğrulama Servislerini Ekleme
            
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.RequireHttpsMetadata = false; 
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidAudience = JwtTokenDefaults.ValidAudience, 
                        ValidIssuer = JwtTokenDefaults.ValidIssuer,     
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key)), 
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ClockSkew = TimeSpan.Zero 
                    };
                });

            // Yetkilendirme Servislerini Ekleme
            builder.Services.AddAuthorization();

            builder.Services.AddApplicationService(builder.Configuration);
            builder.Services.AddControllers(); 
            builder.Services.AddEndpointsApiExplorer(); 
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("CorsPolicy");
            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
