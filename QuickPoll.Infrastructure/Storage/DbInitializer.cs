using Bogus;
using QuickPoll.Application.Interfaces;
using QuickPoll.Domain.Entities;

namespace QuickPoll.Infrastructure.Storage;

public class DbInitializer : IDbInitializer
{
  private readonly IDbContext _dbContext;
  private readonly Faker<Poll> _polls;
  private readonly Faker<Option> _answers;

  public DbInitializer(IDbContext dbContext)
  {
    _dbContext = dbContext;
    _polls = new Faker<Poll>()
      .RuleFor(x => x.Question, x => x.Lorem.Sentence())
      .RuleFor(x => x.Description, x => x.Lorem.Sentences(3));

    _answers = new Faker<Option>()
      .RuleFor(x => x.Text, x => x.Lorem.Word());
  }

  public async Task Seed(CancellationToken token)
  {
    try
    {
      var polls = _polls.Generate(10);

      foreach (var poll in polls)
      {
        var answers = _answers.Generate(3);
        poll.Options = new List<Option>();
        poll.Options.AddRange(answers);
      }

      _dbContext.Polls.AddRange(polls);
      await _dbContext.SaveChanges(token);
    }
    catch (Exception e)
    {
      Console.WriteLine(e);
      throw;
    }
  }
}