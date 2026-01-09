namespace wx.Application.Common.Models;
public class PaginationRequest
{
    private int _pageSize = 10;
    private int _pageIndex = 1;

    public int PageIndex
    {
        get => _pageIndex;
        set => _pageIndex = value < 1 ? 1 : value;
    }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value < 1 ? 10 : (value > 100 ? 100 : value);
    }

    public string? OrderBy { get; set; }
    public bool Descending { get; set; }
    public string? SearchTerm { get; set; }
}
