using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Practice2
{
    class Program
    {
        #region init
        static long n = 400000000;        // last number in range
        static int m = 8;                 // number of threads

        // Parallel
        static bool[] primeNumPar2 = new bool[n + 1];
        static List<long> basePrimesPar2 = new List<long>();

        static bool[] primeNumPar3 = new bool[n + 1];
        static List<long> basePrimesPar3 = new List<long>();

        static bool[] primeNumPar4 = new bool[n + 1];
        static List<long> basePrimesPar4 = new List<long>();
        //static int threadCounterPar4;
        static CountdownEvent threadEventCounterPar4;

        static bool[] primeNumPar5 = new bool[n + 1];
        static List<long> basePrimesPar5 = new List<long>();
        static int primeCounterPar5 = 0;

        static bool[] primeNumPar6 = new bool[n + 1];

        static bool[] primeNumPar7 = new bool[n + 1];
        static List<long> basePrimesPar7 = new List<long>();
        #endregion

        static void Main(string[] args)
        {
            /**
             *  Лабораторная работа №2. Поиск простых чисел
             */

            Console.WriteLine("===== Practice 2 : Prime numbers =====");

            ////////////////////////////////////////////////////////////////////////////////////////
            /**
             *   Последовательный алгоритм «Решето Эратосфена».
             */
            #region sequent_1
            Console.WriteLine("\n----- Consequent algorithm 1 -----");

            bool[] primeNum = new bool[n + 1];
            for (long i = 0; i < primeNum.Length; i++)
                primeNum[i] = true;

            primeNum[0] = primeNum[1] = false;
            long endIndSeq = (long)Math.Ceiling(Math.Pow(n, 0.5));

            DateTime tStart = DateTime.Now;
            for (long currentInd = 2; currentInd <= endIndSeq /*< primeNumInd.Length*/; currentInd++)
            {
                if (primeNum[currentInd] == false)
                    continue;

                for (long i = currentInd * /*currentInd*/ 2; i < primeNum.Length; i = i + currentInd)
                {
                    primeNum[i] = false;
                }
            }
            DateTime tFinish = DateTime.Now;
            TimeSpan span = tFinish - tStart;

            Console.WriteLine("\nPrime numbers in range 2.." + n + ":");
            long counter = 0;

            for (long i = 0; i < primeNum.Length; i++)
            {
                if (primeNum[i])
                {
                    //Console.Write(i + " ");
                    counter++;
                }
            }
            Console.WriteLine("\nSpan time = " + span.TotalMilliseconds + " ms");
            Console.WriteLine("Prime numbers = " + counter);
            #endregion

            ////////////////////////////////////////////////////////////////////////////////////////
            /**
             *   Модифицированный последовательный алгоритм поиска
             *   
             *   Задействуется в работе только одно ядро процессора по максимуму, точнее все ядра по-немногу,
             *   т.е. поток не всегда на одном и том же ядре выполняется.
             */

            #region sequent_2
            Console.WriteLine("\n----- Prepare for Parallel algorithm -----");

            bool[] primeNumPar1 = new bool[n + 1];
            for (long i = 0; i < primeNumPar1.Length; i++)
                primeNumPar1[i] = true;

            primeNumPar1[0] = primeNumPar1[1] = false;
            long endIndPar1 = (long)Math.Ceiling(Math.Pow(n, 0.5));

            DateTime tStartPar1 = DateTime.Now;

            // FIRST PART 
            List<long> basePrimesPar1 = new List<long>();
            for (long currentInd = 2; currentInd <= endIndPar1; currentInd++)
            {
                if (primeNumPar1[currentInd] == false)
                    continue;
                basePrimesPar1.Add(currentInd);
                for (long i = currentInd * 2; i <= endIndPar1; i = i + currentInd)
                    primeNumPar1[i] = false;
            }

            // SECOND PART
            for (int i = 0; i < basePrimesPar1.Count; i++)
            {
                long prime = basePrimesPar1[i];
                long currentInd = endIndPar1 + 1;
                while (currentInd < primeNumPar1.Length)
                {
                    if (currentInd % prime != 0)
                        currentInd++;
                    else
                    {
                        primeNumPar1[currentInd] = false;
                        for (long j = currentInd + prime; j < primeNumPar1.Length; j += prime)
                            primeNumPar1[j] = false;
                        break;
                    }
                }
            }
            DateTime tFinishPar1 = DateTime.Now;
            TimeSpan spanPar1 = tFinishPar1 - tStartPar1;

            Console.WriteLine("\nPrime numbers in range 2.." + n + ":");
            long counterPar1 = 0;
            for (long i = 0; i < primeNumPar1.Length; i++)
            {
                if (primeNumPar1[i])
                {
                    //Console.Write(i + " ");
                    counterPar1++;
                }
            }
            Console.WriteLine("\nSpan time = " + spanPar1.TotalMilliseconds + " ms");
            Console.WriteLine("Prime numbers = " + counterPar1);
            #endregion

            //////////////////////////////////////////////////////////////////////////////////////
            /**  
             *  Параллельный алгоритм №1: декомпозиция по данным
             * 
             *  Получается равномерная загрузка ядер процессоров работой.
             *  Выигрыш по времени примерно в 2 раза на больших данных.
             *  Логических ядер - 4, физических - 2, поэтому и выигрыш примерно в 2 раза, а не в 4.
             *  Синхронизация не применялась.
             */

            #region parallel_1
            Console.WriteLine($"\n----- Parallel algo: DATA RANGE DECOMP, threads: {m} -----");

            for (long i = 0; i < primeNumPar2.Length; i++)
                primeNumPar2[i] = true;

            primeNumPar2[0] = primeNumPar2[1] = false;
            long endIndPar2 = (long)Math.Ceiling(Math.Pow(n, 0.5));

            DateTime tStartPar2 = DateTime.Now;

            // FIRST PART
            for (long currentInd = 2; currentInd <= endIndPar2; currentInd++)
            {
                if (primeNumPar2[currentInd] == false)
                    continue;
                basePrimesPar2.Add(currentInd);
                for (long i = currentInd * 2; i <= endIndPar2; i += currentInd)
                    primeNumPar2[i] = false;
            }

            // SECOND PART
            long rangePar2 = (long)Math.Ceiling(((n - endIndPar2) / (double)m));
            long startIndPar2 = endIndPar2 + 1;

            Thread[] threadsPar2 = new Thread[m];
            for (long i = 0; i < m; i++)
            {
                long start = startIndPar2 + i * rangePar2;
                long end = start + rangePar2;
                if (i == m - 1)
                    end = primeNumPar2.Length;
                object[] obj = new object[] { start, end };

                Thread thread = new Thread(RunPar2);
                thread.Start(obj);
                threadsPar2[i] = thread;
            }

            for (int i = 0; i < m; i++)
            {
                threadsPar2[i].Join();
            }

            DateTime tFinishPar2 = DateTime.Now;
            TimeSpan spanPar2 = tFinishPar2 - tStartPar2;

            Console.WriteLine("\nPrime numbers in range 2.." + n + ":");
            long counterPar2 = 0;
            for (long i = 0; i < primeNumPar2.Length; i++)
            {
                if (primeNumPar2[i])
                {
                    //Console.Write(i + " ");
                    counterPar2++;
                }
            }
            Console.WriteLine("\nSpan time = " + spanPar2.TotalMilliseconds + " ms");
            Console.WriteLine("Prime numbers = " + counterPar2);
            #endregion

            ///////////////////////////////////////////////////////////////////////////////////////////////
            /**
             *  Параллельный алгоритм №2: декомпозиция набора простых чисел
             *  
             *  Получается неравномерная работа у потоков: 1у потоку достаются базовые простые числа меньшие,
             *  соответственно ему для этих чисел надо в большем диапазоне вычеркнуть числа - работает дольше всех.
             *  Результаты : чуть лучше, чем при последовательном алгоритме.
             *  Синхронизация не применялась.
             */

            #region parallel_2
            Console.WriteLine($"\n----- Parallel algo: PRIMES DECOMP, threads: {m} -----\n");

            for (long i = 0; i < primeNumPar3.Length; i++)
                primeNumPar3[i] = true;

            primeNumPar3[0] = primeNumPar3[1] = false;
            long endIndPar3 = (long)Math.Ceiling(Math.Pow(n, 0.5));

            DateTime tStartPar3 = DateTime.Now;

            // FIRST PART
            for (long currentInd = 2; currentInd <= endIndPar3; currentInd++)
            {
                if (primeNumPar3[currentInd] == false)
                    continue;
                basePrimesPar3.Add(currentInd);
                for (long i = currentInd * 2; i <= endIndPar3; i += currentInd)
                    primeNumPar3[i] = false;
            }

            // SECOND PART
            long rangePrimes3 = (long)Math.Ceiling((basePrimesPar3.Count / (double)m));

            Thread[] threadsPar3 = new Thread[m];
            for (long i = 0; i < m; i++)
            {
                long startInd = i * rangePrimes3;
                long endInd = startInd + rangePrimes3;
                if (i == m - 1)
                    endInd = basePrimesPar3.Count;

                List<long> listPrimes = basePrimesPar3.GetRange((int)startInd, (int)(endInd - startInd));
                object[] obj = new object[] { listPrimes, endIndPar3 + 1 };

                Thread thread = new Thread(RunPar3);
                thread.Name = (i + 1).ToString();
                threadsPar3[i] = thread;
                thread.Start(obj);
            }

            for (int i = 0; i < m; i++)
            {
                threadsPar3[i].Join();
            }

            DateTime tFinishPar3 = DateTime.Now;
            TimeSpan spanPar3 = tFinishPar3 - tStartPar3;

            Console.WriteLine("\nPrime numbers in range 2.." + n + ":");
            long counterPar3 = 0;
            for (long i = 0; i < primeNumPar3.Length; i++)
            {
                if (primeNumPar3[i])
                {
                    //Console.Write(i + " ");
                    counterPar3++;
                }
            }
            Console.WriteLine("\nSpan time = " + spanPar3.TotalMilliseconds + " ms");
            Console.WriteLine("Prime numbers = " + counterPar3);
            #endregion

            ///////////////////////////////////////////////////////////////////////////////////////////////
            /**
             *  Параллельный алгоритм №3: применение пула потоков
             *  
             *  Хорошие результаты - как при декомпозиции по данным. Ядра загружаются сбалансированно.
             *  Тут используется динамическая декомпозиция - ThreadPool сам распределяет задания по ядрам,
             *  так чтобы никто из ядер не простаивал.
             *  Используется синхронизация счетчика потоков, массив ManualResetEvent[] работает только до 64х event'ов.
             *  
             *  UPD: вместо синхронизации с счетчиком сделал CountDownEvent - лучше
             */

            #region parallel_3
            Console.WriteLine($"\n----- Parallel algo: using THREAD POOL -----\n");

            for (long i = 0; i < primeNumPar4.Length; i++)
                primeNumPar4[i] = true;

            primeNumPar4[0] = primeNumPar4[1] = false;
            long endIndPar4 = (long)Math.Ceiling(Math.Pow(n, 0.5));

            DateTime tStartPar4 = DateTime.Now;

            // FIRST PART
            for (long currentInd = 2; currentInd <= endIndPar4; currentInd++)
            {
                if (primeNumPar4[currentInd] == false)
                    continue;
                basePrimesPar4.Add(currentInd);
                for (long i = currentInd * 2; i <= endIndPar4; i += currentInd)
                    primeNumPar4[i] = false;
            }
            Console.WriteLine($"Base primes = {basePrimesPar4.Count}");

            // SECOND PART
            //ManualResetEvent[] events = new ManualResetEvent[basePrimesPar4.Count];
            //threadCounterPar4 = basePrimesPar4.Count;
            threadEventCounterPar4 = new CountdownEvent(basePrimesPar4.Count);

            for (int i = 0; i < basePrimesPar4.Count; i++)
            {
                //events[i] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(RunPar4, new object[] { basePrimesPar4[i], endIndPar4 + 1/*, events[i]*/ });
            }
            threadEventCounterPar4.Wait();
            //WaitHandle.WaitAll(events);
            //while (threadCounterPar4 != 0) ;

            DateTime tFinishPar4 = DateTime.Now;
            TimeSpan spanPar4 = tFinishPar4 - tStartPar4;

            Console.WriteLine("\nPrime numbers in range 2.." + n + ":");
            long counterPar4 = 0;
            for (long i = 0; i < primeNumPar4.Length; i++)
            {
                if (primeNumPar4[i])
                {
                    //Console.Write(i + " ");
                    counterPar4++;
                }
            }
            Console.WriteLine("\nSpan time = " + spanPar4.TotalMilliseconds + " ms");
            Console.WriteLine("Prime numbers = " + counterPar4);
            #endregion

            ///////////////////////////////////////////////////////////////////////////////////////////////
            /**
             *  Параллельный алгоритм №4: последовательный перебор простых чисел
             *  Результат хороший, как и в Thread Pool, так как этот алгоритм похож
             *  на самодельный Пул потоков, которые постоянно в работе
             *  Синхронизация использовалась для обхода коллекции базовых простых чисел
             */

            #region parallel_4
            Console.WriteLine($"\n----- Parallel algo: PRIME NUMBERS BRUTE FORCE -----\n");

            for (long i = 0; i < primeNumPar5.Length; i++)
                primeNumPar5[i] = true;

            primeNumPar5[0] = primeNumPar5[1] = false;
            long endIndPar5 = (long)Math.Ceiling(Math.Pow(n, 0.5));

            DateTime tStartPar5 = DateTime.Now;

            // FIRST PART
            for (long currentInd = 2; currentInd <= endIndPar5; currentInd++)
            {
                if (primeNumPar5[currentInd] == false)
                    continue;
                basePrimesPar5.Add(currentInd);
                for (long i = currentInd * 2; i <= endIndPar5; i += currentInd)
                    primeNumPar5[i] = false;
            }
            Console.WriteLine($"Base primes = {basePrimesPar5.Count}");

            // SECOND PART
            Thread[] threadsPar5 = new Thread[m];
            for (long i = 0; i < m; i++)
            {
                object[] obj = new object[] { endIndPar5 + 1 };

                Thread thread = new Thread(RunPar5);
                threadsPar5[i] = thread;
                thread.Start(obj);
            }

            for (int i = 0; i < m; i++)
            {
                threadsPar5[i].Join();
            }

            DateTime tFinishPar5 = DateTime.Now;
            TimeSpan spanPar5 = tFinishPar5 - tStartPar5;

            Console.WriteLine("\nPrime numbers in range 2.." + n + ":");
            long counterPar5 = 0;
            for (long i = 0; i < primeNumPar5.Length; i++)
            {
                if (primeNumPar5[i])
                {
                    //Console.Write(i + " ");
                    counterPar5++;
                }
            }
            Console.WriteLine("\nSpan time = " + spanPar5.TotalMilliseconds + " ms");
            Console.WriteLine("Prime numbers = " + counterPar5);
            #endregion

            ////////////////////////////////////////////////////////////////////////////////////////
            /**
             *   Последовательный алгоритм с применением Parallel.For
             *   Скорость выполнения пока лучшая из всех.
             */
            #region parallel_5
            Console.WriteLine("\n----- Consequent algorithm 1 with PARALLEL.FOR -----");

            for (long i = 0; i < primeNumPar6.Length; i++)
                primeNumPar6[i] = true;

            primeNumPar6[0] = primeNumPar6[1] = false;
            long endIndPar6 = (long)Math.Ceiling(Math.Pow(n, 0.5));

            DateTime tStartPar6 = DateTime.Now;
            for (long currentInd = 2; currentInd <= endIndPar6; currentInd++)
            {
                if (primeNumPar6[currentInd] == false)
                    continue;
                Parallel.For(currentInd * 2, primeNumPar6.Length, i => 
                {
                    primeNumPar6[i] = false;
                    i = i + currentInd;
                });

            }
            DateTime tFinishPar6 = DateTime.Now;
            TimeSpan spanPar6 = tFinishPar6 - tStartPar6;

            Console.WriteLine("\nPrime numbers in range 2.." + n + ":");
            long counterPar6 = 0;

            for (long i = 0; i < primeNum.Length; i++)
            {
                if (primeNum[i])
                {
                    //Console.Write(i + " ");
                    counterPar6++;
                }
            }
            Console.WriteLine("\nSpan time = " + spanPar6.TotalMilliseconds + " ms");
            Console.WriteLine("Prime numbers = " + counterPar6);
            #endregion

            //////////////////////////////////////////////////////////////////////////////////////
            /**  
             *  Параллельный алгоритм №1: декомпозиция по данным - с применением Task
             *  Результаты как у обычного параллельного с помощью Thread, и как у ThreadPool
             */

            #region parallel_6
            Console.WriteLine($"\n----- Parallel algo with TASK: DATA RANGE DECOMP, threads: {m} -----");

            for (long i = 0; i < primeNumPar7.Length; i++)
                primeNumPar7[i] = true;

            primeNumPar7[0] = primeNumPar7[1] = false;
            long endIndPar7 = (long)Math.Ceiling(Math.Pow(n, 0.5));

            DateTime tStartPar7 = DateTime.Now;

            // FIRST PART
            for (long currentInd = 2; currentInd <= endIndPar7; currentInd++)
            {
                if (primeNumPar7[currentInd] == false)
                    continue;
                basePrimesPar7.Add(currentInd);
                for (long i = currentInd * 2; i <= endIndPar7; i += currentInd)
                    primeNumPar7[i] = false;
            }
            Console.WriteLine($"\nBase primes = {basePrimesPar7.Count}");

            // SECOND PART
            long rangePar7 = (long)Math.Ceiling(((n - endIndPar7) / (double)m));
            long startIndPar7 = endIndPar7 + 1;

            Task[] tasksPar7 = new Task[m];
            for (long i = 0; i < m; i++)
            {
                long start = startIndPar7 + i * rangePar7;
                long end = start + rangePar7;
                if (i == m - 1)
                    end = primeNumPar7.Length;
                object[] obj = new object[] { start, end };

                Task task = new Task(RunPar7, obj);
                tasksPar7[i] = task;
                task.Start();
            }
            Task.WaitAll(tasksPar7);

            DateTime tFinishPar7 = DateTime.Now;
            TimeSpan spanPar7 = tFinishPar7 - tStartPar7;

            Console.WriteLine("\nPrime numbers in range 2.." + n + ":");
            long counterPar7 = 0;
            for (long i = 0; i < primeNumPar7.Length; i++)
            {
                if (primeNumPar7[i])
                {
                    //Console.Write(i + " ");
                    counterPar7++;
                }
            }
            Console.WriteLine("\nSpan time = " + spanPar7.TotalMilliseconds + " ms");
            Console.WriteLine("Prime numbers = " + counterPar7);
            #endregion

           

            Console.ReadKey();
            /////////////////////////////////////////////////////////////////////////
        }

        /**
         *  Проверка всех базовых простых чисел
         *  На переданной части диапазона от корень(n) до n
         */
        static void RunPar2(object obj)
        {
            object[] objects = obj as object[];
            long start = (long)objects[0];
            long end = (long)objects[1];

            for (int i = 0; i < basePrimesPar2.Count; i++)
            {
                long prime = basePrimesPar2[i];
                long currentInd = start;
                while (currentInd < end)
                {
                    if (currentInd % prime != 0)
                        currentInd++;
                    else
                    {
                        primeNumPar2[currentInd] = false;
                        for (long j = currentInd + prime; j < end; j += prime)
                            primeNumPar2[j] = false;
                        break;
                    }
                }
            }
        }

        /**
         *  Участвует только переданный диапазон базовых простых чисел
         *  Проверка чисел на диапазоне от корень(n) до n
         */
        static void RunPar3(object obj)
        {
            DateTime tStartThread = DateTime.Now;
            object[] objects = obj as object[];
            List<long> listPrimes = objects[0] as List<long>;
            long start = (long)objects[1];
            long end = primeNumPar3.Length;

            for (int i = 0; i < listPrimes.Count; i++)
            {
                long prime = listPrimes[i];
                long currentInd = start;
                if (prime > currentInd)
                    currentInd = prime;

                while (currentInd < end)
                {
                    if (currentInd % prime != 0)
                        currentInd++;
                    else
                    {
                        primeNumPar3[currentInd] = false;
                        for (long j = currentInd + prime; j < end; j += prime)
                            primeNumPar3[j] = false;
                        break;
                    }
                }
            }
            DateTime tFinishThread = DateTime.Now;
            TimeSpan spanThread = tFinishThread - tStartThread;
            Console.WriteLine($"Base primes {listPrimes[0]}..{listPrimes[listPrimes.Count - 1]}. Thread " + Thread.CurrentThread.Name + " run in " + spanThread.TotalMilliseconds + " ms");
        }

        /**
         *  Для ThreadPool:
         *  Участвует в проверке только одно базовое число
         *  Проверка чисел на диапазоне от корень(n) до n
         */
        static void RunPar4(object obj)
        {
            //DateTime tStartThread = DateTime.Now;

            object[] objects = obj as object[];

            long prime = (long)objects[0];
            long start = (long)objects[1];
            //ManualResetEvent ev = objects[2] as ManualResetEvent;

            long end = primeNumPar4.Length;

            long currentInd = start;
            if (prime > currentInd)
                currentInd = prime;

            while (currentInd < end)
            {
                if (currentInd % prime != 0)
                    currentInd++;
                else
                {
                    primeNumPar4[currentInd] = false;
                    for (long j = currentInd + prime; j < end; j += prime)
                        primeNumPar4[j] = false;
                    break;
                }
            }
            threadEventCounterPar4.Signal();
            /*lock ("synch_par4")
            {
                threadCounterPar4--;
            }*/

            //Console.WriteLine("done");
            //ev.Set();

            //DateTime tFinishThread = DateTime.Now;
            //TimeSpan spanThread = tFinishThread - tStartThread;
            //Console.WriteLine($"Base prime {listPrimes[0]}..{listPrimes[listPrimes.Count - 1]}. Thread " + Thread.CurrentThread.Name + " run in " + spanThread.TotalMilliseconds + " ms");
        }

        /**
         *  Для Brute Force:
         *  Участвует в проверке только одно базовое число
         *  Проверка чисел на диапазоне от корень(n) до n
         */
        static void RunPar5(object obj)
        {
            object[] objects = obj as object[];
            long start = (long)objects[0];
            long end = primeNumPar5.Length;

            long prime = 0;
            while (true)
            {
                try
                {
                    Monitor.Enter("synch_par5");
                    if (primeCounterPar5 >= basePrimesPar5.Count)
                        break;
                    prime = basePrimesPar5[primeCounterPar5];
                    primeCounterPar5++;
                }
                finally
                {
                    Monitor.Exit("synch_par5");
                }
                
                long currentInd = start;
                if (prime > currentInd)
                    currentInd = prime;

                while (currentInd < end)
                {
                    if (currentInd % prime != 0)
                        currentInd++;
                    else
                    {
                        primeNumPar5[currentInd] = false;
                        for (long j = currentInd + prime; j < end; j += prime)
                            primeNumPar5[j] = false;
                        break;
                    }
                }
            }
        }

        /**
         *  Для Task: (копия RunPar2)
         *  Проверка всех базовых простых чисел
         *  На переданной части диапазона от корень(n) до n
         */
        static void RunPar7(object obj)
        {
            object[] objects = obj as object[];
            long start = (long)objects[0];
            long end = (long)objects[1];

            for (int i = 0; i < basePrimesPar7.Count; i++)
            {
                long prime = basePrimesPar7[i];
                long currentInd = start;
                while (currentInd < end)
                {
                    if (currentInd % prime != 0)
                        currentInd++;
                    else
                    {
                        primeNumPar7[currentInd] = false;
                        for (long j = currentInd + prime; j < end; j += prime)
                            primeNumPar7[j] = false;
                        break;
                    }
                }
            }
        }



    }
}
