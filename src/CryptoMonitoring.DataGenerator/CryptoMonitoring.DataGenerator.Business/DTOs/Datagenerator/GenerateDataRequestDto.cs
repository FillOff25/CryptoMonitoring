namespace CryptoMonitoring.DataGenerator.Business.DTOs.DataGenerator;

public class GenerateDataRequestDto
{
    public string Name { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public int Count { get; set; }
}
