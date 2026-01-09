using wx.Application.Common.Services;

namespace wx.Infrastructure.Services.Stock;

public class SinaProvider : IKLineProvider, IFactorProvider
{
    private readonly HttpClient _httpClient;
    public SinaProvider(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public string SourceName => "Sina";

    public Task<List<decimal>> GetFactorAsync(string symbol)
    {
        throw new NotImplementedException();
    }

    public Task<List<KLine>> GetKLinesAsync(DateTime startDate, DateTime endDate, KLinePeriod period, string? symbol = null)
    {
        throw new NotImplementedException();
    }
}
