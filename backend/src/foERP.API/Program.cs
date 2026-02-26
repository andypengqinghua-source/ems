using foERP.Application;
using foERP.Application.Finance;
using foERP.Domain.Finance;
using foERP.Infrastructure;
using foERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok", name = "foERP API" }));

app.MapGet("/api/modules", () => Results.Ok(new[]
{
    "Finance",
    "SupplyChain",
    "Procurement",
    "Sales",
    "HumanCapital",
    "ProjectOperations"
}));

app.MapGet("/api/finance/account-books", async (FoErpDbContext dbContext, CancellationToken cancellationToken) =>
{
    var books = await dbContext.AccountBooks
        .OrderBy(x => x.BookCode)
        .Select(x => new AccountBookDto(x.Id, x.BookCode, x.BookName, x.BaseCurrency, x.IsActive))
        .ToListAsync(cancellationToken);

    return Results.Ok(books);
});

app.MapPost("/api/finance/account-books", async (CreateAccountBookRequest request, FoErpDbContext dbContext, CancellationToken cancellationToken) =>
{
    var exists = await dbContext.AccountBooks.AnyAsync(x => x.BookCode == request.BookCode, cancellationToken);
    if (exists)
    {
        return Results.Conflict(new { message = $"账套编码 {request.BookCode} 已存在" });
    }

    var accountBook = new AccountBook
    {
        BookCode = request.BookCode,
        BookName = request.BookName,
        BaseCurrency = request.BaseCurrency
    };

    dbContext.AccountBooks.Add(accountBook);
    await dbContext.SaveChangesAsync(cancellationToken);

    return Results.Created($"/api/finance/account-books/{accountBook.Id}",
        new AccountBookDto(accountBook.Id, accountBook.BookCode, accountBook.BookName, accountBook.BaseCurrency, accountBook.IsActive));
});

app.MapPost("/api/finance/account-books/{bookId:guid}/ledger-accounts", async (Guid bookId, CreateLedgerAccountRequest request, FoErpDbContext dbContext, CancellationToken cancellationToken) =>
{
    var hasBook = await dbContext.AccountBooks.AnyAsync(x => x.Id == bookId && x.IsActive, cancellationToken);
    if (!hasBook)
    {
        return Results.NotFound(new { message = "账套不存在或已停用" });
    }

    var exists = await dbContext.LedgerAccounts
        .AnyAsync(x => x.AccountBookId == bookId && x.AccountNumber == request.AccountNumber, cancellationToken);

    if (exists)
    {
        return Results.Conflict(new { message = $"账套内科目 {request.AccountNumber} 已存在" });
    }

    var account = new LedgerAccount
    {
        AccountNumber = request.AccountNumber,
        Name = request.Name
    }.AssignToBook(bookId);

    dbContext.LedgerAccounts.Add(account);
    await dbContext.SaveChangesAsync(cancellationToken);

    return Results.Created($"/api/finance/account-books/{bookId}/ledger-accounts/{account.Id}", new
    {
        account.Id,
        account.AccountBookId,
        account.AccountNumber,
        account.Name,
        account.IsPostingAllowed
    });
});

var userDefaults = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

app.MapGet("/api/users/{userId}/personal-settings", async (string userId, FoErpDbContext dbContext, CancellationToken cancellationToken) =>
{
    var activeBooks = await dbContext.AccountBooks
        .Where(x => x.IsActive)
        .OrderBy(x => x.BookCode)
        .Select(x => x.BookCode)
        .ToListAsync(cancellationToken);

    var defaultBookCode = userDefaults.TryGetValue(userId, out var userBookCode)
        ? userBookCode
        : activeBooks.FirstOrDefault();

    return Results.Ok(new UserPersonalSettingsResponse(userId, defaultBookCode, activeBooks));
});

app.MapPut("/api/users/{userId}/personal-settings/default-account-book", async (string userId, UpdateDefaultAccountBookRequest request, FoErpDbContext dbContext, CancellationToken cancellationToken) =>
{
    var hasBook = await dbContext.AccountBooks
        .AnyAsync(x => x.BookCode == request.DefaultAccountBookCode && x.IsActive, cancellationToken);

    if (!hasBook)
    {
        return Results.BadRequest(new { message = "默认账套必须是已启用账套" });
    }

    userDefaults[userId] = request.DefaultAccountBookCode;

    return Results.Ok(new UserPersonalSettingsResponse(userId, request.DefaultAccountBookCode, new[] { request.DefaultAccountBookCode }));
});

app.Run();

public sealed record CreateAccountBookRequest(string BookCode, string BookName, string BaseCurrency);
public sealed record CreateLedgerAccountRequest(string AccountNumber, string Name);
public sealed record UpdateDefaultAccountBookRequest(string DefaultAccountBookCode);
public sealed record UserPersonalSettingsResponse(string UserId, string? DefaultAccountBookCode, IEnumerable<string> AvailableAccountBookCodes);
