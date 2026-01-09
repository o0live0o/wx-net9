using System;
using System.Collections.Generic;
using System.Text;

namespace wx.Application.Common.Services;

public interface IDatabaseProvider
{
    DbContextOptions<TContext> Configure<TContext>(DbContextOptionsBuilder<TContext> optionsBuilder)
        where TContext : DbContext;
}
