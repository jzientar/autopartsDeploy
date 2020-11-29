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
        public int IdMessage { get; set; }
    }
}
