using Core3_Empleados;
using Core3_Empleados.Repository;
using Core3_Empleados.Services;

var builder =
    WebApplication.CreateBuilder(args);

// ==========================================
// REPOSITORY - SINGLETON
// ==========================================

builder.Services.AddSingleton<
    IDbConnectionFactory,
    DbConnectionFactory>();

builder.Services.AddSingleton<
    IEmpleadoRepository,
    EmpleadoRepository>();

// ==========================================
// SERVICES - SINGLETON
// ==========================================

builder.Services.AddSingleton<
    IEmpleadoService,
    EmpleadoService>();

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

app.MapEmpleadoEndpoints();

app.Run();