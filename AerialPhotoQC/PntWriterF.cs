using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AerialPhotoQC
{
    public class PntWriterF
    {
        private String m_sInFileName;
        private String m_sOutFileName;
        private ShpInfoF m_Info;

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
        private String m_sOutFldNames;
        private char[] m_OutFldTypes;
        private int[] m_OutFldLens;
        private int[] m_OutFldDecs;
        private int[] m_OutFldIdxs;

        private int m_nPntsCount;
        private int m_nPartsCount;

        private double[] m_PntsX;
        private double[] m_PntsY;
        private double[] m_PntsZ;
        private int[] m_Parts;

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Public ------------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Public

        /*=== PntWriterF() ===*/
        public PntWriterF()
        {
            m_InSHP = null;
            m_OutSHP = null;

            DoClose();
        }

        /*=== Open() ===*/
        public String Open(String InFileName,
                           String OutFileName,
                           ShpInfoF Info)
        {
            m_sInFileName = InFileName;
            m_sOutFileName = OutFileName;
            m_Info = Info;

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

            m_nOutFldCount = m_nInFldCount + 19;

            m_sOutFldNames = "";
            for (FldIdx = 0; FldIdx < m_nInFldCount; FldIdx++)
                m_sOutFldNames += m_InFldNames[FldIdx] + "|";
            m_sOutFldNames += "LIN_IDX|PIDX_L|PIDX_R|IMG_AREA|" +
                              "OVR_B|OVR_F|OVR_L|OVR_R|OVR_LON|OVR_TRS|" +
                              "GSDA|GSD_MIN|GSD_MAX|GSD_AVG|GSD_STD|" +
                              "H_MIN|H_MAX|H_AVG|H_STD";
            
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

            m_OutFldLens = new int[256];
            m_OutFldDecs = new int[256];
            m_OutFldIdxs = new int[256];
            for (FldIdx = 0; FldIdx < m_nInFldCount; FldIdx++)
            {
                m_OutFldLens[FldIdx] = m_InFldLens[FldIdx];
                m_OutFldDecs[FldIdx] = m_InFldDecs[FldIdx];
                m_OutFldIdxs[FldIdx] = FldIdx;
            }
            m_OutFldLens[m_nInFldCount] = 6;
            m_OutFldDecs[m_nInFldCount] = 0;
            m_OutFldIdxs[m_nInFldCount] = m_nInFldCount;
            m_OutFldLens[m_nInFldCount + 1] = 6;
            m_OutFldDecs[m_nInFldCount + 1] = 0;
            m_OutFldIdxs[m_nInFldCount + 1] = m_nInFldCount + 1;
            m_OutFldLens[m_nInFldCount + 2] = 6;
            m_OutFldDecs[m_nInFldCount + 2] = 0;
            m_OutFldIdxs[m_nInFldCount + 2] = m_nInFldCount + 2;
            m_OutFldLens[m_nInFldCount + 3] = 11;
            m_OutFldDecs[m_nInFldCount + 3] = 2;
            m_OutFldIdxs[m_nInFldCount + 3] = m_nInFldCount + 3;
            m_OutFldLens[m_nInFldCount + 4] = 8;
            m_OutFldDecs[m_nInFldCount + 4] = 2;
            m_OutFldIdxs[m_nInFldCount + 4] = m_nInFldCount + 4;
            m_OutFldLens[m_nInFldCount + 5] = 8;
            m_OutFldDecs[m_nInFldCount + 5] = 2;
            m_OutFldIdxs[m_nInFldCount + 5] = m_nInFldCount + 5;
            m_OutFldLens[m_nInFldCount + 6] = 8;
            m_OutFldDecs[m_nInFldCount + 6] = 2;
            m_OutFldIdxs[m_nInFldCount + 6] = m_nInFldCount + 6;
            m_OutFldLens[m_nInFldCount + 7] = 8;
            m_OutFldDecs[m_nInFldCount + 7] = 2;
            m_OutFldIdxs[m_nInFldCount + 7] = m_nInFldCount + 7;
            m_OutFldLens[m_nInFldCount + 8] = 8;
            m_OutFldDecs[m_nInFldCount + 8] = 2;
            m_OutFldIdxs[m_nInFldCount + 8] = m_nInFldCount + 8;
            m_OutFldLens[m_nInFldCount + 9] = 8;
            m_OutFldDecs[m_nInFldCount + 9] = 2;
            m_OutFldIdxs[m_nInFldCount + 9] = m_nInFldCount + 9;
            m_OutFldLens[m_nInFldCount + 10] = 8;
            m_OutFldDecs[m_nInFldCount + 10] = 2;
            m_OutFldIdxs[m_nInFldCount + 10] = m_nInFldCount + 10;
            m_OutFldLens[m_nInFldCount + 11] = 8;
            m_OutFldDecs[m_nInFldCount + 11] = 2;
            m_OutFldIdxs[m_nInFldCount + 11] = m_nInFldCount + 11;
            m_OutFldLens[m_nInFldCount + 12] = 8;
            m_OutFldDecs[m_nInFldCount + 12] = 2;
            m_OutFldIdxs[m_nInFldCount + 12] = m_nInFldCount + 12;
            m_OutFldLens[m_nInFldCount + 13] = 8;
            m_OutFldDecs[m_nInFldCount + 13] = 2;
            m_OutFldIdxs[m_nInFldCount + 13] = m_nInFldCount + 13;
            m_OutFldLens[m_nInFldCount + 14] = 8;
            m_OutFldDecs[m_nInFldCount + 14] = 2;
            m_OutFldIdxs[m_nInFldCount + 14] = m_nInFldCount + 14;
            m_OutFldLens[m_nInFldCount + 15] = 8;
            m_OutFldDecs[m_nInFldCount + 15] = 2;
            m_OutFldIdxs[m_nInFldCount + 15] = m_nInFldCount + 15;
            m_OutFldLens[m_nInFldCount + 16] = 8;
            m_OutFldDecs[m_nInFldCount + 16] = 2;
            m_OutFldIdxs[m_nInFldCount + 16] = m_nInFldCount + 16;
            m_OutFldLens[m_nInFldCount + 17] = 8;
            m_OutFldDecs[m_nInFldCount + 17] = 2;
            m_OutFldIdxs[m_nInFldCount + 17] = m_nInFldCount + 17;
            m_OutFldLens[m_nInFldCount + 18] = 8;
            m_OutFldDecs[m_nInFldCount + 18] = 2;
            m_OutFldIdxs[m_nInFldCount + 18] = m_nInFldCount + 18;

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
            m_sOutFldNames = "";
            m_OutFldTypes = null;
            m_OutFldLens = null;
            m_OutFldDecs = null;
            m_OutFldIdxs = null;

            m_nPntsCount = 0;
            m_nPartsCount = 0;

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
            int PntLinIdx, PntIdx;

            m_PntsX = new double[1];
            m_PntsY = new double[1];
            m_PntsZ = new double[1];
            m_Parts = new int[1];
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

            RecIdx = -1;
            PntLinIdx = 0;
            PntIdx = -1;
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

                ++PntIdx;
                if (PntIdx == m_Info.m_PntXs[PntLinIdx].Count)
                {
                    ++PntLinIdx;
                    PntIdx = 0;
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
                FldVals += m_Info.m_PntLinIdxs[PntLinIdx].ToString() + "|";
                FldVals += m_Info.m_PntLeftIdxs[PntLinIdx][PntIdx].ToString() + "|";
                FldVals += m_Info.m_PntRightIdxs[PntLinIdx][PntIdx].ToString() + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntImgArea[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntOverB[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntOverF[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntOverL[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntOverR[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntOverLon[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntOverTrs[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntGSDA[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntGSDMin[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntGSDMax[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntGSDAvg[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntGSDStd[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntHMin[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntHMax[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntHAvg[PntLinIdx][PntIdx]) + "|";
                FldVals += String.Format("{0:0.00}", m_Info.m_PntHStd[PntLinIdx][PntIdx]);

                Res = m_OutSHP.ShpPntZ_AddRecs(m_sOutFileName,

                                               1,
                                               m_PntsX,
                                               m_PntsY,
                                               m_PntsZ,
                                               m_nOutFldCount,
                                               m_OutFldIdxs,
                                               FldVals);
                if (!Res)
                    return "Error Writing Shape File " + m_sOutFileName + ".";
            }
            m_InSHP.GetShpInfo_Stop();

            return "OK";
        }

        #endregion Private Write

        #endregion Private
    }
}
