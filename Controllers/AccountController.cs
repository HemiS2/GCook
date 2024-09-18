using Microsoft.AspNetCore.Mvc;
using GCook.ViewModels;
using GCook.Services;

namespace GCook.Controllers;

public class AccountController : Controller
{
        private readonly ILogger<AccountController> _logger;
        private readonly IUsuarioService _usuarioService;

        public AccountController(
            ILogger<AccountController> logger,
            IUsuarioService usuarioService
        )
        {
            _logger = logger;
            _usuarioService = usuarioService;
        }

        [HttGet]
        public IActionResult Login(string returnUrl)
        { 
            LoginVM login = new()
            {
                UrlRetorno = returnUrl ?? returnUrl.Content("~/")
            };
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IAction> Login(LoginVM login)
        {

            if (ModelState.IsValid)
            {

                var result = await _usuarioService.LoginUsuario(login);
                if (result.Succeded)
                    return LocalRedirect(login.UrlRetorno);
                if (result.IsLockedOut)
                    return RedirectToAction("Lockout");
                if (result.IsNotAllowed)
                    ModelState.AddModelError(string.Empty, "Sua conta está confirmada, verifique seu email!!");
                else
                    ModelState.AddModelError(string.Empty, "Usuário e/ou Senha Inválidos!!!");
            }

            return View(login);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _usuariosService.LogoffUsuario();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]