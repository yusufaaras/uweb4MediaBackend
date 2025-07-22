using uweb4Media.Application.Features.CQRS.Commands.Admin.Video;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;

public class UpdateVideoCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;
    private readonly IRepository<VideoLocalizedString> _localizedStringRepository;
    public UpdateVideoCommandHandler(IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository)
    {
        _repository = repository;
    }
    public async Task Handle(UpdateVideoCommand command)
    {var values = await _repository.GetByIdAsync(command.Id); 

        if (values == null)
        {
            throw new ApplicationException($"Video with ID {command.Id} not found.");
        }

        values.Link = command.Link ?? values.Link; 
        values.Thumbnail = command.Thumbnail ?? values.Thumbnail;
        values.Sector = command.Sector ?? values.Sector;
        values.Channel = command.Channel ?? values.Channel;
        values.ContentType = command.ContentType ?? values.ContentType;
        values.PublishStatus = command.PublishStatus ?? values.PublishStatus;
        values.PublishDate = command.PublishDate ?? values.PublishDate;
        values.Tags = command.Tags ?? values.Tags; 
        values.Date = command.Date ?? values.Date; 
        values.Responsible = command.Responsible ?? values.Responsible;
        values.CompanyId = command.CompanyId ?? values.CompanyId; 
        if (command.LocalizedData != null)
        {
            var existingLocalizedStrings = values.LocalizedStrings.ToList(); 
            foreach (var incomingLocalizedDto in command.LocalizedData)
            {
                var existingString = existingLocalizedStrings.FirstOrDefault(
                    ls => ls.LanguageCode == incomingLocalizedDto.LanguageCode);

                if (existingString != null)
                {
                    // Mevcutsa güncelle
                    existingString.Title = incomingLocalizedDto.Title;
                    existingString.Description = incomingLocalizedDto.Description;
                    _localizedStringRepository.UpdateAsync(existingString);  
                }
                else
                {
                    // Yoksa yeni ekle
                    var newLocalizedString = new VideoLocalizedString
                    {
                        Id = Guid.NewGuid(),
                        VideoId = values.Id,
                        LanguageCode = incomingLocalizedDto.LanguageCode,
                        Title = incomingLocalizedDto.Title,
                        Description = incomingLocalizedDto.Description
                    };
                    values.LocalizedStrings.Add(newLocalizedString);  
                    _localizedStringRepository.CreateAsync(newLocalizedString);  
                }
            } 
            var languageCodesInCommand = command.LocalizedData.Select(dto => dto.LanguageCode).ToList();
            var stringsToRemove = existingLocalizedStrings
                .Where(ls => !languageCodesInCommand.Contains(ls.LanguageCode))
                .ToList();

            foreach (var stringToRemove in stringsToRemove)
            {
                values.LocalizedStrings.Remove(stringToRemove);  
                _localizedStringRepository.RemoveAsync(stringToRemove);  
            }
        } 

        await _repository.UpdateAsync(values);
    }
}