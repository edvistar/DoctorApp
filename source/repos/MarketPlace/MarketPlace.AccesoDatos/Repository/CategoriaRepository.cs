using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.AccesoDatos.Repository
{
	public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
	{
		private readonly ApplicationDbContext _db;

        public CategoriaRepository(ApplicationDbContext db) : base(db)
        {
			_db = db;	
        }
        public void Actualizar(Categoria categoria)
		{
			var categoriaDB = _db.Categoria.FirstOrDefault(b => b.Id == categoria.Id);
			if (categoriaDB != null)
			{
                categoriaDB.Name = categoria.Name;
                categoriaDB.Description = categoria.Description;
                categoriaDB.Status = categoria.Status;
				_db.SaveChanges();
			}
		}
	}
}
