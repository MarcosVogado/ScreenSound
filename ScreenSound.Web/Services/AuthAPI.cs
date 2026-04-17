using ScreenSound.Web.Response;
using System.Net.Http.Json;
using System.Security;

namespace ScreenSound.Web.Services;

public class AuthAPI(IHttpClientFactory factory)
{
    private readonly HttpClient _HttpClient = factory.CreateClient("API");
    
    public async Task<AuthResponse> LoginAsync(string email, string senha)
    {
        var response = await _HttpClient.PostAsJsonAsync("auth/login?useCookies=true", new
        {
            email,
            password = senha
        });

        if (response.IsSuccessStatusCode)
        {
            return new AuthResponse { Sucesso = true };
        }

        return new AuthResponse { Sucesso = false, Erros = ["Login ou senha inválidos"] };
    }
}
