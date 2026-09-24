
using Hangfire;
using JobApplication.Application.Features.job.Command.CreateJob;
using JobApplication.Application.interfaces;
using JobApplication.Application.Services;
using JobApplication.Application.Services.imp;
using JobApplication.Application.Settings;
using JobApplication.DataModel.Constants;
using JobApplication.DataModel.Entities;
using JobApplication.infrastructure.Persistence;
using JobApplication.infrastructure.Repository;
using JobApplication.infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace JobApplication.API
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
            builder.Services.AddScoped<INotificationService, EmailNotificationServices>();
            builder.Services.AddScoped<IBackgroundJobScheduler, HangfireBackgroundJobScheduler>();
            builder.Services.AddScoped<IAuthRepo, AuthRepo>();
            builder.Services.AddScoped<IAuthService, AuthServices>();
            builder.Services.AddScoped<IJwtService, JwtService>();
            builder.Services.AddMediatR(cfg =>
                    cfg.RegisterServicesFromAssembly(typeof(CreateJobCommand).Assembly));
            builder.Services.AddHangfire(config => config
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(
                builder.Configuration.GetConnectionString("HangfireConnection")));

            builder.Services.AddHangfireServer();
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();
            var jwtSettings = builder.Configuration
                            .GetSection("Jwt")
                            .Get<JwtSettings>()
                            ?? throw new InvalidOperationException("JWT settings not found.");
            builder.Services
                    .AddAuthentication(options=>
                    {
                        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                    })
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

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Enter your JWT token."
                });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    options.IncludeXmlComments(xmlPath);
                }

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Job Application API v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseHangfireDashboard("/hangfire");

            try
            {
                app.MapControllers();
            }
            catch (ReflectionTypeLoadException ex)
            {
                foreach (var loaderException in ex.LoaderExceptions)
                {
                    Console.WriteLine("========== LOADER EXCEPTION ==========");
                    Console.WriteLine(loaderException?.ToString());
                }

                throw;
            }

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

Recuter 
{
    "email": "Menna2@gmail.com",
  "password": "Menna@12345"
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
