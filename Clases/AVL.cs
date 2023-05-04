using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public  class AVL <T> where T : IComparable<T>
    {
        private Nodo<T> Raiz = new Nodo<T>(); //nodo raiz del arbol
        private Nodo<T> temp = new Nodo<T>(); //nodo temporal, se usa para metodos
        private List<T> listaOrdenada = new List<T>(); //guarda los valores del arbol en una lista
        public int comparaciones = 0;   //guarda la cantidad de comparacion
        public int rot = 0;
        
        public void Add(T value)//metodo para agregar un nuevo valor al arbol
        {
            Insert(value);//llama el metodo de insercion
        }
        public int ObtenerFE(Nodo<T> n)//metodo para obtener factor de equilibrio de un nodo
        {

            if (n == null)
            {
                return -1;
            }
            else
            {
                return n.FE;
            }

        }
        protected void Delete(Nodo<T> nodo)//metododo para eliminar un nodo
        {
            if (nodo.Nodoizquierdo.elvalor == null && nodo.Nododerecho.elvalor == null) // Caso 1, el nodo no tiene hijos
            {
                nodo.elvalor = nodo.Nododerecho.elvalor; //se pone null el nodo aka se elimina
            }
            else if (nodo.Nododerecho.elvalor == null) // Caso 2, el nodo tiene solamente un hijo
            {
                nodo.elvalor = nodo.Nodoizquierdo.elvalor;//el hijo izquierdo pasa a la posicion del nodo a eliminar
                nodo.Nododerecho = nodo.Nodoizquierdo.Nododerecho; 
                nodo.Nodoizquierdo = nodo.Nodoizquierdo.Nodoizquierdo;
            }
            else // Caso 3, el nodo tiene dos hijos
            {
                if (nodo.Nodoizquierdo.elvalor != null)//se reemplaza por el mayor de los menores
                {
                    temp = Derecha(nodo.Nodoizquierdo);
                }
                else
                {
                    temp = Derecha(nodo); // o el menor de los mayores
                }
                nodo.elvalor = temp.elvalor;
            }

        }

        public T Remove(T deleted)//metodo para eliminar un valor
        {
            Nodo<T> busc = new Nodo<T>();//se crea un nodo para buscarlo despues
            busc = Get(Raiz, deleted);//busc se asigna al nodo que tiene el valor de deleted
            if (busc != null)//si el nodo tiene algun valor
            {
                Delete(busc);//eliminar el nodo con el valor a eliminar
            }
            return deleted;
        }
        private Nodo<T> Derecha(Nodo<T> nodo)//metodo que devuelve el menor de los mayores o el mayor de los menores
        {
            if (nodo.Nododerecho.elvalor == null)//si el nodo derecho esta vacio
            {
                if (nodo.Nodoizquierdo.elvalor != null)//el nodo izquierdo tiene un valor
                {
                    return Derecha(nodo.Nodoizquierdo);//seguir  buscando el menor
                }
                else
                {
                    Nodo<T> temporal = new Nodo<T>();
                    temporal.elvalor = nodo.elvalor;
                    nodo.elvalor = nodo.Nododerecho.elvalor;
                    return temporal;
                }
            }
            else//si hay algo en el nodo derecho
            {
                return Derecha(nodo.Nododerecho);//seguir buscando al mayor
            }
        }
        protected Nodo<T> Get(Nodo<T> nodo, T value)//metodo para obtener un nodo
        {
            if (value.CompareTo(nodo.elvalor) == 0)//ya encontro el nodo
            {
                return nodo;
            }
            else if (value.CompareTo(nodo.elvalor) == -1)//si el nodo es menor al actual
            {
                if (nodo.Nodoizquierdo.elvalor == null)//el nodo izquierdo esta vacio
                {
                    return null;
                }
                else
                {
                    return Get(nodo.Nodoizquierdo, value);//devuelve el nodo izquierdo
                }
            }
            else
            {
                if (nodo.Nododerecho.elvalor == null)//el nodo derecho esta vacio
                {
                    return null;
                }
                else
                {
                    return Get(nodo.Nododerecho, value);//devuelve el nodo derecho
                }
            }
        }
        public Nodo<T> InsertarAVL(Nodo<T> nodo, Nodo<T> tempo) //Se inserta el elvalor en el arbol y se verifico si está ordenado
        {
            try
            {
                Nodo<T> nuevoNodo = tempo;

                if (nodo.elvalor.CompareTo(tempo.elvalor) == -1)//si el nodo es menor al actual
                {
                    if (tempo.Nodoizquierdo.elvalor == null)//si el hijo izquerdo esta vacio
                    {
                        tempo.Nodoizquierdo = nodo;//se asigna el nuevo nodo en el hijo izquierdo

                    }
                    else
                    {

                        tempo.Nodoizquierdo = InsertarAVL(nodo, tempo.Nodoizquierdo);//verificar si se puede insertar en el hijo izquierdo

                        if ((ObtenerFE(tempo.Nodoizquierdo) - ObtenerFE(tempo.Nododerecho) == 2))//si el Factor de equilibrio es 2
                        {
                            if (nodo.elvalor.CompareTo(tempo.Nodoizquierdo.elvalor) == -1)//si el el factor de equilibrio es -1
                            {
                                nuevoNodo = RotIzq(tempo);//se hace una rotacion izquierda
                            }
                            else
                            {
                                nuevoNodo = RotDIzq(tempo);//se hace una doble rotacion izquierda
                            }
                        }

                    }
                }
                else if (nodo.elvalor.CompareTo(tempo.elvalor) == 1)//si el nodo a insertar es mayor al actual
                {

                    if (tempo.Nododerecho.elvalor == null)//si el hijo derecho esta vacio
                    {
                        tempo.Nododerecho = nodo;//se asigna el nuevo nodo en el hijo derecho
                    }
                    else
                    {


                        tempo.Nododerecho = InsertarAVL(nodo, tempo.Nododerecho);//se revisa si se puede insertar en el hijo derecho
                        if ((ObtenerFE(tempo.Nododerecho) - ObtenerFE(tempo.Nodoizquierdo) == 2))//si el factor de equilibrio es 2
                        {
                            if (nodo.elvalor.CompareTo(tempo.Nododerecho.elvalor) == 1)//si el factor de equilibrio es -1
                            {
                                nuevoNodo = RotDer(tempo);//se hace una rotacion derecha
                            }
                            else
                            {
                                nuevoNodo = RotDDER(tempo);//se hace una doble rotacion derecha
                            }
                        }

                    }
                }
                //altura
                if ((tempo.Nodoizquierdo == null) && (tempo.Nododerecho != null))//si solo tiene un hijo derecho
                {
                    tempo.FE = tempo.Nododerecho.FE + 1;//se le agrega 1 al FE 
                }
                else if ((tempo.Nodoizquierdo != null) && (tempo.Nododerecho == null))// si solo tiene hijo izquierdo
                {
                    tempo.FE = tempo.Nodoizquierdo.FE + 1;//se le agrega 1 al FE
                }
                else
                {
                    tempo.FE = Math.Max(ObtenerFE(tempo.Nodoizquierdo), ObtenerFE(nodo.Nododerecho)) + 1;//se le suma uno al FE mas alto
                }
                return nuevoNodo;
            }
            catch
            {
                throw;
            }
        }
        public Nodo<T> CrearNodoAVL(T elvalor)//metodo para creat un nuevo nodo con un valor
        {
            Nodo<T> nodo = new Nodo<T>();//se crea un nodo
            nodo.elvalor = elvalor;//se le asignan los valores
            nodo.FE = 0;
            nodo.Nodoizquierdo = new Nodo<T>();
            nodo.Nododerecho = new Nodo<T>();
            return nodo;

        }
        public void Insert(T value)//metodo para insertar un valor al arbol
        {
            try
            {
                Nodo<T> nuevo = CrearNodoAVL(value);//se crea el nodo con el valor a isertar

                if (Raiz.elvalor == null)//si la raiz esta vacia
                {
                    Raiz = nuevo;  //el nodo se convierte en la raiz
                }
                else
                {
                    Raiz = InsertarAVL(nuevo, Raiz);//se inserta el nodo al arbol

                }


            }
            catch
            {
                throw;
            }
        }
        protected Nodo<T> Insert(Nodo<T> nodo, T value)//metodo para insertar nodo al arbol
        {
            try
            {
                Nodo<T> nuevo = CrearNodoAVL(value);//se crea el nodo a insertar

                if (nodo == null)//si el nodo esta vacio
                {
                    nodo = nuevo;//se inserta en el nodo
                }
                else
                {
                    nodo = InsertarAVL(nuevo, nodo);//se revisa si se puede insertar en uno de los dos hijos

                }
                return nodo;

            }
            catch
            {
                throw;
            }
        }
        public Nodo<T> RotIzq(Nodo<T> nodo)//rotacion izquierda
        {
            rot++;
            Nodo<T> aux = nodo.Nodoizquierdo;

            nodo.Nodoizquierdo = aux.Nododerecho;
            aux.Nododerecho = nodo;
            nodo.FE = Math.Max(ObtenerFE(nodo.Nodoizquierdo), ObtenerFE(nodo.Nododerecho)) + 1;//Devuelve el mayor
            aux.FE = Math.Max(ObtenerFE(aux.Nodoizquierdo), ObtenerFE(aux.Nododerecho)) + 1;// Devuelve el mayor
            return aux;
        }

        public Nodo<T> RotDer(Nodo<T> nodo)//rotacion derecha
        {
            rot++;
            Nodo<T> aux = nodo.Nododerecho;
            nodo.Nododerecho = aux.Nodoizquierdo;
            aux.Nodoizquierdo = nodo;
            nodo.FE = Math.Max(ObtenerFE(nodo.Nodoizquierdo), ObtenerFE(nodo.Nododerecho)) + 1;//Devuelve el mayor
            aux.FE = Math.Max(ObtenerFE(aux.Nodoizquierdo), ObtenerFE(aux.Nododerecho)) + 1;// Devuelve el mayor
            return aux;
        }

        public Nodo<T> RotDIzq(Nodo<T> nodo)// Rotación Doble Izquierda
        {
            rot++;
            Nodo<T> tem = new Nodo<T>();
            nodo.Nodoizquierdo = RotDer(nodo.Nodoizquierdo);
            tem = RotIzq(nodo);
            return tem;

        }

        public Nodo<T> RotDDER(Nodo<T> nodo)// Rotación Doble Nododerecho
        {
            rot++;
            Nodo<T> tem = new Nodo<T>();
            nodo.Nododerecho = RotIzq(nodo.Nododerecho);
            tem = RotDer(nodo);
            return tem;

        }

        private void InOrder(Nodo<T> nodo)//metodo que agrega los valores a la lista ordenada
        {//recorre el arbol en InOrder y va agregando los valores uno por uno
            if (nodo.elvalor != null)
            {
                InOrder(nodo.Nodoizquierdo);
                listaOrdenada.Add(nodo.elvalor);
                InOrder(nodo.Nododerecho);
            }
        }

        public List<T> ObtenerLista()//metodo para obtener la lista ordenada
        {
            listaOrdenada.Clear();//se limpian la lista
            InOrder(Raiz);//se agregan los valores a la lista
            return listaOrdenada;
        }

        public List<T> Obtener(Func<T, bool> Predicate)//metodo para obtener ciertos datos de la lista ordenada
        {
            List<T> prov = new List<T>();
            comparaciones = 0;
            ObtenerLista();
            for (int i = 0; i < listaOrdenada.Count(); i++)
            {
                if (Predicate(listaOrdenada[i]))
                {
                    comparaciones = i;
                    prov.Add(listaOrdenada[i]);
                }
            }
            return prov;
        }

        public int GetComparaciones()//metodo que devuelve la cantidad de comparaciones que se hicieron
        {
            return comparaciones;
        }

        public int GetRotacion()//metodo que devuelve la cantidad de rotaciones
        {
            return rot;
        }
        public void Balance()
        {
            Nodo<T> aux = Raiz;
        }
        public int ObtenerProfundidad()//metodo para obtener la altura del arbol
        {
            return ObtenerAltura(Raiz);
        }

        private int ObtenerAltura(Nodo<T> nodo)//metodo para obtener la altura de un nodo 
        {
            if (nodo == null)
            {
                return -1;
            }
            else
            {
                int izquierda = ObtenerAltura(nodo.Nodoizquierdo);
                int derecha = ObtenerAltura(nodo.Nododerecho);
                return Math.Max(izquierda, derecha) + 1;
            }
        }
    }
}
