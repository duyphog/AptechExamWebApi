using System;
namespace api.Models
{
    public class ErrorResponse<T>
    {
        public int ErrorCode { get; set; }

        public string Message { get; set; }

        public T Data { get; set; }
    }
}
