using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace Giacom.CallDetails.CsvGenerator;

// ReSharper disable once ClassNeverInstantiated.Global
public class CallTypeEnumConverter<T> : DefaultTypeConverter  where T : struct
{
    public override string ConvertToString(object? value, IWriterRow row, MemberMapData memberMapData)
    {
        if (Enum.TryParse(value!.ToString(), out T result))
        {
            return Convert.ToInt32(result).ToString();
        }

        throw new InvalidCastException($"Invalid value to EnumConverter. Type: {typeof(T)} Value: {value}");
    }
}