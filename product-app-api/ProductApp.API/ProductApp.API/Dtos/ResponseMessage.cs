using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProductApp.API.Dtos
{
    public class ResponseMessage 
    {
        public ResponseMessage(HttpStatusCode statusCode, bool status, dynamic message, dynamic data)
        {
            this.Code = statusCode;
            this.Status = status;
            this.Message = message;
            this.Data = data;
        }
        public HttpStatusCode Code { get; set; }
        public bool Status { get; set; }
        public dynamic Message { get; set; }
        public dynamic Data { get; set; }
    }
}
