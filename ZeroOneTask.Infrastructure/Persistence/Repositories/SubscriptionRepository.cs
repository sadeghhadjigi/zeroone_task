using Microsoft.EntityFrameworkCore;
using ZeroOneTask.Application.Interfaces;
using ZeroOneTask.Domain.Entities;

namespace ZeroOneTask.Infrastructure.Persistence.Repositories
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        private readonly ZeroOneTaskDbContext _zeroOneDbContext;

        public SubscriptionRepository(ZeroOneTaskDbContext zeroOneTaskDbContext)
        {
            _zeroOneDbContext = zeroOneTaskDbContext;
        }

        public async Task<List<Subscription>> GetSubscriptionsByAgencyId(int agencyId)
        {
            return await _zeroOneDbContext.Subscriptions.Where(x => x.AgencyId == agencyId).AsNoTracking().ToListAsync();
        }
    }
}