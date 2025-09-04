using System.Net;


namespace Services.DTO
{
    public class GenericResponse<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public required T Data { get; set; } 
    }
}
