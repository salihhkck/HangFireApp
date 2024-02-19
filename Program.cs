
using Hangfire;
using HangFireApp.Context;
using HangFireApp.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HangFireApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connectionString = builder.Configuration.GetConnectionString("SqlServer");


            builder.Services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            // Add services to the container.

            builder.Services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(connectionString);
            });

            builder.Services.AddHangfireServer();
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

            app.UseHangfireDashboard();

            RecurringJob.AddOrUpdate("Test-Job", () => BackgroundTestService.Test(), Cron.Minutely());

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
