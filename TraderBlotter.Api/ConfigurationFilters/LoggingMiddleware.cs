
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TraderBlotter.Api.ConfigurationFilters
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public LoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<LoggingMiddleware>(); ;
        }

        public async Task Invoke(HttpContext context)
        {            
            var guid = Guid.NewGuid().ToString();
            _logger.LogInformation($"Guid:{guid} - Request Path:{context.Request?.Path.Value} \n Host:{context.Request?.Host.Value} \n QueryString: {context.Request?.QueryString}");
            await _next(context);
            _logger.LogInformation($"Guid:{guid} - Response Status:{context.Response?.StatusCode}");
        }

    }
}
