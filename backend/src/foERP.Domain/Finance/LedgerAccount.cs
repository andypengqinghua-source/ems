using foERP.Domain.Abstractions;

namespace foERP.Domain.Finance;

public sealed class LedgerAccount : Entity
{
    public required string AccountNumber { get; init; }
    public required string Name { get; init; }
    public bool IsPostingAllowed { get; private set; } = true;

    public void LockPosting()
    {
        IsPostingAllowed = false;
        MarkModified();
    }
}
