using Microsoft.AspNetCore.Mvc;

namespace Orchard.GoogleCloud.AppEngine.HealthCheck
{
    public class HealthCheckController : Controller
    {
        // https://cloud.google.com/appengine/docs/flexible/custom-runtimes/build#health_check_requests
        public StatusCodeResult IsOk()
        {
            return Ok();
        }
    }
}
