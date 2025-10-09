using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITaskService
    {
        public List<TaskEntity> Obtener();
        public List<TaskEntity> Obtener(string value);
        public TaskEntity Obtener(int id);
        public void Crear(TaskEntity task);
        public void Editar(TaskEntity task);
        public void Eliminar(int id);
        public void editarImportante(int id, bool value);
        public void editarCompletado(int id, bool value);
    }
}
