using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Shared.Common_Responses
{
    public enum ErrorType
    {
        Faluire=0,
        Validation=1,
        NotFound=2, 
        UnAuthorized=3,
        Forbidden=4,
        InvalidCredentials=5,   
    }
}
