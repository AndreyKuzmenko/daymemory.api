using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayMemory.Core.Models.Exceptions
{
    public class ErrorDto
    {
        public ErrorDto(string message, int? code = null, object? data = null)
        {
            Message = message;
            Code = code;
            Data = data;
        }
        public string Message { get; set; }

        public int? Code { get; set; }

        public object? Data { get; set; }
    }
}
