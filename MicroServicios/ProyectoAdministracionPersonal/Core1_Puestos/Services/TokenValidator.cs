using System.Net.Http.Headers;

namespace Core1_Puestos.Services;

public class TokenValidator : ITokenValidator
{
    private readonly IHttpClientFactory
        _httpClientFactory;

    private readonly IConfiguration
        _configuration;

    public TokenValidator(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClientFactory =
            httpClientFactory;

        _configuration =
            configuration;
    }

    public async Task<bool> ValidarTokenAsync(
        string token)
    {
        string baseUrl =
            _configuration[
                "Services:Core4Login"]
            ?? throw new InvalidOperationException(
                "No se configuró la URL de Core4_Login.");

        string url =
            $"{baseUrl.TrimEnd('/')}/api/Login/validar";

        HttpClient client =
            _httpClientFactory.CreateClient();

        using HttpRequestMessage request =
            new(
                HttpMethod.Get,
                url);

        request.Headers.Authorization =
            new AuthenticationHeaderValue(
                "Bearer",
                token);

        try
        {
            using HttpResponseMessage response =
                await client.SendAsync(request);

            return response.IsSuccessStatusCode;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine(
                $"Error al validar el token: {ex.Message}");

            return false;
        }
    }
}