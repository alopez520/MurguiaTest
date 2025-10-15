using ejemplomvc.DTOs;
using ejemplomvc.Models;
using ejemplomvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ejemplomvc.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TareasController : ControllerBase
    {
        private readonly ITareaService _service;
        public TareasController(ITareaService service, AppDbContext dbContext)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            IEnumerable<TareaDto> tareas = await _service.GetTareas();
            return Ok(ApiResponseDto<Object>.Success(tareas));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var tarea = await _service.GetTarea(id);
            return tarea == null
                ? NotFound(ApiResponseDto<object>.Error("No se encontro la tarea solicitada"))
                : Ok(ApiResponseDto<object>.Success(tarea));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TareaDto dto)
        {
            var nueva = await _service.CreateTarea(dto);
            return CreatedAtAction(nameof(Get), new { id = nueva.Id }, ApiResponseDto<object>.Success(nueva));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] TareaDto dto)
        {
            var actualizado = await _service.UpdateTarea(id, dto);
            return actualizado ? Ok(ApiResponseDto<object>.Success(null)) : NotFound(ApiResponseDto<object>.Error("No se encontro la tarea a actualizar"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var eliminado = await _service.DeleteTarea(id);
            return eliminado ? Ok(ApiResponseDto<object>.Success(null)) : NotFound(ApiResponseDto<object>.Error("No se encontro la tarea a eliminar"));
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(int id, [FromBody] TareaDto dto)
        {
            var modificado = await _service.PathTarea(id, dto);
            return modificado ? Ok(ApiResponseDto<object>.Success(null)) : NotFound(ApiResponseDto<object>.Error("No se encontro la tarea a actualizar"));
        }
    }
}
