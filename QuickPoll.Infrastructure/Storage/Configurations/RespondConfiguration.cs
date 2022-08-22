using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QuickPoll.Domain.Entities;

namespace QuickPoll.Infrastructure.Storage.Configurations;

public class RespondConfiguration : IEntityTypeConfiguration<Respond>
{
  public void Configure(EntityTypeBuilder<Respond> builder)
  {
    builder.HasKey(x => x.Id);
  }
}