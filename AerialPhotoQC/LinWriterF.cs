using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AerialPhotoQC
{
    public class LinWriterF
    {
        private const double RAD_TO_DEG = 57.295779513082320876798154814105;

        private String m_sInFileName;
        private String m_sOutFileName;
        private ShpInfoF m_Info;
        private Cfg m_Cfg;

        ShpLib m_InSHP;
        ShpLib m_OutSHP;

        private int m_nShapeType;
        private int m_nRecCount;
        private double m_nMinX;
        private double m_nMinY;
        private double m_nMaxX;
        private double m_nMaxY;

        private int m_nInFldCount;
        private String[] m_InFldNames;
        private String[] m_InFldTypes;
        private int[] m_InFldLens;
        private int[] m_InFldDecs;

        private int m_nOutFldCount;
        private int[] m_OutFldIdxs;
        private String m_sOutFldNames;
        private char[] m_OutFldTypes;
        private int[] m_OutFldLens;
        private int[] m_OutFldDecs;

        private int m_nPntsCount;
        private int m_nPartsCount;

        private int m_nAlloced;
        private double[] m_PntsX;
        private double[] m_PntsY;
        private double[] m_PntsZ;
        private int[] m_Parts;

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Public ------------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Public

        /*=== LinWriterF() ===*/
        public LinWriterF()
        {
            m_InSHP = null;
            m_OutSHP = null;

            DoClose();
        }

        /*=== Open() ===*/
        public String Open(String InFileName,
                           String OutFileName,
                           ShpInfoF Info,
                           Cfg cfg)
        {
            m_sInFileName = InFileName;
            m_sOutFileName = OutFileName;
            m_Info = Info;
            m_Cfg = cfg;

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

            Res = DoOpen_GetInHeader();
            if (Res != "OK")
            {
                DoClose();
                return Res;
            }

            Res = DoOpen_SetOutHeader();
            if (Res != "OK")
            {
                DoClose();
                return Res;
            }

            Res = ReadWrite();
            if (Res != "OK")
            {
                DoClose();
                return Res;
            }

            return "OK";
        }

        /*=== DoOpen_GetInHeader() ===*/
        private String DoOpen_GetInHeader()
        {
            bool Res;

            m_nShapeType = 0;
            m_nRecCount = 0;
            m_nMinX = 0.0;
            m_nMinY = 0.0;
            m_nMaxX = 0.0;
            m_nMaxY = 0.0;

            m_nInFldCount = 0;
            m_InFldNames = new String[256];
            m_InFldTypes = new String[256];
            m_InFldLens = new int[256];
            m_InFldDecs = new int[256];

            m_InSHP = new ShpLib();
            Res = m_InSHP.GetGenInfo(m_sInFileName,

                                     ref m_nShapeType,
                                     ref m_nRecCount,
                                     ref m_nMinX,
                                     ref m_nMinY,
                                     ref m_nMaxX,
                                     ref m_nMaxY,

                                     ref m_nInFldCount,
                                     m_InFldNames,
                                     m_InFldTypes,
                                     m_InFldLens,
                                     m_InFldDecs);
            if (!Res)
                return "Error Opening " + m_sInFileName + " Shape File.";

            return "OK";
        }

        /*=== DoOpen_SetOutHeader() ===*/
        private String DoOpen_SetOutHeader()
        {
            bool Res;
            int FldIdx;
            String PrjIn, PrjOut;
            int i;

            m_nOutFldCount = m_nInFldCount + 25;
            if (m_Cfg.m_Data.ObliqueStats)
                m_nOutFldCount += 8;

            m_OutFldIdxs = new int[m_nOutFldCount];
            for (i = 0; i < m_nOutFldCount; i++)
                m_OutFldIdxs[i] = i;

            m_sOutFldNames = "";
            for (FldIdx = 0; FldIdx < m_nInFldCount; FldIdx++)
                m_sOutFldNames += m_InFldNames[FldIdx] + "|";
            m_sOutFldNames += "LIN_L|LIN_R|DIST_L|DIST_R|KAPPA|" +
                              "OVRL_MIN|OVRL_MAX|OVRL_AVG|OVRL_STD|OVRT_MIN|OVRT_MAX|OVRT_AVG|OVRT_STD|" +
                              "GSDA_MIN|GSDA_MAX|GSDA_AVG|GSDA_STD|GSDAVG_MIN|GSDAVG_MAX|GSDAVG_AVG|GSDAVG_STD|" +
                              "HAVG_MIN|HAVG_MAX|HAVG_AVG|HAVG_STD";
            if (m_Cfg.m_Data.ObliqueStats)
                m_sOutFldNames += "|OVROF_MIN|OVROF_MAX|OVROF_AVG|OVROF_STD|OVROL_MIN|OVROL_MAX|OVROL_AVG|OVROL_STD";
            
            m_OutFldTypes = new char[256];
            for (FldIdx = 0; FldIdx < m_nInFldCount; FldIdx++)
                m_OutFldTypes[FldIdx] = char.Parse(m_InFldTypes[FldIdx]);
            m_OutFldTypes[m_nInFldCount] = 'N';
            m_OutFldTypes[m_nInFldCount + 1] = 'N';
            m_OutFldTypes[m_nInFldCount + 2] = 'N';
            m_OutFldTypes[m_nInFldCount + 3] = 'N';
            m_OutFldTypes[m_nInFldCount + 4] = 'N';
            m_OutFldTypes[m_nInFldCount + 5] = 'N';
            m_OutFldTypes[m_nInFldCount + 6] = 'N';
            m_OutFldTypes[m_nInFldCount + 7] = 'N';
            m_OutFldTypes[m_nInFldCount + 8] = 'N';
            m_OutFldTypes[m_nInFldCount + 9] = 'N';
            m_OutFldTypes[m_nInFldCount + 10] = 'N';
            m_OutFldTypes[m_nInFldCount + 11] = 'N';
            m_OutFldTypes[m_nInFldCount + 12] = 'N';
            m_OutFldTypes[m_nInFldCount + 13] = 'N';
            m_OutFldTypes[m_nInFldCount + 14] = 'N';
            m_OutFldTypes[m_nInFldCount + 15] = 'N';
            m_OutFldTypes[m_nInFldCount + 16] = 'N';
            m_OutFldTypes[m_nInFldCount + 17] = 'N';
            m_OutFldTypes[m_nInFldCount + 18] = 'N';
            m_OutFldTypes[m_nInFldCount + 19] = 'N';
            m_OutFldTypes[m_nInFldCount + 20] = 'N';
            m_OutFldTypes[m_nInFldCount + 21] = 'N';
            m_OutFldTypes[m_nInFldCount + 22] = 'N';
            m_OutFldTypes[m_nInFldCount + 23] = 'N';
            m_OutFldTypes[m_nInFldCount + 24] = 'N';
            if (m_Cfg.m_Data.ObliqueStats)
            {
                m_OutFldTypes[m_nInFldCount + 25] = 'N';
                m_OutFldTypes[m_nInFldCount + 26] = 'N';
                m_OutFldTypes[m_nInFldCount + 27] = 'N';
                m_OutFldTypes[m_nInFldCount + 28] = 'N';
                m_OutFldTypes[m_nInFldCount + 29] = 'N';
                m_OutFldTypes[m_nInFldCount + 30] = 'N';
                m_OutFldTypes[m_nInFldCount + 31] = 'N';
                m_OutFldTypes[m_nInFldCount + 32] = 'N';
            }

            m_OutFldLens = new int[256];
            m_OutFldDecs = new int[256];
            for (FldIdx = 0; FldIdx < m_nInFldCount; FldIdx++)
            {
                m_OutFldLens[FldIdx] = m_InFldLens[FldIdx];
                m_OutFldDecs[FldIdx] = m_InFldDecs[FldIdx];
            }
            m_OutFldLens[m_nInFldCount] = 6;
            m_OutFldDecs[m_nInFldCount] = 0;
            m_OutFldLens[m_nInFldCount + 1] = 6;
            m_OutFldDecs[m_nInFldCount + 1] = 0;
            m_OutFldLens[m_nInFldCount + 2] = 8;
            m_OutFldDecs[m_nInFldCount + 2] = 2;
            m_OutFldLens[m_nInFldCount + 3] = 8;
            m_OutFldDecs[m_nInFldCount + 3] = 2;
            m_OutFldLens[m_nInFldCount + 4] = 8;
            m_OutFldDecs[m_nInFldCount + 4] = 2;
            m_OutFldLens[m_nInFldCount + 5] = 8;
            m_OutFldDecs[m_nInFldCount + 5] = 2;
            m_OutFldLens[m_nInFldCount + 6] = 8;
            m_OutFldDecs[m_nInFldCount + 6] = 2;
            m_OutFldLens[m_nInFldCount + 7] = 8;
            m_OutFldDecs[m_nInFldCount + 7] = 2;
            m_OutFldLens[m_nInFldCount + 8] = 8;
            m_OutFldDecs[m_nInFldCount + 8] = 2;
            m_OutFldLens[m_nInFldCount + 9] = 8;
            m_OutFldDecs[m_nInFldCount + 9] = 2;
            m_OutFldLens[m_nInFldCount + 10] = 8;
            m_OutFldDecs[m_nInFldCount + 10] = 2;
            m_OutFldLens[m_nInFldCount + 11] = 8;
            m_OutFldDecs[m_nInFldCount + 11] = 2;
            m_OutFldLens[m_nInFldCount + 12] = 8;
            m_OutFldDecs[m_nInFldCount + 12] = 2;
            m_OutFldLens[m_nInFldCount + 13] = 8;
            m_OutFldDecs[m_nInFldCount + 13] = 2;
            m_OutFldLens[m_nInFldCount + 14] = 8;
            m_OutFldDecs[m_nInFldCount + 14] = 2;
            m_OutFldLens[m_nInFldCount + 15] = 8;
            m_OutFldDecs[m_nInFldCount + 15] = 2;
            m_OutFldLens[m_nInFldCount + 16] = 8;
            m_OutFldDecs[m_nInFldCount + 16] = 2;
            m_OutFldLens[m_nInFldCount + 17] = 8;
            m_OutFldDecs[m_nInFldCount + 17] = 2;
            m_OutFldLens[m_nInFldCount + 18] = 8;
            m_OutFldDecs[m_nInFldCount + 18] = 2;
            m_OutFldLens[m_nInFldCount + 19] = 8;
            m_OutFldDecs[m_nInFldCount + 19] = 2;
            m_OutFldLens[m_nInFldCount + 20] = 8;
            m_OutFldDecs[m_nInFldCount + 20] = 2;
            m_OutFldLens[m_nInFldCount + 21] = 8;
            m_OutFldDecs[m_nInFldCount + 21] = 2;
            m_OutFldLens[m_nInFldCount + 22] = 8;
            m_OutFldDecs[m_nInFldCount + 22] = 2;
            m_OutFldLens[m_nInFldCount + 23] = 8;
            m_OutFldDecs[m_nInFldCount + 23] = 2;
            m_OutFldLens[m_nInFldCount + 24] = 8;
            m_OutFldDecs[m_nInFldCount + 24] = 2;
            if (m_Cfg.m_Data.ObliqueStats)
            {
                m_OutFldLens[m_nInFldCount + 25] = 8;
                m_OutFldDecs[m_nInFldCount + 25] = 2;
                m_OutFldLens[m_nInFldCount + 26] = 8;
                m_OutFldDecs[m_nInFldCount + 26] = 2;
                m_OutFldLens[m_nInFldCount + 27] = 8;
                m_OutFldDecs[m_nInFldCount + 27] = 2;
                m_OutFldLens[m_nInFldCount + 28] = 8;
                m_OutFldDecs[m_nInFldCount + 28] = 2;
                m_OutFldLens[m_nInFldCount + 29] = 8;
                m_OutFldDecs[m_nInFldCount + 29] = 2;
                m_OutFldLens[m_nInFldCount + 30] = 8;
                m_OutFldDecs[m_nInFldCount + 30] = 2;
                m_OutFldLens[m_nInFldCount + 31] = 8;
                m_OutFldDecs[m_nInFldCount + 31] = 2;
                m_OutFldLens[m_nInFldCount + 32] = 8;
                m_OutFldDecs[m_nInFldCount + 32] = 2;
            }

            m_OutSHP = new ShpLib();
            Res = m_OutSHP.CreateShape(m_sOutFileName,

                                       m_nShapeType,
                                       m_nOutFldCount,
                                       m_sOutFldNames,
                                       m_OutFldTypes,
                                       m_OutFldLens,
                                       m_OutFldDecs);
            if (!Res)
                return "Error Creating " + m_sOutFileName + " Shape File.";

            PrjIn = Path.GetDirectoryName(m_sInFileName) + "\\" +
                    Path.GetFileNameWithoutExtension(m_sInFileName) + ".prj";
            PrjOut = Path.GetDirectoryName(m_sOutFileName) + "\\" +
                     Path.GetFileNameWithoutExtension(m_sOutFileName) + ".prj";
            if (File.Exists(PrjOut))
                File.Delete(PrjOut);
            if (File.Exists(PrjIn))
                File.Copy(PrjIn, PrjOut);

            return "OK";
        }

        /*=== DoClose() ===*/
        private void DoClose()
        {
            m_sInFileName = "";
            m_sOutFileName = "";
            m_Info = null;
            m_Cfg = null;

            if (m_InSHP != null)
            {
                m_InSHP.Dispose();
                m_InSHP = null;
            }
            if (m_OutSHP != null)
            {
                m_OutSHP.Dispose();
                m_OutSHP = null;
            }

            m_nShapeType = 0;
            m_nRecCount = 0;
            m_nMinX = 0.0;
            m_nMinY = 0.0;
            m_nMaxX = 0.0;
            m_nMaxY = 0.0;

            m_nInFldCount = 0;
            m_InFldNames = null;
            m_InFldTypes = null;
            m_InFldLens = null;
            m_InFldDecs = null;

            m_nOutFldCount = 0;
            m_OutFldIdxs = null;
            m_sOutFldNames = "";
            m_OutFldTypes = null;
            m_OutFldLens = null;
            m_OutFldDecs = null;

            m_nPntsCount = 0;
            m_nPartsCount = 0;

            m_nAlloced = 0;
            m_PntsX = null;
            m_PntsY = null;
            m_PntsZ = null;
            m_Parts = null;
        }

        #endregion Private Open/Close

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Write -----------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Write

        /*=== ReadWrite() ===*/
        private String ReadWrite()
        {
            bool Res;
            bool FirstGot = false;
            int FldIdx;
            String FldVal;
            int RecIdx;
            String FldVals;

            m_nAlloced = 1000;
            m_PntsX = new double[m_nAlloced];
            m_PntsY = new double[m_nAlloced];
            m_PntsZ = new double[m_nAlloced];
            m_Parts = new int[100];
            m_Parts[0] = 0;

            Res = m_InSHP.GetShpInfo_StartZ(m_sInFileName,
                                            double.MinValue,
                                            double.MinValue,
                                            double.MinValue,
                                            double.MaxValue,
                                            double.MaxValue,
                                            double.MaxValue,
                                            true);
            if (!Res)
                return "Error Reading Shape File " + m_sInFileName + ".";

            // m_OutSHP.BeforeUpdate(m_sOutFileName);
            RecIdx = -1;
            while (true)
            {
                if (!FirstGot)
                {
                    Res = m_InSHP.GetShpInfo_FirstZ(ref m_nPntsCount,
                                                    ref m_nPartsCount);
                    if (!Res)
                        return "Error Reading Shape File " + m_sInFileName + ".";
                    FirstGot = true;
                }
                else
                {
                    Res = m_InSHP.GetShpInfo_NextZ(ref m_nPntsCount,
                                                   ref m_nPartsCount);
                    if (!Res)
                        break;
                }
                ++RecIdx;

                if (m_nPntsCount > m_nAlloced)
                {
                    m_nAlloced = m_nPntsCount + 100;
                    m_PntsX = null;
                    m_PntsY = null;
                    m_PntsZ = null;
                    m_PntsX = new double[m_nAlloced];
                    m_PntsY = new double[m_nAlloced];
                    m_PntsZ = new double[m_nAlloced];
                }

                m_InSHP.GetShpInfo_GeomZ(0,
                                         0,
                                         m_PntsX,
                                         m_PntsY,
                                         m_PntsZ,
                                         m_Parts);
                FldVals = "";
                for (FldIdx = 0; FldIdx < m_nInFldCount; FldIdx++)
                {
                    FldVal = m_InSHP.GetShpInfo_DBF(FldIdx);
                    FldVals += FldVal + "|";
                }
                FldVals += m_Info.m_LinLeftIdxs[RecIdx].ToString() + "|";
                FldVals += m_Info.m_LinRightIdxs[RecIdx].ToString() + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinLeftDist[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinRightDist[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinKappas[RecIdx] * RAD_TO_DEG) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinOverLon_Min[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinOverLon_Max[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinOverLon_Avg[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinOverLon_Std[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinOverTrs_Min[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinOverTrs_Max[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinOverTrs_Avg[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinOverTrs_Std[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinGSDA_Min[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinGSDA_Max[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinGSDA_Avg[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinGSDA_Std[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinGSDAvg_Min[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinGSDAvg_Max[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinGSDAvg_Avg[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinGSDAvg_Std[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinH_Min[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinH_Max[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinH_Avg[RecIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_LinH_Std[RecIdx]);
                if (m_Cfg.m_Data.ObliqueStats)
                {
                    FldVals += "|";
                    FldVals += String.Format("{0:0.00}", m_Info.m_LinOverOblF_Min[RecIdx]) + "|";
                    FldVals += String.Format("{0:0.00}", m_Info.m_LinOverOblF_Max[RecIdx]) + "|";
                    FldVals += String.Format("{0:0.00}", m_Info.m_LinOverOblF_Avg[RecIdx]) + "|";
                    FldVals += String.Format("{0:0.00}", m_Info.m_LinOverOblF_Std[RecIdx]) + "|";
                    FldVals += String.Format("{0:0.00}", m_Info.m_LinOverOblL_Min[RecIdx]) + "|";
                    FldVals += String.Format("{0:0.00}", m_Info.m_LinOverOblL_Max[RecIdx]) + "|";
                    FldVals += String.Format("{0:0.00}", m_Info.m_LinOverOblL_Avg[RecIdx]) + "|";
                    FldVals += String.Format("{0:0.00}", m_Info.m_LinOverOblL_Std[RecIdx]);
                }

                Res = m_OutSHP.ShpLineZ_AddRecs(m_sOutFileName,
                    
                                                1,
                                                new int[] { m_nPntsCount },
                                                m_PntsX,
                                                m_PntsY,
                                                m_PntsZ,
                                                m_nOutFldCount,
                                                m_OutFldIdxs,
                                                FldVals);
                if (!Res)
                    return "Error Writing Shape File " + m_sOutFileName + ".";
            }
            // m_InSHP.GetShpInfo_Stop();
            m_OutSHP.AfterUpdate();

            return "OK";
        }

        #endregion Private Write

        #endregion Private
    }
}
