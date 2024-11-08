
using Microsoft.EntityFrameworkCore;
using OMA_Data.Data;
using OMA_Data.Entities;
using System;
using System.Security.Claims;

namespace OMA_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;
           

            builder.Services.AddControllers();
            // Add services to the container.
            builder.Services.AddAuthorization();

            builder.Services.AddDbContext<OMAContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer") ?? throw new InvalidOperationException("Connection string 'WebAPI' not found.")));
            builder.Services.AddScoped<IDataContext, DataContext>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddAuthentication()
                .AddJwtBearer(options =>
                {
                    options.Authority = configuration["Authentication:Authority"];
                    options.TokenValidationParameters.ValidateAudience = false;
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("HasHotlineUser", policy => policy.RequireRole("Hotline-User"));
                options.AddPolicy("HasAdmin", policy => policy.RequireRole("Admin"));
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGet("identity", (ClaimsPrincipal user) => user.Claims.Select(c => new { c.Type, c.Value }))
            .RequireAuthorization("HasHotlineUser");

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
