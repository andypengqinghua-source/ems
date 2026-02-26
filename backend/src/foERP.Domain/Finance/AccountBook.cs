using foERP.Domain.Abstractions;

namespace foERP.Domain.Finance;

public sealed class AccountBook : Entity
{
    public required string BookCode { get; init; }
    public required string BookName { get; private set; }
    public required string BaseCurrency { get; private set; }
    public bool IsActive { get; private set; } = true;

    public void Rename(string bookName)
    {
        BookName = bookName;
        MarkModified();
    }

    public void ChangeBaseCurrency(string currency)
    {
        BaseCurrency = currency;
        MarkModified();
    }

    public void Disable()
    {
        IsActive = false;
        MarkModified();
    }
}
