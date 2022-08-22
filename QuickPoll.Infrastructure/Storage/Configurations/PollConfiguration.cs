using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickPoll.Domain.Entities;

namespace QuickPoll.Infrastructure.Storage.Configurations;

public class PollConfiguration: IEntityTypeConfiguration<Poll>
{
  public void Configure(EntityTypeBuilder<Poll> builder)
  {
    builder.HasKey(x => x.Id);

    builder.Property(x => x.Question).IsRequired().HasMaxLength(250);
    builder.Property(x => x.Description).HasMaxLength(1000);
    builder.HasMany(x => x.Options);
  }
}