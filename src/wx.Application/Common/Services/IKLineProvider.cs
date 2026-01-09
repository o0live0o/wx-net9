using System;
using System.Collections.Generic;
using System.Text;

namespace wx.Application.Common.Services;

public interface IKLineProvider
{
    string SourceName { get; }

    Task<List<KLine>> GetKLinesAsync(DateTime startDate, DateTime endDate, KLinePeriod period,string? symbol = null);
}

public record KLine(string Symbol,
                                DateTime Time,
                                decimal Open,
                                decimal High,
                                decimal Low,
                                decimal Close,
                                long Volume,
                                decimal? Amount = null);

public enum KLinePeriod
{
    Minute1,
    Minute5,
    Minute15,
    Minute30,
    Hour,
    Day,
    Week,
    Month
}
