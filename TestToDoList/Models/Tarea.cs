using System;

namespace TestToDoList.Models
{
    //Nuestro modelo de tareas 
    public class Tarea
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public string? Prioridad { get; set; } // Alta, Media, Baja
        public string? Estatus { get; set; }   // Pendiente, En Proceso, Completada
        public string? UsuarioAsignado { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
