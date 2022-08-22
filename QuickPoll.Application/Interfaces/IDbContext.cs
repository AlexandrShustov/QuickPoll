using Microsoft.EntityFrameworkCore;
using QuickPoll.Domain.Entities;

namespace QuickPoll.Application.Interfaces;

public interface IDbContext
{
  DbSet<Poll> Polls { get; }
  DbSet<Option> Options { get; }
  DbSet<Respond> Responds { get; } 

  Task<int> SaveChanges(CancellationToken token);
}