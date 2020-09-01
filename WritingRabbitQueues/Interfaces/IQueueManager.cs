using WritingRabbitQueues.Models;

namespace WritingRabbitQueues.Interfaces
{
    public interface IQueueManager
    {
        void AddToQueue(Location location);
    }
}