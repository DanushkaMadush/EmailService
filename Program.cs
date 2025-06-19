
using EmailService.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using EmailService.Infrastructure.Settings;
using EmailService.Application.Interfaces;
using EmailService.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using EmailService.Infrastructure.Database.StoredProcedures;


namespace EmailService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Bind SmtpSettings from appsettings.json
            builder.Services.Configure<SmtpSettings>(
                builder.Configuration.GetSection("SmtpSettings"));

            // Email sending service (infrastructure)
            builder.Services.AddScoped<IEmailService, SmtpMailService>();

            // Repository for DB access via stored procedures
            builder.Services.AddScoped<IEmailRepository, EmailRepository>();


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

            app.UseHttpsRedirection();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
