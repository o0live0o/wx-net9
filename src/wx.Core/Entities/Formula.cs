namespace wx.Core.Entities;

public class Formula : Entity
{
    public string Name { get; set; }
    private ICollection<Material> Materials { get; set; }
}

public class Material : Entity
{
    public string Code { get; set; }

    public string Name { get; set; }
}