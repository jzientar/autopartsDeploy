using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AutoPartsCompany.Models
{
    public class OfferModel
    {
        [Key]
        public int IdOffer { get; set; }
        public int IdSpare { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public String Name { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public String Description { get; set; }
        [Column(TypeName = "Date")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "Date")]
        public DateTime EndDate { get; set; }
        // IMAGENES
        [Column(TypeName ="nvarchar(100)")]
        public string ImageName { get; set; }
        [NotMapped]
        public IFormFile ImageFile { get; set; }
        [NotMapped]
        public string ImageSrc { get; set; }

    }
}