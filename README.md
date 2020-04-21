# Moogie.Queues

> THIS PACKAGE IS STILL UNDER ACTIVE DEVELOPMENT AND AS SUCH MAY NOT WORK, MAY NOT BE RELEASED OR MAY BE DELETED AT ANY 
TIME. DO NOT USE THIS PROJECT UNTIL THIS NOTICE IS REMOVED.

Moogie.Queues is a library of queue providers implemented under a common interface. It allows you to use different 
queue providers without compromising the integrity and future maintainability of your projects. 

## Installation

Install the applicable packages from the following list:

* `Moogie.Queues` - a package that contains the main functionality, interfaces and entities. You should install
this in your startup project and wherever you wish to inject an `IQueueManager` into your class. This package has
no other dependencies.
* `Moogie.Queues.DependencyInjection` - a package that contains an extension method that adds Moogie.Queues to your
`IServiceCollection` implementation instance. If you do not use dependency injection, then you should not install this
package.
* `Moogie.Queues.Providers.Memory` - include this whenever you want to use the `MemoryProvider` (usually when writing
integration tests) and wherever you instantiate a `QueueManager` instance. You do not need to reference this package
in projects where you are solely relying on the `IQueueProvider` interface.
* `Moogie.Queues.Providers.AmazonSQS` - include this whenever you want to use the `SQSProvider` and wherever you
instantiate a `QueueManager` instance. You do not need to reference this package in projects where you are solely relying 
on the `IQueueProvider` interface.
* `Moogie.Queues.Providers.AzureQueueStorage` - include this whenever you want to use the `AzureQueueStorage` and 
wherever you instantiate a `QueueManager` instance. You do not need to reference this package in projects where you are
solely relying on the `IQueueProvider` interface.

Then instantiate a `QueueManager` instance or add it to your `ServiceCollection` instance.

```csharp
serviceCollection.AddMoogieEvents(new QueueRegistration
{
    Name = "default",
    QueueProvider = new MemoryProvider()
});
```

## Adding multiple queue providers

Moogie.Queues gives you the ability to register multiple queue providers under one `QueueManager` out of the box.
You can do this by calling the `AddQueue` method on your `QueueManager` instance or by passing a number of `QueueRegistration`
instances into the `AddMoogieEvents` method.

By specifying a queue with the name of "default" you do not then need to specify the queue name when attempting to send
`Message` or `Receivable` instances to that queue provider.

## Dispatching messages to a queue

Dispatching messages to a queue provider is really easy. Create an instance of the `Dispatchable` class and pass it to 
the `QueueManager.Dispatch` method.

You have a choice of instantiating the `Dispatchable` instance yourself or using the fluent builder interface.

```csharp
// fluent
var dispatchable = Dispatchable.WithContent("abc")
    .OnQueue("a-queue")
    .WithId(Guid.NewGuid())
    .WhichExpiresIn(DateTime.Now.AddSeconds(5));
// or standard
dispatchable = new Dispatchable
{
    Queue = "a-queue",
    Id = Guid.NewGuid(),
    Content = "abc",
    Expiry = DateTime.Now.AddSeconds(5)
};

var response = await _queueManager.Dispatch(dispatchable);

Console.WriteLine(response.MessageId); // If you do not specify a message id then one will be created for you. You can retrieve that message id by inspecting the response from the Dispatch method.
```

## Reading messages from a queue

To receive messages from a queue you should call the `IQueueManager.Receive` method with an instance of a `Receivable`
object as the sole parameter. You can instantiate this instance yourself or use the fluent builder interface. There is 
no difference between the `Message` and `Messages` fluent builder extension methods other than making your code read 
easier.

Should you wish to long poll for messages (i.e. wait for X seconds until a message is received), you should configure
the `SecondsToWait` property if you are instantiating the class yourself or use the `ButWaitFor` extension if you are
using the fluent builder.

```csharp
var receivable = 1.Message().FromQueue("stuff");
receivable = 5.Messages().FromQueue("stuff").ButWaitFor(30);
receivable = new Receivable
{
    Queue = "stuff",
    MessagesToReceive = 5
};

var receiveResponse = await _queueManager.Receive(receivable);
foreach (var message in receiveResponse.Messages)
{
    // do whatever.
}
```

Reading messages from a queue **does not** remove them from that queue. To do so, you should call the `Delete` method
with the relevant parameters (see below).

## Deleting messages from a queue

To delete a message from a queue (also known as acknowledging within some queue providers) you should call the
`IQueueManager.Delete` method. You **should not** attempt to create an instance of the `Deletable` class yourself as it 
will not contain the required parameters for the provider. Instead, you should pass a `ReceivedMessage` or the 
`Deletable` property from a `ReceivedMessage` instance into the `Delete` method. 

```csharp
var receiveResponse = await _queueManager.Receive(1.Message());
foreach (var message in receiveResponse.Messages)
{
    await _queueManager.Delete(message);
}
```

## Provider Details

Some queue providers and their abstracted implementations within Moogie.Queues have particular quirks or implementation 
details that you should be aware of. 

### Amazon's Simple Queue Service Provider

The `SQSProvider` works in a similar way to Amazon's SQS client in that it lets you create an instance without specifying
any SQS client options or credentials. You should read up on Amazon's documentation as to what happens when you
leave these. The `Queue` property on the `SQSProviderOptions` class is **always** required and should be set to
whatever your SQS queue's URL is. Should you wish to use more than one queue URL then you should instantiate a
separate `SQSProvider` instance for each when you create your `QueueManager` instance.

When receiving messages, the number of messages you ask to return is a **maximum** number of messages. You might receive
less, but you will never receive more. For example, if only one message is available in the queue then one will be
returned. If 25 are available then your maximum will be returned.

The maximum number of messages you can return at once is 10. If you configure a number to return over this amount,
the `SQSProvider` will reset it back to 10.

### Azure's Queue Storage Provider

The `AzureQueueStorageProvider` requires that you specify a connection string and a queue name. There is no fallback
logic for this provider.

The `AzureQueueStorageProvider` does not currently support long polling. If you specify a `SecondsToWait` property on
a `Receivable` instance, then you will receive a `FeatureNotYetSupportedException`. You can stop this exception from
being thrown by setting the `IgnoreLongPollingException` property on the `AzureQueueStorageProviderOptions` class to
`true`.