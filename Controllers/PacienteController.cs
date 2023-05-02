using Microsoft.AspNetCore.Mvc;
using PROYECTO_ED1.Models;
using PROYECTO_ED1.Models.Data;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualBasic.FileIO;

namespace PROYECTO_ED1.Controllers
{
    public class PacienteController : Controller 
    {
        private IWebHostEnvironment Environment;
        public PacienteController(IWebHostEnvironment _environment)
        {
            Environment = _environment;
        }
        public IActionResult Index_Paciente()
        {
            return View();

            //agregar cuando 
           // return View(Singleton.Instance.miAVL.ObtenerLista());
        }

        public ActionResult Create_Paciente()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create_Paciente(IFormCollection collection)
        {

            Pacientes AuxPac = new Pacientes();
            try
            {
                string aux = "";
                var NewPaciente = new Models.Pacientes
                {
                    
                    Nombre = collection["nombre"],
                    DPI = collection["dpi"],
                    Edad = collection["edad"],
                    Telefono = collection["telefono"],
                    Descripcion = collection["descripcion"]
                };

                if (Convert.ToDateTime(collection["FDU"]) < Convert.ToDateTime(DateTime.Today))
                {
                    NewPaciente.FDU = Convert.ToDateTime(collection["FDU"]);
                }
                else
                {
                    AuxPac = NewPaciente;
                    TempData["FEC"] = "Ingreso una fecha incorrecta en la consulta pasada";
                    throw new Exception(null);
                }


                aux = collection["FDP"];
                if (aux != "")
                {

                    if (Convert.ToDateTime(collection["FDP"]) > Convert.ToDateTime(DateTime.Today) && Convert.ToDateTime(collection["FDP"]) > Convert.ToDateTime(collection["FDU"]))
                    {

                        int a = Singleton.Instance.ListaPacientes.GetDay(Convert.ToDateTime(collection["FDP"]));
                        if (a == 8)
                        {
                            AuxPac = NewPaciente;
                            TempData["VOP"] = "Unicamente se pueden atender a 8 personas por día.";
                            throw new Exception(null);
                        }
                        else
                        {
                            NewPaciente.FDP = Convert.ToDateTime(collection["FDP"]);
                            Singleton.Instance.ListaPacientes.add(Convert.ToDateTime(NewPaciente.FDP));
                        }

                    }
                    else
                    {
                        AuxPac = NewPaciente;
                        TempData["FEC"] = "Ingreso una fecha pasada para una proxima consulta";
                        throw new Exception(null);
                    }
                }
                else
                {
                    NewPaciente.FDP = null;
                }


                Singleton.Instance.ListaPacientes.adddpi(collection["dpi"]);

                int b = Singleton.Instance.ListaPacientes.GetDPI(collection["dpi"]);

                if (b == 1)
                {
                    NewPaciente.DPI = collection["dpi"];
                }
                else
                {
                    AuxPac = NewPaciente;
                    TempData["DRP"] = "EL dpi que desea ingresar, ya se encuentra registrado.";
                    throw new Exception(null);
                }

                //agregar cuando ya este avl
                //Singleton.Instance.miAVL.Add(NewPaciente);
                Singleton.Instance.bandera = 0;
                return RedirectToAction(nameof(Index_Paciente));
            }
            catch
            {
                return View(AuxPac);
            }
        }

        public ActionResult CargarArchivo()
        {
            return View();
        }

        public ActionResult CargarArchivo2(IFormFile File)
        {
            string Nombre = "", DPI = "", Edad = "", Telefono = "", UConsul = "", PConsul = "", Diagnostico = "", Categoria = "";

            try
            {
                if (File != null)
                {
                    string path = Path.Combine(this.Environment.WebRootPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    string FileName = Path.GetFileName(File.FileName);
                    string FilePath = Path.Combine(path, FileName);
                    using(FileStream stream= new FileStream(FilePath, FileMode.Create))
                    {
                        File.CopyTo(stream);
                    }
                    using (TextFieldParser csvFile = new TextFieldParser(FilePath))
                    {
                        csvFile.CommentTokens = new string[] { "#" };
                        csvFile.SetDelimiters(new string[] { "," });
                        csvFile.HasFieldsEnclosedInQuotes = true;
                        csvFile.ReadLine();

                        while (!csvFile.EndOfData)
                        {
                            string[] fields = csvFile.ReadFields();
                            DPI = Convert.ToString(fields[0]);
                            Edad = Convert.ToString(fields[1]);
                            Telefono = Convert.ToString(fields[2]);
                            UConsul = Convert.ToString(fields[3]);
                            PConsul= Convert.ToString(fields[4]);
                            Diagnostico= Convert.ToString(fields[5]);
                            Categoria= Convert.ToString(fields[6]);
                            Pacientes NuevoPaciente = new Pacientes
                            {
                                Nombre = Nombre,
                                DPI = DPI,
                                Edad = Edad,
                                Telefono = Telefono,
                                FDU = Convert.ToDateTime(UConsul),
                                FDP = Convert.ToDateTime(PConsul),
                                Descripcion = Diagnostico,
                                Asistencia = Categoria,
                            };
                            //Singleton.Instance.ListaPacientes.add(NuevoPaciente)
                        }
                        return RedirectToAction(nameof(Index));
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            catch(Exception)
            {
                ViewData["Message"] = "Algo malo paso";
                return RedirectToAction(nameof(Index));
            }
        }

       /* public void LeerArchivo()
        {
            string RutaTXT = @"Pacientes.csv";
            var Archivo = new StreamReader(RutaTXT);
            {
                string info = Archivo.ReadToEnd().Remove(0, 101);
                foreach(string fila in info.Split("\n"))
                {
                    try
                    {
                        var NuevoPaciente = new Pacientes();
                        {
                            
                        }
                    }
                }
            }
        }*/

        public ActionResult Busqueda_Paciente(string Busqueda)
        {
            try
            {
                //if (Busqueda != null)
                //{
                //   // Pacientes viewpasciente = Singleton.Instance.AVL.ObtenerLista().FirstOrDefault(a => a.Nombre == Busqueda);
                //    if (viewpasciente == null)
                //    {
                //        //viewpasciente = Singleton.Instance.AVL.ObtenerLista().FirstOrDefault(a => a.DPI == Busqueda);
                //        if (viewpasciente == null)
                //        {
                //            TempData["Bus"] = "No se encontro el Paciente";
                //        }
                //        else
                //        {
                //            return View(viewpasciente);
                //        }
                //    }
                //    else
                //    {
                //        return View(viewpasciente);
                //    }
                //}
                return View();
            }
            catch
            {
                TempData["Bus"] = "No se encontro el Paciente";
                return View();
            }
        }
    }
}
