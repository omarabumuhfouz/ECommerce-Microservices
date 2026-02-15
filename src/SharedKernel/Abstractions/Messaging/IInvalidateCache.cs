namespace SharedKernel.Abstractions.Messaging;

public interface IInvalidateCache
{
    string[] Tags { get; }
}