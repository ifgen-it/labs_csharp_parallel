using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Practice5Gui
{
    public partial class Form1 : Form
    {

        delegate void Updater(object obj);
        Updater updater => UpdateControls;
        int timerInterval = 1000;
        System.Timers.Timer timer;
        long timeWasTicked;
        CancellationTokenSource cts;
        bool sorting;
        string sortName = "";
        bool failed;
        SortType sortType;
        bool cannotCancel;

        public Form1()
        {
            InitializeComponent();
            SetTimer();

            FormClosing += Form1_FormClosing;
            btn_StartSorting.Click += BtnStartSorting_Click;
            btn_StopSorting.Click += Btn_StopSorting_Click;
            btn_StartSorting.MouseEnter += (o, e) => { btn_StartSorting.ForeColor = Color.Green; };
            btn_StartSorting.MouseLeave += (o, e) => { btn_StartSorting.ForeColor = Color.MidnightBlue; };
            btn_StopSorting.MouseEnter += (o, e) => { btn_StopSorting.ForeColor = Color.Red; };
            btn_StopSorting.MouseLeave += (o, e) => { btn_StopSorting.ForeColor = Color.MidnightBlue; };

            cb_Sortings.Items.Add("Bubble sort");
            cb_Sortings.Items.Add("Odd-Even sort");
            cb_Sortings.Items.Add("Enumeration sort");
            cb_Sortings.Items.Add("Parallel Odd-Even sort");
            cb_Sortings.Items.Add("Parallel Enumeration sort");
            cb_Sortings.Items.Add("Bucket sort");
            cb_Sortings.Items.Add("Parallel Bucket sort");
            cb_Sortings.Items.Add("Parallel Shell sort");
            cb_Sortings.Items.Add("Merge sort");
            cb_Sortings.Items.Add("Quick sort");
            cb_Sortings.Items.Add("Parallel Quick sort");
            cb_Sortings.Items.Add("Bitonic sort");
            cb_Sortings.Items.Add("Parallel Bitonic sort");
            cb_Sortings.DropDownStyle = ComboBoxStyle.DropDownList;


        }

        void ShowMessageBox(string text)
        {
            MessageBox.Show(text, "Information",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (sorting)
            {
                ShowMessageBox("Press Stop sorting before closing window\n" +
                    "or wait for sorting finish!");
                e.Cancel = true;
                return;
            }
            timer.Dispose();
            if (cts != null)
                cts.Dispose();
        }
        void SetTimer()
        {
            timer = new System.Timers.Timer(timerInterval);
            timer.Elapsed += Timer_Elapsed;
            timer.AutoReset = true;
        }
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timeWasTicked += timerInterval;
            Invoke(updater, new object[] { "time", timeWasTicked } as object);
            int dotNumbers = (int)timeWasTicked / timerInterval % 4;
            string strSym = "";
            switch (dotNumbers)
            {
                case 1:
                    strSym = "-";
                    break;
                case 2:
                    strSym = "\\";
                    break;
                case 3:
                    strSym = "|";
                    break;
                case 0:
                    strSym = "/";
                    break;
                default:
                    break;
            }
            Invoke(updater, new object[] { "process", $"Sorting  {strSym}" } as object);
        }
        public void UpdateControls(object obj)
        {
            object[] input = obj as object[];
            var whatUpdate = input[0] as string;
            switch (whatUpdate)
            {
                case "steps_passed":
                    lab_StepsPassed.Text = $"Steps passed : {input[1]}";
                    break;
                case "time":
                    lab_Time.Text = $"Time : {(long)input[1]} ms";
                    break;
                case "process":
                    lab_Process.Text = input[1] as string;
                    string res = input[1] as string;
                    if (res.Equals("Finished"))
                        lab_Process.ForeColor = Color.LawnGreen;
                    else if (res.Equals("Cancelled") || res.Equals("Failed"))
                        lab_Process.ForeColor = Color.OrangeRed;
                    else
                        lab_Process.ForeColor = Color.Yellow;
                    lab_Process.Text = $"Process : {res}";
                    break;
                case "need_steps":
                    lab_NeedSteps.Text = $"Need steps : {input[1]}";
                    break;
                case "numbers_count":
                    lab_NumbersCount.Text = input[1] as string;
                    break;
                case "array_size":
                    lab_ArraySize.Text = input[1] as string;
                    break;
                case "sort_name":
                    lab_SortName.Text = input[1] as string;
                    break;
                case "btn_stop_enable":
                    btn_StopSorting.Enabled = true;
                    break;
                default:
                    break;
            }

        }
        private void BtnStartSorting_Click(object sender, EventArgs e)
        {
            if (sorting)
                return;
            // SORT NAME
            sortName = cb_Sortings.SelectedItem as string;
            if (sortName == null)
            {
                ShowMessageBox("Select sorting name!");
                return;
            }

            // NUMBERS COUNT
            int numbersCount = ReadNumbersCount();
            if (numbersCount == -1)
                return;

            // MAX RANDOM
            int maxRandom = ReadMaxRandom();
            if (maxRandom == -1)
                return;
            Sorter.SetMaxRandom(maxRandom);

            // SORT TYPE
            sortType = ReadSortType();

            // SORTING
            sorting = true;
            cts = new CancellationTokenSource();
            Task.Run(() => RunSort(numbersCount, cts.Token));
        }
        private void Btn_StopSorting_Click(object sender, EventArgs e)
        {
            if (cannotCancel)
            {
                ShowMessageBox("Cancellation is OFF in sortings, which have recursion under hood:\n" +
                    "- Merge sort\n" +
                    "- Quick sort\n" +
                    "- Parallel Quick sort");
                return;
            }
            if (!sorting)
                return;
            if (cts != null)
                cts.Cancel();
            btn_StopSorting.Enabled = false;
        }
        void RunSort(int numbersCount, CancellationToken token)
        {
            Sorter sorter = null;
            Stopwatch watch1 = null;
            List<int> numbers = null;
            try
            {
                Invoke(updater, new object[] { "time", 0L } as object);
                Invoke(updater, new object[] { "sort_name", $"Name : {sortName}" } as object);
                Invoke(updater, new object[] { "process", "Preparing" } as object);
                Invoke(updater, new object[] { "numbers_count", $"Numbers count : {numbersCount}" } as object);
                Invoke(updater, new object[] { "array_size", $"Array size : {Sorter.FileSize(numbersCount * sizeof(int))}" } as object);
                Invoke(updater, new object[] { "steps_passed", "" } as object);
                Invoke(updater, new object[] { "need_steps", "" } as object);

                sorter = new Sorter(this);
                numbers = sorter.GenerateList(numbersCount, sortType);
                if (token.IsCancellationRequested)
                    return;
                timer.Start();
                watch1 = new Stopwatch();
                watch1.Start();
                Invoke(updater, new object[] { "process", "Sorting" } as object);
                switch (sortName)
                {
                    case "Bubble sort":
                        sorter.BubbleSort(numbers, token);
                        break;
                    case "Odd-Even sort":
                        sorter.OddEvenSort(numbers, token);
                        break;
                    case "Enumeration sort":
                        numbers = sorter.EnumerationSort(numbers, token);
                        break;
                    case "Parallel Odd-Even sort":
                        sorter.ParallelOddEvenSort(numbers, token);
                        break;
                    case "Parallel Enumeration sort":
                        numbers = sorter.ParallelEnumerationSort(numbers, token);
                        break;
                    case "Bucket sort":
                        numbers = sorter.BucketSort(numbers, token);
                        break;
                    case "Parallel Bucket sort":
                        numbers = sorter.ParallelBucketSort(numbers, token);
                        break;
                    case "Parallel Shell sort":
                        sorter.ParallelShellSort(numbers, token);
                        break;
                    case "Merge sort":
                        cannotCancel = true;
                        numbers = sorter.MergeSort(numbers, token);
                        break;
                    case "Quick sort":
                        cannotCancel = true;
                        numbers = sorter.QuickSort(numbers, token);
                        break;
                    case "Parallel Quick sort":
                        cannotCancel = true;
                        numbers = sorter.ParallelQuickSort(numbers, token);
                        break;
                    case "Bitonic sort":
                        sorter.BitonicSort(numbers, token);
                        break;
                    case "Parallel Bitonic sort":
                        sorter.ParallelBitonicSort(numbers, token);
                        break;
                    default:
                        break;
                }

            }
            catch (BitonicSortException e)
            {
                ShowMessageBox($"Error:\n{e.Message}");
                failed = true;
            }
            catch (Exception e)
            {
                ShowMessageBox($"Error:\n{e.Message}");
                failed = true;
            }
            finally
            {
                cannotCancel = false;
                if (watch1 != null)
                {
                    watch1.Stop();
                    Invoke(updater, new object[] { "time", watch1.ElapsedMilliseconds } as object);
                }
                timer.Stop();
                timeWasTicked = 0;
                //sorter.DisposeList(ref numbers);
                if (!token.IsCancellationRequested)
                {
                    if (!failed)
                        Invoke(updater, new object[] { "process", "Finished" } as object);
                    else
                    {
                        Invoke(updater, new object[] { "process", "Failed" } as object);
                        failed = false;
                    }
                }
                else
                {
                    Invoke(updater, new object[] { "process", "Cancelled" } as object);
                    Invoke(updater, new object[] { "btn_stop_enable" } as object);
                }
                sorting = false;
            }
        }
        int ReadNumbersCount()
        {
            string strNumbersCount = tb_NumbersCount.Text.Trim();
            string errorText = "Absolute numbers count value must be in range 1..2147483647.\n" +
                        "Input single value or few values with multiply operator, like 1024*256";

            if (strNumbersCount.Contains("*"))
            {
                var values = strNumbersCount.Split(new char[] { '*' }, StringSplitOptions.RemoveEmptyEntries).ToList();
                int result = 1;
                try
                {
                    foreach (var value in values)
                    {
                        int intValue = int.Parse(value);
                        result *= intValue;
                    }
                }
                catch (Exception)
                {
                    ShowMessageBox(errorText);
                    return -1;
                }
                return result;
            }
            else
            {
                int numbersCount = 0;
                bool res = int.TryParse(strNumbersCount, out numbersCount);
                if (!res || numbersCount <= 0)
                {
                    ShowMessageBox(errorText);
                    return -1;
                }
                else
                    return numbersCount;
            }

        }
        int ReadMaxRandom()
        {
            int highLimit = 10000000;
            string strMaxRandom = tb_MaxRandom.Text.Trim();
            string errorText = $"Max random value must be in range 1..{highLimit}";

            int maxRandom = 0;
            bool res = int.TryParse(strMaxRandom, out maxRandom);
            if (!res || maxRandom <= 0 || maxRandom > highLimit)
            {
                ShowMessageBox(errorText);
                return -1;
            }
            else
                return maxRandom;
        }
        SortType ReadSortType()
        {
            if (rb_NotSorted.Checked)
                return SortType.NOT_SORTED;
            else if (rb_AscSorted.Checked)
                return SortType.ASC_SORTED;
            else
                return SortType.DESC_SORTED;
        }


    }

    enum SortType
    {
        NOT_SORTED, ASC_SORTED, DESC_SORTED
    }
}
