namespace Ambev.DeveloperEvaluation.Common.Messaging;

public interface IProducer
{
    Task PublishAsync<T>(string queueName, T message);
}
