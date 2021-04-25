using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Repository.Models
{
    [Table("t_groupdealermapping")]
    public class GroupDealerMappingView
    {
        [Key]
        public int Id { get; set; }
        public string GroupName { get; set; }
        public string DealerCode { get; set; }
    }
}
