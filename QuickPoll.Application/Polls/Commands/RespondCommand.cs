using Mapster;
using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickPoll.Application.Entities;
using QuickPoll.Application.Interfaces;
using QuickPoll.Application.Models;
using QuickPoll.Domain.Entities;

namespace QuickPoll.Application.Polls.Commands;

public class RespondCommand : RespondModel, IRequest<IResult>
{
}

public class RespondCommandHandler : IRequestHandler<RespondCommand, IResult>
{
  private readonly IDbContext _dbContext;
  private readonly IObfuscationService _obfuscationService;
  private readonly IMapper _mapper;

  public RespondCommandHandler(IDbContext dbContext, IObfuscationService obfuscationService, IMapper mapper)
  {
    _dbContext = dbContext;
    _obfuscationService = obfuscationService;
    _mapper = mapper;
  }

  public async Task<IResult> Handle(RespondCommand request, CancellationToken cancellationToken)
  {
    var option = await _dbContext.Options
      .FirstOrDefaultAsync(x => x.Id == request.OptionId, cancellationToken);

    var respond = _mapper.From(request).AdaptToType<Respond>();

    if (option is null || option.PollId != respond.PollId)
      return new InvalidOptionResult(request.PollId);

    _dbContext.Responds.Add(respond);

    await _dbContext.SaveChanges(cancellationToken);
    return new OkResult();
  }
}