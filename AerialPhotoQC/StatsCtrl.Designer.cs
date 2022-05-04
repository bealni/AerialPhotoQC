namespace AerialPhotoQC
{
    partial class StatsCtrl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.VarCB = new System.Windows.Forms.ComboBox();
            this.VarL = new System.Windows.Forms.Label();
            this.StatsLV = new System.Windows.Forms.ListView();
            this.DataCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ValueCH = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.CleanB = new System.Windows.Forms.Button();
            this.SaveB = new System.Windows.Forms.Button();
            this.LoadB = new System.Windows.Forms.Button();
            this.SaveSD = new System.Windows.Forms.SaveFileDialog();
            this.Graph_Ctrl = new AerialPhotoQC.GraphCtrl();
            this.LoadOD = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // VarCB
            // 
            this.VarCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VarCB.Enabled = false;
            this.VarCB.FormattingEnabled = true;
            this.VarCB.Items.AddRange(new object[] {
            "Longitudinal Overlap, %",
            "Transversal Overlap, %",
            "GSD, cm"});
            this.VarCB.Location = new System.Drawing.Point(76, 9);
            this.VarCB.Name = "VarCB";
            this.VarCB.Size = new System.Drawing.Size(150, 24);
            this.VarCB.TabIndex = 1;
            this.VarCB.SelectedIndexChanged += new System.EventHandler(this.VarCB_SelectedIndexChanged);
            // 
            // VarL
            // 
            this.VarL.AutoSize = true;
            this.VarL.Enabled = false;
            this.VarL.Location = new System.Drawing.Point(6, 12);
            this.VarL.Name = "VarL";
            this.VarL.Size = new System.Drawing.Size(64, 17);
            this.VarL.TabIndex = 0;
            this.VarL.Text = "Variable:";
            // 
            // StatsLV
            // 
            this.StatsLV.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DataCH,
            this.ValueCH});
            this.StatsLV.Enabled = false;
            this.StatsLV.FullRowSelect = true;
            this.StatsLV.GridLines = true;
            this.StatsLV.HideSelection = false;
            this.StatsLV.Location = new System.Drawing.Point(9, 39);
            this.StatsLV.Name = "StatsLV";
            this.StatsLV.Size = new System.Drawing.Size(683, 190);
            this.StatsLV.TabIndex = 5;
            this.StatsLV.UseCompatibleStateImageBehavior = false;
            this.StatsLV.View = System.Windows.Forms.View.Details;
            // 
            // DataCH
            // 
            this.DataCH.Text = "Data";
            // 
            // ValueCH
            // 
            this.ValueCH.Text = "Value";
            this.ValueCH.Width = 169;
            // 
            // CleanB
            // 
            this.CleanB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.CleanB.Enabled = false;
            this.CleanB.Location = new System.Drawing.Point(232, 8);
            this.CleanB.Name = "CleanB";
            this.CleanB.Size = new System.Drawing.Size(75, 23);
            this.CleanB.TabIndex = 2;
            this.CleanB.Text = "Clean";
            this.CleanB.UseVisualStyleBackColor = true;
            this.CleanB.Click += new System.EventHandler(this.CleanB_Click);
            // 
            // SaveB
            // 
            this.SaveB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.SaveB.Enabled = false;
            this.SaveB.Location = new System.Drawing.Point(313, 8);
            this.SaveB.Name = "SaveB";
            this.SaveB.Size = new System.Drawing.Size(75, 23);
            this.SaveB.TabIndex = 3;
            this.SaveB.Text = "Save";
            this.SaveB.UseVisualStyleBackColor = true;
            this.SaveB.Click += new System.EventHandler(this.SaveB_Click);
            // 
            // LoadB
            // 
            this.LoadB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LoadB.Location = new System.Drawing.Point(394, 9);
            this.LoadB.Name = "LoadB";
            this.LoadB.Size = new System.Drawing.Size(75, 23);
            this.LoadB.TabIndex = 4;
            this.LoadB.Text = "Load";
            this.LoadB.UseVisualStyleBackColor = true;
            this.LoadB.Click += new System.EventHandler(this.LoadB_Click);
            // 
            // SaveSD
            // 
            this.SaveSD.DefaultExt = "*.txt";
            this.SaveSD.Filter = "Statistics Text Files|*.txt";
            this.SaveSD.Title = "Select Statistics Text File to Save to";
            // 
            // Graph_Ctrl
            // 
            this.Graph_Ctrl.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Graph_Ctrl.Enabled = false;
            this.Graph_Ctrl.Location = new System.Drawing.Point(0, 235);
            this.Graph_Ctrl.Name = "Graph_Ctrl";
            this.Graph_Ctrl.Size = new System.Drawing.Size(700, 265);
            this.Graph_Ctrl.TabIndex = 6;
            // 
            // LoadOD
            // 
            this.LoadOD.DefaultExt = "*.txt";
            this.LoadOD.Filter = "Statistics Text Files|*.txt";
            this.LoadOD.Title = "Select Statistics Text File to Load from";
            // 
            // StatsCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.LoadB);
            this.Controls.Add(this.SaveB);
            this.Controls.Add(this.CleanB);
            this.Controls.Add(this.StatsLV);
            this.Controls.Add(this.Graph_Ctrl);
            this.Controls.Add(this.VarL);
            this.Controls.Add(this.VarCB);
            this.Name = "StatsCtrl";
            this.Size = new System.Drawing.Size(700, 500);
            this.Resize += new System.EventHandler(this.StatsCtrl_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox VarCB;
        private System.Windows.Forms.Label VarL;
        private GraphCtrl Graph_Ctrl;
        private System.Windows.Forms.ListView StatsLV;
        private System.Windows.Forms.ColumnHeader DataCH;
        private System.Windows.Forms.ColumnHeader ValueCH;
        private System.Windows.Forms.Button CleanB;
        private System.Windows.Forms.Button SaveB;
        private System.Windows.Forms.Button LoadB;
        private System.Windows.Forms.SaveFileDialog SaveSD;
        private System.Windows.Forms.OpenFileDialog LoadOD;
    }
}
