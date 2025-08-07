using AutoMapper;

namespace CryptoMonitoring.Common.Mapper.Converters;

public class NullableStringToNullableDecimalConverter : IValueConverter<string?, decimal?>
{
    public decimal? Convert(string? sourceMember, ResolutionContext context)
    {
        return decimal.TryParse(sourceMember, out var val) ? val : null;
    }
}