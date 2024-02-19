using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HangFireApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            BackgroundJob.Enqueue(() => BackgroundTestService.Test());
            return Ok("Hangfire worked");
        }
    }

    public class BackgroundTestService
    {
        public static void Test()
        {
            Console.WriteLine("Hangfire is running: " + DateTime.Now);
        }
    }
}
