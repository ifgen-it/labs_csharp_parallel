#define SEQUENT
#define SEQUENT_LINQ
#define PLINQ
#define PARALLEL_FOR_FOREACH

//#define TOP_10
//#define PRINT_WORD_LENGTH


using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Practice4
{
    class Program
    {

        static string dirPath = "..\\..\\..\\text_files";
        static char[] delimeters = { ' ', '\n', ',', '.', ';', ':', '!', '?', '\t', '\r' };


        static void Main(string[] args)
        {
            /**
             * Практическая работа №4.
             * Обработка текстовых файлов с помощью средств библиотеки TPL
             *
             * Реализовать приложение для обработки текстовых файлов.
             * Задача:
             * Реализовать последовательные и параллельные алгоритмы
             * решения следующих задач:
             *    ‐ получение общей статистики по встречаемости слов;
             *    ‐ поиск только 10 самых часто встречающихся слов;
             *    ‐ получение распределения числа слов по длине.
             *    
             * *****************************************************   
             * РЕЗУЛЬТАТЫ:
             * 
             * 74 файла, общий размер 285 Мб.
             * 23,2 сек. - последовательный алгоритм
             * 23,4 сек. - LINQ
             * 25,7 сек. - PLINQ - разные типы буферизации, принудит. паралел., масксим. колич. потоков - не улучшают результат
             * 13,0 сек. - Parallel.For
             * 
             */
            Console.WriteLine("====== Practice 4 ======\n");

            /**
             * 1. Последовательный алгоритм без средств TPL и LINQ.
             */

            #region SEQUENT
#if SEQUENT
            Console.WriteLine("====== Sequential algorithm, without TPL and LINQ ======\n");

            // READ ALL FILES AND MAKE DICTIONARY
            Stopwatch watch = new Stopwatch();
            watch.Start();
            IEnumerable<string> fileNames = Directory.EnumerateFiles(dirPath);
            List<FileInfo> files = fileNames.Select(fn => new FileInfo(fn)).ToList();
            long sizeBytes = 0;
            int wordCount = 0;
            Dictionary<string, int> dicWords = new Dictionary<string, int>();

            Console.WriteLine("Reading files...");
            foreach (var file in files)
            {
                sizeBytes += file.Length;
                using (StreamReader sr = new StreamReader(file.FullName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        List<string> lineWords = line.Split(delimeters, StringSplitOptions.RemoveEmptyEntries).ToList();
                        wordCount += lineWords.Count;

                        foreach (var word in lineWords)
                        {
                            if (!dicWords.ContainsKey(word))
                                dicWords.Add(word, 1);
                            else
                                dicWords[word]++;
                        }
                    }
                }
                Console.Write("#");
            }
            Console.WriteLine("\n");
            List<KeyValuePair<string, int>> listWords = dicWords.ToList();
            listWords.Sort((x, y) =>
            {
                return x.Value < y.Value ? 1 : x.Value > y.Value ? -1 : x.Key.CompareTo(y.Key);
            });

            // TOP 10
            List<KeyValuePair<string, int>> top10Words = new List<KeyValuePair<string, int>>();
            for (int i = 0; i < 10; i++)
            {
                if (i == listWords.Count)
                    break;
                top10Words.Add(listWords[i]);
            }

            // WORD LENGTH
            Dictionary<int, int> dicWordsLength = new Dictionary<int, int>();
            foreach (var pair in listWords)
            {
                int length = pair.Key.Length;
                int count = pair.Value;
                if (!dicWordsLength.ContainsKey(length))
                    dicWordsLength.Add(length, count);
                else
                    dicWordsLength[length] += count;
            }
            List<KeyValuePair<int, int>> listWordsLength = dicWordsLength.ToList();
            listWordsLength.Sort((x, y) =>
            {
                return x.Value < y.Value ? 1 : x.Value > y.Value ? -1 :
                x.Key > y.Key ? 1 : x.Key < y.Key ? -1 :
                0;
            });
            watch.Stop();

            Console.WriteLine("Statistics:");
            Console.WriteLine("-----------");
            Console.WriteLine($"Was read...................{files.Count} files, total size = {FileSize(sizeBytes)}");
            Console.WriteLine($"Processing time............{watch.ElapsedMilliseconds / 1000f:f3} sec");
            Console.WriteLine($"Words......................{wordCount}");
            Console.WriteLine($"Different words............{dicWords.Count}");
            Console.WriteLine($"Most frequent word.........{listWords[0].Key} - {listWords[0].Value} times");
            Console.WriteLine($"Least frequent word........{listWords[listWords.Count - 1].Key} - {listWords[listWords.Count - 1].Value} times");
#if TOP_10
            Console.WriteLine("\nTop 10 most frequent words:");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("| Pos | Word         | Count      |");
            Console.WriteLine("-----------------------------------");
            for (int i = 0; i < top10Words.Count; i++)
                Console.WriteLine($"| {i + 1,-3} | {top10Words[i].Key,-12} | {top10Words[i].Value,-10} |");
            Console.WriteLine("-----------------------------------");
#endif
#if PRINT_WORD_LENGTH
            Console.WriteLine($"\nWord length and count of words:");
            Console.WriteLine("---------------------------");
            Console.WriteLine("|  Length   |  Count      |");
            Console.WriteLine("---------------------------");
            foreach (var pair in listWordsLength)
                Console.WriteLine($"|  {pair.Key,-8} |  {pair.Value,-10} |");
            Console.WriteLine("---------------------------");
#endif
#endif
            #endregion


            /**
             * 2. Алгоритм с применением LINQ‐запросов
             */

            #region SEQUENT_LINQ
#if SEQUENT_LINQ
            Console.WriteLine("\n====== Sequential algorithm with LINQ ======\n");

            // READ ALL FILES AND MAKE DICTIONARY
            Stopwatch watch1 = new Stopwatch();
            watch1.Start();
            IEnumerable<string> fileNames1 = Directory.EnumerateFiles(dirPath);
            List<FileInfo> files1 = fileNames1.Select(fn => new FileInfo(fn)).ToList();
            long sizeBytes1 = 0;
            int wordCount1 = 0;
            Console.WriteLine("Reading files...");

            List<KeyValuePair<string, int>> listWords1 = files1
                .SelectMany(file => {
                    Console.Write("#");
                    sizeBytes1 += file.Length;
                    return File.ReadLines(file.FullName).SelectMany(line => line.Split(delimeters, StringSplitOptions.RemoveEmptyEntries));
                })
                .Aggregate(new Dictionary<string, int>(), (dic, w) =>
                {
                    wordCount1++;
                    if (dic.ContainsKey(w))
                        dic[w]++;
                    else
                        dic.Add(w, 1);
                    return dic;
                })
                .OrderBy(pair => -pair.Value)
                .ThenBy(pair => pair.Key)
                .ToList();

            Console.WriteLine("\n");

            // TOP 10
            List<KeyValuePair<string, int>> top10Words1 = listWords1.Take(10).ToList();

            // WORD LENGTH
            List<KeyValuePair<int, int>> listWordsLength1 = listWords1
               .Aggregate(new Dictionary<int, int>(), (dic, pair) =>
               {
                   int wordLength = pair.Key.Length;
                   int count = pair.Value;
                   if (dic.ContainsKey(wordLength))
                       dic[wordLength] += count;
                   else
                       dic.Add(wordLength, count);
                   return dic;
               })
               .OrderBy(pair => -pair.Value)
               .ThenBy(pair => pair.Key)
               .ToList();

            watch1.Stop();

            Console.WriteLine("Statistics:");
            Console.WriteLine("-----------");
            Console.WriteLine($"Was read...................{files1.Count} files, total size = {FileSize(sizeBytes1)}");
            Console.WriteLine($"Processing time............{watch1.ElapsedMilliseconds / 1000f:f3} sec");
            Console.WriteLine($"Words......................{wordCount1}");
            Console.WriteLine($"Different words............{listWords1.Count}");
            Console.WriteLine($"Most frequent word.........{listWords1[0].Key} - {listWords1[0].Value} times");
            Console.WriteLine($"Least frequent word........{listWords1[listWords1.Count - 1].Key} - {listWords1[listWords1.Count - 1].Value} times");
#if TOP_10
            Console.WriteLine("\nTop 10 most frequent words:");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("| Pos | Word         | Count      |");
            Console.WriteLine("-----------------------------------");
            int top10Words1Pos = 1;
            foreach (var pair in top10Words1)
                Console.WriteLine($"| {top10Words1Pos++,-3} | {pair.Key,-12} | {pair.Value,-10} |");
            Console.WriteLine("-----------------------------------");
#endif
#if PRINT_WORD_LENGTH
            Console.WriteLine($"\nWord length and count of words:");
            Console.WriteLine("---------------------------");
            Console.WriteLine("|  Length   |  Count      |");
            Console.WriteLine("---------------------------");
            foreach (var pair in listWordsLength1)
                Console.WriteLine($"|  {pair.Key,-8} |  {pair.Value,-10} |");
            Console.WriteLine("---------------------------");
#endif

#endif

            #endregion

            /**
             * 4. Алгоритм с применением PLINQ‐запросов - версия 2
             */

            #region PLINQ
#if PLINQ
            Console.WriteLine("\n====== Parallel algorithm with PLINQ ======\n");

            // READ ALL FILES AND MAKE DICTIONARY
            Stopwatch watch3 = new Stopwatch();
            watch3.Start();
            IEnumerable<string> fileNames3 = Directory.EnumerateFiles(dirPath);
            List<FileInfo> files3 = fileNames3.Select(fn => new FileInfo(fn)).ToList();
            long sizeBytes3 = 0;
            int wordCount3 = 0;
            Console.WriteLine("Reading files...");
            
            Dictionary<string, int> dicWords3 = new Dictionary<string, int>();

            List<KeyValuePair<string, int>> listWords3 = Partitioner.Create(files3, true)
                .AsParallel()
                //.WithExecutionMode(ParallelExecutionMode.ForceParallelism)
                //.WithMergeOptions(ParallelMergeOptions.FullyBuffered)
                //.WithDegreeOfParallelism(16)
                .SelectMany(file => {
                    Interlocked.Add(ref sizeBytes3, file.Length);
                    Console.Write("#");
                    return File.ReadLines(file.FullName).SelectMany(line => line.Split(delimeters,StringSplitOptions.RemoveEmptyEntries));
                })
                .Aggregate(new Dictionary<string, int>(), (dic, word) => 
                {
                    Interlocked.Increment(ref wordCount3);
                    if (!dic.ContainsKey(word))
                        dic.Add(word, 1);
                    else
                        dic[word]++;
                    return dic;
                }, dic =>
                {
                    foreach (var pair in dic)
                    {
                        string word = pair.Key;
                        int count = pair.Value;
                        if (!dicWords3.ContainsKey(word))
                            dicWords3.Add(word, count);
                        else
                            dicWords3[word]+= count;
                    }
                    return dicWords3;
                })
                .OrderBy(pair => -pair.Value)
                .ThenBy(pair => pair.Key)
                .ToList();
            
            Console.WriteLine("\n");

            // TOP 10
            List<KeyValuePair<string, int>> top10Words3 = listWords3.Take(10).ToList();

            // WORD LENGTH
            List<KeyValuePair<int, int>> listWordsLength3 = listWords3
               .Aggregate(new Dictionary<int, int>(), (dic, pair) =>
               {
                   int wordLength = pair.Key.Length;
                   int count = pair.Value;
                   if (dic.ContainsKey(wordLength))
                       dic[wordLength] += count;
                   else
                       dic.Add(wordLength, count);
                   return dic;
               })
               .OrderBy(pair => -pair.Value)
               .ThenBy(pair => pair.Key)
               .ToList();

            watch3.Stop();

            Console.WriteLine("Statistics:");
            Console.WriteLine("-----------");
            Console.WriteLine($"Was read...................{files3.Count} files, total size = {FileSize(sizeBytes3)}");
            Console.WriteLine($"Processing time............{watch3.ElapsedMilliseconds / 1000f:f3} sec");
            Console.WriteLine($"Words......................{wordCount3}");
            Console.WriteLine($"Different words............{listWords3.Count}");
            Console.WriteLine($"Most frequent word.........{listWords3[0].Key} - {listWords3[0].Value} times");
            Console.WriteLine($"Least frequent word........{listWords3[listWords3.Count - 1].Key} - {listWords3[listWords3.Count - 1].Value} times");
#if TOP_10
            Console.WriteLine("\nTop 10 most frequent words:");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("| Pos | Word         | Count      |");
            Console.WriteLine("-----------------------------------");
            int top10Words3Pos = 1;
            foreach (var pair in top10Words3)
                Console.WriteLine($"| {top10Words3Pos++,-3} | {pair.Key,-12} | {pair.Value,-10} |");
            Console.WriteLine("-----------------------------------");
#endif
#if PRINT_WORD_LENGTH
            Console.WriteLine($"\nWord length and count of words:");
            Console.WriteLine("---------------------------");
            Console.WriteLine("|  Length   |  Count      |");
            Console.WriteLine("---------------------------");
            foreach (var pair in listWordsLength3)
                Console.WriteLine($"|  {pair.Key,-8} |  {pair.Value,-10} |");
            Console.WriteLine("---------------------------");
#endif

#endif

            #endregion


            /**
             * 3. Алгоритм на базе методов Parallel.For или Parallel.ForEach
             */

            #region PARALLEL_FOR_FOREACH
#if PARALLEL_FOR_FOREACH
            Console.WriteLine("\n====== Parallel algorithm with Parallel.For and Parallel.ForEach ======\n");

            // READ ALL FILES AND MAKE DICTIONARY
            Stopwatch watch4 = new Stopwatch();
            watch4.Start();
            IEnumerable<string> fileNames4 = Directory.EnumerateFiles(dirPath);
            List<FileInfo> files4 = fileNames4.Select(fn => new FileInfo(fn)).ToList();
            long sizeBytes4 = 0;
            int wordCount4 = 0;

            ConcurrentDictionary<string, int> dicWords4 = new ConcurrentDictionary<string, int>();
            Console.WriteLine("Reading files...");

            Parallel.For(0, files4.Count,
                () => new Dictionary<string, int>(),
                (i, state, local) =>
                {
                    Interlocked.Add(ref sizeBytes4, files4[i].Length);
                    using (StreamReader sr = new StreamReader(files4[i].FullName))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            List<string> lineWords = line.Split(delimeters, StringSplitOptions.RemoveEmptyEntries).ToList();
                            Interlocked.Add(ref wordCount4, lineWords.Count);
                            foreach (var word in lineWords)
                            {
                                if (!local.ContainsKey(word))
                                    local.Add(word, 1);
                                else
                                    local[word]++;
                            }
                        }
                    }
                    Console.Write("#");
                    return local;
                },
                local =>
                {
                    foreach (var pair in local)
                    {
                        string word = pair.Key;
                        int count = pair.Value;
                        dicWords4.AddOrUpdate(word, count, (key, oldValue) => oldValue + count);
                    }
                }
                );

            Console.WriteLine("\n");
            List<KeyValuePair<string, int>> listWords4 = dicWords4.ToList();
            listWords4.Sort((x, y) =>
            {
                return x.Value < y.Value ? 1 : x.Value > y.Value ? -1 : x.Key.CompareTo(y.Key);
            });

            // TOP 10
            List<KeyValuePair<string, int>> top10Words4 = new List<KeyValuePair<string, int>>();
            for (int i = 0; i < 10; i++)
            {
                if (i == listWords4.Count)
                    break;
                top10Words4.Add(listWords4[i]);
            }

            // WORD LENGTH
            Dictionary<int, int> dicWordsLength4 = new Dictionary<int, int>();
            foreach (var pair in listWords4)
            {
                int length = pair.Key.Length;
                int count = pair.Value;
                if (!dicWordsLength4.ContainsKey(length))
                    dicWordsLength4.Add(length, count);
                else
                    dicWordsLength4[length] += count;
            }
            List<KeyValuePair<int, int>> listWordsLength4 = dicWordsLength4.ToList();
            listWordsLength4.Sort((x, y) =>
            {
                return x.Value < y.Value ? 1 : x.Value > y.Value ? -1 :
                x.Key > y.Key ? 1 : x.Key < y.Key ? -1 :
                0;
            });
            watch4.Stop();

            Console.WriteLine("Statistics:");
            Console.WriteLine("-----------");
            Console.WriteLine($"Was read...................{files4.Count} files, total size = {FileSize(sizeBytes4)}");
            Console.WriteLine($"Processing time............{watch4.ElapsedMilliseconds / 1000f:f3} sec");
            Console.WriteLine($"Words......................{wordCount4}");
            Console.WriteLine($"Different words............{dicWords4.Count}");
            Console.WriteLine($"Most frequent word.........{listWords4[0].Key} - {listWords4[0].Value} times");
            Console.WriteLine($"Least frequent word........{listWords4[listWords4.Count - 1].Key} - {listWords4[listWords4.Count - 1].Value} times");
#if TOP_10
            Console.WriteLine("\nTop 10 most frequent words:");
            Console.WriteLine("-----------------------------------");
            Console.WriteLine("| Pos | Word         | Count      |");
            Console.WriteLine("-----------------------------------");
            for (int i = 0; i < top10Words4.Count; i++)
                Console.WriteLine($"| {i + 1,-3} | {top10Words4[i].Key,-12} | {top10Words4[i].Value,-10} |");
            Console.WriteLine("-----------------------------------");
#endif
#if PRINT_WORD_LENGTH
            Console.WriteLine($"\nWord length and count of words:");
            Console.WriteLine("---------------------------");
            Console.WriteLine("|  Length   |  Count      |");
            Console.WriteLine("---------------------------");
            foreach (var pair in listWordsLength4)
                Console.WriteLine($"|  {pair.Key,-8} |  {pair.Value,-10} |");
            Console.WriteLine("---------------------------");
#endif
#endif
            #endregion



            //////////////////////////////////////////////////////////////
            Console.ReadKey();
        }

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
    }
}
