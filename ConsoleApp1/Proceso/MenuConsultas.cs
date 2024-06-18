using ExamenProgra.Entidades;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ExamenProgra.Proceso
{
    public class MenuConsultas
    {
        public static void Menu(List<Registro> registros)
        {
            while(true)
            {
                Console.Clear();
                Console.WriteLine("******************************************************");
                Console.WriteLine("**         Consulta de depositos de reciclaje       **");
                Console.WriteLine("******************************************************");
                Console.WriteLine("Seleccione una opción: ");
                Console.WriteLine("1- Buscar depósitos por usuario");
                Console.WriteLine("2- Buscar depósito por ID de registro");
                Console.WriteLine("3- Buscar depósitos por material");
                Console.WriteLine("4- Generar reporte general");
                Console.WriteLine("5- Volver al menú principal");
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        BusquedaXUsuario(registros);
                        break;
                    case "2":
                        BusquedaXId(registros);
                        break;
                    case "3":
                        BusquedaXMaterial(registros);
                        break;
                    case "4":
                        InformeGeneral(registros);
                        break;
                    default:
                        Console.WriteLine("Opción no válida, por favor inténtelo de nuevo.");
                        Thread.Sleep(3000); // Espera 3 segundos antes de continuar
                        Console.Clear();
                        break;
                }
            }

        }

        private static void BusquedaXUsuario(List<Registro> registros)
        {
            var lista = new List<Registro>();

            Console.Clear();
            Console.WriteLine("********************************************************");
            Console.WriteLine("**   Consulta de depósitos de reciclaje por Usuario   **");
            Console.WriteLine("********************************************************");
            //Se usa el método para obtener el usuario
            string user = MenuPrincipal.InsertarUsuario();
            //Se obtiene una lista de los registros que contengan ese usuario
            lista = registros.Where(x => x.Usuario.Equals(user)).OrderBy(x => x.IdRegistro).ToList();

            if (lista.Count() > 0)
            {
                Console.WriteLine($"{"ID",-5}{"Material",-15}{"Cantidad",-10}{"Fecha",-20}{"Pts Ganados",-11}");
                Console.WriteLine("===================================================================");

                foreach (var r in lista)
                {
                    MenuPrincipal menu = new MenuPrincipal();
                    //Se obtiene la descripción del material para usarla en la tabla de resultados
                    string material = menu.Materiales.Where(x => x.IdMaterial == r.IdMaterial).Select(x => x.Descripcion).First();
                    //Se formatea la fecha
                    string fecha = r.Fecha.ToString("dd/MM/yyyy HH:mm:ss");
                    Console.WriteLine($"{r.IdRegistro,-5}{material,-15}{r.CantidadKg,-10}{fecha,-20}{r.Puntos,-11}");
                }
                Console.WriteLine("===================================================================");
                Console.WriteLine("Total de depoísitos: {0}", lista.Count.ToString());
            }
            else
            {
                Console.WriteLine("No existen registros para este usuario");
            }
            Console.WriteLine("===================================================================");
            Console.WriteLine("            << Pulse cualquier tecla para volver >>            ");
            Console.ReadKey();
        }
        private static void BusquedaXId(List<Registro> registros)
        {

            var lista = new List<Registro>();

            Console.Clear();
            Console.WriteLine("********************************************************");
            Console.WriteLine("**     Consulta de depósitos de reciclaje por ID      **");
            Console.WriteLine("********************************************************");
            //Se usa el método para obtener el ID
            int id = InsertarID();
            //Se obtiene el registro con ese id
            var registro = registros.Find(x => x.IdRegistro == id);
            MenuPrincipal menu = new MenuPrincipal();
            //Se obtiene la descripción del material para usarla en los resultados
            string material = menu.Materiales.Where(x => x.IdMaterial == registro.IdMaterial).Select(x => x.Descripcion).First();


            if (registro != null)
            {
                Console.WriteLine("Usuario: {0}", registro.Usuario);
                Console.WriteLine("Material: {0}", material);
                Console.WriteLine("Cantidad (kg): {0}", registro.CantidadKg);
                Console.WriteLine("Fecha: {0}", registro.Fecha.ToString("dd/MM/yyyy HH:mm:ss"));
                Console.WriteLine("Puntos ganados: {0}", registro.Puntos);

            }
            else
            {
                Console.WriteLine("No existen registro para este ID");
            }
            Console.WriteLine("===================================================================");
            Console.WriteLine("            << Pulse cualquier tecla para volver >>            ");
            Console.ReadKey();
        }
        private static void BusquedaXMaterial(List<Registro> registros)
        {
            var lista = new List<Registro>();
            //Se obtiene la descripción del material para usarla en los resultados
            MenuPrincipal menu = new MenuPrincipal();
            var materiales = menu.Materiales;

            Console.Clear();
            Console.WriteLine("********************************************************");
            Console.WriteLine("**   Consulta de depósitos de reciclaje por Material  **");
            Console.WriteLine("********************************************************");
            Console.WriteLine("-Seleccione el tipo de material:");
            foreach (var m in materiales)
            {
                Console.WriteLine("{0}- {1}", m.IdMaterial, m.Descripcion);
            }

            int id = InsertarID();

            //Se obtiene una lista de los registros que contengan ese material
            lista = registros.Where(x => x.IdMaterial.Equals(id)).OrderBy(x => x.IdRegistro).ToList();

            if (lista.Count() > 0)
            {
                Console.WriteLine($"{"ID",-5}{"Material",-15}{"Cantidad",-10}{"Fecha",-20}{"Pts Ganados",-11}");
                Console.WriteLine("===================================================================");

                foreach (var r in lista)
                {

                    //Se formatea la fecha
                    string material = menu.Materiales.Where(x => x.IdMaterial == r.IdMaterial).Select(x => x.Descripcion).First();
                    string fecha = r.Fecha.ToString("dd/MM/yyyy HH:mm:ss");
                    Console.WriteLine($"{r.IdRegistro,-5}{material,-15}{r.CantidadKg,-10}{fecha,-20}{r.Puntos,-11}");
                }
                Console.WriteLine("===================================================================");
                Console.WriteLine("Total de depósitos: {0}", lista.Count.ToString());
            }
            else
            {
                Console.WriteLine("No existen registros para este material");
            }
            Console.WriteLine("===================================================================");
            Console.WriteLine("            << Pulse cualquier tecla para volver >>            ");
            Console.ReadKey();
        }
        private static void InformeGeneral(List<Registro> registros)
        {

            var vidrios = new List<Registro>();
            var papeles = new List<Registro>();
            var aluminios = new List<Registro>();
            var plasticos = new List<Registro>();
            var tetrapaks = new List<Registro>();

            vidrios = registros.Where(x => x.IdMaterial == 1).ToList();
            papeles = registros.Where(x => x.IdMaterial == 2).ToList();
            aluminios = registros.Where(x => x.IdMaterial == 3).ToList();
            plasticos = registros.Where(x => x.IdMaterial == 4).ToList();
            tetrapaks = registros.Where(x => x.IdMaterial == 5).ToList();

            //Porcentaje que representa cada material del total recolectado
            var totalRegistros = registros.Count;
            var porcVidrio = vidrios.Count > 0 ? (vidrios.Count * 100) / totalRegistros : 0;
            var porcPapeles = papeles.Count > 0 ? (papeles.Count * 100) / totalRegistros : 0;
            var porcAluminio = aluminios.Count > 0 ? (aluminios.Count * 100) / totalRegistros : 0;
            var porcPlasticos = plasticos.Count > 0 ? (plasticos.Count * 100) / totalRegistros : 0;
            var porcTetrapaks = tetrapaks.Count > 0 ? (tetrapaks.Count * 100) / totalRegistros : 0;

            //Peso total
            var kgVidrio = vidrios.Sum(v => v.CantidadKg);
            var kgPapeles = papeles.Sum(v => v.CantidadKg);
            var kgAluminio = aluminios.Sum(v => v.CantidadKg);
            var kgPlasticos = plasticos.Sum(v => v.CantidadKg);
            var kgTetrapaks = tetrapaks.Sum(v => v.CantidadKg);


            Console.Clear();
            Console.WriteLine("******************************************************************");
            Console.WriteLine("****                    INFORME    GENERAL                    ****");
            Console.WriteLine("******************************************************************");
            Console.WriteLine("Vidrios:");
            Console.WriteLine("-Peso total: {0}", kgVidrio);
            Console.WriteLine("-Cantidad de registros: {0}", vidrios.Count);
            Console.WriteLine("-Porcentaje de ingreso: {0}%", porcVidrio);
            Console.WriteLine("===================================================================");
            Console.WriteLine("\nPapeles y Cartón:");
            Console.WriteLine("-Peso total: {0}", kgPapeles);
            Console.WriteLine("-Cantidad de registros: {0}", papeles.Count);
            Console.WriteLine("-Porcentaje de ingreso: {0}%", porcPapeles);
            Console.WriteLine("===================================================================");
            Console.WriteLine("\nAluminio:");
            Console.WriteLine("-Peso total: {0}", kgAluminio);
            Console.WriteLine("-Cantidad de registros: {0}", aluminios.Count);
            Console.WriteLine("-Porcentaje de ingreso: {0}%", porcAluminio);
            Console.WriteLine("===================================================================");
            Console.WriteLine("\nPlásticos:");
            Console.WriteLine("-Peso total: {0}", kgPlasticos);
            Console.WriteLine("-Cantidad de registros: {0}", plasticos.Count);
            Console.WriteLine("-Porcentaje de ingreso: {0}%", porcPlasticos);
            Console.WriteLine("===================================================================");
            Console.WriteLine("\nTetraPak:");
            Console.WriteLine("-Peso total: {0}", kgTetrapaks);
            Console.WriteLine("-Cantidad de registros: {0}", tetrapaks.Count);
            Console.WriteLine("-Porcentaje de ingreso: {0}%", porcTetrapaks);
            Console.WriteLine("===================================================================");
            Console.WriteLine("Totales:");
            Console.WriteLine("-Peso total: {0}", kgTetrapaks + kgPlasticos + kgPapeles + kgVidrio + kgAluminio);
            Console.WriteLine("-Cantidad de registros: {0}", totalRegistros);
            Console.WriteLine("===================================================================");
            Console.WriteLine("            << Pulse cualquier tecla para volver >>            ");
            Console.ReadKey();
        }

        public static int InsertarID()
        {
            int id;
            while (true)
            {
                try
                {
                    Console.WriteLine("-Ingrese ID que desea buscar: ");
                    id = int.Parse(Console.ReadLine());
                    break;
                }
                catch
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Error: Solo se permite el ingreso de números");
                    Console.ResetColor();
                }

            }
            return id;
            
        }
    }
}
