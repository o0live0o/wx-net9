namespace wx.Application.Categories;

public record CategoryAttrDto(int? AttrId, string Name)
{
    public static CategoryAttrDto FromEntity(CategoryAttribute attr)
    {
        return new CategoryAttrDto(
            AttrId: attr.Id,
            Name: attr.Name
        );
    }
}