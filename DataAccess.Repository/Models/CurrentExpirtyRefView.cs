using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataAccess.Repository.Models
{
    [Table("t_currentexpriydate_ref")]
    public class CurrentExpirtyRefView
    {
        [Key]
        public string Month { get; set; }
        public string ExpiryDate { get; set; }
    }
}
