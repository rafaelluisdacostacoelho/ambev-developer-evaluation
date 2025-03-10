namespace Ambev.DeveloperEvaluation.Common.Messaging;

public interface IEventPublisher
{
    Task PublishEventAsync<T>(T domainEvent) where T : class;
}