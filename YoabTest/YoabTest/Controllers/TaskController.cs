using Microsoft.AspNetCore.Mvc;
using YoabTest.Models;
using YoabTest.Services;

namespace YoabTest.Controllers
{
    public class TaskController : Controller
    {
        private readonly TaskServices _taskService;

        public TaskController()
        {
            _taskService = new TaskServices();
        }

        public IActionResult Index()
        {
            var task = _taskService.GetAll();
            return View(task);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                _taskService.Add(task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        public IActionResult Edit(int id)
        {
            var task = _taskService.GetAll().FirstOrDefault(
                t => t.Id == id);
            if (task ==null)
                return NotFound();
            return View(task);
        }

        [HttpPost]
        public IActionResult ToggleComplete(int id)
        {
            _taskService.ToggleComplete(id);
            return Json(new { success = true });
        }

        [HttpPost]
        public IActionResult Edit(TaskModel task)
        {
            if (ModelState.IsValid)
            {
                _taskService.Update(task);
                return RedirectToAction(nameof(Index));
            }
            return View(task);
        }

        [HttpPost]
        public IActionResult ToggleImportant(int id)
        {
            var task = _taskService.GetById(id);
            if (task == null) return Json(new { success = false }); //Pensado para AJAX, el frontend hace una peticion asincrona al aservidor sin recargar la pagina

            task.IsImportant = !task.IsImportant; //Invierte el valor
            _taskService.Update(task);

            return Json(new { success = true });
        }

        public IActionResult Delete (int id)
        {
            var task = _taskService.GetAll().FirstOrDefault(
                t => t.Id == id);
            return View(task);
        }

        [HttpPost]
        public IActionResult DeleteConfirmed(int id)
        {
            _taskService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult CreateOrEdit(TaskModel task)
        {
            if(task.Deadline < DateTime.Today)
            {
                ModelState.AddModelError("Deadline", "La fecha limite no puede ser anterior a la de hoy");
            }

            if (!ModelState.IsValid)
            {
                var tasks = _taskService.GetAll();
                return View("Index", tasks);
            }

            if (task.Id == 0)
                _taskService.Add(task);
            else
                _taskService.Update(task);

            return RedirectToAction(nameof(Index));
        }

    }
}
