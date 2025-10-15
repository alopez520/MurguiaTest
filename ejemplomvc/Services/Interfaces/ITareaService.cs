using ejemplomvc.DTOs;

namespace ejemplomvc.Services.Interfaces
{
    public interface ITareaService
    {
        Task<IEnumerable<TareaDto>> GetTareas();
        Task<TareaDto?> GetTarea(int id);
        Task<TareaDto> CreateTarea(TareaDto dto);
        Task<bool> UpdateTarea(int id, TareaDto dto);
        Task<bool> DeleteTarea(int id);
        Task<bool> PathTarea(int id, TareaDto dto);
    }
}
