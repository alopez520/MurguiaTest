using ejemplomvc.DTOs;
using ejemplomvc.Exceptions;
using ejemplomvc.Models;
using ejemplomvc.Repositories.Interfaces;
using ejemplomvc.Services.Interfaces;

namespace ejemplomvc.Services
{
    public class TareaService : ITareaService
    {
        private readonly ITareaRepository _repo;

        public TareaService(ITareaRepository repo)
        {
            _repo = repo;
        }

        public async Task<IEnumerable<TareaDto>> GetTareas()
        {
            var tareas = await _repo.GetAllAsync();
            return tareas.Select(t => new TareaDto(t.Id, t.Titulo, t.Descripcion, t.Completada, t.Important));
        }

        public async Task<TareaDto?> GetTarea(int id)
        {
            var t = await _repo.GetByIdAsync(id);
            return t == null ? null : new TareaDto(t.Id, t.Titulo, t.Descripcion, t.Completada, t.Important);
        }

        public async Task<TareaDto> CreateTarea(TareaDto dto)
        {
            if (string.IsNullOrEmpty(dto.Titulo))
            {
                throw new ServiceException("El nombre no puede estar vacío.");
            }

            if (string.IsNullOrEmpty(dto.Descripcion))
            {
                throw new ServiceException("La descripcion no puede estar vacía.");
            }
            var tarea = new Tarea { Titulo = dto.Titulo, Descripcion = dto.Descripcion };
            var creada = await _repo.AddAsync(tarea);
            return new TareaDto(creada.Id, creada.Titulo, creada.Descripcion, creada.Completada, creada.Important);
        }

        public async Task<bool> UpdateTarea(int id, TareaDto dto)
        {
            var tarea = await _repo.GetByIdAsync(id);
            if (tarea == null) return false;

            if (string.IsNullOrEmpty(dto.Titulo))
            {
                throw new ServiceException("El nombre no puede estar vacío.");
            }

            if (string.IsNullOrEmpty(dto.Descripcion))
            {
                throw new ServiceException("La descripcion no puede estar vacía.");
            }

            tarea.Titulo = dto.Titulo;
            tarea.Descripcion = dto.Descripcion;
            tarea.Completada = dto.Completada ?? false;

            await _repo.UpdateAsync(tarea);
            return true;
        }

        public async Task<bool> DeleteTarea(int id)
        {
            var tarea = await _repo.GetByIdAsync(id);
            if (tarea == null) return false;

            await _repo.DeleteAsync(id);
            return true;
        }

        public async Task<bool> PathTarea(int id, TareaDto dto)
        {
            Tarea? tarea = await _repo.GetByIdAsync(id);
            if (tarea == null)
                return false;

            if (dto.Completada.HasValue)
                tarea.Completada = dto.Completada.Value;

            if (dto.Destacada.HasValue)
                tarea.Important = dto.Destacada.Value;

            if (dto.Titulo != null)
            {
                if (dto.Titulo == string.Empty)
                {
                    throw new ServiceException("El nombre no puede estar vacío.");
                }
                tarea.Titulo = dto.Titulo;
            }


            if (dto.Descripcion != null)
            {
                if (dto.Descripcion == string.Empty)
                {
                    throw new ServiceException("La descripcion no puede estar vacía.");
                }
                tarea.Descripcion = dto.Descripcion;
            }

            await _repo.UpdateAsync(tarea);

            return true;
        }
    }
}
