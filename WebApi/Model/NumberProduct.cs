using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApi.Model
{
    public class NumberProduct
    {
        [Key] 
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductNo { get; set; }

        [Required]

        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public string Detalle { get; set; }

        public DateTime FechaCreacion { get; set; }



    }
}
