namespace DuplicatesFinder
{
    partial class FormAccept
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccept));
            buttonSearch = new Button();
            listViewSource = new ListView();
            columnHeaderPath = new ColumnHeader();
            columnHeaderType = new ColumnHeader();
            groupBox1 = new GroupBox();
            label1 = new Label();
            numericUpDownNumBytesToCompare = new NumericUpDown();
            radioButtonCompareMethodSlow = new RadioButton();
            radioButtonCompareMethodFast = new RadioButton();
            checkBoxAllowDifferentExtensions = new CheckBox();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownNumBytesToCompare).BeginInit();
            SuspendLayout();
            // 
            // buttonSearch
            // 
            buttonSearch.Location = new Point(927, 346);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(162, 62);
            buttonSearch.TabIndex = 2;
            buttonSearch.Text = "Search";
            buttonSearch.UseVisualStyleBackColor = true;
            buttonSearch.Click += buttonSearch_Click;
            // 
            // listViewSource
            // 
            listViewSource.CheckBoxes = true;
            listViewSource.Columns.AddRange(new ColumnHeader[] { columnHeaderPath, columnHeaderType });
            listViewSource.FullRowSelect = true;
            listViewSource.GridLines = true;
            listViewSource.Location = new Point(12, 12);
            listViewSource.Name = "listViewSource";
            listViewSource.Size = new Size(843, 658);
            listViewSource.TabIndex = 3;
            listViewSource.UseCompatibleStateImageBehavior = false;
            listViewSource.View = View.Details;
            // 
            // columnHeaderPath
            // 
            columnHeaderPath.Text = "Path";
            columnHeaderPath.Width = 700;
            // 
            // columnHeaderType
            // 
            columnHeaderType.Text = "Type";
            columnHeaderType.TextAlign = HorizontalAlignment.Center;
            columnHeaderType.Width = 80;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(numericUpDownNumBytesToCompare);
            groupBox1.Controls.Add(radioButtonCompareMethodSlow);
            groupBox1.Controls.Add(radioButtonCompareMethodFast);
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            groupBox1.Location = new Point(861, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(294, 205);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Compare method";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(209, 142);
            label1.Name = "label1";
            label1.Size = new Size(62, 30);
            label1.TabIndex = 3;
            label1.Text = "bytes";
            // 
            // numericUpDownNumBytesToCompare
            // 
            numericUpDownNumBytesToCompare.Location = new Point(79, 140);
            numericUpDownNumBytesToCompare.Maximum = new decimal(new int[] { 104857600, 0, 0, 0 });
            numericUpDownNumBytesToCompare.Name = "numericUpDownNumBytesToCompare";
            numericUpDownNumBytesToCompare.Size = new Size(124, 35);
            numericUpDownNumBytesToCompare.TabIndex = 2;
            numericUpDownNumBytesToCompare.Value = new decimal(new int[] { 1024, 0, 0, 0 });
            // 
            // radioButtonCompareMethodSlow
            // 
            radioButtonCompareMethodSlow.AutoSize = true;
            radioButtonCompareMethodSlow.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            radioButtonCompareMethodSlow.Location = new Point(19, 47);
            radioButtonCompareMethodSlow.Name = "radioButtonCompareMethodSlow";
            radioButtonCompareMethodSlow.Size = new Size(208, 34);
            radioButtonCompareMethodSlow.TabIndex = 1;
            radioButtonCompareMethodSlow.Text = "Slow - by full hash";
            radioButtonCompareMethodSlow.UseVisualStyleBackColor = true;
            // 
            // radioButtonCompareMethodFast
            // 
            radioButtonCompareMethodFast.AutoSize = true;
            radioButtonCompareMethodFast.Checked = true;
            radioButtonCompareMethodFast.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            radioButtonCompareMethodFast.Location = new Point(19, 100);
            radioButtonCompareMethodFast.Name = "radioButtonCompareMethodFast";
            radioButtonCompareMethodFast.Size = new Size(209, 34);
            radioButtonCompareMethodFast.TabIndex = 0;
            radioButtonCompareMethodFast.TabStop = true;
            radioButtonCompareMethodFast.Text = "Fast - by last bytes";
            radioButtonCompareMethodFast.UseVisualStyleBackColor = true;
            // 
            // checkBoxAllowDifferentExtensions
            // 
            checkBoxAllowDifferentExtensions.Location = new Point(861, 234);
            checkBoxAllowDifferentExtensions.Name = "checkBoxAllowDifferentExtensions";
            checkBoxAllowDifferentExtensions.Size = new Size(305, 77);
            checkBoxAllowDifferentExtensions.TabIndex = 5;
            checkBoxAllowDifferentExtensions.Text = "Compare even if different extensions e.g. bmp<>jpg";
            checkBoxAllowDifferentExtensions.UseVisualStyleBackColor = true;
            checkBoxAllowDifferentExtensions.Visible = false;
            // 
            // FormAccept
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1167, 682);
            Controls.Add(checkBoxAllowDifferentExtensions);
            Controls.Add(groupBox1);
            Controls.Add(listViewSource);
            Controls.Add(buttonSearch);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormAccept";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Duplicate Files Remover";
            DragDrop += Form1_DragDrop;
            DragEnter += Form1_DragEnter;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownNumBytesToCompare).EndInit();
            ResumeLayout(false);
        }

        #endregion
        private Button buttonSearch;
        private ListView listViewSource;
        private ColumnHeader columnHeaderPath;
        private ColumnHeader columnHeaderType;
        private GroupBox groupBox1;
        private RadioButton radioButtonCompareMethodSlow;
        private RadioButton radioButtonCompareMethodFast;
        private Label label1;
        private NumericUpDown numericUpDownNumBytesToCompare;
        private CheckBox checkBoxAllowDifferentExtensions;
    }
}
