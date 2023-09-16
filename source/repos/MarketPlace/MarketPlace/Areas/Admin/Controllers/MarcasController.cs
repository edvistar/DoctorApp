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
    public class MarcasController : Controller
	{
		private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly UserManager<IdentityUser> _userManager;
        public MarcasController(IUnidadTrabajo unidadTrabajo, UserManager<IdentityUser> userManager)
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
            Marca marca = new Marca();
            if (id == null)
            {
                //Se realiza un insert
                marca.Status = true;
                return View(marca);
            }
            marca = await _unidadTrabajo.Marca.Obtener(id.GetValueOrDefault());
            if (marca == null)
            {
                return NotFound();
            }
            return View(marca);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(Marca marca)
        {
            if (!ModelState.IsValid)
            {
                //Obtener el usuario logeado

                var usuario = await _userManager.GetUserAsync(User);

                if (usuario != null)
                {
                    //asociar e producto con el  usuario
                    marca.UsuarioAplicacionId = usuario.Id;
                }
                if (marca.Id == 0)
                { 
                   await _unidadTrabajo.Marca.Agregar(marca);
                    TempData[DS.Exitosa] = "Marca creada correctamente";
                }
                else
                {
                     _unidadTrabajo.Marca.Actualizar(marca);
                    TempData[DS.Exitosa] = "Marca actualizada correctamente";
                }
               await _unidadTrabajo.Guardar();
                return RedirectToAction(nameof(Index));
            }
            TempData[DS.Error] = "Error al grabar la Marca";
            return View(marca);
        }


        #region API
        [HttpGet]
		public async Task<IActionResult> ObtenerTodos()
		{
            var claimIdentidad = (ClaimsIdentity)User.Identity;
            var claim = claimIdentidad.FindFirst(ClaimTypes.NameIdentifier);
            var todos = await _unidadTrabajo.Marca.ObtenerTodos(x => x.UsuarioAplicacionId == claim.Value);
			return Json(new {data = todos});
		}

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var marcaDb = await _unidadTrabajo.Marca.Obtener(id);
            if (marcaDb == null)
            {
                return Json(new { success = false, message = "NO se pudo borrar la Categoria" });
            }
            _unidadTrabajo.Marca.Remover(marcaDb);
            await _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Categoria borrada con exito" });
        }
        [ActionName("ValidarNombre")]
        public async Task<IActionResult> ValidarNombre(string nombre, int id = 0)
        {
            bool valor = false;
            var lista = await _unidadTrabajo.Marca.ObtenerTodos();
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
