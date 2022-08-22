using MapsterMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickPoll.Application.Entities;
using QuickPoll.Application.Interfaces;
using QuickPoll.Application.Models;

namespace QuickPoll.Application.Polls.Queries;

public class GetPollQuery : IRequest<IResult>
{
  public string PollId { get; set; }
}

public class GetPollQueryHandler : IRequestHandler<GetPollQuery, IResult>
{
  private readonly IDbContext _dbContext;
  private readonly IObfuscationService _obfuscationService;
  private readonly IMapper _mapper;

  public GetPollQueryHandler(IDbContext dbContext, IObfuscationService obfuscationService, IMapper mapper)
  {
    _dbContext = dbContext;
    _obfuscationService = obfuscationService;
    _mapper = mapper;
  }

  public async Task<IResult> Handle(GetPollQuery request, CancellationToken cancellationToken)
  {
    var id = await _obfuscationService.DeObfuscate(request.PollId);

    var poll = await _dbContext
      .Polls
      .Include(x => x.Options)
      .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    
    return poll is not null
      ? new OkResult<PollModel>(_mapper.From(poll).AdaptToType<PollModel>())
      : new NotFoundResult<string>(request.PollId);
  }
}