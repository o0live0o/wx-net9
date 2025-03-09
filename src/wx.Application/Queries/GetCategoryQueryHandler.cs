using MediatR;
using Microsoft.EntityFrameworkCore;
using wx.Infrastructure;

namespace wx.Application.Commands;

public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, IEnumerable<Category>>
{
    private readonly WxContext _dbContext;
    public GetCategoryQueryHandler(WxContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<Category>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
    {
        var query = @"
            WITH CategoryTree AS (
                SELECT * FROM Categories WHERE Id = {0}
                UNION ALL
                SELECT c.* FROM Categories c
                INNER JOIN CategoryTree ct ON c.ParentId = ct.Id
            )
            SELECT * FROM CategoryTree"
        ;
        var categories = await _dbContext.Categories
                    .FromSqlRaw(query, request.parentId)
                    .ToListAsync();
        return categories;
    }
}
