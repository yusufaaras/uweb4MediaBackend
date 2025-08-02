using uweb4Media.Application.Dtos;
using uweb4Media.Application.Features.CQRS.Results.Admin.Video;
using uweb4Media.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video
{
    public class GetVideoQueryHandler
    {
        private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;

        public GetVideoQueryHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository)
        {
            _repository = repository;
        }

        public async Task<List<GetVideoQueryResult>> Handle()
        {
            var values = await _repository.GetAllAsync();

            return values.Select(x => new GetVideoQueryResult
            {
                Id = x.Id,
                Link = x.Link,
                Title = x.Title,
                Description = x.Description,
                Thumbnail = x.Thumbnail,
                Sector = x.Sector,
                Channel = x.Channel,
                ContentType = x.ContentType,
                PublishStatus = x.PublishStatus, 
                IsPremium = x.IsPremium,
                LikesCount = x.LikesCount,
                CommentsCount = x.CommentsCount,
                UserId = x.UserId,
                Tags = x.Tags,
                Date = x.Date,
                Responsible = x.Responsible,
                CompanyId = x.CompanyId
            }).ToList();
        }
    }
}