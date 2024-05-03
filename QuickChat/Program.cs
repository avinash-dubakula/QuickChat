using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using QuickChat.BusinessLogicLayer;
using QuickChat.BusinessLogicLayer.Hubs;
using QuickChat.BusinessLogicLayer.Models.Entities.Identity;
using QuickChat.DataAccessLayer;
using QuickChat.DataAccessLayer.Context;
using QuickChat.IServices;
using QuickChat.Services;
using System.Text;

namespace QuickChat
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<MyIdentityDbContext>(Options => Options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<MyIdentityDbContext>()
                .AddDefaultTokenProviders();
            // Add services to the container.
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = builder.Configuration["JWT:ValidAudience"],
                    ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))

                };

            });
            builder.Services.AddSignalR();
            builder.Services.AddControllers();
            builder.Services.AddBusinessLogicService();
            builder.Services.AddDataAccessService();
            builder.Services.AddScoped<IUserHelp, UserHelp>();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo { Title = "Auth API", Version = "v1" });
                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                option.AddSecurityRequirement(new OpenApiSecurityRequirement
             {
                 {
                     new OpenApiSecurityScheme
                     {
                         Reference=new OpenApiReference
                         {
                             Type=ReferenceType.SecurityScheme,
                             Id="Bearer"
                         },
                         Scheme = "Bearer",
                         Name = "Bearer",
                         In = ParameterLocation.Header,
                     }, new List<string>()
                 }
             });
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin", policy =>
                    policy.WithOrigins("http://localhost:3000")
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials());
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowSpecificOrigin");
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ChatHub>("/ChatHub");
                endpoints.MapControllers();
            });

            app.MapControllers();

            app.Run();
        }
    }
}