using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TraderBlotter.Api.Models.Dto
{
    public class UserRequestDto
    {
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public string RoleName { get; set; }
        public string UserCode { get; set; }
        public bool IsActive { get; set; }
    }
}
