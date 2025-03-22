using Serilog;
using AccountService.API.Mappers;
using AccountService.Core.Interfaces.Repositories;
using AccountService.Application.Interfaces.Services;
using AccountService.Application.Mappers;
using AccountService.Application.Services;
using AccountService.Persistence.Context;
using AccountService.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using AccountService.API.ExceptionHandling;
using AccountService.API.Validators.Profiles;
using FluentValidation;
using FluentValidation.AspNetCore;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning) // чтобы не спамили middleware'ы
    .MinimumLevel.Override("System", LogEventLevel.Warning)
    .MinimumLevel.Debug() // глобальный минимум
    .Enrich.FromLogContext()
    .WriteTo.Console(outputTemplate:
        "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {Properties:j}{NewLine}{Exception}")
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e =>
            e.Level is LogEventLevel.Information or LogEventLevel.Warning)
        .WriteTo.File(
            "logs/info-.log",
            rollingInterval: RollingInterval.Day,
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
            retainedFileCountLimit: 7))
    .WriteTo.Logger(lc => lc
        .Filter.ByIncludingOnly(e =>
            e.Level is LogEventLevel.Error or LogEventLevel.Fatal)
        .WriteTo.File(
            "logs/error-.log",
            rollingInterval: RollingInterval.Day,
            outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}",
            retainedFileCountLimit: 14))
    .WriteTo.File(
        new Serilog.Formatting.Json.JsonFormatter(),
        "logs/log-.json",
        rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();


// Register ProblemDetails for standardized error responses
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

// Add database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();

// Register services
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(UserApiProfile), typeof(UserProfile));

// Add controllers
builder.Services.AddControllers();

// FluentValidation
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateUserRequestValidator>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Development-only config
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }

    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.DocumentTitle = "Account Service API";
        options.DisplayRequestDuration();
    });
}

app.UseHttpsRedirection();
app.UseExceptionHandler();
app.UseAuthorization();
app.MapControllers();
app.Run();

// Ensure logs are flushed
Log.CloseAndFlush();