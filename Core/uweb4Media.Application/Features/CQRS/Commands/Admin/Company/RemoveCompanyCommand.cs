namespace uweb4Media.Application.Features.CQRS.Commands.Admin.Company;

public class RemoveCompanyCommand
{
    public int Id { get; set; }

    public RemoveCompanyCommand(int id)
    {
        Id = id;
    }
}