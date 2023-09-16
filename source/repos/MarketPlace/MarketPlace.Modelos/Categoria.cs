using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MarketPlace.Modelos
{
    public class Categoria
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Nombre de Categoria")]
        [Required(ErrorMessage = "El Nombre es Requerido")]
        [MaxLength(60, ErrorMessage = "El Nombre debe ser Máximo 60 Caracteres")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Descripción")]
        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(100, ErrorMessage = "La descripción Máximo 100 Caracteres")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "El estado es Requerido")]
        [Display(Name = "Estado de la categoria")]
        public bool Status { get; set; }

        [Required]
        public string UsuarioAplicacionId { get; set; } = string.Empty;
        public UsuarioAplicacion UsuarioAplicacion { get; set; }
    }
}
