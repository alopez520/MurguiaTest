using ejemplomvc.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace ejemplomvc.Repositories.Interfaces
{
    public interface ITareaRepository : IRepository<Tarea>
    {
    }
}
