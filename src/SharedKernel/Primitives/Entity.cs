namespace SharedKernel.Primitives;

public abstract record Entity : IEquatable<Entity>
{
    protected Entity(Guid id) => Id = id;

    protected Entity() { }

    public Guid Id { get; private init; }
}