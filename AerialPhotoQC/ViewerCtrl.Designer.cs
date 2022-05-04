namespace AerialPhotoQC
{
    partial class ViewerCtrl
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
            this.MainTS = new System.Windows.Forms.ToolStrip();
            this.VarCB = new System.Windows.Forms.ToolStripComboBox();
            this.CritCB = new System.Windows.Forms.ToolStripComboBox();
            this.LowHighCB = new System.Windows.Forms.ToolStripComboBox();
            this.Sep1TB = new System.Windows.Forms.ToolStripSeparator();
            this.ViewFPsTB = new System.Windows.Forms.ToolStripButton();
            this.ViewPtsTB = new System.Windows.Forms.ToolStripButton();
            this.ViewPolyTB = new System.Windows.Forms.ToolStripButton();
            this.Sep2TB = new System.Windows.Forms.ToolStripSeparator();
            this.ZoomAllTB = new System.Windows.Forms.ToolStripButton();
            this.ZoomCenteredTB = new System.Windows.Forms.ToolStripButton();
            this.Sep3TB = new System.Windows.Forms.ToolStripSeparator();
            this.HelpTB = new System.Windows.Forms.ToolStripButton();
            this.MainSS = new System.Windows.Forms.StatusStrip();
            this.CoordsSL = new System.Windows.Forms.ToolStripStatusLabel();
            this.InfoSL = new System.Windows.Forms.ToolStripStatusLabel();
            this.MainDP = new AerialPhotoQC.DrawPanel();
            this.MainTS.SuspendLayout();
            this.MainSS.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTS
            // 
            this.MainTS.Enabled = false;
            this.MainTS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.VarCB,
            this.CritCB,
            this.LowHighCB,
            this.Sep1TB,
            this.ViewFPsTB,
            this.ViewPtsTB,
            this.ViewPolyTB,
            this.Sep2TB,
            this.ZoomAllTB,
            this.ZoomCenteredTB,
            this.Sep3TB,
            this.HelpTB});
            this.MainTS.Location = new System.Drawing.Point(0, 0);
            this.MainTS.Name = "MainTS";
            this.MainTS.Size = new System.Drawing.Size(700, 31);
            this.MainTS.TabIndex = 0;
            // 
            // VarCB
            // 
            this.VarCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.VarCB.Items.AddRange(new object[] {
            "Longitudinal Overlap, %",
            "Transversal Overlap, %",
            "GSD, cm"});
            this.VarCB.Name = "VarCB";
            this.VarCB.Size = new System.Drawing.Size(160, 31);
            this.VarCB.ToolTipText = "Variable";
            this.VarCB.SelectedIndexChanged += new System.EventHandler(this.VarCB_SelectedIndexChanged);
            // 
            // CritCB
            // 
            this.CritCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CritCB.Items.AddRange(new object[] {
            "Statistics",
            "Limits"});
            this.CritCB.Name = "CritCB";
            this.CritCB.Size = new System.Drawing.Size(160, 31);
            this.CritCB.ToolTipText = "Criterion";
            this.CritCB.SelectedIndexChanged += new System.EventHandler(this.CritCB_SelectedIndexChanged);
            // 
            // LowHighCB
            // 
            this.LowHighCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.LowHighCB.Items.AddRange(new object[] {
            "Low Flight",
            "High Flight"});
            this.LowHighCB.Name = "LowHighCB";
            this.LowHighCB.Size = new System.Drawing.Size(121, 31);
            this.LowHighCB.SelectedIndexChanged += new System.EventHandler(this.LowHighCB_SelectedIndexChanged);
            // 
            // Sep1TB
            // 
            this.Sep1TB.Name = "Sep1TB";
            this.Sep1TB.Size = new System.Drawing.Size(6, 31);
            // 
            // ViewFPsTB
            // 
            this.ViewFPsTB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ViewFPsTB.Image = global::AerialPhotoQC.Properties.Resources.View_Pol;
            this.ViewFPsTB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ViewFPsTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewFPsTB.Name = "ViewFPsTB";
            this.ViewFPsTB.Size = new System.Drawing.Size(28, 28);
            this.ViewFPsTB.ToolTipText = "View Footprints";
            this.ViewFPsTB.Click += new System.EventHandler(this.ViewFPsTB_Click);
            // 
            // ViewPtsTB
            // 
            this.ViewPtsTB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ViewPtsTB.Image = global::AerialPhotoQC.Properties.Resources.View_Vect;
            this.ViewPtsTB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ViewPtsTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewPtsTB.Name = "ViewPtsTB";
            this.ViewPtsTB.Size = new System.Drawing.Size(28, 28);
            this.ViewPtsTB.ToolTipText = "View Centers";
            this.ViewPtsTB.Click += new System.EventHandler(this.ViewPtsTB_Click);
            // 
            // ViewPolyTB
            // 
            this.ViewPolyTB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ViewPolyTB.Image = global::AerialPhotoQC.Properties.Resources.View_Poly;
            this.ViewPolyTB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ViewPolyTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ViewPolyTB.Name = "ViewPolyTB";
            this.ViewPolyTB.Size = new System.Drawing.Size(28, 28);
            this.ViewPolyTB.ToolTipText = "View Polygon";
            this.ViewPolyTB.Click += new System.EventHandler(this.ViewPolyTB_Click);
            // 
            // Sep2TB
            // 
            this.Sep2TB.Name = "Sep2TB";
            this.Sep2TB.Size = new System.Drawing.Size(6, 31);
            // 
            // ZoomAllTB
            // 
            this.ZoomAllTB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomAllTB.Image = global::AerialPhotoQC.Properties.Resources.View_ZoomAll;
            this.ZoomAllTB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ZoomAllTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomAllTB.Name = "ZoomAllTB";
            this.ZoomAllTB.Size = new System.Drawing.Size(28, 28);
            this.ZoomAllTB.ToolTipText = "Zoom All";
            this.ZoomAllTB.Click += new System.EventHandler(this.ZoomAllTB_Click);
            // 
            // ZoomCenteredTB
            // 
            this.ZoomCenteredTB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ZoomCenteredTB.Image = global::AerialPhotoQC.Properties.Resources.View_ZoomCentered;
            this.ZoomCenteredTB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ZoomCenteredTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ZoomCenteredTB.Name = "ZoomCenteredTB";
            this.ZoomCenteredTB.Size = new System.Drawing.Size(28, 28);
            this.ZoomCenteredTB.Text = "Center Zoom on Mouse";
            this.ZoomCenteredTB.Click += new System.EventHandler(this.ZoomCenteredTB_Click);
            // 
            // Sep3TB
            // 
            this.Sep3TB.Name = "Sep3TB";
            this.Sep3TB.Size = new System.Drawing.Size(6, 31);
            // 
            // HelpTB
            // 
            this.HelpTB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.HelpTB.Image = global::AerialPhotoQC.Properties.Resources.Help;
            this.HelpTB.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.HelpTB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.HelpTB.Name = "HelpTB";
            this.HelpTB.Size = new System.Drawing.Size(28, 28);
            this.HelpTB.ToolTipText = "Help";
            this.HelpTB.Click += new System.EventHandler(this.HelpTB_Click);
            // 
            // MainSS
            // 
            this.MainSS.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CoordsSL,
            this.InfoSL});
            this.MainSS.Location = new System.Drawing.Point(0, 478);
            this.MainSS.Name = "MainSS";
            this.MainSS.Size = new System.Drawing.Size(700, 22);
            this.MainSS.TabIndex = 1;
            // 
            // CoordsSL
            // 
            this.CoordsSL.Name = "CoordsSL";
            this.CoordsSL.Size = new System.Drawing.Size(0, 17);
            this.CoordsSL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InfoSL
            // 
            this.InfoSL.Name = "InfoSL";
            this.InfoSL.Size = new System.Drawing.Size(0, 17);
            this.InfoSL.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MainDP
            // 
            this.MainDP.BackColor = System.Drawing.Color.White;
            this.MainDP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainDP.Location = new System.Drawing.Point(0, 31);
            this.MainDP.Name = "MainDP";
            this.MainDP.Size = new System.Drawing.Size(700, 447);
            this.MainDP.TabIndex = 2;
            this.MainDP.Paint += new System.Windows.Forms.PaintEventHandler(this.MainDP_Paint);
            this.MainDP.MouseDown += new System.Windows.Forms.MouseEventHandler(this.MainDP_MouseDown);
            this.MainDP.MouseLeave += new System.EventHandler(this.MainDP_MouseLeave);
            this.MainDP.MouseMove += new System.Windows.Forms.MouseEventHandler(this.MainDP_MouseMove);
            this.MainDP.MouseUp += new System.Windows.Forms.MouseEventHandler(this.MainDP_MouseUp);
            this.MainDP.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MainDP_MouseWheel);
            this.MainDP.Resize += new System.EventHandler(this.MainDP_Resize);
            // 
            // ViewerCtrl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.MainDP);
            this.Controls.Add(this.MainSS);
            this.Controls.Add(this.MainTS);
            this.Name = "ViewerCtrl";
            this.Size = new System.Drawing.Size(700, 500);
            this.MainTS.ResumeLayout(false);
            this.MainTS.PerformLayout();
            this.MainSS.ResumeLayout(false);
            this.MainSS.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip MainTS;
        private DrawPanel MainDP;
        private System.Windows.Forms.ToolStripComboBox VarCB;
        private System.Windows.Forms.ToolStripComboBox CritCB;
        private System.Windows.Forms.ToolStripSeparator Sep1TB;
        private System.Windows.Forms.ToolStripButton ViewFPsTB;
        private System.Windows.Forms.ToolStripButton ViewPtsTB;
        private System.Windows.Forms.StatusStrip MainSS;
        private System.Windows.Forms.ToolStripStatusLabel CoordsSL;
        private System.Windows.Forms.ToolStripSeparator Sep2TB;
        private System.Windows.Forms.ToolStripButton ZoomAllTB;
        private System.Windows.Forms.ToolStripButton ZoomCenteredTB;
        private System.Windows.Forms.ToolStripStatusLabel InfoSL;
        private System.Windows.Forms.ToolStripSeparator Sep3TB;
        private System.Windows.Forms.ToolStripButton HelpTB;
        private System.Windows.Forms.ToolStripButton ViewPolyTB;
        private System.Windows.Forms.ToolStripComboBox LowHighCB;
    }
}
