using MarketPlace.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.AccesoDatos.Repository.IRepository
{
	public interface ICategoriaRepository : IRepository<Categoria>
	{
		void Actualizar(Categoria categoria);
	}
}
