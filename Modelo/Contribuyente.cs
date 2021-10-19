using System;
using System.Collections.Generic;

#nullable disable

namespace RNC_API.Modelo
{
    public partial class Contribuyente
    {
        public int Id { get; set; }
        public string Rnc { get; set; }
        public string RazonSocial { get; set; }
        public string NombreComercial { get; set; }
    }
}
