using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TVBroad.Domain.Entities;
using TVBroad.Domain.Interfaces.Broadcast;

namespace Broadcast.DataService.Service
{
    public class BroadcastService : IBroadcastService
    {
        private readonly IBroadcastRepository _broadcastRepository;

        public BroadcastService(IBroadcastRepository broadcastRepository)
        {
            _broadcastRepository = broadcastRepository;
        }

        public async Task<List<Broadcasting>> GetAllAsync()
        {
            return await _broadcastRepository.GetAllBroadcastsAsync();
        }
        public async Task CreateBroadcastAsync(Broadcasting broadcasting)
        {
            broadcasting.Status = "Pending"; // default status
            await _broadcastRepository.AddBroadcastAsync(broadcasting);
        }

        public async Task<Broadcasting> GetByIdAsync(int id)
        {
            return await _broadcastRepository.GetBroadcastByIdAsync(id);
        }

        //public async Task AddBroadcastAsync((Broadcasting schedule)
        //{
        //    await _broadcastRepository.AddBroadcastAsync(schedule);
        //}

        public async Task UpdateBroadcastAsync(Broadcasting schedule)
        {
            await _broadcastRepository.UpdateBroadcastAsync(schedule);
        }

        public async Task DeleteBroadcastAsync(int id)
        {
            await _broadcastRepository.DeleteBroadcastAsync(id);
        }

        public async Task<bool> IsOverlappingAsync(DateTime startTime, DateTime endTime, int? ignoreId = null)
        {
            return await _broadcastRepository.HasOverlapAsync(startTime, endTime, ignoreId);
        }

        public async Task<List<Broadcasting>> GetWindowBroadcastsAsync(DateTime from, DateTime to)
        {
            return await _broadcastRepository.GetApprovedBroadcastsInWindowAsync(from, to);
        }
    }
}
