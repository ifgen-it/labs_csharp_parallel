namespace Practice5Gui
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.btn_StartSorting = new System.Windows.Forms.Button();
            this.btn_StopSorting = new System.Windows.Forms.Button();
            this.lab_Time = new System.Windows.Forms.Label();
            this.lab_Process = new System.Windows.Forms.Label();
            this.lab_SortName = new System.Windows.Forms.Label();
            this.lab_StepsPassed = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.tb_NumbersCount = new System.Windows.Forms.TextBox();
            this.lab_ArraySize = new System.Windows.Forms.Label();
            this.lab_NumbersCount = new System.Windows.Forms.Label();
            this.cb_Sortings = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lab_NeedSteps = new System.Windows.Forms.Label();
            this.rb_NotSorted = new System.Windows.Forms.RadioButton();
            this.rb_AscSorted = new System.Windows.Forms.RadioButton();
            this.rb_DescSorted = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.tb_MaxRandom = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(99, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(281, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sorting sequent and parallel";
            // 
            // btn_StartSorting
            // 
            this.btn_StartSorting.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_StartSorting.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn_StartSorting.Location = new System.Drawing.Point(26, 310);
            this.btn_StartSorting.Name = "btn_StartSorting";
            this.btn_StartSorting.Size = new System.Drawing.Size(210, 34);
            this.btn_StartSorting.TabIndex = 7;
            this.btn_StartSorting.Text = "Start sorting";
            this.btn_StartSorting.UseVisualStyleBackColor = true;
            // 
            // btn_StopSorting
            // 
            this.btn_StopSorting.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.btn_StopSorting.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btn_StopSorting.Location = new System.Drawing.Point(267, 310);
            this.btn_StopSorting.Name = "btn_StopSorting";
            this.btn_StopSorting.Size = new System.Drawing.Size(210, 34);
            this.btn_StopSorting.TabIndex = 8;
            this.btn_StopSorting.Text = "Stop sorting";
            this.btn_StopSorting.UseVisualStyleBackColor = true;
            // 
            // lab_Time
            // 
            this.lab_Time.AutoSize = true;
            this.lab_Time.Location = new System.Drawing.Point(263, 454);
            this.lab_Time.Name = "lab_Time";
            this.lab_Time.Size = new System.Drawing.Size(51, 20);
            this.lab_Time.TabIndex = 2;
            this.lab_Time.Text = "Time :";
            // 
            // lab_Process
            // 
            this.lab_Process.AutoSize = true;
            this.lab_Process.ForeColor = System.Drawing.Color.Yellow;
            this.lab_Process.Location = new System.Drawing.Point(22, 269);
            this.lab_Process.Name = "lab_Process";
            this.lab_Process.Size = new System.Drawing.Size(74, 20);
            this.lab_Process.TabIndex = 2;
            this.lab_Process.Text = "Process :";
            // 
            // lab_SortName
            // 
            this.lab_SortName.AutoSize = true;
            this.lab_SortName.Location = new System.Drawing.Point(22, 367);
            this.lab_SortName.Name = "lab_SortName";
            this.lab_SortName.Size = new System.Drawing.Size(59, 20);
            this.lab_SortName.TabIndex = 2;
            this.lab_SortName.Text = "Name :";
            // 
            // lab_StepsPassed
            // 
            this.lab_StepsPassed.AutoSize = true;
            this.lab_StepsPassed.Location = new System.Drawing.Point(22, 454);
            this.lab_StepsPassed.Name = "lab_StepsPassed";
            this.lab_StepsPassed.Size = new System.Drawing.Size(115, 20);
            this.lab_StepsPassed.TabIndex = 2;
            this.lab_StepsPassed.Text = "Steps passed :";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 132);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 20);
            this.label3.TabIndex = 2;
            this.label3.Text = "Numbers count :";
            // 
            // tb_NumbersCount
            // 
            this.tb_NumbersCount.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tb_NumbersCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_NumbersCount.ForeColor = System.Drawing.Color.MidnightBlue;
            this.tb_NumbersCount.Location = new System.Drawing.Point(165, 129);
            this.tb_NumbersCount.Name = "tb_NumbersCount";
            this.tb_NumbersCount.Size = new System.Drawing.Size(215, 26);
            this.tb_NumbersCount.TabIndex = 2;
            this.tb_NumbersCount.Text = "16*1024";
            this.tb_NumbersCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lab_ArraySize
            // 
            this.lab_ArraySize.AutoSize = true;
            this.lab_ArraySize.Location = new System.Drawing.Point(263, 411);
            this.lab_ArraySize.Name = "lab_ArraySize";
            this.lab_ArraySize.Size = new System.Drawing.Size(86, 20);
            this.lab_ArraySize.TabIndex = 2;
            this.lab_ArraySize.Text = "Array size :";
            // 
            // lab_NumbersCount
            // 
            this.lab_NumbersCount.AutoSize = true;
            this.lab_NumbersCount.Location = new System.Drawing.Point(263, 367);
            this.lab_NumbersCount.Name = "lab_NumbersCount";
            this.lab_NumbersCount.Size = new System.Drawing.Size(125, 20);
            this.lab_NumbersCount.TabIndex = 2;
            this.lab_NumbersCount.Text = "Numbers count :";
            // 
            // cb_Sortings
            // 
            this.cb_Sortings.BackColor = System.Drawing.Color.LightSteelBlue;
            this.cb_Sortings.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.cb_Sortings.ForeColor = System.Drawing.Color.MidnightBlue;
            this.cb_Sortings.FormattingEnabled = true;
            this.cb_Sortings.Location = new System.Drawing.Point(104, 76);
            this.cb_Sortings.Name = "cb_Sortings";
            this.cb_Sortings.Size = new System.Drawing.Size(276, 28);
            this.cb_Sortings.Sorted = true;
            this.cb_Sortings.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Name :";
            // 
            // lab_NeedSteps
            // 
            this.lab_NeedSteps.AutoSize = true;
            this.lab_NeedSteps.Location = new System.Drawing.Point(22, 411);
            this.lab_NeedSteps.Name = "lab_NeedSteps";
            this.lab_NeedSteps.Size = new System.Drawing.Size(98, 20);
            this.lab_NeedSteps.TabIndex = 2;
            this.lab_NeedSteps.Text = "Need steps :";
            // 
            // rb_NotSorted
            // 
            this.rb_NotSorted.AutoSize = true;
            this.rb_NotSorted.Checked = true;
            this.rb_NotSorted.Location = new System.Drawing.Point(20, 222);
            this.rb_NotSorted.Name = "rb_NotSorted";
            this.rb_NotSorted.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rb_NotSorted.Size = new System.Drawing.Size(101, 24);
            this.rb_NotSorted.TabIndex = 4;
            this.rb_NotSorted.TabStop = true;
            this.rb_NotSorted.Text = "Not sorted";
            this.rb_NotSorted.UseVisualStyleBackColor = true;
            // 
            // rb_AscSorted
            // 
            this.rb_AscSorted.AutoSize = true;
            this.rb_AscSorted.Location = new System.Drawing.Point(144, 222);
            this.rb_AscSorted.Name = "rb_AscSorted";
            this.rb_AscSorted.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rb_AscSorted.Size = new System.Drawing.Size(103, 24);
            this.rb_AscSorted.TabIndex = 5;
            this.rb_AscSorted.Text = "Asc sorted";
            this.rb_AscSorted.UseVisualStyleBackColor = true;
            // 
            // rb_DescSorted
            // 
            this.rb_DescSorted.AutoSize = true;
            this.rb_DescSorted.Location = new System.Drawing.Point(267, 222);
            this.rb_DescSorted.Name = "rb_DescSorted";
            this.rb_DescSorted.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rb_DescSorted.Size = new System.Drawing.Size(113, 24);
            this.rb_DescSorted.TabIndex = 6;
            this.rb_DescSorted.Text = "Desc sorted";
            this.rb_DescSorted.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 181);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(104, 20);
            this.label4.TabIndex = 2;
            this.label4.Text = "Max random :";
            // 
            // tb_MaxRandom
            // 
            this.tb_MaxRandom.BackColor = System.Drawing.Color.LightSteelBlue;
            this.tb_MaxRandom.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tb_MaxRandom.ForeColor = System.Drawing.Color.MidnightBlue;
            this.tb_MaxRandom.Location = new System.Drawing.Point(165, 178);
            this.tb_MaxRandom.Name = "tb_MaxRandom";
            this.tb_MaxRandom.Size = new System.Drawing.Size(215, 26);
            this.tb_MaxRandom.TabIndex = 3;
            this.tb_MaxRandom.Text = "1000";
            this.tb_MaxRandom.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // Form1
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.MidnightBlue;
            this.ClientSize = new System.Drawing.Size(504, 502);
            this.Controls.Add(this.rb_DescSorted);
            this.Controls.Add(this.rb_AscSorted);
            this.Controls.Add(this.rb_NotSorted);
            this.Controls.Add(this.cb_Sortings);
            this.Controls.Add(this.tb_MaxRandom);
            this.Controls.Add(this.tb_NumbersCount);
            this.Controls.Add(this.lab_StepsPassed);
            this.Controls.Add(this.lab_NeedSteps);
            this.Controls.Add(this.lab_SortName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lab_Process);
            this.Controls.Add(this.lab_NumbersCount);
            this.Controls.Add(this.lab_ArraySize);
            this.Controls.Add(this.lab_Time);
            this.Controls.Add(this.btn_StopSorting);
            this.Controls.Add(this.btn_StartSorting);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.Color.Yellow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaximumSize = new System.Drawing.Size(520, 540);
            this.MinimumSize = new System.Drawing.Size(520, 520);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sorting";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_StartSorting;
        private System.Windows.Forms.Button btn_StopSorting;
        private System.Windows.Forms.Label lab_Time;
        private System.Windows.Forms.Label lab_Process;
        private System.Windows.Forms.Label lab_SortName;
        private System.Windows.Forms.Label lab_StepsPassed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tb_NumbersCount;
        private System.Windows.Forms.Label lab_ArraySize;
        private System.Windows.Forms.Label lab_NumbersCount;
        private System.Windows.Forms.ComboBox cb_Sortings;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lab_NeedSteps;
        private System.Windows.Forms.RadioButton rb_NotSorted;
        private System.Windows.Forms.RadioButton rb_AscSorted;
        private System.Windows.Forms.RadioButton rb_DescSorted;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tb_MaxRandom;
    }
}

