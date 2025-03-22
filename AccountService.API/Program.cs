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

var builder = WebApplication.CreateBuilder(args);

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