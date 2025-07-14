
using uweb4Media.Application.Features.CQRS.Commands.Like;
using uweb4Media.Application.Interfaces;
using Uweb4Media.Domain.Entities;

public class CreateLikeCommandHandler
{
    
    private IRepository<Like> _repository;

    public CreateLikeCommandHandler(IRepository<Like> repository)
    {
        _repository = repository;
    }
    public async Task Handle(CreateLikeCommand command)
    {
        await _repository.CreateAsync(new Like
        { 
            UserId = command.UserId,
            MediaContentId = command.MediaContentId,
            LikeDate = command.LikeDate,
        });
    }
}