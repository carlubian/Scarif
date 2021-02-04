using System.IO;
using Microsoft.AspNetCore.Mvc;

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
