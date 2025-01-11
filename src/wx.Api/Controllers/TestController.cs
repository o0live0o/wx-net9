using MediatR;
using Microsoft.AspNetCore.Mvc;
using Polly;
using wx.Application.Events;
using wx.Application.Stock.Create;

namespace wx.Api.Controllers;

public class TestController : WxController
{
    private readonly IMediator mediator;
    private readonly IEventBus eventBus;

    public TestController(IMediator mediator,IEventBus eventBus)
    {
        this.mediator = mediator;
        this.eventBus = eventBus;
    }
    [HttpGet("TestRetry")]
    public async Task<IActionResult> TestRetry()
    {
        var ret =  await mediator.Send(new CreateStockCommand("111"));
        await eventBus.PublishAsync(new OrderCreateEvent(new Guid()));
        return Ok(ret);
    }
}
