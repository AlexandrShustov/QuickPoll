using MediatR;
using Microsoft.EntityFrameworkCore;
using QuickPoll.Application.Entities;
using QuickPoll.Application.Interfaces;
using QuickPoll.Application.Models;

namespace QuickPoll.Application.Polls.Queries;

public class GetRespondsQuery : IRequest<IResult>
{
  public string PollId { get; set; }
}

public class GetRespondsQueryHandler : IRequestHandler<GetRespondsQuery, IResult>
{
  private readonly IDbContext _dbContext;
  private readonly IObfuscationService _obfuscationService;

  public GetRespondsQueryHandler(IDbContext dbContext, IObfuscationService obfuscationService)
  {
    _dbContext = dbContext;
    _obfuscationService = obfuscationService;
  }

  public async Task<IResult> Handle(GetRespondsQuery request, CancellationToken cancellationToken)
  {
    var id = await _obfuscationService.DeObfuscate(request.PollId);

    var responds = await _dbContext
      .Responds
      .Where(x => x.PollId == id)
      .GroupBy(x => x.OptionId)
      .Select(x => new {OptionId = x.Key, Count = x.Count()})
      .ToListAsync(cancellationToken: cancellationToken);

    return responds switch
    {
      { Count: > 0 } x => new OkResult<RespondsModel>(new RespondsModel { RespondsByOption = x.ToDictionary(x => x.OptionId, y => y.Count) }),
      _ => new NotFoundResult<string>(request.PollId)
    };
  }
}