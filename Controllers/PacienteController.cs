using Microsoft.AspNetCore.Mvc;
using PROYECTO_ED1.Models;
using PROYECTO_ED1.Models.Data;

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
            return View(Singleton.Instance.miAVL.ObtenerLista());
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
                if (cont <= 12)
                {
                    TempData["FEC"] = "Ya no se pueden atender mas pacientes en esa fecha, porfavor ingrese otra.";
                    throw new Exception(null);
                }
                else
                {
                    Singleton.Instance.Consultas.Add(consulta);
                }
                return RedirectToAction(nameof(Index_Paciente));
            }
            catch (Exception)
            {

                throw;
                return View();
            }
        }

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
                        }
                    }
                    else
                    {
                        TempData["NEP"] = "No exista la consulta que quiere modificar, porfavor revise sus datos.";
                        throw new Exception(null);
                    }
                }
                
                return RedirectToAction(nameof(Index_Paciente));
            }
            catch (Exception)
            {

                throw;
                return View();
            }
        }
    }
}
