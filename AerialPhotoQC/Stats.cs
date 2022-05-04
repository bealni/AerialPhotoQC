using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ClipperPath = System.Collections.Generic.List<Clipper.IntPoint>;
using ClipperPaths = System.Collections.Generic.List<System.Collections.Generic.List<Clipper.IntPoint>>;

namespace AerialPhotoQC
{
    public class Stats
    {
        private const double Z_COMPARE_DELTA = 1.0;
        private const int MAX_ITER = 5;

        private ShpInfo m_ShpInfo;
        private DTMInfo m_DTMInfo;
        private MonoModel m_MonoModel;
        private int m_nGSDGridSize;

        private InputCtrl m_Ctrl;
        private System.Windows.Forms.Label m_Lbl;

        private double m_nSensorArea;

        public double m_nStats_OverLon_Min;
        public double m_nStats_OverLon_Max;
        public double m_nStats_OverLon_Avg;
        public double m_nStats_OverLon_Med;
        public double m_nStats_OverLon_Std;
        public double m_nStats_OverLon_Perc20Sigmas;
        public double m_nStats_OverLon_Perc25Sigmas;
        public double m_nStats_OverLon_Perc30Sigmas;
        public List<double> m_Stats_OverLon_HistX;
        public List<double> m_Stats_OverLon_HistY;

        public double m_nStats_OverTrs_Min;
        public double m_nStats_OverTrs_Max;
        public double m_nStats_OverTrs_Avg;
        public double m_nStats_OverTrs_Med;
        public double m_nStats_OverTrs_Std;
        public double m_nStats_OverTrs_Perc20Sigmas;
        public double m_nStats_OverTrs_Perc25Sigmas;
        public double m_nStats_OverTrs_Perc30Sigmas;
        public List<double> m_Stats_OverTrs_HistX;
        public List<double> m_Stats_OverTrs_HistY;

        public double m_nStats_GSD_Min;
        public double m_nStats_GSD_Max;
        public double m_nStats_GSD_Avg;
        public double m_nStats_GSD_Med;
        public double m_nStats_GSD_Std;
        public double m_nStats_GSD_Perc20Sigmas;
        public double m_nStats_GSD_Perc25Sigmas;
        public double m_nStats_GSD_Perc30Sigmas;
        public List<double> m_Stats_GSD_HistX;
        public List<double> m_Stats_GSD_HistY;

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Public ------------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Public

        /*=== Stats() ===*/
        public Stats()
        {
            DoClose();
        }

        /*=== Open() ===*/
        public String Open(ShpInfo Shp_Info,
                           DTMInfo DTM_Info,
                           MonoModel Mono_Model,
                           int GSDGridSize,

                           InputCtrl Ctrl_,
                           System.Windows.Forms.Label Lbl_)
        {
            m_ShpInfo = Shp_Info;
            m_DTMInfo = DTM_Info;
            m_MonoModel = Mono_Model;
            m_nGSDGridSize = GSDGridSize;

            m_Ctrl = Ctrl_;
            m_Lbl = Lbl_;

            m_nSensorArea = (double)(m_MonoModel.m_nCamCols) * (double)(m_MonoModel.m_nCamRows);

            return DoOpen();
        }

        /*=== Close() ===*/
        public void Close()
        {
            DoClose();
        }

        #endregion Public

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private -----------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Open/Close ------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Open/Close

        /*=== DoOpen() ===*/
        private String DoOpen()
        {
            return Process();
        }

        /*=== DoClose() ===*/
        private void DoClose()
        {
            m_ShpInfo = null;
            m_DTMInfo = null;
            m_MonoModel = null;
            m_nGSDGridSize = 0;

            m_Ctrl = null;
            m_Lbl = null;

            m_nSensorArea = 0.0;

            m_nStats_OverLon_Min = 0.0;
            m_nStats_OverLon_Max = 0.0;
            m_nStats_OverLon_Avg = 0.0;
            m_nStats_OverLon_Med = 0.0;
            m_nStats_OverLon_Std = 0.0;
            m_nStats_OverLon_Perc20Sigmas = 0.0;
            m_nStats_OverLon_Perc25Sigmas = 0.0;
            m_nStats_OverLon_Perc30Sigmas = 0.0;
            if (m_Stats_OverLon_HistX != null)
            {
                m_Stats_OverLon_HistX.Clear();
                m_Stats_OverLon_HistX = null;
            }
            if (m_Stats_OverLon_HistY != null)
            {
                m_Stats_OverLon_HistY.Clear();
                m_Stats_OverLon_HistY = null;
            }

            m_nStats_OverTrs_Min = 0.0;
            m_nStats_OverTrs_Max = 0.0;
            m_nStats_OverTrs_Avg = 0.0;
            m_nStats_OverTrs_Med = 0.0;
            m_nStats_OverTrs_Std = 0.0;
            m_nStats_OverTrs_Perc20Sigmas = 0.0;
            m_nStats_OverTrs_Perc25Sigmas = 0.0;
            m_nStats_OverTrs_Perc30Sigmas = 0.0;
            if (m_Stats_OverTrs_HistX != null)
            {
                m_Stats_OverTrs_HistX.Clear();
                m_Stats_OverTrs_HistX = null;
            }
            if (m_Stats_OverTrs_HistY != null)
            {
                m_Stats_OverTrs_HistY.Clear();
                m_Stats_OverTrs_HistY = null;
            }

            m_nStats_GSD_Min = 0.0;
            m_nStats_GSD_Max = 0.0;
            m_nStats_GSD_Avg = 0.0;
            m_nStats_GSD_Med = 0.0;
            m_nStats_GSD_Std = 0.0;
            m_nStats_GSD_Perc20Sigmas = 0.0;
            m_nStats_GSD_Perc25Sigmas = 0.0;
            m_nStats_GSD_Perc30Sigmas = 0.0;
            if (m_Stats_GSD_HistX != null)
            {
                m_Stats_GSD_HistX.Clear();
                m_Stats_GSD_HistX = null;
            }
            if (m_Stats_GSD_HistY != null)
            {
                m_Stats_GSD_HistY.Clear();
                m_Stats_GSD_HistY = null;
            }
        }

        #endregion Private Open/Close

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Process ---------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Process

        /*=== Process() ===*/
        private String Process()
        {
            String Res;

            Res = DoProcess_CalcImgs();
            if (Res != "OK")
                return Res;

            Res = DoProcess_CalcOverlaps();
            if (Res != "OK")
                return Res;

            Res = DoProcess_CalcGSDs();
            if (Res != "OK")
                return Res;

            Res = GetFinalStats();
            if (Res != "OK")
                return Res;

            return "OK";
        }

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Calc Images -----------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Calc Images

        /*=== DoProcess_CalcImgs() ===*/
        private String DoProcess_CalcImgs()
        {
            int LinCount, LinIdx;
            String Res;

            LinCount = m_ShpInfo.m_PntLinIdxs.Count;
            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                Res = DoProcess_CalcImgs_Lin(LinIdx);
                if (Res != "OK")
                    return Res;
            }

            return "OK";
        }

        /*=== DoProcess_CalcImgs_Lin() ===*/
        private String DoProcess_CalcImgs_Lin(int LinIdx)
        {
            int PntCount, PntIdx, i;
            double x, y, z;
            double MinX, MinY, Area;
            ClipperPath Poly;

            PntCount = m_ShpInfo.m_PntXs[LinIdx].Count;
            for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
            {
                if (PntIdx == 0)
                    m_MonoModel.SetImgParams(m_ShpInfo.m_PntXs[LinIdx][PntIdx],
                                             m_ShpInfo.m_PntYs[LinIdx][PntIdx],
                                             m_ShpInfo.m_PntZs[LinIdx][PntIdx],

                                             m_ShpInfo.m_LinKappas[m_ShpInfo.m_PntLinIdxs[LinIdx]]);
                else
                    m_MonoModel.SetImgPos(m_ShpInfo.m_PntXs[LinIdx][PntIdx],
                                          m_ShpInfo.m_PntYs[LinIdx][PntIdx],
                                          m_ShpInfo.m_PntZs[LinIdx][PntIdx]);

                MinX = double.MaxValue;
                MinY = double.MaxValue;

                DoProject(0.0,
                          0.0,
                          out x,
                          out y,
                          out z);
                m_ShpInfo.m_PntImgXs[LinIdx][PntIdx][0] = x;
                m_ShpInfo.m_PntImgYs[LinIdx][PntIdx][0] = y;
                m_ShpInfo.m_PntImgZs[LinIdx][PntIdx][0] = z;
                if (x < MinX)
                    MinX = x;
                if (y < MinY)
                    MinY = y;

                DoProject(0.0,
                          (double)(m_MonoModel.m_nCamRows),
                          out x,
                          out y,
                          out z);
                m_ShpInfo.m_PntImgXs[LinIdx][PntIdx][1] = x;
                m_ShpInfo.m_PntImgYs[LinIdx][PntIdx][1] = y;
                m_ShpInfo.m_PntImgZs[LinIdx][PntIdx][1] = z;
                if (x < MinX)
                    MinX = x;
                if (y < MinY)
                    MinY = y;

                DoProject((double)(m_MonoModel.m_nCamCols),
                          (double)(m_MonoModel.m_nCamRows),
                          out x,
                          out y,
                          out z);
                m_ShpInfo.m_PntImgXs[LinIdx][PntIdx][2] = x;
                m_ShpInfo.m_PntImgYs[LinIdx][PntIdx][2] = y;
                m_ShpInfo.m_PntImgZs[LinIdx][PntIdx][2] = z;
                if (x < MinX)
                    MinX = x;
                if (y < MinY)
                    MinY = y;

                DoProject((double)(m_MonoModel.m_nCamCols),
                          0.0,
                          out x,
                          out y,
                          out z);
                m_ShpInfo.m_PntImgXs[LinIdx][PntIdx][3] = x;
                m_ShpInfo.m_PntImgYs[LinIdx][PntIdx][3] = y;
                m_ShpInfo.m_PntImgZs[LinIdx][PntIdx][3] = z;
                if (x < MinX)
                    MinX = x;
                if (y < MinY)
                    MinY = y;

                Poly = new ClipperPath();
                for (i = 0; i < 4; i++)
                    Poly.Add(new Clipper.IntPoint((int)(10000.0 * (m_ShpInfo.m_PntImgXs[LinIdx][PntIdx][i] - MinX)),
                                                  (int)(10000.0 * (m_ShpInfo.m_PntImgYs[LinIdx][PntIdx][i] - MinY))));
                Area = 1.0e-8 * Clipper.Clipper.Area(Poly);
                m_ShpInfo.m_PntImgArea[LinIdx][PntIdx] = Area;
                m_ShpInfo.m_PntGSDA[LinIdx][PntIdx] = 100.0 * Math.Sqrt(Area / m_nSensorArea);
                Poly.Clear();
                Poly = null;
            }

            return "OK";
        }

        #endregion Private Calc Images

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Calc Overlaps ---------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Calc Overlaps

        /*=== DoProcess_CalcOverlaps() ===*/
        private String DoProcess_CalcOverlaps()
        {
            int LinCount, LinIdx;
            String Res;

            LinCount = m_ShpInfo.m_PntLinIdxs.Count;
            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                Res = DoProcess_CalcOverlaps_Lin(LinIdx);
                if (Res != "OK")
                    return Res;
            }

            return "OK";
        }

        /*=== DoProcess_CalcOverlaps_Lin() ===*/
        private String DoProcess_CalcOverlaps_Lin(int LinIdx)
        {
            int PntCount, PntIdx, TrsCount;
            String Res;
            double OverLon_Min, OverLon_Max, OverLon_Avg, OverLon_Avg2, OverLon_Std;
            double OverTrs_Min, OverTrs_Max, OverTrs_Avg, OverTrs_Avg2, OverTrs_Std;
            double GSDA_Min, GSDA_Max, GSDA_Avg, GSDA_Avg2, GSDA_Std;
            double Over, GSDA;

            PntCount = m_ShpInfo.m_PntXs[LinIdx].Count;
            for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
            {
                Res = DoProcess_CalcOverlaps_Pnt(LinIdx,
                                                 PntIdx);

                if (Res != "OK")
                    return Res;
            }

            OverLon_Min = double.MaxValue;
            OverLon_Max = double.MinValue;
            OverLon_Avg = 0.0;
            OverLon_Avg2 = 0.0;
            OverLon_Std = 0.0;
            OverTrs_Min = double.MaxValue;
            OverTrs_Max = double.MinValue;
            OverTrs_Avg = 0.0;
            OverTrs_Avg2 = 0.0;
            OverTrs_Std = 0.0;
            GSDA_Min = double.MaxValue;
            GSDA_Max = double.MinValue;
            GSDA_Avg = 0.0;
            GSDA_Avg2 = 0.0;
            GSDA_Std = 0.0;
            TrsCount = 0;
            for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
            {
                Over = m_ShpInfo.m_PntOverLon[LinIdx][PntIdx];
                if (Over < OverLon_Min)
                    OverLon_Min = Over;
                if (Over > OverLon_Max)
                    OverLon_Max = Over;
                OverLon_Avg += Over;
                OverLon_Avg2 += Over * Over;

                Over = m_ShpInfo.m_PntOverTrs[LinIdx][PntIdx];
                if (Over != 0)
                {
                    if (Over < OverTrs_Min)
                        OverTrs_Min = Over;
                    if (Over > OverTrs_Max)
                        OverTrs_Max = Over;
                    OverTrs_Avg += Over;
                    OverTrs_Avg2 += Over * Over;
                    ++TrsCount;
                }

                GSDA = m_ShpInfo.m_PntGSDA[LinIdx][PntIdx];
                if (GSDA < GSDA_Min)
                    GSDA_Min = GSDA;
                if (GSDA > GSDA_Max)
                    GSDA_Max = GSDA;
                GSDA_Avg += GSDA;
                GSDA_Avg2 += GSDA * GSDA;
            }
            OverLon_Avg /= (double)PntCount;
            OverLon_Avg2 /= (double)PntCount;
            OverTrs_Avg /= (double)TrsCount;
            OverTrs_Avg2 /= (double)TrsCount;
            GSDA_Avg /= (double)PntCount;
            GSDA_Avg2 /= (double)PntCount;

            OverLon_Std = Math.Sqrt(OverLon_Avg2 - OverLon_Avg * OverLon_Avg);
            OverTrs_Std = Math.Sqrt(OverTrs_Avg2 - OverTrs_Avg * OverTrs_Avg);
            GSDA_Std = Math.Sqrt(GSDA_Avg2 - GSDA_Avg * GSDA_Avg);

            m_ShpInfo.m_LinOverLon_Min[LinIdx] = OverLon_Min;
            m_ShpInfo.m_LinOverLon_Max[LinIdx] = OverLon_Max;
            m_ShpInfo.m_LinOverLon_Avg[LinIdx] = OverLon_Avg;
            m_ShpInfo.m_LinOverLon_Std[LinIdx] = OverLon_Std;

            m_ShpInfo.m_LinOverTrs_Min[LinIdx] = OverTrs_Min;
            m_ShpInfo.m_LinOverTrs_Max[LinIdx] = OverTrs_Max;
            m_ShpInfo.m_LinOverTrs_Avg[LinIdx] = OverTrs_Avg;
            m_ShpInfo.m_LinOverTrs_Std[LinIdx] = OverTrs_Std;

            m_ShpInfo.m_LinGSDA_Min[LinIdx] = GSDA_Min;
            m_ShpInfo.m_LinGSDA_Max[LinIdx] = GSDA_Max;
            m_ShpInfo.m_LinGSDA_Avg[LinIdx] = GSDA_Avg;
            m_ShpInfo.m_LinGSDA_Std[LinIdx] = GSDA_Std;

            return "OK";
        }

        /*=== DoProcess_CalcOverlaps_Pnt() ===*/
        private String DoProcess_CalcOverlaps_Pnt(int LinIdx,
                                                  int PntIdx)
        {
            int i;
            double MinX = double.MaxValue, MinY = double.MaxValue;
            ClipperPath ThisPoly;
            double ThisPolyArea;
            double Over;
            double OverLon, OverTrs;
            double CountLon, CountTrs;

            for (i = 0; i < 4; i++)
            {
                if (m_ShpInfo.m_PntImgXs[LinIdx][PntIdx][i] < MinX)
                    MinX = m_ShpInfo.m_PntImgXs[LinIdx][PntIdx][i];
                if (m_ShpInfo.m_PntImgYs[LinIdx][PntIdx][i] < MinY)
                    MinY = m_ShpInfo.m_PntImgYs[LinIdx][PntIdx][i];
            }

            ThisPoly = new ClipperPath();
            for (i = 0; i < 4; i++)
                ThisPoly.Add(new Clipper.IntPoint((int)(10000.0 * (m_ShpInfo.m_PntImgXs[LinIdx][PntIdx][i] - MinX)),
                                                  (int)(10000.0 * (m_ShpInfo.m_PntImgYs[LinIdx][PntIdx][i] - MinY))));

            ThisPolyArea = m_ShpInfo.m_PntImgArea[LinIdx][PntIdx];

            OverLon = 0.0;
            OverTrs = 0.0;
            CountLon = 0.0;
            CountTrs = 0.0;
            if (PntIdx > 0)
            {
                Over = DoProcess_CalcOverlaps_PntB(LinIdx,
                                                   PntIdx,
                                                   MinX,
                                                   MinY,
                                                   ThisPolyArea,
                                                   ThisPoly);
                m_ShpInfo.m_PntOverB[LinIdx][PntIdx] = Over;
                OverLon += Over;
                CountLon += 1.0;
            }

            if (PntIdx < m_ShpInfo.m_PntOverF[LinIdx].Count - 1)
            {
                Over = DoProcess_CalcOverlaps_PntF(LinIdx,
                                                   PntIdx,
                                                   MinX,
                                                   MinY,
                                                   ThisPolyArea,
                                                   ThisPoly);
                m_ShpInfo.m_PntOverF[LinIdx][PntIdx] = Over;
                OverLon += Over;
                CountLon += 1.0;
            }

            if (m_ShpInfo.m_PntLeftIdxs[LinIdx][PntIdx] != -1)
            {
                Over = DoProcess_CalcOverlaps_PntL(LinIdx,
                                                   PntIdx,
                                                   MinX,
                                                   MinY,
                                                   ThisPolyArea,
                                                   ThisPoly);
                m_ShpInfo.m_PntOverL[LinIdx][PntIdx] = Over;
                OverTrs += Over;
                CountTrs += 1.0;
            }

            if (m_ShpInfo.m_PntRightIdxs[LinIdx][PntIdx] != -1)
            {
                Over = DoProcess_CalcOverlaps_PntR(LinIdx,
                                                   PntIdx,
                                                   MinX,
                                                   MinY,
                                                   ThisPolyArea,
                                                   ThisPoly);
                m_ShpInfo.m_PntOverR[LinIdx][PntIdx] = Over;
                OverTrs += Over;
                CountTrs += 1.0;
            }

            if (CountLon > 0)
            {
                OverLon /= CountLon;
                m_ShpInfo.m_PntOverLon[LinIdx][PntIdx] = OverLon;
            }

            if (CountTrs > 0)
            {
                OverTrs /= CountTrs;
                m_ShpInfo.m_PntOverTrs[LinIdx][PntIdx] = OverTrs;
            }

            ThisPoly.Clear();
            ThisPoly = null;

            return "OK";
        }

        /*=== DoProcess_CalcOverlaps_PntB() ===*/
        private double DoProcess_CalcOverlaps_PntB(int LinIdx,
                                                   int PntIdx,
                                                   double MinX,
                                                   double MinY,
                                                   double ThisArea,
                                                   ClipperPath ThisPoly)
        {
            int i, PIdx;
            double Area, Over = 0.0;
            ClipperPath Poly;
            Clipper.Clipper c;
            ClipperPaths solution;

            PIdx = PntIdx - 1;

            Poly = new ClipperPath();
            for (i = 0; i < 4; i++)
                Poly.Add(new Clipper.IntPoint((int)(10000.0 * (m_ShpInfo.m_PntImgXs[LinIdx][PIdx][i] - MinX)),
                                              (int)(10000.0 * (m_ShpInfo.m_PntImgYs[LinIdx][PIdx][i] - MinY))));

            c = new Clipper.Clipper();
            c.AddPath(ThisPoly, Clipper.PolyType.ptClip, true);
            c.AddPath(Poly, Clipper.PolyType.ptSubject, true);
            solution = new ClipperPaths();
            c.Execute(Clipper.ClipType.ctIntersection, solution, Clipper.PolyFillType.pftEvenOdd, Clipper.PolyFillType.pftEvenOdd);
            if (solution.Count == 1)
            {
                Area = Math.Abs(1.0e-8 * Clipper.Clipper.Area(solution[0]));
                Over = 100.0 * (Area / ThisArea);
            }
            
            Poly.Clear();
            Poly = null;
            c.Clear();
            c = null;
            solution.Clear();
            solution = null;

            return Over;
        }

        /*=== DoProcess_CalcOverlaps_PntF() ===*/
        private double DoProcess_CalcOverlaps_PntF(int LinIdx,
                                                   int PntIdx,
                                                   double MinX,
                                                   double MinY,
                                                   double ThisArea,
                                                   ClipperPath ThisPoly)
        {
            int i, PIdx;
            double Area, Over = 0.0;
            ClipperPath Poly;
            Clipper.Clipper c;
            ClipperPaths solution;

            PIdx = PntIdx + 1;

            Poly = new ClipperPath();
            for (i = 0; i < 4; i++)
                Poly.Add(new Clipper.IntPoint((int)(10000.0 * (m_ShpInfo.m_PntImgXs[LinIdx][PIdx][i] - MinX)),
                                              (int)(10000.0 * (m_ShpInfo.m_PntImgYs[LinIdx][PIdx][i] - MinY))));

            c = new Clipper.Clipper();
            c.AddPath(ThisPoly, Clipper.PolyType.ptClip, true);
            c.AddPath(Poly, Clipper.PolyType.ptSubject, true);
            solution = new ClipperPaths();
            c.Execute(Clipper.ClipType.ctIntersection, solution, Clipper.PolyFillType.pftEvenOdd, Clipper.PolyFillType.pftEvenOdd);
            if (solution.Count == 1)
            {
                Area = Math.Abs(1.0e-8 * Clipper.Clipper.Area(solution[0]));
                Over = 100.0 * (Area / ThisArea);
            }

            Poly.Clear();
            Poly = null;
            c.Clear();
            c = null;
            solution.Clear();
            solution = null;

            return Over;
        }

        /*=== DoProcess_CalcOverlaps_PntL() ===*/
        private double DoProcess_CalcOverlaps_PntL(int LinIdx,
                                                   int PntIdx,
                                                   double MinX,
                                                   double MinY,
                                                   double ThisArea,
                                                   ClipperPath ThisPoly)
        {
            int i, Lin_Idx, Pnt_Idx;
            double Area, Over = 0.0;
            ClipperPath Poly;
            Clipper.Clipper c;
            ClipperPaths solution;

            Lin_Idx = m_ShpInfo.m_LinPntIdxs[m_ShpInfo.m_LinLeftIdxs[m_ShpInfo.m_PntLinIdxs[LinIdx]]];
            Pnt_Idx = m_ShpInfo.m_PntLeftIdxs[LinIdx][PntIdx];

            Poly = new ClipperPath();
            for (i = 0; i < 4; i++)
                Poly.Add(new Clipper.IntPoint((int)(10000.0 * (m_ShpInfo.m_PntImgXs[Lin_Idx][Pnt_Idx][i] - MinX)),
                                              (int)(10000.0 * (m_ShpInfo.m_PntImgYs[Lin_Idx][Pnt_Idx][i] - MinY))));

            c = new Clipper.Clipper();
            c.AddPath(ThisPoly, Clipper.PolyType.ptClip, true);
            c.AddPath(Poly, Clipper.PolyType.ptSubject, true);
            solution = new ClipperPaths();
            c.Execute(Clipper.ClipType.ctIntersection, solution, Clipper.PolyFillType.pftEvenOdd, Clipper.PolyFillType.pftEvenOdd);
            if (solution.Count == 1)
            {
                Area = Math.Abs(1.0e-8 * Clipper.Clipper.Area(solution[0]));
                Over = 100.0 * (Area / ThisArea);
            }

            Poly.Clear();
            Poly = null;
            c.Clear();
            c = null;
            solution.Clear();
            solution = null;

            return Over;
        }

        /*=== DoProcess_CalcOverlaps_PntR() ===*/
        private double DoProcess_CalcOverlaps_PntR(int LinIdx,
                                                   int PntIdx,
                                                   double MinX,
                                                   double MinY,
                                                   double ThisArea,
                                                   ClipperPath ThisPoly)
        {
            int i, Lin_Idx, Pnt_Idx;
            double Area, Over = 0.0;
            ClipperPath Poly;
            Clipper.Clipper c;
            ClipperPaths solution;

            Lin_Idx = m_ShpInfo.m_LinPntIdxs[m_ShpInfo.m_LinRightIdxs[m_ShpInfo.m_PntLinIdxs[LinIdx]]];
            Pnt_Idx = m_ShpInfo.m_PntRightIdxs[LinIdx][PntIdx];

            Poly = new ClipperPath();
            for (i = 0; i < 4; i++)
                Poly.Add(new Clipper.IntPoint((int)(10000.0 * (m_ShpInfo.m_PntImgXs[Lin_Idx][Pnt_Idx][i] - MinX)),
                                              (int)(10000.0 * (m_ShpInfo.m_PntImgYs[Lin_Idx][Pnt_Idx][i] - MinY))));

            c = new Clipper.Clipper();
            c.AddPath(ThisPoly, Clipper.PolyType.ptClip, true);
            c.AddPath(Poly, Clipper.PolyType.ptSubject, true);
            solution = new ClipperPaths();
            c.Execute(Clipper.ClipType.ctIntersection, solution, Clipper.PolyFillType.pftEvenOdd, Clipper.PolyFillType.pftEvenOdd);
            if (solution.Count == 1)
            {
                Area = Math.Abs(1.0e-8 * Clipper.Clipper.Area(solution[0]));
                Over = 100.0 * (Area / ThisArea);
            }

            Poly.Clear();
            Poly = null;
            c.Clear();
            c = null;
            solution.Clear();
            solution = null;

            return Over;
        }

        #endregion Private Calc Overlaps

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Calc GSDs -------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Calc GSDs

        /*=== DoProcess_CalcGSDs() ===*/
        private String DoProcess_CalcGSDs()
        {
            int LinCount, LinIdx;
            String Res;

            LinCount = m_ShpInfo.m_PntLinIdxs.Count;
            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                Res = DoProcess_CalcGSDs_Lin(LinIdx);
                if (Res != "OK")
                    return Res;
            }

            return "OK";
        }

        /*=== DoProcess_CalcGSDs_Lin() ===*/
        private String DoProcess_CalcGSDs_Lin(int LinIdx)
        {
            int PntCount, PntIdx;
            int StepX, StepY, col, row;
            int Xpix, Ypix;
            double xw, yw, zw, gsd;
            double GSDMin, GSDMax, GSDAvg, GSDAvg2, GSDStd;
            double HMin, HMax, HAvg, HAvg2, HStd;
            double GSDGridSize2 = (double)(m_nGSDGridSize * m_nGSDGridSize);
            double GSDAvg_Min, GSDAvg_Max, GSDAvg_Avg, GSDAvg_Avg2, GSDAvg_Std;
            double H_Min, H_Max, H_Avg, H_Avg2, H_Std;

            StepX = m_MonoModel.m_nCamCols / (m_nGSDGridSize - 1);
            StepY = m_MonoModel.m_nCamRows / (m_nGSDGridSize - 1);

            PntCount = m_ShpInfo.m_PntXs[LinIdx].Count;
            GSDAvg_Min = double.MaxValue;
            GSDAvg_Max = double.MinValue;
            GSDAvg_Avg = 0.0;
            GSDAvg_Avg2 = 0.0;
            GSDAvg_Std = 0.0;
            H_Min = double.MaxValue;
            H_Max = double.MinValue;
            H_Avg = 0.0;
            H_Avg2 = 0.0;
            H_Std = 0.0;
            for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
            {
                if (PntIdx == 0)
                    m_MonoModel.SetImgParams(m_ShpInfo.m_PntXs[LinIdx][PntIdx],
                                             m_ShpInfo.m_PntYs[LinIdx][PntIdx],
                                             m_ShpInfo.m_PntZs[LinIdx][PntIdx],

                                             m_ShpInfo.m_LinKappas[m_ShpInfo.m_PntLinIdxs[LinIdx]]);
                else
                    m_MonoModel.SetImgPos(m_ShpInfo.m_PntXs[LinIdx][PntIdx],
                                          m_ShpInfo.m_PntYs[LinIdx][PntIdx],
                                          m_ShpInfo.m_PntZs[LinIdx][PntIdx]);

                GSDMin = double.MaxValue;
                GSDMax = double.MinValue;
                GSDAvg = 0.0;
                GSDAvg2 = 0.0;
                GSDStd = 0.0;
                HMin = double.MaxValue;
                HMax = double.MinValue;
                HAvg = 0.0;
                HAvg2 = 0.0;
                HStd = 0.0;
                for (col = 0; col < m_nGSDGridSize; col++)
                {
                    if (col == m_nGSDGridSize - 1)
                        Xpix = m_MonoModel.m_nCamCols;
                    else
                        Xpix = StepX * col;
                    for (row = 0; row < m_nGSDGridSize; row++)
                    {
                        if (row == m_nGSDGridSize - 1)
                            Ypix = m_MonoModel.m_nCamRows;
                        else
                            Ypix = StepY * row;

                        gsd = DoProject((double)Xpix,
                                        (double)Ypix,

                                        out xw,
                                        out yw,
                                        out zw);
                        if (gsd < GSDMin)
                            GSDMin = gsd;
                        if (gsd > GSDMax)
                            GSDMax = gsd;
                        GSDAvg += gsd;
                        GSDAvg2 += gsd * gsd;

                        zw = m_ShpInfo.m_PntZs[LinIdx][PntIdx] - zw;
                        if (zw < HMin)
                            HMin = zw;
                        if (zw > HMax)
                            HMax = zw;
                        HAvg += zw;
                        HAvg2 += zw * zw;
                    }
                }

                GSDAvg /= GSDGridSize2;
                GSDAvg2 /= GSDGridSize2;
                GSDStd = Math.Sqrt(GSDAvg2 - GSDAvg * GSDAvg);

                HAvg /= GSDGridSize2;
                HAvg2 /= GSDGridSize2;
                HStd = Math.Sqrt(HAvg2 - HAvg * HAvg);

                m_ShpInfo.m_PntGSDMin[LinIdx][PntIdx] = GSDMin;
                m_ShpInfo.m_PntGSDMax[LinIdx][PntIdx] = GSDMax;
                m_ShpInfo.m_PntGSDAvg[LinIdx][PntIdx] = GSDAvg;
                m_ShpInfo.m_PntGSDStd[LinIdx][PntIdx] = GSDStd;

                if (GSDAvg < GSDAvg_Min)
                    GSDAvg_Min = GSDAvg;
                if (GSDAvg > GSDAvg_Max)
                    GSDAvg_Max = GSDAvg;
                GSDAvg_Avg += GSDAvg;
                GSDAvg_Avg2 += GSDAvg * GSDAvg;

                m_ShpInfo.m_PntHMin[LinIdx][PntIdx] = HMin;
                m_ShpInfo.m_PntHMax[LinIdx][PntIdx] = HMax;
                m_ShpInfo.m_PntHAvg[LinIdx][PntIdx] = HAvg;
                m_ShpInfo.m_PntHStd[LinIdx][PntIdx] = HStd;

                if (HAvg < H_Min)
                    H_Min = HAvg;
                if (HAvg > H_Max)
                    H_Max = HAvg;
                H_Avg += HAvg;
                H_Avg2 += HAvg * HAvg;
            }

            GSDAvg_Avg /= (double)PntCount;
            GSDAvg_Avg2 /= (double)PntCount;
            GSDAvg_Std = Math.Sqrt(GSDAvg_Avg2 - GSDAvg_Avg * GSDAvg_Avg);

            m_ShpInfo.m_LinGSDAvg_Min[LinIdx] = GSDAvg_Min;
            m_ShpInfo.m_LinGSDAvg_Max[LinIdx] = GSDAvg_Max;
            m_ShpInfo.m_LinGSDAvg_Avg[LinIdx] = GSDAvg_Avg;
            m_ShpInfo.m_LinGSDAvg_Std[LinIdx] = GSDAvg_Std;

            H_Avg /= (double)PntCount;
            H_Avg2 /= (double)PntCount;
            H_Std = Math.Sqrt(H_Avg2 - H_Avg * H_Avg);

            m_ShpInfo.m_LinH_Min[LinIdx] = H_Min;
            m_ShpInfo.m_LinH_Max[LinIdx] = H_Max;
            m_ShpInfo.m_LinH_Avg[LinIdx] = H_Avg;
            m_ShpInfo.m_LinH_Std[LinIdx] = H_Std;

            return "OK";
        }

        #endregion Private Calc GSDs

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Final Stats -----------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Final Stats

        /*=== GetFinalStats() ===*/
        private String GetFinalStats()
        {
            GetFinalStats_OverLon();
            GetFinalStats_OverTrs();
            GetFinalStats_GSD();

            return "OK";
        }

        /*=== GetFinalStats_OverLon() ===*/
        private void GetFinalStats_OverLon()
        {
            int LinCount, LinIdx, PntCount, PntIdx, TotalCount;
            double OverLon, Stats_OverLon_Avg2;

            m_nStats_OverLon_Min = double.MaxValue;
            m_nStats_OverLon_Max = double.MinValue;
            m_nStats_OverLon_Avg = 0.0;
            Stats_OverLon_Avg2 = 0.0;
            m_nStats_OverLon_Med = 0.0;
            m_nStats_OverLon_Std = 0.0;
            m_nStats_OverLon_Perc20Sigmas = 0.0;
            m_nStats_OverLon_Perc25Sigmas = 0.0;
            m_nStats_OverLon_Perc30Sigmas = 0.0;
            m_Stats_OverLon_HistX = new List<double>();
            m_Stats_OverLon_HistY = new List<double>();

            LinCount = m_ShpInfo.m_PntOverLon.Count;
            TotalCount = 0;
            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                PntCount = m_ShpInfo.m_PntOverLon[LinIdx].Count;
                for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
                {
                    OverLon = m_ShpInfo.m_PntOverLon[LinIdx][PntIdx];
                    if (OverLon == 0.0)
                        continue;

                    if (OverLon < m_nStats_OverLon_Min)
                        m_nStats_OverLon_Min = OverLon;
                    if (OverLon > m_nStats_OverLon_Max)
                        m_nStats_OverLon_Max = OverLon;
                    m_nStats_OverLon_Avg += OverLon;
                    Stats_OverLon_Avg2 += OverLon * OverLon;

                    ++TotalCount;
                }
            }
            m_nStats_OverLon_Avg /= (double)TotalCount;
            Stats_OverLon_Avg2 /= (double)TotalCount;
            m_nStats_OverLon_Std = Math.Sqrt(Stats_OverLon_Avg2 - m_nStats_OverLon_Avg * m_nStats_OverLon_Avg);

            GetFinalStats_OverLon_Hist(TotalCount);
        }

        /*=== GetFinalStats_OverLon_Hist() ===*/
        private void GetFinalStats_OverLon_Hist(int TotalCount)
        {
            int BeansCount, BeanIdx;
            double Step;
            int LinCount, LinIdx, PntCount, PntIdx;
            double OverLon, Delta;
            double MaxVal;
            double Over20SigmaLeft, Over20SigmaRight;
            double Over25SigmaLeft, Over25SigmaRight;
            double Over30SigmaLeft, Over30SigmaRight;

            Over20SigmaLeft = m_nStats_OverLon_Avg - 2.0 * m_nStats_OverLon_Std;
            Over20SigmaRight = m_nStats_OverLon_Avg + 2.0 * m_nStats_OverLon_Std;
            Over25SigmaLeft = m_nStats_OverLon_Avg - 2.5 * m_nStats_OverLon_Std;
            Over25SigmaRight = m_nStats_OverLon_Avg + 2.5 * m_nStats_OverLon_Std;
            Over30SigmaLeft = m_nStats_OverLon_Avg - 3.0 * m_nStats_OverLon_Std;
            Over30SigmaRight = m_nStats_OverLon_Avg + 3.0 * m_nStats_OverLon_Std;

            Delta = 1.0 / (double)TotalCount;

            BeansCount = (int)Math.Sqrt((double)TotalCount);
            Step = (m_nStats_OverLon_Max - m_nStats_OverLon_Min) / (double)BeansCount;
            for (BeanIdx = 0; BeanIdx < BeansCount; BeanIdx++)
            {
                m_Stats_OverLon_HistX.Add(m_nStats_OverLon_Min + ((double)BeanIdx + 0.5) * Step);
                m_Stats_OverLon_HistY.Add(0.0);
            }

            LinCount = m_ShpInfo.m_PntOverLon.Count;
            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                PntCount = m_ShpInfo.m_PntOverLon[LinIdx].Count;
                for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
                {
                    OverLon = m_ShpInfo.m_PntOverLon[LinIdx][PntIdx];
                    if (OverLon == 0.0)
                        continue;

                    if (OverLon >= Over20SigmaLeft && OverLon <= Over20SigmaRight)
                        m_nStats_OverLon_Perc20Sigmas += Delta;
                    if (OverLon >= Over25SigmaLeft && OverLon <= Over25SigmaRight)
                        m_nStats_OverLon_Perc25Sigmas += Delta;
                    if (OverLon >= Over30SigmaLeft && OverLon <= Over30SigmaRight)
                        m_nStats_OverLon_Perc30Sigmas += Delta;

                    BeanIdx = (int)(Math.Floor((OverLon - m_nStats_OverLon_Min) / Step));
                    if (BeanIdx < 0)
                        BeanIdx = 0;
                    else
                    if (BeanIdx > BeansCount - 1)
                        BeanIdx = BeansCount - 1;

                    m_Stats_OverLon_HistY[BeanIdx] += Delta;
                }
            }
            m_nStats_OverLon_Perc20Sigmas *= 100.0;
            m_nStats_OverLon_Perc25Sigmas *= 100.0;
            m_nStats_OverLon_Perc30Sigmas *= 100.0;

            MaxVal = double.MinValue;
            for (BeanIdx = 0; BeanIdx < BeansCount; BeanIdx++)
            {
                if (m_Stats_OverLon_HistY[BeanIdx] > MaxVal)
                {
                    MaxVal = m_Stats_OverLon_HistY[BeanIdx];
                    m_nStats_OverLon_Med = m_Stats_OverLon_HistX[BeanIdx];
                }
            }

            /*
            System.IO.StreamWriter SW = new System.IO.StreamWriter("xxx.txt");
            for (BeanIdx = 0; BeanIdx < BeansCount; BeanIdx++)
                SW.WriteLine(String.Format("{0:0.00}\t{1:0.00}", m_Stats_OverLon_HistX[BeanIdx], m_Stats_OverLon_HistY[BeanIdx]));
            SW.Close();
            SW.Dispose();
            SW = null;

            System.Windows.Forms.MessageBox.Show("m_nStats_OverLon_Min = " + m_nStats_OverLon_Min.ToString() + "\n" +
                                                 "m_nStats_OverLon_Max = " + m_nStats_OverLon_Max.ToString() + "\n" +
                                                 "m_nStats_OverLon_Avg = " + m_nStats_OverLon_Avg.ToString() + "\n" +
                                                 "m_nStats_OverLon_Med = " + m_nStats_OverLon_Med.ToString() + "\n" +
                                                 "m_nStats_OverLon_Std = " + m_nStats_OverLon_Std.ToString() + "\n\n" +

                                                 "m_nStats_OverLon_Perc20Sigmas = " + m_nStats_OverLon_Perc20Sigmas.ToString() + "\n" +
                                                 "m_nStats_OverLon_Perc25Sigmas = " + m_nStats_OverLon_Perc25Sigmas.ToString() + "\n" +
                                                 "m_nStats_OverLon_Perc30Sigmas = " + m_nStats_OverLon_Perc30Sigmas.ToString() + "\n");
            */
        }

        /*=== GetFinalStats_OverTrs() ===*/
        private void GetFinalStats_OverTrs()
        {
            int LinCount, LinIdx, PntCount, PntIdx, TotalCount;
            double OverTrs, Stats_OverTrs_Avg2;

            m_nStats_OverTrs_Min = double.MaxValue;
            m_nStats_OverTrs_Max = double.MinValue;
            m_nStats_OverTrs_Avg = 0.0;
            Stats_OverTrs_Avg2 = 0.0;
            m_nStats_OverTrs_Med = 0.0;
            m_nStats_OverTrs_Std = 0.0;
            m_nStats_OverTrs_Perc20Sigmas = 0.0;
            m_nStats_OverTrs_Perc25Sigmas = 0.0;
            m_nStats_OverTrs_Perc30Sigmas = 0.0;
            m_Stats_OverTrs_HistX = new List<double>();
            m_Stats_OverTrs_HistY = new List<double>();

            LinCount = m_ShpInfo.m_PntOverTrs.Count;
            TotalCount = 0;
            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                PntCount = m_ShpInfo.m_PntOverTrs[LinIdx].Count;
                for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
                {
                    OverTrs = m_ShpInfo.m_PntOverTrs[LinIdx][PntIdx];
                    if (OverTrs == 0)
                        continue;

                    if (OverTrs < m_nStats_OverTrs_Min)
                        m_nStats_OverTrs_Min = OverTrs;
                    if (OverTrs > m_nStats_OverTrs_Max)
                        m_nStats_OverTrs_Max = OverTrs;
                    m_nStats_OverTrs_Avg += OverTrs;
                    Stats_OverTrs_Avg2 += OverTrs * OverTrs;

                    ++TotalCount;
                }
            }
            m_nStats_OverTrs_Avg /= (double)TotalCount;
            Stats_OverTrs_Avg2 /= (double)TotalCount;
            m_nStats_OverTrs_Std = Math.Sqrt(Stats_OverTrs_Avg2 - m_nStats_OverTrs_Avg * m_nStats_OverTrs_Avg);

            GetFinalStats_OverTrs_Hist(TotalCount);
        }

        /*=== GetFinalStats_OverTrs_Hist() ===*/
        private void GetFinalStats_OverTrs_Hist(int TotalCount)
        {
            int BeansCount, BeanIdx;
            double Step;
            int LinCount, LinIdx, PntCount, PntIdx;
            double OverTrs, Delta;
            double MaxVal;
            double Over20SigmaLeft, Over20SigmaRight;
            double Over25SigmaLeft, Over25SigmaRight;
            double Over30SigmaLeft, Over30SigmaRight;

            Over20SigmaLeft = m_nStats_OverTrs_Avg - 2.0 * m_nStats_OverTrs_Std;
            Over20SigmaRight = m_nStats_OverTrs_Avg + 2.0 * m_nStats_OverTrs_Std;
            Over25SigmaLeft = m_nStats_OverTrs_Avg - 2.5 * m_nStats_OverTrs_Std;
            Over25SigmaRight = m_nStats_OverTrs_Avg + 2.5 * m_nStats_OverTrs_Std;
            Over30SigmaLeft = m_nStats_OverTrs_Avg - 3.0 * m_nStats_OverTrs_Std;
            Over30SigmaRight = m_nStats_OverTrs_Avg + 3.0 * m_nStats_OverTrs_Std;

            Delta = 1.0 / (double)TotalCount;

            BeansCount = (int)Math.Sqrt((double)TotalCount);
            Step = (m_nStats_OverTrs_Max - m_nStats_OverTrs_Min) / (double)BeansCount;
            for (BeanIdx = 0; BeanIdx < BeansCount; BeanIdx++)
            {
                m_Stats_OverTrs_HistX.Add(m_nStats_OverTrs_Min + ((double)BeanIdx + 0.5) * Step);
                m_Stats_OverTrs_HistY.Add(0.0);
            }

            LinCount = m_ShpInfo.m_PntOverTrs.Count;
            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                PntCount = m_ShpInfo.m_PntOverTrs[LinIdx].Count;
                for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
                {
                    OverTrs = m_ShpInfo.m_PntOverTrs[LinIdx][PntIdx];

                    if (OverTrs >= Over20SigmaLeft && OverTrs <= Over20SigmaRight)
                        m_nStats_OverTrs_Perc20Sigmas += Delta;
                    if (OverTrs >= Over25SigmaLeft && OverTrs <= Over25SigmaRight)
                        m_nStats_OverTrs_Perc25Sigmas += Delta;
                    if (OverTrs >= Over30SigmaLeft && OverTrs <= Over30SigmaRight)
                        m_nStats_OverTrs_Perc30Sigmas += Delta;

                    BeanIdx = (int)(Math.Floor((OverTrs - m_nStats_OverTrs_Min) / Step));
                    if (BeanIdx < 0)
                        BeanIdx = 0;
                    else
                        if (BeanIdx > BeansCount - 1)
                            BeanIdx = BeansCount - 1;

                    m_Stats_OverTrs_HistY[BeanIdx] += Delta;
                }
            }
            m_nStats_OverTrs_Perc20Sigmas *= 100.0;
            m_nStats_OverTrs_Perc25Sigmas *= 100.0;
            m_nStats_OverTrs_Perc30Sigmas *= 100.0;

            MaxVal = double.MinValue;
            for (BeanIdx = 0; BeanIdx < BeansCount; BeanIdx++)
            {
                if (m_Stats_OverTrs_HistY[BeanIdx] > MaxVal)
                {
                    MaxVal = m_Stats_OverTrs_HistY[BeanIdx];
                    m_nStats_OverTrs_Med = m_Stats_OverTrs_HistX[BeanIdx];
                }
            }

            /*
            System.IO.StreamWriter SW = new System.IO.StreamWriter("xxx.txt");
            for (BeanIdx = 0; BeanIdx < BeansCount; BeanIdx++)
                SW.WriteLine(String.Format("{0:0.00}\t{1:0.00}", m_Stats_OverTrs_HistX[BeanIdx], m_Stats_OverTrs_HistY[BeanIdx]));
            SW.Close();
            SW.Dispose();
            SW = null;

            System.Windows.Forms.MessageBox.Show("m_nStats_OverTrs_Min = " + m_nStats_OverTrs_Min.ToString() + "\n" +
                                                 "m_nStats_OverTrs_Max = " + m_nStats_OverTrs_Max.ToString() + "\n" +
                                                 "m_nStats_OverTrs_Avg = " + m_nStats_OverTrs_Avg.ToString() + "\n" +
                                                 "m_nStats_OverTrs_Med = " + m_nStats_OverTrs_Med.ToString() + "\n" +
                                                 "m_nStats_OverTrs_Std = " + m_nStats_OverTrs_Std.ToString() + "\n\n" +

                                                 "m_nStats_OverTrs_Perc20Sigmas = " + m_nStats_OverTrs_Perc20Sigmas.ToString() + "\n" +
                                                 "m_nStats_OverTrs_Perc25Sigmas = " + m_nStats_OverTrs_Perc25Sigmas.ToString() + "\n" +
                                                 "m_nStats_OverTrs_Perc30Sigmas = " + m_nStats_OverTrs_Perc30Sigmas.ToString() + "\n");
            */
        }

        /*=== GetFinalStats_GSD() ===*/
        private void GetFinalStats_GSD()
        {
            int LinCount, LinIdx, PntCount, PntIdx, TotalCount;
            double GSDAvg, Stats_GSD_Avg2;

            m_nStats_GSD_Min = double.MaxValue;
            m_nStats_GSD_Max = double.MinValue;
            m_nStats_GSD_Avg = 0.0;
            Stats_GSD_Avg2 = 0.0;
            m_nStats_GSD_Med = 0.0;
            m_nStats_GSD_Std = 0.0;
            m_nStats_GSD_Perc20Sigmas = 0.0;
            m_nStats_GSD_Perc25Sigmas = 0.0;
            m_nStats_GSD_Perc30Sigmas = 0.0;
            m_Stats_GSD_HistX = new List<double>();
            m_Stats_GSD_HistY = new List<double>();

            LinCount = m_ShpInfo.m_PntGSDAvg.Count;
            TotalCount = 0;
            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                PntCount = m_ShpInfo.m_PntGSDAvg[LinIdx].Count;
                for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
                {
                    GSDAvg = m_ShpInfo.m_PntGSDAvg[LinIdx][PntIdx];

                    if (GSDAvg < m_nStats_GSD_Min)
                        m_nStats_GSD_Min = GSDAvg;
                    if (GSDAvg > m_nStats_GSD_Max)
                        m_nStats_GSD_Max = GSDAvg;
                    m_nStats_GSD_Avg += GSDAvg;
                    Stats_GSD_Avg2 += GSDAvg * GSDAvg;

                    ++TotalCount;
                }
            }
            m_nStats_GSD_Avg /= (double)TotalCount;
            Stats_GSD_Avg2 /= (double)TotalCount;
            m_nStats_GSD_Std = Math.Sqrt(Stats_GSD_Avg2 - m_nStats_GSD_Avg * m_nStats_GSD_Avg);

            GetFinalStats_GSD_Hist(TotalCount);
        }

        /*=== GetFinalStats_GSD_Hist() ===*/
        private void GetFinalStats_GSD_Hist(int TotalCount)
        {
            int BeansCount, BeanIdx;
            double Step;
            int LinCount, LinIdx, PntCount, PntIdx;
            double GSDAvg, Delta;
            double MaxVal;
            double Over20SigmaLeft, Over20SigmaRight;
            double Over25SigmaLeft, Over25SigmaRight;
            double Over30SigmaLeft, Over30SigmaRight;

            Over20SigmaLeft = m_nStats_GSD_Avg - 2.0 * m_nStats_GSD_Std;
            Over20SigmaRight = m_nStats_GSD_Avg + 2.0 * m_nStats_GSD_Std;
            Over25SigmaLeft = m_nStats_GSD_Avg - 2.5 * m_nStats_GSD_Std;
            Over25SigmaRight = m_nStats_GSD_Avg + 2.5 * m_nStats_GSD_Std;
            Over30SigmaLeft = m_nStats_GSD_Avg - 3.0 * m_nStats_GSD_Std;
            Over30SigmaRight = m_nStats_GSD_Avg + 3.0 * m_nStats_GSD_Std;

            Delta = 1.0 / (double)TotalCount;

            BeansCount = (int)Math.Sqrt((double)TotalCount);
            Step = (m_nStats_GSD_Max - m_nStats_GSD_Min) / (double)BeansCount;
            for (BeanIdx = 0; BeanIdx < BeansCount; BeanIdx++)
            {
                m_Stats_GSD_HistX.Add(m_nStats_GSD_Min + ((double)BeanIdx + 0.5) * Step);
                m_Stats_GSD_HistY.Add(0.0);
            }

            LinCount = m_ShpInfo.m_PntGSDAvg.Count;
            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                PntCount = m_ShpInfo.m_PntGSDAvg[LinIdx].Count;
                for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
                {
                    GSDAvg = m_ShpInfo.m_PntGSDAvg[LinIdx][PntIdx];

                    if (GSDAvg >= Over20SigmaLeft && GSDAvg <= Over20SigmaRight)
                        m_nStats_GSD_Perc20Sigmas += Delta;
                    if (GSDAvg >= Over25SigmaLeft && GSDAvg <= Over25SigmaRight)
                        m_nStats_GSD_Perc25Sigmas += Delta;
                    if (GSDAvg >= Over30SigmaLeft && GSDAvg <= Over30SigmaRight)
                        m_nStats_GSD_Perc30Sigmas += Delta;

                    BeanIdx = (int)(Math.Floor((GSDAvg - m_nStats_GSD_Min) / Step));
                    if (BeanIdx < 0)
                        BeanIdx = 0;
                    else
                        if (BeanIdx > BeansCount - 1)
                            BeanIdx = BeansCount - 1;

                    m_Stats_GSD_HistY[BeanIdx] += Delta;
                }
            }
            m_nStats_GSD_Perc20Sigmas *= 100.0;
            m_nStats_GSD_Perc25Sigmas *= 100.0;
            m_nStats_GSD_Perc30Sigmas *= 100.0;

            MaxVal = double.MinValue;
            for (BeanIdx = 0; BeanIdx < BeansCount; BeanIdx++)
            {
                if (m_Stats_GSD_HistY[BeanIdx] > MaxVal)
                {
                    MaxVal = m_Stats_GSD_HistY[BeanIdx];
                    m_nStats_GSD_Med = m_Stats_GSD_HistX[BeanIdx];
                }
            }

            /*
            System.IO.StreamWriter SW = new System.IO.StreamWriter("xxx.txt");
            for (BeanIdx = 0; BeanIdx < BeansCount; BeanIdx++)
                SW.WriteLine(String.Format("{0:0.00}\t{1:0.00}", m_Stats_GSD_HistX[BeanIdx], m_Stats_GSD_HistY[BeanIdx]));
            SW.Close();
            SW.Dispose();
            SW = null;

            System.Windows.Forms.MessageBox.Show("m_nStats_GSD_Min = " + m_nStats_GSD_Min.ToString() + "\n" +
                                                 "m_nStats_GSD_Max = " + m_nStats_GSD_Max.ToString() + "\n" +
                                                 "m_nStats_GSD_Avg = " + m_nStats_GSD_Avg.ToString() + "\n" +
                                                 "m_nStats_GSD_Med = " + m_nStats_GSD_Med.ToString() + "\n" +
                                                 "m_nStats_GSD_Std = " + m_nStats_GSD_Std.ToString() + "\n\n" +

                                                 "m_nStats_GSD_Perc20Sigmas = " + m_nStats_GSD_Perc20Sigmas.ToString() + "\n" +
                                                 "m_nStats_GSD_Perc25Sigmas = " + m_nStats_GSD_Perc25Sigmas.ToString() + "\n" +
                                                 "m_nStats_GSD_Perc30Sigmas = " + m_nStats_GSD_Perc30Sigmas.ToString() + "\n");
            */
        }

        #endregion Private Final Stats

        #endregion Private Process

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Auxiliary -------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Auxiliary

        /*=== DoProject() ===*/
        private double DoProject(double Xpix,
                                 double Ypix,

                                 out double Xwrld,
                                 out double Ywrld,
                                 out double Zwrld)
        {
            double Z, PrevZ;
            double xw, yw, gsd = 0.0;

            Xwrld = 0.0;
            Ywrld = 0.0;
            Zwrld = 0.0;

            Z = m_DTMInfo.GetZ(m_MonoModel.m_PhotoProjCenter[0],
                               m_MonoModel.m_PhotoProjCenter[1]);

            int iter = 0;
            while (true)
            {
                ++iter;
                m_MonoModel.ImgToWrld_ByZ(Xpix,
                                          Ypix,
                                          Z,

                                          out xw,
                                          out yw,
                                          out gsd);

                PrevZ = Z;
                Z = m_DTMInfo.GetZ(xw,
                                   yw);

                if (Math.Abs(PrevZ - Z) < Z_COMPARE_DELTA || iter > MAX_ITER)
                {
                    if (iter > MAX_ITER)
                    {
                        Z = 0.5 * (PrevZ + Z);
                        m_MonoModel.ImgToWrld_ByZ(Xpix,
                                                  Ypix,
                                                  Z,

                                                  out xw,
                                                  out yw,
                                                  out gsd);
                    }

                    Xwrld = xw;
                    Ywrld = yw;
                    Zwrld = Z;

                    break;
                }
            }

            return gsd;
        }

        #endregion Private Auxiliary

        #endregion Private
    }
}
