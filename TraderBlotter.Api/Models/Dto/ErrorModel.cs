using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TraderBlotter.Api.Models.Dto
{
    public class ErrorModel
    {
        public int HttpStatusCode { get; set; }
        public string Message { get; set; }
        public Exception ExceptionObj { get; set; }
    }
}
