namespace WebAppToDoList.Models
{
    public class TaskModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool Important { get; set; }
        public bool Completed { get; set; }
        public DateTime DueDate { get; set; }

        public string Fecha {
            get
            {
                return DueDate.ToString("yyyy-MM-dd");
            }
            set
            {
                Fecha = value;
            }
        }
    }
}
