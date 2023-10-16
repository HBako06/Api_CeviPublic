using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiCevicheria.Models
{
    [Table("CLC_Ordenes")]
    //[PrimaryKey(nameof(ID), nameof(RegistroID))] // Entity framework 7 :")
    public class Orden
    {
        public int ID { get; set; }
        public int RegistroID { get; set; }

        public int? PlatilloID { get; set; } 
        public int? Cantidad { get; set; }
        public DateTime? HoraRegistrada { get; set; }
        public int EstadoID { get; set; }
        public string? NumeroMesa { get; set; }

        [ForeignKey("EstadoID")]
        public Estado? EstadoObjeto { get; set; }

        [ForeignKey("PlatilloID")]
        public Platillo? PlatilloObjeto { get; set; } // Relación con Platillo
    }
}