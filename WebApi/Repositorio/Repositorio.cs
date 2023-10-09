using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApi.Datos;
using WebApi.Repositorio.IRepositorio;

namespace WebApi.Repositorio
{
    public class Repositorio<T> : iRepositorio<T> where T : class
    {

        private readonly AplicationDbContext db;
        internal DbSet<T> dbset;

        public Repositorio(AplicationDbContext dbcontext)
        {
            db = dbcontext;
            this.dbset=db.Set<T>();
        }

        public async Task Crear(T entidad)
        {
            await dbset.AddAsync(entidad);
            await Grabar();

        }

        public async Task Grabar()
        {
            await db.SaveChangesAsync();
        }

        public async Task<T> Obtener(Expression<Func<T, bool>>? filtro = null, bool tracked = true)
        {
            IQueryable<T> query = dbset;
            if (!tracked)
            {
                query = query.AsNoTracking();

            }
            if(filtro  != null)
            {
                query = query.Where(filtro);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> obtenerTodos(Expression<Func<T, bool>>? filtro = null)
        {
            IQueryable<T> query = dbset;
            if (filtro != null)
            {
                query = query.Where(filtro);
            }
            return await query.ToListAsync();
        }

        public async Task Remover(T entidad)
        {
            dbset.Remove(entidad);
            await Grabar();
        }
    }
}
