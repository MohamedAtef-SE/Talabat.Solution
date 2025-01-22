using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Talabat.Core.Application.Abstractions.Services;
using Talabat.Core.Domain.Entities._Common;

namespace Talabat.Infrastructure.Persistence.Data.Interceptors
{
    public class AuditableInterceptor : SaveChangesInterceptor
    {
        private readonly ILoggedInUserService _loggedInUserService;

        public AuditableInterceptor(ILoggedInUserService loggedInUserService)
        {
            _loggedInUserService = loggedInUserService;
        }

        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateAuditableEntites(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateAuditableEntites(eventData.Context);

            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void UpdateAuditableEntites(DbContext? dbContext)
        {
            if (dbContext is null) return;

            var userId = _loggedInUserService.UserId;

            DateTimeOffset date = DateTimeOffset.UtcNow;

            var entries = dbContext.ChangeTracker.Entries<IBaseAuditableEntity>()
                                                   .Where(entry => entry.State is EntityState.Added or EntityState.Modified);

            foreach (var entry in entries)
            {
                if (entry.State is EntityState.Added)
                {
                    entry.Entity.CreatedBy = userId ?? "N/A";
                    entry.Entity.CreatedOn = date;
                }
                entry.Entity.lastModifedBy = userId ?? "N/A";
                entry.Entity.lastModifedOn = date;
            }
        }
    }
}
