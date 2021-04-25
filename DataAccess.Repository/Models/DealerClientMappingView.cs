using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Repository.Models
{
    [Table("t_dealerclientmapping")]
    public class DealerClientMappingView
    {
        [Key]
        public int Id { get; set; }
        public string DealerCode { get; set; }
       
        public string ClientCode { get; set; }

    }
}
