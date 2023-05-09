using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Az_FnHttpAPI.Models
{
    /// <summary>
    /// This object will be serialized from Azure Function 
    /// to client app based on Http Request
    /// T is the actual data object that will be included into the response
    /// e.g. Department, Employee, etc.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ResponseObject<T>
    {
        public List<T> Records { get; set; }
        public T Record { get; set; }
        public string Message { get; set; }
        public int StatucCode { get; set; }
    }
}
