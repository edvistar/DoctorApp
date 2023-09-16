using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.AccesoDatos.Repository
{
	public class BodegaRepository : Repository<Bodega>, IBodegaRepository
	{
		private readonly ApplicationDbContext _db;

        public BodegaRepository(ApplicationDbContext db) : base(db)
        {
			_db = db;	
        }
        public void Actualizar(Bodega bodega)
		{
			var bodegaDB = _db.Bodega.FirstOrDefault(b => b.Id == bodega.Id);
			if (bodegaDB != null)
			{
				bodegaDB.Name = bodega.Name;
				bodegaDB.Description = bodega.Description;
				bodegaDB.Status = bodega.Status;
				_db.SaveChanges();
			}
		}
	}
}
