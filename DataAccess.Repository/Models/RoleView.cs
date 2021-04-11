using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Repository.Models
{
    [Table("t_roles")]
    public class RoleView
    {
        [Key]
        public int RoleId { get; set; }
        public string RoleName { get; set; }
    }
}
