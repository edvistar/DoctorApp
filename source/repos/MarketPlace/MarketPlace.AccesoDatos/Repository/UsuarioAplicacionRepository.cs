using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.AccesoDatos.Repository
{
	public class UsuarioAplicacionRepository : Repository<UsuarioAplicacion>, IUsuarioAplicacionRepository
    {
		private readonly ApplicationDbContext _db;

        public UsuarioAplicacionRepository(ApplicationDbContext db) : base(db)
        {
			_db = db;	
        }

        
    }
}
