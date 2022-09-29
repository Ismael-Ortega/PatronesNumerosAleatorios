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

            Console.Write("Numero de numeros aleatorios (m) vale: " + m + "\n");

            //metemos el primer numero que es 6 (nosotros elegimos este numero arbitrariamente)
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
                ri = item / (m - 1);
                listRi.Add(ri);
                suma += ri;
                //Console.Write("Agregue el numero: " + ri + " a la lista\n");
            }


            //for que muestra los numeros generados en xi
            /* Console.WriteLine("Los numeros generados son: ");
            foreach (int j in list)
            {
                Console.WriteLine("x indice "+list.IndexOf(j)+" con valor: "+j+"\n");
            } */

            //for que muestra los numeros generados en ri
            //Console.WriteLine("Los numeros ri son: ");
            foreach (double j in listRi)
            {
                //Console.WriteLine("ri indice " + listRi.IndexOf(j) + " con valor: " + j + "\n");
            }

            /**********MOSTRAMOS LOS RESULTADOS *******/
            Console.Write("/****************** RESULTADOS *******************/\n");
            //metodo que busca repeticiones
            var result = list.GroupBy(x => x)
                            .Where(g => g.Count() > 1)
                            .Select(x => new { Element = x.Key, Count = x.Count() })
                            .ToList();


            if (result.Count == 0)
            {
                Console.WriteLine("No hay repeticiones en la lista de numeros pseudoaleatorios\n");
            }
            else
            {
                Console.WriteLine("Hay repeticiones el metodo no tiene numeros pseudoaleatorios\n");
                Console.WriteLine(String.Join(", ", result));
            }

            /*********PRUEBA DE MEDIAS**************/
            //obtener promedio
            Console.WriteLine("1.- PRUEBA DE MEDIAS");

            double promedioRi = suma / listRi.Count;
            Console.WriteLine("El promedio es: " + promedioRi);

            /**********LI Y LS**************/
            double li = 0;
            double ls = 0;

            li = (0.5) - (1.96) * (1 / Math.Sqrt(12 * m));
            ls = (0.5) + (1.96) * (1 / Math.Sqrt(12 * m));

            if (promedioRi > li && promedioRi < ls)
            {
                Console.WriteLine("El promedio esta dentro del intervalo");
                Console.WriteLine("Li: " + li + " Ls: " + ls);
                Console.WriteLine("Se acepta la hipotesis nula (H0) en la prueba de medias");
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine("El promedio esta fuera del intervalo");
                Console.WriteLine("El metodo no es aceptado");
            }

            /*********PRUEBA DE VARIANZA**************/
            Console.WriteLine("2.- PRUEBA DE VARIANZA");
            double Sumavarianza = 0;

            foreach (double j in listRi)
            {
                Sumavarianza += Math.Pow((j - promedioRi), 2);
            }

            double varianzaRi = Sumavarianza / (listRi.Count - 1);
            Console.WriteLine("La varianza es: " + varianzaRi);

            //calculamos los limites de aceptacion
            double liVarianza = 0;
            double lsVarianza = 1;

            //calculamos el valor de chiCuadrada con alpha = 0.05 y m = 8192
            double chiCuadrada = 0.9;

            if (varianzaRi > liVarianza && varianzaRi < lsVarianza)
            {
                Console.WriteLine("La varianza esta dentro del intervalo");
                Console.WriteLine("Li: " + liVarianza + " Ls: " + lsVarianza);
                Console.WriteLine("Se acepta la hipotesis nula (H0)\n");
            }
            /**********PRUEBA DE UNIFORMIDAD**************/
            Console.WriteLine("3.- PRUEBA DE UNIFORMIDAD");
            Console.WriteLine("PRUEBA DE CHI CUADRADA");
            /**********PRUEBA DE CHI CUADRADA ************/
            /***** Clasificamos los numeros aleatorios*********/
            //obtenemos m2
            double n = m;
            double m2 = Math.Ceiling(Math.Sqrt(n));
            Console.Write("m2 vale: " + m2 + "\n");

            //obtenemos el ancho de clase
            double anchoClase = Math.Round(1 / m2, 4);
            Console.Write("El ancho de clase es: " + anchoClase + "\n");

            //ahora clasificamos los numeros aleatorios por rango de valores
            //creamos una lista de listas
            List<List<double>> listaFrecuenciaObservada = new List<List<double>>();
            //creamos una lista auxiliar
            List<double> listaAux = new List<double>();


            //clasificamos los numeros m2 veces
            for (int i = 0; i < m2; i++)
            {
                for (int j = 0; j < listRi.Count; j++)
                {
                    if (listRi.ElementAt(j) >= (i * anchoClase) && listRi.ElementAt(j) < ((i + 1) * anchoClase))
                    {
                        listaAux.Add(listRi.ElementAt(j));
                    }
                }
                listaFrecuenciaObservada.Add(listaAux);
                listaAux = new List<double>();
            }
            //borramos la lista auxiliar puesto que ya no se usara
            listaAux = new List<double>();

            //mostramos el numero de valores en cada lista
            /* Console.Write("El numero de valores en cada lista es: \n");
            for (int i = 0; i < listaFrecuenciaObservada.Count; i++)
            {
                Console.Write("Lista " + i + " con rango de " + i * anchoClase + " al " + (i + 1) * anchoClase + " tiene " + listaFrecuenciaObservada.ElementAt(i).Count + " valores\n");
            } */

            //calculamos la frecuencia esperada
            double frecuenciaEsperada = m / m2;

            //creamos una lista para guardar los valores de la ecuacion
            List<double> listaEcuacionChiCuadrada = new List<double>();
            double valorEcuacion = 0;
            //calculamos los valores de la ecuacion
            for (int i = 0; i < listaFrecuenciaObservada.Count; i++)
            {
                valorEcuacion = Math.Pow((listaFrecuenciaObservada.ElementAt(i).Count - frecuenciaEsperada), 2) / frecuenciaEsperada;
                listaEcuacionChiCuadrada.Add(valorEcuacion);
            }

            //sumamos los valores de la ecuacion
            double sumaEcuacion = 0;
            foreach (double item in listaEcuacionChiCuadrada)
            {
                sumaEcuacion += item;

            }

            //calculamos el valor de chi cuadrada

            //si el valor de chi cuadrada es mayor que el valor de la sumatoria de la ecuacion
            //entonces se acepta la hipotesis nula
            if (chiCuadrada > sumaEcuacion)
            {
                Console.WriteLine("El valor de chi cuadrada (" + chiCuadrada + ") es mayor que el valor de la sumatoria de la ecuacion (" + sumaEcuacion + ")");
                Console.WriteLine("Se acepta la hipotesis nula\n");
            }
            else
            {
                Console.WriteLine("El valor de chi cuadrada (" + chiCuadrada + ") es menor que el valor de la sumatoria de la ecuacion (" + sumaEcuacion + ")");
                Console.WriteLine("Se rechaza la hipotesis nula\n");
            }
            /***** PRUEBAS DE INDEPENDENCIA******/

            /**PRUEBA DE CORRIDAS ARRIBA Y ABAJO DE LA MEDIA *******/
            Console.WriteLine("4.- PRUEBA DE INDEPENDENCIA");
            Console.WriteLine("PRUEBA DE CORRIDAS ARRIBA Y ABAJO DE LA MEDIA");
            //revisamos los numeros Ri
            List<double> listaConjuntoS = new List<double>();
            for (int i = 0; i < listRi.Count; i++)
            {
                if (listRi.ElementAt(i) < 0.5)
                {
                    //agregamos un cero a la lista
                    listaConjuntoS.Add(0);
                }
                else
                {
                    //agregamos un uno
                    listaConjuntoS.Add(1);
                }
            }

            //contamos las corridas
            int corridas = 0;
            for (int i = 0; i < listaConjuntoS.Count; i++)
            {
                //si no se sale del rango, hacemos la validacion, y si no terminamos el ciclo
                if ((i + 1) != listaConjuntoS.Count)
                {
                    if (listaConjuntoS.ElementAt(i) != listaConjuntoS.ElementAt(i + 1))
                    {
                        corridas++;
                    }
                }


            }

            //calculamos n0 (cantidad de ceros)
            double n0 = 0;
            for (int i = 0; i < listaConjuntoS.Count; i++)
            {
                if (listaConjuntoS.ElementAt(i) == 0)
                {
                    n0++;
                }
            }
            //calculamos n1 (cantidad de unos)
            double n1 = 0;
            for (int i = 0; i < listaConjuntoS.Count; i++)
            {
                if (listaConjuntoS.ElementAt(i) == 1)
                {
                    n1++;
                }
            }

            //promedio de las corridas observadas
            double promedioCorridas = (2 * n0 * n1) / (n0 + n1) + 0.5;

            //varianza de las corridas observadas
            double varianzaCorridas = (2 * n0 * n1) * ((2 * n0 * n1) - (n0 + n1)) / Math.Pow((n0 + n1), 2) * (n0 + n1 - 1);

            //valor de z0
            double z0 = (corridas - promedioCorridas) / Math.Sqrt(varianzaCorridas);

            //valor de z0 con 5% de significancia
            double z0_5 = 1.96;

            //hacemos la validacion para revisar si la prueba es aceptada o no
            if (-z0_5 < z0 && z0 < z0_5)
            {
                Console.Write("La prueba de corridas arriba y abajo de la media es aceptada\n");
            }
            else
            {
                Console.Write("La prueba de corridas arriba y abajo de la media es rechazada\n");
            }

            /***********CALCULAMOS LA MEDIANA DE LOS VALORES DE LA LISTA RI*************/
            Console.WriteLine("5.- CALCULAMOS LA MEDIANA DE LOS VALORES DE LA LISTA RI");
            //guardamos los valores de la lista Ri en una lista auxiliar
            List<double> listaAuxiliar = new List<double>();
            foreach (double item in listRi)
            {
                listaAuxiliar.Add(item);
            }
            //ordenamos la lista auxiliar
            listaAuxiliar.Sort();
            //calculamos la mediana
            double mediana = 0;
            if (listaAuxiliar.Count % 2 == 0)
            {
                //si la cantidad de elementos es par
                mediana = (listaAuxiliar.ElementAt(listaAuxiliar.Count / 2) + listaAuxiliar.ElementAt((listaAuxiliar.Count / 2) - 1)) / 2;
            }
            else
            {
                //si la cantidad de elementos es impar
                mediana = listaAuxiliar.ElementAt(listaAuxiliar.Count / 2);
            }

            Console.WriteLine("La mediana de los valores de la lista Ri es: " + mediana);
            Console.WriteLine("La media de los valores RI es: " + promedioRi);
            Console.WriteLine("La varianza de los valores RI es: " + varianzaRi);
            double desviacionEstandarRi = Math.Sqrt(varianzaRi);
            Console.WriteLine("La desviacion estandar de los valores RI es: " + desviacionEstandarRi);


        }
    }
}