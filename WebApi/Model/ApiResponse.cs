using System.Net;

namespace WebApi.Model
{
    public class ApiResponse
    {
        public HttpStatusCode StatusCode { get; set; }

        public bool IsExitoso { get; set; } = true;

        public List<string> Errors { get; set; }

        public Object Resultado { get; set; }
    }
}
