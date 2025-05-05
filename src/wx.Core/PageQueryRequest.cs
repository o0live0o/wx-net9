﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Core;

public class PageQueryRequest
{
    private const int MaxPageSize = 100;
    private int _pageSize = 10;

    [Range(1, int.MaxValue, ErrorMessage = "页码必须大于0")]
    public int Page { get; set; } = 1;

    [Range(1, MaxPageSize)]
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public string SortBy { get; set; }
    public SortDirection SortDirection { get; set; } = SortDirection.Asc;

    public string Filters { get; set; }
}

public enum SortDirection
{
    Asc,
    Desc
}
