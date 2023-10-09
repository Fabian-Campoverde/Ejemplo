using WebApi.Model;

namespace WebApi.Repositorio.IRepositorio
{
    public interface iNumberProductRepositorio : iRepositorio <NumberProduct>
    {
        Task<NumberProduct> Actualizar (NumberProduct entidad);
    }
}
