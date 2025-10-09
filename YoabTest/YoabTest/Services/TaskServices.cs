using YoabTest.Models;

namespace YoabTest.Services
{
    public class TaskServices
    {
        private readonly string _filePath; //Funciona sin depender de rutas absolutas

        public TaskServices()
        {
            _filePath = Path.Combine(Directory.GetCurrentDirectory(), "tasks.csv"); //Asegura de encontrar "tasks.csv" en la carpeta correcta

            //Crea el archivo si no existe
            if (!File.Exists(_filePath))
            {
                File.WriteAllText(_filePath, "Id,Title,Description,IsCompleted,IsImportant,Deadline\n");
            }
        }

        public List<TaskModel> GetAll() //Metodo que obtiene todas las tareas
        {
            if (!File.Exists(_filePath)) //Comprueba si existe el archivo
                return new List<TaskModel>();

            return File.ReadAllLines(_filePath) //Lee todas las lineas y devuelve un string con cada linea
                .Skip(1) //Salta la primera linea
                .Where(line => !string.IsNullOrWhiteSpace(line)) //Filtra lineas vacias (null o " ")
                .Select(line =>
                {
                    var parts = line.Split(',');

                    return new TaskModel
                    {
                        Id= int.Parse(parts[0]),
                        Title= parts[1],
                        Description= parts[2],
                        IsCompleted=bool.Parse(parts[3]),
                        IsImportant=bool.Parse(parts[4]),
                        Deadline=DateTime.Parse(parts[5])
                    };
                })
                .ToList();
        }

        public TaskModel? GetById(int id)
        {
            var tasks = GetAll();
            return tasks.FirstOrDefault(
                t => t.Id == id);
        }

        public void Add(TaskModel task)
        {
            var tasks = GetAll(); //Asegura que cada tarea tenga un ID unico y consecutivo
            task.Id = tasks.Any() ? tasks.Max(
                t => t.Id)+1 : 1; //Si existen tareas toma el Id maximo y suma 1, de lo contrario asigna 1 como Id inicial

            var newLine = $"{task.Id},{task.Title},{task.Description},{task.IsCompleted},{task.IsImportant},{task.Deadline.ToString()}";
            File.AppendAllText(_filePath, $"{newLine}\n");
        }

        public void Update(TaskModel task)
        {
            var tasks = GetAll();
            var index = tasks.FindIndex(
                t => t.Id == task.Id);

            if (index == -1)
                return;

            tasks[index]= task;

            SaveAll(tasks);
        }

        public void ToggleComplete(int id)
        {
            var tasks = GetAll();
            var task = tasks.FirstOrDefault(
                t => t.Id == id);
            if (task != null)
            {
                task.IsCompleted = !task.IsCompleted; //Cambia el estado (true -> false, false->true)
                SaveAll(tasks);
            }
        }

        public void Delete(int id)
        {
            var tasks = GetAll();
            tasks.RemoveAll(t => t.Id == id);
            SaveAll(tasks);
        }

        public void SaveAll(List<TaskModel> tasks)
        {
            var lines = new List<string> { "Id, Title, Description, IsCompleted, IsImportant, Deadline" };
            lines.AddRange(tasks.Select(
                t => $"{t.Id},{t.Title},{t.Description},{t.IsCompleted},{t.IsImportant},{t.Deadline}"));

            File.WriteAllLines(_filePath, lines);
        }
    }
}
