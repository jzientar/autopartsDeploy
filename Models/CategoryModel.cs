using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AutoPartsCompany.Models
{
    public class CategoryModel
    {
        [Key]
        public int IdCategory { get; set; }
        public int IdSpare { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public String Name { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public String Description { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        public string ImageSrc { get; set; }
    }
}
