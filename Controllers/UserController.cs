using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using ASP_Dot_Net_MVC_CRUD_Template.Models;

public class UserController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;

    public UserController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto dto)
    {
        var client = _httpClientFactory.CreateClient("AuthClient");

        var response = await client.PostAsJsonAsync("/api/auth/login", dto);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Invalid credentials.";
            return View();
        }

        var result = await response.Content.ReadFromJsonAsync<JwtResponseDto>();

        if (result == null || string.IsNullOrEmpty(result.Token))
        {
            ViewBag.Error = "Invalid JWT response.";
            return View();
        }

        HttpContext.Session.SetString("JWT", result.Token);

        return RedirectToAction("Index", "Item");
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        var client = _httpClientFactory.CreateClient("AuthClient");

        var response = await client.PostAsJsonAsync("/api/auth/register", user);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Registration failed.";
            return View();
        }

        return RedirectToAction("Login");
    }

    public async Task<IActionResult> Profile()
    {
        var token = HttpContext.Session.GetString("JWT");
        if (string.IsNullOrEmpty(token))
        {
            return RedirectToAction("Login");
        }

        var client = _httpClientFactory.CreateClient();

        client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        var response = await client.GetAsync("/api/user/profile");

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Failed to load profile.";
            return RedirectToAction("Login");
        }

        var profile = await response.Content.ReadFromJsonAsync<UserProfileDto>();
        return View(profile);
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("JWT");
        return RedirectToAction("Login");
    }
}
