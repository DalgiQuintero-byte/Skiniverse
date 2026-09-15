using Microsoft.AspNetCore.Mvc;
using Skiniverse.Models;

namespace Skiniverse.Models
{
    public class EncuestaModel
    {
        public string RangoEdad { get; set; } = string.Empty;
        public string TipoPielActual { get; set; } = string.Empty;
        public string Sensibilidad { get; set; } = string.Empty;
        public string ObjetivoPrincipal { get; set; } = string.Empty;
    }
}

