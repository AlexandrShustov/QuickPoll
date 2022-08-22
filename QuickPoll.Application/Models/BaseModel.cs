using Mapster;

namespace QuickPoll.Application.Models;

public class BaseModel<TSource, TDestination> : IRegister
{
  protected TypeAdapterConfig Config { get; set; }
  protected virtual void AddCustomRules() { }

  protected TypeAdapterSetter<TSource, TDestination> Rules() => 
    Config.ForType<TSource, TDestination>();

  protected TypeAdapterSetter<TDestination, TSource> RulesInverse() =>
    Config.ForType<TDestination, TSource>();

  public void Register(TypeAdapterConfig config)
  {
    Config = config;
    AddCustomRules();
  }
}