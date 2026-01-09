using System;
using System.Collections.Generic;
using System.Text;

namespace wx.Application.Common.Services;

public interface ICurrentUserService
{
    string? UserId { get; }
    string? UserName { get; }
    bool IsAuthenticated { get; }
}
