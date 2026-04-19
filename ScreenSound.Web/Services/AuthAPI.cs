using Microsoft.AspNetCore.Components.Authorization;
using ScreenSound.Web.Response;
using System.Net.Http.Json;
using System.Security;
using System.Security.Claims;

namespace ScreenSound.Web.Services;

public class AuthAPI(IHttpClientFactory factory) : AuthenticationStateProvider
{
    private bool _authenticated = false;

    private readonly HttpClient _HttpClient = factory.CreateClient("API");

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        _authenticated = false;

        var pessoa = new ClaimsPrincipal();

        var response = await _HttpClient.GetAsync("auth/manage/info");

        if (response.IsSuccessStatusCode)
        {
            var info = await response.Content.ReadFromJsonAsync<InfoPessoaResponse>();

            Claim[] dados =
            [
                new Claim(ClaimTypes.Name, info.Email),
                new Claim(ClaimTypes.Email, info.Email)
            ];

            var identity = new ClaimsIdentity(dados, "Cookies");
            pessoa = new ClaimsPrincipal(identity);
            _authenticated = true;
        }

        return new AuthenticationState(pessoa);
    }

    public async Task<AuthResponse> LoginAsync(string email, string senha)
    {
        var response = await _HttpClient.PostAsJsonAsync("auth/login?useCookies=true", new
        {
            email,
            password = senha
        });

        if (response.IsSuccessStatusCode)
        {
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return new AuthResponse { Sucesso = true };
        }

        return new AuthResponse { Sucesso = false, Erros = ["Login ou senha inválidos"] };
    }

    public async Task LogoutAsync()
    {
        await _HttpClient.PostAsync("auth/logout", null);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<bool> VerificaAutenticado()
    {
        await GetAuthenticationStateAsync();
        return _authenticated;
    }
}
