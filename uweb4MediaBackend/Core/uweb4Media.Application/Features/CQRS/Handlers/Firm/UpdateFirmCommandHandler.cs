using uweb4Media.Application.Features.CQRS.Commands.Firm;
using uweb4Media.Application.Interfaces;

namespace uweb4Media.Application.Features.CQRS.Handlers.Firm;

public class UpdateFirmCommandHandler
{
    private IRepository<Uweb4Media.Domain.Entities.Firm> _repository;

    public UpdateFirmCommandHandler(IRepository<Uweb4Media.Domain.Entities.Firm> repository)
    {
        _repository = repository;
    }
    public async Task Handle(UpdateFirmCommand command)
    {
        var values = await _repository.GetByIdAsync(command.Id);
        values.FirmName = command.FirmName;
        values.WebSiteUrl = command.WebSiteUrl;
        values.LogoUrl = command.LogoUrl;
        values.Sector = command.Sector;
        values.AuthorizedPersonEmail = command.AuthorizedPersonEmail;
        values.AuthorizedPerson=command.AuthorizedPerson;
        values.Country = command.Country;
        await _repository.UpdateAsync(values);
    }
}