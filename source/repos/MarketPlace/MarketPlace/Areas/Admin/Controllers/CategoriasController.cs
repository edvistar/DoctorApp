using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Modelos;
using MarketPlace.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Security.Claims;

namespace MarketPlace.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = DS.RoleAdmin)]
    public class CategoriasController : Controller
	{
		private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly UserManager<IdentityUser> _userManager;
        public CategoriasController(IUnidadTrabajo unidadTrabajo, UserManager<IdentityUser> userManager)
        {
			_unidadTrabajo = unidadTrabajo;
            _userManager = userManager;
        }
        public IActionResult Index()
		{
			return View();
		}
        [HttpGet]
        public async Task<IActionResult> Upsert(int? id)
        {
            Categoria categoria = new Categoria();
            if (id == null)
            {
                //Se realiza un insert
                categoria.Status = true;
                return View(categoria);
            }
            categoria = await _unidadTrabajo.Categoria.Obtener(id.GetValueOrDefault());
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                //Obtener el usuario logeado

                var usuario = await _userManager.GetUserAsync(User);

                if (usuario != null)
                {
                    //asociar e producto con el  usuario
                    categoria.UsuarioAplicacionId = usuario.Id;
                }
                if (categoria.Id == 0)
                { 
                   await _unidadTrabajo.Categoria.Agregar(categoria);
                    TempData[DS.Exitosa] = "Categoria creada correctamente";
                }
                else
                {
                     _unidadTrabajo.Categoria.Actualizar(categoria);
                    TempData[DS.Exitosa] = "Categoria actualizada correctamente";
                }
               await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar la bodega";
            return View(categoria);
        }


        #region API
        [HttpGet]
		public async Task<IActionResult> ObtenerTodos()
        {
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
            var todos = await _unidadTrabajo.Categoria.ObtenerTodos(x => x.UsuarioAplicacionId == claim.Value);
			return Json(new {data = todos});
		}

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bodegaDb = await _unidadTrabajo.Categoria.Obtener(id);
            if (bodegaDb == null)
            {
                return Json(new { success = false, message = "NO se pudo borrar la Categoria" });
            }
            _unidadTrabajo.Categoria.Remover(bodegaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoria borrada con exito" });
        }
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Categoria.ObtenerTodos();
            if (id == 0)
            {
                valor = lista.Any(b => b.Name.ToLower().Trim() == nombre.ToLower().Trim());
            }
            else
            {
                valor = lista.Any(b =>b.Name.ToLower().Trim()== nombre.ToLower().Trim() && b.Id !=id);
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
