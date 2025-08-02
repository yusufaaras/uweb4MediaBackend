using uweb4Media.Application.Features.CQRS.Commands.Admin.Video;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video
{
    public class UpdateVideoCommandHandler
    {
        private IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;

        public UpdateVideoCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateVideoCommand command)
        {
            var values = await _repository.GetByIdAsync(command.Id);

            if (values == null)
                throw new ApplicationException($"Video with ID {command.Id} not found.");

            // Null-coalescing assignment (if null, keep old value)
            values.Link          = command.Link          ?? values.Link;
            values.Thumbnail     = command.Thumbnail     ?? values.Thumbnail;
            values.Title         = command.Title         ?? values.Title;
            values.Description   = command.Description   ?? values.Description;
            values.Sector        = command.Sector        ?? values.Sector;
            values.Channel       = command.Channel       ?? values.Channel;
            values.ContentType   = command.ContentType   ?? values.ContentType;
            values.PublishStatus = command.PublishStatus ?? values.PublishStatus;
            values.Tags          = command.Tags          ?? values.Tags;
            values.Responsible   = command.Responsible   ?? values.Responsible;  

            await _repository.UpdateAsync(values);
        }
    }
}