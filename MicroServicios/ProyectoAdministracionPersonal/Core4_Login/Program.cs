using Core4_Login;
using Core4_Login.Repository;
using Core4_Login.Services;

var builder = WebApplication.CreateBuilder(args);

// ==========================================
// REPOSITORY - SINGLETON
// ==========================================

builder.Services.AddSingleton<
    IDbConnectionFactory,
    DbConnectionFactory>();

builder.Services.AddSingleton<
    UsuarioRepository>();

// ==========================================
// SERVICES - SINGLETON
// ==========================================

builder.Services.AddSingleton<
    ITokenService,
    TokenService>();

builder.Services.AddSingleton<
    ILoginService,
    LoginService>();

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

app.MapLoginEndpoints();

app.Run();