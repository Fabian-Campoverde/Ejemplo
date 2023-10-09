using System.ComponentModel.DataAnnotations;

namespace WebApi.Model.DTO
{
    public class ProductCreateDTO
    {

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }   

        public string Description { get; set; }

        [Required]
        public string Category { get; set; }
   
        [Required]
        public string Proveedor { get; set; }

        [Required]
        public double Precio { get; set; }

        [Required]
        public int Stock { get; set; }

        public string imgUrl { get; set; }
    }
}
