using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenProgra.Entidades
{
    public class Registro
    {
        public int IdRegistro { get; set; }
        public string Usuario { get; set;}
        public int IdMaterial { get; set;}
        public decimal CantidadKg { get; set;}
        public DateTime Fecha { get; set;}
        public decimal Puntos { get; set;}
    }
}
