using MediatR;
using QuickPoll.Application.Entities;
using QuickPoll.Application.Models;

namespace QuickPoll.Application.Polls.Commands.RespondToPoll;

public class RespondCommand : RespondModel, IRequest<IResult>
{
}