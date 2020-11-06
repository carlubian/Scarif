using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Scarif.Server.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ApiController : ControllerBase
    {
        [HttpGet]
        [Produces("text/html")]
        public string Get()
        {
            using var file = new StreamReader("Pages/index.html");
            return file.ReadToEnd();
        }

    }
}
