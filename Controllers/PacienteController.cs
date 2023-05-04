using Microsoft.AspNetCore.Mvc;
using PROYECTO_ED1.Models;
using PROYECTO_ED1.Models.Data;
using System;

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

        public ActionResult Busqueda_Paciente(string Busqueda)
        {
            try
            {
                if (Busqueda != null)
                {
                    Pacientes viewpasciente = Singleton.Instance.AVL.ObtenerLista().FirstOrDefault(a => a.Nombre == Busqueda);
                    if (viewpasciente == null)
                    {
                        viewpasciente = Singleton.Instance.AVL.ObtenerLista().FirstOrDefault(a => a.DPI == Busqueda);
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
        public ActionResult Filtro2()
        {
            string Cognitivo = "Cognitivo";
            string Conductual = "Conductual";
            try
            {
                Singleton.Instance.bandera = 1;
                Singleton.Instance.AuxP = Singleton.Instance.AVL.Obtener2(a => a.Descripcion == Cognitivo, b => b.Descripcion == Conductual, p => (DateTime.Now - p.FDU).TotalDays > 30);
                int a = Singleton.Instance.AVL.GetComparaciones();
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
        public ActionResult Filtro4()
        {
            string Logoterapia = "Logoterapia";
            string Psicodinamica = "Psicodinámica";
            try
            {
                Singleton.Instance.bandera = 1;
                Singleton.Instance.AuxP = Singleton.Instance.AVL.Obtener2(a => a.Descripcion == Logoterapia, b => b.Descripcion == Psicodinamica, p => (DateTime.Now - p.FDU).TotalDays > 14);
                int a = Singleton.Instance.AVL.GetComparaciones();
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
    }
}
