using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCevicheria.Models
{

    [Table("CLC_Platillos")]
    public class PlatilloAgregar
    {
        [Required(ErrorMessage = "El nombre del platillo es obligatorio.")]
        public string NombrePlatillo { get; set; }

        //[Range(1, int.MaxValue, ErrorMessage = "Por favor, selecciona una categoría válida.")]
        public int CategoriaID { get; set; }
    }

}
