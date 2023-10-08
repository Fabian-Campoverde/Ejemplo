using System.ComponentModel.DataAnnotations;

namespace WebApi.Model.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; }


        [Required]
        [MaxLength(100)]
        public string Name { get; set; }   

        public string Description { get; set; }
    }
}
