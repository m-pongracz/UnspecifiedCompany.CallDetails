using Giacom.CallDetails.Domain.CallDetails;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Giacom.CallDetails.Persistence.Configurations;

public class CallDetailConfiguration : IEntityTypeConfiguration<CallDetail>
{
    const int PhoneNumberMaxLength = 20;
    
    public void Configure(EntityTypeBuilder<CallDetail> builder)
    {
        builder.HasKey(x => x.CallDetailId);
        builder.Property(x => x.CallDetailId).ValueGeneratedNever();
        builder.Property(x => x.Cost).HasPrecision(3);
        builder.Property(x => x.CallDetailId).HasMaxLength(50);
        builder.Property(x => x.Currency).HasMaxLength(3);
        builder.Property(x => x.Recipient).HasMaxLength(PhoneNumberMaxLength);
        builder.Property(x => x.CallerId).HasMaxLength(PhoneNumberMaxLength);
        builder.Property(x => x.EndTime).HasMaxLength(8);
    }
}