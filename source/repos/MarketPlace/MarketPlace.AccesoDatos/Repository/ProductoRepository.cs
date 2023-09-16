using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Modelos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.AccesoDatos.Repository
{
	public class ProductoRepository : Repository<Producto>, IProductoRepository
	{
		private readonly ApplicationDbContext _db;
        

        public ProductoRepository(ApplicationDbContext db) : base(db)
        {
			_db = db;	
            
        }

        public void Actualizar(Producto producto)
		{
			var productoDB = _db.Producto.FirstOrDefault(b => b.Id == producto.Id);
			if (productoDB != null)
			{
				if (producto.ImageUrl !=null)
				{
					productoDB.ImageUrl = producto.ImageUrl;

                }
                productoDB.Name = producto.Name;
				productoDB.SerialNumber = producto.SerialNumber;
                productoDB.Description = producto.Description;
                productoDB.Status = producto.Status;
				productoDB.Offer = producto.Offer;	
				productoDB.Cost = producto.Cost;
				productoDB.Price = producto.Price;
				productoDB.CategoriaId = producto.CategoriaId;
				productoDB.MarcaId = producto.MarcaId;
                productoDB.UsuarioAplicacionId = producto.UsuarioAplicacionId;
                productoDB.Descuento = producto.Descuento;
                productoDB.TieneDescuento = producto.TieneDescuento;
                productoDB.ContactPhoneNumber = producto.ContactPhoneNumber;

				_db.SaveChanges();
			}
		}

        public IEnumerable<SelectListItem> ObtenertTodosDropdownLista(string obj)
        {
            
            if (obj == "Categoria")
            {
                return _db.Categoria.Where(c => c.Status == true).Select(c => new SelectListItem
				{
					Text = c.Name,
					Value = c.Id.ToString()
				});
			}
            if (obj == "Marca")
            {
                return _db.Marca.Where(c => c.Status == true).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }
            if (obj == "Producto")
            {
                return _db.Producto.Where(c => c.Status == true).Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });
            }
            return null;
        }
    }
}
