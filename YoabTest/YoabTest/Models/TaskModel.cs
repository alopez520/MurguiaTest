namespace YoabTest.Models
{
    public class TaskModel
    {
        public int Id { get; set; } //Identificador unico
        public string Title { get; set; } = string.Empty; //Titulo de la tarea
        public string Description { get; set; } = string.Empty; //Descripcion detallada
        public bool IsCompleted { get; set; } //Estado de la tarea
        public bool IsImportant { get; set; } = false; // Destacar tareas importantes
        public DateTime Deadline { get; set; }//Fecha limite de entrega
    }
}
