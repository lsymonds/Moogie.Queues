using Microsoft.Extensions.DependencyInjection;
using Moogie.Queues.Providers.Memory;

namespace Moogie.Queues.Tests.Providers.Memory
{
    public abstract class BaseMemoryProviderTests
    {
        protected MemoryProvider QueueProvider { get; }
        protected IQueueManager QueueManager { get; }

        protected BaseMemoryProviderTests()
        {
            QueueProvider = new MemoryProvider();

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddMoogieQueues(new QueueRegistration
            {
                Name = "default",
                QueueProvider = QueueProvider
            });
            var serviceProvider = serviceCollection.BuildServiceProvider();

            QueueManager = serviceProvider.GetService<IQueueManager>();
        }
    }
}
