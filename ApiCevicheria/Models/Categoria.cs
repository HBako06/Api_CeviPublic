using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCevicheria.Models
{
    [Table("CLC_Categorias")]
    public class Categoria
    {
        [Key] // Especifica que ID es la clave primaria de la tabla
        public int ID { get; set; }
        public string NombreCategoria { get; set; }
    }
}
