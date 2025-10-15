using ejemplomvc.Exceptions;
using ejemplomvc.Models;
using ejemplomvc.Repositories.Interfaces;
using System.Text;
using System.Threading;

namespace ejemplomvc.Repositories
{
    public class TareaRepositoryCsv : ITareaRepository
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _rutaCsv;

        private int getNextId()
        {
            IEnumerable<string> lineas = System.IO.File.ReadAllLines(_rutaCsv).Skip(1);
            if (lineas.Count() == 0)
            {
                return 1;
            }
            int lastId = int.Parse(lineas.Last().Split(',')[0]);
            return ++lastId;
        }

        public TareaRepositoryCsv(IWebHostEnvironment env)
        {
            _env = env;
            _rutaCsv = Path.Combine(_env.ContentRootPath, "Models", "Datos.csv");
            CheckDatabase();
        }

        public async Task<Tarea> AddAsync(Tarea tarea)
        {
            string nuevaTarea = $"{getNextId()},{tarea.Titulo},{tarea.Descripcion},{tarea.Completada},{tarea.Important},{DateTime.UtcNow}";


            if (File.Exists(_rutaCsv))
            {
                string? lastLine = File.ReadLines(_rutaCsv).LastOrDefault();
                if (!string.IsNullOrEmpty(lastLine) && !lastLine.EndsWith(Environment.NewLine))
                {
                    using (var fs = new FileStream(_rutaCsv, FileMode.Open, FileAccess.Write))
                    {
                        fs.Seek(0, SeekOrigin.End);
                        byte[] newLine = Encoding.UTF8.GetBytes(Environment.NewLine);
                        fs.Write(newLine, 0, newLine.Length);
                    }
                }
            }

            using (StreamWriter writer = new StreamWriter(_rutaCsv, append: true))
            {
                await writer.WriteLineAsync(nuevaTarea);
            }

            return tarea;
        }

        public async Task DeleteAsync(int id)
        {
            var tareas = (await GetAllAsync()).ToList();
            tareas.RemoveAll(t => t.Id == id);
            await File.WriteAllTextAsync(_rutaCsv, buildHeader() + Environment.NewLine + TareasToCsv(tareas));
        }

        public async Task<IEnumerable<Tarea>> GetAllAsync()
        {
            return await Task.Run(() =>
            {
                List<Tarea> tareas = [];

                var lineas = File.ReadAllLines(_rutaCsv, Encoding.UTF8).Skip(1);
                foreach (var linea in lineas)
                {
                    if (string.IsNullOrEmpty(linea)) continue;

                    var partes = linea.Split(',');

                    tareas.Add(new Tarea
                    {
                        Id = int.Parse(partes[0]),
                        Titulo = partes[1],
                        Descripcion = partes[2],
                        Completada = bool.Parse(partes[3]),
                        Important = bool.Parse(partes[4]),
                        FechaCreacion = DateTime.Parse(partes[5])
                    });
                }

                return tareas.AsEnumerable().OrderByDescending(t => t.Important);
            });
        }

        public async Task<Tarea?> GetByIdAsync(int id)
        {
            try
            {
                IEnumerable<Tarea> tareas = await GetAllAsync();
                return tareas.Where(t => t.Id == id).Single();
            }
            catch (Exception)
            {
                return null;
            }
        }

        public async Task UpdateAsync(Tarea tarea)
        {
            var tareas = (await GetAllAsync()).ToList();
            var tareaUpdate = tareas.FirstOrDefault(t => t.Id == tarea.Id);
            if (tareaUpdate != null)
            {
                tareaUpdate.Titulo = tarea.Titulo;
                tareaUpdate.Descripcion = tarea.Descripcion;
                tareaUpdate.Completada = tarea.Completada;
                tareaUpdate.Important = tarea.Important;
                tareaUpdate.FechaCreacion = tarea.FechaCreacion;

            }
            await File.WriteAllTextAsync(_rutaCsv, buildHeader() + Environment.NewLine + TareasToCsv(tareas));
        }

        private String TareasToCsv(IEnumerable<Tarea> tareas)
        {
            List<string> csvLines = [];
            foreach (var tarea in tareas)
            {
                csvLines.Add(TareaToCsvLine(tarea));
            }
            return string.Join(Environment.NewLine, csvLines);
        }

        private string TareaToCsvLine(Tarea tarea)
        {
            return $"{tarea.Id},{tarea.Titulo},{tarea.Descripcion},{tarea.Completada},{tarea.Important},{tarea.FechaCreacion}";
        }

        private string buildHeader()
        {
            return "Id,Titulo,Descripcion,Completada,Important,FechaCreacion";
        }

        private void CheckDatabase() {
            if (!File.Exists(_rutaCsv))
            {
                throw new RepositoryException("No fue posible conectar a la base de datos");
            }
            string? firstLine = File.ReadLines(_rutaCsv).FirstOrDefault();
            if (firstLine == null || !firstLine.StartsWith("Id,Titulo,Descripcion,Completada,Important,FechaCreacion"))
            {
                string content = File.ReadAllText(_rutaCsv);
                File.WriteAllText(_rutaCsv, buildHeader() + Environment.NewLine + content);
            }
        }
    }
}
