using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Backend.Models;

var builder = WebApplication.CreateBuilder(args);

// Cargar variables de entorno
Env.Load();

// Configurar base de datos Pomelo MySQL
var connectionString = $"Server={Environment.GetEnvironmentVariable("DB_SERVER")};" +
                       $"Port={Environment.GetEnvironmentVariable("DB_PORT")};" +
                       $"Database={Environment.GetEnvironmentVariable("DB_NAME")};" +
                       $"Uid={Environment.GetEnvironmentVariable("DB_USER")};" +
                       $"Pwd={Environment.GetEnvironmentVariable("DB_PASSWORD")};" +
                       $"CharSet={Environment.GetEnvironmentVariable("DB_CHARSET")};";

builder.Services.AddDbContext<ProyectoContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Configurar JWT Authentication
var jwtKey = (Environment.GetEnvironmentVariable("JWT_KEY") ?? "").PadRight(32, '.');
var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER");
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE");

if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("JWT_KEY")))
    throw new InvalidOperationException("JWT_KEY environment variable is not set.");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Leer JWT desde cookie HttpOnly si no hay header Authorization
            var cookieToken = context.Request.Cookies["auth_token"];
            if (!string.IsNullOrEmpty(cookieToken))
                context.Token = cookieToken;

            return Task.CompletedTask;
        }
    };

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

// Antiforgery: Angular lee la cookie XSRF-TOKEN y envía el header X-XSRF-TOKEN
builder.Services.AddAntiforgery(options =>
{
    options.HeaderName = "X-XSRF-TOKEN";
    options.Cookie.Name = "XSRF-TOKEN";
    options.Cookie.HttpOnly = false;          // Angular necesita leerla con JS
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;  // Consistente con SameSite=None
    options.Cookie.SameSite = SameSiteMode.None;
});

// CORS: permite cookies con withCredentials desde Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "WebProyecto API",
        Version = "v1"
    });

    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Ingresa solo el token JWT, Swagger agrega 'Bearer' automáticamente."
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebProyecto API v1");
    c.RoutePrefix = "directory";
});

app.UseHttpsRedirection();
app.UseCors("AngularPolicy");
app.UseMiddleware<Backend.Middlewares.LoggingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
