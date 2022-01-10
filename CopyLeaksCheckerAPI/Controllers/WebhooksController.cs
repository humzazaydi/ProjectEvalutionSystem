using System;
using Copyleaks.SDK.V3.API.Models.Callbacks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CopyLeaksCheckerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        [HttpPost("[controller]/completed/{scanId}")]
        public ActionResult Completed([FromRoute] string scanId, [FromBody] CompletedCallback model)
        {
            Console.WriteLine($"scan {scanId} has found {model.Results.Score.IdenticalWords} identical copied words");
            // Do something with completed scan...
            return Ok();
        }

        [HttpPost("[controller]/indexed/{scanId}")]
        public ActionResult Indexed([FromRoute] string scanId, [FromBody] IndexOnlyCallback model)
        {
            Console.WriteLine($"scan {scanId} was indexed");
            return Ok();
        }

        [HttpPost("[controller]/creditsChecked/{scanId}")]
        public ActionResult Credits([FromRoute] string scanId, [FromBody] CreditsCheckCallback model)
        {
            Console.WriteLine($"scan {scanId} will consume {model.Credits}");
            // Decide whether or not to trigger the scan...
            return Ok();
        }

        [HttpPost("[controller]/error/{scanId}")]
        public ActionResult Error([FromRoute] string scanId, [FromBody] ErrorCallback model)
        {
            Console.WriteLine($"scan {scanId} completed with error: {model.Error.Message}");
            // Do something  with the error...
            return Ok();
        }

        [HttpPost("[controller]/results/{scanId}")]
        public ActionResult NewResultWebhook([FromRoute] string scanId, [FromBody] NewResultCallback model)
        {
            Console.WriteLine($"scan {scanId} got a new result");
            // Do something with the result...
            return Ok();
        }
	}
}
