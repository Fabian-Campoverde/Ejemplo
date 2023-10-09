using WebApi.Datos;
using WebApi.Model;
using WebApi.Repositorio.IRepositorio;

namespace WebApi.Repositorio
{
    public class NumberProductRepositorio : Repositorio<NumberProduct>, iNumberProductRepositorio
    {
        private readonly AplicationDbContext context;

        public NumberProductRepositorio(AplicationDbContext _context): base(_context)
        {
            context= _context;
        }
        public async Task<NumberProduct> Actualizar(NumberProduct entidad)
        {
            entidad.FechaCreacion=DateTime.Now;
            context.NumberProducts.Update(entidad);
            await context.SaveChangesAsync();
            return entidad;
        }
    }
}
