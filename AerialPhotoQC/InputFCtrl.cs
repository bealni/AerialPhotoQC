using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AerialPhotoQC
{
    public partial class InputFCtrl : UserControl
    {
        private const double MARGIN = 2000.0;

        private const int DEF_MIN_LIN_DIST_LOW = 250;
        private const int DEF_MAX_LIN_DIST_LOW = 750;
        private const int DEF_MIN_LIN_DIST_HIGH = 1500;
        private const int DEF_MAX_LIN_DIST_HIGH = 2500;

        private ShpInfoF m_ShpInfo;
        private DTMInfoF m_DTMInfo;
        private PRJInfo m_PRJInfo;
        private MonoModel m_MonoModel;

        private StatsF m_Stats;

        public delegate void OnBeforeStartedFHandler();
        public OnBeforeStartedFHandler OnBeforeStartedF;
        public delegate void OnFinishedFHandler(String ImgsFile,
                                                String PolyFile,
                                                StatsF S);
        public OnFinishedFHandler OnFinishedF;

        /*=== InputFCtrl() ===*/
        public InputFCtrl()
        {
            InitializeComponent();

            m_ShpInfo = null;
            m_DTMInfo = null;
            m_PRJInfo = null;
            m_MonoModel = null;

            m_Stats = null;

            OnBeforeStartedF = null;
            OnFinishedF = null;

            DefLinDistCB.SelectedIndex = 0;
        }

        /*=== InputFCtrl_Resize() ===*/
        private void InputFCtrl_Resize(object sender, EventArgs e)
        {
            DTMB.Left = this.Width - 41;
            DTMTB.Width = this.Width - 159;
            PRJB.Left = DTMB.Left;
            PRJTB.Width = DTMTB.Width;
            LinB.Left = DTMB.Left;
            LinTB.Width = DTMTB.Width;
            PntB.Left = DTMB.Left;
            PntTB.Width = DTMTB.Width;
            PrjPolyB.Left = DTMB.Left;
            PrjPolyTB.Width = DTMTB.Width;
        }

        /*=== DTMB_Click() ===*/
        private void DTMB_Click(object sender, EventArgs e)
        {
            if (DTMOD.ShowDialog() == DialogResult.OK)
                DTMTB.Text = DTMOD.FileName;
        }

        /*=== PRJB_Click() ===*/
        private void PRJB_Click(object sender, EventArgs e)
        {
            if (PRJOD.ShowDialog() == DialogResult.OK)
                PRJTB.Text = PRJOD.FileName;
        }

        /*=== LinB_Click() ===*/
        private void LinB_Click(object sender, EventArgs e)
        {
            if (LinOD.ShowDialog() == DialogResult.OK)
                LinTB.Text = LinOD.FileName;
        }

        /*=== PntB_Click() ===*/
        private void PntB_Click(object sender, EventArgs e)
        {
            if (PntOD.ShowDialog() == DialogResult.OK)
                PntTB.Text = PntOD.FileName;
        }

        /*=== PrjPolyB_Click() ===*/
        private void PrjPolyB_Click(object sender, EventArgs e)
        {
            if (PrjPolyOD.ShowDialog() == DialogResult.OK)
                PrjPolyTB.Text = PrjPolyOD.FileName;
        }

        /*=== DefLinDistB_Click() ===*/
        private void DefLinDistB_Click(object sender, EventArgs e)
        {
            if (DefLinDistCB.SelectedIndex == 0)
            {
                MinLinDistTB.Text = DEF_MIN_LIN_DIST_LOW.ToString();
                MaxLinDistTB.Text = DEF_MAX_LIN_DIST_LOW.ToString();
            }
            else
            {
                MinLinDistTB.Text = DEF_MIN_LIN_DIST_HIGH.ToString();
                MaxLinDistTB.Text = DEF_MAX_LIN_DIST_HIGH.ToString();
            }
        }

        /*=== ProcB_Click() ===*/
        private void ProcB_Click(object sender, EventArgs e)
        {
            String Res;
            int iRes;
            double MinLinDist, MaxLinDist, MaxAngTol;
            int GSDGridSize;

            if (OnBeforeStartedF != null)
                OnBeforeStartedF();

            DTMTB.Text = DTMTB.Text.Trim();
            PRJTB.Text = PRJTB.Text.Trim();
            LinTB.Text = LinTB.Text.Trim();
            PntTB.Text = PntTB.Text.Trim();
            PrjPolyTB.Text = PrjPolyTB.Text.Trim();
            Application.DoEvents();

            if (DTMTB.Text == "")
            {
                MessageBox.Show("DTM File is not specified.",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (PRJTB.Text == "")
            {
                MessageBox.Show("PRJ File is not specified.",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (LinTB.Text == "")
            {
                MessageBox.Show("Lines File is not specified.",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (PntTB.Text == "")
            {
                MessageBox.Show("Points File is not specified.",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (PrjPolyTB.Text == "")
            {
                if (MessageBox.Show("Polygon File is not specified.\nContinue anyway?",
                                    "WARNING",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }

            if (!double.TryParse(MinLinDistTB.Text, out MinLinDist))
            {
                MessageBox.Show("Min Lines Dist is not a valid number.",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            if (MinLinDist < 50)
            {
                MessageBox.Show("Min Lines Dist must be greater than 50.",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(MaxLinDistTB.Text, out MaxLinDist))
            {
                MessageBox.Show("Max Lines Dist is not a valid number.",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            if (MaxLinDist > 5000)
            {
                MessageBox.Show("Max Lines Dist must be less than 5000.",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (MinLinDist > MaxLinDist)
            {
                MessageBox.Show("Min Lines Dist must be less than Max Lines Dist.",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (!double.TryParse(MaxAngTolTB.Text, out MaxAngTol))
            {
                MessageBox.Show("Max Ang Tol is not a valid number.",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            if (MaxAngTol < 1.0 || MaxAngTol >  45.0)
            {
                MessageBox.Show("Max Ang Tol must be in the range [1, 45].",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(GSDGridSizeTB.Text, out GSDGridSize))
            {
                MessageBox.Show("GSD Grid Size is not a valid integer number.",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            if (GSDGridSize < 2 || GSDGridSize > 100)
            {
                MessageBox.Show("GSD Grid Size must be in the range [2, 100].",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            this.Enabled = false;
            ProcL.Text = "Reading Shapes...";
            ProcL.Visible = true;
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;

            m_ShpInfo = new ShpInfoF();
            Res = m_ShpInfo.Open(LinTB.Text,
                                 PntTB.Text,
                                 PrjPolyTB.Text,
                                 
                                 MinLinDist,
                                 MaxLinDist,
                                 MaxAngTol);
            if (Res != "OK")
            {
                ProcL.Text = "";
                ProcL.Visible = false;
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                m_ShpInfo.Close();
                m_ShpInfo = null;
                MessageBox.Show("Error: " + Res,
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            ProcL.Text = "Reading PRJ...";
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;

            m_PRJInfo = new PRJInfo();
            iRes = m_PRJInfo.Open(PRJTB.Text);
            if (iRes != PRJInfo.ERR_NONE)
            {
                ProcL.Text = "";
                ProcL.Visible = false;
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                m_ShpInfo.Close();
                m_ShpInfo = null;
                Res = m_PRJInfo.GetErrStr(iRes);
                m_PRJInfo.Close();
                m_PRJInfo = null;
                MessageBox.Show("Error: " + Res,
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            m_MonoModel = new MonoModel();
            m_MonoModel.Open(m_PRJInfo.m_CamMatrix,
                             m_PRJInfo.m_CamMatrixInv,
                             m_PRJInfo.m_nCamCols,
                             m_PRJInfo.m_nCamRows,
                             m_PRJInfo.m_nCamFocal);

            ProcL.Text = "Reading DTM...";
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;

            m_DTMInfo = new DTMInfoF();
            Res = m_DTMInfo.Open(DTMTB.Text,

                                 m_ShpInfo.m_nMinX - MARGIN,
                                 m_ShpInfo.m_nMinY - MARGIN,
                                 m_ShpInfo.m_nMaxX + MARGIN,
                                 m_ShpInfo.m_nMaxY + MARGIN,
                                 
                                 this,
                                 ProcL);
            if (Res != "OK")
            {
                ProcL.Text = "";
                ProcL.Visible = false;
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                m_ShpInfo.Close();
                m_ShpInfo = null;
                m_DTMInfo.Close();
                m_DTMInfo = null;
                MessageBox.Show("Error: " + Res,
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            // Process
            ProcL.Text = "Computing Statistics...";
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;

            m_Stats = new StatsF();
            Res = m_Stats.Open(m_ShpInfo,
                               m_DTMInfo,
                               m_MonoModel,
                               GSDGridSize,

                               this,
                               ProcL);
            if (Res != "OK")
            {
                ProcL.Text = "";
                ProcL.Visible = false;
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                m_ShpInfo.Close();
                m_ShpInfo = null;
                m_DTMInfo.Close();
                m_DTMInfo = null;
                m_Stats.Close();
                m_Stats = null;
                MessageBox.Show("Error: " + Res,
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            // Write Output
            ProcL.Text = "Writing Lines...";
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;

            LinWriterF LW = new LinWriterF();
            Res = LW.Open(m_ShpInfo.m_sLinFileName,
                            Path.GetDirectoryName(m_ShpInfo.m_sLinFileName) + "\\" +
                            Path.GetFileNameWithoutExtension(m_ShpInfo.m_sLinFileName) + "_Stats.shp",
                            m_ShpInfo);
            if (Res != "OK")
            {
                ProcL.Text = "";
                ProcL.Visible = false;
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                m_ShpInfo.Close();
                m_ShpInfo = null;
                m_DTMInfo.Close();
                m_DTMInfo = null;
                m_Stats.Close();
                m_Stats = null;
                LW.Close();
                LW = null;
                MessageBox.Show("Error: " + Res,
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            LW.Close();
            LW = null;

            ProcL.Text = "Writing Points...";
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;

            PntWriterF PW = new PntWriterF();
            Res = PW.Open(m_ShpInfo.m_sPntFileName,
                          Path.GetDirectoryName(m_ShpInfo.m_sPntFileName) + "\\" +
                          Path.GetFileNameWithoutExtension(m_ShpInfo.m_sPntFileName) + "_Stats.shp",
                          m_ShpInfo);
            if (Res != "OK")
            {
                ProcL.Text = "";
                ProcL.Visible = false;
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                m_ShpInfo.Close();
                m_ShpInfo = null;
                m_DTMInfo.Close();
                m_DTMInfo = null;
                m_Stats.Close();
                m_Stats = null;
                PW.Close();
                PW = null;
                MessageBox.Show("Error: " + Res,
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            PW.Close();
            PW = null;

            ProcL.Text = "Writing Images...";
            Application.DoEvents();
            this.Cursor = Cursors.WaitCursor;

            ImgWriterF IW = new ImgWriterF();
            Res = IW.Open(Path.GetDirectoryName(m_ShpInfo.m_sPntFileName) + "\\" +
                          Path.GetFileNameWithoutExtension(m_ShpInfo.m_sPntFileName) + ".prj",
                          Path.GetDirectoryName(m_ShpInfo.m_sPntFileName) + "\\" +
                          Path.GetFileNameWithoutExtension(m_ShpInfo.m_sPntFileName) + "_Imgs.shp",
                          m_ShpInfo);
            if (Res != "OK")
            {
                ProcL.Text = "";
                ProcL.Visible = false;
                this.Enabled = true;
                this.Cursor = Cursors.Default;
                m_ShpInfo.Close();
                m_ShpInfo = null;
                m_DTMInfo.Close();
                m_DTMInfo = null;
                m_Stats.Close();
                m_Stats = null;
                IW.Close();
                IW = null;
                MessageBox.Show("Error: " + Res,
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }
            IW.Close();
            IW = null;

            ProcL.Text = "";
            ProcL.Visible = false;
            this.Enabled = true;
            this.Cursor = Cursors.Default;
            if (OnFinishedF != null)
            {
                OnFinishedF(Path.GetDirectoryName(m_ShpInfo.m_sPntFileName) + "\\" +
                            Path.GetFileNameWithoutExtension(m_ShpInfo.m_sPntFileName) + "_Imgs.shp",
                            PrjPolyTB.Text,
                            m_Stats);
                Application.DoEvents();
            }

            m_ShpInfo.Close();
            m_ShpInfo = null;
            m_DTMInfo.Close();
            m_DTMInfo = null;
            m_Stats.Close();
            m_Stats = null;
            GC.Collect();
        }
    }
}
