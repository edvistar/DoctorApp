using MarketPlace.AccesoDatos.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.AccesoDatos.Repository
{
	public class UnidadTrabajo : IUnidadTrabajo
	{
		private readonly ApplicationDbContext _db;
		public IBodegaRepository Bodega { get; private set; }
        public ICategoriaRepository Categoria { get; private set; }
		public IMarcaRepository Marca { get; private set; }
		public IProductoRepository Producto { get; private set; }
		public IUsuarioAplicacionRepository UsuarioAplicacion { get; private set; }
        public UnidadTrabajo(ApplicationDbContext db)
        {
			_db = db;	
			Bodega = new BodegaRepository(db);
			Categoria = new CategoriaRepository(db);
			Marca = new MarcaRepository(db);
			Producto = new ProductoRepository(db);
			UsuarioAplicacion = new UsuarioAplicacionRepository(db);
        }

        public void Dispose()
		{
			_db.Dispose();
		}

		public async Task Guardar()
		{
			await _db.SaveChangesAsync();
		}
	}
}
