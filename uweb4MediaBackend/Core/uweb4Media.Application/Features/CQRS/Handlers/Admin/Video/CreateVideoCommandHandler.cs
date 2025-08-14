using uweb4Media.Application.Features.CQRS.Commands.Admin.Video;
using uweb4Media.Application.Features.CQRS.Commands.User;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;
using Uweb4Media.Domain.Entities.Admin.Video;

namespace uweb4Media.Application.Features.CQRS.Handlers.Admin.Video;

public class CreateVideoCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> _repository;
    private IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> _companyRepository;  
    private IRepository<AppUser> _userRepository;

    public CreateVideoCommandHandler(
        IRepository<Uweb4Media.Domain.Entities.Admin.Video.Video> repository,
        IRepository<Uweb4Media.Domain.Entities.Admin.CompanyManagement.Company> companyRepository,
        IRepository<AppUser> userRepository
    )
    {
        _repository = repository;
        _companyRepository = companyRepository;
        _userRepository = userRepository;
    }

    public async Task Handle(CreateVideoCommand command)
    {
        string responsibleName = command.Responsible ?? "";

        if (command.CompanyId != null)
        {
            var company = await _companyRepository.GetByIdAsync(command.CompanyId.Value);
            if (company != null)
            {
                responsibleName = company.Name;
            }
        }

        // --- Önce token kontrolü yap ---
        AppUser? user = null;
        if (command.UserId.HasValue)
        {
            user = await _userRepository.GetByIdAsync(command.UserId.Value);
            if (user == null)
                throw new Exception("Kullanıcı bulunamadı.");

            if (user.PostToken < 1)
                throw new Exception("Your token has expired. Please purchase more tokens to upload a new video.");
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
 
        if (user != null)
        {
            user.PostToken -= 1;
            await _userRepository.UpdateAsync(user);
        }
    }
}