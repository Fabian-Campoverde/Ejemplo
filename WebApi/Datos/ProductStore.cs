using WebApi.Model.DTO;

namespace WebApi.Datos
{
    public static class ProductStore
    {
        public static List<ProductDTO> lista = new List<ProductDTO>
        {
            new ProductDTO { Id = 1, Name = "La Iliada", Description = "De Homero", Category="Obra"},
            new ProductDTO { Id = 2, Name = "La Odisea", Description = "De homero x2", Category="Obra x2" }
        };
    }
}

