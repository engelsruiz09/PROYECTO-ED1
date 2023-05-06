using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace PROYECTO_ED1.Models
{
    public class Pacientes: IComparable<Pacientes>
    {
        //nombre completo - requerido
        [Display(Name = "Nombre completo del paciente")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Nombre { get; set; }


        //numero de DPI  o partida de nacimiento - requerido
        [Display(Name = "Número de DPI o partida de nacimiento")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "Tamaño incorrecto de DPI")]
        public string DPI { get; set; }


        //edad-requerido
        [Display(Name = "Edad del paciente")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Edad { get; set; }


        //telefono de contacto - requerido
        [Display(Name = "Número de teléfono")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        [StringLength(8, MinimumLength = 8, ErrorMessage = "Tamaño incorrecto de DPI")]
        public string Telefono { get; set; }


        //fecha de la ultima consulta - requerido
        [Display(Name = "Fecha de última consulta")]
        [DataType(DataType.Date)]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public DateTime FDU { get; set; }


        //fecha de proxima consulta - opcional
        [Display(Name = "Fecha de próxima consulta (Opcional)")]
        [DataType(DataType.Date)]
        public DateTime? FDP { get; set; }


        //breve descripcion del ultimo diagnostico o tratamiento en curso - opcional
        [Display(Name = "Descripción del último diagnóstico o tratamiento que posee (Opcional)")]
        public string Descripcion { get; set; }


        //asistencias virtual, presencial, hibrido
        [Display(Name = "Modo de asistencia a consultas")]
        [Required(ErrorMessage = "El campo {0} es requerido.")]
        public string Asistencia { get; set; }

        public int CompareTo(Pacientes otro)
        {
            if (otro == null)
            {
                return 0;
            }
            else
            {
                return this.Nombre.CompareTo(otro.Nombre);
            }
        }
    }
}
