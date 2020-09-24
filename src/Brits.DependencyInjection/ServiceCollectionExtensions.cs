using Microsoft.Extensions.DependencyInjection;

namespace Brits
{
    /// <summary>
    /// Dependency injection extensions which adds Brits to the relevant <see cref="IServiceCollection"/>
    /// implementation instance.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Brits to the specified <see cref="IServiceCollection"/> implementation instance.
        /// </summary>
        /// <param name="serviceCollection">The service collection to add Brits to.</param>
        /// <param name="queueRegistrations">
        /// An array of <see cref="QueueRegistration"/> entities to register as queues.
        /// </param>
        /// <returns>The modified <see cref="IServiceCollection"/> implementation instance.</returns>
        public static IServiceCollection AddBrits(
            this IServiceCollection serviceCollection,
            params QueueRegistration[] queueRegistrations
        )
        {
            var queueManager = new QueueManager();

            foreach (var queueRegistration in queueRegistrations)
                queueManager.AddQueue(queueRegistration.Name, queueRegistration.QueueProvider);

            serviceCollection.AddSingleton<IQueueManager>(queueManager);
            return serviceCollection;
        }
    }

    /// <summary>
    /// Class which represents a queue to register in the <see cref="IQueueManager"/> implementation instance.
    /// </summary>
    public class QueueRegistration
    {
        /// <summary>
        /// Gets or sets the name of the queue to be registered.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the provider of the queue to be registered.
        /// </summary>
        public IQueueProvider QueueProvider { get; set; }
    }
}
