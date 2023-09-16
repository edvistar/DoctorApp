using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Modelos.Espicificaciones;
using MarketPlace.Modelos.ViewModels;
using Microsoft.AspNetCore.Mvc;

using System.Diagnostics;

namespace SistemaInventario.Areas.Inventario.Controllers
{
    [Area("Inventario")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnidadTrabajo _unidadTrabajo;
        [BindProperty]
        private  DetalleVM detalleVM { get; set; }  

        public HomeController(ILogger<HomeController> logger, IUnidadTrabajo unidadTrabajo)
        {
            _logger = logger;
            _unidadTrabajo = unidadTrabajo;
        }

        public async Task<IActionResult> Index(int pageNumber = 1, string busqueda = "", string busquedaActual = "")
        {
            if (!String.IsNullOrEmpty(busqueda))
            {
                pageNumber = 1;
            }
            else
            {
                busqueda = busquedaActual;
            }
            ViewData["busquedaActual"] = busqueda;
            if (pageNumber < 1) pageNumber = 1;

            Parametros parametros = new Parametros()
            {
                PageNumber = pageNumber,
                PageSize = 30
            };
            var resultado =  _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros);

            if (!String.IsNullOrEmpty(busqueda))
            {
                resultado = _unidadTrabajo.Producto.ObtenerTodosPaginado(parametros, p => p.Description.Contains(busqueda));
            }

            ViewData["TotalPaginas"] = resultado.MetaData.TotalPages;
            ViewData["TotalRegistros"] = resultado.MetaData.TotalCount;
            ViewData["PageSize"] = resultado.MetaData.PageSize;
            ViewData["PageNumber"] = pageNumber;
            ViewData["Previo"] = "disabled"; //Clase css para desactivar el boton
            ViewData["Siguiente"] = "";

            if (pageNumber > 1) { ViewData["Previo"] = ""; }
            if (resultado.MetaData.TotalPages <= pageNumber) { ViewData["Siguiente"] = "disabled"; }
            return View(resultado);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            detalleVM = new DetalleVM();

            detalleVM.Producto = await _unidadTrabajo.Producto.ObtenerPrimero(c => c.Id == id,
                                                            incluirPropiedades: "Marca,Categoria"); 
            
            return View(detalleVM);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}