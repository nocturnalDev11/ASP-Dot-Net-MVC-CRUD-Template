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
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/login", dto);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Invalid credentials.";
            return View();
        }

        var result = await response.Content.ReadFromJsonAsync<JwtResponseDto>();
        HttpContext.Session.SetString("JWT", result.Token);
        return RedirectToAction("Index", "Item");
    }

    public IActionResult Register() => View();

    [HttpPost]
    public async Task<IActionResult> Register(User user)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.PostAsJsonAsync("/api/auth/register", user);

        if (!response.IsSuccessStatusCode)
        {
            ViewBag.Error = "Registration failed.";
            return View();
        }

        return RedirectToAction("Login");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("JWT");
        return RedirectToAction("Login");
    }
}
