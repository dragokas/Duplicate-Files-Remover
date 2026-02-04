namespace DuplicatesFinder
{
    partial class FormResults
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormResults));
            listViewResult = new ListView();
            columnHeaderPath = new ColumnHeader();
            columnHeaderDuplicateCount = new ColumnHeader();
            columnHeaderDuplicatesSize = new ColumnHeader();
            buttonRemoveFiles = new Button();
            label1 = new Label();
            label2 = new Label();
            labelFileSizeSelected = new Label();
            buttonCancelSearch = new Button();
            checkBoxRecycleBin = new CheckBox();
            groupBox1 = new GroupBox();
            labelFileCountSelected = new Label();
            label3 = new Label();
            label4 = new Label();
            labelSearchTime = new Label();
            labelProgressBar = new Label();
            timerUpdate = new System.Windows.Forms.Timer(components);
            contextMenuStrip1 = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            groupBox1.SuspendLayout();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // listViewResult
            // 
            listViewResult.CheckBoxes = true;
            listViewResult.Columns.AddRange(new ColumnHeader[] { columnHeaderPath, columnHeaderDuplicateCount, columnHeaderDuplicatesSize });
            listViewResult.FullRowSelect = true;
            listViewResult.GridLines = true;
            listViewResult.Location = new Point(12, 12);
            listViewResult.Name = "listViewResult";
            listViewResult.Size = new Size(1071, 570);
            listViewResult.TabIndex = 4;
            listViewResult.UseCompatibleStateImageBehavior = false;
            listViewResult.View = View.Details;
            listViewResult.ItemChecked += listViewResult_ItemChecked;
            listViewResult.MouseUp += listViewResult_MouseUp;
            // 
            // columnHeaderPath
            // 
            columnHeaderPath.Text = "File location";
            columnHeaderPath.Width = 800;
            // 
            // columnHeaderDuplicateCount
            // 
            columnHeaderDuplicateCount.Text = "Count";
            columnHeaderDuplicateCount.TextAlign = HorizontalAlignment.Center;
            columnHeaderDuplicateCount.Width = 80;
            // 
            // columnHeaderDuplicatesSize
            // 
            columnHeaderDuplicatesSize.Text = "Size";
            columnHeaderDuplicatesSize.TextAlign = HorizontalAlignment.Center;
            columnHeaderDuplicatesSize.Width = 140;
            // 
            // buttonRemoveFiles
            // 
            buttonRemoveFiles.Location = new Point(371, 60);
            buttonRemoveFiles.Name = "buttonRemoveFiles";
            buttonRemoveFiles.Size = new Size(241, 54);
            buttonRemoveFiles.TabIndex = 5;
            buttonRemoveFiles.Text = "Remove selected files";
            buttonRemoveFiles.UseVisualStyleBackColor = true;
            buttonRemoveFiles.Click += buttonRemoveFiles_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 31);
            label1.Name = "label1";
            label1.Size = new Size(198, 30);
            label1.TabIndex = 6;
            label1.Text = "Selected files count:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(149, 72);
            label2.Name = "label2";
            label2.Size = new Size(55, 30);
            label2.TabIndex = 7;
            label2.Text = "Size:";
            // 
            // labelFileSizeSelected
            // 
            labelFileSizeSelected.AutoSize = true;
            labelFileSizeSelected.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelFileSizeSelected.Location = new Point(210, 72);
            labelFileSizeSelected.Name = "labelFileSizeSelected";
            labelFileSizeSelected.Size = new Size(25, 30);
            labelFileSizeSelected.TabIndex = 9;
            labelFileSizeSelected.Text = "0";
            // 
            // buttonCancelSearch
            // 
            buttonCancelSearch.Location = new Point(12, 601);
            buttonCancelSearch.Name = "buttonCancelSearch";
            buttonCancelSearch.Size = new Size(152, 54);
            buttonCancelSearch.TabIndex = 10;
            buttonCancelSearch.Text = "Cancel search";
            buttonCancelSearch.UseVisualStyleBackColor = true;
            buttonCancelSearch.Click += buttonCancelSearch_Click;
            // 
            // checkBoxRecycleBin
            // 
            checkBoxRecycleBin.AutoSize = true;
            checkBoxRecycleBin.Location = new Point(371, 20);
            checkBoxRecycleBin.Name = "checkBoxRecycleBin";
            checkBoxRecycleBin.Size = new Size(168, 34);
            checkBoxRecycleBin.TabIndex = 11;
            checkBoxRecycleBin.Text = "to Recycle Bin";
            checkBoxRecycleBin.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(labelFileCountSelected);
            groupBox1.Controls.Add(buttonRemoveFiles);
            groupBox1.Controls.Add(checkBoxRecycleBin);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(labelFileSizeSelected);
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new Point(456, 582);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(627, 131);
            groupBox1.TabIndex = 12;
            groupBox1.TabStop = false;
            // 
            // labelFileCountSelected
            // 
            labelFileCountSelected.AutoSize = true;
            labelFileCountSelected.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            labelFileCountSelected.Location = new Point(210, 31);
            labelFileCountSelected.Name = "labelFileCountSelected";
            labelFileCountSelected.Size = new Size(25, 30);
            labelFileCountSelected.TabIndex = 12;
            labelFileCountSelected.Text = "0";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(170, 606);
            label3.Name = "label3";
            label3.Size = new Size(127, 30);
            label3.TabIndex = 13;
            label3.Text = "Progressbar:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(170, 654);
            label4.Name = "label4";
            label4.Size = new Size(127, 30);
            label4.TabIndex = 14;
            label4.Text = "Search time:";
            // 
            // labelSearchTime
            // 
            labelSearchTime.AutoSize = true;
            labelSearchTime.Location = new Point(304, 654);
            labelSearchTime.Name = "labelSearchTime";
            labelSearchTime.Size = new Size(24, 30);
            labelSearchTime.TabIndex = 15;
            labelSearchTime.Text = "0";
            // 
            // labelProgressBar
            // 
            labelProgressBar.AutoSize = true;
            labelProgressBar.Location = new Point(304, 606);
            labelProgressBar.Name = "labelProgressBar";
            labelProgressBar.Size = new Size(55, 30);
            labelProgressBar.TabIndex = 16;
            labelProgressBar.Text = "0 / 0";
            // 
            // timerUpdate
            // 
            timerUpdate.Interval = 1000;
            timerUpdate.Tick += timerUpdate_Tick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(28, 28);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(138, 40);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(137, 36);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // FormResults
            // 
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1095, 725);
            Controls.Add(labelProgressBar);
            Controls.Add(labelSearchTime);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(groupBox1);
            Controls.Add(buttonCancelSearch);
            Controls.Add(listViewResult);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormResults";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Results";
            FormClosing += FormResults_FormClosing;
            Shown += FormResults_Shown;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            contextMenuStrip1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView listViewResult;
        private ColumnHeader columnHeaderPath;
        private ColumnHeader columnHeaderDuplicateCount;
        private ColumnHeader columnHeaderDuplicatesSize;
        private Button buttonRemoveFiles;
        private Label label1;
        private Label label2;
        private Label labelFileSizeSelected;
        private Button buttonCancelSearch;
        private CheckBox checkBoxRecycleBin;
        private GroupBox groupBox1;
        private Label labelFileCountSelected;
        private Label label3;
        private Label label4;
        private Label labelSearchTime;
        private Label labelProgressBar;
        private System.Windows.Forms.Timer timerUpdate;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem openToolStripMenuItem;
    }
}