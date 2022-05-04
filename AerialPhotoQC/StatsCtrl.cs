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
    public partial class StatsCtrl : UserControl
    {
        private String m_sImgsFile;
        private String m_sPolyFile;

        private double m_nOverLon_Min;
        private double m_nOverLon_Max;
        private double m_nOverLon_Avg;
        private double m_nOverLon_Med;
        private double m_nOverLon_Std;
        private double m_nOverLon_Perc20Sigmas;
        private double m_nOverLon_Perc25Sigmas;
        private double m_nOverLon_Perc30Sigmas;
        private List<double> m_OverLon_HistX;
        private List<double> m_OverLon_HistY;

        private double m_nOverTrs_Min;
        private double m_nOverTrs_Max;
        private double m_nOverTrs_Avg;
        private double m_nOverTrs_Med;
        private double m_nOverTrs_Std;
        private double m_nOverTrs_Perc20Sigmas;
        private double m_nOverTrs_Perc25Sigmas;
        private double m_nOverTrs_Perc30Sigmas;
        private List<double> m_OverTrs_HistX;
        private List<double> m_OverTrs_HistY;

        private double m_nGSD_Min;
        private double m_nGSD_Max;
        private double m_nGSD_Avg;
        private double m_nGSD_Med;
        private double m_nGSD_Std;
        private double m_nGSD_Perc20Sigmas;
        private double m_nGSD_Perc25Sigmas;
        private double m_nGSD_Perc30Sigmas;
        private List<double> m_GSD_HistX;
        private List<double> m_GSD_HistY;

        private bool m_bIsFlight;

        private bool m_bStatsExist;

        public delegate void OnStatsLoadedHandler(bool IsFlight,

                                                  String ImgsFile,
                                                  String PolyFile,

                                                  double OverLon_Avg,
                                                  double OverLon_Std,

                                                  double OverTrs_Avg,
                                                  double OverTrs_Std,

                                                  double GSD_Avg,
                                                  double GSD_Std);
        public OnStatsLoadedHandler OnStatsLoaded;
        public delegate void OnStatsClearedHandler();
        public OnStatsClearedHandler OnStatsCleared;

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Public ------------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Public

        /*=== StatsCtrl() ===*/
        public StatsCtrl()
        {
            InitializeComponent();

            m_bIsFlight = false;

            m_bStatsExist = false;
            VarCB.SelectedIndex = 0;

            string[] s = new string[2];
            s[1] = "";

            s[0] = "Minimum";
            StatsLV.Items.Add(new ListViewItem(s));
            s[0] = "Maximum";
            StatsLV.Items.Add(new ListViewItem(s));
            s[0] = "Average";
            StatsLV.Items.Add(new ListViewItem(s));
            s[0] = "σ (STD)";
            StatsLV.Items.Add(new ListViewItem(s));
            s[0] = "Mode (Histogram)";
            StatsLV.Items.Add(new ListViewItem(s));
            s[0] = "% within 2.0σ";
            StatsLV.Items.Add(new ListViewItem(s));
            s[0] = "% within 2.5σ";
            StatsLV.Items.Add(new ListViewItem(s));
            s[0] = "% within 3.0σ";
            StatsLV.Items.Add(new ListViewItem(s));
            StatsLV.Columns[0].Width = -2;
            StatsLV.Columns[1].Width = -2;

            Rectangle R = StatsLV.GetItemRect(0);
            StatsLV.Height = 10 * R.Height;

            m_OverLon_HistX = null;
            m_OverLon_HistY = null;

            m_OverTrs_HistX = null;
            m_OverTrs_HistY = null;

            m_GSD_HistX = null;
            m_GSD_HistY = null;

            OnStatsLoaded = null;
            OnStatsCleared = null;
        }

        /*=== SetStats() ===*/
        public void SetStats(String ImgsFile,
                             String PolyFile,
                             Stats S)
        {
            int i, Count;

            m_sImgsFile = ImgsFile;
            m_sPolyFile = PolyFile;

            m_nOverLon_Min = S.m_nStats_OverLon_Min;
            m_nOverLon_Max = S.m_nStats_OverLon_Max;
            m_nOverLon_Avg = S.m_nStats_OverLon_Avg;
            m_nOverLon_Med = S.m_nStats_OverLon_Med;
            m_nOverLon_Std = S.m_nStats_OverLon_Std;
            m_nOverLon_Perc20Sigmas = S.m_nStats_OverLon_Perc20Sigmas;
            m_nOverLon_Perc25Sigmas = S.m_nStats_OverLon_Perc25Sigmas;
            m_nOverLon_Perc30Sigmas = S.m_nStats_OverLon_Perc30Sigmas;
            m_OverLon_HistX = new List<double>();
            m_OverLon_HistY = new List<double>();
            Count = S.m_Stats_OverLon_HistX.Count;
            for (i = 0; i < Count; i++)
            {
                m_OverLon_HistX.Add(S.m_Stats_OverLon_HistX[i]);
                m_OverLon_HistY.Add(S.m_Stats_OverLon_HistY[i]);
            }

            m_nOverTrs_Min = S.m_nStats_OverTrs_Min;
            m_nOverTrs_Max = S.m_nStats_OverTrs_Max;
            m_nOverTrs_Avg = S.m_nStats_OverTrs_Avg;
            m_nOverTrs_Med = S.m_nStats_OverTrs_Med;
            m_nOverTrs_Std = S.m_nStats_OverTrs_Std;
            m_nOverTrs_Perc20Sigmas = S.m_nStats_OverTrs_Perc20Sigmas;
            m_nOverTrs_Perc25Sigmas = S.m_nStats_OverTrs_Perc25Sigmas;
            m_nOverTrs_Perc30Sigmas = S.m_nStats_OverTrs_Perc30Sigmas;
            m_OverTrs_HistX = new List<double>();
            m_OverTrs_HistY = new List<double>();
            Count = S.m_Stats_OverTrs_HistX.Count;
            for (i = 0; i < Count; i++)
            {
                m_OverTrs_HistX.Add(S.m_Stats_OverTrs_HistX[i]);
                m_OverTrs_HistY.Add(S.m_Stats_OverTrs_HistY[i]);
            }

            m_nGSD_Min = S.m_nStats_GSD_Min;
            m_nGSD_Max = S.m_nStats_GSD_Max;
            m_nGSD_Avg = S.m_nStats_GSD_Avg;
            m_nGSD_Med = S.m_nStats_GSD_Med;
            m_nGSD_Std = S.m_nStats_GSD_Std;
            m_nGSD_Perc20Sigmas = S.m_nStats_GSD_Perc20Sigmas;
            m_nGSD_Perc25Sigmas = S.m_nStats_GSD_Perc25Sigmas;
            m_nGSD_Perc30Sigmas = S.m_nStats_GSD_Perc30Sigmas;
            m_GSD_HistX = new List<double>();
            m_GSD_HistY = new List<double>();
            Count = S.m_Stats_GSD_HistX.Count;
            for (i = 0; i < Count; i++)
            {
                m_GSD_HistX.Add(S.m_Stats_GSD_HistX[i]);
                m_GSD_HistY.Add(S.m_Stats_GSD_HistY[i]);
            }

            VarL.Enabled = true;
            VarCB.Enabled = true;
            CleanB.Enabled = true;
            SaveB.Enabled = true;
            StatsLV.Enabled = true;

            m_bIsFlight = false;

            m_bStatsExist = true;

            VarCB.SelectedIndex = -1;
            VarCB.SelectedIndex = 0;
        }

        /*=== SetStatsF() ===*/
        public void SetStatsF(String ImgsFile,
                              String PolyFile,
                              StatsF S)
        {
            int i, Count;

            m_sImgsFile = ImgsFile;
            m_sPolyFile = PolyFile;

            m_nOverLon_Min = S.m_nStats_OverLon_Min;
            m_nOverLon_Max = S.m_nStats_OverLon_Max;
            m_nOverLon_Avg = S.m_nStats_OverLon_Avg;
            m_nOverLon_Med = S.m_nStats_OverLon_Med;
            m_nOverLon_Std = S.m_nStats_OverLon_Std;
            m_nOverLon_Perc20Sigmas = S.m_nStats_OverLon_Perc20Sigmas;
            m_nOverLon_Perc25Sigmas = S.m_nStats_OverLon_Perc25Sigmas;
            m_nOverLon_Perc30Sigmas = S.m_nStats_OverLon_Perc30Sigmas;
            m_OverLon_HistX = new List<double>();
            m_OverLon_HistY = new List<double>();
            Count = S.m_Stats_OverLon_HistX.Count;
            for (i = 0; i < Count; i++)
            {
                m_OverLon_HistX.Add(S.m_Stats_OverLon_HistX[i]);
                m_OverLon_HistY.Add(S.m_Stats_OverLon_HistY[i]);
            }

            m_nOverTrs_Min = S.m_nStats_OverTrs_Min;
            m_nOverTrs_Max = S.m_nStats_OverTrs_Max;
            m_nOverTrs_Avg = S.m_nStats_OverTrs_Avg;
            m_nOverTrs_Med = S.m_nStats_OverTrs_Med;
            m_nOverTrs_Std = S.m_nStats_OverTrs_Std;
            m_nOverTrs_Perc20Sigmas = S.m_nStats_OverTrs_Perc20Sigmas;
            m_nOverTrs_Perc25Sigmas = S.m_nStats_OverTrs_Perc25Sigmas;
            m_nOverTrs_Perc30Sigmas = S.m_nStats_OverTrs_Perc30Sigmas;
            m_OverTrs_HistX = new List<double>();
            m_OverTrs_HistY = new List<double>();
            Count = S.m_Stats_OverTrs_HistX.Count;
            for (i = 0; i < Count; i++)
            {
                m_OverTrs_HistX.Add(S.m_Stats_OverTrs_HistX[i]);
                m_OverTrs_HistY.Add(S.m_Stats_OverTrs_HistY[i]);
            }

            m_nGSD_Min = S.m_nStats_GSD_Min;
            m_nGSD_Max = S.m_nStats_GSD_Max;
            m_nGSD_Avg = S.m_nStats_GSD_Avg;
            m_nGSD_Med = S.m_nStats_GSD_Med;
            m_nGSD_Std = S.m_nStats_GSD_Std;
            m_nGSD_Perc20Sigmas = S.m_nStats_GSD_Perc20Sigmas;
            m_nGSD_Perc25Sigmas = S.m_nStats_GSD_Perc25Sigmas;
            m_nGSD_Perc30Sigmas = S.m_nStats_GSD_Perc30Sigmas;
            m_GSD_HistX = new List<double>();
            m_GSD_HistY = new List<double>();
            Count = S.m_Stats_GSD_HistX.Count;
            for (i = 0; i < Count; i++)
            {
                m_GSD_HistX.Add(S.m_Stats_GSD_HistX[i]);
                m_GSD_HistY.Add(S.m_Stats_GSD_HistY[i]);
            }

            VarL.Enabled = true;
            VarCB.Enabled = true;
            CleanB.Enabled = true;
            SaveB.Enabled = true;
            StatsLV.Enabled = true;

            m_bIsFlight = true;

            m_bStatsExist = true;

            VarCB.SelectedIndex = -1;
            VarCB.SelectedIndex = 0;
        }

        /*=== ClearStats() ===*/
        public void ClearStats()
        {
            int i;

            m_bStatsExist = false;

            m_sImgsFile = "";
            m_sPolyFile = "";

            m_nOverLon_Min = 0.0;
            m_nOverLon_Max = 0.0;
            m_nOverLon_Avg = 0.0;
            m_nOverLon_Med = 0.0;
            m_nOverLon_Std = 0.0;
            m_nOverLon_Perc20Sigmas = 0.0;
            m_nOverLon_Perc25Sigmas = 0.0;
            m_nOverLon_Perc30Sigmas = 0.0;
            if (m_OverLon_HistX != null)
            {
                m_OverLon_HistX.Clear();
                m_OverLon_HistX = null;
            }
            if (m_OverLon_HistY != null)
            {
                m_OverLon_HistY.Clear();
                m_OverLon_HistY = null;
            }

            m_nOverTrs_Min = 0.0;
            m_nOverTrs_Max = 0.0;
            m_nOverTrs_Avg = 0.0;
            m_nOverTrs_Med = 0.0;
            m_nOverTrs_Std = 0.0;
            m_nOverTrs_Perc20Sigmas = 0.0;
            m_nOverTrs_Perc25Sigmas = 0.0;
            m_nOverTrs_Perc30Sigmas = 0.0;
            if (m_OverTrs_HistX != null)
            {
                m_OverTrs_HistX.Clear();
                m_OverTrs_HistX = null;
            }
            if (m_OverTrs_HistY != null)
            {
                m_OverTrs_HistY.Clear();
                m_OverTrs_HistY = null;
            }

            m_nGSD_Min = 0.0;
            m_nGSD_Max = 0.0;
            m_nGSD_Avg = 0.0;
            m_nGSD_Med = 0.0;
            m_nGSD_Std = 0.0;
            m_nGSD_Perc20Sigmas = 0.0;
            m_nGSD_Perc25Sigmas = 0.0;
            m_nGSD_Perc30Sigmas = 0.0;
            if (m_GSD_HistX != null)
            {
                m_GSD_HistX.Clear();
                m_GSD_HistX = null;
            }
            if (m_GSD_HistY != null)
            {
                m_GSD_HistY.Clear();
                m_GSD_HistY = null;
            }

            VarL.Enabled = false;
            VarCB.SelectedIndex = 0;
            VarCB.Enabled = false;
            CleanB.Enabled = false;
            SaveB.Enabled = false;
            for (i = 0; i < 8; i++)
                StatsLV.Items[i].SubItems[1].Text = "";
            StatsLV.Columns[0].Width = -2;
            StatsLV.Columns[1].Width = -2;
            for (i = 0; i < 8; i++)
            {
                StatsLV.Items[i].Selected = false;
                StatsLV.Items[i].Focused = false;
            }
            StatsLV.Items[0].Selected = true;
            StatsLV.Items[0].Focused = true;
            StatsLV.Enabled = false;
            Graph_Ctrl.CleanInfo();
            Graph_Ctrl.Enabled = false;
        }

        #endregion Public

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private -----------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private

        /*=== StatsCtrl_Resize() ===*/
        private void StatsCtrl_Resize(object sender, EventArgs e)
        {
            StatsLV.Width = this.Width - 17;
            Graph_Ctrl.Height = this.Height - StatsLV.Top - StatsLV.Height - 10;
        }

        /*=== VarCB_SelectedIndexChanged() ==*/
        private void VarCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i;

            if (!m_bStatsExist)
                return;

            if (VarCB.SelectedIndex == 0)
            {
                StatsLV.Items[0].SubItems[1].Text = String.Format("{0:0.00}", m_nOverLon_Min);
                StatsLV.Items[1].SubItems[1].Text = String.Format("{0:0.00}", m_nOverLon_Max);
                StatsLV.Items[2].SubItems[1].Text = String.Format("{0:0.00}", m_nOverLon_Avg);
                StatsLV.Items[3].SubItems[1].Text = String.Format("{0:0.00}", m_nOverLon_Std);
                StatsLV.Items[4].SubItems[1].Text = String.Format("{0:0.00}", m_nOverLon_Med);
                StatsLV.Items[5].SubItems[1].Text = String.Format("{0:0.00}", m_nOverLon_Perc20Sigmas);
                StatsLV.Items[6].SubItems[1].Text = String.Format("{0:0.00}", m_nOverLon_Perc25Sigmas);
                StatsLV.Items[7].SubItems[1].Text = String.Format("{0:0.00}", m_nOverLon_Perc30Sigmas);

                Graph_Ctrl.SetInfo(VarCB.Items[VarCB.SelectedIndex].ToString(),
                                   5,

                                   m_nOverLon_Min,
                                   m_nOverLon_Max,
                                   m_nOverLon_Avg,
                                   m_nOverLon_Med,
                                   m_nOverLon_Std,
                                   m_nOverLon_Perc20Sigmas,
                                   m_nOverLon_Perc25Sigmas,
                                   m_nOverLon_Perc30Sigmas,
                                   m_OverLon_HistX,
                                   m_OverLon_HistY);
            }

            else
            if (VarCB.SelectedIndex == 1)
            {
                StatsLV.Items[0].SubItems[1].Text = String.Format("{0:0.00}", m_nOverTrs_Min);
                StatsLV.Items[1].SubItems[1].Text = String.Format("{0:0.00}", m_nOverTrs_Max);
                StatsLV.Items[2].SubItems[1].Text = String.Format("{0:0.00}", m_nOverTrs_Avg);
                StatsLV.Items[3].SubItems[1].Text = String.Format("{0:0.00}", m_nOverTrs_Std);
                StatsLV.Items[4].SubItems[1].Text = String.Format("{0:0.00}", m_nOverTrs_Med);
                StatsLV.Items[5].SubItems[1].Text = String.Format("{0:0.00}", m_nOverTrs_Perc20Sigmas);
                StatsLV.Items[6].SubItems[1].Text = String.Format("{0:0.00}", m_nOverTrs_Perc25Sigmas);
                StatsLV.Items[7].SubItems[1].Text = String.Format("{0:0.00}", m_nOverTrs_Perc30Sigmas);

                Graph_Ctrl.SetInfo(VarCB.Items[VarCB.SelectedIndex].ToString(),
                                   5,

                                   m_nOverTrs_Min,
                                   m_nOverTrs_Max,
                                   m_nOverTrs_Avg,
                                   m_nOverTrs_Med,
                                   m_nOverTrs_Std,
                                   m_nOverTrs_Perc20Sigmas,
                                   m_nOverTrs_Perc25Sigmas,
                                   m_nOverTrs_Perc30Sigmas,
                                   m_OverTrs_HistX,
                                   m_OverTrs_HistY);
            }

            else
            if (VarCB.SelectedIndex == 2)
            {
                StatsLV.Items[0].SubItems[1].Text = String.Format("{0:0.00}", m_nGSD_Min);
                StatsLV.Items[1].SubItems[1].Text = String.Format("{0:0.00}", m_nGSD_Max);
                StatsLV.Items[2].SubItems[1].Text = String.Format("{0:0.00}", m_nGSD_Avg);
                StatsLV.Items[3].SubItems[1].Text = String.Format("{0:0.00}", m_nGSD_Std);
                StatsLV.Items[4].SubItems[1].Text = String.Format("{0:0.00}", m_nGSD_Med);
                StatsLV.Items[5].SubItems[1].Text = String.Format("{0:0.00}", m_nGSD_Perc20Sigmas);
                StatsLV.Items[6].SubItems[1].Text = String.Format("{0:0.00}", m_nGSD_Perc25Sigmas);
                StatsLV.Items[7].SubItems[1].Text = String.Format("{0:0.00}", m_nGSD_Perc30Sigmas);

                Graph_Ctrl.SetInfo(VarCB.Items[VarCB.SelectedIndex].ToString(),
                                   1,

                                   m_nGSD_Min,
                                   m_nGSD_Max,
                                   m_nGSD_Avg,
                                   m_nGSD_Med,
                                   m_nGSD_Std,
                                   m_nGSD_Perc20Sigmas,
                                   m_nGSD_Perc25Sigmas,
                                   m_nGSD_Perc30Sigmas,
                                   m_GSD_HistX,
                                   m_GSD_HistY);
            }

            StatsLV.Columns[0].Width = -2;
            StatsLV.Columns[1].Width = -2;
            for (i = 0; i < 8; i++)
            {
                StatsLV.Items[i].Selected = false;
                StatsLV.Items[i].Focused = false;
            }
            StatsLV.Items[0].Selected = true;
            StatsLV.Items[0].Focused = true;
        }

        /*=== CleanB_Click() ===*/
        private void CleanB_Click(object sender, EventArgs e)
        {
            if (!m_bStatsExist)
                return;

            ClearStats();
            if (OnStatsCleared != null)
                OnStatsCleared();
        }

        /*=== SaveB_Click() ===*/
        private void SaveB_Click(object sender, EventArgs e)
        {
            StreamWriter SW = null;
            int i, Count;

            if (!m_bStatsExist)
                return;

            if (SaveSD.ShowDialog() == DialogResult.OK)
            {
                this.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    SW = new StreamWriter(SaveSD.FileName);

                    SW.WriteLine("AerialPhotoQC Statistics File");

                    SW.WriteLine("");
                    SW.WriteLine("IsFlight\t" + m_bIsFlight.ToString());
                    SW.WriteLine("Footprints\t" + m_sImgsFile);
                    SW.WriteLine("Polygon\t" + m_sPolyFile);
                    SW.WriteLine("");
                    SW.WriteLine("Longitudinal Overlap, %");
                    SW.WriteLine("");
                    SW.WriteLine(String.Format("Min\t{0:0.00}", m_nOverLon_Min));
                    SW.WriteLine(String.Format("Max\t{0:0.00}", m_nOverLon_Max));
                    SW.WriteLine(String.Format("Avg\t{0:0.00}", m_nOverLon_Avg));
                    SW.WriteLine(String.Format("Std\t{0:0.00}", m_nOverLon_Std));
                    SW.WriteLine(String.Format("Med\t{0:0.00}", m_nOverLon_Med));
                    SW.WriteLine(String.Format("20S\t{0:0.00}", m_nOverLon_Perc20Sigmas));
                    SW.WriteLine(String.Format("25S\t{0:0.00}", m_nOverLon_Perc25Sigmas));
                    SW.WriteLine(String.Format("30S\t{0:0.00}", m_nOverLon_Perc30Sigmas));
                    SW.WriteLine("");
                    SW.WriteLine("Histogram");
                    Count = m_OverLon_HistX.Count;
                    SW.WriteLine("Count\t" + Count.ToString());
                    SW.WriteLine("HistX\tHistY");
                    for (i = 0; i < Count; i++)
                        SW.WriteLine(String.Format("{0:0.000}\t{1:0.000000}", m_OverLon_HistX[i], m_OverLon_HistY[i]));

                    SW.WriteLine("");
                    SW.WriteLine("Transversal Overlap, %");
                    SW.WriteLine("");
                    SW.WriteLine(String.Format("Min\t{0:0.00}", m_nOverTrs_Min));
                    SW.WriteLine(String.Format("Max\t{0:0.00}", m_nOverTrs_Max));
                    SW.WriteLine(String.Format("Avg\t{0:0.00}", m_nOverTrs_Avg));
                    SW.WriteLine(String.Format("Std\t{0:0.00}", m_nOverTrs_Std));
                    SW.WriteLine(String.Format("Med\t{0:0.00}", m_nOverTrs_Med));
                    SW.WriteLine(String.Format("20S\t{0:0.00}", m_nOverTrs_Perc20Sigmas));
                    SW.WriteLine(String.Format("25S\t{0:0.00}", m_nOverTrs_Perc25Sigmas));
                    SW.WriteLine(String.Format("30S\t{0:0.00}", m_nOverTrs_Perc30Sigmas));
                    SW.WriteLine("");
                    SW.WriteLine("Histogram");
                    Count = m_OverTrs_HistX.Count;
                    SW.WriteLine("Count\t" + Count.ToString());
                    SW.WriteLine("HistX\tHistY");
                    for (i = 0; i < Count; i++)
                        SW.WriteLine(String.Format("{0:0.000}\t{1:0.000000}", m_OverTrs_HistX[i], m_OverTrs_HistY[i]));

                    SW.WriteLine("");
                    SW.WriteLine("GSD, cm");
                    SW.WriteLine("");
                    SW.WriteLine(String.Format("Min\t{0:0.00}", m_nGSD_Min));
                    SW.WriteLine(String.Format("Max\t{0:0.00}", m_nGSD_Max));
                    SW.WriteLine(String.Format("Avg\t{0:0.00}", m_nGSD_Avg));
                    SW.WriteLine(String.Format("Std\t{0:0.00}", m_nGSD_Std));
                    SW.WriteLine(String.Format("Med\t{0:0.00}", m_nGSD_Med));
                    SW.WriteLine(String.Format("20S\t{0:0.00}", m_nGSD_Perc20Sigmas));
                    SW.WriteLine(String.Format("25S\t{0:0.00}", m_nGSD_Perc25Sigmas));
                    SW.WriteLine(String.Format("30S\t{0:0.00}", m_nGSD_Perc30Sigmas));
                    SW.WriteLine("");
                    SW.WriteLine("Histogram");
                    Count = m_GSD_HistX.Count;
                    SW.WriteLine("Count\t" + Count.ToString());
                    SW.WriteLine("HistX\tHistY");
                    for (i = 0; i < Count; i++)
                        SW.WriteLine(String.Format("{0:0.000}\t{1:0.000000}", m_GSD_HistX[i], m_GSD_HistY[i]));

                    SW.Close();
                    SW.Dispose();
                    SW = null;
                }
                catch (Exception ex)
                {
                    if (SW != null)
                    {
                        SW.Close();
                        SW.Dispose();
                        SW = null;
                    }

                    this.Enabled = true;
                    this.Cursor = Cursors.Default;

                    MessageBox.Show("Error Writing File " + SaveSD.FileName + ". Exception:\n" + ex.Message,
                                    "ERROR",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
                this.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        /*=== LoadB_Click() ===*/
        private void LoadB_Click(object sender, EventArgs e)
        {
            StreamReader SR = null;
            int i, Count;
            String S;
            string[] s;
            char[] Sep = new char[] { '\t' };

            double OverLon_Min;
            double OverLon_Max;
            double OverLon_Avg;
            double OverLon_Med;
            double OverLon_Std;
            double OverLon_Perc20Sigmas;
            double OverLon_Perc25Sigmas;
            double OverLon_Perc30Sigmas;
            List<double> OverLon_HistX = null;
            List<double> OverLon_HistY = null;

            double OverTrs_Min;
            double OverTrs_Max;
            double OverTrs_Avg;
            double OverTrs_Med;
            double OverTrs_Std;
            double OverTrs_Perc20Sigmas;
            double OverTrs_Perc25Sigmas;
            double OverTrs_Perc30Sigmas;
            List<double> OverTrs_HistX = null;
            List<double> OverTrs_HistY = null;

            double GSD_Min;
            double GSD_Max;
            double GSD_Avg;
            double GSD_Med;
            double GSD_Std;
            double GSD_Perc20Sigmas;
            double GSD_Perc25Sigmas;
            double GSD_Perc30Sigmas;
            List<double> GSD_HistX = null;
            List<double> GSD_HistY = null;

            Stats St = null;
            StatsF StF = null;
            double x, y;

            if (LoadOD.ShowDialog() == DialogResult.OK)
            {
                ClearStats();
                Application.DoEvents();

                this.Enabled = false;
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    SR = new StreamReader(LoadOD.FileName);

                    S = SR.ReadLine();
                    if (S != "AerialPhotoQC Statistics File")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }

                    // IsFlight, Footprints & DTM
                    /////////////////////////////////////////////////////////////////////
                    S = SR.ReadLine();
                    if (S != "")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "IsFlight")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!bool.TryParse(s[1], out m_bIsFlight))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Footprints")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    m_sImgsFile = s[1];
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s[0] != "Polygon")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s.Length == 2)
                        m_sPolyFile = s[1];
                    else
                        m_sPolyFile = "";

                    // OverLon
                    /////////////////////////////////////////////////////////////////////
                    S = SR.ReadLine();
                    if (S != "")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "Longitudinal Overlap, %")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Min")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverLon_Min))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Max")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverLon_Max))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Avg")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverLon_Avg))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Std")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverLon_Std))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Med")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverLon_Med))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "20S")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverLon_Perc20Sigmas))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "25S")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverLon_Perc25Sigmas))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "30S")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverLon_Perc30Sigmas))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "Histogram")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Count")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!int.TryParse(s[1], out Count))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "HistX	HistY")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    OverLon_HistX = new List<double>();
                    OverLon_HistY = new List<double>();
                    for (i = 0; i < Count; i++)
                    {
                        S = SR.ReadLine();
                        s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                        if (s.Length != 2)
                        {
                            this.Enabled = true;
                            this.Cursor = Cursors.Default;
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            MessageBox.Show("Invalid Statistics File.",
                                            "ERROR",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        if (!double.TryParse(s[0], out x))
                        {
                            this.Enabled = true;
                            this.Cursor = Cursors.Default;
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            MessageBox.Show("Invalid Statistics File.",
                                            "ERROR",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        if (!double.TryParse(s[1], out y))
                        {
                            this.Enabled = true;
                            this.Cursor = Cursors.Default;
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            MessageBox.Show("Invalid Statistics File.",
                                            "ERROR",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        OverLon_HistX.Add(x);
                        OverLon_HistY.Add(y);
                    }

                    // OverTrs
                    /////////////////////////////////////////////////////////////////////
                    S = SR.ReadLine();
                    if (S != "")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "Transversal Overlap, %")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Min")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverTrs_Min))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Max")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverTrs_Max))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Avg")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverTrs_Avg))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Std")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverTrs_Std))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Med")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverTrs_Med))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "20S")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverTrs_Perc20Sigmas))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "25S")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverTrs_Perc25Sigmas))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "30S")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out OverTrs_Perc30Sigmas))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "Histogram")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Count")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!int.TryParse(s[1], out Count))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "HistX	HistY")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    OverTrs_HistX = new List<double>();
                    OverTrs_HistY = new List<double>();
                    for (i = 0; i < Count; i++)
                    {
                        S = SR.ReadLine();
                        s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                        if (s.Length != 2)
                        {
                            this.Enabled = true;
                            this.Cursor = Cursors.Default;
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            MessageBox.Show("Invalid Statistics File.",
                                            "ERROR",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        if (!double.TryParse(s[0], out x))
                        {
                            this.Enabled = true;
                            this.Cursor = Cursors.Default;
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            MessageBox.Show("Invalid Statistics File.",
                                            "ERROR",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        if (!double.TryParse(s[1], out y))
                        {
                            this.Enabled = true;
                            this.Cursor = Cursors.Default;
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            MessageBox.Show("Invalid Statistics File.",
                                            "ERROR",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        OverTrs_HistX.Add(x);
                        OverTrs_HistY.Add(y);
                    }

                    // GSD
                    /////////////////////////////////////////////////////////////////////
                    S = SR.ReadLine();
                    if (S != "")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "GSD, cm")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Min")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out GSD_Min))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Max")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out GSD_Max))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Avg")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out GSD_Avg))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Std")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out GSD_Std))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Med")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out GSD_Med))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "20S")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out GSD_Perc20Sigmas))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "25S")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out GSD_Perc25Sigmas))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "30S")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!double.TryParse(s[1], out GSD_Perc30Sigmas))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "Histogram")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                    if (s.Length != 2)
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (s[0] != "Count")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    if (!int.TryParse(s[1], out Count))
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    S = SR.ReadLine();
                    if (S != "HistX	HistY")
                    {
                        this.Enabled = true;
                        this.Cursor = Cursors.Default;
                        SR.Close();
                        SR.Dispose();
                        SR = null;
                        MessageBox.Show("Invalid Statistics File.",
                                        "ERROR",
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error);
                        return;
                    }
                    GSD_HistX = new List<double>();
                    GSD_HistY = new List<double>();
                    for (i = 0; i < Count; i++)
                    {
                        S = SR.ReadLine();
                        s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                        if (s.Length != 2)
                        {
                            this.Enabled = true;
                            this.Cursor = Cursors.Default;
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            MessageBox.Show("Invalid Statistics File.",
                                            "ERROR",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        if (!double.TryParse(s[0], out x))
                        {
                            this.Enabled = true;
                            this.Cursor = Cursors.Default;
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            MessageBox.Show("Invalid Statistics File.",
                                            "ERROR",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        if (!double.TryParse(s[1], out y))
                        {
                            this.Enabled = true;
                            this.Cursor = Cursors.Default;
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            MessageBox.Show("Invalid Statistics File.",
                                            "ERROR",
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Error);
                            return;
                        }
                        GSD_HistX.Add(x);
                        GSD_HistY.Add(y);
                    }

                    SR.Close();
                    SR.Dispose();
                    SR = null;

                    if (!m_bIsFlight)
                    {
                        St = new Stats();

                        St.m_nStats_OverLon_Min = OverLon_Min;
                        St.m_nStats_OverLon_Max = OverLon_Max;
                        St.m_nStats_OverLon_Avg = OverLon_Avg;
                        St.m_nStats_OverLon_Std = OverLon_Std;
                        St.m_nStats_OverLon_Med = OverLon_Med;
                        St.m_nStats_OverLon_Perc20Sigmas = OverLon_Perc20Sigmas;
                        St.m_nStats_OverLon_Perc25Sigmas = OverLon_Perc25Sigmas;
                        St.m_nStats_OverLon_Perc30Sigmas = OverLon_Perc30Sigmas;
                        St.m_Stats_OverLon_HistX = OverLon_HistX;
                        St.m_Stats_OverLon_HistY = OverLon_HistY;

                        St.m_nStats_OverTrs_Min = OverTrs_Min;
                        St.m_nStats_OverTrs_Max = OverTrs_Max;
                        St.m_nStats_OverTrs_Avg = OverTrs_Avg;
                        St.m_nStats_OverTrs_Std = OverTrs_Std;
                        St.m_nStats_OverTrs_Med = OverTrs_Med;
                        St.m_nStats_OverTrs_Perc20Sigmas = OverTrs_Perc20Sigmas;
                        St.m_nStats_OverTrs_Perc25Sigmas = OverTrs_Perc25Sigmas;
                        St.m_nStats_OverTrs_Perc30Sigmas = OverTrs_Perc30Sigmas;
                        St.m_Stats_OverTrs_HistX = OverTrs_HistX;
                        St.m_Stats_OverTrs_HistY = OverTrs_HistY;

                        St.m_nStats_GSD_Min = GSD_Min;
                        St.m_nStats_GSD_Max = GSD_Max;
                        St.m_nStats_GSD_Avg = GSD_Avg;
                        St.m_nStats_GSD_Std = GSD_Std;
                        St.m_nStats_GSD_Med = GSD_Med;
                        St.m_nStats_GSD_Perc20Sigmas = GSD_Perc20Sigmas;
                        St.m_nStats_GSD_Perc25Sigmas = GSD_Perc25Sigmas;
                        St.m_nStats_GSD_Perc30Sigmas = GSD_Perc30Sigmas;
                        St.m_Stats_GSD_HistX = GSD_HistX;
                        St.m_Stats_GSD_HistY = GSD_HistY;

                        SetStats(m_sImgsFile,
                                 m_sPolyFile,
                                 St);
                        Application.DoEvents();
                        St.Close();
                        St = null;
                    }
                    else
                    {
                        StF = new StatsF();

                        StF.m_nStats_OverLon_Min = OverLon_Min;
                        StF.m_nStats_OverLon_Max = OverLon_Max;
                        StF.m_nStats_OverLon_Avg = OverLon_Avg;
                        StF.m_nStats_OverLon_Std = OverLon_Std;
                        StF.m_nStats_OverLon_Med = OverLon_Med;
                        StF.m_nStats_OverLon_Perc20Sigmas = OverLon_Perc20Sigmas;
                        StF.m_nStats_OverLon_Perc25Sigmas = OverLon_Perc25Sigmas;
                        StF.m_nStats_OverLon_Perc30Sigmas = OverLon_Perc30Sigmas;
                        StF.m_Stats_OverLon_HistX = OverLon_HistX;
                        StF.m_Stats_OverLon_HistY = OverLon_HistY;

                        StF.m_nStats_OverTrs_Min = OverTrs_Min;
                        StF.m_nStats_OverTrs_Max = OverTrs_Max;
                        StF.m_nStats_OverTrs_Avg = OverTrs_Avg;
                        StF.m_nStats_OverTrs_Std = OverTrs_Std;
                        StF.m_nStats_OverTrs_Med = OverTrs_Med;
                        StF.m_nStats_OverTrs_Perc20Sigmas = OverTrs_Perc20Sigmas;
                        StF.m_nStats_OverTrs_Perc25Sigmas = OverTrs_Perc25Sigmas;
                        StF.m_nStats_OverTrs_Perc30Sigmas = OverTrs_Perc30Sigmas;
                        StF.m_Stats_OverTrs_HistX = OverTrs_HistX;
                        StF.m_Stats_OverTrs_HistY = OverTrs_HistY;

                        StF.m_nStats_GSD_Min = GSD_Min;
                        StF.m_nStats_GSD_Max = GSD_Max;
                        StF.m_nStats_GSD_Avg = GSD_Avg;
                        StF.m_nStats_GSD_Std = GSD_Std;
                        StF.m_nStats_GSD_Med = GSD_Med;
                        StF.m_nStats_GSD_Perc20Sigmas = GSD_Perc20Sigmas;
                        StF.m_nStats_GSD_Perc25Sigmas = GSD_Perc25Sigmas;
                        StF.m_nStats_GSD_Perc30Sigmas = GSD_Perc30Sigmas;
                        StF.m_Stats_GSD_HistX = GSD_HistX;
                        StF.m_Stats_GSD_HistY = GSD_HistY;

                        SetStatsF(m_sImgsFile,
                                  m_sPolyFile,
                                  StF);
                        Application.DoEvents();
                        StF.Close();
                        StF = null;
                    }
                }
                catch (Exception ex)
                {
                    if (OverLon_HistX != null)
                    {
                        OverLon_HistX.Clear();
                        OverLon_HistX = null;
                    }
                    if (OverLon_HistY != null)
                    {
                        OverLon_HistY.Clear();
                        OverLon_HistY = null;
                    }

                    if (OverTrs_HistX != null)
                    {
                        OverTrs_HistX.Clear();
                        OverTrs_HistX = null;
                    }
                    if (OverTrs_HistY != null)
                    {
                        OverTrs_HistY.Clear();
                        OverTrs_HistY = null;
                    }

                    if (GSD_HistX != null)
                    {
                        GSD_HistX.Clear();
                        GSD_HistX = null;
                    }
                    if (GSD_HistY != null)
                    {
                        GSD_HistY.Clear();
                        GSD_HistY = null;
                    }

                    St = null;

                    ClearStats();
                    Application.DoEvents();

                    this.Enabled = true;
                    this.Cursor = Cursors.Default;

                    MessageBox.Show("Error Reading File " + LoadOD.FileName + ". Exception:\n" + ex.Message,
                                    "ERROR",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    return;
                }
                this.Enabled = true;
                this.Cursor = Cursors.Default;

                if (OnStatsLoaded != null)
                    OnStatsLoaded(m_bIsFlight,

                                  m_sImgsFile,
                                  m_sPolyFile,

                                  m_nOverLon_Avg,
                                  m_nOverLon_Std,

                                  m_nOverTrs_Avg,
                                  m_nOverTrs_Std,

                                  m_nGSD_Avg,
                                  m_nGSD_Std);
            }
        }

        #endregion Private
    }
}
