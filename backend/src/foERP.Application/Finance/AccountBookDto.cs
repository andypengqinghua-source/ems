namespace foERP.Application.Finance;

public sealed record AccountBookDto(
    Guid Id,
    string BookCode,
    string BookName,
    string BaseCurrency,
    bool IsActive);
