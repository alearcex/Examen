using ExamenProgra.Entidades;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ExamenProgra.Proceso
{
    public class MenuPrincipal
    {
        public List<Registro> Registros { get; set; }
        public List<Materiales> Materiales { get; set; }
        public object ListaEstudiantes { get; private set; }

        public MenuPrincipal()
        {
            Registros = new List<Registro>();
            Materiales = new List<Materiales> { new Materiales(1, "Vidrio", 15),
                                                new Materiales(2, "Papel y Cartón", 10),
                                                new Materiales(3, "Aluminio", 25),
                                                new Materiales(4, "Plástico", 18),
                                                new Materiales(5, "TetraPak", 13) };
        }
        public void Menu()  
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("********************************************************");
                Console.WriteLine("**            SISTEMA COMUNAL DE RECICLAJE            **");
                Console.WriteLine("********************************************************");
                Console.WriteLine("Seleccione una opción: ");
                Console.WriteLine("1- Registrar entrega de reciclaje");
                Console.WriteLine("2- Consulta de registros");
                Console.WriteLine("3- Modificar registro");
                Console.WriteLine("4- Eliminar registro");
                Console.WriteLine("5- Salir");
                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        InsertarRegistro();
                        break;
                    case "2":
                        if(Registros.Count > 0)
                        {
                            MenuConsultas.Menu(Registros);
                        }
                        else
                        {
                            Console.WriteLine("No hay registros");
                            Thread.Sleep(3000); // Espera 3 segundos antes de continuar
                            Console.Clear();
                        }

                        break;
                    case "3":
                        ModificarRegistro();
                        break;
                    case "4":
                        EliminarRegistro();
                        break;
                    case "5":
                        Console.WriteLine("Saliendo...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.BackgroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Opción no válida, por favor inténtelo de nuevo.");
                        Console.ResetColor();
                        Thread.Sleep(3000); // Espera 3 segundos antes de continuar
                        Console.Clear();
                        break;

                }
            }
        }

        #region "Métodos para las opciones del menú"
        public void InsertarRegistro()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("******************************************************");
                Console.WriteLine("**         Registro de entrega de Reciclaje         **");
                Console.WriteLine("******************************************************");
                //Se crea el nuevo  para agregarlo a la lista
                Registro registro = new Registro();

                //Se llenan los datos utilizando los métodos de ingreso
                registro.IdRegistro = Registros.Count + 1; //simula el identity
                registro.Usuario = InsertarUsuario();
                registro.IdMaterial = InsertarMaterial(Materiales);       
                //Se obtienen los puntos que da ese material por kilo depositado
                int puntos = Materiales.Where(x => x.IdMaterial == registro.IdMaterial).Select(x => x.PuntosXKg).First();
                registro.CantidadKg = InsertarPeso();
                registro.Fecha = DateTime.Now;
                registro.Puntos = puntos * registro.CantidadKg;
                Console.WriteLine("Puntos acumulados por depósito: {0}", registro.Puntos);

                Registros.Add(registro);

                //Se consulta si se quiere seguir en esta opción
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("¿Desea ingresar información de otro registro? (S/N): ");
                string continuar = Console.ReadLine();
                if (continuar.ToUpper() != "S")
                {
                    break;
                }

            }
        }
        public void ModificarRegistro()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("******************************************************");
                Console.WriteLine("**              Modificar  Registro                 **");
                Console.WriteLine("******************************************************");
                var id = MenuConsultas.InsertarID();


                //Se valida si hay un registro con ese id
                if (Existe(id))
                {
                    //Obtiene la información del registro
                    Registro registroModificar = Registros.Where(r => r.IdRegistro == id).First();

                    //Vuelven a pedir los datos del registro para que sean modificados
                    MenuPrincipal menu = new MenuPrincipal();
                    registroModificar.IdMaterial = InsertarMaterial(menu.Materiales);
                    registroModificar.CantidadKg = InsertarPeso();
                    registroModificar.Fecha = DateTime.Now;
                    var material = menu.Materiales.Where(m => m.IdMaterial == registroModificar.IdMaterial).First();
                    registroModificar.Puntos = registroModificar.CantidadKg * material.PuntosXKg;

                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine("Cambios realizados con éxito.");//Se confirma que todo salió bien
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("Estudiante no encontrado.");
                }

                //Consulta si desea iniciar de nuevo en esta opción
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("¿Desea realizar otra consulta para modificar? (S/N): ");
                string continuar = Console.ReadLine();
                if (continuar.ToUpper() != "S")
                {
                    break;
                }

            }
        }
        public void EliminarRegistro()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("******************************************************");
                Console.WriteLine("**                Eliminar  Registros               **");
                Console.WriteLine("******************************************************");
                var id = MenuConsultas.InsertarID();

                //Se valida si existe registro con esa cédula
                if (Existe(id))
                {
                    //Se elimina el registro con esa cédula
                    Registros.RemoveAll(registro => registro.IdRegistro == id);
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.WriteLine("El registro se eliminó correctamente");
                    Console.ResetColor();

                }
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.WriteLine("Registro no encontrado.");
                    Console.ResetColor();
                }

                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("¿Desea realizar otra consulta para eliminar? (S/N): ");
                string continuar = Console.ReadLine();
                if (continuar.ToUpper() != "S")
                {
                    break;
                }
            }

        }
        #endregion

        #region Metodos de insertar
        private static decimal InsertarPeso()
        {
            decimal peso;
            while (true)
            {
                try
                {
                    Console.WriteLine("-Ingrese el peso total de material depositado: ");
                    //Se cambian los puntos por comas
                    var valor = Console.ReadLine().Replace(",", ".");
                    peso = Convert.ToDecimal(valor, new CultureInfo("en-US"));

                    // se verifica si el peso está dentro del rango válido
                    if (peso < (decimal)0.000)
                    {
                        //Mensaje que tendrá la excepción
                        throw new Exception("El peso a ingresar debe der mayor a 0");
                    }

                    break;
                }
                catch (FormatException)
                {
                    //Excepción en caso de no ser decimal
                    Console.WriteLine("Error: Debe ingresar un número (se aceptan decimales).");
                }
                catch (Exception ex)
                {
                    //Excepción en caso de no ser mayor a 0
                    Console.WriteLine("Error: " + ex.Message); 
                }
            }

            //Se usa para redondear el número a 3 decimales
            return Math.Round(peso, 3); 
        }
        private static int InsertarMaterial(List<Materiales> materiales)
        {
            int id;
            while (true)
            {
                try
                {
                    Console.WriteLine("-Seleccione el tipo de material:");
                    foreach (var m in materiales)
                    {
                        Console.WriteLine("{0}- {1}", m.IdMaterial, m.Descripcion);
                    }

                    id = int.Parse(Console.ReadLine());
                    // se verifica si el peso está dentro del rango válido
                    if (id < 0 || id > materiales.Count)
                    {
                        //Mensaje que tendrá la excepción
                        throw new Exception("El código de material ingrsado no existe");
                    }

                    break;
                }
                catch (FormatException)
                {
                    //Excepción en caso de no ser decimal
                    Console.WriteLine("Error: Debe ingresar un número.");
                }
                catch (Exception ex)
                {
                    //Excepción en caso de no ser mayor a 0
                    Console.WriteLine("Error: " + ex.Message); 
                }
            }

            return id; 
        }
        public static string InsertarUsuario()
        {
            string user;
            while (true)
            {
                try
                {
                    Console.WriteLine("-Ingrese el nombre de usuario:");
                    user = Console.ReadLine();


                    // Validar que no sea nulo o solo espacios
                    if (string.IsNullOrWhiteSpace(user))
                    {
                        throw new Exception("El usuario no puede estar vacío ni consistir solo de espacios.");
                    }


                    // Validar que el usuario solo contenga letras o números
                    foreach (char caracter in user)
                    {
                        if (!char.IsLetterOrDigit(caracter) && caracter != ' ' && !char.IsWhiteSpace(caracter))
                        {
                            throw new Exception("El usuario debe contener solo letras y números.");
                        }
                    }

                    break; // Salir del bucle si no hay excepciones
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return user;
        }

        #endregion
        public bool Existe(int id)
        {
            //Expresión lambda que devuelve true si encuentra que alguno de los registros tiene el id que vino como parámetro
            return Registros.Any(r => r.IdRegistro == id);
        }

    }
}
