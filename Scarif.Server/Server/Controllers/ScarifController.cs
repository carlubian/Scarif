using Google.Protobuf;
using Microsoft.AspNetCore.Mvc;
using Scarif.Protobuf;
using Scarif.Server.Server.Persistence;

namespace Scarif.Server.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ScarifController
    {
        [HttpPut]
        public void PutNewApp([FromForm]string appId, [FromForm]string appName)
        {
            ScarifAdapter.InsertApp(appId, appName);
        }

        [HttpPost]
        public void PostNewLog([FromForm]string base64proto)
        {
            var message = LogMessage.Parser.ParseFrom(ByteString.FromBase64(base64proto));
            ScarifAdapter.InsertLog(message);
        }
    }
}
