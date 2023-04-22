using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public class ListaPacientes
    {
        private class Nodo_lista
        {
            //el numero de personas atendidas en un dia
            public int numeroatencion = 0;
            public DateTime fechaatencion;
            public string DNI; //variable para verificar dpi
            public Nodo_lista siguiente;
        }

        Nodo_lista head = new Nodo_lista();

        public ListaPacientes()
        {
            head = null;
        }

        public void add(DateTime fecha)//declaracion del metodo add que toma un argumento de tipo datetime llamado fecha
        {
            Nodo_lista newnodo = new Nodo_lista();//creacion de una nueva instancia de la clase nodolista llamada newnodo
            Nodo_lista aux = head; // inicializacion de la variable auxiliar con el valor de la variable head (cabeza de la lista enlazada)

            newnodo.fechaatencion = fecha;//asignacion del valor del argumento fecha al campo fechaatencion de newnodo
            newnodo.siguiente = null; // estableciendo el puntero siguiente de newnodo a null que indica que actualmente no apunta a ningun nodo
            if (head == null)//comprobamos de que si la lista esta vacia
            {
                head = newnodo;//asignacion de newnodo a la variable head estableciendo newnodo como el primer elemento de la lista enlazada
                return;//salida inmediata del metodo insertamos el nuevo nodo en la lista vacia
            }
            else //si la lista no esta vacia
            {
                while(aux.siguiente != null)//mientras no sea null
                {
                    aux = aux.siguiente; // avanza al siguiente nodo en la lista actualizando el valor de aux con el puntero siguiente del nodo actual
                }
                aux.siguiente = newnodo; //una vez se encuentre el ultimo nodo de la lista se establece el puntero siguiente al newnodo añadiendo asi newnodo al final de la lista enlazada 
            }

        }

        public void adddpi(string dni)
        {
            Nodo_lista newnodo = new Nodo_lista();
            Nodo_lista aux = head;

            newnodo.DNI = dni;
            newnodo.siguiente = null;

            if(head == null)
            {
                head = newnodo;
                return;
            }
            else
            {
                while(aux.siguiente != null)
                {
                    aux = aux.siguiente;
                }
                aux.siguiente = newnodo;
            }
        }

        public int GetDPI(string dpi)//declaracion del metodo getdpi que toma un argumento de tipo string llamado dpi
        {
            Nodo_lista newNodo = new Nodo_lista();//creacion de una nueva instancia de la clase nodolista llamada newnodo
            Nodo_lista aux = head;//inicializacion de la variable auxiliar aux con el valor de la variable head
            int cont = 0;
            while (aux != null)       //Recorro la lista con el while
            {
                if (aux.DNI == dpi)  //Si la fecha del nodo es igual a la fecha de interés
                {
                    cont++;             //El contador se sumará en 1
                }

                aux = aux.siguiente;
            }
            return cont; //Retorno cont, si cont es mayor a 3, se hará la excepción 
        }


        public int GetDay(DateTime fecha)
        {
            Nodo_lista newNodo = new Nodo_lista();
            Nodo_lista aux = head;
            int cont = 0;
            while (aux != null)       //Recorro la lista con el while
            {
                if (aux.fechaatencion == fecha)  //Si la fecha del nodo es igual a la fecha de interés
                {
                    cont++;             //El contador se sumará en 1
                }

                aux = aux.siguiente;
            }
            return cont; //Retorno cont, si cont es mayor a 3, se hará la excepción 
        }

    }
}
