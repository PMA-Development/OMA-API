
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OMA_API.Services;
using OMA_API.Services.Interfaces;
using OMA_Data.Core.Utils;
using OMA_Data.Data;
using OMA_Data.Entities;
using OMQ_Mqtt;
using Serilog;
using System.Security.Claims;

namespace OMA_API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            Serilog.Core.Logger logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .CreateLogger();

            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
            builder.Services.AddHttpContextAccessor();

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
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = false, 
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true
                    };
                    options.RequireHttpsMetadata = true; 
                });
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("HasHotlineUser", policy => policy.RequireRole("Hotline-User"));
                options.AddPolicy("HasAdmin", policy => policy.RequireRole("Admin"));
            });



            builder.Services.AddHostedService<MqttScopedServiceHostedService>();
            builder.Services.AddScoped<IMqttScopedProcessingService, MqttScopedProcessingService>();

            builder.Services.AddScoped<IGenericRepository<Alarm>, GenericRepository<Alarm>>();
            builder.Services.AddScoped<IGenericRepository<AlarmConfig>, GenericRepository<AlarmConfig>>();
            builder.Services.AddScoped<IGenericRepository<OMA_Data.Entities.Attribute>, GenericRepository<OMA_Data.Entities.Attribute>>();
            builder.Services.AddScoped<IGenericRepository<Device>, GenericRepository<Device>>();
            builder.Services.AddScoped<IGenericRepository<DeviceAction>, GenericRepository<DeviceAction>>();
            builder.Services.AddScoped<IGenericRepository<DeviceData>, GenericRepository<DeviceData>>();
            builder.Services.AddScoped<IGenericRepository<Drone>, GenericRepository<Drone>>();
            builder.Services.AddScoped<IGenericRepository<Island>, GenericRepository<Island>>();
            builder.Services.AddScoped<IGenericRepository<OMA_Data.Entities.Log>, GenericRepository<OMA_Data.Entities.Log>>();
            builder.Services.AddScoped<IGenericRepository<OMA_Data.Entities.Task>, GenericRepository<OMA_Data.Entities.Task>>();
            builder.Services.AddScoped<IGenericRepository<Turbine>, GenericRepository<Turbine>>();
            builder.Services.AddScoped<IGenericRepository<User>, GenericRepository<User>>();
            builder.Services.AddScoped<ILoggingService, LoggingService>();

            builder.Services.AddHttpContextAccessor();

            //TODO : CHANGE CORS
            builder.Services.AddCors(options => { options.AddPolicy("AllowCors", policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod()); });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseCors("AllowCors");

            app.MapGet("identity", (ClaimsPrincipal user) => user.Claims.Select(c => new { c.Type, c.Value }))
            .RequireAuthorization("HasHotlineUser");

            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
