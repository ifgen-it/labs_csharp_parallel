using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Practice1
{
    class Program
    {
        // TASK 1
        static int N = 1000;              // elements in vector
        static double factor = 1.789;
        static double[] arrSrc = new double[N];
        static Random rand = new Random();

        // TASK 2
        static int M = 10;               // number of threads
        static double[] arr2 = new double[N];

        // TASK 4
        static double[] arr24 = new double[N];
        static int K = 100;              // complexicity

        // TASK 5
        static double[] arr25 = new double[N];

        // TASK 6
        static double[] arr26 = new double[N];


        static void Main(string[] args)
        {
            Console.WriteLine("Elements in vector - N = " + N);
            Console.WriteLine("Number of threads - M = " + M);
            Console.WriteLine("Number of threads in OS = " + System.Environment.ProcessorCount);
            Console.WriteLine("\n----------------------------");

            // TASK 1 - one thread
            for (int i = 0; i < arrSrc.Length; i++)
            {
                arrSrc[i] = rand.Next(0, 100);
            }

            //PrintVector(arrSrc);

            double[] arr1 = new double[arrSrc.Length];
            DateTime t1 = DateTime.Now;

            for (int i = 0; i < arrSrc.Length; i++)
            {
                arr1[i] = Math.Pow(arrSrc[i], factor); 
            }
            DateTime t2 = DateTime.Now;
            TimeSpan tDelta = t2 - t1;

            Console.WriteLine($"Powered by {factor} in one thread:");
            //PrintVector(arr1);

            Console.WriteLine($"Total time: {tDelta.TotalMilliseconds} ms");
            Console.WriteLine("\n----------------------------\n");

            // TASK 2 - multithreading

            Console.WriteLine("---- Simple calculation -----");

            int blockSize = (int)Math.Ceiling((double)N / M);
            Console.WriteLine("block size = " + blockSize);
            int beginInd = 0;
            List<Thread> threads = new List<Thread>();
            bool exit = false;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (exit == false)
            {
                int endInd = beginInd + blockSize - 1;
                if (endInd >= arr2.Length-1)
                {
                    endInd = arr2.Length - 1;
                    exit = true;
                }

                Range range = new Range(beginInd, endInd);
                Thread thread = new Thread(Run);
                threads.Add(thread);
                thread.Start(range);

                beginInd = endInd + 1;
            }

            for (int i = 0; i < threads.Count; i++)
            {
                threads[i].Join();
            }
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;

            Console.WriteLine($"Powered by {factor} with {M} threads:");
            //PrintVector(arr2);

            Console.WriteLine($"Total time: {ts.TotalMilliseconds} ms");
            Console.WriteLine("\n----------------------------\n");

            // TASK 4 - more complex calculation

            Console.WriteLine("---- Complex calculation -----");

            int blockSize4 = (int)Math.Ceiling((double)N / M);
            Console.WriteLine("block size = " + blockSize4);
            int beginInd4 = 0;
            List<Thread> threads4 = new List<Thread>();
            bool exit4 = false;

            Stopwatch stopwatch4 = new Stopwatch();
            stopwatch4.Start();

            while (exit4 == false)
            {
                int endInd4 = beginInd4 + blockSize4 - 1;
                if (endInd4 >= arr24.Length - 1)
                {
                    endInd4 = arr24.Length - 1;
                    exit4 = true;
                }

                Range range = new Range(beginInd4, endInd4);
                Thread thread = new Thread(Run4);
                threads4.Add(thread);
                thread.Start(range);

                beginInd4 = endInd4 + 1;
            }

            for (int i = 0; i < threads4.Count; i++)
            {
                threads4[i].Join();
            }
            stopwatch4.Stop();
            TimeSpan ts4 = stopwatch4.Elapsed;

            Console.WriteLine($"Powered by {factor} with {M} threads:");
            //PrintVector(arr24);

            Console.WriteLine($"Total time: {ts4.TotalMilliseconds} ms");
            Console.WriteLine("\n----------------------------\n");

            // TASK 5 - gradient complexity

            Console.WriteLine("---- Gradient complexity calculation -----");

            int blockSize5 = (int)Math.Ceiling((double)N / M);
            Console.WriteLine("block size = " + blockSize4);
            int beginInd5 = 0;
            List<Thread> threads5 = new List<Thread>();
            bool exit5 = false;

            Stopwatch stopwatch5 = new Stopwatch();
            stopwatch5.Start();

            while (exit5 == false)
            {
                int endInd5 = beginInd5 + blockSize5 - 1;
                if (endInd5 >= arr25.Length - 1)
                {
                    endInd5 = arr25.Length - 1;
                    exit5 = true;
                }

                Range range = new Range(beginInd5, endInd5);
                Thread thread = new Thread(Run5);
                threads5.Add(thread);
                thread.Start(range);

                beginInd5 = endInd5 + 1;
            }

            for (int i = 0; i < threads5.Count; i++)
            {
                threads5[i].Join();
            }
            stopwatch5.Stop();
            TimeSpan ts5 = stopwatch5.Elapsed;

            Console.WriteLine($"Powered by {factor} with {M} threads:");
            //PrintVector(arr25);

            Console.WriteLine($"Total time: {ts5.TotalMilliseconds} ms");
            Console.WriteLine("\n----------------------------\n");


            // TASK 6 - cyclic data parallelism

            Console.WriteLine("---- Cyclic data parallelism calculation -----");

            List<Thread> threads6 = new List<Thread>();
            
            Stopwatch stopwatch6 = new Stopwatch();
            stopwatch6.Start();

            for (int i = 0; i < M; i++)
            {
                if (i == arr26.Length)
                {
                    M = arr26.Length; break;
                }
                Range range = new Range(i, 0);
                Thread thread = new Thread(Run6);
                threads6.Add(thread);
                thread.Start(range);
            }
            
            for (int i = 0; i < threads6.Count; i++)
            {
                threads6[i].Join();
            }
            stopwatch6.Stop();
            TimeSpan ts6 = stopwatch6.Elapsed;

            Console.WriteLine($"Powered by {factor} with {M} threads:");
            //PrintVector(arr26);

            Console.WriteLine($"Total time: {ts6.TotalMilliseconds} ms");
            Console.WriteLine("\n----------------------------\n");



            Console.ReadKey();
        }

        static void Run(object obj)
        {
            Range range = (Range)obj;
            for (int i = range.Begin; i <= range.End; i++)
            {
                arr2[i] = Math.Pow(arrSrc[i], factor);
            }

        }

        static void Run4(object obj)
        {
            Range range = (Range)obj;
            for (int i = range.Begin; i <= range.End; i++)
            {
                for(int j = 0; j < K; j++)
                    arr24[i] += Math.Pow(arrSrc[i], factor);
            }

        }

        static void Run5(object obj)
        {
            Range range = (Range)obj;
            for (int i = range.Begin; i <= range.End; i++)
            {
                for (int j = 0; j < i+1; j++)
                    arr25[i] += Math.Pow(arrSrc[i], factor);
            }

        }

        static void Run6(object obj)
        {
            Range range = (Range)obj;

            for (int i = range.Begin; i < arr26.Length; i = i + M)
            {
                for (int j = 0; j < i + 1; j++)
                    arr26[i] += Math.Pow(arrSrc[i], factor);
            }
        }

        static void PrintVector(double[] arr)
        {
            Console.WriteLine();
            foreach (var it in arr)
            {
                Console.Write(it + " ");
            }
            Console.WriteLine("\n");
        }
    }

    class Range
    {
        public Range()
        {
            Begin = 0; End = 0;
        }
        public Range(int begin, int end)
        {
            Begin = begin; End = end;
        }
        public int Begin { get; set; }
        public int End { get; set; }
    }
}
