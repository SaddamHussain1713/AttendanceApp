
using AttendanceApp.Repositories;
using AttendanceApp.Services;
using AttendanceSystem.Data;
using AttendanceSystem.Repositories;
using AttendanceSystem.Validators;

namespace AttendanceApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddSingleton(new AttendanceDbContext(connectionString));
            // Register the repositories 
            builder.Services.AddScoped<IStudentRepository,StudentRepository>();
            builder.Services.AddScoped<IClassEntityRepository,ClassEntityRepository>();
            builder.Services.AddScoped<IClassSessionRepository,ClassSessionRepository>();
            // Register the services
            builder.Services.AddScoped<IClassEntityService, ClassEntityService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            // Register the validators
            builder.Services.AddScoped<StudentValidator>();
            builder.Services.AddScoped<ClassEntityValidator>();
            builder.Services.AddScoped<ClassSessionValidator>();

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

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
