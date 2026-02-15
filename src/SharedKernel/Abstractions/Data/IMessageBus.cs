namespace SharedKernel.Abstractions;

public interface IMessageBus
{
    /// <summary>
    /// Publishes a message to a specific exchange.
    /// </summary>
    /// <param name="exchangeName">The name of the fanout exchange.</param>
    /// <param name="messageBody">The message, serialized to a byte array.</param>
    void Publish(string exchangeName, byte[] messageBody);
}