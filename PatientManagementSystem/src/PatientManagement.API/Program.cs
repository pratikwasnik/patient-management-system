#region Namespaces
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PatientManagement.API.Middleware;
using PatientManagement.API.Repositories;
using PatientManagement.API.Services;
using PatientManagement.API.Validators;
using System.Text;
#endregion

var builder = WebApplication.CreateBuilder(args);

#region Configuration

var configuration = builder.Configuration;
#endregion

#region Services

// Controllers
builder.Services.AddControllers();

// FluentValidation 
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<PatientCreateDtoValidator>();

// Swagger + JWT setup
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "PatientManagement.API", Version = "v1" });

    #region Swagger JWT Security
    var jwtScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Description = "Enter JWT Bearer token **_only_**",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
    };
    c.AddSecurityDefinition("Bearer", jwtScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtScheme, new string[] { } }
    });
    #endregion
});

// Database (Dapper)
builder.Services.AddScoped<System.Data.IDbConnection>(sp =>
{
    var cs = configuration.GetConnectionString("DefaultConnection");
    return new Microsoft.Data.SqlClient.SqlConnection(cs);
});

// Dependency Injection (Repositories + Services)
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();

#endregion

#region JWT Authentication
var jwtSection = configuration.GetSection("Jwt");
var secret = jwtSection.GetValue<string>("Key")
             ?? "uY6n!vX9wQ3eP8sT2aR7zK4bL1cJ5fN0hG8mD2qE9rT6pL4oB7yV!";
var key = Encoding.ASCII.GetBytes(secret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,
        ClockSkew = TimeSpan.Zero
    };
});
#endregion

var app = builder.Build();

#region Middleware Pipeline

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

#endregion

app.Run();
