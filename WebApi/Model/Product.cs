using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Model
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public DateTime FechaCreacion { get; set; }

        [Required]
        public string Category { get; set; }

        [Required]
        public string Proveedor { get; set; }

        [Required]
        public double Precio { get; set; }

        [Required]
        public int Stock {  get; set; }

        public string? Tipo { get; set; }

        public string imgUrl { get; set; }


    }
}
