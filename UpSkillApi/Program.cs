using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.Repositories;

namespace UpSkillApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        // Register DbContext with connection string from appsettings
        builder.Services.AddDbContext<UpSkillDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // Register repository directly
        builder.Services.AddScoped<WorkerRepository>();
        builder.Services.AddScoped<AdvertisementRepository>();
        builder.Services.AddScoped<UserRepository>();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}