using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TestToDoList.Models;

namespace TestToDoList.Repositories
{
    public class UsuarioRepository
    {
        private readonly string rutaArchivo;

        public UsuarioRepository()
        {
            var dataDir = Path.Combine(Directory.GetCurrentDirectory(), "Data");
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
            rutaArchivo = Path.Combine(dataDir, "usuarios.csv");

            if (!File.Exists(rutaArchivo))
            {
                File.WriteAllText(rutaArchivo, "Id,Nombre,Email,Password,Rol\n");
                // Crear usuario admin por defecto
                var usuarios = new List<Usuario>
                {
                    new Usuario { Id = 1, Nombre = "Admin", Email = "admin@correo.com", Password = "admin123", Rol = "Admin" }
                };
                Guardar(usuarios);
            }
        }

        public List<Usuario> ObtenerTodos()
        {
            var usuarios = new List<Usuario>();
            if (!File.Exists(rutaArchivo)) return usuarios;

            var lineas = File.ReadAllLines(rutaArchivo).Skip(1);
            foreach (var linea in lineas)
            {
                if (string.IsNullOrWhiteSpace(linea)) continue;
                var valores = linea.Split(',');
                if (valores.Length < 5) continue;
                if (!int.TryParse(valores[0], out var id)) continue;

                usuarios.Add(new Usuario
                {
                    Id = id,
                    Nombre = valores[1],
                    Email = valores[2],
                    Password = valores[3],
                    Rol = valores[4]
                });
            }
            return usuarios;
        }

        public void Guardar(List<Usuario> usuarios)
        {
            using var writer = new StreamWriter(rutaArchivo);
            writer.WriteLine("Id,Nombre,Email,Password,Rol");
            foreach (var u in usuarios)
            {
                writer.WriteLine($"{u.Id},{u.Nombre},{u.Email},{u.Password},{u.Rol}");
            }
        }

        public void Agregar(Usuario usuario)
        {
            var usuarios = ObtenerTodos();
            usuario.Id = usuarios.Any() ? usuarios.Max(u => u.Id) + 1 : 1;
            usuarios.Add(usuario);
            Guardar(usuarios);
        }

        public Usuario? ObtenerPorEmail(string email)
        {
            return ObtenerTodos().FirstOrDefault(u => u.Email?.ToLower() == email.ToLower());
        }

        public Usuario? ObtenerPorId(int id)
        {
            return ObtenerTodos().FirstOrDefault(u => u.Id == id);
        }

        public List<Usuario> ObtenerUsuariosActivos()
        {
            return ObtenerTodos().Where(u => u.Rol != "Admin").ToList();
        }
    }
}