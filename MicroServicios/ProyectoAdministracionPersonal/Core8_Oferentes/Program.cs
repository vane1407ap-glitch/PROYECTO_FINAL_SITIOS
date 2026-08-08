using Core8_Oferentes;
using Core8_Oferentes.Repository;
using Core8_Oferentes.Services;

var builder =
    WebApplication.CreateBuilder(args);

// ==========================================
// REPOSITORY - SINGLETON
// ==========================================

builder.Services.AddSingleton<
    IDbConnectionFactory,
    DbConnectionFactory>();

builder.Services.AddSingleton<
    IOferenteCORE8Repository,
    OferenteCORE8Repository>();

// ==========================================
// SERVICES - SINGLETON
// ==========================================

builder.Services.AddSingleton<
    IOferenteCORE8Service,
    OferenteCORE8Service>();

builder.Services.AddHttpClient();

builder.Services.AddSingleton<
    ITokenValidator,
    TokenValidator>();

// ==========================================
// CORS
// ==========================================

builder.Services.AddCors(options =>
{
    options.AddPolicy(
        "CorsPolicy",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// ==========================================
// MIDDLEWARE
// ==========================================

app.UseCors("CorsPolicy");

// ==========================================
// MINIMAL API ENDPOINTS
// ==========================================

app.MapOferentesEndpoints();

// ==========================================
// HEALTH CHECK
// ==========================================

app.MapGet("/api/health", () =>
{
    return Results.Ok(new
    {
        status = "OK",
        service = "Core8_Oferentes",
        timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
    });
});

app.Run();