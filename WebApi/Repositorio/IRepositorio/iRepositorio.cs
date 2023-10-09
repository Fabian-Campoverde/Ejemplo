using System.Linq.Expressions;

namespace WebApi.Repositorio.IRepositorio
{
    public interface iRepositorio<T> where T : class
    {
        Task Crear(T entidad);
        Task<List<T>> obtenerTodos(Expression <Func<T,bool>>? filtro =null);

        Task<T> Obtener(Expression<Func<T,bool>> filtro = null,bool tracked=true);

        Task Remover(T entidad);

        Task Grabar();



    }
}
