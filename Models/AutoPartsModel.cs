using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AutoPartsCompany.Models
{
    public class AutoPartsModel
    {
        [Key]
        public int IdSpare { get; set; }
        
        public int IdCategory { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Name { get; set; }
        
        [Column(TypeName = "nvarchar(100)")]
        public string MarkSpare { get; set; }
        
        [Column(TypeName = "nvarchar(100)")]
        public string Description { get; set; }
        public double Price { get; set; }
        
        [Column(TypeName = "nvarchar(50)")]
        public string TypeVehicle { get; set; }
        
        [Column(TypeName = "nvarchar(50)")]
        public string MarkVehicle { get; set; }
        
        [Column(TypeName = "nvarchar(50)")]
        public string ModelVehicle { get; set; }

        [Column(TypeName = "nvarchar(100)")]
        public string ImageName { get; set; }

        [NotMapped]
        public IFormFile ImageFile { get; set; }

        [NotMapped]
        public string ImageSrc { get; set; }

    }
}
