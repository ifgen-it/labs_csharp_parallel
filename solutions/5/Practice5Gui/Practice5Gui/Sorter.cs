using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Practice5Gui
{
    class Sorter
    {
        delegate void Updater(object obj);
        Form1 form;
        Updater updater;

        static int maxNumber = 100;
        const int cap = 5;
        static string filePath = "..\\..\\..\\numbers.txt";
        static int qSortPermits = 3;

        public static void SetMaxRandom(int maxRandom)
        {
            maxNumber = maxRandom;
        }
        public Sorter(Form1 form)
        {
            this.form = form;
            updater = form.UpdateControls;
        }

        public void ParallelOddEvenSort(List<int> numbers, CancellationToken token)
        {
            int threadsNum = System.Environment.ProcessorCount;
            int chunksNum = threadsNum * 2;
            int chunkSize = numbers.Count / chunksNum;

            int step = 0;
            int needSteps = chunksNum + chunksNum;
            InitSteps(needSteps);

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

                MakeStep(Interlocked.Increment(ref step));
            });
            if (token.IsCancellationRequested)
                return;


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

                MakeStep(++step);
                if (token.IsCancellationRequested)
                    return;

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

                MakeStep(++step);
                if (token.IsCancellationRequested)
                    return;
            }
        }
        public void ParallelOddEvenSort_bad(List<int> numbers)
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
        public List<int> ParallelEnumerationSort(List<int> numbers, CancellationToken token)
        {
            int step = 0;
            InitSteps(numbers.Count);

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

                    MakeStep(Interlocked.Increment(ref step));
                    if (token.IsCancellationRequested)
                        break;
                }

            });
            if (token.IsCancellationRequested)
                return null;

            return result;
        }
        public List<int> ParallelBucketSort(List<int> numbers, CancellationToken token)
        {
            // PREPARE
            int threadsNum = System.Environment.ProcessorCount;
            int bucketNum = threadsNum;

            int step = 0;
            InitSteps(2 + 2 * bucketNum);

            int min = numbers[0];
            int max = numbers[0];
            foreach (var number in numbers)
            {
                if (number < min)
                    min = number;
                if (number > max)
                    max = number;
            }
            int bucketSize = (max - min + 1) / bucketNum;
            MakeStep(++step);
            if (token.IsCancellationRequested)
                return null;

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
            MakeStep(++step);
            if (token.IsCancellationRequested)
                return null;

            // SORT
            Stopwatch sortWatch = new Stopwatch();
            sortWatch.Start();

            void RunSort(object obj)
            {
                Stopwatch thWatch = new Stopwatch();
                thWatch.Start();
                int thNum = Thread.CurrentThread.ManagedThreadId;
                List<int> bucket = obj as List<int>;
                bucket.Sort();
                thWatch.Stop();

                MakeStep(Interlocked.Increment(ref step));
            }

            Thread[] threads = new Thread[bucketNum];
            for (int i = 0; i < bucketNum; i++)
            {
                threads[i] = new Thread(RunSort);
                threads[i].Start(buckets[i]);
            }

            for (int i = 0; i < threads.Length; i++)
                threads[i].Join();
            sortWatch.Stop();
            if (token.IsCancellationRequested)
                return null;


            // COLLECT
            List<int> result = new List<int>(numbers.Count);
            for (int i = 0; i < buckets.Count; i++)
            {
                var bucket = buckets[i];
                result.AddRange(bucket);
                //DisposeList(ref bucket);

                MakeStep(++step);
                if (token.IsCancellationRequested)
                    return null;
            }

            return result;
        }
        // cancellation is off      
        public List<int> ParallelQuickSort(List<int> Numbers, CancellationToken Token)
        {
            // Cancellation is OFF because recursion have much steps => slow working with asking token about IsCancel
            //int step = 0;
            InitSteps(-1);

            List<int> PQS(object obj)
            {
                object[] objects = obj as object[];
                List<int> numbers = objects[0] as List<int>;
                CancellationToken token = (CancellationToken)objects[1];

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
                    Task<List<int>> tLeft = new Task<List<int>>(PQS, new object[] { left, token });
                    Task<List<int>> tRight = new Task<List<int>>(PQS, new object[] { right, token });
                    tLeft.Start();
                    tRight.Start();
                    Task.WaitAll(tLeft, tRight);
                    leftSorted = tLeft.Result;
                    rightSorted = tRight.Result;
                }
                else
                {
                    leftSorted = PQS(new object[] { left, token });
                    rightSorted = PQS(new object[] { right, token });
                }

                /*if (token.IsCancellationRequested)
                    return null;
                MakeStep(Interlocked.Increment(ref step));*/

                List<int> result = new List<int>();
                result.AddRange(leftSorted);
                result.AddRange(pivot);
                result.AddRange(rightSorted);

                /*leftSorted = rightSorted = pivot = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();*/
                return result;
            }

            return PQS(new object[] { Numbers, Token });
        }
        public void ParallelShellSort(List<int> numbers, CancellationToken token)
        {
            // PREPARE BLOCKS
            int threadsNum = System.Environment.ProcessorCount;   // p = q / 2
            int blocksNum = threadsNum * 2;    // q = 2^N
            int blockSize = numbers.Count / blocksNum;
            int iterNum = IntToBinaryString(blocksNum - 1).TrimStart('0').Length; // for STEP = 2, used below in code

            int step = 0;
            InitSteps(blocksNum + iterNum * blocksNum / 2 + 1);

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
            Parallel.ForEach(blocks, block =>
            {
                numbers.Sort(block.Key, (block.Value - block.Key), null);
                MakeStep(Interlocked.Increment(ref step));
            });
            if (token.IsCancellationRequested)
                return;

            // STEP 2 - ITERATIONS MERGE-SPLIT FOR BLOCKS
            /*2 этап: N итераций merge-split для блоков
                на каждой i-итерации взаимодействуют блоки, номера которых
                различаются только в (N-i)-разряде в битовом представлении*/
            // iterNum = N - was defined higher in code
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

                    MakeStep(Interlocked.Increment(ref step));
                });
                if (token.IsCancellationRequested)
                    return;
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
                if (token.IsCancellationRequested)
                    return;

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
                if (token.IsCancellationRequested)
                    return;

                // ALL ARRAY IS SORTED
                var test = evenSorted.Union(oddSorted);
                if (!test.Contains(false))
                    break;
            }
            MakeStep(++step);

        }
        public void ParallelBitonicSort(List<int> numbers, CancellationToken token)
        {
            if (IntToBinaryString(numbers.Count).Trim('0').Length != 1)
            {
                Console.WriteLine("Failure. Numbers count != 2^N");
                throw new BitonicSortException("For Bitonic sort numbers count must be = 2^N");
            }

            int step = 0;
            int needSteps = 0;
            for (int bmSize = 2; bmSize <= numbers.Count / 2; bmSize *= 2)
                needSteps++;
            InitSteps(needSteps + 1);

            // MAKE BITONIC SEQUENCE : M(2) + M(4) + M(8) + ... + M(N/2)
            for (int bmSize = 2; bmSize <= numbers.Count / 2; bmSize *= 2)
            {
                var steppedNumbers = Enumerable.Range(0, numbers.Count / bmSize).Select(i => i * bmSize);
                Parallel.ForEach(steppedNumbers, i => BMrec(i, bmSize, IsAsc(i, bmSize)));

                MakeStep(++step);
                if (token.IsCancellationRequested)
                    return;
            }

            // SORT BITONIC SEQUENCE : B(N), B(N/2), B(N/4), ... B(2)
            BMrec(0, numbers.Count, true);
            MakeStep(++step);

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
        }

        //////////////////////////////////////////

        public void InnerSort(List<int> numbers)
        {
            numbers.Sort();
        }
        public void BitonicSort(List<int> numbers, CancellationToken token)
        {
            if (IntToBinaryString(numbers.Count).Trim('0').Length != 1)
            {
                Console.WriteLine("Failure. Numbers count != 2^N");
                throw new BitonicSortException("For Bitonic sort numbers count must be = 2^N");
            }

            int step = 0;
            int needSteps = 0;
            for (int bmSize = 2; bmSize <= numbers.Count / 2; bmSize *= 2)
                needSteps++;
            InitSteps(needSteps + 1);

            // MAKE BITONIC SEQUENCE : M(2) + M(4) + M(8) + ... + M(N/2)
            for (int bmSize = 2; bmSize <= numbers.Count / 2; bmSize *= 2)
            {
                for (int i = 0; i < numbers.Count; i += bmSize)
                    BMrec(i, bmSize, IsAsc(i, bmSize));

                MakeStep(++step);
                if (token.IsCancellationRequested)
                    return;
            }

            // SORT BITONIC SEQUENCE : B(N), B(N/2), B(N/4), ... B(2)
            BMrec(0, numbers.Count, true);
            MakeStep(++step);

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
        }
        public List<int> BucketSort(List<int> numbers, CancellationToken token)
        {
            int threadsNum = System.Environment.ProcessorCount;
            int bucketNum = threadsNum;

            int step = 0;
            InitSteps(2 + 2 * bucketNum);

            int min = numbers[0];
            int max = numbers[0];
            foreach (var number in numbers)
            {
                if (number < min)
                    min = number;
                if (number > max)
                    max = number;
            }
            MakeStep(++step);
            if (token.IsCancellationRequested)
                return null;

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
            MakeStep(++step);
            if (token.IsCancellationRequested)
                return null;

            // SORT
            Stopwatch sortWatch = new Stopwatch();
            sortWatch.Start();
            foreach (var bucket in buckets)
            {
                bucket.Sort();

                MakeStep(++step);
                if (token.IsCancellationRequested)
                    return null;
            }

            sortWatch.Stop();

            // COLLECT
            List<int> result = new List<int>();
            for (int i = 0; i < bucketNum; i++)
            {
                result.AddRange(buckets[i]);
                var temp = buckets[i];
                //DisposeList(ref temp);

                MakeStep(++step);
                if (token.IsCancellationRequested)
                    return null;
            }

            return result;
        }
        public List<int> EnumerationSort(List<int> numbers, CancellationToken token)
        {
            int step = 0;
            InitSteps(numbers.Count);

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

                MakeStep(++step);
                if (token.IsCancellationRequested)
                    return null;
            }

            return result;
        }
        // cancellation is off 
        public List<int> MergeSort(List<int> Numbers, CancellationToken Token)
        {
            // Cancellation is OFF because recursion have much steps => slow working with asking token about IsCancel
            //int step = 0;
            //InitSteps(Numbers.Count - 1);
            InitSteps(-1);

            List<int> MS(List<int> numbers, CancellationToken token)
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

                List<int> leftSorted = MS(left, token);
                List<int> rightSorted = MS(right, token);

                /*if (token.IsCancellationRequested)
                    return null;
                MakeStep(++step);*/

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

            return MS(Numbers, Token);
        }
        // cancellation is off 
        public List<int> QuickSort(List<int> Numbers, CancellationToken Token)
        {
            // Cancellation is OFF because recursion have much steps => slow working with asking token about IsCancel
            //int step = 0;
            InitSteps(-1);

            List<int> QS(List<int> numbers, CancellationToken token)
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

                List<int> leftSorted = QS(left, token);
                List<int> rightSorted = QS(right, token);

                /*if (token.IsCancellationRequested)
                    return null;
                MakeStep(++step);*/

                List<int> result = new List<int>();
                result.AddRange(leftSorted);
                result.AddRange(pivot);
                result.AddRange(rightSorted);

                /*leftSorted = rightSorted = pivot = null;
                GC.Collect();
                GC.WaitForPendingFinalizers();*/

                return result;
            }

            return QS(Numbers, Token);
        }
        public void BubbleSort(List<int> numbers, CancellationToken token)
        {
            int step = 0;
            InitSteps(numbers.Count - 1);

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

                MakeStep(++step);
                if (token.IsCancellationRequested)
                    return;
            }
        }
        public void OddEvenSort(List<int> numbers, CancellationToken token)
        {
            int step = 0;
            InitSteps(numbers.Count / 2);

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

                MakeStep(++step);
                if (token.IsCancellationRequested)
                    return;
            }
        }



        //////////////////////////////////////////

        // GUI UPDATE
        void InitSteps(int needSteps)
        {
            form.Invoke(updater, new object[] { "steps_passed", 0 } as object);
            if (needSteps == -1)
                form.Invoke(updater, new object[] { "need_steps", "Undefined" } as object);
            else
                form.Invoke(updater, new object[] { "need_steps", needSteps } as object);
        }
        void MakeStep(int step)
        {
            form.Invoke(updater, new object[] { "steps_passed", step } as object);
        }


        // FILE
        public static string FileSize(long sizeBytes)
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
        List<int> ReadFileToList(int numbersCount)
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
        void ReadFile(int numbersCount)
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
        void WriteFile(int numbersCount)
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
                        int value = rand.Next(0, maxNumber);
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

        // LIST
        public List<int> GenerateList(int numbersCount, SortType sortType)
        {
            List<int> numbers = new List<int>(numbersCount);
            Random random = new Random();
            for (int i = 0; i < numbersCount; i++)
                numbers.Add(random.Next(0, maxNumber));

            if (sortType == SortType.ASC_SORTED)
                numbers.Sort();
            else if (sortType == SortType.DESC_SORTED)
            {
                numbers.Sort();
                numbers.Reverse();
            }
            return numbers;
        }
        public void DisposeList(ref List<int> numbers)
        {
            numbers = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        public string IntToBinaryString(int number)
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
    public class BitonicSortException : Exception
    {
        public BitonicSortException(string message) : base(message) { }
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
