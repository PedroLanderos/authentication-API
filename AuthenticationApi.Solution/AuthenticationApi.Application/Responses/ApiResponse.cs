using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationApi.Application.Responses
{
    public record ApiResponse(bool Flag = false, string Message = null!);
    
   
}
