using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCevicheria.Models
{
    [Table("CLC_Platillos")]
    public class Platillo
    {
        [Key] // Especifica que ID es la clave primaria de la tabla
        public int? ID { get; set; }
        public string? NombrePlatillo { get; set; }
        public int? CategoriaID { get; set; } // Cambiado de idCategoria a Categoria
        public bool? EsMenuDelDia { get; set; }
        public decimal? Precio { get; set; }

        // Propiedad de navegación para la categoría
        [ForeignKey("CategoriaID")]
        public Categoria? CategoriaObjeto { get; set; }
    }

}
