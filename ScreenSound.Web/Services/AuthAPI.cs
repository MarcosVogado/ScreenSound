using System.Security;

namespace ScreenSound.Web.Services;

public class AuthAPI(IHttpClientFactory factory)
{
    private readonly HttpClient _HttpClient = factory.CreateClient("API");
    
    public Task<AuthResponse> LoginAsync(string email, SecureString senha)
    {

    }
}
