using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenProgra.Entidades
{
    public class Materiales
    {
        public int IdMaterial { get; set; }
        public string Descripcion { get; set; }
        public int PuntosXKg { get; set; }

        //Constructor
        public Materiales(int id, string descripcion, int puntosXKg)
        {
            IdMaterial = id;
            Descripcion = descripcion;
            PuntosXKg = puntosXKg;
        }
    }
}
