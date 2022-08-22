using MediatR;
using QuickPoll.Application.Entities;

namespace QuickPoll.Application.Polls.Queries.GetResponds;

public class GetRespondsQuery : IRequest<IResult>
{
  public string PollId { get; set; }
}