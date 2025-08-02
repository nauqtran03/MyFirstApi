
using MyFirstApi.Data;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using MyFirstApi.Model;
namespace MyFirstApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDbContext>(static options =>
    options.UseInMemoryDatabase("ProductsDb"));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "MyFirstApi", Version = "v1" });
                var xml = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xml);
                //c.IncludeXmlComments(xmlPath);
            });
            builder.Services.AddFluentValidation(fv=>
            fv.RegisterValidatorsFromAssemblyContaining<ProductValidator>());
            var app = builder.Build();

            app.MapGet("/", () => $"Hello at {DateTime.Now}");

            app.MapGet("/ping", () => new { message = "pong", time = DateTime.UtcNow })
                .WithName("Ping");
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider
                    .GetRequiredService<ApplicationDbContext>();
                db.Products.AddRange(
                   new Model.Product {Id =1, Name = "Laptop",Price = 1200},
                   new Model.Product{ Id = 2, Name = "Smartphone", Price = 800 },
                   new Model.Product{ Id = 3, Name = "Tablet", Price = 600 }
                );
                db.SaveChanges();
            };

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
