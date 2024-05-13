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
        [Route("Daily Job")]
        public ActionResult CreateSimpleRecurringJob()
        {
            RecurringJob.AddOrUpdate("DailyJob", () => Console.WriteLine("Zadanie wykonane o: " + DateTime.UtcNow.ToString()), Cron.Daily(22,1)); //Czas UTC (2h do tyłu. U nas będzie 00:01)
            return Ok();
        }


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
