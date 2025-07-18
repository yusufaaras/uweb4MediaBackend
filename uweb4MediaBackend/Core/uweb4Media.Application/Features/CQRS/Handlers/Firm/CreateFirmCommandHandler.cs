using uweb4Media.Application.Features.CQRS.Commands.Firm;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Firm;

public class CreateFirmCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Firm> _repository;

    public CreateFirmCommandHandler(IRepository<Uweb4Media.Domain.Entities.Firm> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateFirmCommand command)
    {
        await _repository.CreateAsync(new Uweb4Media.Domain.Entities.Firm
        {
            FirmName = command.FirmName,
            LogoUrl = command.LogoUrl,
            WebSiteUrl = command.WebSiteUrl,
            Sector = command.Sector,
            Country = command.Country,
            AuthorizedPerson = command.AuthorizedPerson,
            AuthorizedPersonEmail = command.AuthorizedPersonEmail,
            UserId = command.UserId,
            status = false 
        });

    }
}