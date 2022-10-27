using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //variables a crear nuestro archivo de registro
            StreamWriter log;
            log = new StreamWriter("C:/Users/ormoj/Documents/Semestre 5/Simulacion/programaNumerosAleatoriosV2/file.log");
            log.AutoFlush = true;

            //algoritmo lineal para generar los numeros aleatorios

            float x0 = 6;
            double xi = 0;
            float k = 15;
            float g = 13;
            float c = 8191;
            float a = 1 + 4 * k;
            double m = Math.Pow(2, g);
            double ri = 0;

            //pedimos datos al usuario


            //prueba de medias
            double suma = 0;

            //para chi cuadrada
            double gradosDeLibertad = 0;
            double chiCuadrada = 0;
            /*
            
            m = 2^g
            a = 1+4k
            k es un numero entero
            c relativamente primo a m
            g debe ser entero
            
            */

            Console.Write("Numero de numeros aleatorios (m) vale: " + m + "\n");
            log.WriteLine("Parametros:");
            log.WriteLine("m = " + m);
            log.WriteLine("a = " + a);
            log.WriteLine("c = " + c);
            log.WriteLine("g = " + g);
            log.WriteLine("k = " + k);
            log.WriteLine("Semilla x0 = " + x0);
            log.WriteLine("\nNumeros aleatorios generados: \n");
            //metemos el primer numero que es 6 (nosotros elegimos este numero arbitrariamente)
            list.Add((int)x0);

            //for que genera los numeros aleatorios
            for (int i = 1; i < m; i++)
            {
                xi = (a * list.ElementAt(i - 1) + c) % m;
                list.Add((int)xi);
                //Console.Write("Agregue el numero: " + xi + " a la lista\n");
            }
            Console.WriteLine("Numeros pesudoaleatorios generados exitosamente");
            //for que genera los numeros ri
            foreach (int item in list)
            {
                ri = item / (m - 1);
                listRi.Add(ri);
                suma += ri;
                //Console.Write("Agregue el numero: " + ri + " a la lista\n");
            }


            //for que muestra los numeros generados en xi
            //Console.WriteLine("Los numeros generados son: ");
            foreach (int j in list)
            {
                //log.WriteLine("x indice "+list.IndexOf(j)+" con valor: "+j+"\n");
            }
            //Console.WriteLine("x indice " + list.ElementAt(0) + " con valor: " + 0 + "\n");
            //Console.WriteLine("x indice " + list.ElementAt(1) + " con valor: " + 1 + "\n");


            //for que muestra los numeros generados en ri
            //Console.WriteLine("Los numeros ri son: ");
            foreach (double j in listRi)
            {
                log.WriteLine("ri indice " + listRi.IndexOf(j) + " con valor: " + j + "\n");
            }
            //Console.WriteLine("ri indice " + listRi.ElementAt(0) + " con valor: " + 0 + "\n");
            //Console.WriteLine("ri indice " + listRi.ElementAt(1) + " con valor: " + 1 + "\n");


            /**********MOSTRAMOS LOS RESULTADOS *******/
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
            log.WriteLine("1.- PRUEBA DE MEDIAS");

            double promedioRi = suma / listRi.Count;
            log.WriteLine("El promedio es: " + promedioRi);

            /**********LI Y LS**************/
            double li = 0;
            double ls = 0;

            li = (0.5) - (1.96) * (1 / Math.Sqrt(12 * m));
            ls = (0.5) + (1.96) * (1 / Math.Sqrt(12 * m));

            if (promedioRi > li && promedioRi < ls)
            {
                log.WriteLine("El promedio esta dentro del intervalo " + li + " < " + promedioRi + " < " + ls);
                log.WriteLine("Li: " + li + " Ls: " + ls);
                log.WriteLine("Se acepta la hipotesis nula (H0) en la prueba de medias\n");
                //Console.WriteLine("El promedio esta dentro del intervalo " + li + " < " + promedioRi + " < " + ls);
            }
            else
            {
                log.WriteLine("El promedio esta fuera del intervalo");
                log.WriteLine("El metodo no es aceptado");
            }

            /*********PRUEBA DE VARIANZA**************/
            Console.WriteLine("2.- PRUEBA DE VARIANZA");
            log.WriteLine("2.- PRUEBA DE VARIANZA");
            double Sumavarianza = 0;

            foreach (double j in listRi)
            {
                Sumavarianza += Math.Pow((j - promedioRi), 2);
            }

            double varianzaRi = Sumavarianza / (listRi.Count - 1);
            log.WriteLine("La varianza es: " + varianzaRi);

            //calculamos los limites de aceptacion
            double liVarianza = 0;
            double lsVarianza = 1;

            //calculamos el valor de chiCuadrada con alpha = 0.05 y m = 8192
            //grados de libertad
            gradosDeLibertad = m - 1;
            chiCuadrada = 0.5 * Math.Pow(1.96 + Math.Sqrt(2 * gradosDeLibertad - 1), 2);

            if (varianzaRi > liVarianza && varianzaRi < lsVarianza)
            {
                log.WriteLine("La varianza esta dentro del intervalo " + liVarianza + " < " + varianzaRi + " < " + lsVarianza);
                log.WriteLine("Se acepta la hipotesis nula (H0) en la prueba de varianza\n");
            }
            /**********PRUEBA DE UNIFORMIDAD**************/
            Console.WriteLine("3.- PRUEBA DE UNIFORMIDAD");
            Console.WriteLine("PRUEBA DE CHI CUADRADA");
            log.WriteLine("3.- PRUEBA DE UNIFORMIDAD");
            log.WriteLine("PRUEBA DE CHI CUADRADA");
            /**********PRUEBA DE CHI CUADRADA ************/
            /***** Clasificamos los numeros aleatorios*********/
            //obtenemos m2
            double n = m;
            double m2 = Math.Ceiling(Math.Sqrt(n));
            log.Write("m2 vale: " + m2 + "\n");

            //obtenemos el ancho de clase
            double anchoClase = Math.Round(1 / m2, 4);
            log.Write("El ancho de clase es: " + anchoClase + "\n");

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
            gradosDeLibertad = m2 - 1;
            chiCuadrada = 0.5 * Math.Pow(1.96 + Math.Sqrt(2 * gradosDeLibertad - 1), 2);

            //si el valor de chi cuadrada es mayor que el valor de la sumatoria de la ecuacion
            //entonces se acepta la hipotesis nula
            if (chiCuadrada > sumaEcuacion)
            {
                log.WriteLine("El valor de chi cuadrada (" + chiCuadrada + ") es mayor que el valor de la sumatoria de la ecuacion (" + sumaEcuacion + ")");
                log.WriteLine("Se acepta la hipotesis nula (H0) en la prueba de uniformidad\n");
            }
            else
            {
                log.WriteLine("El valor de chi cuadrada (" + chiCuadrada + ") es menor que el valor de la sumatoria de la ecuacion (" + sumaEcuacion + ")");
                log.WriteLine("Se rechaza la hipotesis nula\n");
            }
            /***** PRUEBAS DE INDEPENDENCIA******/

            /**PRUEBA DE CORRIDAS ARRIBA Y ABAJO DE LA MEDIA *******/
            Console.WriteLine("4.- PRUEBA DE INDEPENDENCIA");
            Console.WriteLine("PRUEBA DE CORRIDAS ARRIBA Y ABAJO DE LA MEDIA");
            log.WriteLine("4.- PRUEBA DE INDEPENDENCIA");
            log.WriteLine("PRUEBA DE CORRIDAS ARRIBA Y ABAJO DE LA MEDIA");

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
            /* int corridas = 0;
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
            } */

            //calculamos n0 (cantidad de ceros)
            double n0 = 0;
            for (int i = 0; i < listaConjuntoS.Count; i++)
            {
                if (listaConjuntoS.ElementAt(i) == 0)
                {
                    n0++;
                }
            }
            //el numero de corridas es el numero de ceros
            double corridas = n0;
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
            double promedioCorridas = ((2 * n0 * n1) / (n0 + n1)) + 0.5;

            //varianza de las corridas observadas
            double varianzaCorridas = (2 * n0 * n1) * ((2 * n0 * n1) - (n0 + n1)) / Math.Pow((n0 + n1), 2) * (n0 + n1 - 1);

            //valor de z0
            double z0 = (corridas - promedioCorridas) / Math.Sqrt(varianzaCorridas);

            //valor de z0 con 5% de significancia
            double z0_5 = 1.96;

            //hacemos la validacion para revisar si la prueba es aceptada o no
            if (-z0_5 < z0 && z0 < z0_5)
            {
                log.WriteLine("La prueba de corridas arriba y abajo de la media es aceptada ");
                log.WriteLine("Se acepta la hipotesis nula (H0) en la prueba de independencia\n");
            }
            else
            {
                Console.WriteLine("La prueba de corridas arriba y abajo de la media es rechazada\n");
            }

            /***********CALCULAMOS LA MEDIANA DE LOS VALORES DE LA LISTA RI*************/
            //Console.WriteLine("5.- CALCULAMOS LA MEDIANA DE LOS VALORES DE LA LISTA RI");
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


            /***********PRUEBA DE 1 DADO*************/
            //Obtenemos la prueba de dados con 1 dado y seran 10 corridas
            //creamos una funcion para la prueba de dados que reciba como parametro el numero de numeros aleatorios que ocuparas
            void dados(int cantidadCorridas)
            {
                log.WriteLine("/*********PRUEBA DE DADOS***********/");
                Console.WriteLine("\n/*********PRUEBA DE DADOS***********/");

                //int cantidadCorridas = 500;
                for (int ordenCorrida = 0; ordenCorrida < cantidadCorridas; ordenCorrida += 50)
                {
                    int rangoInf = 0;
                    int rangoSup = 0;
                    rangoInf = ordenCorrida + 1;
                    rangoSup = ordenCorrida + 50;
                    if (rangoInf == 1)
                    {
                        rangoInf = 0;
                    }

                    //Console.WriteLine("\nCorrida en el rango de " + (rangoInf) + " a " + (rangoSup));
                    log.WriteLine("\nCorrida en el rango de " + (rangoInf) + " a " + (rangoSup));
                    //obtenemos el ancho que tendra cada rango donde clasificaremos los numeros aleatorios
                    double anchoRango = 1.0 / 6.0;

                    //obtenemos la cantidad de numeros aleatorios que caeran en cada rango
                    //creamos una lista para guardar las listas que creamos para cada rango
                    List<List<double>> listaRangos = new List<List<double>>();
                    //creamos una lista auxiliar para guardar los numeros aleatorios que caen en cada rango
                    List<double> listaAuxiliarRangos = new List<double>();

                    //clasificaremos los primero 50 numeros aleatorios ri y luego de 51 al 100

                    //recorremos el dado del numero 1 al 6
                    for (int i = 0; i < 6; i++)
                    {
                        //recorremos la lista de numeros aleatorios ri
                        for (int j = rangoInf; j < rangoSup; j++)
                        {
                            //si el numero aleatorio ri cae en el rango
                            if (listRi.ElementAt(j) >= (i * anchoRango) && listRi.ElementAt(j) < ((i + 1) * anchoRango))
                            {
                                //agregamos el numero aleatorio a la lista auxiliar
                                listaAuxiliarRangos.Add(listRi.ElementAt(j));
                            }
                        }
                        //agregamos la lista auxiliar a la lista de rangos
                        listaRangos.Add(listaAuxiliarRangos);
                        //limpiamos la lista auxiliar
                        listaAuxiliarRangos = new List<double>();
                    }

                    //mostramos los numeros aleatorios que caen en cada rango
                    for (int i = 0; i < listaRangos.Count; i++)
                    {
                        //Console.WriteLine("El numero de numeros aleatorios que caen en el numero del dado " + (i + 1) + " son: " + listaRangos.ElementAt(i).Count);
                        log.WriteLine("El numero de numeros aleatorios que caen en el numero del dado " + (i + 1) + " son: " + listaRangos.ElementAt(i).Count);
                        //Listamos los numeros aleatorios que caen en cada rango
                        foreach (double item in listaRangos.ElementAt(i))
                        {
                            //Console.WriteLine("El numero aleatorio " + item + " cae en el rango del numero del dado " + (i + 1));
                            //Console.WriteLine(item);
                        }
                    }

                    /************MEDIA, MEDIANA, VARIANZA Y DESVIACION ESTANDAR*********/

                    //calculamos media de los numeros aleatorios que caen en cada rango
                    double mediaRangos = 0;
                    for (int i = 0; i < listaRangos.Count; i++)
                    {
                        mediaRangos += listaRangos.ElementAt(i).Count * (1 + i);
                    }
                    //Console.WriteLine("Numero de intervalos: " + listaRangos.Count);
                    mediaRangos = mediaRangos / 50;
                    //Console.WriteLine("La media de los numeros en el rango de " + (rangoInf) + " a " + (rangoSup) + " es: " + mediaRangos);
                    log.WriteLine("La media de los numeros en el rango de " + (rangoInf) + " a " + (rangoSup) + " es: " + mediaRangos);
                    //Obtenemos mediana
                    double medianaRangos = 0;
                    //guardamos los numeros aleatorios en donde calcularemos la mediana
                    List<double> listaAuxiliarMediana = new List<double>();
                    for (int i = 0; i < listaRangos.Count; i++)
                    {
                        for (int j = 0; j < listaRangos.ElementAt(i).Count; j++)
                        {
                            listaAuxiliarMediana.Add(listaRangos.ElementAt(i).ElementAt(j));
                        }
                    }
                    //ordenamos la lista auxiliar
                    listaAuxiliarMediana.Sort();
                    //calculamos la mediana
                    if (listaAuxiliarMediana.Count % 2 == 0)
                    {
                        //si la cantidad de elementos es par
                        medianaRangos = (listaAuxiliarMediana.ElementAt(listaAuxiliarMediana.Count / 2) + listaAuxiliarMediana.ElementAt((listaAuxiliarMediana.Count / 2) - 1)) / 2;
                    }
                    else
                    {
                        //si la cantidad de elementos es impar
                        medianaRangos = listaAuxiliarMediana.ElementAt(listaAuxiliarMediana.Count / 2);
                    }
                    //revisamos en que rango esta para determinar que numero de dado cayo
                    int numeroDado = 0;
                    for (int i = 0; i < listaRangos.Count; i++)
                    {
                        for (int j = 0; j < listaRangos.ElementAt(i).Count; j++)
                        {
                            if (medianaRangos == listaRangos.ElementAt(i).ElementAt(j))
                            {
                                numeroDado = i + 1;
                            }
                        }
                    }
                    //Console.WriteLine("La mediana de los numeros en el rango de " + (ordenCorrida + 1) + " a " + (ordenCorrida + 50) + " es: " + medianaRangos + " con numero de dado: " + numeroDado);
                    log.WriteLine("La mediana de los numeros en el rango de " + (ordenCorrida + 1) + " a " + (ordenCorrida + 50) + " es: " + medianaRangos + " con numero de dado: " + numeroDado);
                    //Calculamos moda
                    int modaRangos = 0;
                    int contadorModa = 0;
                    for (int i = 0; i < listaRangos.Count; i++)
                    {
                        if (listaRangos.ElementAt(i).Count > contadorModa)
                        {
                            contadorModa = listaRangos.ElementAt(i).Count;
                            modaRangos = i + 1;
                        }
                    }
                    //Console.WriteLine("La moda de los numeros en el rango de " + (ordenCorrida + 1) + " a " + (ordenCorrida + 50) + " es: " + modaRangos);
                    log.WriteLine("La moda de los numeros en el rango de " + (ordenCorrida + 1) + " a " + (ordenCorrida + 50) + " es: " + modaRangos);
                    //Calculamos varianza
                    double varianzaRangos = 0;
                    for (int i = 0; i < listaRangos.Count; i++)
                    {
                        varianzaRangos += Math.Pow((listaRangos.ElementAt(i).Count - mediaRangos), 2);
                    }
                    varianzaRangos = varianzaRangos / 50;
                    //Console.WriteLine("La varianza de los numeros en el rango de " + (rangoInf) + " a " + (rangoSup) + " es: " + varianzaRangos);
                    log.WriteLine("La varianza de los numeros en el rango de " + (rangoInf) + " a " + (rangoSup) + " es: " + varianzaRangos);

                    //Calculamos desviacion estandar
                    double desviacionEstandarRangos = Math.Sqrt(varianzaRangos);
                    //Console.WriteLine("La desviacion estandar de los numeros en el rango de " + (rangoInf) + " a " + (rangoSup) + " es: " + desviacionEstandarRangos);
                    log.WriteLine("La desviacion estandar de los numeros en el rango de " + (rangoInf) + " a " + (rangoSup) + " es: " + desviacionEstandarRangos);

                }

            }


            void teoriaDeColas(int numPersonas, int corridas)
            {
                log.WriteLine("\n/************Simulacion de la teoria de colas***************/");
                Console.WriteLine("\n/************Simulacion de la teoria de colas***************/");
                if (numPersonas < 3 || numPersonas > 7)
                {
                    Console.WriteLine("No existe un equipo con ese numero de personas");
                    log.WriteLine("No existe un equipo con ese numero de personas");
                    return;
                }
                //Variables iguales para todos los casos
                DateTime hora = new DateTime(2022, 10, 07, 23, 0, 0);
                DateTime horaLimite = new DateTime(2022, 10, 08, 07, 30, 0);
                DateTime horaComida = new DateTime(2022, 10, 08, 03, 00, 0);
                int i = 0;
                int camionesEnEspera = 0;
                double rCamionesEnEspera = 0;
                int corridaActual = 1;

                //Costos
                int hrsTiempoNormal = 8;
                double hrsTiempoExtra = 0;
                double hrsTiempoEspera = 0;
                double hrsAlmacen = 0;
                double costoTotal = 0;
                bool comida = false;

                int inversaCamiones(double r)
                {
                    if (r >= 0 && r < 0.5)
                    {
                        return 0;
                    }
                    else if (r >= 0.5 && r < 0.75)
                    {
                        return 1;
                    }
                    else if (r >= 0.75 && r < 0.9)
                    {
                        return 2;
                    }
                    else if (r >= 0.9 && r <= 1)
                    {
                        return 3;
                    }
                    return 0;
                }

                int inversaTiempoLlegada(double r)
                {
                    if (r >= 0 && r < 0.02)
                    {
                        return 20;
                    }
                    else if (r >= 0.02 && r < 0.1)
                    {
                        return 25;
                    }
                    else if (r >= 0.1 && r < 0.22)
                    {
                        return 30;
                    }
                    else if (r >= 0.22 && r <= 0.47)
                    {
                        return 35;
                    }
                    else if (r >= 0.47 && r <= 0.67)
                    {
                        return 40;
                    }
                    else if (r >= 0.67 && r <= 0.82)
                    {
                        return 45;
                    }
                    else if (r >= 0.82 && r <= 0.92)
                    {
                        return 50;
                    }
                    else if (r >= 0.92 && r <= 0.97)
                    {
                        return 55;
                    }
                    else if (r >= 0.97 && r <= 1)
                    {
                        return 60;
                    }
                    return 0;
                }

                switch (numPersonas)
                {
                    case 3:
                        int inversaTiempoServicioTres(double r)
                        {
                            if (r >= 0 && r < 0.05)
                            {
                                return 20;
                            }
                            else if (r >= 0.05 && r < 0.15)
                            {
                                return 25;
                            }
                            else if (r >= 0.15 && r < 0.35)
                            {
                                return 30;
                            }
                            else if (r >= 0.35 && r <= 0.6)
                            {
                                return 35;
                            }
                            else if (r >= 0.6 && r <= 0.72)
                            {
                                return 40;
                            }
                            else if (r >= 0.72 && r <= 0.82)
                            {
                                return 45;
                            }
                            else if (r >= 0.82 && r <= 0.9)
                            {
                                return 50;
                            }
                            else if (r >= 0.9 && r <= 0.96)
                            {
                                return 55;
                            }
                            else if (r >= 0.96 && r <= 1)
                            {
                                return 60;
                            }
                            return 0;
                        }
                        i = 0;
                        camionesEnEspera = 0;
                        rCamionesEnEspera = 0;
                        corridaActual = 1;

                        //Costos
                        hrsTiempoNormal = 8;
                        hrsTiempoExtra = 0;
                        hrsTiempoEspera = 0;
                        hrsAlmacen = 0;
                        costoTotal = 0;

                        void imprimeCosto()
                        {
                            //la hora por trabajador en tiempo normal se paga a $25 la hora por persona
                            //la hora por trabajador en tiempo extra se paga a $37.5 la hora por persona
                            //las horas en espera de los camiones se pagan a $100
                            //costo de operar el almacen por hora es de $500
                            costoTotal = ((hrsTiempoNormal * 25) * 3) + ((hrsTiempoExtra * 37.5) * 3) + (hrsTiempoEspera * 100) + (hrsAlmacen * 500);
                            log.WriteLine("Costo total: $" + Math.Round(costoTotal, 4));
                        }

                        while (corridaActual <= corridas)
                        {
                            //Comida
                            comida = false;

                            log.WriteLine("\n\nCorrida: " + corridaActual);
                            rCamionesEnEspera = listRi[i];
                            i++;
                            camionesEnEspera = inversaCamiones(rCamionesEnEspera);
                            log.WriteLine("Camiones en estado espera: " + camionesEnEspera + "\nRi:  " + rCamionesEnEspera);
                            log.WriteLine("#Ri " + "\t\tH.Llegada" + "\t\tHora.Ent.Des" + "\t#Ri " + "\t\tT.Des" + "\tH.Salida" + "\tT.Espera");
                            llegaCamion(camionesEnEspera);
                            imprimeCosto();
                            corridaActual++;
                        }


                        void llegaCamion(int camionesEnEspera)
                        {
                            double naleatorio1;
                            double naleatorio2;
                            DateTime horaLlegada = hora;
                            DateTime horaEntradaDescarga;
                            DateTime horaSalidaCamion = hora;
                            double tiempoEsperaTotal = 0;
                            int tiempoEspera;
                            int tiempoDescarga;
                            int camiones;

                            if (camionesEnEspera == 1)
                            {
                                camiones = 1;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    naleatorio2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioTres(naleatorio2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        DateTime temp = horaSalidaCamion;
                                        log.WriteLine("Inicia hora de comida de trabajadores:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("Termina hora de comer de los trabajadores:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioTres(naleatorio2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(naleatorio2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 2)
                            {
                                camiones = 2;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    naleatorio2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioTres(naleatorio2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioTres(naleatorio2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(naleatorio2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 3)
                            {
                                camiones = 3;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    naleatorio2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioTres(naleatorio2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioTres(naleatorio2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(naleatorio2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }

                            while (horaLlegada < horaLimite)
                            {
                                naleatorio1 = listRi[i];
                                i++;
                                naleatorio2 = listRi[i];
                                i++;
                                horaLlegada = horaLlegada.AddMinutes(inversaTiempoLlegada(naleatorio1));
                                //Nose si esto jala bien
                                if (horaLlegada > horaLimite)
                                {
                                    i--;
                                    i--;
                                    break;
                                }
                                //Si llega a la hora de comida, se espera 30 minutos
                                if (horaLlegada == horaComida && comida == false)
                                {
                                    log.WriteLine("El personal comienza a comer a las:" + horaLlegada.TimeOfDay);
                                    horaLlegada = horaLlegada.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaLlegada.TimeOfDay);
                                    comida = true;
                                }
                                //
                                if (horaSalidaCamion < horaLlegada)
                                {
                                    horaEntradaDescarga = horaLlegada;
                                }
                                else
                                {
                                    horaEntradaDescarga = horaSalidaCamion;
                                }
                                horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioTres(naleatorio2));
                                //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                if ((horaSalidaCamion >= horaComida) && (comida == false))
                                {
                                    comida = true;
                                    DateTime temp = horaSalidaCamion;
                                    log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                    horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioTres(naleatorio2);
                                    log.WriteLine(Math.Round(naleatorio1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(naleatorio2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                                    continue;
                                }
                                tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                tiempoDescarga = inversaTiempoServicioTres(naleatorio2);
                                log.WriteLine(Math.Round(naleatorio1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(naleatorio2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                            }
                            hrsTiempoExtra = (double)horaSalidaCamion.Subtract(horaLlegada).TotalHours;
                            hrsTiempoEspera = tiempoEsperaTotal / 60;
                            hrsAlmacen = (double)horaSalidaCamion.Subtract(hora).TotalHours;
                        }
                        break;
                    case 4:
                        int inversaTiempoServicioCuatro(double r)
                        {
                            if (r >= 0 && r < 0.05)
                            {
                                return 15;
                            }
                            else if (r >= 0.05 && r < 0.20)
                            {
                                return 20;
                            }
                            else if (r >= 0.20 && r < 0.40)
                            {
                                return 25;
                            }
                            else if (r >= 0.40 && r <= 0.6)
                            {
                                return 30;
                            }
                            else if (r >= 0.6 && r <= 0.75)
                            {
                                return 35;
                            }
                            else if (r >= 0.75 && r <= 0.87)
                            {
                                return 40;
                            }
                            else if (r >= 0.87 && r <= 0.95)
                            {
                                return 45;
                            }
                            else if (r >= 0.95 && r <= 0.99)
                            {
                                return 50;
                            }
                            else if (r >= 0.99 && r <= 1)
                            {
                                return 55;
                            }
                            return 0;
                        }
                        i = 0;
                        camionesEnEspera = 0;
                        rCamionesEnEspera = 0;
                        corridaActual = 1;

                        //Costos
                        hrsTiempoNormal = 8;
                        hrsTiempoExtra = 0;
                        hrsTiempoEspera = 0;
                        hrsAlmacen = 0;
                        costoTotal = 0;

                        while (corridaActual <= corridas)
                        {
                            //Comida
                            comida = false;

                            log.WriteLine("\n\nCorrida: " + corridaActual);
                            Console.WriteLine("\n\nCorrida: " + corridaActual);

                            rCamionesEnEspera = listRi[i];
                            camionesEnEspera = inversaCamiones(rCamionesEnEspera);
                            i++;
                            log.WriteLine("Camiones en espera: " + camionesEnEspera + "\nPSE: " + rCamionesEnEspera);
                            log.WriteLine("#PSE" + "\t\tH.Llegada" + "\t\tHora.Ent.Des" + "\t#PSE" + "\t\tT.Des" + "\tH.Salida" + "\tT.Espera");
                            llegaCamionCuatro(camionesEnEspera);
                            imprimeCostoCuatro();
                            corridaActual++;
                        }
                        //log.WriteLine("#PSE" + "\t\tHora de llegada" + "\t\tHora de entrada a descarga" + "\t#PSE" + "\t\tTiempo de descarga" + "\tHora de salida del camion" + "\tTiempo de espera" );
                        void imprimeCostoCuatro()
                        {
                            costoTotal = ((hrsTiempoNormal * 25) * 4) + ((hrsTiempoExtra * 37.5) * 4) + (hrsTiempoEspera * 100) + (hrsAlmacen * 500);
                            log.WriteLine("Costo total: $" + Math.Round(costoTotal, 4));
                        }

                        void llegaCamionCuatro(int camionesEnEspera)
                        {
                            double PSE1;
                            double PSE2;
                            DateTime horaLlegada = hora;
                            DateTime horaEntradaDescarga;
                            DateTime horaSalidaCamion = hora;
                            double tiempoEsperaTotal = 0;
                            int tiempoEspera;
                            int tiempoDescarga;
                            int camiones;

                            if (camionesEnEspera == 1)
                            {
                                camiones = 1;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCuatro(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCuatro(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 2)
                            {
                                camiones = 2;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCuatro(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCuatro(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 3)
                            {
                                camiones = 3;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCuatro(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCuatro(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }

                            while (horaLlegada < horaLimite)
                            {
                                PSE1 = listRi[i];
                                i++;
                                PSE2 = listRi[i];
                                i++;
                                horaLlegada = horaLlegada.AddMinutes(inversaTiempoLlegada(PSE1));
                                //Nose si esto jala bien
                                if (horaLlegada > horaLimite)
                                {
                                    i--;
                                    i--;
                                    break;
                                }
                                if (horaLlegada == horaComida)
                                {
                                    log.WriteLine("El personal comienza a comer a las:" + horaLlegada.TimeOfDay);
                                    horaLlegada = horaLlegada.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaLlegada.TimeOfDay);
                                    comida = true;
                                }
                                //
                                if (horaSalidaCamion < horaLlegada)
                                {
                                    horaEntradaDescarga = horaLlegada;
                                }
                                else
                                {
                                    horaEntradaDescarga = horaSalidaCamion;
                                }
                                horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCuatro(PSE2));
                                //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                if ((horaSalidaCamion >= horaComida) && (comida == false))
                                {
                                    comida = true;
                                    DateTime temp = horaSalidaCamion;
                                    log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                    horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCuatro(PSE2);
                                    log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                                    continue;
                                }
                                tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                tiempoDescarga = inversaTiempoServicioCuatro(PSE2);
                                log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                            }
                            hrsTiempoExtra = (double)horaSalidaCamion.Subtract(horaLlegada).TotalHours;
                            hrsTiempoEspera = tiempoEsperaTotal / 60;
                            hrsAlmacen = (double)horaSalidaCamion.Subtract(hora).TotalHours;
                        }
                        break;
                    case 5:
                        int inversaTiempoServicioCinco(double r)
                        {
                            if (r >= 0 && r < 0.1)
                            {
                                return 10;
                            }
                            else if (r >= 0.1 && r < 0.28)
                            {
                                return 15;
                            }
                            else if (r >= 0.28 && r < 0.5)
                            {
                                return 20;
                            }
                            else if (r >= 0.5 && r <= 0.68)
                            {
                                return 25;
                            }
                            else if (r >= 0.68 && r <= 0.78)
                            {
                                return 30;
                            }
                            else if (r >= 0.78 && r <= 0.86)
                            {
                                return 35;
                            }
                            else if (r >= 0.86 && r <= 0.92)
                            {
                                return 40;
                            }
                            else if (r >= 0.92 && r <= 0.97)
                            {
                                return 45;
                            }
                            else if (r >= 0.97 && r <= 1)
                            {
                                return 50;
                            }
                            return 0;
                        }
                        i = 0;
                        camionesEnEspera = 0;
                        rCamionesEnEspera = 0;
                        corridaActual = 1;

                        //Costos
                        hrsTiempoNormal = 8;
                        hrsTiempoExtra = 0;
                        hrsTiempoEspera = 0;
                        hrsAlmacen = 0;
                        costoTotal = 0;

                        while (corridaActual <= corridas)
                        {
                            //Comida
                            comida = false;

                            log.WriteLine("\n\nCorrida: " + corridaActual);
                            Console.WriteLine("\n\nCorrida: " + corridaActual);

                            rCamionesEnEspera = listRi[i];
                            camionesEnEspera = inversaCamiones(rCamionesEnEspera);
                            i++;
                            log.WriteLine("Camiones en espera: " + camionesEnEspera + "\nPSE: " + rCamionesEnEspera);
                            log.WriteLine("#PSE" + "\t\tH.Llegada" + "\t\tHora.Ent.Des" + "\t#PSE" + "\t\tT.Des" + "\tH.Salida" + "\tT.Espera");
                            llegaCamionCinco(camionesEnEspera);
                            imprimeCostoCinco();
                            corridaActual++;
                        }
                        //log.WriteLine("#PSE" + "\t\tHora de llegada" + "\t\tHora de entrada a descarga" + "\t#PSE" + "\t\tTiempo de descarga" + "\tHora de salida del camion" + "\tTiempo de espera" );
                        void imprimeCostoCinco()
                        {
                            costoTotal = ((hrsTiempoNormal * 25) * 5) + ((hrsTiempoExtra * 37.5) * 5) + (hrsTiempoEspera * 100) + (hrsAlmacen * 500);
                            log.WriteLine("Costo total: $" + Math.Round(costoTotal, 4));
                        }

                        void llegaCamionCinco(int camionesEnEspera)
                        {
                            double PSE1;
                            double PSE2;
                            DateTime horaLlegada = hora;
                            DateTime horaEntradaDescarga;
                            DateTime horaSalidaCamion = hora;
                            double tiempoEsperaTotal = 0;
                            int tiempoEspera;
                            int tiempoDescarga;
                            int camiones;

                            if (camionesEnEspera == 1)
                            {
                                camiones = 1;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCinco(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCinco(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 2)
                            {
                                camiones = 2;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCinco(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCinco(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 3)
                            {
                                camiones = 3;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCinco(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCinco(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }

                            while (horaLlegada < horaLimite)
                            {
                                PSE1 = listRi[i];
                                i++;
                                PSE2 = listRi[i];
                                i++;
                                horaLlegada = horaLlegada.AddMinutes(inversaTiempoLlegada(PSE1));
                                //Nose si esto jala bien
                                if (horaLlegada > horaLimite)
                                {
                                    i--;
                                    i--;
                                    break;
                                }
                                if (horaLlegada == horaComida)
                                {
                                    log.WriteLine("El personal comienza a comer a las:" + horaLlegada.TimeOfDay);
                                    horaLlegada = horaLlegada.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaLlegada.TimeOfDay);
                                    comida = true;
                                }
                                //
                                if (horaSalidaCamion < horaLlegada)
                                {
                                    horaEntradaDescarga = horaLlegada;
                                }
                                else
                                {
                                    horaEntradaDescarga = horaSalidaCamion;
                                }
                                horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioCinco(PSE2));
                                //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                if ((horaSalidaCamion >= horaComida) && (comida == false))
                                {
                                    comida = true;
                                    DateTime temp = horaSalidaCamion;
                                    log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                    horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioCinco(PSE2);
                                    log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                                    continue;
                                }
                                tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                tiempoDescarga = inversaTiempoServicioCinco(PSE2);

                                log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                            }
                            hrsTiempoExtra = (double)horaSalidaCamion.Subtract(horaLlegada).TotalHours;
                            hrsTiempoEspera = tiempoEsperaTotal / 60;
                            hrsAlmacen = (double)horaSalidaCamion.Subtract(hora).TotalHours;
                        }
                        break;
                    case 6:
                        int inversaTiempoServicioSeis(double r)
                        {
                            if (r >= 0 && r < 0.12)
                            {
                                return 5;
                            }
                            else if (r >= 0.12 && r < 0.27)
                            {
                                return 10;
                            }
                            else if (r >= 0.27 && r < 0.53)
                            {
                                return 15;
                            }
                            else if (r >= 0.53 && r <= 0.68)
                            {
                                return 20;
                            }
                            else if (r >= 0.68 && r <= 0.8)
                            {
                                return 25;
                            }
                            else if (r >= 0.8 && r <= 0.88)
                            {
                                return 30;
                            }
                            else if (r >= 0.88 && r <= 0.94)
                            {
                                return 35;
                            }
                            else if (r >= 0.94 && r <= 0.98)
                            {
                                return 40;
                            }
                            else if (r >= 0.98 && r <= 1)
                            {
                                return 45;
                            }
                            return 0;
                        }
                        i = 0;
                        camionesEnEspera = 0;
                        rCamionesEnEspera = 0;
                        corridaActual = 1;

                        //Costos
                        hrsTiempoNormal = 8;
                        hrsTiempoExtra = 0;
                        hrsTiempoEspera = 0;
                        hrsAlmacen = 0;
                        costoTotal = 0;

                        while (corridaActual <= corridas)
                        {
                            //Comida
                            comida = false;

                            log.WriteLine("\n\nCorrida: " + corridaActual);
                            Console.WriteLine("\n\nCorrida: " + corridaActual);

                            rCamionesEnEspera = listRi[i];
                            camionesEnEspera = inversaCamiones(rCamionesEnEspera);
                            i++;
                            log.WriteLine("Camiones en espera: " + camionesEnEspera + "\nPSE: " + rCamionesEnEspera);
                            log.WriteLine("#PSE" + "\t\tH.Llegada" + "\t\tHora.Ent.Des" + "\t#PSE" + "\t\tT.Des" + "\tH.Salida" + "\tT.Espera");
                            llegaCamionSeis(camionesEnEspera);
                            imprimeCostoSeis();
                            corridaActual++;
                        }
                        //log.WriteLine("#PSE" + "\t\tHora de llegada" + "\t\tHora de entrada a descarga" + "\t#PSE" + "\t\tTiempo de descarga" + "\tHora de salida del camion" + "\tTiempo de espera" );
                        void imprimeCostoSeis()
                        {
                            costoTotal = ((hrsTiempoNormal * 25) * 6) + ((hrsTiempoExtra * 37.5) * 6) + (hrsTiempoEspera * 100) + (hrsAlmacen * 500);
                            log.WriteLine("Costo total: $" + Math.Round(costoTotal, 4));
                        }

                        void llegaCamionSeis(int camionesEnEspera)
                        {
                            double PSE1;
                            double PSE2;
                            DateTime horaLlegada = hora;
                            DateTime horaEntradaDescarga;
                            DateTime horaSalidaCamion = hora;
                            double tiempoEsperaTotal = 0;
                            int tiempoEspera;
                            int tiempoDescarga;
                            int camiones;

                            if (camionesEnEspera == 1)
                            {
                                camiones = 1;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioSeis(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioSeis(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 2)
                            {
                                camiones = 2;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioSeis(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioSeis(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }
                            else if (camionesEnEspera == 3)
                            {
                                camiones = 3;
                                while (camiones != 0)
                                {
                                    if (camiones == 0) break;
                                    horaLlegada = hora;
                                    PSE2 = listRi[i];
                                    i++;
                                    if (horaSalidaCamion < horaLlegada)
                                    {
                                        horaEntradaDescarga = horaLlegada;
                                    }
                                    else
                                    {
                                        horaEntradaDescarga = horaSalidaCamion;
                                    }
                                    horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioSeis(PSE2));
                                    //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                    if ((horaSalidaCamion >= horaComida) && (comida == false))
                                    {
                                        comida = true;
                                        log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                        horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                        log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    }
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioSeis(PSE2);

                                    log.WriteLine("0.0000" + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + horaSalidaCamion.TimeOfDay + "\t\t" + tiempoEspera);
                                    camiones--;
                                }

                            }

                            while (horaLlegada < horaLimite)
                            {
                                PSE1 = listRi[i];
                                i++;
                                PSE2 = listRi[i];
                                i++;
                                horaLlegada = horaLlegada.AddMinutes(inversaTiempoLlegada(PSE1));
                                //Nose si esto jala bien
                                if (horaLlegada > horaLimite)
                                {
                                    i--;
                                    i--;
                                    break;
                                }
                                if (horaLlegada == horaComida)
                                {
                                    log.WriteLine("El personal comienza a comer a las:" + horaLlegada.TimeOfDay);
                                    horaLlegada = horaLlegada.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaLlegada.TimeOfDay);
                                    comida = true;

                                }
                                //
                                if (horaSalidaCamion < horaLlegada)
                                {
                                    horaEntradaDescarga = horaLlegada;
                                }
                                else
                                {
                                    horaEntradaDescarga = horaSalidaCamion;
                                }
                                horaSalidaCamion = horaEntradaDescarga.AddMinutes(inversaTiempoServicioSeis(PSE2));
                                //Si acaba de salir un camion a la hora de comida, se espera 30 minutos
                                if ((horaSalidaCamion >= horaComida) && (comida == false))
                                {
                                    comida = true;
                                    DateTime temp = horaSalidaCamion;
                                    log.WriteLine("El personal comienza a comer a las:" + horaSalidaCamion.TimeOfDay);
                                    horaSalidaCamion = horaSalidaCamion.AddMinutes(30);
                                    log.WriteLine("El personal termina de comer a las:" + horaSalidaCamion.TimeOfDay);
                                    tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                    tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                    tiempoDescarga = inversaTiempoServicioSeis(PSE2);
                                    log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                                    continue;
                                }
                                tiempoEspera = (int)horaEntradaDescarga.Subtract(horaLlegada).TotalMinutes;
                                tiempoEsperaTotal = tiempoEsperaTotal + tiempoEspera;
                                tiempoDescarga = inversaTiempoServicioSeis(PSE2);

                                log.WriteLine(Math.Round(PSE1, 4) + "\t\t" + horaLlegada.TimeOfDay + "\t\t" + horaEntradaDescarga.TimeOfDay + "\t\t" + Math.Round(PSE2, 4) + "\t\t" + tiempoDescarga + "\t\t" + (horaSalidaCamion.AddMinutes(-30)).TimeOfDay + "\t\t" + tiempoEspera);
                            }
                            hrsTiempoExtra = (double)horaSalidaCamion.Subtract(horaLlegada).TotalHours;
                            hrsTiempoEspera = tiempoEsperaTotal / 60;
                            hrsAlmacen = (double)horaSalidaCamion.Subtract(hora).TotalHours;
                        }
                        break;
                }
            }

            //metodo que calcula promedios
            /* double promedios(double sumatoria, int cantidadDatos)
            {
                double promedio = (double)sumatoria / cantidadDatos;
                return promedio;
            } */

            //generador de variables aleatorias con Poisson
            void generadorPoisson(int lambda, int nVariablesAleatorias)
            {
                int contIndexRi = 0;
                Console.WriteLine("\n/************Generador de variables aleatorias con distribucion Poisson************/");
                log.WriteLine("\n\t/*********Generador de variables aleatorias con distribucion Poisson*****/");
                for (int j = 0; j < nVariablesAleatorias; j++)
                {
                    
                    int N = 0;
                    double T = 1;
                    double TAux = T;
                    for (int i = contIndexRi; TAux >= Math.Exp(-lambda); i++)
                    {
                        //generamos el numero aleatorio
                        //Paso 1
                        //Console.WriteLine("Ri = "+listRi[i]);
                        log.WriteLine("Ri = " + listRi[i]);
                        TAux = T * listRi[i];
                        //Paso 2
                        N++;
                        T = TAux;
                    }
                    //cuando generamos los numeros aumentamos el contador para no usar los mismos numeros aleatorios
                    contIndexRi++;

                    Console.WriteLine("Variable aleatoria Pi = " + N);
                    log.WriteLine("Variable aleatoria Pi = " + N);
                }

            }

            //generador de variables aleatorias con distribucion normal
            void generadorNormal(double desviacionStd, double miu, int nVariablesAleatorias)
            {
                int contIndexRi = 0;
                Console.WriteLine("\n/************Generador de variables aleatorias con distribucion normal************/");
                log.WriteLine("\n\t/*********Generador de variables aleatorias con distribucion normal*****/");
                for (int i = 0; i < nVariablesAleatorias; i++)
                {
                    //sumamos 12 variables aleatorias
                    double sumatoria = 0;


                    for (int j = contIndexRi; j < contIndexRi + 12; j++)
                    {
                        //Console.WriteLine("Indice Ri = " + j);
                        sumatoria = sumatoria + listRi[j];
                    }
                    contIndexRi = contIndexRi + 12;

                    //le restamos 6
                    sumatoria = sumatoria - 6;
                    //multiplicamos por la desviacion estandar
                    sumatoria = sumatoria * desviacionStd;
                    //le sumamos el miu
                    sumatoria = sumatoria + miu;
                    //Mostramos la variable aleatoria
                    Console.WriteLine("Variable aleatoria N" + (i + 1) + " = " + sumatoria);
                    log.WriteLine("Variable aleatoria N" + (i + 1) + " = " + sumatoria);
                }
            }

            dados(500);
            teoriaDeColas(3, 10);
            //Console.WriteLine(generadorPoisson(17));
            generadorPoisson(17);
            generadorNormal(6.5, 10, 5);
        }

        //metodo para crear el archivo csv
        static void escribirCSV(String nombre, String apellido, String pais)
        {
            String ruta = @"C:\Users\ormoj\Documents\Semestre 5\Simulacion\pruebaArchivoLog\registroCamiones.csv";
            String separador = ",";
            StringBuilder salida = new StringBuilder();

            String cadena = nombre + "," + apellido + "," + pais;
            List<String> lista = new List<string>();
            lista.Add(cadena);

            for (int i = 0; i < lista.Count; i++)
                salida.AppendLine(string.Join(separador, lista[i]));

            // CREA Y ESCRIBE EL ARCHIVO CSV
            //File.WriteAllText(ruta, salida.ToString());

            // AÑADE MAS LINEAS AL ARCHIVO CSV
            File.AppendAllText(ruta, salida.ToString());
        }
    }
}