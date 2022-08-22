namespace QuickPoll.Application.Options;

public class HashIdOptions
{
  public static string Section { get; set; } = "HashId";

  public string Salt { get; set; }
  public short MinIdLength { get; set; }
}