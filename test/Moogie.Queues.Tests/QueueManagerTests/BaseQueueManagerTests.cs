namespace Moogie.Queues.Tests
{
    public abstract class BaseQueueManagerTest
    {
        protected IQueueManager QueueManager { get; }
        protected One ProviderOne { get; }
        protected Two ProviderTwo { get;  }

        protected BaseQueueManagerTest()
        {
            ProviderOne = new One();
            ProviderTwo = new Two();
            QueueManager = new QueueManager();
        }

        protected class One : FakeProvider
        {
        }

        protected class Two : FakeProvider
        {
        }
    }
}
