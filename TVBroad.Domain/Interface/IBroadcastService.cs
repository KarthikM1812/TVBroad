using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TVBroad.Domain.Entities;

namespace TVBroad.Domain.Interfaces.Broadcast
{
    public interface IBroadcastService
    {
        
        Task<List<Broadcasting>> GetAllAsync();
        Task<Broadcasting> GetByIdAsync(int id);
        //Task AddBroadcastAsync(Broadcasting schedule);
        Task UpdateBroadcastAsync(Broadcasting schedule);
        Task CreateBroadcastAsync(Broadcasting schedule);
        Task DeleteBroadcastAsync(int id);
        Task<bool> IsOverlappingAsync(DateTime startTime, DateTime endTime, int? ignoreId = null);
        Task<List<Broadcasting>> GetWindowBroadcastsAsync(DateTime from, DateTime to);
    }
}
