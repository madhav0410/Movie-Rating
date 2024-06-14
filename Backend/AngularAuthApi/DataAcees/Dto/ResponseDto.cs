using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAceess.Dto
{
    public class ResponseDto<T>
    {
       public bool Success { get; set; }
       public string? Message { get; set; }
       public T Data {  get; set; }
    }
}
