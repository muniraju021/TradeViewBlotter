using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Repository.Models
{
    [Table("t_groups")]
    public class GroupView
    {
        [Key]
        public string GroupName { get; set; }
    }

    [Table("t_dealers")]
    public class DealerView
    {
        [Key]
        public string DealerCode { get; set; }
    }

    [Table("t_clients")]
    public class ClientView
    {
        [Key]
        public string ClientCode { get; set; }
    }
}
