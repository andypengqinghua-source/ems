using foERP.Domain.Abstractions;

namespace foERP.Domain.Finance;

public sealed class LedgerAccount : Entity
{
    public Guid AccountBookId { get; private set; }
    public required string AccountNumber { get; init; }
    public required string Name { get; init; }
    public bool IsPostingAllowed { get; private set; } = true;

    public LedgerAccount AssignToBook(Guid accountBookId)
    {
        AccountBookId = accountBookId;
        MarkModified();
        return this;
    }

    public void LockPosting()
    {
        IsPostingAllowed = false;
        MarkModified();
    }
}
