using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPartsCompany.Models
{
    public class MessageModel
    {
        [Key]
        public int IdMessage { get; set; }
        public int IdInbox { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string FullName { get; set; }
        [Column(TypeName = "nvarchar(20)")]
        public string PhoneNumber { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string Subject { get; set; }

        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; }

        [Column(TypeName = "nvarchar(1000)")]
        public String Message { get; set; }

        [Column(TypeName = "Date")]
        public DateTime Date { get; set; }
        public bool Read { get; set; }
    }
}
