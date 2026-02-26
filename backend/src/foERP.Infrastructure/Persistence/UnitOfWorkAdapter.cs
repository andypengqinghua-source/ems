using foERP.Application.Abstractions;

namespace foERP.Infrastructure.Persistence;

public static class UnitOfWorkAdapter
{
    public static Task<int> SaveChangesAsync(this IUnitOfWork unitOfWork, CancellationToken cancellationToken = default)
        => unitOfWork.SaveChangesAsync(cancellationToken);
}
