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
using Microsoft.AspNetCore.Components.Forms;

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
            if (Singleton.Instance.bandera == 1)
            {
                Singleton.Instance.bandera = 0;
                return View(Singleton.Instance.AuxP);
            }
            else if (Singleton.Instance.bandera == 2)
            {
                Singleton.Instance.bandera = 0;
                return View(Singleton.Instance.Consultas);
            }
            else if (Singleton.Instance.bandera == 3)
            {
                Singleton.Instance.bandera = 0;
                return View(Singleton.Instance.Aux);
            }
            else
            {
                return View(Singleton.Instance.miAVL.ObtenerLista());
            }
        }

        public ActionResult Consultas() {
            if (Singleton.Instance.bandera == 2)
            {
                Singleton.Instance.bandera = 0;
                return View(Singleton.Instance.Consultas);
            }
            return View();
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
                               
                Singleton.Instance.miAVL.Add(NewPaciente);
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
                            Nombre = Convert.ToString(fields[0]);
                            DPI = Convert.ToString(fields[1]);
                            Edad = Convert.ToString(fields[2]);
                            Telefono = Convert.ToString(fields[3]);
                            UConsul = Convert.ToString(fields[4]);
                            PConsul= Convert.ToString(fields[5]);
                            Categoria = Convert.ToString(fields[6]);
                            Diagnostico= Convert.ToString(fields[7]);
                            Pacientes NuevoPaciente = new Pacientes
                            {
                                Nombre = Nombre,
                                DPI = DPI,
                                Edad = Edad,
                                Telefono = Telefono,
                                FDU = Convert.ToDateTime(UConsul),
                                FDP = Convert.ToDateTime(PConsul),
                                Asistencia = Categoria,
                                Descripcion = Diagnostico,
                            };
                            Singleton.Instance.miAVL.Add(NuevoPaciente);
                        }
                    }
                }
                return RedirectToAction(nameof(Index_Paciente));
            }
            catch(Exception)
            {
                ViewData["Message"] = "Algo malo paso";
                return RedirectToAction(nameof(Index_Paciente));
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
                if (Busqueda != null)
                {
                    Pacientes viewpasciente = Singleton.Instance.miAVL.ObtenerLista().FirstOrDefault(a => a.Nombre == Busqueda);
                    if (viewpasciente == null)
                    {
                        viewpasciente = Singleton.Instance.miAVL.ObtenerLista().FirstOrDefault(a => a.DPI == Busqueda);
                        if (viewpasciente == null)
                        {
                            TempData["Bus"] = "No se encontro el Paciente";
                        }
                        else
                        {
                            return View(viewpasciente);
                        }
                    }
                    else
                    {
                        return View(viewpasciente);
                    }
                }
                return View();
            }
            catch
            {
                TempData["Bus"] = "No se encontro el Paciente";
                return View();
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Filtro1()
        {
            string SinTratamiento = "";
            try
            {
                Singleton.Instance.bandera = 3;
                Singleton.Instance.Aux = Singleton.Instance.miAVL.Obtener2(a => a.Descripcion == SinTratamiento, p => (DateTime.Now - p.FDU).TotalDays > 180);
                int a = Singleton.Instance.miAVL.GetComparaciones();
                TempData["TComp3"] = "Se realizaron: " + Convert.ToString(a) + " comparaciones.";
                return RedirectToAction(nameof(Index_Paciente));
            }
            catch (Exception)
            {
                Singleton.Instance.bandera = 0;
                ViewData["Message"] = "No Encontrado";
                return RedirectToAction(nameof(Index_Paciente));
            }
        }
        public ActionResult Filtro2()
        {
            string Cognitivo = "Cognitivo";
            string Conductual = "Conductual";
            try
            {
                Singleton.Instance.bandera = 3;
                Singleton.Instance.Aux = Singleton.Instance.miAVL.Obtener2(a => a.Descripcion == Cognitivo || a.Descripcion == Conductual, p => (DateTime.Now - p.FDU).TotalDays > 30);
                int a = Singleton.Instance.miAVL.GetComparaciones();
                TempData["TComp3"] = "Se realizaron: " + Convert.ToString(a) + " comparaciones.";
                return RedirectToAction(nameof(Index_Paciente));
            }
            catch (Exception)
            {
                Singleton.Instance.bandera = 0;
                ViewData["Message"] = "No Encontrado";
                return RedirectToAction(nameof(Index_Paciente));
            }
        }
        public ActionResult Filtro3()
        {
            string Gestaltica = "Gestaltica";
            try
            {
                Singleton.Instance.bandera = 3;
                Singleton.Instance.Aux = Singleton.Instance.miAVL.Obtener2(a => a.Descripcion == Gestaltica, p => (DateTime.Now - p.FDU).TotalDays > 60);
                int a = Singleton.Instance.miAVL.GetComparaciones();
                TempData["TComp3"] = "Se realizaron: " + Convert.ToString(a) + " comparaciones.";
                return RedirectToAction(nameof(Index_Paciente));
            }
            catch (Exception)
            {
                Singleton.Instance.bandera = 0;
                ViewData["Message"] = "No Encontrado";
                return RedirectToAction(nameof(Index_Paciente));
            }
        }

        public ActionResult RegistrarConsulta()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegistrarConsulta(IFormCollection collection)
        {           
          int cont = 0;//contador para fechas
            try
            {
                Consulta consulta = new Models.Consulta //se crea nueva consulta con el paciente y la fecha
                {
                    paciente = Singleton.Instance.miAVL.ObtenerLista().FirstOrDefault(a => a.DPI == collection["DPI"]),//se busca al paciente en el avl 
                    fecha = Convert.ToDateTime(collection["FDP"])//se agrega la fecha de la consulta
                };
                
                foreach (var con in Singleton.Instance.Consultas)
                {
                    if (con.fecha == consulta.fecha)
                    {
                        cont++;
                    }
                }
                if (cont >= 12)
                {
                    TempData["FEC"] = "Ya no se pueden atender mas pacientes en esa fecha, porfavor ingrese otra.";
                    throw new Exception(null);
                }
                else
                {
                    Singleton.Instance.Consultas.Add(consulta);
                    Singleton.Instance.bandera = 2;
                }
                return RedirectToAction(nameof(Consultas));
            }
            catch (Exception)
            {
                Singleton.Instance.bandera = 0;
                ViewData["Message"] = "No Encontrado";
                return RedirectToAction(nameof(Consultas));
            }
        }
        public ActionResult Filtro4()
        {
            string Logoterapia = "Logoterapia";
            string Psicodinamica = "Psicodinámica";
            try
            {
                Singleton.Instance.bandera = 3;
                Singleton.Instance.Aux = Singleton.Instance.miAVL.Obtener2(a => a.Descripcion == Logoterapia || a.Descripcion == Psicodinamica, p => (DateTime.Now - p.FDU).TotalDays > 30);
                int a = Singleton.Instance.miAVL.GetComparaciones();
                TempData["TComp3"] = "Se realizaron: " + Convert.ToString(a) + " comparaciones.";
                return RedirectToAction(nameof(Index_Paciente));
            }
            catch (Exception)
            {
                Singleton.Instance.bandera = 0;
                ViewData["Message"] = "No Encontrado";
                return RedirectToAction(nameof(Index_Paciente));
            }
        }

        public ActionResult ModificarConsulta()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ModificarConsulta(IFormCollection collection)
        {
            int cont = 0;//contador para fechas
            try
            {
                Consulta consulta = new Models.Consulta //se crea nueva consulta con el paciente y la fecha
                {
                    paciente = Singleton.Instance.miAVL.ObtenerLista().FirstOrDefault(a => a.DPI == collection["DPI"]),//se busca al paciente en el avl 
                    fecha = Convert.ToDateTime(collection["FUP"])//se agrega la fecha de la consulta
                };
                foreach (var c  in Singleton.Instance.Consultas)
                {
                    if (c == consulta)
                    {
                        foreach (var con in Singleton.Instance.Consultas)
                        {
                            if (con.fecha == consulta.fecha)
                            {
                                cont++;
                            }
                        }
                        if (cont <= 12)
                        {
                            TempData["FEC"] = "Ya no se pueden atender mas pacientes en esa fecha, porfavor ingrese otra.";
                            throw new Exception(null);
                        }
                        else
                        {
                            consulta.fecha = Convert.ToDateTime(collection["FDP"]);
                            Singleton.Instance.Consultas.Remove(c);
                            Singleton.Instance.Consultas.Add(consulta);
                            Singleton.Instance.bandera = 2;
                        }
                    }
                    else
                    {
                        TempData["NEP"] = "No exista la consulta que quiere modificar, porfavor revise sus datos.";
                        throw new Exception(null);
                    }
                }
                return RedirectToAction(nameof(Consultas));
            }
            catch (Exception)
            {
                Singleton.Instance.bandera = 0;
                ViewData["Message"] = "No Encontrado";
                return RedirectToAction(nameof(Consultas));
            }
        }
    }
}
