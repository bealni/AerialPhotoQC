namespace AerialPhotoQC
{
    partial class GraphCtrl
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
            this.MainDP = new AerialPhotoQC.DrawPanel();
            this.SuspendLayout();
            // 
            // MainDP
            // 
            this.MainDP.BackColor = System.Drawing.Color.White;
            this.MainDP.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainDP.Location = new System.Drawing.Point(0, 0);
            this.MainDP.Name = "MainDP";
            this.MainDP.Size = new System.Drawing.Size(700, 500);
            this.MainDP.TabIndex = 0;
            this.MainDP.Paint += new System.Windows.Forms.PaintEventHandler(this.MainDP_Paint);
            this.MainDP.Resize += new System.EventHandler(this.MainDP_Resize);
            // 
            // GraphCtrl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.MainDP);
            this.Name = "GraphCtrl";
            this.Size = new System.Drawing.Size(700, 500);
            this.ResumeLayout(false);

        }

        #endregion

        private DrawPanel MainDP;
    }
}
