using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Clases
{
    public abstract class IDG<T> where T : IComparable<T>//declaracion de la clase abstracta IDG que acepta un tipo generico T que debe implementar la interfaz Icomparable<T>
    {
        protected abstract Nodo<T> Insert(Nodo<T> nodo, T value);//declaracion de un metodo abstracto protegido llamado insert toma dos argumentos un objeto nodo<T> llamado nodo y un objeto generico T llamado value el metodo retorna un objeto Nodo<T>
        protected abstract Nodo<T> Delete(Nodo<T> nodo);//declaración de un metodo abstracto protegido llamado Delete que toma un argumento objeto Nodo<T> llamado nodo el método no retorna ningún valor.
        protected abstract Nodo<T> Get(Nodo<T> nodo, T value);//declaracion de un metodo abstracto protegido llamado Get que toma dos argumentos un objeto Nodo<T> llamado nodo y un objeto de tipo generico T llamado value. El metodo retorna un objeto Nodo<T>.
    }
}
