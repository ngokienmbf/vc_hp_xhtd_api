using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XHTDHP_API.Models.Wrapper
{
    public class Response<T>
    {
        public Response()
        {
        }

        public Response(T data)
        {
            Data = data;
            Succeeded = true;
            Errors = null;
            Message = string.Empty;
        }

        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
    }
}