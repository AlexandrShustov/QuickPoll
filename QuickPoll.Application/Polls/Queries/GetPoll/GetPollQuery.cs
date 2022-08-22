using MediatR;
using QuickPoll.Application.Entities;

namespace QuickPoll.Application.Polls.Queries.GetPoll;

public class GetPollQuery : IRequest<IResult>
{
  public string PollId { get; set; }
}