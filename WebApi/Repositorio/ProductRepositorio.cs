using WebApi.Datos;
using WebApi.Model;
using WebApi.Repositorio.IRepositorio;

namespace WebApi.Repositorio
{
    public class ProductRepositorio : Repositorio<Product>, iProductRepositorio
    {
        private readonly AplicationDbContext context;

        public ProductRepositorio(AplicationDbContext _context): base(_context)
        {
            context= _context;
        }
        public async Task<Product> Actualizar(Product entidad)
        {
            entidad.FechaCreacion=DateTime.Now;
            context.Products.Update(entidad);
            await context.SaveChangesAsync();
            return entidad;
        }
    }
}
