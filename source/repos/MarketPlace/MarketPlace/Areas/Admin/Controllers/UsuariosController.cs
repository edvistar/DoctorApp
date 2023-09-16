using MarketPlace.AccesoDatos;
using MarketPlace.AccesoDatos.Repository.IRepository;
using MarketPlace.Utilidades;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace MarketPlace.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = DS.RoleAdmin)]
    public class UsuariosController : Controller
    {
        private readonly IUnidadTrabajo _unidadTrabajo;
        private readonly ApplicationDbContext _db;
        public UsuariosController(IUnidadTrabajo unidadTrabajo, ApplicationDbContext db)
        {
            _unidadTrabajo = unidadTrabajo;
            _db = db;
        }
        public IActionResult Index()
        {
            return View();
        }

        #region API
        public async Task<IActionResult> ObtenerTodos()
        {
            var listaUsuarios = await _unidadTrabajo.UsuarioAplicacion.ObtenerTodos();
            var userRol = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();

            foreach (var usuario in listaUsuarios)
            {
                var roleId = userRol.FirstOrDefault(u => u.UserId == usuario.Id).RoleId;
                usuario.Role = roles.FirstOrDefault(u=> u.Id == roleId).Name;
            }
            return Json(new { data = listaUsuarios });
        }

        public IActionResult BloquearDesbloquear([FromBody] string id)
        {
            var usuario = _db.UsuarioAplicacion.FirstOrDefault(u => u.Id == id);
            if (usuario == null)
            {
                return Json(new { success = false, message = "Error de Usuario" });
            }
            if (usuario.LockoutEnd != null && usuario.LockoutEnd > DateTime.Now)
            {
                //Usuario Bloqueado
                usuario.LockoutEnd = DateTime.Now;
            }
            else
            {
                usuario.LockoutEnd = DateTime.Now.AddYears(1000);
            }
            _unidadTrabajo.Guardar();
            return Json(new { success = true, message = "Operación Exitosa" });
        }
        #endregion
    }
}
