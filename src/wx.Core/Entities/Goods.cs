using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Core.Entities;

public class Goods : AuditEntity, IAggregateRoot
{
    public string Name { get; set; }

}
