using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        //this Class Handle Reult and Check For it then Send this To any class will inherit from ApiBaseController
        //كل دا بعمله عشان مش عايز الController بتاعه يعمل Check For Result Data اللى راجعه عشان مش عايزه يعمل اى logic
    }
}
