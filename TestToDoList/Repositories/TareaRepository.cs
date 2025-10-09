//Instancia para acceder a los datos de las tareas

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestToDoList.Models;

namespace TestToDoList.Repositories
{
    public class TareaRepository
    {
        private readonly string rutaArchivo;

        //Se crea la carpeta DATA por si no existe, y si no contiene el archivo tareas.csv crea uno.
        public TareaRepository()
        {
            var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
            rutaArchivo = Path.Combine(dataDir, "tareas.csv");

            if (!File.Exists(rutaArchivo))
            {
                File.WriteAllText(rutaArchivo, "Id,Titulo,Descripcion,Prioridad,Estatus,UsuarioAsignado,FechaCreacion\n");
            }
        }

        //Hace la divicion en el archivo CSV por comas, en este caso cuando se van agregando
        private string[] SafeSplit(string line)
        {
            return line.Split(',');
        }

        //Se empiezan a leer los datos
        public List<Tarea> ObtenerTodas()
        {
            var tareas = new List<Tarea>();
            var lineas = File.ReadAllLines(rutaArchivo).Skip(1); // se saltan los encabezados
            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue; //Ignora lineas vacías, para que no se paré el proceso
                var valores = SafeSplit(linea);
                if (valores.Length < 7) continue;                   
                if (!int.TryParse(valores[0], out var id)) continue; //El ID lo convierte en numero
                if (!DateTime.TryParse(valores[6], out var fecha)) fecha = DateTime.Now; //Si la fecha fallo, se va a colocar la fecha actual
                tareas.Add(new Tarea  //Aqui es donde se crea el objeto de la Tarea
                {
                    Id = id,
                    Titulo = valores[1],
                    Descripcion = valores[2],
                    Prioridad = valores[3],
                    Estatus = valores[4],
                    UsuarioAsignado = valores[5],
                    FechaCreacion = fecha
                });
            }
            return tareas.OrderBy(t => t.Id).ToList(); //Realiza un return por ID
        }


        public void Guardar(List<Tarea> tareas)
        {
            using var writer = new StreamWriter(rutaArchivo);
            writer.WriteLine("Id,Titulo,Descripcion,Prioridad,Estatus,UsuarioAsignado,FechaCreacion");
            foreach (var t in tareas)
            {
                //Realiza una limpieza en las lineas, como saltos de lineas que es uno de los mas comunes
                string safe(string? s) => (s ?? "").Replace("\n"," ").Replace("\r"," ").Replace(",",";");
                writer.WriteLine($"{t.Id},{safe(t.Titulo)},{safe(t.Descripcion)},{safe(t.Prioridad)},{safe(t.Estatus)},{safe(t.UsuarioAsignado)},{t.FechaCreacion:O}");
            }
        }

        //Aqui es donde se crea una nueva tarea
        public void Agregar(Tarea tarea)
        {
            var tareas = ObtenerTodas();
            tarea.Id = tareas.Any() ? tareas.Max(t => t.Id) + 1 : 1; //Se aplica el ID autoincremental
            tarea.FechaCreacion = DateTime.Now;
            tareas.Add(tarea);
            Guardar(tareas);
        }

        //Realiza la actualización de la tarea 
        public void Editar(Tarea tarea)
        {
            var tareas = ObtenerTodas();
            var tareaExistente = tareas.FirstOrDefault(t => t.Id == tarea.Id);
            if (tareaExistente != null)
            {
                tareaExistente.Titulo = tarea.Titulo;
                tareaExistente.Descripcion = tarea.Descripcion;
                tareaExistente.Prioridad = tarea.Prioridad;
                tareaExistente.Estatus = tarea.Estatus;
                tareaExistente.UsuarioAsignado = tarea.UsuarioAsignado;
                Guardar(tareas);
            }
        }

        //Borra la tarea eliminar
        public void Eliminar(int id)
        {
            var tareas = ObtenerTodas();
            tareas.RemoveAll(t => t.Id == id);
            Guardar(tareas);
        }

        //Se utiliza para la busqueda por ID esto se utiliza en el controller
        public Tarea? ObtenerPorId(int id)
        {
            return ObtenerTodas().FirstOrDefault(t => t.Id == id);
        }
    }
}
