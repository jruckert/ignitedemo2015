using Ignite.Entities;
using Ignite.Services.Infrastructure;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ignite.Api
{
    [Route("api/[controller]")]
    public class TicketStatusController : SimpleController<TicketStatus>
    {
        public TicketStatusController(IBaseService<TicketStatus> baseService, ILoggerFactory loggerFactory)
            : base(baseService, loggerFactory)
        {

        }
    }
}
