using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using TestToDoList.Models;
using TestToDoList.Repositories;

namespace TestToDoList.Controllers
{
    [Authorize] //Solo usuarios logueados pueden acceder al sistema principal
    public class TareasController : Controller
    {
        //Repositorios que son TAREAS y USUARIOS
        private readonly TareaRepository _repo = new TareaRepository();
        private readonly UsuarioRepository _usuarioRepo = new UsuarioRepository();

        //Se obtiene el gmail del usuario desde la autenticacion
        private string GetCurrentUserEmail()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value ?? "";
        }

        //Se verifica si el rol es administrador
        private bool IsAdmin()
        {
            return User.IsInRole("Admin");
        }

        //Se muestran la lista de las tareas agregadas
        public IActionResult Index()
        {
            var tareas = _repo.ObtenerTodas();

            if (!IsAdmin())
            {
                //El usuario que no es administrador, solo ve sus tareas que le fueron asignadas
                var userEmail = GetCurrentUserEmail();
                tareas = tareas.Where(t => string.Equals(t.UsuarioAsignado, userEmail, StringComparison.OrdinalIgnoreCase)).ToList();
                ViewBag.UsuarioFiltro = userEmail;
            }
            else
            {
                //El admin puede ver las tareas de todos
                var usuarioFiltro = Request.Query["usuario"].ToString();
                if (!string.IsNullOrEmpty(usuarioFiltro))
                {
                    ViewBag.UsuarioFiltro = usuarioFiltro;
                    tareas = tareas.Where(t => string.Equals(t.UsuarioAsignado, usuarioFiltro, StringComparison.OrdinalIgnoreCase)).ToList();
                }
            }

            return View(tareas);
        }
        //Metodo de Crear tarea
        
        public IActionResult Create()
        {
            ViewBag.Usuarios = _usuarioRepo.ObtenerUsuariosActivos();
            return View(new Tarea
            {
                Prioridad = "Media",
                Estatus = "Pendiente",
                UsuarioAsignado = IsAdmin() ? "" : GetCurrentUserEmail()
            });
        }
        //Admin: puede seleccionar usuarios
        //usuario: ya tiene predeterminado su email  ya que no puede asignar
        [HttpPost]
        public IActionResult Create(Tarea tarea)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Usuarios = _usuarioRepo.ObtenerUsuariosActivos();
                return View(tarea);
            }

            if (!IsAdmin() && tarea.UsuarioAsignado != GetCurrentUserEmail())
            {
                ModelState.AddModelError("UsuarioAsignado", "Solo puede asignarse tareas a sí mismo");
                ViewBag.Usuarios = _usuarioRepo.ObtenerUsuariosActivos();
                return View(tarea);
            }

            _repo.Agregar(tarea);
            return RedirectToAction("Index");
        }

        //Metodo que edita
        public IActionResult Edit(int id)
        {
            var tarea = _repo.ObtenerPorId(id);
            if (tarea == null) return NotFound();

            if (!IsAdmin() && tarea.UsuarioAsignado != GetCurrentUserEmail())
            {
                return Forbid();
            }

            ViewBag.Usuarios = _usuarioRepo.ObtenerUsuariosActivos();
            return View(tarea);
        }

        [HttpPost]
        public IActionResult Edit(Tarea tarea)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Usuarios = _usuarioRepo.ObtenerUsuariosActivos();
                return View(tarea);
            }

            var tareaExistente = _repo.ObtenerPorId(tarea.Id);
            if (!IsAdmin() && tareaExistente?.UsuarioAsignado != GetCurrentUserEmail())
            {
                return Forbid();
            }

            if (!IsAdmin() && tarea.UsuarioAsignado != GetCurrentUserEmail())
            {
                ModelState.AddModelError("UsuarioAsignado", "Solo puede asignarse tareas a si mismo");
                ViewBag.Usuarios = _usuarioRepo.ObtenerUsuariosActivos();
                return View(tarea);
            }

            _repo.Editar(tarea);
            return RedirectToAction("Index");
        }

        //Metodo que elimina
        public IActionResult Delete(int id)
        {
            var tarea = _repo.ObtenerPorId(id);

            if (!IsAdmin() && tarea?.UsuarioAsignado != GetCurrentUserEmail())
            {
                return Forbid();
            }

            _repo.Eliminar(id);
            return RedirectToAction("Index");
        }

        //Metodo que muestra los detalles
        public IActionResult Details(int id)
        {
            var tarea = _repo.ObtenerPorId(id);
            if (tarea == null) return NotFound();

            if (!IsAdmin() && tarea.UsuarioAsignado != GetCurrentUserEmail())
            {
                return Forbid();
            }

            return View(tarea);
        }
    }
}