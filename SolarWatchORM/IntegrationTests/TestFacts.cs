using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using SolarWatchORM; 
using Microsoft.AspNetCore.Mvc.Testing;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Json;
using Microsoft.VisualStudio.TestPlatform.TestHost;
public class AuthTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public AuthTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            HandleCookies = true
        });
    }

    [Fact]
    public async Task Login_Should_Return_Token_On_Valid_Credentials()
    {
        var loginModel = new
        {
            userName = "admin",
            Password = "AdminPassword123!"
        };

        var response = await _client.PostAsJsonAsync("/api/Auth/login", loginModel);

        response.EnsureSuccessStatusCode();

        var setCookieHeader = response.Headers.GetValues("Set-Cookie");
        Assert.Contains(setCookieHeader, cookie => cookie.Contains("jwt"));
    }

    [Fact]
    public async Task GetProtectedResource_Should_Return_Unauthorized_If_Not_Authenticated()
    {
        var response = await _client.GetAsync("/api/SolarWatch/getData?CityName=Miskolc&Date=2000-12-12");

        Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetProtectedResource_Should_Return_OK_If_Authenticated()
    {
        var loginModel = new
        {
            userName = "admin",
            Password = "AdminPassword123!"
        };

        var loginResponse = await _client.PostAsJsonAsync("/api/Auth/login", loginModel);
        loginResponse.EnsureSuccessStatusCode();

        var jwtCookie = loginResponse.Headers.GetValues("Set-Cookie")
            .FirstOrDefault(c => c.StartsWith("jwt"));

        var jwt = jwtCookie.Split(';').First().Split('=').Last();

        _client.DefaultRequestHeaders.Add("Cookie", $"jwt={jwt}");

        var response = await _client.GetAsync("/api/SolarWatch/getData?CityName=Miskolc&Date=2000-12-12");

        response.EnsureSuccessStatusCode();
    }
}
