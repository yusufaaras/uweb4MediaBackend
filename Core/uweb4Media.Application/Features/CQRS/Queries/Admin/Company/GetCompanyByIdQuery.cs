namespace uweb4Media.Application.Features.CQRS.Queries.Admin.Company;

public class GetCompanyByIdQuery
{
    public GetCompanyByIdQuery(int id)
    {
        Id = id;
    }
    public int Id { get; set; }
}