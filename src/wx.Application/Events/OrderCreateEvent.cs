﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wx.Application.Events;

public record OrderCreateEvent(Guid Id) : IntegrationEvent(Id);