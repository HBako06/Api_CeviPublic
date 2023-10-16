namespace ApiCevicheria.Models
{
    public class PlatilloActualizar
    {
        public string? NombrePlatillo { get; set; }
        public int? CategoriaID { get; set; }
        public bool? EsMenuDelDia { get; set; }
        public decimal? Precio { get; set; }
    }

}
