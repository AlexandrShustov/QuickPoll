using MapsterMapper;
using MediatR;
using QuickPoll.Application.Entities;
using QuickPoll.Application.Interfaces;
using QuickPoll.Domain.Entities;

namespace QuickPoll.Application.Polls.Commands.CreatePoll;

public class CreatePollCommandHandler : IRequestHandler<CreatePollCommand, IResult>
{
  private readonly IDbContext _dbContext;
  private readonly IObfuscationService _obfuscationService;
  private readonly IMapper _mapper;

  public CreatePollCommandHandler(IDbContext dbContext, IObfuscationService obfuscationService, IMapper mapper)
  {
    _dbContext = dbContext;
    _obfuscationService = obfuscationService;
    _mapper = mapper;
  }

  public async Task<IResult> Handle(CreatePollCommand request, CancellationToken cancellationToken)
  {
    var poll = _mapper.From(request).AdaptToType<Poll>();

    _dbContext.Polls.Add(poll);

    await _dbContext.SaveChanges(cancellationToken);
    return new OkResult<string>(await _obfuscationService.Obfuscate(poll.Id));
  }
}