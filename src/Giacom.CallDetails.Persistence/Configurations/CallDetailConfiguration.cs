using Giacom.CallDetails.Domain.CallDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giacom.CallDetails.Persistence.Configurations;

public class CallDetailConfiguration : IEntityTypeConfiguration<CallDetail>
{
    public void Configure(EntityTypeBuilder<CallDetail> builder)
    {
        builder.HasKey(x => x.CallDetailId);
        builder.Property(x => x.CallDetailId).ValueGeneratedNever();
    }
}