
using JobApplication.Application.interfaces;
using JobApplication.Application.Services;
using JobApplication.Application.Services.imp;
using JobApplication.Application.Settings;
using JobApplication.DataModel.Entities;
using JobApplication.infrastructure.Persistence;
using JobApplication.infrastructure.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
using JobApplication.DataModel.Constants;

namespace JobApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection("Jwt"));
            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddScoped<IJobServices, JobServices>();
            builder.Services.AddScoped<IJobRepository, JobRepository>();
            builder.Services.AddScoped<IApplicationRepo, ApplicationRepo>();
            builder.Services.AddScoped<IApplicationService, ApplicationService>();
            builder.Services.AddScoped<IAuthRepo, AuthRepo>();
            builder.Services.AddScoped<IAuthService, AuthServices>();
            builder.Services.AddScoped<IJwtService, JwtService>();


            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();
            var jwtSettings = builder.Configuration
                            .GetSection("Jwt")
                            .Get<JwtSettings>()
                            ?? throw new InvalidOperationException("JWT settings not found.");
            builder.Services
                    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,

                            IssuerSigningKey = new SymmetricSecurityKey(
                                Encoding.UTF8.GetBytes(jwtSettings.Key)
                            ),

                            ValidateIssuer = true,
                            ValidIssuer = jwtSettings.Issuer,

                            ValidateAudience = true,
                            ValidAudience = jwtSettings.Audience,

                            ValidateLifetime = true
                        };
                    });

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi();
            var connectionString =
            builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string"
                + "'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDBContext>(options =>
                options.UseSqlServer(connectionString));

            var app = builder.Build();
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider
                    .GetRequiredService<RoleManager<IdentityRole>>();

                var roles = new[]
                {
                    Roles.Candidate,
                    Roles.Recruiter,
                    Roles.Admin
                };

                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.MapScalarApiReference();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
/*{
"fullName": "Menna",
  "email": "Menna@gmail.com",
  "password": "Menna@3062005"
}
{
  "fullName": "Menna",
  "email": "Menna@12345",
  "password": "Menna@3062005"
}
*/
/*
 * {
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNmYTE4OThkLWE3YzMtNDlkNS05NzkyLTc1ZWNiM2NlZTQzOCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Ik1lbm5hQGdtYWlsLmNvbSIsImV4cCI6MTc4OTc0MDM5MX0.SUwBwKadUMTuPKr5KCqSrtVkkvXdD1sx1TSficiLd9w",
  "refreshToken": "1eXjU6lvDSeg7SWpR6ktW/tjf2nzMq6ZRw+RyVNNPuQ3+WJZDy/SmZ+xM6udOuPKsXBiJ1HaEVUvU24+3S8Tpg=="
}*/
/*
 * {
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjNmYTE4OThkLWE3YzMtNDlkNS05NzkyLTc1ZWNiM2NlZTQzOCIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL2VtYWlsYWRkcmVzcyI6Ik1lbm5hQGdtYWlsLmNvbSIsImV4cCI6MTc4OTc0MDQzN30._PyLasZgltoIoTsLcTt6u3gYU-F1qpvw6mgxgcv7k4Y",
  "refreshToken": "Cr/srvL30gBB+vVPDh9ab2/Wrkq2LioLOOa0fxNiB7f4L/FhRGvCBsBKxTL6pbqb6l3MhGONodZSH+cZYa/I8A=="
}*/
