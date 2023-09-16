using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Modelos
{
    public class Bodega
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="El Nombre es Requerido")]
        [MaxLength(60,ErrorMessage = "El nombre debe tener máximo 60 caracteres")]
        [Display(Name="Nombre de Bodega")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es Requerida")]
        [MaxLength(100, ErrorMessage = "La descripción debe tener máximo 100 caracteres")]
        [Display(Name = "Descripción")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage ="El estado es requerido")]
        [Display(Name = "Estado")]
        public bool Status { get; set; }

        [Required]
        public string UsuarioAplicacionId { get; set; } = string.Empty;
        public UsuarioAplicacion UsuarioAplicacion { get; set; }
    }
}
