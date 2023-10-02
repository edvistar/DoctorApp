using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs
{
    public class RegistroDto
    {
        [Required(ErrorMessage = "Username es requerido")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password es requerido")]
        [StringLength(10, MinimumLength =4, ErrorMessage="El password debe tener minimo 4 y maximo 10 caracteres")]
        public string Password { get; set; }
    }
}
