using ZeroOneTask.Domain.Entities;

namespace ZeroOneTask.Application.Interfaces
{
    public interface ISubscriptionRepository
    {
        Task<List<Subscription>> GetSubscriptionsByAgencyId(int agencyId);
    }
}