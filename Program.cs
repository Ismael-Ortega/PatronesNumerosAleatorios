namespace programaNumerosAleatoriosV2
{
    class Program
    {
        static void Main(string[] args)
        {
            //Lista que generara numeros aleatorios
            List<int> list = new List<int>() /* { 5, -1, 8, 4, 5, 9, -1, 8 } */;

            //Lista que genera los valores de ri
            List<double> listRi = new List<double>();

            float x0 = 6;
            double xi = 0;
            float k = 3;
            float g = 13;
            float c = 8191;
            float a = 1 + 4 * k;
            double m = Math.Pow(2, g);


            double ri = 0;

            //prueba de medias
            double suma = 0;

            /*
            m = 2^g
            a = 1+4k
            k es un numero entero
            c relativamente primo a m
            g debe ser entero
            */

            Console.Write("m vale: " + m + "\n");

            //metemos el primer numero que es 6
            list.Add((int)x0);

            //for que genera los numeros aleatorios
            for (int i = 1; i < m; i++)
            {
                xi = (a * list.ElementAt(i - 1) + c) % m;
                list.Add((int)xi);
                //Console.Write("Agregue el numero: " + xi + " a la lista\n");
            }

            //for que genera los numeros ri
            foreach (int item in list)
            {
                ri = item / (m-1);
                listRi.Add(ri);
                suma += ri;
                //Console.Write("Agregue el numero: " + ri + " a la lista\n");
            }
            
            //for que muestra los numeros generados en xi
            Console.WriteLine("Los numeros generados son: ");
            foreach (int j in list)
            {
                Console.WriteLine("x indice "+list.IndexOf(j)+" con valor: "+j+"\n");
            }

            //for que muestra los numeros generados en ri
            Console.WriteLine("Los numeros ri son: ");
            foreach (double j in listRi)
            {
                Console.WriteLine("ri indice " + listRi.IndexOf(j) + " con valor: " + j + "\n");
            }

            //metodo que busca repeticiones
            var result = list.GroupBy(x => x)
                            .Where(g => g.Count() > 1)
                            .Select(x => new { Element = x.Key, Count = x.Count() })
                            .ToList();

            if (result.Count == 0)
            {
                Console.WriteLine("No hay repeticiones");
            }
            else
            {
                Console.WriteLine("Hay repeticiones el metodo no tiene numeros pseudoaleatorios\n");
                Console.WriteLine(String.Join(", ", result));
            }

            /*********PRUEBA DE MEDIAS**************/
            //obtener promedio

            double promedio = suma / listRi.Count;
            Console.Write("El promedio es: " + promedio + "\n");

            /**********LI Y LS**************/
            double li = 0;
            double ls = 0;

            li = (0.5)-(1.96)*(1/Math.Sqrt(12*m));
            ls = (0.5)+(1.96)*(1/Math.Sqrt(12*m));

            if(promedio>li && promedio<ls)
            {
                Console.Write("El promedio esta dentro del intervalo\n");
            }
            else
            {
                Console.Write("El promedio esta fuera del intervalo\n");
            }
        }
    }
}