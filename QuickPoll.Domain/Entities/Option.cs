using System.Security.AccessControl;

namespace QuickPoll.Domain.Entities;

public class Option : EntityBase
{
  public long PollId { get; set; }
  public string Text { get; set; }

  // for database seeding
  public Option()
  { }

  public Option(string text, long pollId)
  {
    Text = text;
    PollId = pollId;
  }
}