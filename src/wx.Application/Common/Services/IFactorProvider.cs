namespace wx.Application.Common.Services;

public interface IFactorProvider
{
    Task<List<decimal>> GetFactorAsync(string symbol);
}
