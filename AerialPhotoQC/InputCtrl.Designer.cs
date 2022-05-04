namespace AerialPhotoQC
{
    partial class InputCtrl
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
            this.DTMTB = new System.Windows.Forms.TextBox();
            this.DTML = new System.Windows.Forms.Label();
            this.DTMB = new System.Windows.Forms.Button();
            this.PRJB = new System.Windows.Forms.Button();
            this.PRJL = new System.Windows.Forms.Label();
            this.PRJTB = new System.Windows.Forms.TextBox();
            this.LinB = new System.Windows.Forms.Button();
            this.LinL = new System.Windows.Forms.Label();
            this.LinTB = new System.Windows.Forms.TextBox();
            this.PntB = new System.Windows.Forms.Button();
            this.PntL = new System.Windows.Forms.Label();
            this.PntTB = new System.Windows.Forms.TextBox();
            this.ProcB = new System.Windows.Forms.Button();
            this.DTMOD = new System.Windows.Forms.OpenFileDialog();
            this.PRJOD = new System.Windows.Forms.OpenFileDialog();
            this.LinOD = new System.Windows.Forms.OpenFileDialog();
            this.PntOD = new System.Windows.Forms.OpenFileDialog();
            this.MinLinDistTB = new System.Windows.Forms.TextBox();
            this.MinLinDistL = new System.Windows.Forms.Label();
            this.MaxLinDistL = new System.Windows.Forms.Label();
            this.MaxLinDistTB = new System.Windows.Forms.TextBox();
            this.ProcL = new System.Windows.Forms.Label();
            this.MaxAngTolL = new System.Windows.Forms.Label();
            this.MaxAngTolTB = new System.Windows.Forms.TextBox();
            this.GSDGridSizeL = new System.Windows.Forms.Label();
            this.GSDGridSizeTB = new System.Windows.Forms.TextBox();
            this.PrjPolyB = new System.Windows.Forms.Button();
            this.PrjPolyL = new System.Windows.Forms.Label();
            this.PrjPolyTB = new System.Windows.Forms.TextBox();
            this.PrjPolyOD = new System.Windows.Forms.OpenFileDialog();
            this.DefLinDistCB = new System.Windows.Forms.ComboBox();
            this.DefLinDistB = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // DTMTB
            // 
            this.DTMTB.Location = new System.Drawing.Point(112, 9);
            this.DTMTB.Name = "DTMTB";
            this.DTMTB.Size = new System.Drawing.Size(541, 22);
            this.DTMTB.TabIndex = 1;
            // 
            // DTML
            // 
            this.DTML.AutoSize = true;
            this.DTML.Location = new System.Drawing.Point(6, 12);
            this.DTML.Name = "DTML";
            this.DTML.Size = new System.Drawing.Size(42, 17);
            this.DTML.TabIndex = 0;
            this.DTML.Text = "DTM:";
            // 
            // DTMB
            // 
            this.DTMB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DTMB.Location = new System.Drawing.Point(659, 8);
            this.DTMB.Name = "DTMB";
            this.DTMB.Size = new System.Drawing.Size(35, 23);
            this.DTMB.TabIndex = 2;
            this.DTMB.Text = "...";
            this.DTMB.UseVisualStyleBackColor = true;
            this.DTMB.Click += new System.EventHandler(this.DTMB_Click);
            // 
            // PRJB
            // 
            this.PRJB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PRJB.Location = new System.Drawing.Point(659, 36);
            this.PRJB.Name = "PRJB";
            this.PRJB.Size = new System.Drawing.Size(35, 23);
            this.PRJB.TabIndex = 5;
            this.PRJB.Text = "...";
            this.PRJB.UseVisualStyleBackColor = true;
            this.PRJB.Click += new System.EventHandler(this.PRJB_Click);
            // 
            // PRJL
            // 
            this.PRJL.AutoSize = true;
            this.PRJL.Location = new System.Drawing.Point(6, 40);
            this.PRJL.Name = "PRJL";
            this.PRJL.Size = new System.Drawing.Size(38, 17);
            this.PRJL.TabIndex = 3;
            this.PRJL.Text = "PRJ:";
            // 
            // PRJTB
            // 
            this.PRJTB.Location = new System.Drawing.Point(112, 37);
            this.PRJTB.Name = "PRJTB";
            this.PRJTB.Size = new System.Drawing.Size(541, 22);
            this.PRJTB.TabIndex = 4;
            // 
            // LinB
            // 
            this.LinB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.LinB.Location = new System.Drawing.Point(659, 64);
            this.LinB.Name = "LinB";
            this.LinB.Size = new System.Drawing.Size(35, 23);
            this.LinB.TabIndex = 8;
            this.LinB.Text = "...";
            this.LinB.UseVisualStyleBackColor = true;
            this.LinB.Click += new System.EventHandler(this.LinB_Click);
            // 
            // LinL
            // 
            this.LinL.AutoSize = true;
            this.LinL.Location = new System.Drawing.Point(6, 68);
            this.LinL.Name = "LinL";
            this.LinL.Size = new System.Drawing.Size(46, 17);
            this.LinL.TabIndex = 6;
            this.LinL.Text = "Lines:";
            // 
            // LinTB
            // 
            this.LinTB.Location = new System.Drawing.Point(112, 65);
            this.LinTB.Name = "LinTB";
            this.LinTB.Size = new System.Drawing.Size(541, 22);
            this.LinTB.TabIndex = 7;
            // 
            // PntB
            // 
            this.PntB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PntB.Location = new System.Drawing.Point(659, 92);
            this.PntB.Name = "PntB";
            this.PntB.Size = new System.Drawing.Size(35, 23);
            this.PntB.TabIndex = 11;
            this.PntB.Text = "...";
            this.PntB.UseVisualStyleBackColor = true;
            this.PntB.Click += new System.EventHandler(this.PntB_Click);
            // 
            // PntL
            // 
            this.PntL.AutoSize = true;
            this.PntL.Location = new System.Drawing.Point(6, 96);
            this.PntL.Name = "PntL";
            this.PntL.Size = new System.Drawing.Size(51, 17);
            this.PntL.TabIndex = 9;
            this.PntL.Text = "Points:";
            // 
            // PntTB
            // 
            this.PntTB.Location = new System.Drawing.Point(112, 93);
            this.PntTB.Name = "PntTB";
            this.PntTB.Size = new System.Drawing.Size(541, 22);
            this.PntTB.TabIndex = 10;
            // 
            // ProcB
            // 
            this.ProcB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ProcB.Location = new System.Drawing.Point(9, 266);
            this.ProcB.Name = "ProcB";
            this.ProcB.Size = new System.Drawing.Size(75, 23);
            this.ProcB.TabIndex = 25;
            this.ProcB.Text = "Process";
            this.ProcB.UseVisualStyleBackColor = true;
            this.ProcB.Click += new System.EventHandler(this.ProcB_Click);
            // 
            // DTMOD
            // 
            this.DTMOD.DefaultExt = "*.las";
            this.DTMOD.Filter = "DTM LAS Files|*.las";
            this.DTMOD.Title = "Select DTM LAS File";
            // 
            // PRJOD
            // 
            this.PRJOD.DefaultExt = "*.prj";
            this.PRJOD.Filter = "PRJ Files|*.prj";
            this.PRJOD.Title = "Select PRJ File";
            // 
            // LinOD
            // 
            this.LinOD.DefaultExt = "*.shp";
            this.LinOD.Filter = "Lines Shape Files|*.shp";
            this.LinOD.Title = "Select Lines Shape File";
            // 
            // PntOD
            // 
            this.PntOD.DefaultExt = "*.shp";
            this.PntOD.Filter = "Points Shape Files|*.shp";
            this.PntOD.Title = "Select Points Shape File";
            // 
            // MinLinDistTB
            // 
            this.MinLinDistTB.Location = new System.Drawing.Point(112, 149);
            this.MinLinDistTB.Name = "MinLinDistTB";
            this.MinLinDistTB.Size = new System.Drawing.Size(100, 22);
            this.MinLinDistTB.TabIndex = 16;
            this.MinLinDistTB.Text = "250";
            // 
            // MinLinDistL
            // 
            this.MinLinDistL.AutoSize = true;
            this.MinLinDistL.Location = new System.Drawing.Point(6, 152);
            this.MinLinDistL.Name = "MinLinDistL";
            this.MinLinDistL.Size = new System.Drawing.Size(100, 17);
            this.MinLinDistL.TabIndex = 15;
            this.MinLinDistL.Text = "Min Lines Dist:";
            // 
            // MaxLinDistL
            // 
            this.MaxLinDistL.AutoSize = true;
            this.MaxLinDistL.Location = new System.Drawing.Point(6, 180);
            this.MaxLinDistL.Name = "MaxLinDistL";
            this.MaxLinDistL.Size = new System.Drawing.Size(103, 17);
            this.MaxLinDistL.TabIndex = 17;
            this.MaxLinDistL.Text = "Max Lines Dist:";
            // 
            // MaxLinDistTB
            // 
            this.MaxLinDistTB.Location = new System.Drawing.Point(112, 177);
            this.MaxLinDistTB.Name = "MaxLinDistTB";
            this.MaxLinDistTB.Size = new System.Drawing.Size(100, 22);
            this.MaxLinDistTB.TabIndex = 18;
            this.MaxLinDistTB.Text = "750";
            // 
            // ProcL
            // 
            this.ProcL.AutoSize = true;
            this.ProcL.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ProcL.Location = new System.Drawing.Point(109, 269);
            this.ProcL.Name = "ProcL";
            this.ProcL.Size = new System.Drawing.Size(14, 17);
            this.ProcL.TabIndex = 26;
            this.ProcL.Text = "-";
            this.ProcL.Visible = false;
            // 
            // MaxAngTolL
            // 
            this.MaxAngTolL.AutoSize = true;
            this.MaxAngTolL.Location = new System.Drawing.Point(6, 208);
            this.MaxAngTolL.Name = "MaxAngTolL";
            this.MaxAngTolL.Size = new System.Drawing.Size(90, 17);
            this.MaxAngTolL.TabIndex = 21;
            this.MaxAngTolL.Text = "Max Ang Tol:";
            // 
            // MaxAngTolTB
            // 
            this.MaxAngTolTB.Location = new System.Drawing.Point(112, 205);
            this.MaxAngTolTB.Name = "MaxAngTolTB";
            this.MaxAngTolTB.Size = new System.Drawing.Size(100, 22);
            this.MaxAngTolTB.TabIndex = 22;
            this.MaxAngTolTB.Text = "20";
            // 
            // GSDGridSizeL
            // 
            this.GSDGridSizeL.AutoSize = true;
            this.GSDGridSizeL.Location = new System.Drawing.Point(6, 236);
            this.GSDGridSizeL.Name = "GSDGridSizeL";
            this.GSDGridSizeL.Size = new System.Drawing.Size(104, 17);
            this.GSDGridSizeL.TabIndex = 23;
            this.GSDGridSizeL.Text = "GSD Grid Size:";
            // 
            // GSDGridSizeTB
            // 
            this.GSDGridSizeTB.Location = new System.Drawing.Point(112, 233);
            this.GSDGridSizeTB.Name = "GSDGridSizeTB";
            this.GSDGridSizeTB.Size = new System.Drawing.Size(100, 22);
            this.GSDGridSizeTB.TabIndex = 24;
            this.GSDGridSizeTB.Text = "10";
            // 
            // PrjPolyB
            // 
            this.PrjPolyB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.PrjPolyB.Location = new System.Drawing.Point(659, 120);
            this.PrjPolyB.Name = "PrjPolyB";
            this.PrjPolyB.Size = new System.Drawing.Size(35, 23);
            this.PrjPolyB.TabIndex = 14;
            this.PrjPolyB.Text = "...";
            this.PrjPolyB.UseVisualStyleBackColor = true;
            this.PrjPolyB.Click += new System.EventHandler(this.PrjPolyB_Click);
            // 
            // PrjPolyL
            // 
            this.PrjPolyL.AutoSize = true;
            this.PrjPolyL.Location = new System.Drawing.Point(6, 124);
            this.PrjPolyL.Name = "PrjPolyL";
            this.PrjPolyL.Size = new System.Drawing.Size(63, 17);
            this.PrjPolyL.TabIndex = 12;
            this.PrjPolyL.Text = "Polygon:";
            // 
            // PrjPolyTB
            // 
            this.PrjPolyTB.Location = new System.Drawing.Point(112, 121);
            this.PrjPolyTB.Name = "PrjPolyTB";
            this.PrjPolyTB.Size = new System.Drawing.Size(541, 22);
            this.PrjPolyTB.TabIndex = 13;
            // 
            // PrjPolyOD
            // 
            this.PrjPolyOD.DefaultExt = "*.shp";
            this.PrjPolyOD.Filter = "Polygon Shape Files|*.shp";
            this.PrjPolyOD.Title = "Select Polygon Shape File";
            // 
            // DefLinDistCB
            // 
            this.DefLinDistCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DefLinDistCB.FormattingEnabled = true;
            this.DefLinDistCB.Items.AddRange(new object[] {
            "Low Flight",
            "High Flight"});
            this.DefLinDistCB.Location = new System.Drawing.Point(220, 164);
            this.DefLinDistCB.Name = "DefLinDistCB";
            this.DefLinDistCB.Size = new System.Drawing.Size(100, 24);
            this.DefLinDistCB.TabIndex = 19;
            // 
            // DefLinDistB
            // 
            this.DefLinDistB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DefLinDistB.Location = new System.Drawing.Point(326, 163);
            this.DefLinDistB.Name = "DefLinDistB";
            this.DefLinDistB.Size = new System.Drawing.Size(75, 23);
            this.DefLinDistB.TabIndex = 20;
            this.DefLinDistB.Text = "Set";
            this.DefLinDistB.UseVisualStyleBackColor = true;
            this.DefLinDistB.Click += new System.EventHandler(this.DefLinDistB_Click);
            // 
            // InputCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.DefLinDistB);
            this.Controls.Add(this.DefLinDistCB);
            this.Controls.Add(this.PrjPolyB);
            this.Controls.Add(this.PrjPolyL);
            this.Controls.Add(this.PrjPolyTB);
            this.Controls.Add(this.GSDGridSizeL);
            this.Controls.Add(this.GSDGridSizeTB);
            this.Controls.Add(this.MaxAngTolL);
            this.Controls.Add(this.MaxAngTolTB);
            this.Controls.Add(this.ProcL);
            this.Controls.Add(this.MaxLinDistL);
            this.Controls.Add(this.MaxLinDistTB);
            this.Controls.Add(this.MinLinDistL);
            this.Controls.Add(this.MinLinDistTB);
            this.Controls.Add(this.ProcB);
            this.Controls.Add(this.PntB);
            this.Controls.Add(this.PntL);
            this.Controls.Add(this.PntTB);
            this.Controls.Add(this.LinB);
            this.Controls.Add(this.LinL);
            this.Controls.Add(this.LinTB);
            this.Controls.Add(this.PRJB);
            this.Controls.Add(this.PRJL);
            this.Controls.Add(this.PRJTB);
            this.Controls.Add(this.DTMB);
            this.Controls.Add(this.DTML);
            this.Controls.Add(this.DTMTB);
            this.Name = "InputCtrl";
            this.Size = new System.Drawing.Size(700, 500);
            this.Resize += new System.EventHandler(this.InputCtrl_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox DTMTB;
        private System.Windows.Forms.Label DTML;
        private System.Windows.Forms.Button DTMB;
        private System.Windows.Forms.Button PRJB;
        private System.Windows.Forms.Label PRJL;
        private System.Windows.Forms.TextBox PRJTB;
        private System.Windows.Forms.Button LinB;
        private System.Windows.Forms.Label LinL;
        private System.Windows.Forms.TextBox LinTB;
        private System.Windows.Forms.Button PntB;
        private System.Windows.Forms.Label PntL;
        private System.Windows.Forms.TextBox PntTB;
        private System.Windows.Forms.Button ProcB;
        private System.Windows.Forms.OpenFileDialog DTMOD;
        private System.Windows.Forms.OpenFileDialog PRJOD;
        private System.Windows.Forms.OpenFileDialog LinOD;
        private System.Windows.Forms.OpenFileDialog PntOD;
        private System.Windows.Forms.TextBox MinLinDistTB;
        private System.Windows.Forms.Label MinLinDistL;
        private System.Windows.Forms.Label MaxLinDistL;
        private System.Windows.Forms.TextBox MaxLinDistTB;
        private System.Windows.Forms.Label ProcL;
        private System.Windows.Forms.Label MaxAngTolL;
        private System.Windows.Forms.TextBox MaxAngTolTB;
        private System.Windows.Forms.Label GSDGridSizeL;
        private System.Windows.Forms.TextBox GSDGridSizeTB;
        private System.Windows.Forms.Button PrjPolyB;
        private System.Windows.Forms.Label PrjPolyL;
        private System.Windows.Forms.TextBox PrjPolyTB;
        private System.Windows.Forms.OpenFileDialog PrjPolyOD;
        private System.Windows.Forms.ComboBox DefLinDistCB;
        private System.Windows.Forms.Button DefLinDistB;
    }
}
