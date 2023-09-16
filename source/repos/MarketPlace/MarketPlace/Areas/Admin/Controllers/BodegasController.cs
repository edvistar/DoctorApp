using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Modelos;
using MarketPlace.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemaInventario.Utilidades;
using System.Security.Claims;

namespace MarketPlace.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Roles = DS.RoleAdmin)]
	public class BodegasController : Controller
	{
		private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly UserManager<IdentityUser> _userManager;
        public BodegasController(IUnidadTrabajo unidadTrabajo, UserManager<IdentityUser> userManager)
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
            Bodega bodega = new Bodega();
            if (id == null)
            {
                //Se realiza un insert
                bodega.Status = true;
                return View(bodega);
            }
            bodega = await _unidadTrabajo.Bodega.Obtener(id.GetValueOrDefault());
            if (bodega == null)
            {
                return NotFound();
            }
            return View(bodega);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Bodega bodega)
        {
            if (!ModelState.IsValid)
            {
                //Obtener el usuario logeado

                var usuario = await _userManager.GetUserAsync(User);

                if (usuario != null)
                {
                    //asociar e producto con el  usuario
                    bodega.UsuarioAplicacionId = usuario.Id;
                }
                if (bodega.Id == 0)
                { 
                   await _unidadTrabajo.Bodega.Agregar(bodega);
                    TempData[DS.Exitosa] = "Bodega creada correctamente";
                }
                else
                {
                     _unidadTrabajo.Bodega.Actualizar(bodega);
                    TempData[DS.Exitosa] = "Bodega actualizada correctamente";
                }
               await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar la bodega";
            return View(bodega);
        }


        #region API
        [HttpGet]
		public async Task<IActionResult> ObtenerTodos()
		{
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
            var todos = await _unidadTrabajo.Bodega.ObtenerTodos(x => x.UsuarioAplicacionId == claim.Value);
			return Json(new {data = todos});
		}

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var bodegaDb = await _unidadTrabajo.Bodega.Obtener(id);
            if (bodegaDb == null)
            {
                return Json(new { success = false, message = "NO se pudo borrar la bodega" });
            }
            _unidadTrabajo.Bodega.Remover(bodegaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Bodega borrada con exito" });
        }
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Bodega.ObtenerTodos();
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
