using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Model.DTO
{
    public class NumberProductCreateDTO
    {

        [Required]
        public int ProductNo { get; set; }

        [Required]
        public int ProductId { get; set; } 

        public string Detalle { get; set; }

        
    }
}
