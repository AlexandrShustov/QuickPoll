using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickPoll.Domain.Entities;

namespace QuickPoll.Infrastructure.Storage.Configurations;

public class OptionConfiguration : IEntityTypeConfiguration<Option>
{
  public void Configure(EntityTypeBuilder<Option> builder)
  {
    builder.HasKey(x => x.Id);

    builder.Property(x => x.Text).IsRequired().HasMaxLength(250);
    builder.HasIndex(x => x.PollId);
  }
}