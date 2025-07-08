using Microsoft.EntityFrameworkCore;
using Uweb4Media.Domain.Entities;
using Uweb4Media.Domain.Repositories;

namespace Uweb4Media.Persistence.Repositories
{
    public class MediaContentRepository : IMediaContentRepository
    {
        private readonly AppDbContext _dbContext;

        public MediaContentRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(MediaContent mediaContent)
        {
            _dbContext.MediaContents.Add(mediaContent);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<MediaContent>> GetAllAsync()
        {
            return await _dbContext.MediaContents
                .Include(m => m.User)
                .ToListAsync();
        }
    }
}