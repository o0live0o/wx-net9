﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace wx.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class WxController : ControllerBase
    {
    }
}
