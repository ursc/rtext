namespace rtext
{
    partial class Form1
    {
        /// <summary>
        /// Designer variable used to keep track of non-visual components.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Disposes resources used by the form.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing) {
                if (components != null) {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// This method is required for Windows Forms designer support.
        /// Do not change the method contents inside the source code editor. The Forms designer might
        /// not be able to load this method if it was changed manually.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.filesDataGridView = new System.Windows.Forms.DataGridView();
            this.ColFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColExt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLayouts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColResult = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.filesAddButton = new System.Windows.Forms.Button();
            this.folderAddButton = new System.Windows.Forms.Button();
            this.filesClearButton = new System.Windows.Forms.Button();
            this.findSeparator = new System.Windows.Forms.Panel();
            this.caseCheckBox = new System.Windows.Forms.CheckBox();
            this.rxCheckBox = new System.Windows.Forms.CheckBox();
            this.findTextBox = new System.Windows.Forms.WatermarkTextBox();
            this.findButton = new System.Windows.Forms.Button();
            this.replaceSeparator = new System.Windows.Forms.Panel();
            this.replaceTextBox = new System.Windows.Forms.WatermarkTextBox();
            this.replaceButton = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            ((System.ComponentModel.ISupportInitialize)(this.filesDataGridView)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // filesDataGridView
            // 
            this.filesDataGridView.AllowUserToAddRows = false;
            this.filesDataGridView.AllowUserToResizeRows = false;
            this.filesDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.filesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.filesDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColFile,
            this.ColExt,
            this.ColSize,
            this.ColLayouts,
            this.ColResult});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Gray;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.filesDataGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.filesDataGridView.Location = new System.Drawing.Point(4, 4);
            this.filesDataGridView.Name = "filesDataGridView";
            this.filesDataGridView.ReadOnly = true;
            this.filesDataGridView.RowHeadersVisible = false;
            this.filesDataGridView.RowHeadersWidth = 40;
            this.filesDataGridView.RowTemplate.Height = 18;
            this.filesDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.filesDataGridView.Size = new System.Drawing.Size(912, 425);
            this.filesDataGridView.TabIndex = 0;
            this.filesDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.FilesCellDoubleClick);
            this.filesDataGridView.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.FilesUserDeletedRow);
            this.filesDataGridView.MouseClick += new System.Windows.Forms.MouseEventHandler(this.FilesMouseClick);
            // 
            // ColFile
            // 
            this.ColFile.FillWeight = 500F;
            this.ColFile.HeaderText = "Filename";
            this.ColFile.Name = "ColFile";
            this.ColFile.ReadOnly = true;
            this.ColFile.ToolTipText = "Filename";
            this.ColFile.Width = 500;
            // 
            // ColExt
            // 
            this.ColExt.FillWeight = 40F;
            this.ColExt.HeaderText = "Ext";
            this.ColExt.Name = "ColExt";
            this.ColExt.ReadOnly = true;
            this.ColExt.ToolTipText = "Ext";
            this.ColExt.Width = 40;
            // 
            // ColSize
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ColSize.DefaultCellStyle = dataGridViewCellStyle1;
            this.ColSize.FillWeight = 80F;
            this.ColSize.HeaderText = "Size";
            this.ColSize.Name = "ColSize";
            this.ColSize.ReadOnly = true;
            this.ColSize.ToolTipText = "Size";
            this.ColSize.Width = 80;
            // 
            // ColLayouts
            // 
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ColLayouts.DefaultCellStyle = dataGridViewCellStyle2;
            this.ColLayouts.FillWeight = 60F;
            this.ColLayouts.HeaderText = "Layouts";
            this.ColLayouts.Name = "ColLayouts";
            this.ColLayouts.ReadOnly = true;
            this.ColLayouts.ToolTipText = "Layouts";
            this.ColLayouts.Width = 60;
            // 
            // ColResult
            // 
            this.ColResult.FillWeight = 200F;
            this.ColResult.HeaderText = "Result";
            this.ColResult.Name = "ColResult";
            this.ColResult.ReadOnly = true;
            this.ColResult.ToolTipText = "Result";
            this.ColResult.Width = 200;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "AutoCAD files|*.dwg;*.dxf;*.dwt";
            this.openFileDialog1.Multiselect = true;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.AutoSize = true;
            this.flowLayoutPanel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanel1.Controls.Add(this.filesAddButton);
            this.flowLayoutPanel1.Controls.Add(this.folderAddButton);
            this.flowLayoutPanel1.Controls.Add(this.filesClearButton);
            this.flowLayoutPanel1.Controls.Add(this.findSeparator);
            this.flowLayoutPanel1.Controls.Add(this.caseCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.rxCheckBox);
            this.flowLayoutPanel1.Controls.Add(this.findTextBox);
            this.flowLayoutPanel1.Controls.Add(this.findButton);
            this.flowLayoutPanel1.Controls.Add(this.replaceSeparator);
            this.flowLayoutPanel1.Controls.Add(this.replaceTextBox);
            this.flowLayoutPanel1.Controls.Add(this.replaceButton);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.flowLayoutPanel1.Location = new System.Drawing.Point(920, 4);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(186, 253);
            this.flowLayoutPanel1.TabIndex = 11;
            this.flowLayoutPanel1.WrapContents = false;
            // 
            // filesAddButton
            // 
            this.filesAddButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.filesAddButton.Location = new System.Drawing.Point(3, 1);
            this.filesAddButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.filesAddButton.Name = "filesAddButton";
            this.filesAddButton.Size = new System.Drawing.Size(180, 23);
            this.filesAddButton.TabIndex = 0;
            this.filesAddButton.Text = "Add files";
            this.filesAddButton.UseVisualStyleBackColor = true;
            this.filesAddButton.Click += new System.EventHandler(this.FilesAddClick);
            // 
            // folderAddButton
            // 
            this.folderAddButton.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.folderAddButton.Location = new System.Drawing.Point(3, 26);
            this.folderAddButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.folderAddButton.Name = "folderAddButton";
            this.folderAddButton.Size = new System.Drawing.Size(180, 23);
            this.folderAddButton.TabIndex = 1;
            this.folderAddButton.Text = "Add folder";
            this.folderAddButton.UseVisualStyleBackColor = true;
            this.folderAddButton.Click += new System.EventHandler(this.FolderClick);
            // 
            // filesClearButton
            // 
            this.filesClearButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.filesClearButton.Location = new System.Drawing.Point(3, 51);
            this.filesClearButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.filesClearButton.Name = "filesClearButton";
            this.filesClearButton.Size = new System.Drawing.Size(180, 23);
            this.filesClearButton.TabIndex = 2;
            this.filesClearButton.Text = "Clear";
            this.filesClearButton.UseVisualStyleBackColor = true;
            this.filesClearButton.Click += new System.EventHandler(this.FilesClearClick);
            // 
            // findSeparator
            // 
            this.findSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.findSeparator.Location = new System.Drawing.Point(3, 78);
            this.findSeparator.Name = "findSeparator";
            this.findSeparator.Size = new System.Drawing.Size(180, 4);
            this.findSeparator.TabIndex = 3;
            // 
            // caseCheckBox
            // 
            this.caseCheckBox.Checked = true;
            this.caseCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.caseCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.caseCheckBox.Location = new System.Drawing.Point(3, 88);
            this.caseCheckBox.Name = "caseCheckBox";
            this.caseCheckBox.Size = new System.Drawing.Size(180, 19);
            this.caseCheckBox.TabIndex = 4;
            this.caseCheckBox.Text = "Case sensitive";
            this.caseCheckBox.UseVisualStyleBackColor = true;
            // 
            // rxCheckBox
            // 
            this.rxCheckBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.rxCheckBox.Location = new System.Drawing.Point(3, 113);
            this.rxCheckBox.Name = "rxCheckBox";
            this.rxCheckBox.Size = new System.Drawing.Size(180, 19);
            this.rxCheckBox.TabIndex = 5;
            this.rxCheckBox.Text = "Regular expression";
            this.rxCheckBox.UseVisualStyleBackColor = true;
            // 
            // findTextBox
            // 
            this.findTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.findTextBox.ForeColor = System.Drawing.Color.Gray;
            this.findTextBox.Location = new System.Drawing.Point(3, 138);
            this.findTextBox.Name = "findTextBox";
            this.findTextBox.Size = new System.Drawing.Size(180, 23);
            this.findTextBox.TabIndex = 6;
            this.findTextBox.Text = "Find what ...";
            this.findTextBox.WatermarkActive = true;
            this.findTextBox.WatermarkText = "Find what ...";
            this.findTextBox.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // findButton
            // 
            this.findButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.findButton.Location = new System.Drawing.Point(3, 165);
            this.findButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.findButton.Name = "findButton";
            this.findButton.Size = new System.Drawing.Size(180, 23);
            this.findButton.TabIndex = 7;
            this.findButton.Text = "Find";
            this.findButton.UseVisualStyleBackColor = true;
            this.findButton.Click += new System.EventHandler(this.FindClick);
            // 
            // replaceSeparator
            // 
            this.replaceSeparator.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.replaceSeparator.Location = new System.Drawing.Point(3, 192);
            this.replaceSeparator.Name = "replaceSeparator";
            this.replaceSeparator.Size = new System.Drawing.Size(180, 4);
            this.replaceSeparator.TabIndex = 8;
            // 
            // replaceTextBox
            // 
            this.replaceTextBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.replaceTextBox.ForeColor = System.Drawing.Color.Gray;
            this.replaceTextBox.Location = new System.Drawing.Point(3, 202);
            this.replaceTextBox.Name = "replaceTextBox";
            this.replaceTextBox.Size = new System.Drawing.Size(180, 23);
            this.replaceTextBox.TabIndex = 9;
            this.replaceTextBox.Text = "Replace with ...";
            this.replaceTextBox.WatermarkActive = true;
            this.replaceTextBox.WatermarkText = "Replace with ...";
            this.replaceTextBox.TextChanged += new System.EventHandler(this.TextBoxTextChanged);
            // 
            // replaceButton
            // 
            this.replaceButton.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.replaceButton.Location = new System.Drawing.Point(3, 229);
            this.replaceButton.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.replaceButton.Name = "replaceButton";
            this.replaceButton.Size = new System.Drawing.Size(180, 23);
            this.replaceButton.TabIndex = 10;
            this.replaceButton.Text = "Replace";
            this.replaceButton.UseVisualStyleBackColor = true;
            this.replaceButton.Click += new System.EventHandler(this.FindClick);
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.folderBrowserDialog1.ShowNewFolderButton = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1108, 433);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.filesDataGridView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 350);
            this.Name = "Form1";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Text = "Search and replace text";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1FormClosing);
            this.Shown += new System.EventHandler(this.Form1Shown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1KeyPress);
            ((System.ComponentModel.ISupportInitialize)(this.filesDataGridView)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.DataGridView filesDataGridView;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button filesAddButton;
        private System.Windows.Forms.Button folderAddButton;
        private System.Windows.Forms.Button filesClearButton;
        private System.Windows.Forms.Panel findSeparator;
        private System.Windows.Forms.CheckBox caseCheckBox;
        private System.Windows.Forms.CheckBox rxCheckBox;
        private System.Windows.Forms.WatermarkTextBox findTextBox;
        private System.Windows.Forms.Button findButton;
        private System.Windows.Forms.Panel replaceSeparator;
        private System.Windows.Forms.Button replaceButton;
        private System.Windows.Forms.WatermarkTextBox replaceTextBox;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColExt;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLayouts;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColResult;

    }
}
