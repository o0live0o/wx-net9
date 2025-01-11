using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Core.Entities;

public class User : Entity
{
    public string UserName { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}
