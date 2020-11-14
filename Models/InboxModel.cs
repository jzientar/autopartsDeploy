using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPartsCompany.Models
{
    public class InboxModel
    {
        [Key]
        public int IdInbox { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public String Message { get; set; }
        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }
    }
}
