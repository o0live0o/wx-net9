using System.ComponentModel.DataAnnotations;

namespace wx.Core.Entities;

public class Stock : AuditEntity, IAggregateRoot
{
    public string Code { get; set; }

    public string Name { get; set; }

    [Timestamp]
    public byte[] RowVersion { get; set; }
}
