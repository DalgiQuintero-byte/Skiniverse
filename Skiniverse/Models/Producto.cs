using System.ComponentModel.DataAnnotations;

namespace Skiniverse.Models
{
    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        public string NombreProducto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public string TipoPielRecomendado { get; set; } = string.Empty;
        public string IngredientesActivos { get; set; } = string.Empty;
        public decimal PrecioRegular { get; set; }
        public int StockActual { get; set; }
        public string? ImagenUrl { get; set; }
    }
}