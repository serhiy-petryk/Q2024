﻿
namespace Quote2024
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpLoader = new System.Windows.Forms.TabPage();
            this.btnRunMultiItemsLoader = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Image = new System.Windows.Forms.DataGridViewImageColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Started = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Duration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.loaderItemBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnTest = new System.Windows.Forms.Button();
            this.lblEoddataLogged = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnMinuteYahooLog = new System.Windows.Forms.Button();
            this.statusStrip1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpLoader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loaderItemBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 388);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(976, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(67, 17);
            this.StatusLabel.Text = "StatusLabel";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpLoader);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(976, 388);
            this.tabControl1.TabIndex = 1;
            // 
            // tpLoader
            // 
            this.tpLoader.AutoScroll = true;
            this.tpLoader.Controls.Add(this.btnMinuteYahooLog);
            this.tpLoader.Controls.Add(this.btnRunMultiItemsLoader);
            this.tpLoader.Controls.Add(this.dataGridView1);
            this.tpLoader.Controls.Add(this.btnTest);
            this.tpLoader.Controls.Add(this.lblEoddataLogged);
            this.tpLoader.Location = new System.Drawing.Point(4, 24);
            this.tpLoader.Name = "tpLoader";
            this.tpLoader.Padding = new System.Windows.Forms.Padding(3);
            this.tpLoader.Size = new System.Drawing.Size(968, 360);
            this.tpLoader.TabIndex = 0;
            this.tpLoader.Text = "Loader";
            this.tpLoader.UseVisualStyleBackColor = true;
            // 
            // btnRunMultiItemsLoader
            // 
            this.btnRunMultiItemsLoader.Location = new System.Drawing.Point(397, 21);
            this.btnRunMultiItemsLoader.Name = "btnRunMultiItemsLoader";
            this.btnRunMultiItemsLoader.Size = new System.Drawing.Size(144, 23);
            this.btnRunMultiItemsLoader.TabIndex = 68;
            this.btnRunMultiItemsLoader.Text = "Run multiItems loader";
            this.btnRunMultiItemsLoader.UseVisualStyleBackColor = true;
            this.btnRunMultiItemsLoader.Click += new System.EventHandler(this.btnRunMultiItemsLoader_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ColumnHeadersVisible = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.checkedDataGridViewCheckBoxColumn,
            this.Image,
            this.nameDataGridViewTextBoxColumn,
            this.Started,
            this.Duration});
            this.dataGridView1.DataSource = this.loaderItemBindingSource;
            this.dataGridView1.Location = new System.Drawing.Point(17, 18);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 18;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(374, 319);
            this.dataGridView1.TabIndex = 67;
            this.dataGridView1.SelectionChanged += new System.EventHandler(this.dataGridView1_SelectionChanged);
            // 
            // checkedDataGridViewCheckBoxColumn
            // 
            this.checkedDataGridViewCheckBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.checkedDataGridViewCheckBoxColumn.DataPropertyName = "Checked";
            this.checkedDataGridViewCheckBoxColumn.HeaderText = "Checked";
            this.checkedDataGridViewCheckBoxColumn.Name = "checkedDataGridViewCheckBoxColumn";
            this.checkedDataGridViewCheckBoxColumn.Width = 5;
            // 
            // Image
            // 
            this.Image.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Image.DataPropertyName = "Image";
            this.Image.HeaderText = "Image";
            this.Image.Name = "Image";
            this.Image.ReadOnly = true;
            this.Image.Width = 5;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            this.nameDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Started
            // 
            this.Started.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Started.DataPropertyName = "Started";
            this.Started.HeaderText = "Started";
            this.Started.MinimumWidth = 2;
            this.Started.Name = "Started";
            this.Started.ReadOnly = true;
            this.Started.Width = 2;
            // 
            // Duration
            // 
            this.Duration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
            this.Duration.DataPropertyName = "Duration";
            this.Duration.HeaderText = "Duration";
            this.Duration.MinimumWidth = 24;
            this.Duration.Name = "Duration";
            this.Duration.ReadOnly = true;
            this.Duration.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Duration.Width = 24;
            // 
            // loaderItemBindingSource
            // 
            this.loaderItemBindingSource.DataSource = typeof(Data.Models.LoaderItem);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(762, 265);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(75, 23);
            this.btnTest.TabIndex = 66;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // lblEoddataLogged
            // 
            this.lblEoddataLogged.AutoSize = true;
            this.lblEoddataLogged.BackColor = System.Drawing.Color.Red;
            this.lblEoddataLogged.ForeColor = System.Drawing.Color.White;
            this.lblEoddataLogged.Location = new System.Drawing.Point(337, 3);
            this.lblEoddataLogged.Name = "lblEoddataLogged";
            this.lblEoddataLogged.Size = new System.Drawing.Size(153, 15);
            this.lblEoddataLogged.TabIndex = 65;
            this.lblEoddataLogged.Text = "Not logged in eoddata.com";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(968, 360);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnMinuteYahooLog
            // 
            this.btnMinuteYahooLog.Location = new System.Drawing.Point(575, 19);
            this.btnMinuteYahooLog.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnMinuteYahooLog.Name = "btnMinuteYahooLog";
            this.btnMinuteYahooLog.Size = new System.Drawing.Size(170, 27);
            this.btnMinuteYahooLog.TabIndex = 69;
            this.btnMinuteYahooLog.Text = "Minute Yahoo Log (for zip)";
            this.btnMinuteYahooLog.UseVisualStyleBackColor = true;
            this.btnMinuteYahooLog.Click += new System.EventHandler(this.btnMinuteYahooLog_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(976, 410);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpLoader.ResumeLayout(false);
            this.tpLoader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loaderItemBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpLoader;
        private System.Windows.Forms.Label lblEoddataLogged;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.BindingSource loaderItemBindingSource;
        private System.Windows.Forms.DataGridViewCheckBoxColumn checkedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewImageColumn Image;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn Started;
        private System.Windows.Forms.DataGridViewTextBoxColumn Duration;
        private System.Windows.Forms.Button btnRunMultiItemsLoader;
        private System.Windows.Forms.Button btnMinuteYahooLog;
    }
}