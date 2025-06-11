using Microsoft.EntityFrameworkCore;
using UpSkillApi.Data;
using UpSkillApi.Repositories;
using UpSkillApi.Hubs; // لازم تضيف ال namespace اللي فيه ChatHub

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

        // ✅ Register DbContext
        builder.Services.AddDbContext<UpSkillDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

        // ✅ Register Repositories
        builder.Services.AddScoped<WorkerRepository>();
        builder.Services.AddScoped<AdvertisementRepository>();
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<AuthRepository>();
        builder.Services.AddScoped<VolunteeringRepository>();
        builder.Services.AddScoped<ClientPostRepository>();
        builder.Services.AddScoped<WorkerApplicationRepository>();
        builder.Services.AddScoped<VolunteeringApplicationRepository>();
        builder.Services.AddScoped<ChatRepository>();
        builder.Services.AddScoped<WorkerImageRepository>();
        builder.Services.AddScoped<RatingRepository>();




        // ✅ Add SignalR
        builder.Services.AddSignalR();

        // ✅ Enable CORS
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy
                    .WithOrigins(
                        "http://localhost",
                        "http://127.0.0.1",
                        "null", // لو الملف HTML محلي
                        "file://", // نفس السبب
                        "https://upskill.eu.ngrok.io" // رابط ngrok بتاعك
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials(); // 👈 مهم جدًا لـ SignalR
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        // ✅ Use CORS before authorization
        app.UseCors("AllowAll");

        app.UseAuthorization();
        
        app.UseStaticFiles(); 


        app.MapControllers();

        // ✅ Map SignalR endpoint
        app.MapHub<ChatHub>("/chatHub");

        app.Run();
    }
}