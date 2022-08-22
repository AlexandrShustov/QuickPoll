using QuickPoll.Domain.Enums;

namespace QuickPoll.Domain.Entities;

public class Poll : EntityBase
{
  public PollType Type { get; init; }

  public string Question { get; set; }
  public string? Description { get; set; }

  public List<Option> Options { get; set; }

  // for database seeding
  public Poll()
  {
    Options = new List<Option>();
  }

  public Poll(string question)
  {
    Question = question;
    Options = new List<Option>();
  }
}