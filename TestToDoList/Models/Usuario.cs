namespace TestToDoList.Models
{
    //Nuestro modelo de datos para el manejo de usuarios
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Rol { get; set; }
    }
}