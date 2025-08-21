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

        AppUser? user = null;
        if (command.UserId.HasValue)
        {
            user = await _userRepository.GetByIdAsync(command.UserId.Value);
            if (user == null)
                throw new Exception("User not found.");

            // Kullanıcıda en az 3 token yoksa video oluşturulamaz!
            if (user.PostToken < 3)
                throw new Exception("You need at least 3 tokens to upload a video. Please purchase more tokens.");
        }

        // Video oluşturuluyor
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

        // Video oluşturulursa 3 token eksilt
        if (user != null)
        {
            user.PostToken -= 3;
            await _userRepository.UpdateAsync(user);
        }
    }
}