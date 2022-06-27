using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AerialPhotoQC
{
    public class ShpInfoF
    {
        private const String LINE_FLD_NAME = "FLIGHTLINE";
        private const String OMEGA_FLD_NAME = "OMEGA";
        private const String PHI_FLD_NAME = "PHI";
        private const String KAPPA_FLD_NAME = "KAPPA";
        private const double CLOSE_TOL = 1.0;
        private const double PARALLEL_COS = 0.99;
        private const double DIST_FACTOR = 1.1;

        public String m_sLinFileName;
        public String m_sPntFileName;
        public String m_sPolyFileName;

        public double m_nMinLinDist;
        public double m_nMaxLinDist;
        public double m_nMaxAngTol;

        public double m_nPntMinX;
        public double m_nPntMinY;
        public double m_nPntMaxX;
        public double m_nPntMaxY;
        public List<List<double>> m_PntXs;
        public List<List<double>> m_PntYs;
        public List<List<double>> m_PntZs;
        public List<List<double>> m_PntEO_Omega;
        public List<List<double>> m_PntEO_Phi;
        public List<List<double>> m_PntEO_Kappa;
        public List<int> m_PntLinID;
        public List<int> m_PntLinIdxs;
        public List<List<int>> m_PntLeftIdxs;
        public List<List<int>> m_PntRightIdxs;
        public List<List<double[]>> m_PntImgXs;
        public List<List<double[]>> m_PntImgYs;
        public List<List<double[]>> m_PntImgZs;
        public List<List<double>> m_PntImgArea;
        public List<List<double>> m_PntOverB;
        public List<List<double>> m_PntOverF;
        public List<List<double>> m_PntOverL;
        public List<List<double>> m_PntOverR;
        public List<List<double>> m_PntOverLon;
        public List<List<double>> m_PntOverTrs;
        public List<List<double>> m_PntGSDA;
        public List<List<double>> m_PntGSDMin;
        public List<List<double>> m_PntGSDMax;
        public List<List<double>> m_PntGSDAvg;
        public List<List<double>> m_PntGSDStd;
        public List<List<double>> m_PntHMin;
        public List<List<double>> m_PntHMax;
        public List<List<double>> m_PntHAvg;
        public List<List<double>> m_PntHStd;
        public List<List<double>> m_PntOverOblF;
        public List<List<double>> m_PntOverOblL;

        public double m_nLinMinX;
        public double m_nLinMinY;
        public double m_nLinMaxX;
        public double m_nLinMaxY;
        public List<List<double>> m_LinXs;
        public List<List<double>> m_LinYs;
        public List<List<double>> m_LinZs;
        public List<int> m_LinLinID;
        public List<double> m_LinKappas;
        public List<int> m_LinPntIdxs;
        public List<int> m_LinLeftIdxs;
        public List<int> m_LinRightIdxs;
        public List<double> m_LinLeftDist;
        public List<double> m_LinRightDist;
        public List<double> m_LinOverLon_Min;
        public List<double> m_LinOverLon_Max;
        public List<double> m_LinOverLon_Avg;
        public List<double> m_LinOverLon_Std;
        public List<double> m_LinOverTrs_Min;
        public List<double> m_LinOverTrs_Max;
        public List<double> m_LinOverTrs_Avg;
        public List<double> m_LinOverTrs_Std;
        public List<double> m_LinGSDA_Min;
        public List<double> m_LinGSDA_Max;
        public List<double> m_LinGSDA_Avg;
        public List<double> m_LinGSDA_Std;
        public List<double> m_LinGSDAvg_Min;
        public List<double> m_LinGSDAvg_Max;
        public List<double> m_LinGSDAvg_Avg;
        public List<double> m_LinGSDAvg_Std;
        public List<double> m_LinH_Min;
        public List<double> m_LinH_Max;
        public List<double> m_LinH_Avg;
        public List<double> m_LinH_Std;
        public List<double> m_LinOverOblF_Min;
        public List<double> m_LinOverOblF_Max;
        public List<double> m_LinOverOblF_Avg;
        public List<double> m_LinOverOblF_Std;
        public List<double> m_LinOverOblL_Min;
        public List<double> m_LinOverOblL_Max;
        public List<double> m_LinOverOblL_Avg;
        public List<double> m_LinOverOblL_Std;

        public double m_nMinX;
        public double m_nMinY;
        public double m_nMaxX;
        public double m_nMaxY;

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Public ------------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Public

        /*=== ShpInfoF() ===*/
        public ShpInfoF()
        {
            m_PntXs = null;
            m_PntYs = null;
            m_PntZs = null;
            m_PntEO_Omega = null;
            m_PntEO_Phi = null;
            m_PntEO_Kappa = null;
            m_PntLinID = null;
            m_PntLinIdxs = null;
            m_PntLeftIdxs = null;
            m_PntRightIdxs = null;
            m_PntImgXs = null;
            m_PntImgYs = null;
            m_PntImgZs = null;
            m_PntImgArea = null;
            m_PntOverB = null;
            m_PntOverF = null;
            m_PntOverL = null;
            m_PntOverR = null;
            m_PntOverLon = null;
            m_PntOverTrs = null;
            m_PntGSDA = null;
            m_PntGSDMin = null;
            m_PntGSDMax = null;
            m_PntGSDAvg = null;
            m_PntGSDStd = null;
            m_PntHMin = null;
            m_PntHMax = null;
            m_PntHAvg = null;
            m_PntHStd = null;
            m_PntOverOblF = null;
            m_PntOverOblL = null;

            m_LinXs = null;
            m_LinYs = null;
            m_LinZs = null;
            m_LinLinID = null;
            m_LinKappas = null;
            m_LinPntIdxs = null;
            m_LinLeftIdxs = null;
            m_LinRightIdxs = null;
            m_LinLeftDist = null;
            m_LinRightDist = null;
            m_LinOverLon_Min = null;
            m_LinOverLon_Max = null;
            m_LinOverLon_Avg = null;
            m_LinOverLon_Std = null;
            m_LinOverTrs_Min = null;
            m_LinOverTrs_Max = null;
            m_LinOverTrs_Avg = null;
            m_LinOverTrs_Std = null;
            m_LinGSDA_Min = null;
            m_LinGSDA_Max = null;
            m_LinGSDA_Avg = null;
            m_LinGSDA_Std = null;
            m_LinGSDAvg_Min = null;
            m_LinGSDAvg_Max = null;
            m_LinGSDAvg_Avg = null;
            m_LinGSDAvg_Std = null;
            m_LinH_Min = null;
            m_LinH_Max = null;
            m_LinH_Avg = null;
            m_LinH_Std = null;
            m_LinOverOblF_Min = null;
            m_LinOverOblF_Max = null;
            m_LinOverOblF_Avg = null;
            m_LinOverOblF_Std = null;
            m_LinOverOblL_Min = null;
            m_LinOverOblL_Max = null;
            m_LinOverOblL_Avg = null;
            m_LinOverOblL_Std = null;

            DoClose();
        }

        /*=== Open() ===*/
        public String Open(String LinFileName,
                           String PntFileName,
                           String PolyFileName,
            
                           double MinLinDist,
                           double MaxLinDist,
                           double MaxAngTol)
        {
            m_sLinFileName = LinFileName;
            m_sPntFileName = PntFileName;
            m_sPolyFileName = PolyFileName;

            m_nMinLinDist = MinLinDist;
            m_nMaxLinDist = MaxLinDist;
            m_nMaxAngTol = MaxAngTol;

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
            String Res;

            Res = DoOpen_Pnt();
            if (Res != "OK")
            {
                DoClose();
                return Res;
            }

            Res = DoOpen_Lin();
            if (Res != "OK")
            {
                DoClose();
                return Res;
            }

            m_nMinX = Math.Min(m_nPntMinX, m_nLinMinX);
            m_nMinY = Math.Min(m_nPntMinY, m_nLinMinY);
            m_nMaxX = Math.Max(m_nPntMaxX, m_nLinMaxX);
            m_nMaxY = Math.Max(m_nPntMaxY, m_nLinMaxY);
            
            Res = DoOpen_AssignLines();
            if (Res != "OK")
            {
                DoClose();
                return Res;
            }

            Res = DoOpen_AssignNeighbourLines();
            if (Res != "OK")
            {
                DoClose();
                return Res;
            }

            Res = DoOpen_AssignNeighbourPoints();
            if (Res != "OK")
            {
                DoClose();
                return Res;
            }

            InitOutInfo();

            return "OK";
        }

        /*=== DoClose() ===*/
        private void DoClose()
        {
            int i, Count, j, Count1;

            m_sLinFileName = "";
            m_sPntFileName = "";
            m_sPolyFileName = "";

            m_nMinLinDist = 0.0;
            m_nMaxLinDist = 0.0;
            m_nMaxAngTol = 0.0;

            m_nPntMinX = 0.0;
            m_nPntMinY = 0.0;
            m_nPntMaxX = 0.0;
            m_nPntMaxY = 0.0;
            if (m_PntXs != null)
            {
                Count = m_PntXs.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntXs[i].Clear();
                    m_PntXs[i] = null;
                }
                m_PntXs.Clear();
                m_PntXs = null;
            }
            if (m_PntYs != null)
            {
                Count = m_PntYs.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntYs[i].Clear();
                    m_PntYs[i] = null;
                }
                m_PntYs.Clear();
                m_PntYs = null;
            }
            if (m_PntZs != null)
            {
                Count = m_PntZs.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntZs[i].Clear();
                    m_PntZs[i] = null;
                }
                m_PntZs.Clear();
                m_PntZs = null;
            }
            if (m_PntEO_Omega != null)
            {
                Count = m_PntEO_Omega.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntEO_Omega[i].Clear();
                    m_PntEO_Omega[i] = null;
                }
                m_PntEO_Omega.Clear();
                m_PntEO_Omega = null;
            }
            if (m_PntEO_Phi != null)
            {
                Count = m_PntEO_Phi.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntEO_Phi[i].Clear();
                    m_PntEO_Phi[i] = null;
                }
                m_PntEO_Phi.Clear();
                m_PntEO_Phi = null;
            }
            if (m_PntEO_Kappa != null)
            {
                Count = m_PntEO_Kappa.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntEO_Kappa[i].Clear();
                    m_PntEO_Kappa[i] = null;
                }
                m_PntEO_Kappa.Clear();
                m_PntEO_Kappa = null;
            }
            if (m_PntLinID != null)
            {
                m_PntLinID.Clear();
                m_PntLinID = null;
            }
            if (m_PntLinIdxs != null)
            {
                m_PntLinIdxs.Clear();
                m_PntLinIdxs = null;
            }
            if (m_PntLeftIdxs != null)
            {
                Count = m_PntLeftIdxs.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntLeftIdxs[i].Clear();
                    m_PntLeftIdxs[i] = null;
                }
                m_PntLeftIdxs.Clear();
                m_PntLeftIdxs = null;
            }
            if (m_PntRightIdxs != null)
            {
                Count = m_PntRightIdxs.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntRightIdxs[i].Clear();
                    m_PntRightIdxs[i] = null;
                }
                m_PntRightIdxs.Clear();
                m_PntRightIdxs = null;
            }
            if (m_PntImgXs != null)
            {
                Count = m_PntImgXs.Count;
                for (i = 0; i < Count; i++)
                {
                    Count1 = m_PntImgXs[i].Count;
                    for (j = 0; j < Count1; j++)
                        m_PntImgXs[i][j] = null;
                    m_PntImgXs[i].Clear();
                    m_PntImgXs[i] = null;
                }
                m_PntImgXs.Clear();
                m_PntImgXs = null;
            }
            if (m_PntImgYs != null)
            {
                Count = m_PntImgYs.Count;
                for (i = 0; i < Count; i++)
                {
                    Count1 = m_PntImgYs[i].Count;
                    for (j = 0; j < Count1; j++)
                        m_PntImgYs[i][j] = null;
                    m_PntImgYs[i].Clear();
                    m_PntImgYs[i] = null;
                }
                m_PntImgYs.Clear();
                m_PntImgYs = null;
            }
            if (m_PntImgZs != null)
            {
                Count = m_PntImgZs.Count;
                for (i = 0; i < Count; i++)
                {
                    Count1 = m_PntImgZs[i].Count;
                    for (j = 0; j < Count1; j++)
                        m_PntImgZs[i][j] = null;
                    m_PntImgZs[i].Clear();
                    m_PntImgZs[i] = null;
                }
                m_PntImgZs.Clear();
                m_PntImgZs = null;
            }
            if (m_PntImgArea != null)
            {
                Count = m_PntImgArea.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntImgArea[i].Clear();
                    m_PntImgArea[i] = null;
                }
                m_PntImgArea.Clear();
                m_PntImgArea = null;
            }
            if (m_PntOverB != null)
            {
                Count = m_PntOverB.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntOverB[i].Clear();
                    m_PntOverB[i] = null;
                }
                m_PntOverB.Clear();
                m_PntOverB = null;
            }
            if (m_PntOverF != null)
            {
                Count = m_PntOverF.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntOverF[i].Clear();
                    m_PntOverF[i] = null;
                }
                m_PntOverF.Clear();
                m_PntOverF = null;
            }
            if (m_PntOverL != null)
            {
                Count = m_PntOverL.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntOverL[i].Clear();
                    m_PntOverL[i] = null;
                }
                m_PntOverL.Clear();
                m_PntOverL = null;
            }
            if (m_PntOverR != null)
            {
                Count = m_PntOverR.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntOverR[i].Clear();
                    m_PntOverR[i] = null;
                }
                m_PntOverR.Clear();
                m_PntOverR = null;
            }
            if (m_PntOverLon != null)
            {
                Count = m_PntOverLon.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntOverLon[i].Clear();
                    m_PntOverLon[i] = null;
                }
                m_PntOverLon.Clear();
                m_PntOverLon = null;
            }
            if (m_PntOverTrs != null)
            {
                Count = m_PntOverTrs.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntOverTrs[i].Clear();
                    m_PntOverTrs[i] = null;
                }
                m_PntOverTrs.Clear();
                m_PntOverTrs = null;
            }
            if (m_PntGSDA != null)
            {
                Count = m_PntGSDA.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntGSDA[i].Clear();
                    m_PntGSDA[i] = null;
                }
                m_PntGSDA.Clear();
                m_PntGSDA = null;
            }
            if (m_PntGSDMin != null)
            {
                Count = m_PntGSDMin.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntGSDMin[i].Clear();
                    m_PntGSDMin[i] = null;
                }
                m_PntGSDMin.Clear();
                m_PntGSDMin = null;
            }
            if (m_PntGSDMax != null)
            {
                Count = m_PntGSDMax.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntGSDMax[i].Clear();
                    m_PntGSDMax[i] = null;
                }
                m_PntGSDMax.Clear();
                m_PntGSDMax = null;
            }
            if (m_PntGSDAvg != null)
            {
                Count = m_PntGSDAvg.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntGSDAvg[i].Clear();
                    m_PntGSDAvg[i] = null;
                }
                m_PntGSDAvg.Clear();
                m_PntGSDAvg = null;
            }
            if (m_PntGSDStd != null)
            {
                Count = m_PntGSDStd.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntGSDStd[i].Clear();
                    m_PntGSDStd[i] = null;
                }
                m_PntGSDStd.Clear();
                m_PntGSDStd = null;
            }
            if (m_PntHMin != null)
            {
                Count = m_PntHMin.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntHMin[i].Clear();
                    m_PntHMin[i] = null;
                }
                m_PntHMin.Clear();
                m_PntHMin = null;
            }
            if (m_PntHMax != null)
            {
                Count = m_PntHMax.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntHMax[i].Clear();
                    m_PntHMax[i] = null;
                }
                m_PntHMax.Clear();
                m_PntHMax = null;
            }
            if (m_PntHAvg != null)
            {
                Count = m_PntHAvg.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntHAvg[i].Clear();
                    m_PntHAvg[i] = null;
                }
                m_PntHAvg.Clear();
                m_PntHAvg = null;
            }
            if (m_PntHStd != null)
            {
                Count = m_PntHStd.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntHStd[i].Clear();
                    m_PntHStd[i] = null;
                }
                m_PntHStd.Clear();
                m_PntHStd = null;
            }
            if (m_PntOverOblF != null)
            {
                Count = m_PntOverOblF.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntOverOblF[i].Clear();
                    m_PntOverOblF[i] = null;
                }
                m_PntOverOblF.Clear();
                m_PntOverOblF = null;
            }
            if (m_PntOverOblL != null)
            {
                Count = m_PntOverOblL.Count;
                for (i = 0; i < Count; i++)
                {
                    m_PntOverOblL[i].Clear();
                    m_PntOverOblL[i] = null;
                }
                m_PntOverOblL.Clear();
                m_PntOverOblL = null;
            }

            m_nLinMinX = 0.0;
            m_nLinMinY = 0.0;
            m_nLinMaxX = 0.0;
            m_nLinMaxY = 0.0;
            if (m_LinXs != null)
            {
                Count = m_LinXs.Count;
                for (i = 0; i < Count; i++)
                {
                    m_LinXs[i].Clear();
                    m_LinXs[i] = null;
                }
                m_LinXs.Clear();
                m_LinXs = null;
            }
            if (m_LinYs != null)
            {
                Count = m_LinYs.Count;
                for (i = 0; i < Count; i++)
                {
                    m_LinYs[i].Clear();
                    m_LinYs[i] = null;
                }
                m_LinYs.Clear();
                m_LinYs = null;
            }
            if (m_LinZs != null)
            {
                Count = m_LinZs.Count;
                for (i = 0; i < Count; i++)
                {
                    m_LinZs[i].Clear();
                    m_LinZs[i] = null;
                }
                m_LinZs.Clear();
                m_LinZs = null;
            }
            if (m_LinLinID != null)
            {
                m_LinLinID.Clear();
                m_LinLinID = null;
            }
            if (m_LinKappas != null)
            {
                m_LinKappas.Clear();
                m_LinKappas = null;
            }
            if (m_LinPntIdxs != null)
            {
                m_LinPntIdxs.Clear();
                m_LinPntIdxs = null;
            }
            if (m_LinLeftIdxs != null)
            {
                m_LinLeftIdxs.Clear();
                m_LinLeftIdxs = null;
            }
            if (m_LinRightIdxs != null)
            {
                m_LinRightIdxs.Clear();
                m_LinRightIdxs = null;
            }
            if (m_LinLeftDist != null)
            {
                m_LinLeftDist.Clear();
                m_LinLeftDist = null;
            }
            if (m_LinRightDist != null)
            {
                m_LinRightDist.Clear();
                m_LinRightDist = null;
            }
            if (m_LinOverLon_Min != null)
            {
                m_LinOverLon_Min.Clear();
                m_LinOverLon_Min = null;
            }
            if (m_LinOverLon_Max != null)
            {
                m_LinOverLon_Max.Clear();
                m_LinOverLon_Max = null;
            }
            if (m_LinOverLon_Avg != null)
            {
                m_LinOverLon_Avg.Clear();
                m_LinOverLon_Avg = null;
            }
            if (m_LinOverLon_Std != null)
            {
                m_LinOverLon_Std.Clear();
                m_LinOverLon_Std = null;
            }
            if (m_LinOverTrs_Min != null)
            {
                m_LinOverTrs_Min.Clear();
                m_LinOverTrs_Min = null;
            }
            if (m_LinOverTrs_Max != null)
            {
                m_LinOverTrs_Max.Clear();
                m_LinOverTrs_Max = null;
            }
            if (m_LinOverTrs_Avg != null)
            {
                m_LinOverTrs_Avg.Clear();
                m_LinOverTrs_Avg = null;
            }
            if (m_LinOverTrs_Std != null)
            {
                m_LinOverTrs_Std.Clear();
                m_LinOverTrs_Std = null;
            }
            if (m_LinGSDA_Min != null)
            {
                m_LinGSDA_Min.Clear();
                m_LinGSDA_Min = null;
            }
            if (m_LinGSDA_Max != null)
            {
                m_LinGSDA_Max.Clear();
                m_LinGSDA_Max = null;
            }
            if (m_LinGSDA_Avg != null)
            {
                m_LinGSDA_Avg.Clear();
                m_LinGSDA_Avg = null;
            }
            if (m_LinGSDA_Std != null)
            {
                m_LinGSDA_Std.Clear();
                m_LinGSDA_Std = null;
            }
            if (m_LinGSDAvg_Min != null)
            {
                m_LinGSDAvg_Min.Clear();
                m_LinGSDAvg_Min = null;
            }
            if (m_LinGSDAvg_Max != null)
            {
                m_LinGSDAvg_Max.Clear();
                m_LinGSDAvg_Max = null;
            }
            if (m_LinGSDAvg_Avg != null)
            {
                m_LinGSDAvg_Avg.Clear();
                m_LinGSDAvg_Avg = null;
            }
            if (m_LinGSDAvg_Std != null)
            {
                m_LinGSDAvg_Std.Clear();
                m_LinGSDAvg_Std = null;
            }
            if (m_LinH_Min != null)
            {
                m_LinH_Min.Clear();
                m_LinH_Min = null;
            }
            if (m_LinH_Max != null)
            {
                m_LinH_Max.Clear();
                m_LinH_Max = null;
            }
            if (m_LinH_Avg != null)
            {
                m_LinH_Avg.Clear();
                m_LinH_Avg = null;
            }
            if (m_LinH_Std != null)
            {
                m_LinH_Std.Clear();
                m_LinH_Std = null;
            }
            if (m_LinOverOblF_Min != null)
            {
                m_LinOverOblF_Min.Clear();
                m_LinOverOblF_Min = null;
            }
            if (m_LinOverOblF_Max != null)
            {
                m_LinOverOblF_Max.Clear();
                m_LinOverOblF_Max = null;
            }
            if (m_LinOverOblF_Avg != null)
            {
                m_LinOverOblF_Avg.Clear();
                m_LinOverOblF_Avg = null;
            }
            if (m_LinOverOblF_Std != null)
            {
                m_LinOverOblF_Std.Clear();
                m_LinOverOblF_Std = null;
            }
            if (m_LinOverOblL_Min != null)
            {
                m_LinOverOblL_Min.Clear();
                m_LinOverOblL_Min = null;
            }
            if (m_LinOverOblL_Max != null)
            {
                m_LinOverOblL_Max.Clear();
                m_LinOverOblL_Max = null;
            }
            if (m_LinOverOblL_Avg != null)
            {
                m_LinOverOblL_Avg.Clear();
                m_LinOverOblL_Avg = null;
            }
            if (m_LinOverOblL_Std != null)
            {
                m_LinOverOblL_Std.Clear();
                m_LinOverOblL_Std = null;
            }
        }

        /*=== DoOpen_Pnt() ===*/
        private String DoOpen_Pnt()
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

            int LINE_FldIdx = -1;
            int OMEGA_FldIdx = -1;
            int PHI_FldIdx = -1;
            int KAPPA_FldIdx = -1;

            bool FirstGot = false;

            int PntsCount = 0;
            int PartsCount = 0;

            double[] PntsX;
            double[] PntsY;
            double[] PntsZ;
            int[] Parts;

            double X, Y, Z;

            String sLine;
            int Line, PrevLine = -1;
            String sOmega, sPhi, sKappa;
            double Omega, Phi, Kappa;

            int LinIdx = -1;

            //=============//
            // Read Header //
            //=============//
            SHP = new ShpLib();

            Res = SHP.GetGenInfo(m_sPntFileName,

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

            for (FldIdx = 0; FldIdx < FldCount; FldIdx++)
            {
                if (FldNames[FldIdx].ToUpper() == LINE_FLD_NAME)
                    LINE_FldIdx = FldIdx;
                else
                if (FldNames[FldIdx].ToUpper() == OMEGA_FLD_NAME)
                    OMEGA_FldIdx = FldIdx;
                else
                if (FldNames[FldIdx].ToUpper() == PHI_FLD_NAME)
                    PHI_FldIdx = FldIdx;
                else
                if (FldNames[FldIdx].ToUpper() == KAPPA_FLD_NAME)
                    KAPPA_FldIdx = FldIdx;
            }
            if (LINE_FldIdx == -1)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Points Shape File: Field '" + LINE_FLD_NAME + "' does not exist.";
            }
            if (OMEGA_FldIdx == -1)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Points Shape File: Field '" + OMEGA_FLD_NAME + "' does not exist.";
            }
            if (PHI_FldIdx == -1)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Points Shape File: Field '" + PHI_FLD_NAME + "' does not exist.";
            }
            if (KAPPA_FldIdx == -1)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Points Shape File: Field '" + KAPPA_FLD_NAME + "' does not exist.";
            }

            //=============//
            // Read Points //
            //=============//
            m_nPntMinX = double.MaxValue;
            m_nPntMinY = double.MaxValue;
            m_nPntMaxX = double.MinValue;
            m_nPntMaxY = double.MinValue;

            m_PntXs = new List<List<double>>();
            m_PntYs = new List<List<double>>();
            m_PntZs = new List<List<double>>();
            m_PntEO_Omega = new List<List<double>>();
            m_PntEO_Phi = new List<List<double>>();
            m_PntEO_Kappa = new List<List<double>>();
            m_PntLinID = new List<int>();

            Res = SHP.GetShpInfo_StartZ(m_sPntFileName,
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

                sLine = SHP.GetShpInfo_DBF(LINE_FldIdx);
                if (!int.TryParse(sLine, out Line))
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
                    return "Error Reading Points Shape File: Field '" + LINE_FLD_NAME + "' is not an integer number.";
                }

                sOmega = SHP.GetShpInfo_DBF(OMEGA_FldIdx);
                if (!double.TryParse(sOmega, out Omega))
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
                    return "Error Reading Points Shape File: Field '" + OMEGA_FLD_NAME + "' is not a real number.";
                }
                sPhi = SHP.GetShpInfo_DBF(PHI_FldIdx);
                if (!double.TryParse(sPhi, out Phi))
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
                    return "Error Reading Points Shape File: Field '" + PHI_FLD_NAME + "' is not a real number.";
                }
                sKappa = SHP.GetShpInfo_DBF(KAPPA_FldIdx);
                if (!double.TryParse(sKappa, out Kappa))
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
                    return "Error Reading Points Shape File: Field '" + KAPPA_FLD_NAME + "' is not a real number.";
                }

                if (Line != PrevLine)
                {
                    m_PntXs.Add(new List<double>());
                    m_PntYs.Add(new List<double>());
                    m_PntZs.Add(new List<double>());
                    m_PntEO_Omega.Add(new List<double>());
                    m_PntEO_Phi.Add(new List<double>());
                    m_PntEO_Kappa.Add(new List<double>());
                    m_PntLinID.Add(Line);
                    LinIdx = m_PntXs.Count - 1;
                }

                X = PntsX[0];
                Y = PntsY[0];
                Z = PntsZ[0];

                m_PntXs[LinIdx].Add(X);
                m_PntYs[LinIdx].Add(Y);
                m_PntZs[LinIdx].Add(Z);

                m_PntEO_Omega[LinIdx].Add(Omega);
                m_PntEO_Phi[LinIdx].Add(Phi);
                m_PntEO_Kappa[LinIdx].Add(Kappa);

                if (X < m_nPntMinX)
                    m_nPntMinX = X;
                if (Y < m_nPntMinY)
                    m_nPntMinY = Y;
                if (X > m_nPntMaxX)
                    m_nPntMaxX = X;
                if (Y > m_nPntMaxY)
                    m_nPntMaxY = Y;

                PrevLine = Line;
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

        /*=== DoOpen_Lin() ===*/
        private String DoOpen_Lin()
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

            int LINE_FldIdx = -1;

            bool FirstGot = false;

            int PntsCount = 0;
            int PartsCount = 0;
            int PntIdx;

            double[] PntsX;
            double[] PntsY;
            double[] PntsZ;
            int[] Parts;
            int Alloced = 10000;

            double X, Y, Z;

            String sLine;
            int Line;

            int LinIdx = -1;

            int Kappa_PntsCount;
            double Kappa;

            //=============//
            // Read Header //
            //=============//
            SHP = new ShpLib();

            Res = SHP.GetGenInfo(m_sLinFileName,

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

            if (ShapeType != ShpLib.SHPTYPE_POLYLINEZ)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Lines Shape File: Shape Type must be POLYLINEZ.";
            }

            for (FldIdx = 0; FldIdx < FldCount; FldIdx++)
            {
                if (FldNames[FldIdx].ToUpper() == LINE_FLD_NAME)
                    LINE_FldIdx = FldIdx;
            }
            if (LINE_FldIdx == -1)
            {
                FldNames = null;
                FldTypes = null;
                FldLens = null;
                FldDecs = null;
                SHP.GetShpInfo_Stop();
                SHP.Dispose();
                SHP = null;
                return "Error Opening Lines Shape File: Field '" + LINE_FLD_NAME + "' does not exist.";
            }

            //============//
            // Read Lines //
            //============//
            m_nLinMinX = double.MaxValue;
            m_nLinMinY = double.MaxValue;
            m_nLinMaxX = double.MinValue;
            m_nLinMaxY = double.MinValue;

            m_LinXs = new List<List<double>>();
            m_LinYs = new List<List<double>>();
            m_LinZs = new List<List<double>>();
            m_LinLinID = new List<int>();
            m_LinKappas = new List<double>();

            Res = SHP.GetShpInfo_StartZ(m_sLinFileName,
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
                return "Error Reading Lines Shape File.";
            }

            PntsX = new double[Alloced];
            PntsY = new double[Alloced];
            PntsZ = new double[Alloced];
            Parts = new int[100];
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
                        return "Error Reading Lines Shape File.";
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

                sLine = SHP.GetShpInfo_DBF(LINE_FldIdx);
                if (!int.TryParse(sLine, out Line))
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
                    return "Error Reading Lines Shape File: Field '" + LINE_FLD_NAME + "' is not an integer number.";
                }

                m_LinXs.Add(new List<double>());
                m_LinYs.Add(new List<double>());
                m_LinZs.Add(new List<double>());
                m_LinLinID.Add(Line);
                LinIdx = m_LinXs.Count - 1;

                for (PntIdx = 0; PntIdx < PntsCount; PntIdx++)
                {
                    X = PntsX[PntIdx];
                    Y = PntsY[PntIdx];
                    Z = PntsZ[PntIdx];

                    m_LinXs[LinIdx].Add(X);
                    m_LinYs[LinIdx].Add(Y);
                    m_LinZs[LinIdx].Add(Z);                    

                    if (X < m_nLinMinX)
                        m_nLinMinX = X;
                    if (Y < m_nLinMinY)
                        m_nLinMinY = Y;
                    if (X > m_nLinMaxX)
                        m_nLinMaxX = X;
                    if (Y > m_nLinMaxY)
                        m_nLinMaxY = Y;
                }

                Kappa_PntsCount = m_LinXs[LinIdx].Count;
                if (Kappa_PntsCount == 1)
                    Kappa = (Math.PI / 180.0) * m_PntEO_Kappa[LinIdx][0];
                else
                {
                    PntIdx = Kappa_PntsCount - 1;
                    X = m_LinXs[LinIdx][PntIdx] - m_LinXs[LinIdx][0];
                    Y = m_LinYs[LinIdx][PntIdx] - m_LinYs[LinIdx][0];
                    Kappa = Math.Atan2(Y, X);
                }
                m_LinKappas.Add(Kappa);
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

        #endregion Private Open/Close

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Assign Lines ----------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Assign Lines

        /*=== DoOpen_AssignLines() ===*/
        private String DoOpen_AssignLines()
        {
            int i, j, Count;

            if (m_PntXs.Count != m_LinXs.Count)
                return "Error: Points and Lines mismatch.";

            Count = m_PntXs.Count;
            m_PntLinIdxs = new List<int>();
            m_LinPntIdxs = new List<int>();
            for (i = 0; i < Count; i++)
            {
                m_PntLinIdxs.Add(-1);
                m_LinPntIdxs.Add(-1);
            }

            for (i = 0; i < Count; i++)
            {
                for (j = 0; j < Count; j++)
                {
                    if (m_LinPntIdxs[j] != -1)
                        continue;

                    if (m_PntLinID[i] != m_LinLinID[j])
                        continue;

                    m_PntLinIdxs[i] = j;
                    m_LinPntIdxs[j] = i;

                    break;
                }
            }

            for (i = 0; i < Count; i++)
            {
                if (m_PntLinIdxs[i] == -1)
                    return "Error: Points and Lines mismatch.";
                if (m_LinPntIdxs[i] == -1)
                    return "Error: Points and Lines mismatch.";
            }

            return "OK";
        }

        /*=== DoOpen_AssignNeighbourLines() ===*/
        private String DoOpen_AssignNeighbourLines()
        {
            int i, Count, Idx;
            double dist;

            Count = m_LinXs.Count;
            m_LinLeftIdxs = new List<int>();
            m_LinRightIdxs = new List<int>();
            m_LinLeftDist = new List<double>();
            m_LinRightDist = new List<double>();
            m_LinOverLon_Min = new List<double>();
            m_LinOverLon_Max = new List<double>();
            m_LinOverLon_Avg = new List<double>();
            m_LinOverLon_Std = new List<double>();
            m_LinOverTrs_Min = new List<double>();
            m_LinOverTrs_Max = new List<double>();
            m_LinOverTrs_Avg = new List<double>();
            m_LinOverTrs_Std = new List<double>();
            m_LinGSDA_Min = new List<double>();
            m_LinGSDA_Max = new List<double>();
            m_LinGSDA_Avg = new List<double>();
            m_LinGSDA_Std = new List<double>();
            m_LinGSDAvg_Min = new List<double>();
            m_LinGSDAvg_Max = new List<double>();
            m_LinGSDAvg_Avg = new List<double>();
            m_LinGSDAvg_Std = new List<double>();
            m_LinH_Min = new List<double>();
            m_LinH_Max = new List<double>();
            m_LinH_Avg = new List<double>();
            m_LinH_Std = new List<double>();
            m_LinOverOblF_Min = new List<double>();
            m_LinOverOblF_Max = new List<double>();
            m_LinOverOblF_Avg = new List<double>();
            m_LinOverOblF_Std = new List<double>();
            m_LinOverOblL_Min = new List<double>();
            m_LinOverOblL_Max = new List<double>();
            m_LinOverOblL_Avg = new List<double>();
            m_LinOverOblL_Std = new List<double>();

            for (i = 0; i < Count; i++)
            {
                m_LinLeftIdxs.Add(-1);
                m_LinRightIdxs.Add(-1);
                m_LinLeftDist.Add(0.0);
                m_LinRightDist.Add(0.0);
                m_LinOverLon_Min.Add(0.0);
                m_LinOverLon_Max.Add(0.0);
                m_LinOverLon_Avg.Add(0.0);
                m_LinOverLon_Std.Add(0.0);
                m_LinOverTrs_Min.Add(0.0);
                m_LinOverTrs_Max.Add(0.0);
                m_LinOverTrs_Avg.Add(0.0);
                m_LinOverTrs_Std.Add(0.0);
                m_LinGSDA_Min.Add(0.0);
                m_LinGSDA_Max.Add(0.0);
                m_LinGSDA_Avg.Add(0.0);
                m_LinGSDA_Std.Add(0.0);
                m_LinGSDAvg_Min.Add(0.0);
                m_LinGSDAvg_Max.Add(0.0);
                m_LinGSDAvg_Avg.Add(0.0);
                m_LinGSDAvg_Std.Add(0.0);
                m_LinH_Min.Add(0.0);
                m_LinH_Max.Add(0.0);
                m_LinH_Avg.Add(0.0);
                m_LinH_Std.Add(0.0);
                m_LinOverOblF_Min.Add(0.0);
                m_LinOverOblF_Max.Add(0.0);
                m_LinOverOblF_Avg.Add(0.0);
                m_LinOverOblF_Std.Add(0.0);
                m_LinOverOblL_Min.Add(0.0);
                m_LinOverOblL_Max.Add(0.0);
                m_LinOverOblL_Avg.Add(0.0);
                m_LinOverOblL_Std.Add(0.0);
            }
            for (i = 0; i < Count; i++)
            {
                Idx = DoOpen_AssignNeighbourLines_Left(i, out dist);
                if (Idx != -1)
                {
                    m_LinLeftIdxs[i] = Idx;
                    m_LinLeftDist[i] = dist;
                }
                Idx = DoOpen_AssignNeighbourLines_Right(i, out dist);
                if (Idx != -1)
                {
                    m_LinRightIdxs[i] = Idx;
                    m_LinRightDist[i] = dist;
                }
            }

            return "OK";
        }

        /*=== DoOpen_AssignNeighbourLines_Left() ===*/
        private int DoOpen_AssignNeighbourLines_Left(int LinIdx,
                                                     out double dist)
        {
            int i, Count, idx, Idx;
            double vx, vy, d, vx1, vy1, d1;
            double cx, cy, cx1, cy1;
            double dot, cos;
            double cdist, mincdist;
            int idxdist, minidxdist;

            dist = 0.0;

            if (m_PntXs[LinIdx].Count == 1)
                return -1;

            idx = m_LinXs[LinIdx].Count - 1;
            vx = m_LinXs[LinIdx][idx] - m_LinXs[LinIdx][0];
            vy = m_LinYs[LinIdx][idx] - m_LinYs[LinIdx][0];
            d = Util.Norm2D(vx, vy);
            cx = 0.5 * (m_LinXs[LinIdx][idx] + m_LinXs[LinIdx][0]);
            cy = 0.5 * (m_LinYs[LinIdx][idx] + m_LinYs[LinIdx][0]);

            Count = m_LinXs.Count;
            Idx = -1;
            mincdist = double.MaxValue;
            minidxdist = int.MaxValue;
            for (i = 0; i < Count; i++)
            {
                if (i == LinIdx)
                    continue;

                // Must be at the Left
                idx = m_LinXs[i].Count - 1;
                vx1 = m_LinXs[i][0] - m_LinXs[LinIdx][0];
                vy1 = m_LinYs[i][0] - m_LinYs[LinIdx][0];
                if (Util.Cross2D(vx1, vy1, vx, vy) >= 0)
                    continue;
                vx1 = m_LinXs[i][idx] - m_LinXs[LinIdx][0];
                vy1 = m_LinYs[i][idx] - m_LinYs[LinIdx][0];
                if (Util.Cross2D(vx1, vy1, vx, vy) >= 0)
                    continue;

                vx1 = m_LinXs[i][idx] - m_LinXs[i][0];
                vy1 = m_LinYs[i][idx] - m_LinYs[i][0];
                d1 = Util.Norm2D(vx1, vy1);

                dot = Util.Dot2D(vx, vy, vx1, vy1);
                cos = dot / (d * d1);

                // Must be Parallel
                if (Math.Abs(cos) < PARALLEL_COS)
                    continue;

                // Must be Close enough
                dist = Util.DistPntToLin2D(m_LinXs[i][0],
                                           m_LinYs[i][0],
                                           m_LinXs[i][idx],
                                           m_LinYs[i][idx],
                                           m_LinXs[LinIdx][0],
                                           m_LinYs[LinIdx][0]);

                if (dist < m_nMinLinDist || dist > m_nMaxLinDist)
                    continue;

                cx1 = 0.5 * (m_LinXs[i][idx] + m_LinXs[i][0]);
                cy1 = 0.5 * (m_LinYs[i][idx] + m_LinYs[i][0]);
                cdist = Util.Norm2D(cx1 - cx,
                                    cy1 - cy);
                if (cdist > d && cdist > d1)
                    continue;

                idxdist = Math.Abs(LinIdx - i);
                if (cdist < mincdist && idxdist < minidxdist)
                {
                    Idx = i;
                    mincdist = cdist;
                    minidxdist = idxdist;
                }
            }

            if (Idx != -1)
            {
                idx = m_LinXs[Idx].Count - 1;
                dist = Util.DistPntToLin2D(m_LinXs[Idx][0],
                                           m_LinYs[Idx][0],
                                           m_LinXs[Idx][idx],
                                           m_LinYs[Idx][idx],
                                           m_LinXs[LinIdx][0],
                                           m_LinYs[LinIdx][0]);
            }
            else
                dist = 0.0;

            return Idx;
        }

        /*=== DoOpen_AssignNeighbourLines_Right() ===*/
        private int DoOpen_AssignNeighbourLines_Right(int LinIdx,
                                                      out double dist)
        {
            int i, Count, idx, Idx;
            double vx, vy, d, vx1, vy1, d1;
            double cx, cy, cx1, cy1;
            double dot, cos;
            double cdist, mincdist;
            int idxdist, minidxdist;

            dist = 0.0;

            if (m_PntXs[LinIdx].Count == 1)
                return -1;

            idx = m_LinXs[LinIdx].Count - 1;
            vx = m_LinXs[LinIdx][idx] - m_LinXs[LinIdx][0];
            vy = m_LinYs[LinIdx][idx] - m_LinYs[LinIdx][0];
            d = Util.Norm2D(vx, vy);
            cx = 0.5 * (m_LinXs[LinIdx][idx] + m_LinXs[LinIdx][0]);
            cy = 0.5 * (m_LinYs[LinIdx][idx] + m_LinYs[LinIdx][0]);

            Count = m_LinXs.Count;
            Idx = -1;
            mincdist = double.MaxValue;
            minidxdist = int.MaxValue;
            for (i = 0; i < Count; i++)
            {
                if (i == LinIdx)
                    continue;

                // Must be at the Right
                idx = m_LinXs[i].Count - 1;
                vx1 = m_LinXs[i][0] - m_LinXs[LinIdx][0];
                vy1 = m_LinYs[i][0] - m_LinYs[LinIdx][0];
                if (Util.Cross2D(vx1, vy1, vx, vy) <= 0)
                    continue;
                vx1 = m_LinXs[i][idx] - m_LinXs[LinIdx][0];
                vy1 = m_LinYs[i][idx] - m_LinYs[LinIdx][0];
                if (Util.Cross2D(vx1, vy1, vx, vy) <= 0)
                    continue;

                vx1 = m_LinXs[i][idx] - m_LinXs[i][0];
                vy1 = m_LinYs[i][idx] - m_LinYs[i][0];
                d1 = Util.Norm2D(vx1, vy1);

                dot = Util.Dot2D(vx, vy, vx1, vy1);
                cos = dot / (d * d1);

                // Must be Parallel
                if (Math.Abs(cos) < PARALLEL_COS)
                    continue;

                // Must be Close enough
                dist = Util.DistPntToLin2D(m_LinXs[i][0],
                                           m_LinYs[i][0],
                                           m_LinXs[i][idx],
                                           m_LinYs[i][idx],
                                           m_LinXs[LinIdx][0],
                                           m_LinYs[LinIdx][0]);

                if (dist < m_nMinLinDist || dist > m_nMaxLinDist)
                    continue;

                cx1 = 0.5 * (m_LinXs[i][idx] + m_LinXs[i][0]);
                cy1 = 0.5 * (m_LinYs[i][idx] + m_LinYs[i][0]);
                cdist = Util.Norm2D(cx1 - cx,
                                    cy1 - cy);
                if (cdist > d && cdist > d1)
                    continue;

                idxdist = Math.Abs(LinIdx - i);
                if (cdist < mincdist && idxdist < minidxdist)
                {
                    Idx = i;
                    mincdist = cdist;
                    minidxdist = idxdist;
                }
            }

            if (Idx != -1)
            {
                idx = m_LinXs[Idx].Count - 1;
                dist = Util.DistPntToLin2D(m_LinXs[Idx][0],
                                           m_LinYs[Idx][0],
                                           m_LinXs[Idx][idx],
                                           m_LinYs[Idx][idx],
                                           m_LinXs[LinIdx][0],
                                           m_LinYs[LinIdx][0]);
            }
            else
                dist = 0.0;

            return Idx;
        }

        #endregion Private Assign Lines

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Assign Neighbour Points -----------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Assign Neighbour Points

        /*=== DoOpen_AssignNeighbourPoints() ===*/
        private String DoOpen_AssignNeighbourPoints()
        {
            int LinCount, LinIdx, ThisLinIdx, LeftLinIdx, RightLinIdx;
            int PntCount, PntIdx;
            double LeftDist, RightDist;
            String Res;

            m_PntLeftIdxs = new List<List<int>>();
            m_PntRightIdxs = new List<List<int>>();

            LinCount = m_PntLinIdxs.Count;
            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                m_PntLeftIdxs.Add(new List<int>());
                m_PntRightIdxs.Add(new List<int>());

                PntCount = m_PntXs[LinIdx].Count;
                for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
                {
                    m_PntLeftIdxs[LinIdx].Add(-1);
                    m_PntRightIdxs[LinIdx].Add(-1);
                }
            }

            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                ThisLinIdx = m_PntLinIdxs[LinIdx];
                LeftLinIdx = m_LinLeftIdxs[ThisLinIdx];
                RightLinIdx = m_LinRightIdxs[ThisLinIdx];
                LeftDist = m_LinLeftDist[ThisLinIdx];
                RightDist = m_LinRightDist[ThisLinIdx];

                Res = DoOpen_AssignNeighbourPoints_ByLine(ThisLinIdx,
                                                          LeftLinIdx,
                                                          RightLinIdx,
                                                          LeftDist,
                                                          RightDist);
                if (Res != "OK")
                    return Res;

            }

            return "OK";
        }

        /*=== DoOpen_AssignNeighbourPoints_ByLine() ===*/
        private String DoOpen_AssignNeighbourPoints_ByLine(int ThisLinIdx,
                                                           int LeftLinIdx,
                                                           int RightLinIdx,
                                                           double LeftDist,
                                                           double RightDist)
        {
            int ThisPntsIdx, LeftPntsIdx, RightPntsIdx;
            int ThisPntCount, ThisPntIdx;
            int LeftPntCount, LeftPntIdx;
            int RightPntCount, RightPntIdx;
            double dx, dy, d, mind;
            int Idx;
            double deg2rad = Math.PI / 180.0;
            double fact = 1.0 / Math.Cos(m_nMaxAngTol * deg2rad);

            ThisPntsIdx = m_LinPntIdxs[ThisLinIdx];
            if (LeftLinIdx != -1)
                LeftPntsIdx = m_LinPntIdxs[LeftLinIdx];
            else
                LeftPntsIdx = -1;
            if (RightLinIdx != -1)
                RightPntsIdx = m_LinPntIdxs[RightLinIdx];
            else
                RightPntsIdx = -1;

            ThisPntCount = m_PntXs[ThisPntsIdx].Count;
            for (ThisPntIdx = 0; ThisPntIdx < ThisPntCount; ThisPntIdx++)
            {
                if (m_PntLeftIdxs[ThisPntsIdx][ThisPntIdx] == -1 &&
                    LeftPntsIdx != -1)
                {
                    LeftPntCount = m_PntXs[LeftPntsIdx].Count;
                    mind = double.MaxValue;
                    Idx = -1;
                    for (LeftPntIdx = 0; LeftPntIdx < LeftPntCount; LeftPntIdx++)
                    {
                        dx = m_PntXs[ThisPntsIdx][ThisPntIdx] - m_PntXs[LeftPntsIdx][LeftPntIdx];
                        dy = m_PntYs[ThisPntsIdx][ThisPntIdx] - m_PntYs[LeftPntsIdx][LeftPntIdx];
                        d = Math.Sqrt(dx * dx + dy * dy);
                        if (d < mind)
                        {
                            mind = d;
                            Idx = LeftPntIdx;
                        }
                    }
                    if (Idx != -1 && mind < LeftDist * fact)
                        m_PntLeftIdxs[ThisPntsIdx][ThisPntIdx] = Idx;
                }

                if (m_PntRightIdxs[ThisPntsIdx][ThisPntIdx] == -1 &&
                    RightPntsIdx != -1)
                {
                    RightPntCount = m_PntXs[RightPntsIdx].Count;
                    mind = double.MaxValue;
                    Idx = -1;
                    for (RightPntIdx = 0; RightPntIdx < RightPntCount; RightPntIdx++)
                    {
                        dx = m_PntXs[ThisPntsIdx][ThisPntIdx] - m_PntXs[RightPntsIdx][RightPntIdx];
                        dy = m_PntYs[ThisPntsIdx][ThisPntIdx] - m_PntYs[RightPntsIdx][RightPntIdx];
                        d = Math.Sqrt(dx * dx + dy * dy);
                        if (d < mind)
                        {
                            mind = d;
                            Idx = RightPntIdx;
                        }
                    }
                    if (Idx != -1 && mind < RightDist * fact)
                        m_PntRightIdxs[ThisPntsIdx][ThisPntIdx] = Idx;
                }
            }

            return "OK";
        }

        #endregion Private Assign Neighbour Points

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Init Out Info ---------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Init Out Info

        /*=== InitOutInfo() ===*/
        private void InitOutInfo()
        {
            int LinCount, LinIdx;
            int PntCount, PntIdx;

            m_PntImgXs = new List<List<double[]>>();
            m_PntImgYs = new List<List<double[]>>();
            m_PntImgZs = new List<List<double[]>>();
            m_PntImgArea = new List<List<double>>();
            m_PntOverB = new List<List<double>>();
            m_PntOverF = new List<List<double>>();
            m_PntOverL = new List<List<double>>();
            m_PntOverR = new List<List<double>>();
            m_PntOverLon = new List<List<double>>();
            m_PntOverTrs = new List<List<double>>();
            m_PntGSDA = new List<List<double>>();
            m_PntGSDMin = new List<List<double>>();
            m_PntGSDMax = new List<List<double>>();
            m_PntGSDAvg = new List<List<double>>();
            m_PntGSDStd = new List<List<double>>();
            m_PntHMin = new List<List<double>>();
            m_PntHMax = new List<List<double>>();
            m_PntHAvg = new List<List<double>>();
            m_PntHStd = new List<List<double>>();
            m_PntOverOblF = new List<List<double>>();
            m_PntOverOblL = new List<List<double>>();

            LinCount = m_PntXs.Count;
            for (LinIdx = 0; LinIdx < LinCount; LinIdx++)
            {
                m_PntImgXs.Add(new List<double[]>());
                m_PntImgYs.Add(new List<double[]>());
                m_PntImgZs.Add(new List<double[]>());
                m_PntImgArea.Add(new List<double>());
                m_PntOverB.Add(new List<double>());
                m_PntOverF.Add(new List<double>());
                m_PntOverL.Add(new List<double>());
                m_PntOverR.Add(new List<double>());
                m_PntOverLon.Add(new List<double>());
                m_PntOverTrs.Add(new List<double>());
                m_PntGSDA.Add(new List<double>());
                m_PntGSDMin.Add(new List<double>());
                m_PntGSDMax.Add(new List<double>());
                m_PntGSDAvg.Add(new List<double>());
                m_PntGSDStd.Add(new List<double>());
                m_PntHMin.Add(new List<double>());
                m_PntHMax.Add(new List<double>());
                m_PntHAvg.Add(new List<double>());
                m_PntHStd.Add(new List<double>());
                m_PntOverOblF.Add(new List<double>());
                m_PntOverOblL.Add(new List<double>());

                PntCount = m_PntXs[LinIdx].Count;
                for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
                {
                    m_PntImgXs[LinIdx].Add(new double[] { 0.0, 0.0, 0.0, 0.0 });
                    m_PntImgYs[LinIdx].Add(new double[] { 0.0, 0.0, 0.0, 0.0 });
                    m_PntImgZs[LinIdx].Add(new double[] { 0.0, 0.0, 0.0, 0.0 });
                    m_PntImgArea[LinIdx].Add(0.0);
                    m_PntOverB[LinIdx].Add(0.0);
                    m_PntOverF[LinIdx].Add(0.0);
                    m_PntOverL[LinIdx].Add(0.0);
                    m_PntOverR[LinIdx].Add(0.0);
                    m_PntOverLon[LinIdx].Add(0.0);
                    m_PntOverTrs[LinIdx].Add(0.0);
                    m_PntGSDA[LinIdx].Add(0.0);
                    m_PntGSDMin[LinIdx].Add(0.0);
                    m_PntGSDMax[LinIdx].Add(0.0);
                    m_PntGSDAvg[LinIdx].Add(0.0);
                    m_PntGSDStd[LinIdx].Add(0.0);
                    m_PntHMin[LinIdx].Add(0.0);
                    m_PntHMax[LinIdx].Add(0.0);
                    m_PntHAvg[LinIdx].Add(0.0);
                    m_PntHStd[LinIdx].Add(0.0);
                    m_PntOverOblF[LinIdx].Add(0.0);
                    m_PntOverOblL[LinIdx].Add(0.0);
                }
            }
        }

        #endregion Private Init Out Info

        #endregion Private
    }
}
