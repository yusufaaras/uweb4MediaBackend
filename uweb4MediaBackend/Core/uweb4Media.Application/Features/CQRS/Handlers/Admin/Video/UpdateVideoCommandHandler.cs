using uweb4Media.Application.Features.CQRS.Commands.Admin.Video;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video
{
    public class UpdateVideoCommandHandler
    {
        private readonly IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;
        private readonly IRepository<VideoLocalizedString> _localizedStringRepository;

        public UpdateVideoCommandHandler(
            IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository,
            IRepository<VideoLocalizedString> localizedStringRepository)
        {
            _repository = repository;
            _localizedStringRepository = localizedStringRepository;
        }

        public async Task Handle(UpdateVideoCommand command)
        {
            var values = await _repository.GetByIdAsync(command.Id);

            if (values == null)
                throw new ApplicationException($"Video with ID {command.Id} not found.");

            // Null-coalescing assignment (if null, keep old value)
            values.Link          = command.Link          ?? values.Link;
            values.Thumbnail     = command.Thumbnail     ?? values.Thumbnail;
            values.Sector        = command.Sector        ?? values.Sector;
            values.Channel       = command.Channel       ?? values.Channel;
            values.ContentType   = command.ContentType   ?? values.ContentType;
            values.PublishStatus = command.PublishStatus ?? values.PublishStatus;
            values.Tags          = command.Tags          ?? values.Tags;
            values.Responsible   = command.Responsible   ?? values.Responsible; 

            if (command.LocalizedData != null)
            {
                var existingLocalizedStrings = values.LocalizedStrings.ToList();

                foreach (var incoming in command.LocalizedData)
                {
                    var existing = existingLocalizedStrings.FirstOrDefault(x => x.Id == incoming.Id);

                    if (existing != null)
                    {
                        existing.Title = incoming.Title;
                        existing.Description = incoming.Description;
                        await _localizedStringRepository.UpdateAsync(existing);
                        existingLocalizedStrings.Remove(existing);
                    }
                    else
                    {
                        var newLocalized = new VideoLocalizedString
                        {
                            VideoId = values.Id,
                            Title = incoming.Title,
                            Description = incoming.Description
                        };
                        values.LocalizedStrings.Add(newLocalized);
                        await _localizedStringRepository.CreateAsync(newLocalized);
                    }
                }
 
                foreach (var toRemove in existingLocalizedStrings)
                {
                    values.LocalizedStrings.Remove(toRemove);
                    await _localizedStringRepository.RemoveAsync(toRemove);
                }
            }

            await _repository.UpdateAsync(values);
        }
    }
}