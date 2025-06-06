//using ClientsTravels.Models;
//using ClientsTravels.repositories;
//using ClientsTravels.services;

using ClientsTravels.Models;
using ClientsTravels.repositories;
using ClientsTravels.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace ClientsTravels;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

        builder.Services.AddDbContext<MasterContext>(options =>
            options.UseSqlServer(connectionString));


        builder.Services.AddAuthorization();
        builder.Services.AddControllers();
        builder.Services.AddScoped<ITripRepository, TripRepository>();
        builder.Services.AddScoped<ITripService, TripService>();
        builder.Services.AddScoped<IClientRepository, ClientRepository>();
        builder.Services.AddScoped<IClientService, ClientService>();


        builder.Services.AddOpenApi();
        
        
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Client Trips",
                Version = "v1",
                Description = "Rest API for managing client trips",
                Contact = new OpenApiContact
                {
                    Name = "API Suppoert",
                    Email = "support@example.com",
                    Url = new Uri("https://example/support")
                },
                License = new OpenApiLicense
                {
                    Name = "MIT License",
                    Url = new Uri("https://opensource.org/licenses/MIT")
                }
            });
        });

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
        
        app.UseSwagger();
    
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Rental v1");

            //Basic UI Customization
            c.DocExpansion(DocExpansion.List);
            c.DefaultModelsExpandDepth(0); //Hide schemas section by default
            c.DisplayRequestDuration(); //Show request duration
            c.EnableFilter(); //Enable filtering operration
        });
        
        //app.UseGlobalExceptionHandling(); 
        app.UseSwagger();

        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "TripsClients v1");

            c.DocExpansion(DocExpansion.List);
            c.DefaultModelsExpandDepth(0); 
            c.DisplayRequestDuration();
            c.EnableFilter(); 
        });
        
        app.UseHttpsRedirection();

        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}