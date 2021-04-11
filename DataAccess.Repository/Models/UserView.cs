using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Repository.Models
{
    [Table("t_users")]
    public class UserView
    {
        [Key]
        public string LoginName { get; set; }
        public string Password { get; set; }
        public string EmailId { get; set; }
        public int RoleId { get; set; }
        public string GroupName { get; set; }
        public string DealerCode { get; set; }
        public string ClientCode { get; set; }
        public bool IsActive { get; set; }

    }
}
