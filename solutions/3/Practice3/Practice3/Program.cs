using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Practice3
{
    class Program
    {
        #region init
        static string buffer = "___";
        static int writersJobCount = 5;
        static bool bufferIsEmpty = true;
        static bool writersFinished = false;

        static int readersCount = 10;
        static int writersCount = 10;

        static ConcurrentBag<string> wrMessages = new ConcurrentBag<string>();
        static ConcurrentBag<string> rdMessages = new ConcurrentBag<string>();

        // LOCK
        static bool bufferIsEmpty1 = true;
        static bool writersFinished1 = false;
        static ConcurrentBag<string> wrMessages1 = new ConcurrentBag<string>();
        static ConcurrentBag<string> rdMessages1 = new ConcurrentBag<string>();

        // EVENT
        static bool writersFinished2 = false;
        static ConcurrentBag<string> wrMessages2 = new ConcurrentBag<string>();
        static ConcurrentBag<string> rdMessages2 = new ConcurrentBag<string>();

        // SEMAPHORE
        static bool writersFinished3 = false;
        static ConcurrentBag<string> wrMessages3 = new ConcurrentBag<string>();
        static ConcurrentBag<string> rdMessages3 = new ConcurrentBag<string>();

        // SEMAPHORE SLIM
        static bool writersFinished4 = false;
        static ConcurrentBag<string> wrMessages4 = new ConcurrentBag<string>();
        static ConcurrentBag<string> rdMessages4 = new ConcurrentBag<string>();

        // INTERLOCKED
        static int readersCanRead = 0;
        static int writersCanWrite = 1;
        static bool writersFinished5 = false;
        static ConcurrentBag<string> wrMessages5 = new ConcurrentBag<string>();
        static ConcurrentBag<string> rdMessages5 = new ConcurrentBag<string>();
        #endregion

        static void Main(string[] args)
        {
            /**
             * Практика №3. Синхронизация доступа к одноэлементному буферу
             */
            Console.WriteLine("===================== Practice 3 ======================");

            /**
             * 1. Реализуйте взаимодействие потоков-читателей и потоков-писателей
             *    с общим буфером без каких-либо средств синхронизации.
             *    Проиллюстрируйте проблему совместного доступа.
             *    Почему возникает проблема доступа?
             *    
             *    Ответ: Возникает гонка данных - разные результаты получаются в зависимости от того, 
             *    в какой очередности поток выполняет код. Есть участки кода, которые выполнять должен
             *    поток атомарно, но без средств синхронизации сделать это не возможно.
             *    Что получается:
             *      - несколько читателей могут прочитать буфер
             *      - писатели могут затереть буфер, пока его еще не прочитали
             *      - читатели могут прочитать старое значение, которго уже давно нет
             *      - некоторые значения могут быть не прочитаны (не выведены на консоль), успевают новые считаться
             */

            #region WITHOUT_SYNCH
            Console.WriteLine("\n\n========= Interaction WITHOUT SYNCHRONIZATION =========\n");

            DateTime tStart = DateTime.Now;
            Thread[] readers = new Thread[readersCount];
            for (int i = 0; i < readersCount; i++)
            {
                readers[i] = new Thread(ReadFromBuffer);
                readers[i].Name = "Reader" + (i + 1);
                readers[i].Start();
            }

            Thread[] writers = new Thread[writersCount];
            for (int i = 0; i < writersCount; i++)
            {
                writers[i] = new Thread(WriteInBuffer);
                writers[i].Name = "Writer" + (i + 1);
                writers[i].Start(i);
            }

            for (int i = 0; i < writers.Length; i++)
                writers[i].Join();
            writersFinished = true;

            for (int i = 0; i < readers.Length; i++)
                readers[i].Join();
            DateTime tFinish = DateTime.Now;
            TimeSpan tSpan = tFinish - tStart;

            List<string> wrMessageList = wrMessages.ToList();
            List<string> rdMessageList = rdMessages.ToList();

            int distinctRdCnt = rdMessageList.Distinct().Count();
            int repeatedMessagesCnt = rdMessageList.Count - distinctRdCnt;
            int lostMessagesCnt = wrMessageList.Count - distinctRdCnt;

            Console.WriteLine("---------------------");
            Console.WriteLine("\nWITHOUT SYNCH results:");
            Console.WriteLine($"\nTime span = {tSpan.TotalMilliseconds} ms");
            Console.WriteLine($"Repeated messages in readers = {repeatedMessagesCnt}");
            Console.WriteLine($"Losted messages from writers = {lostMessagesCnt}");
            #endregion

            /**
             * 2. Реализуйте доступ «читателей» и «писателей» к буферу с применением
             *    следующих средств синхронизации:
             *       - блокировки (lock)
             *       
             *    Работает правильно - гонки данных нет
             */

            #region WITH_LOCK
            Console.WriteLine("\n\n========= Interaction WITH LOCK =========\n");

            DateTime tStart1 = DateTime.Now;
            Thread[] readers1 = new Thread[readersCount];
            for (int i = 0; i < readersCount; i++)
            {
                readers1[i] = new Thread(ReadFromBuffer_Lock);
                readers1[i].Name = "Reader" + (i + 1);
                readers1[i].Start();
            }

            Thread[] writers1 = new Thread[writersCount];
            for (int i = 0; i < writersCount; i++)
            {
                writers1[i] = new Thread(WriteInBuffer_Lock);
                writers1[i].Name = "Writer" + (i + 1);
                writers1[i].Start(i);
            }

            for (int i = 0; i < writers1.Length; i++)
                writers1[i].Join();
            writersFinished1 = true;

            for (int i = 0; i < readers1.Length; i++)
                readers1[i].Join();
            DateTime tFinish1 = DateTime.Now;
            TimeSpan tSpan1 = tFinish1 - tStart1;

            List<string> wrMessageList1 = wrMessages1.ToList();
            List<string> rdMessageList1 = rdMessages1.ToList();

            int distinctRdCnt1 = rdMessageList1.Distinct().Count();
            int repeatedMessagesCnt1 = rdMessageList1.Count - distinctRdCnt1;
            int lostMessagesCnt1 = wrMessageList1.Count - distinctRdCnt1;

            Console.WriteLine("---------------------");
            Console.WriteLine("\nWITH LOCK results:");
            Console.WriteLine($"\nTime span = {tSpan1.TotalMilliseconds} ms");
            Console.WriteLine($"Repeated messages in readers = {repeatedMessagesCnt1}");
            Console.WriteLine($"Losted messages from writers = {lostMessagesCnt1}");
            #endregion

            /**
             * 2. Реализуйте доступ «читателей» и «писателей» к буферу с применением
             *    следующих средств синхронизации:
             *       - сигнальные сообщения (ManualResetEvent,
             *         AutoResetEvent, ManualResetEventSlim);
             *         
             *    Ответ: использован AutoResetEvent - аналог Lock, всегда только один 
             *    поток получает возможность выполнения, остальные ждут очереди.
             *    
             *    Выполнение ускоряется, т.к. потоки не делают теперь бесполезную работу в цикле, ожидая
             *    изменения флага, - как в WITH_LOCK. Потоки теперь выгружаются и не занимают процессорное
             *    время, пока не будет установлен сигнал для ожидаемого события.
             */

            #region WITH_EVENT
            Console.WriteLine("\n\n========= Interaction WITH EVENT =========\n");

            AutoResetEvent evEmpty = new AutoResetEvent(true);
            AutoResetEvent evBusy = new AutoResetEvent(false);

            DateTime tStart2 = DateTime.Now;
            Thread[] readers2 = new Thread[readersCount];
            for (int i = 0; i < readersCount; i++)
            {
                readers2[i] = new Thread(ReadFromBuffer_Event);
                readers2[i].Name = "Reader" + (i + 1);
                readers2[i].Start(new object[] { evEmpty, evBusy });
            }

            Thread[] writers2 = new Thread[writersCount];
            for (int i = 0; i < writersCount; i++)
            {
                writers2[i] = new Thread(WriteInBuffer_Event);
                writers2[i].Name = "Writer" + (i + 1);
                writers2[i].Start(new object[] { evEmpty, evBusy, i });
            }

            for (int i = 0; i < writers2.Length; i++)
                writers2[i].Join();
            writersFinished2 = true;
            evBusy.Set();

            for (int i = 0; i < readers2.Length; i++)
                readers2[i].Join();
            DateTime tFinish2 = DateTime.Now;
            TimeSpan tSpan2 = tFinish2 - tStart2;

            List<string> wrMessageList2 = wrMessages2.ToList();
            List<string> rdMessageList2 = rdMessages2.ToList();

            int distinctRdCnt2 = rdMessageList2.Distinct().Count();
            int repeatedMessagesCnt2 = rdMessageList2.Count - distinctRdCnt2;
            int lostMessagesCnt2 = wrMessageList2.Count - distinctRdCnt2;

            Console.WriteLine("---------------------");
            Console.WriteLine("\nWITH EVENT results:");
            Console.WriteLine($"\nTime span = {tSpan2.TotalMilliseconds} ms");
            Console.WriteLine($"Repeated messages in readers = {repeatedMessagesCnt2}");
            Console.WriteLine($"Losted messages from writers = {lostMessagesCnt2}");

            #endregion

            /**
             * 2. Реализуйте доступ «читателей» и «писателей» к буферу с применением
             *    следующих средств синхронизации:
             *       - семафоры (Semaphore). 
             */

            #region WITH_SEMAPHORE
            Console.WriteLine("\n\n========= Interaction WITH SEMAPHORE =========\n");

            Semaphore semEmpty = new Semaphore(1, 1);
            Semaphore semBusy = new Semaphore(0, 1);

            DateTime tStart3 = DateTime.Now;
            Thread[] readers3 = new Thread[readersCount];
            for (int i = 0; i < readersCount; i++)
            {
                readers3[i] = new Thread(ReadFromBuffer_Sem);
                readers3[i].Name = "Reader" + (i + 1);
                readers3[i].Start(new object[] { semEmpty, semBusy });
            }

            Thread[] writers3 = new Thread[writersCount];
            for (int i = 0; i < writersCount; i++)
            {
                writers3[i] = new Thread(WriteInBuffer_Sem);
                writers3[i].Name = "Writer" + (i + 1);
                writers3[i].Start(new object[] { semEmpty, semBusy, i });
            }

            for (int i = 0; i < writers3.Length; i++)
                writers3[i].Join();
            writersFinished3 = true;
            semBusy.Release();

            for (int i = 0; i < readers3.Length; i++)
                readers3[i].Join();
            DateTime tFinish3 = DateTime.Now;
            TimeSpan tSpan3 = tFinish3 - tStart3;

            List<string> wrMessageList3 = wrMessages3.ToList();
            List<string> rdMessageList3 = rdMessages3.ToList();

            int distinctRdCnt3 = rdMessageList3.Distinct().Count();
            int repeatedMessagesCnt3 = rdMessageList3.Count - distinctRdCnt3;
            int lostMessagesCnt3 = wrMessageList3.Count - distinctRdCnt3;

            Console.WriteLine("---------------------");
            Console.WriteLine("\nWITH SEMAPHORE results:");
            Console.WriteLine($"\nTime span = {tSpan3.TotalMilliseconds} ms");
            Console.WriteLine($"Repeated messages in readers = {repeatedMessagesCnt3}");
            Console.WriteLine($"Losted messages from writers = {lostMessagesCnt3}");
            #endregion

            /**
             * 2. Реализуйте доступ «читателей» и «писателей» к буферу с применением
             *    следующих средств синхронизации:
             *       - семафоры (SemaphoreSlim).    
             */

            #region WITH_SEMAPHORE_SLIM
            Console.WriteLine("\n\n========= Interaction WITH SEMAPHORE SLIM =========\n");

            SemaphoreSlim semSlimEmpty = new SemaphoreSlim(1, 1);
            SemaphoreSlim semSlimBusy = new SemaphoreSlim(0, 1);

            DateTime tStart4 = DateTime.Now;
            Thread[] readers4 = new Thread[readersCount];
            for (int i = 0; i < readersCount; i++)
            {
                readers4[i] = new Thread(ReadFromBuffer_SemSlim);
                readers4[i].Name = "Reader" + (i + 1);
                readers4[i].Start(new object[] { semSlimEmpty, semSlimBusy });
            }

            Thread[] writers4 = new Thread[writersCount];
            for (int i = 0; i < writersCount; i++)
            {
                writers4[i] = new Thread(WriteInBuffer_SemSlim);
                writers4[i].Name = "Writer" + (i + 1);
                writers4[i].Start(new object[] { semSlimEmpty, semSlimBusy, i });
            }

            for (int i = 0; i < writers4.Length; i++)
                writers4[i].Join();
            writersFinished4 = true;
            semSlimBusy.Release();

            for (int i = 0; i < readers4.Length; i++)
                readers4[i].Join();
            DateTime tFinish4 = DateTime.Now;
            TimeSpan tSpan4 = tFinish4 - tStart4;

            List<string> wrMessageList4 = wrMessages4.ToList();
            List<string> rdMessageList4 = rdMessages4.ToList();

            int distinctRdCnt4 = rdMessageList4.Distinct().Count();
            int repeatedMessagesCnt4 = rdMessageList4.Count - distinctRdCnt4;
            int lostMessagesCnt4 = wrMessageList4.Count - distinctRdCnt4;

            Console.WriteLine("---------------------");
            Console.WriteLine("\nWITH SEMAPHORE SLIM results:");
            Console.WriteLine($"\nTime span = {tSpan4.TotalMilliseconds} ms");
            Console.WriteLine($"Repeated messages in readers = {repeatedMessagesCnt4}");
            Console.WriteLine($"Losted messages from writers = {lostMessagesCnt4}");
            #endregion

            /**
             * 2. Реализуйте доступ «читателей» и «писателей» к буферу с применением
             *    следующих средств синхронизации:
             *       - атомарные операторы (Interlocked)
             *       
             *    Применение оператора Interlocked недостаточно, чтобы избежать гонки данных.
             *    Т.к. он не охватывает весь кусок кода, который должен выполняться атомарно.
             */

            #region WITH_INTERLOCKED
            Console.WriteLine("\n\n========= Interaction WITH INTERLOCKED =========\n");

            DateTime tStart5 = DateTime.Now;
            Thread[] readers5 = new Thread[readersCount];
            for (int i = 0; i < readersCount; i++)
            {
                readers5[i] = new Thread(ReadFromBuffer_Interlocked);
                readers5[i].Name = "Reader" + (i + 1);
                readers5[i].Start();
            }

            Thread[] writers5 = new Thread[writersCount];
            for (int i = 0; i < writersCount; i++)
            {
                writers5[i] = new Thread(WriteInBuffer_Interlocked);
                writers5[i].Name = "Writer" + (i + 1);
                writers5[i].Start(i);
            }

            for (int i = 0; i < writers1.Length; i++)
                writers5[i].Join();
            writersFinished5 = true;

            for (int i = 0; i < readers5.Length; i++)
                readers5[i].Join();
            DateTime tFinish5 = DateTime.Now;
            TimeSpan tSpan5 = tFinish5 - tStart5;

            List<string> wrMessageList5 = wrMessages5.ToList();
            List<string> rdMessageList5 = rdMessages5.ToList();

            int distinctRdCnt5 = rdMessageList5.Distinct().Count();
            int repeatedMessagesCnt5 = rdMessageList5.Count - distinctRdCnt5;
            int lostMessagesCnt5 = wrMessageList5.Count - distinctRdCnt5;

            Console.WriteLine("---------------------");
            Console.WriteLine("\nWITH INTERLOCKED results:");
            Console.WriteLine($"\nTime span = {tSpan5.TotalMilliseconds} ms");
            Console.WriteLine($"Repeated messages in readers = {repeatedMessagesCnt5}");
            Console.WriteLine($"Losted messages from writers = {lostMessagesCnt5}");
            #endregion

            //////////////////////////////////////////////////////////////////////
            Console.WriteLine("\n---------------------\n" + ".......Finish.......");
            Console.ReadKey();
        }

        static void WriteInBuffer(object obj)
        {
            int threadNumber = (int)obj;
            string letter = ((char)(threadNumber + (int)'A')).ToString();
            string baseText = letter + letter + letter + letter;

            List<string> messages = new List<string>();
            for (int i = 0; i < writersJobCount; i++)
            {
                messages.Add(baseText + (i + 1));
            }
            messages.ForEach(it => wrMessages.Add(it));

            for (int i = 0; i < messages.Count; i++)
            {
                while (!bufferIsEmpty) ;

                string text = messages[i];
                buffer = text;
                //Console.WriteLine("---------------------\n" + Thread.CurrentThread.Name + " >> " + text + "\n");
                bufferIsEmpty = false;

            }
        }
        static void ReadFromBuffer()
        {
            List<string> messages = new List<string>();
            while (!writersFinished)
            {
                if (!bufferIsEmpty)
                {
                    string text = buffer;
                    messages.Add(text);
                    //Console.WriteLine(Thread.CurrentThread.Name + " << " + text);
                    bufferIsEmpty = true;
                }
            }
            messages.ForEach(it => rdMessages.Add(it));
        }

        static void WriteInBuffer_Lock(object obj)
        {
            int threadNumber = (int)obj;
            string letter = ((char)(threadNumber + (int)'A')).ToString();
            string baseText = letter + letter + letter + letter;

            List<string> messages = new List<string>();
            for (int i = 0; i < writersJobCount; i++)
                messages.Add(baseText + (i + 1));
            messages.ForEach(it => wrMessages1.Add(it));

            int j = 0;
            while (j < messages.Count)
            {
                if (bufferIsEmpty1)
                {
                    lock ("writer_lock")
                    {
                        if (bufferIsEmpty1)
                        {
                            string text = messages[j++];
                            buffer = text;
                            //Console.WriteLine("---------------------\n" + Thread.CurrentThread.Name + " >> " + text + "\n");
                            bufferIsEmpty1 = false;
                        }
                    }
                }
            }

        }
        static void ReadFromBuffer_Lock()
        {
            List<string> messages = new List<string>();
            while (!writersFinished1)
            {
                if (!bufferIsEmpty1)
                {
                    lock ("reader_lock")
                    {
                        if (!bufferIsEmpty1)
                        {
                            string text = buffer;
                            messages.Add(text);
                            //Console.WriteLine(Thread.CurrentThread.Name + " << " + text);
                            bufferIsEmpty1 = true;
                        }
                    }
                }
            }
            messages.ForEach(it => rdMessages1.Add(it));
        }

        static void WriteInBuffer_Event(object obj)
        {
            AutoResetEvent evEmpty = ((object[])obj)[0] as AutoResetEvent;
            AutoResetEvent evBusy = ((object[])obj)[1] as AutoResetEvent;
            int threadNumber = (int)(((object[])obj)[2]);

            string letter = ((char)(threadNumber + (int)'A')).ToString();
            string baseText = letter + letter + letter + letter;

            List<string> messages = new List<string>();
            for (int i = 0; i < writersJobCount; i++)
                messages.Add(baseText + (i + 1));
            messages.ForEach(it => wrMessages2.Add(it));

            int j = 0;
            while (j < messages.Count)
            {
                evEmpty.WaitOne();
                string text = messages[j++];
                buffer = text;
                //Console.WriteLine("---------------------\n" + Thread.CurrentThread.Name + " >> " + text + "\n");
                evBusy.Set();
            }

        }
        static void ReadFromBuffer_Event(object obj)
        {
            AutoResetEvent evEmpty = ((object[])obj)[0] as AutoResetEvent;
            AutoResetEvent evBusy = ((object[])obj)[1] as AutoResetEvent;

            List<string> messages = new List<string>();
            while (!writersFinished2)
            {
                evBusy.WaitOne();
                if (writersFinished2)
                {
                    evBusy.Set();
                    break;
                }
                string text = buffer;
                messages.Add(text);
                //Console.WriteLine(Thread.CurrentThread.Name + " << " + text);
                evEmpty.Set();
            }
            messages.ForEach(it => rdMessages2.Add(it));
        }

        static void WriteInBuffer_Sem(object obj)
        {
            Semaphore semEmpty = ((object[])obj)[0] as Semaphore;
            Semaphore semBusy = ((object[])obj)[1] as Semaphore;
            int threadNumber = (int)(((object[])obj)[2]);

            string letter = ((char)(threadNumber + (int)'A')).ToString();
            string baseText = letter + letter + letter + letter;

            List<string> messages = new List<string>();
            for (int i = 0; i < writersJobCount; i++)
                messages.Add(baseText + (i + 1));
            messages.ForEach(it => wrMessages3.Add(it));

            int j = 0;
            while (j < messages.Count)
            {
                semEmpty.WaitOne();
                string text = messages[j++];
                buffer = text;
                //Console.WriteLine("---------------------\n" + Thread.CurrentThread.Name + " >> " + text + "\n");
                semBusy.Release();
            }

        }
        static void ReadFromBuffer_Sem(object obj)
        {
            Semaphore semEmpty = ((object[])obj)[0] as Semaphore;
            Semaphore semBusy = ((object[])obj)[1] as Semaphore;

            List<string> messages = new List<string>();
            while (!writersFinished3)
            {
                semBusy.WaitOne();
                if (writersFinished3)
                {
                    semBusy.Release();
                    break;
                }
                string text = buffer;
                messages.Add(text);
                //Console.WriteLine(Thread.CurrentThread.Name + " << " + text);
                semEmpty.Release();
            }
            messages.ForEach(it => rdMessages3.Add(it));
        }

        static void WriteInBuffer_SemSlim(object obj)
        {
            SemaphoreSlim semSlimEmpty = ((object[])obj)[0] as SemaphoreSlim;
            SemaphoreSlim semSlimBusy = ((object[])obj)[1] as SemaphoreSlim;
            int threadNumber = (int)(((object[])obj)[2]);

            string letter = ((char)(threadNumber + (int)'A')).ToString();
            string baseText = letter + letter + letter + letter;

            List<string> messages = new List<string>();
            for (int i = 0; i < writersJobCount; i++)
                messages.Add(baseText + (i + 1));
            messages.ForEach(it => wrMessages4.Add(it));

            int j = 0;
            while (j < messages.Count)
            {
                semSlimEmpty.Wait();
                string text = messages[j++];
                buffer = text;
                //Console.WriteLine("---------------------\n" + Thread.CurrentThread.Name + " >> " + text + "\n");
                semSlimBusy.Release();
            }
        }
        static void ReadFromBuffer_SemSlim(object obj)
        {
            SemaphoreSlim semSlimEmpty = ((object[])obj)[0] as SemaphoreSlim;
            SemaphoreSlim semSlimBusy = ((object[])obj)[1] as SemaphoreSlim;

            List<string> messages = new List<string>();
            while (!writersFinished4)
            {
                semSlimBusy.Wait();
                if (writersFinished4)
                {
                    semSlimBusy.Release();
                    break;
                }
                string text = buffer;
                messages.Add(text);
                //Console.WriteLine(Thread.CurrentThread.Name + " << " + text);
                semSlimEmpty.Release();
            }
            messages.ForEach(it => rdMessages4.Add(it));
        }

        static void WriteInBuffer_Interlocked(object obj)
        {
            int threadNumber = (int)obj;
            string letter = ((char)(threadNumber + (int)'A')).ToString();
            string baseText = letter + letter + letter + letter;

            List<string> messages = new List<string>();
            for (int i = 0; i < writersJobCount; i++)
                messages.Add(baseText + (i + 1));
            messages.ForEach(it => wrMessages5.Add(it));

            int j = 0;
            while (j < messages.Count)
            {
                if (Interlocked.CompareExchange(ref writersCanWrite, 0, 1) == 1)
                {
                    string text = messages[j++];
                    buffer = text;
                    //Console.WriteLine("---------------------\n" + Thread.CurrentThread.Name + " >> " + text + "\n");
                    readersCanRead = 1;
                }
                Thread.Sleep(50);
            }
        }
        static void ReadFromBuffer_Interlocked()
        {
            List<string> messages = new List<string>();
            while (!writersFinished5)
            {
                if (Interlocked.CompareExchange(ref readersCanRead, 0, 1) == 1)
                {
                    string text = buffer;
                    messages.Add(text);
                    //Console.WriteLine(Thread.CurrentThread.Name + " << " + text);
                    writersCanWrite = 1;
                }
                Thread.Sleep(50);
            }
            messages.ForEach(it => rdMessages5.Add(it));
        }


    }
}
