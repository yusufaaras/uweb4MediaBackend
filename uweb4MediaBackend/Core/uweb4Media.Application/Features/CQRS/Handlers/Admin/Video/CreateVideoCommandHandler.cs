using uweb4Media.Application.Features.CQRS.Commands.Admin.Video;
using uweb4Media.Application.Features.CQRS.Commands.User;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;

public class CreateVideoCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;
    private IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> _companyRepository;  

    public CreateVideoCommandHandler(
        IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository,
        IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> companyRepository  
    )
    {
        _repository = repository;
        _companyRepository = companyRepository;
    }

    public async Task Handle(CreateVideoCommand command)
    {
        string responsibleName = command.Responsible ?? "";

        // Eğer CompanyId varsa, Company'nin adını responsible olarak ata
        if (command.CompanyId != null)
        {
            var company = await _companyRepository.GetByIdAsync(command.CompanyId.Value);
            if (company != null)
            {
                responsibleName = company.Name;
            }
        }

        var video = new Uweb4Media.Domain.Entities.Admin.Video.Video
        { 
            UserId = command.UserId,
            Link = command.Link,
            Title = command.Title ?? "",
            Description = command.Description ?? "",
            Thumbnail = command.Thumbnail ?? "",
            Sector = command.Sector ?? "",
            Channel = command.Channel ?? "",
            ContentType = command.ContentType ?? "",
            PublishStatus = command.PublishStatus ?? "Incelemede", 
            Tags = command.Tags ?? new List<string>(),
            IsPremium = command.IsPremium ?? false,
            Date = command.Date ?? DateTime.UtcNow,
            Responsible = responsibleName, 
            CompanyId = command.CompanyId,
            LikesCount = command.LikesCount ?? 0,
            CommentsCount = command.CommentsCount ?? 0, 
        }; 

        await _repository.CreateAsync(video); 
    }
}