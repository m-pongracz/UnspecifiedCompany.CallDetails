using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Giacom.CallDetails.Persistence.Converters;

// ReSharper disable once ClassNeverInstantiated.Global
internal class DateOnlyConverter : ValueConverter<DateOnly, DateTime>
{
    public DateOnlyConverter()
        : base(d => d.ToDateTime(TimeOnly.MinValue),
            d => DateOnly.FromDateTime(d))
    { }
}