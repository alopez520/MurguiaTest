using ejemplomvc.Exceptions;
using ejemplomvc.Models;
using ejemplomvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace ejemplomvc.Repositories
{
    public class TareaRepository : ITareaRepository
    {
        private readonly AppDbContext _context;

        public TareaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tarea>> GetAllAsync()
        {
            try
            {
                return await _context.Tareas.OrderByDescending(t => t.Important).ToListAsync();
            }
            catch (Exception e)
            {
                throw new RepositoryException("Error al consultar las tareas", e);
            }
        }

        public async Task<Tarea?> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Tareas.FindAsync(id);
            }
            catch (Exception e)
            {
                throw new RepositoryException($"Error al consultar la tarea", e);
            }
        }

        public async Task<Tarea> AddAsync(Tarea tarea)
        {
            try
            {
                _context.Tareas.Add(tarea);
                await _context.SaveChangesAsync();
                return tarea;
            }
            catch (Exception e)
            {
                throw new RepositoryException("Error al agregar una nueva tarea", e);
            }

        }

        public async Task UpdateAsync(Tarea tarea)
        {
            try
            {
                _context.Entry(tarea).State = EntityState.Modified;
                await _context.SaveChangesAsync();
               
            }
            catch (Exception e)
            {
                throw new RepositoryException($"Error al actualizar la tarea", e);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var tarea = await _context.Tareas.FindAsync(id);
                if (tarea != null)
                {
                    _context.Tareas.Remove(tarea);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e) {
                throw new RepositoryException($"Error al eliminar la tarea", e);
            }
        }
    }
}
