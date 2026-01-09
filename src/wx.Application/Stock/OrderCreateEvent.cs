using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wx.Core.Domain.Events;

namespace wx.Application.Stock;

public record OrderCreateEvent(Guid Id) : IntegrationEvent(Id);