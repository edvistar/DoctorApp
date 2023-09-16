using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Modelos.ViewModels
{
    public class ProductoVM
    {
        public Producto Producto { get; set; } = new Producto();
        public IEnumerable<SelectListItem> ListaCategorias { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> ListaMarcas { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<SelectListItem> ListaPadre { get; set; } = Enumerable.Empty<SelectListItem>();
        public IEnumerable<Producto> ListaProductos { get; set; } = Enumerable.Empty<Producto>();
        public IEnumerable<UsuarioAplicacion> UsuarioAplicacion { get; set; }
        public IEnumerable<SelectListItem> categoriausuario { get; set; }
        
    }
}
