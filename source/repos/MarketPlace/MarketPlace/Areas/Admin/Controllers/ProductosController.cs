using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Modelos;
using MarketPlace.Modelos.ViewModels;
using MarketPlace.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using NuGet.Packaging.Signing;
using System.Security.Claims;

namespace MarketPlace.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = DS.RoleAdmin + "," + DS.RoleCliente)]
    public class ProductosController : Controller
	{
		private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductosController(IUnidadTrabajo unidadTrabajo, IWebHostEnvironment webHostEnvironment, UserManager<IdentityUser> userManager)
        {
            _unidadTrabajo = unidadTrabajo;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }
        public IActionResult Index()
		{
			return View();
		}
        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            //filtrar por id de usuario
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
            var categoriausuario = await _unidadTrabajo.Categoria.ObtenerTodos(x => x.UsuarioAplicacionId == claim.Value);
            ProductoVM productoVM = new ProductoVM()
            {

                Producto = new Producto(),

                ListaCategorias = _unidadTrabajo.Producto.ObtenertTodosDropdownLista("Categoria"),
                ListaMarcas = _unidadTrabajo.Producto.ObtenertTodosDropdownLista("Marca"),
                ListaPadre = _unidadTrabajo.Producto.ObtenertTodosDropdownLista("Producto"),
                


        };
            if (id == null)
            {
                //Crear Producto
                productoVM.Producto.Status = true;
                return View(productoVM);
            }
            else
            {
                productoVM.Producto = await _unidadTrabajo.Producto.Obtener(id.GetValueOrDefault());
                if (productoVM.Producto == null)
                {
                    return NotFound();
                }
                return View(productoVM);
            }
           
            
        }
        [HttpPost]
        public async Task<IActionResult> Upsert(ProductoVM productoVM)
        {
            if (!ModelState.IsValid) 
            {

                //Obtener el usuario logeado
                
                var usuario =  await _userManager.GetUserAsync(User);

                if (usuario != null)
                {
                    //asociar e producto con el  usuario
                    productoVM.Producto.UsuarioAplicacionId = usuario.Id;
                }
                //Cargar Imagenes
                string webRootPath = _webHostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (productoVM.Producto.Id == 0)
                {
                    //Crear
                    string upload = Path.Combine(webRootPath + DS.ImageRoute);
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                    {
                        files[0].CopyTo(fileStream);
                    }
                    productoVM.Producto.ImageUrl = fileName + extension;
                    await _unidadTrabajo.Producto.Agregar(productoVM.Producto);
                }
                else
                {
                    //Actualizar
                    var objProducto = await _unidadTrabajo.Producto.ObtenerPrimero(p=> p.Id == productoVM.Producto.Id, isTracking:false);
                    if (files.Count >0)
                    {
                        string upload = Path.Combine(webRootPath + DS.ImageRoute);
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        // Para editar se Borra la imagen anterior
                        var anteriorFile = Path.Combine(upload, objProducto.ImageUrl);

                        if (System.IO.File.Exists(anteriorFile))
                        {
                            System.IO.File.Delete(anteriorFile);
                        }
                        using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStream);
                        }
                        productoVM.Producto.ImageUrl = fileName + extension;
                        

                    }// Si no se carga una imagen nueva
                    else
                    {
                        productoVM.Producto.ImageUrl = objProducto.ImageUrl;
                    }
                     _unidadTrabajo.Producto.Actualizar(productoVM.Producto);
                }
                TempData[DS.Exitosa] = "Transaccion Exitosa";
                await _unidadTrabajo.Guardar();
                return View("Index");
            }// Si algo falla
            productoVM.ListaCategorias = _unidadTrabajo.Producto.ObtenertTodosDropdownLista("Categoria");
            productoVM.ListaCategorias = _unidadTrabajo.Producto.ObtenertTodosDropdownLista("Marca");
            productoVM.ListaPadre = _unidadTrabajo.Producto.ObtenertTodosDropdownLista("Producto");
            return View(productoVM);
        }
        


        #region API
        [HttpGet]
        public async Task<IActionResult> ObtenerTodos()
        {
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
            var todos = await _unidadTrabajo.Producto.ObtenerTodos(x => x.UsuarioAplicacionId == claim.Value, 
                incluirPropiedades: "Categoria,Marca");
                
            return Json(new { data = todos });
        }
        //public async Task<IActionResult>Obte

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var productoDb = await _unidadTrabajo.Producto.Obtener(id);
            if (productoDb == null)
            {
                return Json(new { success = false, message = "No se pudo borrar el Producto" });
            }
            //Remover imagen

            string upload = _webHostEnvironment.WebRootPath + DS.ImageRoute;
            var anteriorFile = Path.Combine(upload, productoDb.ImageUrl);
            if (System.IO.File.Exists(anteriorFile))
            {
                System.IO.File.Delete(anteriorFile);
            }

            _unidadTrabajo.Producto.Remover(productoDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Producto borrado con exito" });
        }
        [ActionName("ValidarSerie")]
        public async Task<IActionResult> ValidarSerie(string serie, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Producto.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.SerialNumber.ToLower().Trim() == serie.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b =>b.SerialNumber.ToLower().Trim()== serie.ToLower().Trim() && b.Id !=id);
            }
            if (valor)
            {
                return Json(new { data = true });
            }
            return Json(new { dta = false });
        }

        #endregion
    }
}
