using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AerialPhotoQC
{
    public partial class MainF : Form
    {
        /*=== MainF() ===*/
        public MainF()
        {
            InitializeComponent();

            Input_Ctrl.OnBeforeStarted += new InputCtrl.OnBeforeStartedHandler(OnInputCtrlBeforeStarted);
            Input_Ctrl.OnFinished += new InputCtrl.OnFinishedHandler(OnInputCtrlFinished);

            InputF_Ctrl.OnBeforeStartedF += new InputFCtrl.OnBeforeStartedFHandler(OnInputCtrlBeforeStartedF);
            InputF_Ctrl.OnFinishedF += new InputFCtrl.OnFinishedFHandler(OnInputCtrlFinishedF);

            Stats_Ctrl.OnStatsLoaded += new StatsCtrl.OnStatsLoadedHandler(OnStatsCtrlLoaded);
            Stats_Ctrl.OnStatsCleared += new StatsCtrl.OnStatsClearedHandler(OnStatsCtrlCleared);
        }

        /*=== OnInputCtrlBeforeStarted() ===*/
        private void OnInputCtrlBeforeStarted()
        {
            Stats_Ctrl.ClearStats();
            Viewer_Ctrl.ClearStats();
        }

        /*=== OnInputCtrlFinished() ===*/
        private void OnInputCtrlFinished(String sImgsFile,
                                         String sPolyFile,
                                         Stats S)
        {
            Stats_Ctrl.SetStats(sImgsFile,
                                sPolyFile,
                                S);
            Viewer_Ctrl.SetStats(false,
                                 sImgsFile,
                                 sPolyFile,

                                 S.m_nStats_OverLon_Avg,
                                 S.m_nStats_OverLon_Std,

                                 S.m_nStats_OverTrs_Avg,
                                 S.m_nStats_OverTrs_Std,

                                 S.m_nStats_GSD_Avg,
                                 S.m_nStats_GSD_Std);
        }

        /*=== OnInputCtrlBeforeStartedF() ===*/
        private void OnInputCtrlBeforeStartedF()
        {
            Stats_Ctrl.ClearStats();
            Viewer_Ctrl.ClearStats();
        }

        /*=== OnInputCtrlFinishedF() ===*/
        private void OnInputCtrlFinishedF(String sImgsFile,
                                          String sPolyFile,
                                          StatsF S)
        {
            Stats_Ctrl.SetStatsF(sImgsFile,
                                 sPolyFile,
                                 S);
            Viewer_Ctrl.SetStats(true,
                                 sImgsFile,
                                 sPolyFile,

                                 S.m_nStats_OverLon_Avg,
                                 S.m_nStats_OverLon_Std,

                                 S.m_nStats_OverTrs_Avg,
                                 S.m_nStats_OverTrs_Std,

                                 S.m_nStats_GSD_Avg,
                                 S.m_nStats_GSD_Std);
        }

        /*=== OnStatsCtrlCleared() ===*/
        private void OnStatsCtrlCleared()
        {
            Viewer_Ctrl.ClearStats();
        }

        /*=== OnStatsCtrlLoaded() ===*/
        private void OnStatsCtrlLoaded(bool IsFlight,

                                       String ImgsFile,
                                       String PolyFile,

                                       double OverLon_Avg,
                                       double OverLon_Std,

                                       double OverTrs_Avg,
                                       double OverTrs_Std,

                                       double GSD_Avg,
                                       double GSD_Std)
        {
            Viewer_Ctrl.SetStats(IsFlight,
                                 ImgsFile,
                                 PolyFile,

                                 OverLon_Avg,
                                 OverLon_Std,

                                 OverTrs_Avg,
                                 OverTrs_Std,

                                 GSD_Avg,
                                 GSD_Std);
        }
    }
}
