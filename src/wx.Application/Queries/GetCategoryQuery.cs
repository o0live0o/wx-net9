using MediatR;

namespace wx.Application.Commands;

public record GetCategoryQuery(int? parentId) : IRequest<IEnumerable<Category>>;