namespace wx.Application.Categories;

public record CategoryDto(int Id, string Name, int? ParentId, CategoryAttrDto[]? CategoryAttrs = null)
{
    public static CategoryDto FromEntity(Category category)
    {
        return new CategoryDto(
            Id: category.Id,
            Name: category.Name,
            ParentId: category.ParentId,
            CategoryAttrs: category.Attributes?
                .Select(attr => CategoryAttrDto.FromEntity(attr))
                .ToArray()
        );
    }
}

