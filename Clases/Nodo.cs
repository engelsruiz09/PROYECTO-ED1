namespace Clases
{
    public class Nodo<T> where T : IComparable<T>
    {
        public Nodo<T> Nodoizquierdo { get; set; } 
        public Nodo<T> Nododerecho { get; set; }
        public T elvalor { get; set; }

        public int FE { get; set; }
        //FE = altura subarbol derecho - altura subarbolizquierdo por definicio para un arbol AVL este valor debe ser -1, 0 , 1 si el factor equilibrio de un nodo es 0 -> el nodo esta equilibrado y sus subarboles tienen exactamente la misma altura.
    }
}