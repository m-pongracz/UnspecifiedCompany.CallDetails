using System.Globalization;
using CsvHelper.Configuration;
using Giacom.CallDetails.Domain.CallDetails;
using Giacom.CallDetails.WebApi.Dtos;

namespace Giacom.CallDetails.CsvGenerator;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class CallDetailRecordCsvMap : ClassMap<CallDetailRecordDto>
{
    public CallDetailRecordCsvMap()
    {
        AutoMap(CultureInfo.InvariantCulture);
        Map(m => m.CallDate).TypeConverterOption.Format("dd/MM/yyyy");
        Map(m => m.CallType).TypeConverter<CallTypeEnumConverter<CallType>>();
    }
}