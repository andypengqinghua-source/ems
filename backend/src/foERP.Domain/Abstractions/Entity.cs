namespace foERP.Domain.Abstractions;

public abstract class Entity
{
    public Guid Id { get; protected set; } = Guid.NewGuid();
    public DateTime CreatedAtUtc { get; protected set; } = DateTime.UtcNow;
    public DateTime? ModifiedAtUtc { get; protected set; }

    public void MarkModified() => ModifiedAtUtc = DateTime.UtcNow;
}
