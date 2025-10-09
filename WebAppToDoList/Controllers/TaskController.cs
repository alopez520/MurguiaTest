using System.Diagnostics;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using WebAppToDoList.Models;

namespace WebAppToDoList.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITaskService _service;

        public TaskController(ITaskService sevice)
        {
            _service = sevice;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<TaskModel> tasks = new List<TaskModel>();
                foreach (var data in _service.Obtener())
                {
                    TaskModel task = new TaskModel()
                    {
                        Id = data.Id,
                        Title = data.Title,
                        Description = data.Description,
                        Important = data.Important,
                        Completed = data.Completed,
                        DueDate = data.DueDate
                    };
                    tasks.Add(task);
                }
                return Json(new { success = true, obj = tasks });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult GetId(int id)
        {
            try
            {
                var data = _service.Obtener(id);
                TaskModel task = new TaskModel()
                {
                    Id = data.Id,
                    Title = data.Title,
                    Description = data.Description,
                    Important = data.Important,
                    Completed = data.Completed,
                    DueDate = data.DueDate
                };
                return Json(new { success = true, obj = task });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult Create([FromBody] TaskModel data)
        {
            try
            {
                var task = new TaskEntity()
                {
                    Id = data.Id,
                    Title = data.Title,
                    Description = data.Description,
                    Important = data.Important,
                    Completed = data.Completed,
                    DueDate = data.DueDate
                };
                _service.Crear(task);
                return Json(new { success = true, message = "Tarea creada" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult Edit([FromBody] TaskModel data)
        {
            try
            {
                var task = new TaskEntity()
                {
                    Id = data.Id,
                    Title = data.Title,
                    Description = data.Description,
                    //Important = data.Important,
                    //Completed = data.Completed,
                    DueDate = data.DueDate
                };
                _service.Editar(task);
                return Json(new { success = true, message = "Tarea actualizada" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult Delete(int id)
        {
            try
            {
                _service.Eliminar(id);
                return Json(new { success = true, message = "Tarea eliminada" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult EditImportant(int id, [FromBody] bool value)
        {
            try
            {
                _service.editarImportante(id, value);
                return Json(new { success = true, message = "Estatus actualizado" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        public IActionResult EditCompleted(int id, [FromBody] bool value)
        {
            try
            {
                _service.editarCompletado(id, value);
                return Json(new { success = true, message = "Estatus actualizado" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult Search(string datos)
        {
            try
            {
                List<TaskModel> tasks = new List<TaskModel>();
                foreach (var data in _service.Obtener(datos))
                {
                    TaskModel task = new TaskModel()
                    {
                        Id = data.Id,
                        Title = data.Title,
                        Description = data.Description,
                        Important = data.Important,
                        Completed = data.Completed,
                        DueDate = data.DueDate
                    };
                    tasks.Add(task);
                }
                return Json(new { success = true, obj = tasks });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
