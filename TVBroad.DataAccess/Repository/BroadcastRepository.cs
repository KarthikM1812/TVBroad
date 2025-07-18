using Microsoft.EntityFrameworkCore;
using TVBroad.DataAccess.Data;
using TVBroad.Domain.Entities;
using TVBroad.Domain.Interfaces.Broadcast;

namespace Broadcast.DataAccess.Repository
{
    public class BroadcastRepository : IBroadcastRepository
    {
        private readonly AppDbContext _context;

        public BroadcastRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Broadcasting>> GetAllBroadcastsAsync()
        {
            return await _context.Broadcast
                                 .OrderBy(b => b.StartTime)
                                 .ToListAsync();
        }
        public async Task AddBroadcastAsync(Broadcasting broadcasting)
        {
            _context.Broadcast.Add(broadcasting); // or whatever your DbSet is named
            await _context.SaveChangesAsync();
        }

        public async Task<Broadcasting> GetBroadcastByIdAsync(int id)
        {
            return await _context.Broadcast.FindAsync(id);
        }

       
        public async Task UpdateBroadcastAsync(Broadcasting broadcast)
        {
            _context.Broadcast.Update(broadcast);
            broadcast.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteBroadcastAsync(int id)
        {
            var item = await _context.Broadcast.FindAsync(id);
            if (item != null)
            {
                _context.Broadcast.Remove(item);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> HasOverlapAsync(DateTime startTime, DateTime endTime, int? ignoreId = null)
        {
            return await _context.Broadcast
                .AnyAsync(b =>
                    b.Status == "Approved" &&
                    (ignoreId == null || b.Id != ignoreId) &&
                    b.StartTime < endTime &&
                    b.EndTime > startTime);
        }

        public async Task<List<Broadcasting>> GetApprovedBroadcastsInWindowAsync(DateTime from, DateTime to)
        {
            return await _context.Broadcast
                .Where(b => b.Status == "Approved" && b.StartTime >= from && b.EndTime <= to)
                .OrderBy(b => b.StartTime)
                .ToListAsync();
        }
    }
}
