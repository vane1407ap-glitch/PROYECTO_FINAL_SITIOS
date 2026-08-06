using Core1_Puestos;
using Core1_Puestos.Repository;
using Core1_Puestos.Services;

var builder =
    WebApplication.CreateBuilder(args);

// ==========================================
// REPOSITORY - SINGLETON
// ==========================================

builder.Services.AddSingleton<
    IDbConnectionFactory,
    DbConnectionFactory>();

builder.Services.AddSingleton<
    IPuestoRepository,
    PuestoRepository>();

// ==========================================
// SERVICES - SINGLETON
// ==========================================

builder.Services.AddSingleton<
    IPuestoService,
    PuestoService>();

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

app.MapPuestoEndpoints();

app.Run();