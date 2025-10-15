using System.Text.Json.Serialization;

namespace ejemplomvc.DTOs
{
    public class ApiResponseDto<T>
    {
        [JsonPropertyName("exito")]
        public bool Exito { get; set; }
        [JsonPropertyName("mensaje")]
        public string Mensaje { get; set; }
        [JsonPropertyName("datos")]
        public T? Datos { get; set; }
        [JsonPropertyName("errores")]
        public List<string>? Errores { get; set; }

        public ApiResponseDto(bool exito, string mensaje, T? datos = default, List<string>? errores = null)
        {
            Exito = exito;
            Mensaje = mensaje;
            Datos = datos;
            Errores = errores;
        }
        public static ApiResponseDto<T> Success(T? datos)
        {
            return new ApiResponseDto<T>(true, "Operación realizada con éxito", datos, null);
        }

        public static ApiResponseDto<T> Error(String mensaje, List<String>? errores = null)
        {
            return new ApiResponseDto<T>(false, mensaje, default, errores);
        }
    }
}
