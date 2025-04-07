using Application;
using Domain.Services;
using Infrastructure;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Forțează aplicația să asculte pe portul 8080 și toate interfețele de rețea
builder.WebHost.UseUrls("http://0.0.0.0:8080");

// Configurare CORS
var AllowAllOrigins = "AllowAllOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: AllowAllOrigins,
        policy =>
        {
            policy.AllowAnyOrigin();
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
        });
});

// Retrieve the connection string from the configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Adăugarea DbContext în colecția de servicii
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Configurare servicii
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration, false); // Assuming you don't need the in-memory database

// Configurare HttpClient pentru serviciile externe
builder.Services.AddHttpClient<IMedicationExternalService, MedicationExternalService>(client =>
{
    // Folosește host.docker.internal pentru comunicare între containere
    client.BaseAddress = new Uri("http://host.docker.internal:8000");
});

// Adaugă serviciul HttpClient generic (pentru controllerul tău care apelează funcția Cloud)
builder.Services.AddHttpClient();

// Servicii adiționale
builder.Services.AddSingleton<GoogleCalendarService>();
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
builder.Services.AddControllers();

// Configurare Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "JWT Token from login endpoint",
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
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

// Middleware Pipeline
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "PHMS API v1");
        c.RoutePrefix = string.Empty; // Accesează Swagger la rădăcină
    });
}

app.UseStaticFiles();
app.UseRouting();
app.UseCors(AllowAllOrigins);
app.UseAuthorization();

// Endpoint simplu pentru verificare healthcheck
app.MapGet("/", () => Results.Ok("PHMS API is running!"));
app.MapControllers();

await app.RunAsync();

public partial class Program { }