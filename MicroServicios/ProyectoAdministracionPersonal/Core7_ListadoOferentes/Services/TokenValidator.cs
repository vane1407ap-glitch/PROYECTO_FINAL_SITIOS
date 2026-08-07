using System.Net.Http.Headers;

namespace Core7_ListadoOferentes.Services;

public class TokenValidator : ITokenValidator
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IConfiguration _configuration;

    public TokenValidator(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _configuration = configuration;
    }

    public async Task<bool> ValidarTokenAsync(string token)
    {
        string baseUrl =
            _configuration["Services:Core4Login"]
            ?? throw new InvalidOperationException(
                "No se configuró la URL de Core4_Login.");

        string url =
            $"{baseUrl.TrimEnd('/')}/api/Login/validar";

        HttpClient client =
            _httpClientFactory.CreateClient();

        using HttpRequestMessage request =
            new(HttpMethod.Get, url);

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        try
        {
            using HttpResponseMessage response =
                await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Token validado: {content}");
                return true;
            }

            Console.WriteLine($"Token inválido: {response.StatusCode}");
            return false;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error al validar el token: {ex.Message}");
            return false;
        }
    }
}