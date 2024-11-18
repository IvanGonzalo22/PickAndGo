using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PickAndGo.Server.Data;
using PickAndGo.Server.Services;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor
builder.Services.AddControllers();

// Configuración de DbContext con SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de autenticación con JWT
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]))
        };
    });

// Configuración de CORS - Permitir solicitudes desde tu frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:5173") // Replace with your frontend's origin
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configuración de Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Configurar información básica sobre la API
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PickAndGo API",
        Version = "v1",
        Description = "API para gestionar el registro de usuarios y autenticación"
    });
});

// Registrar el servicio de usuarios
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configuración del pipeline de solicitudes
if (app.Environment.IsDevelopment())
{
    // Habilitar Swagger y la interfaz de Swagger UI solo en desarrollo
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PickAndGo API v1");
        c.RoutePrefix = string.Empty;
    });
}

// Redirección HTTPS
app.UseHttpsRedirection();

// Habilitar autenticación y autorización
app.UseCors("CorsPolicy");
app.UseAuthentication(); // Habilitar autenticación
app.UseAuthorization(); // Habilitar autorización

// Mapear los controladores
app.MapControllers();

// Fallback para SPA (Single Page Application)
app.MapFallbackToFile("/index.html");

app.Run();
