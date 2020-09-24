namespace Brits.Tests
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
            public override string ProviderName { get; } = "ProviderOne";
        }

        protected class Two : FakeProvider
        {
            public override string ProviderName { get; } = "ProviderTwo";
        }
    }
}
