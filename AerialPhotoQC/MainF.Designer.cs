namespace AerialPhotoQC
{
    partial class MainF
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainF));
            this.MainTC = new System.Windows.Forms.TabControl();
            this.InputTP = new System.Windows.Forms.TabPage();
            this.InputFTP = new System.Windows.Forms.TabPage();
            this.StatsTP = new System.Windows.Forms.TabPage();
            this.ViewerTP = new System.Windows.Forms.TabPage();
            this.Input_Ctrl = new AerialPhotoQC.InputCtrl();
            this.InputF_Ctrl = new AerialPhotoQC.InputFCtrl();
            this.Stats_Ctrl = new AerialPhotoQC.StatsCtrl();
            this.Viewer_Ctrl = new AerialPhotoQC.ViewerCtrl();
            this.MainTC.SuspendLayout();
            this.InputTP.SuspendLayout();
            this.InputFTP.SuspendLayout();
            this.StatsTP.SuspendLayout();
            this.ViewerTP.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainTC
            // 
            this.MainTC.Controls.Add(this.InputTP);
            this.MainTC.Controls.Add(this.InputFTP);
            this.MainTC.Controls.Add(this.StatsTP);
            this.MainTC.Controls.Add(this.ViewerTP);
            this.MainTC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTC.Location = new System.Drawing.Point(0, 0);
            this.MainTC.Name = "MainTC";
            this.MainTC.SelectedIndex = 0;
            this.MainTC.Size = new System.Drawing.Size(782, 553);
            this.MainTC.TabIndex = 0;
            // 
            // InputTP
            // 
            this.InputTP.BackColor = System.Drawing.SystemColors.Control;
            this.InputTP.Controls.Add(this.Input_Ctrl);
            this.InputTP.Location = new System.Drawing.Point(4, 25);
            this.InputTP.Name = "InputTP";
            this.InputTP.Padding = new System.Windows.Forms.Padding(3);
            this.InputTP.Size = new System.Drawing.Size(774, 524);
            this.InputTP.TabIndex = 0;
            this.InputTP.Text = "Plan";
            // 
            // InputFTP
            // 
            this.InputFTP.BackColor = System.Drawing.SystemColors.Control;
            this.InputFTP.Controls.Add(this.InputF_Ctrl);
            this.InputFTP.Location = new System.Drawing.Point(4, 25);
            this.InputFTP.Name = "InputFTP";
            this.InputFTP.Padding = new System.Windows.Forms.Padding(3);
            this.InputFTP.Size = new System.Drawing.Size(774, 524);
            this.InputFTP.TabIndex = 1;
            this.InputFTP.Text = "Flight";
            // 
            // StatsTP
            // 
            this.StatsTP.BackColor = System.Drawing.SystemColors.Control;
            this.StatsTP.Controls.Add(this.Stats_Ctrl);
            this.StatsTP.Location = new System.Drawing.Point(4, 25);
            this.StatsTP.Name = "StatsTP";
            this.StatsTP.Padding = new System.Windows.Forms.Padding(3);
            this.StatsTP.Size = new System.Drawing.Size(774, 524);
            this.StatsTP.TabIndex = 2;
            this.StatsTP.Text = "Statistics";
            // 
            // ViewerTP
            // 
            this.ViewerTP.BackColor = System.Drawing.SystemColors.Control;
            this.ViewerTP.Controls.Add(this.Viewer_Ctrl);
            this.ViewerTP.Location = new System.Drawing.Point(4, 25);
            this.ViewerTP.Name = "ViewerTP";
            this.ViewerTP.Padding = new System.Windows.Forms.Padding(3);
            this.ViewerTP.Size = new System.Drawing.Size(774, 524);
            this.ViewerTP.TabIndex = 3;
            this.ViewerTP.Text = "Viewer";
            // 
            // Input_Ctrl
            // 
            this.Input_Ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Input_Ctrl.Location = new System.Drawing.Point(3, 3);
            this.Input_Ctrl.Name = "Input_Ctrl";
            this.Input_Ctrl.Size = new System.Drawing.Size(768, 518);
            this.Input_Ctrl.TabIndex = 0;
            // 
            // InputF_Ctrl
            // 
            this.InputF_Ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InputF_Ctrl.Location = new System.Drawing.Point(3, 3);
            this.InputF_Ctrl.Name = "InputF_Ctrl";
            this.InputF_Ctrl.Size = new System.Drawing.Size(768, 518);
            this.InputF_Ctrl.TabIndex = 0;
            // 
            // Stats_Ctrl
            // 
            this.Stats_Ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Stats_Ctrl.Location = new System.Drawing.Point(3, 3);
            this.Stats_Ctrl.Name = "Stats_Ctrl";
            this.Stats_Ctrl.Size = new System.Drawing.Size(768, 518);
            this.Stats_Ctrl.TabIndex = 0;
            // 
            // Viewer_Ctrl
            // 
            this.Viewer_Ctrl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Viewer_Ctrl.Location = new System.Drawing.Point(3, 3);
            this.Viewer_Ctrl.Name = "Viewer_Ctrl";
            this.Viewer_Ctrl.Size = new System.Drawing.Size(768, 518);
            this.Viewer_Ctrl.TabIndex = 0;
            // 
            // MainF
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.MainTC);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "MainF";
            this.Text = "Aerial Photo Quality Control, Version 2.5 (2022/05/04 12:00)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.MainTC.ResumeLayout(false);
            this.InputTP.ResumeLayout(false);
            this.InputFTP.ResumeLayout(false);
            this.StatsTP.ResumeLayout(false);
            this.ViewerTP.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl MainTC;
        private System.Windows.Forms.TabPage InputTP;
        private System.Windows.Forms.TabPage InputFTP;
        private System.Windows.Forms.TabPage StatsTP;
        private System.Windows.Forms.TabPage ViewerTP;
        private InputCtrl Input_Ctrl;
        private InputFCtrl InputF_Ctrl;
        private StatsCtrl Stats_Ctrl;
        private ViewerCtrl Viewer_Ctrl;
    }
}

