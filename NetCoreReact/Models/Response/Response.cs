using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace NetCoreReact.Models.Response
{
    public class Response<T>
    {
        public int Status { get; set; }

        public T Data { get; set; }

        public string Message { get; set; }

        public Response()
        {
            HttpStatusCode statusCode = HttpStatusCode.OK;

            Status = Convert.ToInt32(statusCode);
            Message = statusCode.ToString();
        }

        public Response(HttpStatusCode statusCode)
        {
            Status = Convert.ToInt32(statusCode);
            Message = statusCode.ToString();
        }

        public Response(HttpStatusCode statusCode, T data)
        {
            Status = Convert.ToInt32(statusCode);
            Message = statusCode.ToString();
            Data = data;
        }

        public Response(HttpStatusCode statusCode, string message)
        {
            Status = Convert.ToInt32(statusCode);
            Message = message;
        }

        public Response(HttpStatusCode statusCode, Exception exception)
        {
            Status = Convert.ToInt32(statusCode);
            Message = statusCode.ToString() + ": " + exception.ToString();
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    public class Response : Response<object>
    {
        public Response() : base()
        {

        }

        public Response(HttpStatusCode statusCode) : base(statusCode)
        {

        }

        public Response(HttpStatusCode statusCode, object data) : base(statusCode, data)
        {

        }

        public Response(HttpStatusCode statusCode, string message) : base(statusCode, message)
        {

        }

        public Response(HttpStatusCode statusCode, Exception exception) : base(statusCode, exception)
        {

        }
    }

}
