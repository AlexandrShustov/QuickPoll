using MediatR;
using QuickPoll.Application.Entities;
using QuickPoll.Application.Models;

namespace QuickPoll.Application.Polls.Commands.CreatePoll;

public class CreatePollCommand : PollModel, IRequest<IResult>
{ }