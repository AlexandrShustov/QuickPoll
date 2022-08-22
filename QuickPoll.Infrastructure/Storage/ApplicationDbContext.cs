using System.Reflection;
using Microsoft.EntityFrameworkCore;
using QuickPoll.Application.Interfaces;
using QuickPoll.Domain.Entities;

namespace QuickPoll.Infrastructure.Storage;

public class ApplicationDbContext : DbContext, IDbContext
{
  public DbSet<Poll> Polls { get; set; }
  public DbSet<Option> Options { get; set; }
  public DbSet<Respond> Responds { get; set; }

  public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
  { }

  public Task<int> SaveChanges(CancellationToken token) => base.SaveChangesAsync(token);
  
  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    base.OnModelCreating(modelBuilder);
  }
}