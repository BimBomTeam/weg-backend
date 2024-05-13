using Hangfire;
using Hangfire.Storage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WEG.Infrastructure.Services;

namespace WEG_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HangfireJobController : ControllerBase
    {
        [HttpPost]
        [Route("DeleteAllJobs")]
        public ActionResult DeleteAllJobs() {
            var recurringJobs = JobStorage.Current.GetConnection().GetRecurringJobs();
            foreach (var job in recurringJobs)
            {
                RecurringJob.RemoveIfExists(job.Id);
            }
            return Ok();
        }
    }
}
