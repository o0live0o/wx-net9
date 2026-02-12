namespace wx.Application.Security.Menus;
public record CreateMenuCommand(string Name, string? Description = null, int ParentId = 0) : IRequest<Unit>;