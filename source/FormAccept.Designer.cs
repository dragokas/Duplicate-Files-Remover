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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAccept));
            buttonSearch = new Button();
            listViewSource = new ListView();
            columnHeaderPath = new ColumnHeader();
            columnHeaderType = new ColumnHeader();
            groupBox1 = new GroupBox();
            checkBoxMatchExtensions = new CheckBox();
            label1 = new Label();
            numericUpDownNumBytesToCompare = new NumericUpDown();
            radioButtonCompareMethodSlow = new RadioButton();
            radioButtonCompareMethodFast = new RadioButton();
            panel1 = new Panel();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            pictureBoxDropFiles = new PictureBox();
            buttonAddFolder = new Button();
            buttonClearList = new Button();
            contextMenuStrip1 = new ContextMenuStrip(components);
            openToolStripMenuItem = new ToolStripMenuItem();
            deleteFromListToolStripMenuItem = new ToolStripMenuItem();
            toolTip1 = new ToolTip(components);
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownNumBytesToCompare).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBoxDropFiles).BeginInit();
            contextMenuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // buttonSearch
            // 
            buttonSearch.Location = new Point(73, 272);
            buttonSearch.Name = "buttonSearch";
            buttonSearch.Size = new Size(162, 70);
            buttonSearch.TabIndex = 2;
            buttonSearch.Text = "Search";
            buttonSearch.UseVisualStyleBackColor = true;
            buttonSearch.Click += buttonSearch_Click;
            // 
            // listViewSource
            // 
            listViewSource.CheckBoxes = true;
            listViewSource.Columns.AddRange(new ColumnHeader[] { columnHeaderPath, columnHeaderType });
            listViewSource.Dock = DockStyle.Fill;
            listViewSource.FullRowSelect = true;
            listViewSource.GridLines = true;
            listViewSource.Location = new Point(0, 0);
            listViewSource.Name = "listViewSource";
            listViewSource.Size = new Size(824, 371);
            listViewSource.TabIndex = 3;
            listViewSource.UseCompatibleStateImageBehavior = false;
            listViewSource.View = View.Details;
            listViewSource.MouseClick += listViewSource_MouseClick;
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
            groupBox1.Controls.Add(checkBoxMatchExtensions);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(numericUpDownNumBytesToCompare);
            groupBox1.Controls.Add(radioButtonCompareMethodSlow);
            groupBox1.Controls.Add(radioButtonCompareMethodFast);
            groupBox1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 204);
            groupBox1.Location = new Point(7, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(303, 232);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Compare method";
            // 
            // checkBoxMatchExtensions
            // 
            checkBoxMatchExtensions.AutoSize = true;
            checkBoxMatchExtensions.Checked = true;
            checkBoxMatchExtensions.CheckState = CheckState.Checked;
            checkBoxMatchExtensions.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            checkBoxMatchExtensions.Location = new Point(19, 183);
            checkBoxMatchExtensions.Name = "checkBoxMatchExtensions";
            checkBoxMatchExtensions.Size = new Size(269, 34);
            checkBoxMatchExtensions.TabIndex = 4;
            checkBoxMatchExtensions.Text = "Extensions should match";
            toolTip1.SetToolTip(checkBoxMatchExtensions, "If enabled, the speed is a little bit faster");
            checkBoxMatchExtensions.UseVisualStyleBackColor = true;
            checkBoxMatchExtensions.Visible = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(209, 129);
            label1.Name = "label1";
            label1.Size = new Size(62, 30);
            label1.TabIndex = 3;
            label1.Text = "bytes";
            // 
            // numericUpDownNumBytesToCompare
            // 
            numericUpDownNumBytesToCompare.Location = new Point(79, 127);
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
            radioButtonCompareMethodFast.Location = new Point(19, 87);
            radioButtonCompareMethodFast.Name = "radioButtonCompareMethodFast";
            radioButtonCompareMethodFast.Size = new Size(214, 34);
            radioButtonCompareMethodFast.TabIndex = 0;
            radioButtonCompareMethodFast.TabStop = true;
            radioButtonCompareMethodFast.Text = "Fast - by last bytes:";
            radioButtonCompareMethodFast.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.Controls.Add(groupBox1);
            panel1.Controls.Add(buttonSearch);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(322, 450);
            panel1.TabIndex = 6;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            splitContainer1.Panel1MinSize = 820;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panel1);
            splitContainer1.Panel2MinSize = 320;
            splitContainer1.Size = new Size(1150, 450);
            splitContainer1.SplitterDistance = 824;
            splitContainer1.TabIndex = 7;
            splitContainer1.SplitterMoved += splitContainer1_SplitterMoved;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(listViewSource);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(pictureBoxDropFiles);
            splitContainer2.Panel2.Controls.Add(buttonAddFolder);
            splitContainer2.Panel2.Controls.Add(buttonClearList);
            splitContainer2.Panel2MinSize = 75;
            splitContainer2.Size = new Size(824, 450);
            splitContainer2.SplitterDistance = 371;
            splitContainer2.TabIndex = 8;
            splitContainer2.SplitterMoved += splitContainer2_SplitterMoved;
            // 
            // pictureBoxDropFiles
            // 
            pictureBoxDropFiles.Location = new Point(186, 0);
            pictureBoxDropFiles.Name = "pictureBoxDropFiles";
            pictureBoxDropFiles.Size = new Size(461, 72);
            pictureBoxDropFiles.TabIndex = 8;
            pictureBoxDropFiles.TabStop = false;
            // 
            // buttonAddFolder
            // 
            buttonAddFolder.Location = new Point(12, 4);
            buttonAddFolder.Name = "buttonAddFolder";
            buttonAddFolder.Size = new Size(168, 51);
            buttonAddFolder.TabIndex = 6;
            buttonAddFolder.Text = "Add Folder ...";
            buttonAddFolder.UseVisualStyleBackColor = true;
            buttonAddFolder.Click += buttonAddFolder_Click;
            // 
            // buttonClearList
            // 
            buttonClearList.Location = new Point(653, 6);
            buttonClearList.Name = "buttonClearList";
            buttonClearList.Size = new Size(168, 51);
            buttonClearList.TabIndex = 7;
            buttonClearList.Text = "Clear list";
            buttonClearList.UseVisualStyleBackColor = true;
            buttonClearList.Click += buttonClearList_Click;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(28, 28);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { openToolStripMenuItem, deleteFromListToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(229, 76);
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(228, 36);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // deleteFromListToolStripMenuItem
            // 
            deleteFromListToolStripMenuItem.Name = "deleteFromListToolStripMenuItem";
            deleteFromListToolStripMenuItem.Size = new Size(228, 36);
            deleteFromListToolStripMenuItem.Text = "Delete from list";
            deleteFromListToolStripMenuItem.Click += deleteFromListToolStripMenuItem_Click;
            // 
            // FormAccept
            // 
            AllowDrop = true;
            AutoScaleDimensions = new SizeF(12F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1150, 450);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "FormAccept";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Find My Duplicates";
            Load += FormAccept_Load;
            LocationChanged += FormAccept_LocationChanged;
            DragDrop += Form1_DragDrop;
            DragEnter += Form1_DragEnter;
            Resize += FormAccept_Resize;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownNumBytesToCompare).EndInit();
            panel1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBoxDropFiles).EndInit();
            contextMenuStrip1.ResumeLayout(false);
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
        private Panel panel1;
        private SplitContainer splitContainer1;
        private Button buttonAddFolder;
        private SplitContainer splitContainer2;
        private Button buttonClearList;
        private ContextMenuStrip contextMenuStrip1;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem deleteFromListToolStripMenuItem;
        private CheckBox checkBoxMatchExtensions;
        private ToolTip toolTip1;
        private PictureBox pictureBoxDropFiles;
    }
}
