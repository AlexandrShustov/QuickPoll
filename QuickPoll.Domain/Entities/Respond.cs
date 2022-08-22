namespace QuickPoll.Domain.Entities;

public class Respond : EntityBase
{
  public long PollId { get; set; }
  public long OptionId { get; set; }
}