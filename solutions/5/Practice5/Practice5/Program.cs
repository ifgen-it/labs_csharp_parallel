using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Practice5
{
    class Program
    {
        static int numbersCount = 7000000;  //50000000;
        static int maxNumber = 100;
        const int cap = 5;
        static string filePath = "..\\..\\..\\numbers.txt";

        static int qSortPermits = 3;
        static void Main(string[] args)
        {

            Console.WriteLine("====== Practice 5 : Parallel sorting ======\n");
            Console.WriteLine($"Numbers count = {numbersCount}");

            //WriteFile();
            //ReadFile();
            //List<int> sourceNumbers = ReadFileToList();

            Console.WriteLine($"Generation of source numbers = {FileSize(numbersCount * 4)} ...");
            //List<int> sourceNumbers = GenerateList();
            //sourceNumbers.Print();
            //Console.ReadKey();

            Console.WriteLine($"\n== SEQ SORT ==");

            // INNER SORT
            Console.WriteLine("\nInner C# sort");
            List<int> numbers1 = GenerateList();
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            numbers1.Sort();
            watch1.Stop();
            //numbers1.Print();
            DisposeList(ref numbers1);
            Console.WriteLine($"Time = {watch1.ElapsedMilliseconds} ms");
            //Console.ReadKey();


            // BUBBLE SORT
            /*Console.WriteLine("\nBubble sort");
            List<int> numbers2 = GenerateList();
            Stopwatch watch2 = new Stopwatch();
            watch2.Start();
            BubbleSort(numbers2);
            watch2.Stop();
            DisposeList(ref numbers2);
            Console.WriteLine($"Time = {watch2.ElapsedMilliseconds} ms");
            //numbers2.Print();
            //Console.ReadKey();*/

            // ODD-EVEN SORT
            /*Console.WriteLine("\nOdd-Even sort");
            List<int> numbers3 = GenerateList();
            Stopwatch watch3 = new Stopwatch();
            watch3.Start();
            OddEvenSort(numbers3);
            watch3.Stop();
            DisposeList(ref numbers3);
            Console.WriteLine($"Time = {watch3.ElapsedMilliseconds} ms");
            //numbers3.Print();
            //Console.ReadKey();*/

            // QUICK SORT
            Console.WriteLine("\nQuick sort");
            List<int> numbers4 = GenerateList();
            Stopwatch watch4 = new Stopwatch();
            //Console.WriteLine("Before start:");
            //numbers4.Print();
            watch4.Start();
            QuickSort(numbers4);
            watch4.Stop();
            //Console.WriteLine();
            //numbers4.Print();
            DisposeList(ref numbers4);
            Console.WriteLine($"Time = {watch4.ElapsedMilliseconds} ms");
            //Console.ReadKey();

            // MERGE SORT
            /*Console.WriteLine("\nMerge sort");
            List<int> numbers5 = GenerateList();
            Stopwatch watch5 = new Stopwatch();
            watch5.Start();
            numbers5 = MergeSort(numbers5);
            watch5.Stop();
            DisposeList(ref numbers5);
            Console.WriteLine($"Time = {watch5.ElapsedMilliseconds} ms");
            //numbers5.Print();
            //Console.ReadKey();*/

            // ENUMERATION SORT
            /*Console.WriteLine("\nEnumeration sort");
            List<int> numbers7 = GenerateList();
            Stopwatch watch7 = new Stopwatch();
            watch7.Start();
            numbers7 = EnumerationSort(numbers7);
            watch7.Stop();
            //numbers7.Print();
            DisposeList(ref numbers7);
            Console.WriteLine($"Time = {watch7.ElapsedMilliseconds} ms");
            //Console.ReadKey();*/

            // BUCKET SORT
            /*Console.WriteLine("\nBucket sort");
            List<int> numbers9 = GenerateList();
            Stopwatch watch9 = new Stopwatch();
            watch9.Start();
            numbers9 = BucketSort(numbers9);
            watch9.Stop();
            //numbers9.Print();
            DisposeList(ref numbers9);
            Console.WriteLine($"Time = {watch9.ElapsedMilliseconds} ms");
            //Console.ReadKey();*/

            // BITONIC SORT
            /*Console.WriteLine("\nBitonic sort");
            List<int> numbers13 = GenerateList(); //new List<int> { 10, 20, 5, 9, 3, 8, 12, 14, 90, 0, 60, 40, 23, 35, 95, 18 };
            Stopwatch watch13 = new Stopwatch();
            watch13.Start();
            BitonicSort(numbers13);
            watch13.Stop();
            //numbers13.Print();
            DisposeList(ref numbers13);
            Console.WriteLine($"Time = {watch13.ElapsedMilliseconds} ms");
            //Console.ReadKey();*/

            ///////////////////////////  PARALLEL SORT  //////////////////////////////////
            Console.WriteLine($"\n== PARALLEL SORT - processors: {System.Environment.ProcessorCount} ==");

            // BITONIC SORT - PARALLEL
            /*Console.WriteLine("\nBitonic sort");
            List<int> numbers14 = GenerateList(); //new List<int> { 10, 20, 5, 9, 3, 8, 12, 14, 90, 0, 60, 40, 23, 35, 95, 18 };
            Stopwatch watch14 = new Stopwatch();
            watch14.Start();
            ParallelBitonicSort(numbers14);
            watch14.Stop();
            //numbers14.Print();
            DisposeList(ref numbers14);
            Console.WriteLine($"Time = {watch14.ElapsedMilliseconds} ms");
            //Console.ReadKey();*/

            // SHELL SORT - PARALLEL
            /*Console.WriteLine("\nParallel Shell sort");
            List<int> numbers12 = GenerateList();
            Stopwatch watch12 = new Stopwatch();
            watch12.Start();
            ParallelShellSort(numbers12);
            watch12.Stop();
            //numbers12.Print();
            DisposeList(ref numbers12);
            Console.WriteLine($"Time = {watch12.ElapsedMilliseconds} ms");
            //Console.ReadKey();*/

            // QUICK SORT - PARALLEL
            /*Console.WriteLine("\nParallel Quick sort");
            List<int> numbers11 = GenerateList();
            Stopwatch watch11 = new Stopwatch();
            watch11.Start();
            numbers11 = ParallelQuickSort(numbers11);
            watch11.Stop();
            //numbers11.Print();
            DisposeList(ref numbers11);
            Console.WriteLine($"Time = {watch11.ElapsedMilliseconds} ms");
            //Console.ReadKey();*/

            // BUCKET SORT - PARALLEL
            /*Console.WriteLine("\nParallel Bucket sort");
            List<int> numbers10 = GenerateList();
            Stopwatch watch10 = new Stopwatch();
            watch10.Start();
            numbers10 = ParallelBucketSort(numbers10);
            watch10.Stop();
            //numbers10.Print();
            DisposeList(ref numbers10);
            Console.WriteLine($"Time = {watch10.ElapsedMilliseconds} ms");
            //Console.ReadKey();*/

            // ENUMERATION SORT - PARALLEL -- долгая
            /*Console.WriteLine("\nParallel Enumeration sort");
            List<int> numbers8 = GenerateList();
            Stopwatch watch8 = new Stopwatch();
            watch8.Start();
            numbers8 = ParallelEnumerationSort(numbers8);
            watch8.Stop();
            //numbers8.Print();
            DisposeList(ref numbers8);
            Console.WriteLine($"Time = {watch8.ElapsedMilliseconds} ms");
            //Console.ReadKey();*/

            // ODD-EVEN SORT - PARALLEL
            /*Console.WriteLine("\nParallel Odd-Even sort");
            List<int> numbers6 = GenerateList();
            Stopwatch watch6 = new Stopwatch();
            watch6.Start();
            ParallelOddEvenSort_good(numbers6);
            watch6.Stop();
            DisposeList(ref numbers6);
            Console.WriteLine($"Time = {watch6.ElapsedMilliseconds} ms");
            //numbers6.Print();*/


            ///////////////////////////////////////////////////
            Console.WriteLine("\nfinish");
            Console.ReadKey();
        }

        static void ParallelOddEvenSort_good(List<int> numbers)
        {
            int threadsNum = System.Environment.ProcessorCount;
            //Console.WriteLine($"Threads = {threadsNum}");
            int chunksNum = threadsNum * 2;
            int chunkSize = numbers.Count / chunksNum;

            Dictionary<int, KeyValuePair<int, int>> chunks = new Dictionary<int, KeyValuePair<int, int>>();
            int leftBorder = 0;
            for (int i = 0; i < chunksNum; i++)
            {
                int chunkId = i;
                int rightBorder = leftBorder + chunkSize;
                if (i == chunksNum - 1)
                    rightBorder = numbers.Count;
                chunks.Add(chunkId, new KeyValuePair<int, int>(leftBorder, rightBorder));
                leftBorder = rightBorder;
            }
            // INITIAL SORT ALL CHUNKS
            Parallel.For(0, chunks.Count, (i) =>
            {
                var chunk = chunks[i];
                int size = chunk.Value - chunk.Key;
                int start = chunk.Key;
                numbers.Sort(start, size, null);
            });
            //chunks.ToList().ForEach(ch => Console.WriteLine($"chunk = {ch.Key} : {ch.Value.Key} - {ch.Value.Value}"));

            for (int it = 0; it < chunks.Count / 2; it++)
            {
                // EVEN CHUNKS
                Parallel.For(0, chunks.Count / 2, (j) =>
                {
                    int i = 2 * j;
                    var leftChunk = chunks[i];
                    var rightChunk = chunks[i + 1];
                    int doubleSize = rightChunk.Value - leftChunk.Key;

                    // MERGE
                    List<int> result = new List<int>(doubleSize);
                    int l = leftChunk.Key;
                    int r = rightChunk.Key;
                    while (l < leftChunk.Value && r < rightChunk.Value)
                    {
                        if (numbers[l] < numbers[r])
                        {
                            result.Add(numbers[l]);
                            l++;
                        }
                        else
                        {
                            result.Add(numbers[r]);
                            r++;
                        }
                    }
                    if (l < leftChunk.Value)
                    {
                        for (int n = l; n < leftChunk.Value; n++)
                            result.Add(numbers[n]);
                    }
                    if (r < rightChunk.Value)
                    {
                        for (int n = r; n < rightChunk.Value; n++)
                            result.Add(numbers[n]);
                    }

                    //COPY
                    for (int n = 0; n < result.Count; n++)
                        numbers[leftChunk.Key + n] = result[n];

                });

                // ODD CHUNKS
                Parallel.For(0, chunks.Count / 2 - 1, (j) =>
                {
                    int i = 2 * j + 1;
                    var leftChunk = chunks[i];
                    var rightChunk = chunks[i + 1];
                    int doubleSize = rightChunk.Value - leftChunk.Key;

                    // MERGE
                    List<int> result = new List<int>(doubleSize);
                    int l = leftChunk.Key;
                    int r = rightChunk.Key;
                    while (l < leftChunk.Value && r < rightChunk.Value)
                    {
                        if (numbers[l] < numbers[r])
                        {
                            result.Add(numbers[l]);
                            l++;
                        }
                        else
                        {
                            result.Add(numbers[r]);
                            r++;
                        }
                    }
                    if (l < leftChunk.Value)
                    {
                        for (int n = l; n < leftChunk.Value; n++)
                            result.Add(numbers[n]);
                    }
                    if (r < rightChunk.Value)
                    {
                        for (int n = r; n < rightChunk.Value; n++)
                            result.Add(numbers[n]);
                    }

                    //COPY
                    for (int n = 0; n < result.Count; n++)
                        numbers[leftChunk.Key + n] = result[n];
                });
            }
        }
        static void ParallelOddEvenSort_bad(List<int> numbers)
        {
            int threadsNum = System.Environment.ProcessorCount;
            //Console.WriteLine($"Threads = {threadsNum}");
            int chunksNum = threadsNum * 2;
            int chunkSize = numbers.Count / chunksNum;

            Dictionary<int, KeyValuePair<int, int>> chunks = new Dictionary<int, KeyValuePair<int, int>>();
            int leftBorder = 0;
            for (int i = 0; i < chunksNum; i++)
            {
                int chunkId = i;
                int rightBorder = leftBorder + chunkSize;
                if (i == chunksNum - 1)
                    rightBorder = numbers.Count;
                chunks.Add(chunkId, new KeyValuePair<int, int>(leftBorder, rightBorder));
                leftBorder = rightBorder;
            }

            //chunks.ToList().ForEach(ch => Console.WriteLine($"chunk = {ch.Key} : {ch.Value.Key} - {ch.Value.Value}"));

            for (int it = 0; it < chunks.Count / 2; it++)
            {
                Parallel.For(0, chunks.Count / 2, (j) =>
                  {
                      int i = 2 * j;
                      var leftChunk = chunks[i];
                      var rightChunk = chunks[i + 1];
                      int doubleSize = rightChunk.Value - leftChunk.Key;
                      numbers.Sort(leftChunk.Key, doubleSize, null);
                  });
                Parallel.For(0, chunks.Count / 2 - 1, (j) =>
                {
                    int i = 2 * j + 1;
                    var leftChunk = chunks[i];
                    var rightChunk = chunks[i + 1];
                    int doubleSize = rightChunk.Value - leftChunk.Key;
                    numbers.Sort(leftChunk.Key, doubleSize, null);
                });
            }
        }
        static List<int> ParallelEnumerationSort(List<int> numbers)
        {
            List<int> result = Enumerable.Repeat(0, numbers.Count).ToList();
            int threadsNum = System.Environment.ProcessorCount;
            Parallel.ForEach(Partitioner.Create(0, numbers.Count, numbers.Count / threadsNum), range =>
            {
                for (int i = range.Item1; i < range.Item2; i++)
                {
                    int rank = 0;
                    for (int j = 0; j < numbers.Count; j++)
                    {
                        if (numbers[i] > numbers[j] || (numbers[i] == numbers[j] && j > i))
                            rank++;
                    }
                    result[rank] = numbers[i];
                }
            });

            return result;
        }
        static List<int> ParallelBucketSort(List<int> numbers)
        {
            // PREPARE
            int min = numbers[0];
            int max = numbers[0];
            foreach (var number in numbers)
            {
                if (number < min)
                    min = number;
                if (number > max)
                    max = number;
            }
            int threadsNum = System.Environment.ProcessorCount;
            int bucketNum = threadsNum;
            int bucketSize = (max - min + 1) / bucketNum;
            List<KeyValuePair<int, int>> borders = new List<KeyValuePair<int, int>>(bucketNum);
            for (int i = 0; i < bucketNum; i++)
            {
                int begin = min + i * bucketSize;
                int end = begin + bucketSize;
                if (i == bucketNum - 1)
                    end = max + 1;
                borders.Add(new KeyValuePair<int, int>(begin, end));
            }
            List<List<int>> buckets = new List<List<int>>();
            for (int i = 0; i < bucketNum; i++)
                buckets.Add(new List<int>());

            // FILL BUCKETS
            foreach (var number in numbers)
            {
                for (int i = 0; i < bucketNum; i++)
                {
                    int endValue = borders[i].Value;
                    if (number < endValue)
                    {
                        buckets[i].Add(number);
                        break;
                    }
                }
            }

            // SORT
            //Console.WriteLine("sorting...");
            Stopwatch sortWatch = new Stopwatch();
            sortWatch.Start();
            //Parallel.ForEach(buckets, bucket => bucket.Sort());
            //Parallel.For(0, buckets.Count, (i) => buckets[i].Sort());

            void RunSort(object obj)
            {
                Stopwatch thWatch = new Stopwatch();
                thWatch.Start();
                int thNum = Thread.CurrentThread.ManagedThreadId;
                //Console.WriteLine($"thread {thNum} started..");
                List<int> bucket = obj as List<int>;
                bucket.Sort();
                thWatch.Stop();
                //Console.WriteLine($"thread {thNum} finished. Time = {thWatch.ElapsedMilliseconds} ms");
            }

            Thread[] threads = new Thread[bucketNum];
            //Console.WriteLine("threads : " + threads.Length);
            for (int i = 0; i < bucketNum; i++)
            {
                threads[i] = new Thread(RunSort);
                threads[i].Start(buckets[i]);
            }
            //Console.WriteLine("All threads were started");

            for (int i = 0; i < threads.Length; i++)
                threads[i].Join();

            sortWatch.Stop();
            //Console.WriteLine($"sorting time = {sortWatch.ElapsedMilliseconds} ms");

            // COLLECT
            //Console.WriteLine("collecting...");
            int size = numbers.Count;
            DisposeList(ref numbers);
            List<int> result = new List<int>(size);
            buckets.ForEach(bucket => result.AddRange(bucket));

            return result;
        }
        static List<int> ParallelQuickSort(object obj)
        {
            List<int> numbers = obj as List<int>;
            if (numbers.Count <= 1)
                return numbers;

            //int pivotNum = numbers[0];
            int minInd = 0;
            int maxInd = numbers.Count - 1;
            int midInd = maxInd / 2;
            int pivotNum = (numbers[minInd] + numbers[midInd] + numbers[maxInd]) / 3;


            List<int> left = new List<int>();
            List<int> pivot = new List<int>();
            List<int> right = new List<int>();
            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] < pivotNum)
                    left.Add(numbers[i]);
                else if (numbers[i] == pivotNum)
                    pivot.Add(numbers[i]);
                else
                    right.Add(numbers[i]);
            }

            // PARALLEL 
            List<int> leftSorted = null;
            List<int> rightSorted = null;
            if (qSortPermits <= 3 && qSortPermits > 0)
            {
                Interlocked.Decrement(ref qSortPermits);
                Task<List<int>> tLeft = new Task<List<int>>(ParallelQuickSort, left);
                Task<List<int>> tRight = new Task<List<int>>(ParallelQuickSort, right);
                tLeft.Start();
                tRight.Start();
                Task.WaitAll(tLeft, tRight);
                leftSorted = tLeft.Result;
                rightSorted = tRight.Result;
            }
            else
            {
                leftSorted = ParallelQuickSort(left);
                rightSorted = ParallelQuickSort(right);
            }

            List<int> result = new List<int>();
            result.AddRange(leftSorted);
            result.AddRange(pivot);
            result.AddRange(rightSorted);

            leftSorted = rightSorted = pivot = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return result;
        }
        static void ParallelShellSort(List<int> numbers)
        {
            // PREPARE BLOCKS
            int threadsNum = 8; // System.Environment.ProcessorCount;   // p = q / 2
            int blocksNum = threadsNum * 2;    // q = 2^N
            int blockSize = numbers.Count / blocksNum;
            List<KeyValuePair<int, int>> blocks = new List<KeyValuePair<int, int>>(blocksNum);
            for (int i = 0; i < blocksNum; i++)
            {
                int begin = i * blockSize;
                int end = begin + blockSize;
                if (i == blocksNum - 1)
                    end = numbers.Count;
                blocks.Add(new KeyValuePair<int, int>(begin, end));
            }

            // STEP 1 - LOCAL SORT IN BLOCKS
            Parallel.ForEach(blocks, block => numbers.Sort(block.Key, (block.Value - block.Key), null));

            // STEP 2 - ITERATIONS MERGE-SPLIT FOR BLOCKS
            /*2 этап: N итераций merge-split для блоков
                на каждой i-итерации взаимодействуют блоки, номера которых
                различаются только в (N-i)-разряде в битовом представлении*/
            int iterNum = IntToBinaryString(blocks.Count - 1).TrimStart('0').Length; // iterNum = N
            for (int i = 0; i < iterNum; i++)
            {
                int bit = iterNum - i;
                int mask = 1;
                mask <<= (bit - 1); // (N-i)b

                // MAKE BLOCK PAIRS
                List<KeyValuePair<int, int>> blockPairs = new List<KeyValuePair<int, int>>(blocksNum / 2);
                var blockItems = new LinkedList<int>(Enumerable.Range(0, blocksNum));
                while (blockItems.Count > 0)
                {
                    int first = blockItems.First();
                    blockItems.RemoveFirst();
                    foreach (var second in blockItems)
                    {
                        if ((first ^ second) == mask)
                        {
                            blockPairs.Add(new KeyValuePair<int, int>(first, second));
                            blockItems.Remove(second);
                            break;
                        }
                    }
                }

                // MERGE-SPLIT
                Parallel.ForEach(blockPairs, blockPair =>
                {
                    var firstBlock = blocks[blockPair.Key];
                    var secondBlock = blocks[blockPair.Value];
                    int resultSize = firstBlock.Value - firstBlock.Key + secondBlock.Value - secondBlock.Key;
                    List<int> result = new List<int>(resultSize);

                    int l = firstBlock.Key;
                    int r = secondBlock.Key;
                    while (l < firstBlock.Value && r < secondBlock.Value)
                    {
                        if (numbers[l] < numbers[r])
                        {
                            result.Add(numbers[l]);
                            l++;
                        }
                        else
                        {
                            result.Add(numbers[r]);
                            r++;
                        }
                    }
                    if (l < firstBlock.Value)
                    {
                        for (int n = l; n < firstBlock.Value; n++)
                            result.Add(numbers[n]);
                    }
                    if (r < secondBlock.Value)
                    {
                        for (int n = r; n < secondBlock.Value; n++)
                            result.Add(numbers[n]);
                    }

                    //COPY
                    int resultCounter = 0;
                    for (int f = firstBlock.Key; f < firstBlock.Value; f++)
                        numbers[f] = result[resultCounter++];

                    for (int s = secondBlock.Key; s < secondBlock.Value; s++)
                        numbers[s] = result[resultCounter++];
                });
            }

            // STEP 3 - ODD-EVEN SORT - PARALLEL
            // NEED TO EXIT WHEN WILL NO ANY CHANGES
            for (int it = 0; it < blocksNum / 2; it++)
            {
                // EVEN BLOCKS
                var evenSorted = Enumerable.Repeat(false, blocksNum / 2).ToList();
                Parallel.For(0, blocksNum / 2, (j) =>
                {
                    int i = 2 * j;
                    var leftChunk = blocks[i];
                    var rightChunk = blocks[i + 1];
                    int doubleSize = rightChunk.Value - leftChunk.Key;

                    // IS SORTED
                    bool allSorted = true;
                    for (int t = leftChunk.Key; t < rightChunk.Value - 1; t++)
                    {
                        if (numbers[t] > numbers[t + 1])
                        {
                            allSorted = false;
                            break;
                        }
                    }
                    if (allSorted)
                        evenSorted[j] = true;
                    else
                    {
                        // MERGE
                        List<int> result = new List<int>(doubleSize);
                        int l = leftChunk.Key;
                        int r = rightChunk.Key;
                        while (l < leftChunk.Value && r < rightChunk.Value)
                        {
                            if (numbers[l] < numbers[r])
                            {
                                result.Add(numbers[l]);
                                l++;
                            }
                            else
                            {
                                result.Add(numbers[r]);
                                r++;
                            }
                        }
                        if (l < leftChunk.Value)
                        {
                            for (int n = l; n < leftChunk.Value; n++)
                                result.Add(numbers[n]);
                        }
                        if (r < rightChunk.Value)
                        {
                            for (int n = r; n < rightChunk.Value; n++)
                                result.Add(numbers[n]);
                        }

                        //COPY
                        for (int n = 0; n < result.Count; n++)
                            numbers[leftChunk.Key + n] = result[n];
                    }
                });

                // ODD BLOCKS
                var oddSorted = Enumerable.Repeat(false, blocksNum / 2 - 1).ToList();
                Parallel.For(0, blocksNum / 2 - 1, (j) =>
                {
                    int i = 2 * j + 1;
                    var leftChunk = blocks[i];
                    var rightChunk = blocks[i + 1];
                    int doubleSize = rightChunk.Value - leftChunk.Key;

                    // IS SORTED
                    bool allSorted = true;
                    for (int t = leftChunk.Key; t < rightChunk.Value - 1; t++)
                    {
                        if (numbers[t] > numbers[t + 1])
                        {
                            allSorted = false;
                            break;
                        }
                    }
                    if (allSorted)
                        oddSorted[j] = true;
                    else
                    {
                        // MERGE
                        List<int> result = new List<int>(doubleSize);
                        int l = leftChunk.Key;
                        int r = rightChunk.Key;
                        while (l < leftChunk.Value && r < rightChunk.Value)
                        {
                            if (numbers[l] < numbers[r])
                            {
                                result.Add(numbers[l]);
                                l++;
                            }
                            else
                            {
                                result.Add(numbers[r]);
                                r++;
                            }
                        }
                        if (l < leftChunk.Value)
                        {
                            for (int n = l; n < leftChunk.Value; n++)
                                result.Add(numbers[n]);
                        }
                        if (r < rightChunk.Value)
                        {
                            for (int n = r; n < rightChunk.Value; n++)
                                result.Add(numbers[n]);
                        }

                        //COPY
                        for (int n = 0; n < result.Count; n++)
                            numbers[leftChunk.Key + n] = result[n];
                    }
                });

                // ALL ARRAY IS SORTED
                var test = evenSorted.Union(oddSorted);
                if (!test.Contains(false))
                    break;
            }
        }
        static void ParallelBitonicSort(List<int> numbers)
        {
            if (IntToBinaryString(numbers.Count).Trim('0').Length != 1)
            {
                Console.WriteLine("Failure. Numbers count != 2^N");
                return;
            }
            bool HasBitInPosition(int number, int position)
            {
                int mask = 1 << position;
                int res = number & mask;
                if (res != 0)
                    return true;
                else
                    return false;
            }
            void SwapNumbers(int pos1, int pos2)
            {
                int temp = numbers[pos1];
                numbers[pos1] = numbers[pos2];
                numbers[pos2] = temp;
            }
            bool IsAsc(int begin, int size)
            {
                int ascBit = IntToBinaryString(size).TrimStart('0').Length - 1; // size = 2; ascBit = 1 bit
                bool asc = !HasBitInPosition(begin, ascBit); // asc/desc
                return asc;
            }
            void BitonicMerge(int begin, int size, bool asc)
            {
                int pairBit = IntToBinaryString(size).TrimStart('0').Length - 2; // size = 2; pairBit = 0 bit
                var indexes = Enumerable.Range(begin, size);
                var zeros = indexes.TakeWhile(n => HasBitInPosition(n, pairBit) == false).ToList();
                var ones = indexes.SkipWhile(n => HasBitInPosition(n, pairBit) == false).ToList();

                for (int i = 0; i < zeros.Count; i++)
                {
                    if (asc) // ascBit = 0
                    {
                        if (numbers[zeros[i]] > numbers[ones[i]])
                            SwapNumbers(zeros[i], ones[i]);
                    }
                    else // ascBit = 1
                    {
                        if (numbers[zeros[i]] < numbers[ones[i]])
                            SwapNumbers(zeros[i], ones[i]);
                    }
                }
            }
            void BMrec(int begin, int size, bool asc)
            {
                if (size == 2)
                {
                    BitonicMerge(begin, size, asc);
                    return;
                }
                BitonicMerge(begin, size, asc);
                int halfSize = size / 2;
                BMrec(begin, halfSize, asc);
                BMrec(begin + halfSize, halfSize, asc);
            }

            // MAKE BITONIC SEQUENCE : M(2) + M(4) + M(8) + ... + M(N/2)
            for (int bmSize = 2; bmSize <= numbers.Count / 2; bmSize *= 2)
            {
                var steppedNumbers = Enumerable.Range(0, numbers.Count / bmSize).Select(i => i * bmSize);
                Parallel.ForEach(steppedNumbers, i => BMrec(i, bmSize, IsAsc(i, bmSize)));
            }

            // SORT BITONIC SEQUENCE : B(N), B(N/2), B(N/4), ... B(2)
            BMrec(0, numbers.Count, true);
        }

        //////////////////////////////////////////

        static void BitonicSort(List<int> numbers)
        {
            if (IntToBinaryString(numbers.Count).Trim('0').Length != 1)
            {
                Console.WriteLine("Failure. Numbers count != 2^N");
                return;
            }
            bool HasBitInPosition(int number, int position)
            {
                int mask = 1 << position;
                int res = number & mask;
                if (res != 0)
                    return true;
                else
                    return false;
            }
            void SwapNumbers(int pos1, int pos2)
            {
                int temp = numbers[pos1];
                numbers[pos1] = numbers[pos2];
                numbers[pos2] = temp;
            }
            bool IsAsc(int begin, int size)
            {
                int ascBit = IntToBinaryString(size).TrimStart('0').Length - 1; // size = 2; ascBit = 1 bit
                bool asc = !HasBitInPosition(begin, ascBit); // asc/desc
                return asc;
            }
            void BitonicMerge(int begin, int size, bool asc)
            {
                int pairBit = IntToBinaryString(size).TrimStart('0').Length - 2; // size = 2; pairBit = 0 bit
                var indexes = Enumerable.Range(begin, size);
                var zeros = indexes.TakeWhile(n => HasBitInPosition(n, pairBit) == false).ToList();
                var ones = indexes.SkipWhile(n => HasBitInPosition(n, pairBit) == false).ToList();

                for (int i = 0; i < zeros.Count; i++)
                {
                    if (asc) // ascBit = 0
                    {
                        if (numbers[zeros[i]] > numbers[ones[i]])
                            SwapNumbers(zeros[i], ones[i]);
                    }
                    else // ascBit = 1
                    {
                        if (numbers[zeros[i]] < numbers[ones[i]])
                            SwapNumbers(zeros[i], ones[i]);
                    }
                }
            }
            void BMrec(int begin, int size, bool asc)
            {
                if (size == 2)
                {
                    BitonicMerge(begin, size, asc);
                    return;
                }
                BitonicMerge(begin, size, asc);
                int halfSize = size / 2;
                BMrec(begin, halfSize, asc);
                BMrec(begin + halfSize, halfSize, asc);
            }

            // MAKE BITONIC SEQUENCE : M(2) + M(4) + M(8) + ... + M(N/2)
            for (int bmSize = 2; bmSize <= numbers.Count / 2; bmSize *= 2)
            {
                for (int i = 0; i < numbers.Count; i += bmSize)
                    BMrec(i, bmSize, IsAsc(i, bmSize));
            }

            // SORT BITONIC SEQUENCE : B(N), B(N/2), B(N/4), ... B(2)
            BMrec(0, numbers.Count, true);
        }
        static List<int> BucketSort(List<int> numbers)
        {
            int min = numbers[0];
            int max = numbers[0];
            foreach (var number in numbers)
            {
                if (number < min)
                    min = number;
                if (number > max)
                    max = number;
            }
            int threadsNum = System.Environment.ProcessorCount;
            int bucketNum = threadsNum;
            int bucketSize = (max - min + 1) / bucketNum;
            //Console.WriteLine($"min = {min}, max = {max}, bucketSize = {bucketSize}");
            List<KeyValuePair<int, int>> borders = new List<KeyValuePair<int, int>>(bucketNum);
            for (int i = 0; i < bucketNum; i++)
            {
                int begin = min + i * bucketSize;
                int end = begin + bucketSize;
                if (i == bucketNum - 1)
                    end = max + 1;
                borders.Add(new KeyValuePair<int, int>(begin, end));
                //Console.WriteLine($"begin = {begin}, end = {end}");
            }
            List<List<int>> buckets = new List<List<int>>();
            for (int i = 0; i < bucketNum; i++)
                buckets.Add(new List<int>());

            // FILL BUCKETS
            foreach (var number in numbers)
            {
                for (int i = 0; i < bucketNum; i++)
                {
                    int endValue = borders[i].Value;
                    if (number < endValue)
                    {
                        buckets[i].Add(number);
                        break;
                    }
                }
            }

            // SORT
            //Console.WriteLine("sorting...");
            Stopwatch sortWatch = new Stopwatch();
            sortWatch.Start();
            buckets.ForEach(bucket => bucket.Sort());

            sortWatch.Stop();
            //Console.WriteLine($"sorting time = {sortWatch.ElapsedMilliseconds} ms");

            // COLLECT
            //Console.WriteLine("collecting...");

            List<int> result = new List<int>();
            /*buckets.ForEach(bucket => 
            {
                result.AddRange(bucket);
                DisposeList(ref bucket);
                });*/
            for (int i = 0; i < bucketNum; i++)
            {
                result.AddRange(buckets[i]);
                var temp = buckets[i];
                DisposeList(ref temp);
            }

            return result;
        }
        static List<int> EnumerationSort(List<int> numbers)
        {
            List<int> result = Enumerable.Repeat(0, numbers.Count).ToList();
            for (int i = 0; i < numbers.Count; i++)
            {
                int rank = 0;
                for (int j = 0; j < numbers.Count; j++)
                {
                    if (numbers[i] > numbers[j] || (numbers[i] == numbers[j] && j > i))
                        rank++;
                }
                result[rank] = numbers[i];
            }

            return result;
        }
        static List<int> MergeSort(List<int> numbers)
        {
            if (numbers.Count <= 1)
                return numbers;

            int pivot = numbers.Count / 2;
            List<int> left = new List<int>();
            List<int> right = new List<int>();
            for (int i = 0; i < pivot; i++)
                left.Add(numbers[i]);
            for (int i = pivot; i < numbers.Count; i++)
                right.Add(numbers[i]);

            List<int> leftSorted = MergeSort(left);
            List<int> rightSorted = MergeSort(right);

            List<int> result = new List<int>();
            int l = 0;
            int r = 0;
            while (l < leftSorted.Count && r < rightSorted.Count)
            {
                if (leftSorted[l] < rightSorted[r])
                {
                    result.Add(leftSorted[l]);
                    l++;
                }
                else
                {
                    result.Add(rightSorted[r]);
                    r++;
                }
            }
            if (l < leftSorted.Count)
            {
                for (int i = l; i < leftSorted.Count; i++)
                    result.Add(leftSorted[i]);
            }
            if (r < rightSorted.Count)
            {
                for (int i = r; i < rightSorted.Count; i++)
                    result.Add(rightSorted[i]);
            }

            return result;
        }
        static List<int> QuickSort(List<int> numbers)
        {
            if (numbers.Count <= 1)
                return numbers;

            //int pivotNum = numbers[0];
            int minInd = 0;
            int maxInd = numbers.Count - 1;
            int midInd = maxInd / 2;
            int pivotNum = (numbers[minInd] + numbers[midInd] + numbers[maxInd]) / 3;

            List<int> left = new List<int>();
            List<int> pivot = new List<int>();
            List<int> right = new List<int>();
            for (int i = 0; i < numbers.Count; i++)
            {
                if (numbers[i] < pivotNum)
                    left.Add(numbers[i]);
                else if (numbers[i] == pivotNum)
                    pivot.Add(numbers[i]);
                else
                    right.Add(numbers[i]);
            }
            List<int> leftSorted = QuickSort(left);
            List<int> rightSorted = QuickSort(right);
            List<int> result = new List<int>();
            result.AddRange(leftSorted);
            result.AddRange(pivot);
            result.AddRange(rightSorted);

            leftSorted = rightSorted = pivot = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();

            return result;
        }
        static void BubbleSort(List<int> numbers)
        {
            for (int j = numbers.Count - 1; j >= 1; j--)
            {
                for (int i = 0; i < j; i++)
                {
                    if (numbers[i] > numbers[i + 1])
                    {
                        int temp = numbers[i];
                        numbers[i] = numbers[i + 1];
                        numbers[i + 1] = temp;
                    }
                }
            }
        }
        static void OddEvenSort(List<int> numbers)
        {
            for (int it = 0; it < numbers.Count / 2; it++)
            {
                for (int i = 0; i < numbers.Count - 1; i += 2)
                {
                    if (numbers[i] > numbers[i + 1])
                    {
                        int temp = numbers[i];
                        numbers[i] = numbers[i + 1];
                        numbers[i + 1] = temp;
                    }
                }
                for (int i = 1; i < numbers.Count - 1; i += 2)
                {
                    if (numbers[i] > numbers[i + 1])
                    {
                        int temp = numbers[i];
                        numbers[i] = numbers[i + 1];
                        numbers[i + 1] = temp;
                    }
                }
            }
        }



        //////////////////////////////////////////
        static string FileSize(long sizeBytes)
        {
            string size;
            if (sizeBytes < 1024)
                size = sizeBytes.ToString() + " B";
            else if (sizeBytes < 1024 * 1024)
                size = (sizeBytes / 1024).ToString() + " KB";
            else
                size = (sizeBytes / 1024 / 1024).ToString() + " MB";
            return size;
        }
        static List<int> ReadFileToList()
        {
            Console.WriteLine("\nReading file ...");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int onePercent = numbersCount / 10 / 100;
            onePercent = onePercent == 0 ? 1 : onePercent;
            List<int> numbers = new List<int>();
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    int lineCounter = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        numbers.AddRange(
                                line.Split(new char[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                    .ToList()
                                    .Select(word => int.Parse(word))
                                    .ToList());
                        lineCounter++;
                        //Console.WriteLine(line);
                        if (lineCounter % onePercent == 0)
                            Console.Write("#");
                    }
                }
                watch.Stop();
                Console.WriteLine($"\nFile '{filePath}' was read. Time = {watch.ElapsedMilliseconds / 1000f:f2} sec.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return numbers;
        }
        static void ReadFile()
        {
            Console.WriteLine("\nReading file ...");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            int onePercent = numbersCount / 10 / 100;
            onePercent = onePercent == 0 ? 1 : onePercent;
            try
            {
                using (StreamReader sr = new StreamReader(filePath))
                {
                    string line;
                    int lineCounter = 0;
                    while ((line = sr.ReadLine()) != null)
                    {
                        lineCounter++;
                        //Console.WriteLine(line);
                        if (lineCounter % onePercent == 0)
                            Console.Write("#");
                    }
                }
                watch.Stop();
                Console.WriteLine($"\nFile '{filePath}' was read. Time = {watch.ElapsedMilliseconds / 1000f:f2} sec.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static void WriteFile()
        {
            Console.WriteLine("Writing file ...");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            Random rand = new Random();
            int onePercent = numbersCount / 100;
            try
            {
                using (StreamWriter sw = new StreamWriter(filePath, false))
                {
                    for (int i = 1; i < numbersCount + 1; i++)
                    {
                        int value = rand.Next(0, maxNumber + 1);
                        if (i % 10 != 0)
                            sw.Write($"{value,-cap} ");
                        else
                            sw.WriteLine($"{value,-cap}");
                        if (i % onePercent == 0)
                            Console.Write("#");
                    }
                }
                FileInfo file = new FileInfo(filePath);
                string size = FileSize(file.Length);

                watch.Stop();
                Console.WriteLine($"\nFile '{filePath}' was written, size = {size}. Time = {watch.ElapsedMilliseconds / 1000f:f2} sec.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        static List<int> GenerateList()
        {
            List<int> numbers = new List<int>(numbersCount);
            Random random = new Random();

            for (int i = 0; i < numbersCount; i++)
                numbers.Add(random.Next(0, maxNumber + 1));
            return numbers;
        }
        static void DisposeList(ref List<int> numbers)
        {
            numbers = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public static string IntToBinaryString(int number)
        {
            string result = "";
            for (int i = 0; i < 32; i++)
            {
                int mask = 1;
                mask = mask << i;
                int bitResult = number & mask;

                if (bitResult == 0)
                    result = "0" + result;
                else
                    result = "1" + result;
            }
            return result;
        }

    }

    public static class Extender
    {
        public static void Print(this List<int> list)
        {
            list.ForEach(n => Console.Write(n + " "));
            Console.WriteLine();
        }
    }

}
