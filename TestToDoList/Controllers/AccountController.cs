using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using TestToDoList.Models;
using TestToDoList.Repositories;

namespace TestToDoList.Controllers
{
    public class AccountController : Controller
    {
        private readonly UsuarioRepository _usuarioRepo = new UsuarioRepository();

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var usuario = _usuarioRepo.ObtenerPorEmail(email);
            if (usuario != null && usuario.Password == password)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Name, usuario.Nombre ?? ""),
                    new Claim(ClaimTypes.Email, usuario.Email ?? ""),
                    new Claim(ClaimTypes.Role, usuario.Rol ?? "Usuario")
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                return RedirectToAction("Index", "Tareas");
            }

            ViewBag.Error = "Email o contraseña incorrectos";
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(string Nombre, string Email, string Password)
        {
            if (_usuarioRepo.ObtenerPorEmail(Email) != null)
            {
                ViewBag.Error = "El email ya está registrado";
                return View();
            }

            var usuario = new Usuario
            {
                Nombre = Nombre,
                Email = Email,
                Password = Password,
                Rol = "Usuario"
            };

            _usuarioRepo.Agregar(usuario);

            ViewBag.Success = "Usuario registrado correctamente. Ahora puede iniciar sesión.";
            return View("Login");
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}