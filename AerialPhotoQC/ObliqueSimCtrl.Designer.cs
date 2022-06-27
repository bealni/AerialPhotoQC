namespace AerialPhotoQC
{
    partial class ObliqueSimCtrl
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
            this.CtrlP = new System.Windows.Forms.Panel();
            this.TitleOverOblL = new System.Windows.Forms.Label();
            this.TitleOverNadL = new System.Windows.Forms.Label();
            this.SelOverL = new System.Windows.Forms.Label();
            this.SelOverCB = new System.Windows.Forms.ComboBox();
            this.ObliqueTrnsL = new System.Windows.Forms.Label();
            this.ObliqueTrnsTB = new System.Windows.Forms.TextBox();
            this.GSDL = new System.Windows.Forms.Label();
            this.GSDTB = new System.Windows.Forms.TextBox();
            this.ObliqueLonL = new System.Windows.Forms.Label();
            this.ObliqueLonTB = new System.Windows.Forms.TextBox();
            this.NadirLonL = new System.Windows.Forms.Label();
            this.NadirLonTB = new System.Windows.Forms.TextBox();
            this.ApplyB = new System.Windows.Forms.Button();
            this.DrawDP = new AerialPhotoQC.DrawPanel();
            this.PlanePB = new System.Windows.Forms.PictureBox();
            this.CtrlP.SuspendLayout();
            this.DrawDP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PlanePB)).BeginInit();
            this.SuspendLayout();
            // 
            // CtrlP
            // 
            this.CtrlP.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CtrlP.Controls.Add(this.TitleOverOblL);
            this.CtrlP.Controls.Add(this.TitleOverNadL);
            this.CtrlP.Controls.Add(this.SelOverL);
            this.CtrlP.Controls.Add(this.SelOverCB);
            this.CtrlP.Controls.Add(this.ObliqueTrnsL);
            this.CtrlP.Controls.Add(this.ObliqueTrnsTB);
            this.CtrlP.Controls.Add(this.GSDL);
            this.CtrlP.Controls.Add(this.GSDTB);
            this.CtrlP.Controls.Add(this.ObliqueLonL);
            this.CtrlP.Controls.Add(this.ObliqueLonTB);
            this.CtrlP.Controls.Add(this.NadirLonL);
            this.CtrlP.Controls.Add(this.NadirLonTB);
            this.CtrlP.Controls.Add(this.ApplyB);
            this.CtrlP.Dock = System.Windows.Forms.DockStyle.Left;
            this.CtrlP.Location = new System.Drawing.Point(0, 0);
            this.CtrlP.Name = "CtrlP";
            this.CtrlP.Size = new System.Drawing.Size(230, 500);
            this.CtrlP.TabIndex = 1;
            // 
            // TitleOverOblL
            // 
            this.TitleOverOblL.AutoSize = true;
            this.TitleOverOblL.Location = new System.Drawing.Point(12, 127);
            this.TitleOverOblL.Name = "TitleOverOblL";
            this.TitleOverOblL.Size = new System.Drawing.Size(221, 17);
            this.TitleOverOblL.TabIndex = 6;
            this.TitleOverOblL.Text = "Oblique Transversal Overlaps, %:";
            // 
            // TitleOverNadL
            // 
            this.TitleOverNadL.AutoSize = true;
            this.TitleOverNadL.Location = new System.Drawing.Point(12, 43);
            this.TitleOverNadL.Name = "TitleOverNadL";
            this.TitleOverNadL.Size = new System.Drawing.Size(131, 17);
            this.TitleOverNadL.TabIndex = 2;
            this.TitleOverNadL.Text = "Nadiral Overlap, %:";
            // 
            // SelOverL
            // 
            this.SelOverL.AutoSize = true;
            this.SelOverL.Location = new System.Drawing.Point(11, 230);
            this.SelOverL.Name = "SelOverL";
            this.SelOverL.Size = new System.Drawing.Size(105, 17);
            this.SelOverL.TabIndex = 11;
            this.SelOverL.Text = "Select Overlap:";
            // 
            // SelOverCB
            // 
            this.SelOverCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelOverCB.FormattingEnabled = true;
            this.SelOverCB.Items.AddRange(new object[] {
            "None",
            "Nadiral",
            "Oblque Frontal",
            "Oblque Lateral"});
            this.SelOverCB.Location = new System.Drawing.Point(118, 227);
            this.SelOverCB.Name = "SelOverCB";
            this.SelOverCB.Size = new System.Drawing.Size(100, 24);
            this.SelOverCB.TabIndex = 12;
            this.SelOverCB.SelectedIndexChanged += new System.EventHandler(this.SelOverCB_SelectedIndexChanged);
            // 
            // ObliqueTrnsL
            // 
            this.ObliqueTrnsL.AutoSize = true;
            this.ObliqueTrnsL.Location = new System.Drawing.Point(11, 174);
            this.ObliqueTrnsL.Name = "ObliqueTrnsL";
            this.ObliqueTrnsL.Size = new System.Drawing.Size(56, 17);
            this.ObliqueTrnsL.TabIndex = 9;
            this.ObliqueTrnsL.Text = "Lateral:";
            // 
            // ObliqueTrnsTB
            // 
            this.ObliqueTrnsTB.Location = new System.Drawing.Point(118, 171);
            this.ObliqueTrnsTB.Name = "ObliqueTrnsTB";
            this.ObliqueTrnsTB.ReadOnly = true;
            this.ObliqueTrnsTB.Size = new System.Drawing.Size(100, 22);
            this.ObliqueTrnsTB.TabIndex = 10;
            // 
            // GSDL
            // 
            this.GSDL.AutoSize = true;
            this.GSDL.Location = new System.Drawing.Point(11, 14);
            this.GSDL.Name = "GSDL";
            this.GSDL.Size = new System.Drawing.Size(68, 17);
            this.GSDL.TabIndex = 0;
            this.GSDL.Text = "GSD, cm:";
            // 
            // GSDTB
            // 
            this.GSDTB.Location = new System.Drawing.Point(118, 11);
            this.GSDTB.Name = "GSDTB";
            this.GSDTB.Size = new System.Drawing.Size(100, 22);
            this.GSDTB.TabIndex = 1;
            // 
            // ObliqueLonL
            // 
            this.ObliqueLonL.AutoSize = true;
            this.ObliqueLonL.Location = new System.Drawing.Point(11, 150);
            this.ObliqueLonL.Name = "ObliqueLonL";
            this.ObliqueLonL.Size = new System.Drawing.Size(56, 17);
            this.ObliqueLonL.TabIndex = 7;
            this.ObliqueLonL.Text = "Frontal:";
            // 
            // ObliqueLonTB
            // 
            this.ObliqueLonTB.Location = new System.Drawing.Point(118, 147);
            this.ObliqueLonTB.Name = "ObliqueLonTB";
            this.ObliqueLonTB.ReadOnly = true;
            this.ObliqueLonTB.Size = new System.Drawing.Size(100, 22);
            this.ObliqueLonTB.TabIndex = 8;
            // 
            // NadirLonL
            // 
            this.NadirLonL.AutoSize = true;
            this.NadirLonL.Location = new System.Drawing.Point(11, 66);
            this.NadirLonL.Name = "NadirLonL";
            this.NadirLonL.Size = new System.Drawing.Size(89, 17);
            this.NadirLonL.TabIndex = 3;
            this.NadirLonL.Text = "Longitudinal:";
            // 
            // NadirLonTB
            // 
            this.NadirLonTB.Location = new System.Drawing.Point(118, 63);
            this.NadirLonTB.Name = "NadirLonTB";
            this.NadirLonTB.Size = new System.Drawing.Size(100, 22);
            this.NadirLonTB.TabIndex = 4;
            // 
            // ApplyB
            // 
            this.ApplyB.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ApplyB.Location = new System.Drawing.Point(15, 91);
            this.ApplyB.Name = "ApplyB";
            this.ApplyB.Size = new System.Drawing.Size(75, 23);
            this.ApplyB.TabIndex = 5;
            this.ApplyB.Text = "Apply";
            this.ApplyB.UseVisualStyleBackColor = true;
            this.ApplyB.Click += new System.EventHandler(this.ApplyB_Click);
            // 
            // DrawDP
            // 
            this.DrawDP.BackColor = System.Drawing.Color.White;
            this.DrawDP.Controls.Add(this.PlanePB);
            this.DrawDP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DrawDP.Location = new System.Drawing.Point(230, 0);
            this.DrawDP.Name = "DrawDP";
            this.DrawDP.Size = new System.Drawing.Size(470, 500);
            this.DrawDP.TabIndex = 2;
            this.DrawDP.Paint += new System.Windows.Forms.PaintEventHandler(this.DrawDP_Paint);
            this.DrawDP.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DrawDP_MouseDown);
            this.DrawDP.MouseMove += new System.Windows.Forms.MouseEventHandler(this.DrawDP_MouseMove);
            this.DrawDP.Resize += new System.EventHandler(this.DrawDP_Resize);
            // 
            // PlanePB
            // 
            this.PlanePB.Image = global::AerialPhotoQC.Properties.Resources.Plane;
            this.PlanePB.Location = new System.Drawing.Point(0, 0);
            this.PlanePB.Name = "PlanePB";
            this.PlanePB.Size = new System.Drawing.Size(128, 128);
            this.PlanePB.TabIndex = 1;
            this.PlanePB.TabStop = false;
            // 
            // ObliqueSimCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.DrawDP);
            this.Controls.Add(this.CtrlP);
            this.Name = "ObliqueSimCtrl";
            this.Size = new System.Drawing.Size(700, 500);
            this.CtrlP.ResumeLayout(false);
            this.CtrlP.PerformLayout();
            this.DrawDP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PlanePB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel CtrlP;
        private System.Windows.Forms.Label TitleOverOblL;
        private System.Windows.Forms.Label TitleOverNadL;
        private System.Windows.Forms.Label SelOverL;
        private System.Windows.Forms.ComboBox SelOverCB;
        private System.Windows.Forms.Label ObliqueTrnsL;
        private System.Windows.Forms.TextBox ObliqueTrnsTB;
        private System.Windows.Forms.Label GSDL;
        private System.Windows.Forms.TextBox GSDTB;
        private System.Windows.Forms.Label ObliqueLonL;
        private System.Windows.Forms.TextBox ObliqueLonTB;
        private System.Windows.Forms.Label NadirLonL;
        private System.Windows.Forms.TextBox NadirLonTB;
        private System.Windows.Forms.Button ApplyB;
        private DrawPanel DrawDP;
        private System.Windows.Forms.PictureBox PlanePB;
    }
}
