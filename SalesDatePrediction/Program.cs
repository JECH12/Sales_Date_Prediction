
using Entities.Context;
using Microsoft.EntityFrameworkCore;
using Services.Interfaces;
using Services.Services;

namespace SalesDatePrediction
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngular",
                    policy =>
                    {
                        policy.WithOrigins("http://localhost:4200") 
                              .AllowAnyHeader()
                              .AllowAnyMethod();
                    });
            });

            builder.Services.AddDbContext<StoreSampleContext>(options =>
                             options.UseSqlServer(builder.Configuration.GetConnectionString("StoreSample")));

            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<IStoredProcedureExecutor, StoredProcedureExecutor>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IShipperService, ShipperService>();
            builder.Services.AddScoped<IProductService, ProductService>();


            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseCors("AllowAngular");
            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
