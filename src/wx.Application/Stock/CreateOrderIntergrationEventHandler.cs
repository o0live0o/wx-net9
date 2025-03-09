using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using wx.Application.Events;

namespace wx.Application.Stock
{
    public class CreateOrderIntergrationEventHandler : INotificationHandler<OrderCreateEvent>
    {
        public async Task Handle(OrderCreateEvent notification, CancellationToken cancellationToken)
        {
            var _lock = new Lock();
            lock(_lock)
            {

            }
            await Task.Delay(5000);
        }
    }
}
