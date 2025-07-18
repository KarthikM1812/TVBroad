using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TVBroad.Domain.Entities;

namespace TVBroad.Domain.Interfaces.Broadcast
{
    public interface IBroadcastRepository
    {
        Task<List<Broadcasting>> GetAllBroadcastsAsync();
        Task<Broadcasting> GetBroadcastByIdAsync(int id);
        Task AddBroadcastAsync(Broadcasting broadcast);
        Task UpdateBroadcastAsync(Broadcasting broadcast);
         
        Task DeleteBroadcastAsync(int id);
        Task<bool> HasOverlapAsync(DateTime startTime, DateTime endTime, int? ignoreId = null);
        Task<List<Broadcasting>> GetApprovedBroadcastsInWindowAsync(DateTime from, DateTime to);
    }
}
