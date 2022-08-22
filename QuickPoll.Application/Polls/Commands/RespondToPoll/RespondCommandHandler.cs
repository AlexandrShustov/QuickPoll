using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickPoll.Application.Entities;
using QuickPoll.Application.Interfaces;
using QuickPoll.Domain.Entities;

namespace QuickPoll.Application.Polls.Commands.RespondToPoll;

public class RespondCommandHandler : IRequestHandler<RespondCommand, IResult>
{
  private readonly IDbContext _dbContext;
  private readonly IMapper _mapper;

  public RespondCommandHandler(IDbContext dbContext, IMapper mapper)
  {
    _dbContext = dbContext;
    _mapper = mapper;
  }

  public async Task<IResult> Handle(RespondCommand request, CancellationToken cancellationToken)
  {
    var option = await _dbContext
      .Options
      .FirstOrDefaultAsync(x => x.Id == request.OptionId, cancellationToken);

    if (option is null)
      return new NotFoundResult()
        .WithMessage($"There is no option with id {request.OptionId}.");

    var respond = _mapper.From(request).AdaptToType<Respond>();
    if (option.PollId != respond.PollId)
      return new ErrorResult()
        .WithMessage($"The poll doesn't contain an option with id {request.OptionId}");

    _dbContext.Responds.Add(respond);

    await _dbContext.SaveChanges(cancellationToken);
    return new OkResult();
  }
}