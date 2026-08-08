using Core7_ListadoOferentes;
using Core7_ListadoOferentes.Repository;
using Core7_ListadoOferentes.Services;

var builder =
    WebApplication.CreateBuilder(args);

// ==========================================
// REPOSITORY - SINGLETON
// ==========================================

builder.Services.AddSingleton<
    IDbConnectionFactory,
    DbConnectionFactory>();

builder.Services.AddSingleton<
    IOferenteRepository,
    OferenteRepository>();

// ==========================================
// SERVICES - SINGLETON
// ==========================================

builder.Services.AddSingleton<
    IOferenteService,
    OferenteService>();

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

app.MapListadoOferentesEndpoints();

// ==========================================
// HEALTH CHECK
// ==========================================

app.MapGet("/api/health", () =>
{
    return Results.Ok(new
    {
        status = "OK",
        service = "Core7_ListadoOferentes",
        timestamp = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ss.fffZ")
    });
});

app.Run();