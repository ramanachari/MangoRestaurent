namespace Mango.MessageBus
{
    public interface IMessageBus
    {
        Task PublishMessageAsync(BaseMessage message, string topicName, string connectionString);
    }
}