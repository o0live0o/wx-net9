using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Core;

public class WxException : Exception
{
    public WxException()
    {
    }

    public WxException(string message): base(message) 
    {
    }

    public WxException(string message,Exception innerException):base(message, innerException)
    {
    }
}

