using Microsoft.AspNetCore.Mvc;
using WritingRabbitQueues.Interfaces;
using WritingRabbitQueues.Models;

namespace WritingRabbitQueues.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationSampleController : ControllerBase
    {
        private readonly IQueueManager queueManager;

        public LocationSampleController(IQueueManager queueManager)
        {
            this.queueManager = queueManager;
        }

        [HttpPost]
        public void Post([FromBody] Location location)
        {
            queueManager.AddToQueue(location);
        }
    }
}