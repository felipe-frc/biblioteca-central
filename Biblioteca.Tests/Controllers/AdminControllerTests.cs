using System.Security.Claims;
using Biblioteca.Web.Constants;
using Biblioteca.Web.Controllers;
using Biblioteca.Web.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Biblioteca.Tests.Controllers;

public class AdminControllerTests
{
    private static AdminController CriarController(
        IConfiguration? configuration = null,
        AuthenticationServiceFake? authenticationService = null,
        ClaimsPrincipal? user = null)
    {
        authenticationService ??= new AuthenticationServiceFake();

        var services = new ServiceCollection()
            .AddSingleton<IAuthenticationService>(authenticationService)
            .BuildServiceProvider();

        var httpContext = new DefaultHttpContext
        {
            RequestServices = services,
            User = user ?? new ClaimsPrincipal(new ClaimsIdentity())
        };

        var controller = new AdminController(
            configuration ?? CriarConfiguracao("admin", "123456"),
            NullLogger<AdminController>.Instance)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            },
            TempData = new TempDataDictionary(
                httpContext,
                new TempDataProviderFake()),
            Url = new UrlHelperFake()
        };

        return controller;
    }

    private static IConfiguration CriarConfiguracao(
        string? usuario = "admin",
        string? senha = "123456")
    {
        var dados = new Dictionary<string, string?>();

        if (usuario is not null)
            dados["AdminCredentials:Username"] = usuario;

        if (senha is not null)
            dados["AdminCredentials:Password"] = senha;

        return new ConfigurationBuilder()
            .AddInMemoryCollection(dados)
            .Build();
    }

    private static LoginViewModel CriarLoginViewModel(
        string usuario = "admin",
        string senha = "123456",
        bool lembrarMe = false)
    {
        return new LoginViewModel
        {
            Usuario = usuario,
            Senha = senha,
            LembrarMe = lembrarMe
        };
    }

    // =========================================================
    // Login GET
    // =========================================================

    [Fact]
    public void Login_Get_UsuarioNaoAutenticado_DeveRetornarViewComModel()
    {
        var controller = CriarController();

        var result = controller.Login("/Dashboard");

        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsType<LoginViewModel>(viewResult.Model);

        Assert.Equal(string.Empty, model.Usuario);
        Assert.Equal(string.Empty, model.Senha);
        Assert.False(model.LembrarMe);
        Assert.Equal("/Dashboard", controller.ViewData["ReturnUrl"]);
    }

    [Fact]
    public void Login_Get_UsuarioAutenticado_DeveRedirecionarParaDashboard()
    {
        var identity = new ClaimsIdentity(
            new[] { new Claim(ClaimTypes.Name, "admin") },
            CookieAuthenticationDefaults.AuthenticationScheme);

        var user = new ClaimsPrincipal(identity);

        var controller = CriarController(user: user);

        var result = controller.Login();

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Dashboard", redirectResult.ControllerName);
    }

    // =========================================================
    // Login POST
    // =========================================================

    [Fact]
    public async Task Login_Post_ComModelStateInvalido_DeveRetornarViewComMesmoModel()
    {
        var controller = CriarController();
        var model = CriarLoginViewModel();

        controller.ModelState.AddModelError(nameof(model.Usuario), "O usuário é obrigatório.");

        var result = await controller.Login(model, "/Dashboard");

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Same(model, viewResult.Model);
        Assert.False(controller.ModelState.IsValid);
        Assert.Equal("/Dashboard", controller.ViewData["ReturnUrl"]);
    }

    [Fact]
    public async Task Login_Post_ComCredenciaisNaoConfiguradas_DeveAdicionarErroGeral()
    {
        var configuration = CriarConfiguracao(usuario: null, senha: null);
        var controller = CriarController(configuration);
        var model = CriarLoginViewModel();

        var result = await controller.Login(model);

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Same(model, viewResult.Model);
        Assert.True(controller.ModelState.ContainsKey(string.Empty));
        Assert.Contains(
            controller.ModelState[string.Empty]!.Errors,
            error => error.ErrorMessage == Messages.ErroCredenciaisAdminNaoConfiguradas);
    }

    [Fact]
    public async Task Login_Post_ComUsuarioNaoConfigurado_DeveAdicionarErroGeral()
    {
        var configuration = CriarConfiguracao(usuario: null, senha: "123456");
        var controller = CriarController(configuration);
        var model = CriarLoginViewModel();

        var result = await controller.Login(model);

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Same(model, viewResult.Model);
        Assert.Contains(
            controller.ModelState[string.Empty]!.Errors,
            error => error.ErrorMessage == Messages.ErroCredenciaisAdminNaoConfiguradas);
    }

    [Fact]
    public async Task Login_Post_ComSenhaNaoConfigurada_DeveAdicionarErroGeral()
    {
        var configuration = CriarConfiguracao(usuario: "admin", senha: null);
        var controller = CriarController(configuration);
        var model = CriarLoginViewModel();

        var result = await controller.Login(model);

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Same(model, viewResult.Model);
        Assert.Contains(
            controller.ModelState[string.Empty]!.Errors,
            error => error.ErrorMessage == Messages.ErroCredenciaisAdminNaoConfiguradas);
    }

    [Fact]
    public async Task Login_Post_ComCredenciaisInvalidas_DeveAdicionarErroGeral()
    {
        var controller = CriarController();
        var model = CriarLoginViewModel(usuario: "admin", senha: "senha-errada");

        var result = await controller.Login(model);

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Same(model, viewResult.Model);
        Assert.True(controller.ModelState.ContainsKey(string.Empty));
        Assert.Contains(
            controller.ModelState[string.Empty]!.Errors,
            error => error.ErrorMessage == Messages.ErroCredenciaisInvalidas);
    }

    [Fact]
    public async Task Login_Post_ComUsuarioInvalido_DeveAdicionarErroGeral()
    {
        var controller = CriarController();
        var model = CriarLoginViewModel(usuario: "outro-admin", senha: "123456");

        var result = await controller.Login(model);

        var viewResult = Assert.IsType<ViewResult>(result);

        Assert.Same(model, viewResult.Model);
        Assert.Contains(
            controller.ModelState[string.Empty]!.Errors,
            error => error.ErrorMessage == Messages.ErroCredenciaisInvalidas);
    }

    [Fact]
    public async Task Login_Post_ComCredenciaisValidas_DeveAutenticarERedirecionarParaDashboard()
    {
        var authenticationService = new AuthenticationServiceFake();
        var controller = CriarController(authenticationService: authenticationService);
        var model = CriarLoginViewModel(usuario: "admin", senha: "123456", lembrarMe: true);

        var result = await controller.Login(model);

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Dashboard", redirectResult.ControllerName);

        Assert.True(authenticationService.SignInChamado);
        Assert.Equal(CookieAuthenticationDefaults.AuthenticationScheme, authenticationService.SignInScheme);
        Assert.NotNull(authenticationService.SignInPrincipal);
        Assert.NotNull(authenticationService.SignInProperties);

        Assert.Equal("admin", authenticationService.SignInPrincipal!.Identity!.Name);
        Assert.True(authenticationService.SignInProperties!.IsPersistent);
        Assert.True(authenticationService.SignInProperties.ExpiresUtc > DateTimeOffset.UtcNow);

        Assert.Contains(
            authenticationService.SignInPrincipal.Claims,
            claim => claim.Type == ClaimTypes.Role && claim.Value == "Administrador");
    }

    [Fact]
    public async Task Login_Post_ComReturnUrlLocal_DeveRedirecionarParaReturnUrl()
    {
        var authenticationService = new AuthenticationServiceFake();
        var controller = CriarController(authenticationService: authenticationService);
        var model = CriarLoginViewModel();

        var result = await controller.Login(model, "/Livros");

        var redirectResult = Assert.IsType<RedirectResult>(result);

        Assert.Equal("/Livros", redirectResult.Url);
        Assert.True(authenticationService.SignInChamado);
    }

    [Fact]
    public async Task Login_Post_ComReturnUrlExterna_DeveIgnorarReturnUrlERedirecionarParaDashboard()
    {
        var authenticationService = new AuthenticationServiceFake();
        var controller = CriarController(authenticationService: authenticationService);
        var model = CriarLoginViewModel();

        var result = await controller.Login(model, "https://site-malicioso.com");

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal("Index", redirectResult.ActionName);
        Assert.Equal("Dashboard", redirectResult.ControllerName);
        Assert.True(authenticationService.SignInChamado);
    }

    [Fact]
    public async Task Login_Post_ComLembrarMeFalso_DeveAutenticarSemPersistencia()
    {
        var authenticationService = new AuthenticationServiceFake();
        var controller = CriarController(authenticationService: authenticationService);
        var model = CriarLoginViewModel(lembrarMe: false);

        var result = await controller.Login(model);

        Assert.IsType<RedirectToActionResult>(result);
        Assert.True(authenticationService.SignInChamado);
        Assert.False(authenticationService.SignInProperties!.IsPersistent);
    }

    // =========================================================
    // Logout
    // =========================================================

    [Fact]
    public async Task Logout_DeveDesautenticarUsuarioERedirecionarParaLogin()
    {
        var authenticationService = new AuthenticationServiceFake();
        var controller = CriarController(authenticationService: authenticationService);

        var result = await controller.Logout();

        var redirectResult = Assert.IsType<RedirectToActionResult>(result);

        Assert.Equal(nameof(AdminController.Login), redirectResult.ActionName);
        Assert.Null(redirectResult.ControllerName);
        Assert.True(authenticationService.SignOutChamado);
        Assert.Equal(CookieAuthenticationDefaults.AuthenticationScheme, authenticationService.SignOutScheme);
        Assert.Equal(Messages.LogoutRealizado, controller.TempData["Sucesso"]);
    }

    // =========================================================
    // Fakes
    // =========================================================

    private sealed class AuthenticationServiceFake : IAuthenticationService
    {
        public bool SignInChamado { get; private set; }
        public string? SignInScheme { get; private set; }
        public ClaimsPrincipal? SignInPrincipal { get; private set; }
        public AuthenticationProperties? SignInProperties { get; private set; }

        public bool SignOutChamado { get; private set; }
        public string? SignOutScheme { get; private set; }

        public Task<AuthenticateResult> AuthenticateAsync(HttpContext context, string? scheme)
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        public Task ChallengeAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            return Task.CompletedTask;
        }

        public Task ForbidAsync(HttpContext context, string? scheme, AuthenticationProperties? properties)
        {
            return Task.CompletedTask;
        }

        public Task SignInAsync(
            HttpContext context,
            string? scheme,
            ClaimsPrincipal principal,
            AuthenticationProperties? properties)
        {
            SignInChamado = true;
            SignInScheme = scheme;
            SignInPrincipal = principal;
            SignInProperties = properties;

            return Task.CompletedTask;
        }

        public Task SignOutAsync(
            HttpContext context,
            string? scheme,
            AuthenticationProperties? properties)
        {
            SignOutChamado = true;
            SignOutScheme = scheme;

            return Task.CompletedTask;
        }
    }

    private sealed class TempDataProviderFake : ITempDataProvider
    {
        public IDictionary<string, object> LoadTempData(HttpContext context)
        {
            return new Dictionary<string, object>();
        }

        public void SaveTempData(HttpContext context, IDictionary<string, object> values)
        {
        }
    }

    private sealed class UrlHelperFake : IUrlHelper
    {
        public ActionContext ActionContext => new();

        public string? Action(UrlActionContext actionContext)
        {
            return null;
        }

        public string? Content(string? contentPath)
        {
            return contentPath;
        }

        public bool IsLocalUrl(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return false;

            return url.StartsWith("/", StringComparison.Ordinal) &&
                   !url.StartsWith("//", StringComparison.Ordinal) &&
                   !url.StartsWith("/\\", StringComparison.Ordinal);
        }

        public string? Link(string? routeName, object? values)
        {
            return null;
        }

        public string? RouteUrl(UrlRouteContext routeContext)
        {
            return null;
        }
    }
}