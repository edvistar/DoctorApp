using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.AccesoDatos.Repository.IRepository
{
	public interface IProductoRepository : IRepository<Producto>
	{
		void Actualizar(Producto producto);

		IEnumerable<SelectListItem> ObtenertTodosDropdownLista(string obj);
	}
}
