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
    public partial class ViewerCtrl : UserControl
    {
        private const String OVER_LON_FLD_NAME = "OVR_LON";
        private const String OVER_TRS_FLD_NAME = "OVR_TRS";
        private const String GSD_FLD_NAME = "GSD_AVG";
        private const String ELEVATION_FLD_NAME = "ELEVATION";

        private const double OVER_LON_MIN_LOW = 65.0;
        private const double OVER_LON_MAX_LOW = 75.0;
        private const double OVER_TRS_MIN_LOW = 35.0;
        private const double OVER_TRS_MAX_LOW = 100.0001;
        private const double GSD_MIN_LOW = 7.0;
        private const double GSD_MAX_LOW = 9.0;

        private const double OVER_LON_MIN_HIGH = 65.0;
        private const double OVER_LON_MAX_HIGH = 75.0;
        private const double OVER_TRS_MIN_HIGH = 25.0;
        private const double OVER_TRS_MAX_HIGH = 100.0001;
        private const double GSD_MIN_HIGH = 17.0;
        private const double GSD_MAX_HIGH = 23.0;

        private const int MOUSE_DELTA = 5;

        private bool m_bIsFlight;

        private String m_sImgsFile;
        private String m_sPolyFile;

        private double m_nOverLon_Avg;
        private double m_nOverLon_Std;

        private double m_nOverTrs_Avg;
        private double m_nOverTrs_Std;

        private double m_nGSD_Avg;
        private double m_nGSD_Std;

        private bool m_bStatsExist;

        private List<double> m_CXs;
        private List<double> m_CYs;
        private List<double> m_CZs;
        private List<double[]> m_Xs;
        private List<double[]> m_Ys;
        private List<double[]> m_Zs;
        private List<double> m_OverLon;
        private List<double> m_OverTrs;
        private List<double> m_GSD;

        double m_nMinX;
        double m_nMinY;
        double m_nMaxX;
        double m_nMaxY;

        private bool m_bPolyExists;
        private List<List<List<double>>> m_PXs;
        private List<List<List<double>>> m_PYs;

        private double m_nWrldCenterX;
        private double m_nWrldCenterY;
        private double m_nWrldToPix;

        private bool m_bMouseDown;
        private int m_nIniX;
        private int m_nIniY;
        private int m_nPrevX;
        private int m_nPrevY;
        private int m_nSelIdx;

        private Pen m_PenGreen;
        private Pen m_PenBlue;
        private Pen m_PenBrown;
        private Pen m_PenRed;
        private Pen m_PenGray;
        private Pen m_PenSel;
        private Pen m_PenPoly;

        private Pen m_DashPenGreen;
        private Pen m_DashPenBlue;
        private Pen m_DashPenBrown;
        private Pen m_DashPenRed;
        private Pen m_DashPenGray;

        private Brush m_BrushGreen;
        private Brush m_BrushBlue;
        private Brush m_BrushBrown;
        private Brush m_BrushRed;
        private Brush m_BrushGray;

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Public ------------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Public

        /*=== ViewerCtrl() ===*/
        public ViewerCtrl()
        {
            InitializeComponent();

            m_CXs = null;
            m_CYs = null;
            m_CZs = null;
            m_Xs = null;
            m_Ys = null;
            m_Zs = null;
            m_OverLon = null;
            m_OverTrs = null;
            m_GSD = null;

            m_PXs = null;
            m_PYs = null;

            m_PenGreen = null;
            m_PenBlue = null;
            m_PenBrown = null;
            m_PenRed = null;
            m_PenGray = null;
            m_PenSel = null;
            m_PenPoly = null;

            m_DashPenGreen = null;
            m_DashPenBlue = null;
            m_DashPenBrown = null;
            m_DashPenRed = null;
            m_DashPenGray = null;

            m_BrushGreen = null;
            m_BrushBlue = null;
            m_BrushBrown = null;
            m_BrushRed = null;
            m_BrushGray = null;

            ClearStats();
        }

        /*=== SetStats() ===*/
        public void SetStats(bool IsFlight,
            
                             String ImgsFile,
                             String PolyFile,

                             double OverLon_Avg,
                             double OverLon_Std,

                             double OverTrs_Avg,
                             double OverTrs_Std,

                             double GSD_Avg,
                             double GSD_Std)
        {
            String Res;

            ClearStats();

            m_bIsFlight = IsFlight;

            m_sImgsFile = ImgsFile;
            m_sPolyFile = PolyFile;
            if (m_sPolyFile != "")
                m_bPolyExists = true;
            else
                m_bPolyExists = false;

            m_nOverLon_Avg = OverLon_Avg;
            m_nOverLon_Std = OverLon_Std;

            m_nOverTrs_Avg = OverTrs_Avg;
            m_nOverTrs_Std = OverTrs_Std;

            m_nGSD_Avg = GSD_Avg;
            m_nGSD_Std = GSD_Std;

            Res = LoadInfo();
            if (Res != "OK")
            {
                ClearStats();
                MessageBox.Show("Error Loading Info:\n" + Res,
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return;
            }

            CreatePensAndBrushes();
            CritCB.SelectedIndex = 0;
            LowHighCB.SelectedIndex = 0;
            ViewFPsTB.Checked = true;
            ViewPtsTB.Checked = true;
            ViewPolyTB.Checked = true;
            ZoomCenteredTB.Checked = false;
            m_bStatsExist = true;
            GetZoomAll();
            VarCB.SelectedIndex = -1;
            VarCB.SelectedIndex = 0;

            MainTS.Enabled = true;
        }

        /*=== ClearStats() ===*/
        public void ClearStats()
        {
            int i, Count, j, Count1;

            ClearPensAndBrushes();

            m_bStatsExist = false;
            VarCB.SelectedIndex = 0;
            CritCB.SelectedIndex = 0;
            LowHighCB.SelectedIndex = 0;
            ViewFPsTB.Checked = true;
            ViewPtsTB.Checked = true;
            ViewPolyTB.Checked = true;
            ZoomCenteredTB.Checked = false;
            MainDP.Invalidate();

            m_bIsFlight = false;

            m_sImgsFile = "";
            m_sPolyFile = "";

            m_nOverLon_Avg = 0.0;
            m_nOverLon_Std = 0.0;

            m_nOverTrs_Avg = 0.0;
            m_nOverTrs_Std = 0.0;

            m_nGSD_Avg = 0.0;
            m_nGSD_Std = 0.0;

            if (m_CXs != null)
            {
                m_CXs.Clear();
                m_CXs = null;
            }
            if (m_CYs != null)
            {
                m_CYs.Clear();
                m_CYs = null;
            }
            if (m_CZs != null)
            {
                m_CZs.Clear();
                m_CZs = null;
            }
            if (m_Xs != null)
            {
                Count = m_Xs.Count;
                for (i = 0; i < Count; i++)
                    m_Xs[i] = null;
                m_Xs.Clear();
                m_Xs = null;
            }
            if (m_Ys != null)
            {
                Count = m_Ys.Count;
                for (i = 0; i < Count; i++)
                    m_Ys[i] = null;
                m_Ys.Clear();
                m_Ys = null;
            }
            if (m_Zs != null)
            {
                Count = m_Zs.Count;
                for (i = 0; i < Count; i++)
                    m_Zs[i] = null;
                m_Zs.Clear();
                m_Zs = null;
            }

            if (m_OverLon != null)
            {
                m_OverLon.Clear();
                m_OverLon = null;
            }
            if (m_OverTrs != null)
            {
                m_OverTrs.Clear();
                m_OverTrs = null;
            }
            if (m_GSD != null)
            {
                m_GSD.Clear();
                m_GSD = null;
            }

            m_nMinX = 0.0;
            m_nMinY = 0.0;
            m_nMaxX = 0.0;
            m_nMaxY = 0.0;

            m_bPolyExists = false;;
            if (m_PXs != null)
            {
                Count = m_PXs.Count;
                for (i = 0; i < Count; i++)
                {
                    Count1 = m_PXs[i].Count;
                    for (j = 0; j < Count1; j++)
                    {
                        m_PXs[i][j].Clear();
                        m_PXs[i][j] = null;
                    }
                    m_PXs[i].Clear();
                    m_PXs[i] = null;
                }
                m_PXs.Clear();
                m_PXs = null;
            }
            if (m_PYs != null)
            {
                Count = m_PYs.Count;
                for (i = 0; i < Count; i++)
                {
                    Count1 = m_PYs[i].Count;
                    for (j = 0; j < Count1; j++)
                    {
                        m_PYs[i][j].Clear();
                        m_PYs[i][j] = null;
                    }
                    m_PYs[i].Clear();
                    m_PYs[i] = null;
                }
                m_PYs.Clear();
                m_PYs = null;
            }

            m_nWrldCenterX = 0.0;
            m_nWrldCenterY = 0.0;
            m_nWrldToPix = 0.0;

            m_bMouseDown = false;
            m_nIniX = 0;
            m_nIniY = 0;
            m_nPrevX = 0;
            m_nPrevY = 0;
            m_nSelIdx = -1;

            ClearPensAndBrushes();

            MainTS.Enabled = false;
            CoordsSL.Text = "";
            InfoSL.Text = "";
        }

        #endregion Public

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private -----------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Load Info -------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Load Info

        /*=== LoadInfo() ===*/
        private String LoadInfo()
        {
            String Res;

            Res = LoadInfo_SHP();
            if (Res != "OK")
                return Res;

            return "OK";
        }

        /*=== LoadInfo_SHP() ===*/
        private String LoadInfo_SHP()
        {
            String Res;

            Res = LoadInfo_Centros();
            if (Res != "OK")
                return Res;

            Res = LoadInfo_Imgs();
            if (Res != "OK")
                return Res;

            if (m_bPolyExists)
            {
                Res = LoadInfo_Poly();
                if (Res != "OK")
                    return Res;
            }

            return "OK";
        }

        /*=== LoadInfo_Centros() ===*/
        private String LoadInfo_Centros()
        {
            String Res;

            if (!m_bIsFlight)
                Res = LoadInfo_Centros_2D();
            else
                Res = LoadInfo_Centros_3D();

            return Res;
        }

        /*=== LoadInfo_Centros_2D() ===*/
        private String LoadInfo_Centros_2D()
        {
            ShpLib SHP;
            bool Res;

            int ShapeType = 0;
            int RecCount = 0;
            double MinX = 0;
            double MinY = 0;
            double MaxX = 0;
            double MaxY = 0;

            int FldCount = 0;
            int FldIdx;
            String[] FldNames = new String[256];
            String[] FldTypes = new String[256];
            int[] FldLens = new int[256];
            int[] FldDecs = new int[256];

            int ELEVATION_FldIdx = -1;

            bool FirstGot = false;

            int PntsCount = 0;
            int PartsCount = 0;

            double[] PntsX;
            double[] PntsY;
            int[] Parts;

            String sElevation;
            double Elevation;

            String PntFileName;

            PntFileName = Path.GetFileNameWithoutExtension(m_sImgsFile);
            PntFileName = Path.GetDirectoryName(m_sImgsFile) + "\\" +
                          PntFileName.Substring(0, PntFileName.Length - 5) + ".shp";

            //=============//
            // Read Header //
            //=============//
            SHP = new ShpLib();

            Res = SHP.GetGenInfo(PntFileName,

                                 ref ShapeType,
                                 ref RecCount,
                                 ref MinX,
                                 ref MinY,
                                 ref MaxX,
                                 ref MaxY,

                                 ref FldCount,
                                 FldNames,
                                 FldTypes,
                                 FldLens,
                                 FldDecs);
            if (!Res)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Points Shape File.";
            }

            if (ShapeType != ShpLib.SHPTYPE_POINT)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Points Shape File: Shape Type must be POINT.";
            }

            for (FldIdx = 0; FldIdx < FldCount; FldIdx++)
            {
                if (FldNames[FldIdx].ToUpper() == ELEVATION_FLD_NAME)
                    ELEVATION_FldIdx = FldIdx;
            }
            if (ELEVATION_FldIdx == -1)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Points Shape File: Field '" + ELEVATION_FLD_NAME + "' does not exist.";
            }

            //=============//
            // Read Points //
            //=============//
            m_CXs = new List<double>();
            m_CYs = new List<double>();
            m_CZs = new List<double>();

            Res = SHP.GetShpInfo_Start(PntFileName,
                                       double.MinValue,
                                       double.MinValue,
                                       double.MaxValue,
                                       double.MaxValue,
                                       true);
            if (!Res)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Reading Points Shape File.";
            }

            PntsX = new double[1];
            PntsY = new double[1];
            Parts = new int[1];
            Parts[0] = 0;
            while (true)
            {
                if (!FirstGot)
                {
                    Res = SHP.GetShpInfo_First(ref PntsCount,
                                               ref PartsCount);
                    if (!Res)
                    {
                        FldNames = null;
                        FldTypes = null;
                        FldLens = null;
                        FldDecs = null;
                        SHP.GetShpInfo_Stop();
                        SHP.Dispose();
                        SHP = null;
                        PntsX = null;
                        PntsY = null;
                        Parts = null;
                        return "Error Reading Points Shape File.";
                    }
                    FirstGot = true;
                }
                else
                {
                    Res = SHP.GetShpInfo_Next(ref PntsCount,
                                              ref PartsCount);
                    if (!Res)
                        break;
                }

                SHP.GetShpInfo_Geom(0,
                                    0,
                                    PntsX,
                                    PntsY,
                                    Parts);

                sElevation = SHP.GetShpInfo_DBF(ELEVATION_FldIdx);
                if (!double.TryParse(sElevation, out Elevation))
                {
                    FldNames = null;
                    FldTypes = null;
                    FldLens = null;
                    FldDecs = null;
                    SHP.GetShpInfo_Stop();
                    SHP.Dispose();
                    SHP = null;
                    PntsX = null;
                    PntsY = null;
                    Parts = null;
                    return "Error Reading Points Shape File: Field '" + ELEVATION_FLD_NAME + "' is not a real number.";
                }

                m_CXs.Add(PntsX[0]);
                m_CYs.Add(PntsY[0]);
                m_CZs.Add(Elevation);
            }

            FldNames = null;
            FldTypes = null;
            FldLens = null;
            FldDecs = null;
            SHP.GetShpInfo_Stop();
            SHP.Dispose();
            SHP = null;
            PntsX = null;
            PntsY = null;
            Parts = null;

            return "OK";
        }

        /*=== LoadInfo_Centros_3D() ===*/
        private String LoadInfo_Centros_3D()
        {
            ShpLib SHP;
            bool Res;

            int ShapeType = 0;
            int RecCount = 0;
            double MinX = 0;
            double MinY = 0;
            double MaxX = 0;
            double MaxY = 0;

            int FldCount = 0;
            String[] FldNames = new String[256];
            String[] FldTypes = new String[256];
            int[] FldLens = new int[256];
            int[] FldDecs = new int[256];

            bool FirstGot = false;

            int PntsCount = 0;
            int PartsCount = 0;

            double[] PntsX;
            double[] PntsY;
            double[] PntsZ;
            int[] Parts;

            String PntFileName;

            PntFileName = Path.GetFileNameWithoutExtension(m_sImgsFile);
            PntFileName = Path.GetDirectoryName(m_sImgsFile) + "\\" +
                          PntFileName.Substring(0, PntFileName.Length - 5) + ".shp";

            //=============//
            // Read Header //
            //=============//
            SHP = new ShpLib();

            Res = SHP.GetGenInfo(PntFileName,

                                 ref ShapeType,
                                 ref RecCount,
                                 ref MinX,
                                 ref MinY,
                                 ref MaxX,
                                 ref MaxY,

                                 ref FldCount,
                                 FldNames,
                                 FldTypes,
                                 FldLens,
                                 FldDecs);
            if (!Res)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Points Shape File.";
            }

            if (ShapeType != ShpLib.SHPTYPE_POINTZ)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Points Shape File: Shape Type must be POINTZ.";
            }

            //=============//
            // Read Points //
            //=============//
            m_CXs = new List<double>();
            m_CYs = new List<double>();
            m_CZs = new List<double>();

            Res = SHP.GetShpInfo_StartZ(PntFileName,
                                        double.MinValue,
                                        double.MinValue,
                                        double.MinValue,
                                        double.MaxValue,
                                        double.MaxValue,
                                        double.MaxValue,
                                        false);
            if (!Res)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Reading Points Shape File.";
            }

            PntsX = new double[1];
            PntsY = new double[1];
            PntsZ = new double[1];
            Parts = new int[1];
            Parts[0] = 0;
            while (true)
            {
                if (!FirstGot)
                {
                    Res = SHP.GetShpInfo_FirstZ(ref PntsCount,
                                                ref PartsCount);
                    if (!Res)
                    {
                        FldNames = null;
                        FldTypes = null;
                        FldLens = null;
                        FldDecs = null;
                        SHP.GetShpInfo_Stop();
                        SHP.Dispose();
                        SHP = null;
                        PntsX = null;
                        PntsY = null;
                        PntsZ = null;
                        Parts = null;
                        return "Error Reading Points Shape File.";
                    }
                    FirstGot = true;
                }
                else
                {
                    Res = SHP.GetShpInfo_NextZ(ref PntsCount,
                                               ref PartsCount);
                    if (!Res)
                        break;
                }

                SHP.GetShpInfo_GeomZ(0,
                                     0,
                                     PntsX,
                                     PntsY,
                                     PntsZ,
                                     Parts);

                m_CXs.Add(PntsX[0]);
                m_CYs.Add(PntsY[0]);
                m_CZs.Add(PntsZ[0]);
            }

            FldNames = null;
            FldTypes = null;
            FldLens = null;
            FldDecs = null;
            SHP.GetShpInfo_Stop();
            SHP.Dispose();
            SHP = null;
            PntsX = null;
            PntsY = null;
            PntsZ = null;
            Parts = null;

            return "OK";
        }

        /*=== LoadInfo_Imgs() ===*/
        private String LoadInfo_Imgs()
        {
            ShpLib SHP;
            bool Res;

            int ShapeType = 0;
            int RecCount = 0;
            double MinX = 0;
            double MinY = 0;
            double MaxX = 0;
            double MaxY = 0;

            int FldCount = 0;
            int FldIdx;
            String[] FldNames = new String[256];
            String[] FldTypes = new String[256];
            int[] FldLens = new int[256];
            int[] FldDecs = new int[256];

            int OverLon_FldIdx = -1;
            int OverTrs_FldIdx = -1;
            int GSD_FldIdx = -1;

            bool FirstGot = false;

            int PntsCount = 0;
            int PartsCount = 0;
            int PntIdx;

            int RecIdx;
            double[] PntsX;
            double[] PntsY;
            double[] PntsZ;
            int[] Parts;
            int Alloced = 4;

            double X, Y, Z;

            String sOverLon, sOverTrs, sGSD;
            double OverLon, OverTrs, GSD;

            //=============//
            // Read Header //
            //=============//
            SHP = new ShpLib();

            Res = SHP.GetGenInfo(m_sImgsFile,

                                 ref ShapeType,
                                 ref RecCount,
                                 ref MinX,
                                 ref MinY,
                                 ref MaxX,
                                 ref MaxY,

                                 ref FldCount,
                                 FldNames,
                                 FldTypes,
                                 FldLens,
                                 FldDecs);
            if (!Res)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Lines Shape File.";
            }

            if (ShapeType != ShpLib.SHPTYPE_POLYGONZ)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Footprints: Shape Type must be POLYGONZ.";
            }

            for (FldIdx = 0; FldIdx < FldCount; FldIdx++)
            {
                if (FldNames[FldIdx].ToUpper() == OVER_LON_FLD_NAME)
                    OverLon_FldIdx = FldIdx;
                else
                if (FldNames[FldIdx].ToUpper() == OVER_TRS_FLD_NAME)
                    OverTrs_FldIdx = FldIdx;
                else
                if (FldNames[FldIdx].ToUpper() == GSD_FLD_NAME)
                    GSD_FldIdx = FldIdx;
            }
            if (OverLon_FldIdx == -1)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Footprints: Field '" + OVER_LON_FLD_NAME + "' does not exist.";
            }
            if (OverTrs_FldIdx == -1)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Footprints: Field '" + OVER_TRS_FLD_NAME + "' does not exist.";
            }
            if (GSD_FldIdx == -1)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Footprints: Field '" + GSD_FLD_NAME + "' does not exist.";
            }

            //=================//
            // Read Footprints //
            //=================//
            m_nMinX = double.MaxValue;
            m_nMinY = double.MaxValue;
            m_nMaxX = double.MinValue;
            m_nMaxY = double.MinValue;

            m_Xs = new List<double[]>();
            m_Ys = new List<double[]>();
            m_Zs = new List<double[]>();
            m_OverLon = new List<double>();
            m_OverTrs = new List<double>();
            m_GSD = new List<double>();

            Res = SHP.GetShpInfo_StartZ(m_sImgsFile,
                                        double.MinValue,
                                        double.MinValue,
                                        double.MinValue,
                                        double.MaxValue,
                                        double.MaxValue,
                                        double.MaxValue,
                                        true);
            if (!Res)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Reading Footprints.";
            }

            PntsX = new double[Alloced];
            PntsY = new double[Alloced];
            PntsZ = new double[Alloced];
            Parts = new int[10];
            Parts[0] = 0;
            while (true)
            {
                if (!FirstGot)
                {
                    Res = SHP.GetShpInfo_FirstZ(ref PntsCount,
                                                ref PartsCount);
                    if (!Res)
                    {
                        FldNames = null;
                        FldTypes = null;
                        FldLens = null;
                        FldDecs = null;
                        SHP.GetShpInfo_Stop();
                        SHP.Dispose();
                        SHP = null;
                        PntsX = null;
                        PntsY = null;
                        PntsZ = null;
                        Parts = null;
                        return "Error Reading Footprints.";
                    }
                    FirstGot = true;
                }
                else
                {
                    Res = SHP.GetShpInfo_NextZ(ref PntsCount,
                                               ref PartsCount);
                    if (!Res)
                        break;
                }

                if (PntsCount > Alloced)
                {
                    PntsX = null;
                    PntsY = null;
                    PntsZ = null;

                    Alloced = PntsCount + 100;
                    PntsX = new double[Alloced];
                    PntsY = new double[Alloced];
                    PntsZ = new double[Alloced];
                }

                SHP.GetShpInfo_GeomZ(0,
                                     0,
                                     PntsX,
                                     PntsY,
                                     PntsZ,
                                     Parts);

                sOverLon = SHP.GetShpInfo_DBF(OverLon_FldIdx);
                if (!double.TryParse(sOverLon, out OverLon))
                {
                    FldNames = null;
                    FldTypes = null;
                    FldLens = null;
                    FldDecs = null;
                    SHP.GetShpInfo_Stop();
                    SHP.Dispose();
                    SHP = null;
                    PntsX = null;
                    PntsY = null;
                    PntsZ = null;
                    Parts = null;
                    return "Error Reading Footprints: Field '" + OVER_LON_FLD_NAME + "' is not a real number.";
                }
                sOverTrs = SHP.GetShpInfo_DBF(OverTrs_FldIdx);
                if (!double.TryParse(sOverTrs, out OverTrs))
                {
                    FldNames = null;
                    FldTypes = null;
                    FldLens = null;
                    FldDecs = null;
                    SHP.GetShpInfo_Stop();
                    SHP.Dispose();
                    SHP = null;
                    PntsX = null;
                    PntsY = null;
                    PntsZ = null;
                    Parts = null;
                    return "Error Reading Footprints: Field '" + OVER_TRS_FLD_NAME + "' is not a real number.";
                }
                sGSD = SHP.GetShpInfo_DBF(GSD_FldIdx);
                if (!double.TryParse(sGSD, out GSD))
                {
                    FldNames = null;
                    FldTypes = null;
                    FldLens = null;
                    FldDecs = null;
                    SHP.GetShpInfo_Stop();
                    SHP.Dispose();
                    SHP = null;
                    PntsX = null;
                    PntsY = null;
                    PntsZ = null;
                    Parts = null;
                    return "Error Reading Footprints: Field '" + GSD_FLD_NAME + "' is not a real number.";
                }

                m_Xs.Add(new double[4]);
                m_Ys.Add(new double[4]);
                m_Zs.Add(new double[4]);
                RecIdx = m_Xs.Count - 1;
                m_OverLon.Add(OverLon);
                m_OverTrs.Add(OverTrs);
                m_GSD.Add(GSD);

                for (PntIdx = 0; PntIdx < 4; PntIdx++)
                {
                    X = PntsX[PntIdx];
                    Y = PntsY[PntIdx];
                    Z = PntsZ[PntIdx];

                    m_Xs[RecIdx][PntIdx] = X;
                    m_Ys[RecIdx][PntIdx] = Y;
                    m_Zs[RecIdx][PntIdx] = Z;

                    if (X < m_nMinX)
                        m_nMinX = X;
                    if (Y < m_nMinY)
                        m_nMinY = Y;
                    if (X > m_nMaxX)
                        m_nMaxX = X;
                    if (Y > m_nMaxY)
                        m_nMaxY = Y;
                }
            }

            FldNames = null;
            FldTypes = null;
            FldLens = null;
            FldDecs = null;
            SHP.GetShpInfo_Stop();
            SHP.Dispose();
            SHP = null;
            PntsX = null;
            PntsY = null;
            PntsZ = null;
            Parts = null;

            return "OK";
        }

        /*=== LoadInfo_Poly() ===*/
        private String LoadInfo_Poly()
        {
            ShpLib SHP;
            bool Res;

            int ShapeType = 0;
            int RecCount = 0;
            double MinX = 0;
            double MinY = 0;
            double MaxX = 0;
            double MaxY = 0;

            int FldCount = 0;
            String[] FldNames = new String[256];
            String[] FldTypes = new String[256];
            int[] FldLens = new int[256];
            int[] FldDecs = new int[256];

            bool FirstGot = false;

            int PntsCount = 0;
            int PartsCount = 0;
            int PntIdx;
            int PartIdx;
            int From, To;

            int RecIdx;
            double[] PntsX;
            double[] PntsY;
            int[] Parts;
            int AllocedPnts = 10000;
            int AllocedParts = 10;

            double X, Y;

            //=============//
            // Read Header //
            //=============//
            SHP = new ShpLib();

            Res = SHP.GetGenInfo(m_sPolyFile,

                                 ref ShapeType,
                                 ref RecCount,
                                 ref MinX,
                                 ref MinY,
                                 ref MaxX,
                                 ref MaxY,

                                 ref FldCount,
                                 FldNames,
                                 FldTypes,
                                 FldLens,
                                 FldDecs);
            if (!Res)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Polygon Shape File.";
            }

            if (!(ShapeType == ShpLib.SHPTYPE_POLYGON ||
                  ShapeType == ShpLib.SHPTYPE_POLYGONZ))
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Polygon: Shape Type must be POLYGON or POLYGONZ.";
            }

            //===============//
            // Read Polygons //
            //===============//
            m_PXs = new List<List<List<double>>>();
            m_PYs = new List<List<List<double>>>();

            Res = SHP.GetShpInfo_Start(m_sPolyFile,
                                       m_nMinX,
                                       m_nMinY,
                                       m_nMaxX,
                                       m_nMaxY,
                                       false);
            if (!Res)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Reading Polygon.";
            }

            PntsX = new double[AllocedPnts];
            PntsY = new double[AllocedPnts];
            Parts = new int[AllocedParts];
            while (true)
            {
                if (!FirstGot)
                {
                    Res = SHP.GetShpInfo_First(ref PntsCount,
                                               ref PartsCount);
                    if (!Res)
                    {
                        FldNames = null;
                        FldTypes = null;
                        FldLens = null;
                        FldDecs = null;
                        SHP.GetShpInfo_Stop();
                        SHP.Dispose();
                        SHP = null;
                        PntsX = null;
                        PntsY = null;
                        Parts = null;
                        return "Error Reading Polygon.";
                    }
                    FirstGot = true;
                }
                else
                {
                    Res = SHP.GetShpInfo_Next(ref PntsCount,
                                              ref PartsCount);
                    if (!Res)
                        break;
                }

                if (PntsCount > AllocedPnts)
                {
                    PntsX = null;
                    PntsY = null;

                    AllocedPnts = PntsCount + 100;
                    PntsX = new double[AllocedPnts];
                    PntsY = new double[AllocedPnts];
                }
                if (PartsCount > AllocedParts)
                {
                    Parts = null;

                    AllocedParts = PartsCount + 10;
                    Parts = new int[AllocedParts];
                }

                SHP.GetShpInfo_Geom(0,
                                    0,
                                    PntsX,
                                    PntsY,
                                    Parts);

                m_PXs.Add(new List<List<double>>());
                m_PYs.Add(new List<List<double>>());
                RecIdx = m_PXs.Count - 1;

                for (PartIdx = 0; PartIdx < PartsCount; PartIdx++)
                {
                    From = Parts[PartIdx];
                    if (PartIdx < PartsCount - 1)
                        To = Parts[PartIdx + 1];
                    else
                        To = PntsCount;
                    m_PXs[RecIdx].Add(new List<double>());
                    m_PYs[RecIdx].Add(new List<double>());
                    for (PntIdx = From; PntIdx < To; PntIdx++)
                    {
                        X = PntsX[PntIdx];
                        Y = PntsY[PntIdx];

                        m_PXs[RecIdx][PartIdx].Add(X);
                        m_PYs[RecIdx][PartIdx].Add(Y);
                    }
                }
            }

            FldNames = null;
            FldTypes = null;
            FldLens = null;
            FldDecs = null;
            SHP.GetShpInfo_Stop();
            SHP.Dispose();
            SHP = null;
            PntsX = null;
            PntsY = null;
            Parts = null;

            return "OK";
        }

        #endregion Private Load Info

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private MainTB Events ---------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private MainTB Events

        /*=== VarCB_SelectedIndexChanged() ===*/
        private void VarCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            double zc, z1, z2, z3, z4, zavg, height;
            String val;

            if (LowHighCB.SelectedIndex == 0)
            {
                if (VarCB.SelectedIndex == 0)
                    CritCB.Items[1] = "Limits " + String.Format("[{0:0}, {1:0}]%", OVER_LON_MIN_LOW, OVER_LON_MAX_LOW);
                else
                if (VarCB.SelectedIndex == 1)
                    CritCB.Items[1] = "Limits " + String.Format("[{0:0}, {1:0}]%", OVER_TRS_MIN_LOW, OVER_TRS_MAX_LOW);
                else
                    CritCB.Items[1] = "Limits " + String.Format("[{0:0}, {1:0}]cm", GSD_MIN_LOW, GSD_MAX_LOW);
            }
            else
            {
                if (VarCB.SelectedIndex == 0)
                    CritCB.Items[1] = "Limits " + String.Format("[{0:0}, {1:0}]%", OVER_LON_MIN_HIGH, OVER_LON_MAX_HIGH);
                else
                if (VarCB.SelectedIndex == 1)
                    CritCB.Items[1] = "Limits " + String.Format("[{0:0}, {1:0}]%", OVER_TRS_MIN_HIGH, OVER_TRS_MAX_HIGH);
                else
                    CritCB.Items[1] = "Limits " + String.Format("[{0:0}, {1:0}]cm", GSD_MIN_HIGH, GSD_MAX_HIGH);
            }

            if (m_nSelIdx != -1 && m_bStatsExist)
            {
                zc = m_CZs[m_nSelIdx];
                z1 = m_Zs[m_nSelIdx][0];
                z2 = m_Zs[m_nSelIdx][1];
                z3 = m_Zs[m_nSelIdx][2];
                z4 = m_Zs[m_nSelIdx][3];
                zavg = 0.25 * (z1 + z2 + z3 + z4);
                height = zc - zavg;

                if (VarCB.SelectedIndex == 0)
                    val = String.Format("{0:0.00}%", m_OverLon[m_nSelIdx]);
                else
                if (VarCB.SelectedIndex == 1)
                    val = String.Format("{0:0.00}%", m_OverTrs[m_nSelIdx]);
                else
                    val = String.Format("{0:0.00}cm", m_GSD[m_nSelIdx]);

                InfoSL.Text = String.Format("Z = {0:0.00}, Height = {1:0.00} " +
                                            "(zavg = {2:0.00}, zs = ({3:0.00}, {4:0.00}, {5:0.00}, {6:0.00})) " +
                                            ", Var Value = ",
                                            zc, height, zavg, z1, z2, z3, z4) + val;
                MainDP.Invalidate();
            }

            MainDP.Invalidate();
        }

        /*=== CritCB_SelectedIndexChanged() ===*/
        private void CritCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            MainDP.Invalidate();
        }

        /*=== LowHighCB_SelectedIndexChanged() ===*/
        private void LowHighCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            VarCB_SelectedIndexChanged((object)VarCB, e);

            MainDP.Invalidate();
        }

        /*=== ViewFPsTB_Click() ===*/
        private void ViewFPsTB_Click(object sender, EventArgs e)
        {
            ViewFPsTB.Checked = !ViewFPsTB.Checked;
            MainDP.Invalidate();
        }

        /*=== ViewPtsTB_Click() ===*/
        private void ViewPtsTB_Click(object sender, EventArgs e)
        {
            ViewPtsTB.Checked = !ViewPtsTB.Checked;
            MainDP.Invalidate();
        }

        /*=== ViewPolyTB_Click() ===*/
        private void ViewPolyTB_Click(object sender, EventArgs e)
        {
            ViewPolyTB.Checked = !ViewPolyTB.Checked;
            MainDP.Invalidate();
        }

        /*=== ZoomAllTB_Click() ===*/
        private void ZoomAllTB_Click(object sender, EventArgs e)
        {
            GetZoomAll();
            MainDP.Invalidate();
        }

        /*=== ZoomCenteredTB_Click() ===*/
        private void ZoomCenteredTB_Click(object sender, EventArgs e)
        {
            ZoomCenteredTB.Checked = !ZoomCenteredTB.Checked;
        }

        /*=== HelpTB_Click() ===*/
        private void HelpTB_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("ViewerHelp.pdf");
        }

        #endregion Private MainTB Events

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private MainDP Events ---------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private MainDP Events

        /*=== MainDP_Resize() ===*/
        private void MainDP_Resize(object sender, EventArgs e)
        {
            if (!m_bStatsExist)
                return;

            m_nWrldToPix = Math.Min((double)(MainDP.Width - 20) / (m_nMaxX - m_nMinX),
                                    (double)(MainDP.Height - 20) / (m_nMaxY - m_nMinY));
            MainDP.Invalidate();
        }

        /*=== MainDP_MouseDown() ===*/
        private void MainDP_MouseDown(object sender, MouseEventArgs e)
        {
            int xs, ys;
            double xw, yw;

            if (!m_bStatsExist)
                return;

            xs = e.X;
            ys = e.Y;

            if (!ScrnToWrld(xs,
                            ys,
                            out xw,
                            out yw))
                return;

            if (e.Button == MouseButtons.Left)
            {
                m_bMouseDown = true;
                m_nIniX = e.X;
                m_nIniY = e.Y;
                m_nPrevX = e.X;
                m_nPrevY = e.Y;
            }
            else
            if (e.Button == MouseButtons.Right)
            {
                DoIdentify(e.X, e.Y);
            }
        }

        /*=== MainDP_MouseMove() ===*/
        private void MainDP_MouseMove(object sender, MouseEventArgs e)
        {
            if (!m_bStatsExist)
                return;

            int xs, ys;
            double xw, yw;

            xs = e.X;
            ys = e.Y;

            if (!ScrnToWrld(xs,
                            ys,
                            out xw,
                            out yw))
            {
                CoordsSL.Text = "";
                return;
            }

            if (m_bIsFlight)
                CoordsSL.Text = String.Format("({0:0.00}, {1:0.00})",
                                              xw, yw);
            else
                CoordsSL.Text = String.Format("({0:0.000000}, {1:0.000000})",
                                              xw, yw);

            MainDP.Focus();

            if (m_bMouseDown)
            {
                m_nWrldCenterX -= (e.X - m_nPrevX) / m_nWrldToPix;
                m_nWrldCenterY += (e.Y - m_nPrevY) / m_nWrldToPix;
                m_nPrevX = e.X;
                m_nPrevY = e.Y;
                MainDP.Invalidate();
            }
        }

        /*=== MainDP_MouseUp() ===*/
        private void MainDP_MouseUp(object sender, MouseEventArgs e)
        {
            if (!m_bStatsExist)
                return;

            if (m_bMouseDown)
            {
                if (Math.Abs(e.X - m_nIniX) < MOUSE_DELTA &&
                    Math.Abs(e.Y - m_nIniY) < MOUSE_DELTA)
                {
                    m_nWrldCenterX += (e.X - MainDP.Width / 2) / m_nWrldToPix;
                    m_nWrldCenterY -= (e.Y - MainDP.Height / 2) / m_nWrldToPix;
                }

                m_bMouseDown = false;
                m_nIniX = 0;
                m_nIniY = 0;
                m_nPrevX = 0;
                m_nPrevY = 0;
            }

            MainDP.Invalidate();
        }

        /*=== MainDP_MouseWheel() ===*/
        private void MainDP_MouseWheel(object sender, MouseEventArgs e)
        {
            int xs, ys, newxs, newys;
            double xw, yw;

            xs = e.X;
            ys = e.Y;

            if (!ScrnToWrld(xs,
                            ys,
                            out xw,
                            out yw))
            {
                CoordsSL.Text = "";
                return;
            }

            if (e.Delta > 0)
                m_nWrldToPix *= 1.2;
            else
                m_nWrldToPix /= 1.2;

            if (ZoomCenteredTB.Checked)
            {
                WrldToScrn(xw,
                           yw,
                           out newxs,
                           out newys);
                ScrnToWrld(MainDP.Width / 2 - xs + newxs,
                           MainDP.Height / 2 - ys + newys,
                           out xw,
                           out yw);
                m_nWrldCenterX = xw;
                m_nWrldCenterY = yw;
            }

            MainDP.Invalidate();
        }

        /*=== MainDP_MouseLeave() ===*/
        private void MainDP_MouseLeave(object sender, EventArgs e)
        {
            if (!m_bStatsExist)
                return;

            CoordsSL.Text = "";
        }

        /*=== MainDP_Paint() ===*/
        private void MainDP_Paint(object sender, PaintEventArgs e)
        {
            if (!m_bStatsExist)
                return;

            DoPaint(e.Graphics);
        }

        #endregion Private MainDP Events

        /*--------------------------------------------------------------------*/
        /*------------------------ Private Pens and Brushes Stuff ------------*/
        /*--------------------------------------------------------------------*/
        #region Private Pens and Brushes Stuff

        /*=== CreatePensAndBrushes() ===*/
        private void CreatePensAndBrushes()
        {
            m_PenGreen = new Pen(Color.DarkGreen, 2.0f);
            m_PenBlue = new Pen(Color.Blue, 2.0f);
            m_PenBrown = new Pen(Color.DarkRed, 2.0f);
            m_PenRed = new Pen(Color.Red, 2.0f);
            m_PenGray = new Pen(Color.Gray, 2.0f);
            m_PenSel = new Pen(Color.Cyan, 3.0f);
            m_PenPoly = new Pen(Color.Black, 2.0f);

            m_DashPenGreen = new Pen(Color.DarkGreen, 1.0f);
            m_DashPenGreen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            m_DashPenBlue = new Pen(Color.Blue, 1.0f);
            m_DashPenBlue.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            m_DashPenBrown = new Pen(Color.DarkRed, 1.0f);
            m_DashPenBrown.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            m_DashPenRed = new Pen(Color.Red, 1.0f);
            m_DashPenRed.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            m_DashPenGray = new Pen(Color.Gray, 1.0f);
            m_DashPenGray.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;

            m_BrushGreen = new SolidBrush(Color.DarkGreen);
            m_BrushBlue = new SolidBrush(Color.Blue);
            m_BrushBrown = new SolidBrush(Color.DarkRed);
            m_BrushRed = new SolidBrush(Color.Red);
            m_BrushGray = new SolidBrush(Color.Gray);
        }

        /*=== ClearPensAndBrushes() ===*/
        private void ClearPensAndBrushes()
        {
            if (m_PenGreen != null)
            {
                m_PenGreen.Dispose();
                m_PenGreen = null;
            }
            if (m_PenBlue != null)
            {
                m_PenBlue.Dispose();
                m_PenBlue = null;
            }
            if (m_PenBrown != null)
            {
                m_PenBrown.Dispose();
                m_PenBrown = null;
            }
            if (m_PenRed != null)
            {
                m_PenRed.Dispose();
                m_PenRed = null;
            }
            if (m_PenGray != null)
            {
                m_PenGray.Dispose();
                m_PenGray = null;
            }
            if (m_PenSel != null)
            {
                m_PenSel.Dispose();
                m_PenSel = null;
            }
            if (m_PenPoly != null)
            {
                m_PenPoly.Dispose();
                m_PenPoly = null;
            }

            if (m_DashPenGreen != null)
            {
                m_DashPenGreen.Dispose();
                m_DashPenGreen = null;
            }
            if (m_DashPenBlue != null)
            {
                m_DashPenBlue.Dispose();
                m_DashPenBlue = null;
            }
            if (m_DashPenBrown != null)
            {
                m_DashPenBrown.Dispose();
                m_DashPenBrown = null;
            }
            if (m_DashPenRed != null)
            {
                m_DashPenRed.Dispose();
                m_DashPenRed = null;
            }
            if (m_DashPenGray != null)
            {
                m_DashPenGray.Dispose();
                m_DashPenGray = null;
            }

            if (m_BrushGreen != null)
            {
                m_BrushGreen.Dispose();
                m_BrushGreen = null;
            }
            if (m_BrushBlue != null)
            {
                m_BrushBlue.Dispose();
                m_BrushBlue = null;
            }
            if (m_BrushBrown != null)
            {
                m_BrushBrown.Dispose();
                m_BrushBrown = null;
            }
            if (m_BrushRed != null)
            {
                m_BrushRed.Dispose();
                m_BrushRed = null;
            }
            if (m_BrushGray != null)
            {
                m_BrushGray.Dispose();
                m_BrushGray = null;
            }
        }

        #endregion Private Pens and Brushes Stuff

        /*--------------------------------------------------------------------*/
        /*------------------------ Private WrldScrn Stuff --------------------*/
        /*--------------------------------------------------------------------*/
        #region Private WrldScrn Stuff

        /*=== WrldToScrn() ===*/
        private void WrldToScrn(double xw,
                                double yw,
                                out int xs,
                                out int ys)
        {
            xs = 0;
            ys = 0;

            if (!m_bStatsExist)
                return;

            xs = MainDP.Width / 2 +
                 (int)((xw - m_nWrldCenterX) * m_nWrldToPix);
            ys = MainDP.Height / 2 -
                 (int)((yw - m_nWrldCenterY) * m_nWrldToPix);
        }

        /*=== ScrnToWrld() ===*/
        private bool ScrnToWrld(int xs,
                                int ys,
                                out double xw,
                                out double yw)
        {
            xw = 0;
            yw = 0;

            if (!m_bStatsExist)
                return false;
            if (m_nWrldToPix == 0)
                return false;

            xw = m_nWrldCenterX + (double)(xs - MainDP.Width / 2) / m_nWrldToPix;
            yw = m_nWrldCenterY + (double)(MainDP.Height / 2 - ys) / m_nWrldToPix;

            return true;
        }

        #endregion Private WrldScrn Stuff

        /*--------------------------------------------------------------------*/
        /*------------------------ Private Zoom Stuff ------------------------*/
        /*--------------------------------------------------------------------*/
        #region Private Zoom Stuff

        /*=== GetZoomAll() ===*/
        private void GetZoomAll()
        {
            if (!m_bStatsExist)
                return;

            m_nWrldCenterX = 0.5 * (m_nMinX + m_nMaxX);
            m_nWrldCenterY = 0.5 * (m_nMinY + m_nMaxY);
            m_nWrldToPix = Math.Min((double)(MainDP.Width - 20) / (m_nMaxX - m_nMinX),
                                    (double)(MainDP.Height - 20) / (m_nMaxY - m_nMinY));
        }

        #endregion Private Zoom Stuff

        /*--------------------------------------------------------------------*/
        /*------------------------ Private Paint Stuff -----------------------*/
        /*--------------------------------------------------------------------*/
        #region Private Paint Stuff

        /*=== DoPaint() ===*/
        private void DoPaint(Graphics g)
        {
            if (ViewFPsTB.Checked)
                DoPaint_Footprints(g);
            if (ViewPtsTB.Checked)
                DoPaint_Centers(g);
            if (m_bPolyExists && ViewPolyTB.Checked)
                DoPaint_Polygon(g);
        }

        /*=== DoPaint_Footprints() ===*/
        private void DoPaint_Footprints(Graphics g)
        {
            int i, Count;
            double xw, yw;
            int x1, y1, x2, y2, x3, y3, x4, y4;
            Pen p, dashp, seldashp = null;

            Count = m_Xs.Count;
            for (i = 0; i < Count; i++)
            {
                DoPaint_GetPens(i,
                                out p,
                                out dashp);

                if (i == m_nSelIdx)
                {
                    seldashp = dashp;
                    continue;
                }

                xw = m_Xs[i][0];
                yw = m_Ys[i][0];
                WrldToScrn(xw,
                           yw,
                           out x1,
                           out y1);
                xw = m_Xs[i][1];
                yw = m_Ys[i][1];
                WrldToScrn(xw,
                           yw,
                           out x2,
                           out y2);
                xw = m_Xs[i][2];
                yw = m_Ys[i][2];
                WrldToScrn(xw,
                           yw,
                           out x3,
                           out y3);
                xw = m_Xs[i][3];
                yw = m_Ys[i][3];
                WrldToScrn(xw,
                           yw,
                           out x4,
                           out y4);

                g.DrawLine(p, x1, y1, x2, y2);
                g.DrawLine(p, x2, y2, x3, y3);
                g.DrawLine(p, x3, y3, x4, y4);
                g.DrawLine(dashp, x1, y1, x4, y4);
            }

            if (m_nSelIdx != -1)
            {
                i = m_nSelIdx;

                xw = m_Xs[i][0];
                yw = m_Ys[i][0];
                WrldToScrn(xw,
                           yw,
                           out x1,
                           out y1);
                xw = m_Xs[i][1];
                yw = m_Ys[i][1];
                WrldToScrn(xw,
                           yw,
                           out x2,
                           out y2);
                xw = m_Xs[i][2];
                yw = m_Ys[i][2];
                WrldToScrn(xw,
                           yw,
                           out x3,
                           out y3);
                xw = m_Xs[i][3];
                yw = m_Ys[i][3];
                WrldToScrn(xw,
                           yw,
                           out x4,
                           out y4);

                g.DrawLine(m_PenSel, x1, y1, x2, y2);
                g.DrawLine(m_PenSel, x2, y2, x3, y3);
                g.DrawLine(m_PenSel, x3, y3, x4, y4);

                g.DrawLine(seldashp, x1, y1, x4, y4);
            }
        }

        /*=== DoPaint_Centers() ===*/
        private void DoPaint_Centers(Graphics g)
        {
            int i, Count;
            double xw, yw;
            int x, y;
            Brush b;

            Count = m_CXs.Count;
            for (i = 0; i < Count; i++)
            {
                DoPaint_GetBrush(i,
                                 out b);

                xw = m_CXs[i];
                yw = m_CYs[i];
                WrldToScrn(xw,
                           yw,
                           out x,
                           out y);

                g.FillEllipse(b, (float)(x - 3), (float)(y - 3), 7f, 7f);
                if (i != m_nSelIdx)
                    g.DrawEllipse(Pens.Gray, (float)(x - 3), (float)(y - 3), 7f, 7f);
                else
                    g.DrawEllipse(m_PenSel, (float)(x - 3), (float)(y - 3), 7f, 7f);
            }
        }

        /*=== DoPaint_Polygon() ===*/
        private void DoPaint_Polygon(Graphics g)
        {
            int RecCount, RecIdx, PartCount, PartIdx, PntCount, PntIdx;
            double xw, yw;
            int x, y, prevx = 0, prevy = 0;

            RecCount = m_PXs.Count;
            for (RecIdx = 0; RecIdx < RecCount; RecIdx++)
            {
                PartCount = m_PXs[RecIdx].Count;
                for (PartIdx = 0; PartIdx < PartCount; PartIdx++)
                {
                    PntCount = m_PXs[RecIdx][PartIdx].Count;
                    for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
                    {
                        xw = m_PXs[RecIdx][PartIdx][PntIdx];
                        yw = m_PYs[RecIdx][PartIdx][PntIdx];
                        WrldToScrn(xw,
                                   yw,
                                   out x,
                                   out y);

                        if (PntIdx > 0)
                            g.DrawLine(m_PenPoly, x, y, prevx, prevy);

                        prevx = x;
                        prevy = y;
                    }
                }
            }
        }

        /*=== DoPaint_GetPens() ===*/
        private void DoPaint_GetPens(int Idx,
                                     out Pen p,
                                     out Pen pdash)
        {
            p = null;
            pdash = null;

            if (CritCB.SelectedIndex == 0)
            {
                if (VarCB.SelectedIndex == 0)
                {
                    if (m_OverLon[Idx] >= m_nOverLon_Avg - 2.0 * m_nOverLon_Std &&
                        m_OverLon[Idx] <= m_nOverLon_Avg + 2.0 * m_nOverLon_Std)
                    {
                        p = m_PenGreen;
                        pdash = m_DashPenGreen;
                    }
                    else
                    if (m_OverLon[Idx] >= m_nOverLon_Avg - 2.5 * m_nOverLon_Std &&
                        m_OverLon[Idx] <= m_nOverLon_Avg + 2.5 * m_nOverLon_Std)
                    {
                        p = m_PenBlue;
                        pdash = m_DashPenBlue;
                    }
                    else
                    if (m_OverLon[Idx] >= m_nOverLon_Avg - 3.0 * m_nOverLon_Std &&
                        m_OverLon[Idx] <= m_nOverLon_Avg + 3.0 * m_nOverLon_Std)
                    {
                        p = m_PenBrown;
                        pdash = m_DashPenBrown;
                    }
                    else
                    if (m_OverLon[Idx] > 0)
                    {
                        p = m_PenRed;
                        pdash = m_DashPenRed;
                    }
                    else
                    {
                        p = m_PenGray;
                        pdash = m_DashPenGray;
                    }
                }
                else
                if (VarCB.SelectedIndex == 1)
                {
                    if (m_OverTrs[Idx] >= m_nOverTrs_Avg - 2.0 * m_nOverTrs_Std &&
                        m_OverTrs[Idx] <= m_nOverTrs_Avg + 2.0 * m_nOverTrs_Std)
                    {
                        p = m_PenGreen;
                        pdash = m_DashPenGreen;
                    }
                    else
                    if (m_OverTrs[Idx] >= m_nOverTrs_Avg - 2.5 * m_nOverTrs_Std &&
                        m_OverTrs[Idx] <= m_nOverTrs_Avg + 2.5 * m_nOverTrs_Std)
                    {
                        p = m_PenBlue;
                        pdash = m_DashPenBlue;
                    }
                    else
                    if (m_OverTrs[Idx] >= m_nOverTrs_Avg - 3.0 * m_nOverTrs_Std &&
                        m_OverTrs[Idx] <= m_nOverTrs_Avg + 3.0 * m_nOverTrs_Std)
                    {
                        p = m_PenBrown;
                        pdash = m_DashPenBrown;
                    }
                    else
                    if (m_OverTrs[Idx] > 0)
                    {
                        p = m_PenRed;
                        pdash = m_DashPenRed;
                    }
                    else
                    {
                        p = m_PenGray;
                        pdash = m_DashPenGray;
                    }
                }
                else
                {
                    if (m_GSD[Idx] >= m_nGSD_Avg - 2.0 * m_nGSD_Std &&
                        m_GSD[Idx] <= m_nGSD_Avg + 2.0 * m_nGSD_Std)
                    {
                        p = m_PenGreen;
                        pdash = m_DashPenGreen;
                    }
                    else
                    if (m_GSD[Idx] >= m_nGSD_Avg - 2.5 * m_nGSD_Std &&
                        m_GSD[Idx] <= m_nGSD_Avg + 2.5 * m_nGSD_Std)
                    {
                        p = m_PenBlue;
                        pdash = m_DashPenBlue;
                    }
                    else
                    if (m_GSD[Idx] >= m_nGSD_Avg - 3.0 * m_nGSD_Std &&
                        m_GSD[Idx] <= m_nGSD_Avg + 3.0 * m_nGSD_Std)
                    {
                        p = m_PenBrown;
                        pdash = m_DashPenBrown;
                    }
                    else
                    if (m_GSD[Idx] > 0)
                    {
                        p = m_PenRed;
                        pdash = m_DashPenRed;
                    }
                    else
                    {
                        p = m_PenGray;
                        pdash = m_DashPenGray;
                    }
                }
            }
            else
            {
                if (LowHighCB.SelectedIndex == 0)
                {
                    if (VarCB.SelectedIndex == 0)
                    {
                        if (m_OverLon[Idx] >= OVER_LON_MIN_LOW &&
                            m_OverLon[Idx] <= OVER_LON_MAX_LOW)
                        {
                            p = m_PenGreen;
                            pdash = m_DashPenGreen;
                        }
                        else
                        if (m_OverLon[Idx] > 0)
                        {
                            p = m_PenRed;
                            pdash = m_DashPenRed;
                        }
                        else
                        {
                            p = m_PenGray;
                            pdash = m_DashPenGray;
                        }
                    }
                    else
                    if (VarCB.SelectedIndex == 1)
                    {
                        if (m_OverTrs[Idx] >= OVER_TRS_MIN_LOW &&
                            m_OverTrs[Idx] <= OVER_TRS_MAX_LOW)
                        {
                            p = m_PenGreen;
                            pdash = m_DashPenGreen;
                        }
                        else
                        if (m_OverTrs[Idx] > 0)
                        {
                            p = m_PenRed;
                            pdash = m_DashPenRed;
                        }
                        else
                        {
                            p = m_PenGray;
                            pdash = m_DashPenGray;
                        }
                    }
                    else
                    {
                        if (m_GSD[Idx] >= GSD_MIN_LOW &&
                            m_GSD[Idx] <= GSD_MAX_LOW)
                        {
                            p = m_PenGreen;
                            pdash = m_DashPenGreen;
                        }
                        else
                        if (m_GSD[Idx] > 0)
                        {
                            p = m_PenRed;
                            pdash = m_DashPenRed;
                        }
                        else
                        {
                            p = m_PenGray;
                            pdash = m_DashPenGray;
                        }
                    }
                }
                else
                {
                    if (VarCB.SelectedIndex == 0)
                    {
                        if (m_OverLon[Idx] >= OVER_LON_MIN_HIGH &&
                            m_OverLon[Idx] <= OVER_LON_MAX_HIGH)
                        {
                            p = m_PenGreen;
                            pdash = m_DashPenGreen;
                        }
                        else
                        if (m_OverLon[Idx] > 0)
                        {
                            p = m_PenRed;
                            pdash = m_DashPenRed;
                        }
                        else
                        {
                            p = m_PenGray;
                            pdash = m_DashPenGray;
                        }
                    }
                    else
                    if (VarCB.SelectedIndex == 1)
                    {
                        if (m_OverTrs[Idx] >= OVER_TRS_MIN_HIGH &&
                            m_OverTrs[Idx] <= OVER_TRS_MAX_HIGH)
                        {
                            p = m_PenGreen;
                            pdash = m_DashPenGreen;
                        }
                        else
                        if (m_OverTrs[Idx] > 0)
                        {
                            p = m_PenRed;
                            pdash = m_DashPenRed;
                        }
                        else
                        {
                            p = m_PenGray;
                            pdash = m_DashPenGray;
                        }
                    }
                    else
                    {
                        if (m_GSD[Idx] >= GSD_MIN_HIGH &&
                            m_GSD[Idx] <= GSD_MAX_HIGH)
                        {
                            p = m_PenGreen;
                            pdash = m_DashPenGreen;
                        }
                        else
                        if (m_GSD[Idx] > 0)
                        {
                            p = m_PenRed;
                            pdash = m_DashPenRed;
                        }
                        else
                        {
                            p = m_PenGray;
                            pdash = m_DashPenGray;
                        }
                    }
                }
            }
        }

        /*=== DoPaint_GetBrush() ===*/
        private void DoPaint_GetBrush(int Idx,
                                      out Brush b)
        {
            b = null;

            if (CritCB.SelectedIndex == 0)
            {
                if (VarCB.SelectedIndex == 0)
                {
                    if (m_OverLon[Idx] >= m_nOverLon_Avg - 2.0 * m_nOverLon_Std &&
                        m_OverLon[Idx] <= m_nOverLon_Avg + 2.0 * m_nOverLon_Std)
                        b = m_BrushGreen;
                    else
                    if (m_OverLon[Idx] >= m_nOverLon_Avg - 2.5 * m_nOverLon_Std &&
                        m_OverLon[Idx] <= m_nOverLon_Avg + 2.5 * m_nOverLon_Std)
                        b = m_BrushBlue;
                    else
                    if (m_OverLon[Idx] >= m_nOverLon_Avg - 3.0 * m_nOverLon_Std &&
                        m_OverLon[Idx] <= m_nOverLon_Avg + 3.0 * m_nOverLon_Std)
                        b = m_BrushBrown;
                    else
                    if (m_OverLon[Idx] > 0)
                        b = m_BrushRed;
                    else
                        b = m_BrushGray;
                }
                else
                if (VarCB.SelectedIndex == 1)
                {
                    if (m_OverTrs[Idx] >= m_nOverTrs_Avg - 2.0 * m_nOverTrs_Std &&
                        m_OverTrs[Idx] <= m_nOverTrs_Avg + 2.0 * m_nOverTrs_Std)
                        b = m_BrushGreen;
                    else
                    if (m_OverTrs[Idx] >= m_nOverTrs_Avg - 2.5 * m_nOverTrs_Std &&
                        m_OverTrs[Idx] <= m_nOverTrs_Avg + 2.5 * m_nOverTrs_Std)
                        b = m_BrushBlue;
                    else
                    if (m_OverTrs[Idx] >= m_nOverTrs_Avg - 3.0 * m_nOverTrs_Std &&
                        m_OverTrs[Idx] <= m_nOverTrs_Avg + 3.0 * m_nOverTrs_Std)
                        b = m_BrushBrown;
                    else
                    if (m_OverTrs[Idx] > 0)
                        b = m_BrushRed;
                    else
                        b = m_BrushGray;
                }
                else
                {
                    if (m_GSD[Idx] >= m_nGSD_Avg - 2.0 * m_nGSD_Std &&
                        m_GSD[Idx] <= m_nGSD_Avg + 2.0 * m_nGSD_Std)
                        b = m_BrushGreen;
                    else
                    if (m_GSD[Idx] >= m_nGSD_Avg - 2.5 * m_nGSD_Std &&
                        m_GSD[Idx] <= m_nGSD_Avg + 2.5 * m_nGSD_Std)
                        b = m_BrushBlue;
                    else
                    if (m_GSD[Idx] >= m_nGSD_Avg - 3.0 * m_nGSD_Std &&
                        m_GSD[Idx] <= m_nGSD_Avg + 3.0 * m_nGSD_Std)
                        b = m_BrushBrown;
                    else
                    if (m_GSD[Idx] > 0)
                        b = m_BrushRed;
                    else
                        b = m_BrushGray;
                }
            }
            else
            {
                if (LowHighCB.SelectedIndex == 0)
                {
                    if (VarCB.SelectedIndex == 0)
                    {
                        if (m_OverLon[Idx] >= OVER_LON_MIN_LOW &&
                            m_OverLon[Idx] <= OVER_LON_MAX_LOW)
                            b = m_BrushGreen;
                        else
                        if (m_OverLon[Idx] > 0)
                            b = m_BrushRed;
                        else
                            b = m_BrushGray;
                    }
                    else
                    if (VarCB.SelectedIndex == 1)
                    {
                        if (m_OverTrs[Idx] >= OVER_TRS_MIN_LOW &&
                            m_OverTrs[Idx] <= OVER_TRS_MAX_LOW)
                            b = m_BrushGreen;
                        else
                        if (m_OverTrs[Idx] > 0)
                            b = m_BrushRed;
                        else
                            b = m_BrushGray;
                    }
                    else
                    {
                        if (m_GSD[Idx] >= GSD_MIN_LOW &&
                            m_GSD[Idx] <= GSD_MAX_LOW)
                            b = m_BrushGreen;
                        else
                        if (m_GSD[Idx] > 0)
                            b = m_BrushRed;
                        else
                            b = m_BrushGray;
                    }
                }
                else
                {
                    if (VarCB.SelectedIndex == 0)
                    {
                        if (m_OverLon[Idx] >= OVER_LON_MIN_HIGH &&
                            m_OverLon[Idx] <= OVER_LON_MAX_HIGH)
                            b = m_BrushGreen;
                        else
                        if (m_OverLon[Idx] > 0)
                            b = m_BrushRed;
                        else
                            b = m_BrushGray;
                    }
                    else
                    if (VarCB.SelectedIndex == 1)
                    {
                        if (m_OverTrs[Idx] >= OVER_TRS_MIN_HIGH &&
                            m_OverTrs[Idx] <= OVER_TRS_MAX_HIGH)
                            b = m_BrushGreen;
                        else
                        if (m_OverTrs[Idx] > 0)
                            b = m_BrushRed;
                        else
                            b = m_BrushGray;
                    }
                    else
                    {
                        if (m_GSD[Idx] >= GSD_MIN_HIGH &&
                            m_GSD[Idx] <= GSD_MAX_HIGH)
                            b = m_BrushGreen;
                        else
                        if (m_GSD[Idx] > 0)
                            b = m_BrushRed;
                        else
                            b = m_BrushGray;
                    }
                }
            }
        }

        #endregion Private Paint Stuff

        /*--------------------------------------------------------------------*/
        /*------------------------ Private Identify Stuff --------------------*/
        /*--------------------------------------------------------------------*/
        #region Private Identify Stuff

        /*=== DoIdentify() ===*/
        private void DoIdentify(int X,
                                int Y)
        {
            int i, Count, Idx;
            double xw, yw;
            int x, y;
            double zc, z1, z2, z3, z4, zavg, height;
            String val;

            Count = m_CXs.Count;
            Idx = -1;
            for (i = 0; i < Count; i++)
            {
                xw = m_CXs[i];
                yw = m_CYs[i];
                WrldToScrn(xw,
                           yw,
                           out x,
                           out y);
                if (Math.Abs(X - x) < MOUSE_DELTA && Math.Abs(Y - y) < MOUSE_DELTA)
                {
                    Idx = i;
                    break;
                }
            }
            if (Idx == -1)
            {
                InfoSL.Text = "";
                m_nSelIdx = -1;
                MainDP.Invalidate();
                return;
            }

            zc = m_CZs[Idx];
            z1 = m_Zs[Idx][0];
            z2 = m_Zs[Idx][1];
            z3 = m_Zs[Idx][2];
            z4 = m_Zs[Idx][3];
            zavg = 0.25 * (z1 + z2 + z3 + z4);
            height = zc - zavg;

            if (VarCB.SelectedIndex == 0)
                val = String.Format("{0:0.00}%", m_OverLon[Idx]);
            else
            if (VarCB.SelectedIndex == 1)
                val = String.Format("{0:0.00}%", m_OverTrs[Idx]);
            else
                val = String.Format("{0:0.00}cm", m_GSD[Idx]);

            InfoSL.Text = String.Format("Z = {0:0.00}, Height = {1:0.00} " +
                                        "(zavg = {2:0.00}, zs = ({3:0.00}, {4:0.00}, {5:0.00}, {6:0.00})) " +
                                        ", Var Value = ",
                                        zc, height, zavg, z1, z2, z3, z4) + val;
            m_nSelIdx = Idx;
            MainDP.Invalidate();
        }

        #endregion Private Identify Stuff

        #endregion Private
    }
}
