using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MarketPlace.Modelos
{
    public class UsuarioAplicacion : IdentityUser
    {
        [Required(ErrorMessage ="El Nombre es Requerido")]
        [Display(Name = "Nombres")]
        [MaxLength(80)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Apellidos son Requeridos")]
        [Display(Name = "Apellidos")]
        [MaxLength(80)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "La Dirección es Requerida")]
        [Display(Name = "Dirección")]
        [MaxLength(200)]
        public string Address { get; set; }

        [Required(ErrorMessage = "La Ciudad es Requerida")]
        [Display(Name = "Ciudad")]
        [MaxLength(60)]
        public string City { get; set; }

        [Required(ErrorMessage = "El Pais es Requerido")]
        [Display(Name = "Pais")]
        [MaxLength(60)]
        public string Country { get; set; }
        [NotMapped]
        public string Role { get; set; }

        public IEnumerable<Producto> Producto { get; set; }
        public IEnumerable<Marca>Marca { get; set; }
        public IEnumerable<Categoria> Categoria { get; set; }  
        public IEnumerable<Bodega> Bodega { get; set; }
    }
}
