using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Modelos.Espicificaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.AccesoDatos.Repository
{
	public class Repository<T> : IRepository<T> where T : class
	{
		private readonly ApplicationDbContext _db;
		internal DbSet<T> dbSet;

        public Repository(ApplicationDbContext db)
        {
			_db = db;	
			this.dbSet = _db.Set<T>();
        }
        public async Task Agregar(T entidad)
		{
			await dbSet.AddAsync(entidad); // insert into table
		}

		public async Task<T> Obtener(int id)
		{
			return await dbSet.FindAsync(id); // select * from (solo por Id)
		}
		public async Task<IEnumerable<T>> ObtenerTodos(Expression<Func<T, bool>> filtro = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null, bool isTracking = true)
		{
			IQueryable<T> query = dbSet;
			if(filtro != null)
			{
				query = query.Where(filtro); // Select * from where
			}
			if (incluirPropiedades !=null)
			{
				foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(incluirProp);
				}
			}
			if(orderBy != null)
			{
				query = orderBy(query);
			}
			if (!isTracking)
			{
				query = query.AsNoTracking();
			}
			return await query.ToListAsync();
		}

		public async Task<T> ObtenerPrimero(Expression<Func<T, bool>> filtro = null, string incluirPropiedades = null, bool isTracking = true)
		{
			IQueryable<T> query = dbSet;
			if (filtro != null)
			{
				query = query.Where(filtro); // Select * from where
			}
			if (incluirPropiedades != null)
			{
				foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
				{
					query = query.Include(incluirProp);
				}
			}
			
			if (!isTracking)
			{
				query = query.AsNoTracking();
			}
			return await query.FirstOrDefaultAsync();
		}

		public void Remover(T entidad)
		{
			dbSet.Remove(entidad);
		}

		public void RemoverRango(IEnumerable<T> entidad)
		{
			dbSet.RemoveRange(entidad);
		}

        public PagedList<T> ObtenerTodosPaginado(Parametros parametros, Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string incluirPropiedades = null)
        {
            IQueryable<T> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);   // select * from where ...
            }

            if (incluirPropiedades != null)
            {
                foreach (var incluirProp in incluirPropiedades.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(incluirProp);
                }
            }

            if (orderBy != null)
            {
                orderBy(query).ToListAsync();
                return PagedList<T>.ToPagedList(query, parametros.PageNumber, parametros.PageSize);
            }
            return PagedList<T>.ToPagedList(query, parametros.PageNumber, parametros.PageSize);
        }
    }
}
