namespace ejemplomvc.DTOs
{
    public class TareaDto
    {
        public int? Id { get; set; }
        public string? Titulo { get; set; }
        public string? Descripcion { get; set; }
        public bool? Completada { get; set; }
        public bool? Destacada { get; set; }

        public TareaDto(int? Id, string? Titulo, string? Descripcion, bool? Completada, bool? Destacada)
        {
            this.Id = Id;
            this.Titulo = Titulo;
            this.Descripcion = Descripcion;
            this.Completada = Completada;
            this.Destacada = Destacada;
        }
    }
}
