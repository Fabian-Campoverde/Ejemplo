using WebApi.Model;

namespace WebApi.Repositorio.IRepositorio
{
    public interface iProductRepositorio: iRepositorio <Product>
    {
        Task<Product> Actualizar (Product entidad);
    }
}
