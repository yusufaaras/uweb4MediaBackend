using Uweb4Media.Domain.Entities;

namespace Uweb4Media.Domain.Repositories
{
    public interface IMediaContentRepository
    {
        Task AddAsync(MediaContent mediaContent);
        Task<List<MediaContent>> GetAllAsync();
    }
}