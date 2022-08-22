using Mapster;
using QuickPoll.Application.Interfaces;
using QuickPoll.Domain.Entities;
using QuickPoll.Domain.Enums;

namespace QuickPoll.Application.Models;

public class PollModel : BaseModel<PollModel, Poll>
{
  public string Id { get; set; }
  public PollType Type { get; set; }
  public string Question { get; set; }
  public string? Description { get; set; }
  public List<OptionModel> Options { get; set; }

  protected override void AddCustomRules()
  {
    RulesInverse()
      .Ignore(d => d.Id)
      .AfterMapping(async (s, d) =>
        d.Id = await MapContext.Current.GetService<IObfuscationService>().Obfuscate(s.Id) );
  }
}