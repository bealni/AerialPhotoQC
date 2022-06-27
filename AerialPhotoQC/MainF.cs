using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace AerialPhotoQC
{
    public partial class MainF : Form
    {
        private const String CFG_NAME = "Cfg.json";

        private String m_sExePath;

        private Cfg m_Cfg;

        /*=== MainF() ===*/
        public MainF()
        {
            InitializeComponent();

            m_sExePath = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);

            m_Cfg = new Cfg();
            if (!File.Exists(m_sExePath + "\\" + CFG_NAME))
                m_Cfg.Open(m_sExePath + "\\" + CFG_NAME, true);
            else
                m_Cfg.Open(m_sExePath + "\\" + CFG_NAME, false);

            Input_Ctrl.Open(m_Cfg);
            Input_Ctrl.OnBeforeStarted += new InputCtrl.OnBeforeStartedHandler(OnInputCtrlBeforeStarted);
            Input_Ctrl.OnFinished += new InputCtrl.OnFinishedHandler(OnInputCtrlFinished);

            InputF_Ctrl.Open(m_Cfg);
            InputF_Ctrl.OnBeforeStartedF += new InputFCtrl.OnBeforeStartedFHandler(OnInputCtrlBeforeStartedF);
            InputF_Ctrl.OnFinishedF += new InputFCtrl.OnFinishedFHandler(OnInputCtrlFinishedF);

            Stats_Ctrl.OnStatsLoaded += new StatsCtrl.OnStatsLoadedHandler(OnStatsCtrlLoaded);
            Stats_Ctrl.OnStatsCleared += new StatsCtrl.OnStatsClearedHandler(OnStatsCtrlCleared);

            ObliqueSim_Ctrl.Open(m_Cfg);
        }

        /*=== OnInputCtrlBeforeStarted() ===*/
        private void OnInputCtrlBeforeStarted()
        {
            Stats_Ctrl.ClearStats();
            Viewer_Ctrl.ClearStats();
        }

        /*=== OnInputCtrlFinished() ===*/
        private void OnInputCtrlFinished(bool IsObliqueStats,
                                         String sImgsFile,
                                         String sPolyFile,
                                         Stats S)
        {
            Stats_Ctrl.SetStats(IsObliqueStats,
                                sImgsFile,
                                sPolyFile,
                                S);
            Viewer_Ctrl.SetStats(false,
                                 IsObliqueStats,

                                 sImgsFile,
                                 sPolyFile,

                                 S.m_nStats_OverLon_Avg,
                                 S.m_nStats_OverLon_Std,

                                 S.m_nStats_OverTrs_Avg,
                                 S.m_nStats_OverTrs_Std,

                                 S.m_nStats_GSD_Avg,
                                 S.m_nStats_GSD_Std,
                                 
                                 S.m_nStats_OverOblF_Avg,
                                 S.m_nStats_OverOblF_Std,

                                 S.m_nStats_OverOblL_Avg,
                                 S.m_nStats_OverOblL_Std);
        }

        /*=== OnInputCtrlBeforeStartedF() ===*/
        private void OnInputCtrlBeforeStartedF()
        {
            Stats_Ctrl.ClearStats();
            Viewer_Ctrl.ClearStats();
        }

        /*=== OnInputCtrlFinishedF() ===*/
        private void OnInputCtrlFinishedF(bool IsObliqueStats,
                                          String sImgsFile,
                                          String sPolyFile,
                                          StatsF S)
        {
            Stats_Ctrl.SetStatsF(IsObliqueStats,
                                 sImgsFile,
                                 sPolyFile,
                                 S);
            Viewer_Ctrl.SetStats(true,
                                 IsObliqueStats,

                                 sImgsFile,
                                 sPolyFile,

                                 S.m_nStats_OverLon_Avg,
                                 S.m_nStats_OverLon_Std,

                                 S.m_nStats_OverTrs_Avg,
                                 S.m_nStats_OverTrs_Std,

                                 S.m_nStats_GSD_Avg,
                                 S.m_nStats_GSD_Std,
                                 
                                 S.m_nStats_OverOblF_Avg,
                                 S.m_nStats_OverOblF_Std,

                                 S.m_nStats_OverOblL_Avg,
                                 S.m_nStats_OverOblL_Std);
        }

        /*=== OnStatsCtrlCleared() ===*/
        private void OnStatsCtrlCleared()
        {
            Viewer_Ctrl.ClearStats();
        }

        /*=== OnStatsCtrlLoaded() ===*/
        private void OnStatsCtrlLoaded(bool IsFlight,
                                       bool IsObliqueStats,

                                       String ImgsFile,
                                       String PolyFile,

                                       double OverLon_Avg,
                                       double OverLon_Std,

                                       double OverTrs_Avg,
                                       double OverTrs_Std,

                                       double GSD_Avg,
                                       double GSD_Std,
            
                                       double OverOblF_Avg,
                                       double OverOblF_Std,

                                       double OverOblL_Avg,
                                       double OverOblL_Std)
        {
            Viewer_Ctrl.SetStats(IsFlight,
                                 IsObliqueStats,

                                 ImgsFile,
                                 PolyFile,

                                 OverLon_Avg,
                                 OverLon_Std,

                                 OverTrs_Avg,
                                 OverTrs_Std,

                                 GSD_Avg,
                                 GSD_Std,
                                 
                                 OverOblF_Avg,
                                 OverOblF_Std,

                                 OverOblL_Avg,
                                 OverOblL_Std);
        }
    }
}
