using Data;
using Data.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTOs;
using Models.Entidades;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    public class UsuariosController : BaseApiController
    {
        private readonly ApplicationDbContext _db;
        private readonly ITokenServicio _tokenServicio;

        public UsuariosController(ApplicationDbContext db, ITokenServicio tokenServicio)
        {
            _db = db;
            _tokenServicio = tokenServicio;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var usuarios = await _db.Usuario.ToListAsync();
            return Ok(usuarios);
        }

        [Authorize]
        [HttpGet("{id}")] // api/usuarios/1
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _db.Usuario.FindAsync(id);
            
            return Ok(usuario);
        }

        
        [HttpPost("registro")] // POST: api/usuarios/registro
        public async Task<ActionResult<UsuarioDto>> Registro(RegistroDto registroDto)
        {
            if (await UsuarioExiste(registroDto.UserName)) return BadRequest("UserName ya esta Registrado");

            using var hmac = new HMACSHA512();
            var usuario = new Usuario
            {
                UserName = registroDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registroDto.Password)),
                PasswordSalt = hmac.Key
            };
            _db.Usuario.Add(usuario);
            await _db.SaveChangesAsync();
            return new UsuarioDto
            {
                UserName = usuario.UserName,
                Token = _tokenServicio.CrearToken(usuario)
            };
        }

        [HttpPost("login")]// POST: api/usuarios/login
        public async Task<ActionResult<UsuarioDto>> Login(LoginDto loginDto)
        {
            var usuario = await _db.Usuario.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (usuario == null) return Unauthorized("Usuario No valido");
            using var hmac = new HMACSHA512(usuario.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != usuario.PasswordHash[i]) return Unauthorized("Password No Valido");
                
            }
            return new UsuarioDto
            {
                UserName = usuario.UserName,
                Token = _tokenServicio.CrearToken(usuario)
            };
        }

        private async Task<bool> UsuarioExiste(string username)
        {
            return await _db.Usuario.AnyAsync(x => x.UserName == username.ToLower());
        }

        

    }
}
