using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.AccesoDatos.Repository
{
	public class MarcaRepository : Repository<Marca>, IMarcaRepository
	{
		private readonly ApplicationDbContext _db;

        public MarcaRepository(ApplicationDbContext db) : base(db)
        {
			_db = db;	
        }
        public void Actualizar(Marca marca)
		{
			var marcaDB = _db.Marca.FirstOrDefault(b => b.Id == marca.Id);
			if (marcaDB != null)
			{
                marcaDB.Name = marca.Name;
                marcaDB.Description = marca.Description;
                marcaDB.Status = marca.Status;
				_db.SaveChanges();
			}
		}
	}
}
