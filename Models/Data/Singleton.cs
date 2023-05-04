namespace PROYECTO_ED1.Models.Data
{
    public class Singleton // para garantizar que una clase tenga solo una instancia y proporcionar un punto de acceso global a dicha instancia
    {
        private static Singleton _instance = null; //_instance para almacenar la unica instancia de la clase, se evita que se creen instancia adicionales de la clase desde otros lugares del codigo siendo la clase privada
                                                   //public Clases.AVL<Pacientes> AVL = new Clases.AVL<Pacientes>();

        public static Singleton Instance // proporciona un punto de acceso global a esta instancia
        {
            get
            { //get comprueba si la instancia ya existe, si no existe se crea una nueva instancia utilizando el constructor privado de la clase y se le asigna a _instance si ya existe una instancia se devuelve la instancia existente.
                if (_instance == null) _instance = new Singleton();
                return _instance;
            } //garantiza que solo se cree una instancia de la clase singleton y que se pueda acceder a ella desde cualquier lugar del codigo usando Instance
        }

        // Arbol AVL y objeto Paciente para mostrar 
        public int bandera;
        public Pacientes AuxP = new Pacientes();
        public Clases.AVL<Pacientes> miAVL = new Clases.AVL<Pacientes>();
        public Clases.ListaPacientes ListaPacientes = new Clases.ListaPacientes();
        //public List<Consulta> Consultas = new List<Consulta>();





    }
}

