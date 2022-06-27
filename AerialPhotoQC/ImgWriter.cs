using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AerialPhotoQC
{
    public class ImgWriter
    {
        private String m_sPrjFileName;
        private String m_sOutFileName;
        private ShpInfo m_Info;
        private Cfg m_Cfg;

        ShpLib m_OutSHP;

        private int m_nOutFldCount;
        private String m_sOutFldNames;
        private char[] m_OutFldTypes;
        private int[] m_OutFldLens;
        private int[] m_OutFldDecs;
        private int[] m_OutFldIdxs;

        private double[] m_PntsX;
        private double[] m_PntsY;
        private double[] m_PntsZ;
        private int[] m_Parts;

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Public ------------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Public

        /*=== ImgWriter() ===*/
        public ImgWriter()
        {
            m_OutSHP = null;

            DoClose();
        }

        /*=== Open() ===*/
        public String Open(String PrjFileName,
                           String OutFileName,
                           ShpInfo Info,
                           Cfg cfg)
        {
            m_sPrjFileName = PrjFileName;
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

        /*=== DoOpen_SetOutHeader() ===*/
        private String DoOpen_SetOutHeader()
        {
            bool Res;
            String PrjIn, PrjOut;

            m_nOutFldCount = 20;
            if (m_Cfg.m_Data.ObliqueStats)
                m_nOutFldCount += 2;

            m_sOutFldNames = "LIN_IDX|PNT_IDX|PIDX_L|PIDX_R|IMG_AREA|" +
                             "OVR_B|OVR_F|OVR_L|OVR_R|OVR_LON|OVR_TRS|" +
                             "GSDA|GSD_MIN|GSD_MAX|GSD_AVG|GSD_STD|" +
                             "H_MIN|H_MAX|H_AVG|H_STD";
            if (m_Cfg.m_Data.ObliqueStats)
                m_sOutFldNames += "|OVROBL_F|OVROBL_L";
            
            m_OutFldTypes = new char[256];
            m_OutFldTypes[0] = 'N';
            m_OutFldTypes[1] = 'N';
            m_OutFldTypes[2] = 'N';
            m_OutFldTypes[3] = 'N';
            m_OutFldTypes[4] = 'N';
            m_OutFldTypes[5] = 'N';
            m_OutFldTypes[6] = 'N';
            m_OutFldTypes[7] = 'N';
            m_OutFldTypes[8] = 'N';
            m_OutFldTypes[9] = 'N';
            m_OutFldTypes[10] = 'N';
            m_OutFldTypes[11] = 'N';
            m_OutFldTypes[12] = 'N';
            m_OutFldTypes[13] = 'N';
            m_OutFldTypes[14] = 'N';
            m_OutFldTypes[15] = 'N';
            m_OutFldTypes[16] = 'N';
            m_OutFldTypes[17] = 'N';
            m_OutFldTypes[18] = 'N';
            m_OutFldTypes[19] = 'N';
            if (m_Cfg.m_Data.ObliqueStats)
            {
                m_OutFldTypes[20] = 'N';
                m_OutFldTypes[21] = 'N';
            }

            m_OutFldLens = new int[256];
            m_OutFldDecs = new int[256];
            m_OutFldIdxs = new int[256];
            m_OutFldLens[0] = 6;
            m_OutFldDecs[0] = 0;
            m_OutFldIdxs[0] = 0;
            m_OutFldLens[1] = 6;
            m_OutFldDecs[1] = 0;
            m_OutFldIdxs[1] = 1;
            m_OutFldLens[2] = 6;
            m_OutFldDecs[2] = 0;
            m_OutFldIdxs[2] = 2;
            m_OutFldLens[3] = 6;
            m_OutFldDecs[3] = 0;
            m_OutFldIdxs[3] = 3;
            m_OutFldLens[4] = 11;
            m_OutFldDecs[4] = 2;
            m_OutFldIdxs[4] = 4;
            m_OutFldLens[5] = 8;
            m_OutFldDecs[5] = 2;
            m_OutFldIdxs[5] = 5;
            m_OutFldLens[6] = 8;
            m_OutFldDecs[6] = 2;
            m_OutFldIdxs[6] = 6;
            m_OutFldLens[7] = 8;
            m_OutFldDecs[7] = 2;
            m_OutFldIdxs[7] = 7;
            m_OutFldLens[8] = 8;
            m_OutFldDecs[8] = 2;
            m_OutFldIdxs[8] = 8;
            m_OutFldLens[9] = 8;
            m_OutFldDecs[9] = 2;
            m_OutFldIdxs[9] = 9;
            m_OutFldLens[10] = 8;
            m_OutFldDecs[10] = 2;
            m_OutFldIdxs[10] = 10;
            m_OutFldLens[11] = 8;
            m_OutFldDecs[11] = 2;
            m_OutFldIdxs[11] = 11;
            m_OutFldLens[12] = 8;
            m_OutFldDecs[12] = 2;
            m_OutFldIdxs[12] = 12;
            m_OutFldLens[13] = 8;
            m_OutFldDecs[13] = 2;
            m_OutFldIdxs[13] = 13;
            m_OutFldLens[14] = 8;
            m_OutFldDecs[14] = 2;
            m_OutFldIdxs[14] = 14;
            m_OutFldLens[15] = 8;
            m_OutFldDecs[15] = 2;
            m_OutFldIdxs[15] = 15;
            m_OutFldLens[16] = 8;
            m_OutFldDecs[16] = 2;
            m_OutFldIdxs[16] = 16;
            m_OutFldLens[17] = 8;
            m_OutFldDecs[17] = 2;
            m_OutFldIdxs[17] = 17;
            m_OutFldLens[18] = 8;
            m_OutFldDecs[18] = 2;
            m_OutFldIdxs[18] = 18;
            m_OutFldLens[19] = 8;
            m_OutFldDecs[19] = 2;
            m_OutFldIdxs[19] = 19;
            if (m_Cfg.m_Data.ObliqueStats)
            {
                m_OutFldLens[20] = 8;
                m_OutFldDecs[20] = 2;
                m_OutFldIdxs[20] = 20;
                m_OutFldLens[21] = 8;
                m_OutFldDecs[21] = 2;
                m_OutFldIdxs[21] = 21;
            }

            m_OutSHP = new ShpLib();
            Res = m_OutSHP.CreateShape(m_sOutFileName,

                                       ShpLib.SHPTYPE_POLYGONZ,
                                       m_nOutFldCount,
                                       m_sOutFldNames,
                                       m_OutFldTypes,
                                       m_OutFldLens,
                                       m_OutFldDecs);
            if (!Res)
                return "Error Creating " + m_sOutFileName + " Shape File.";

            PrjIn = m_sPrjFileName;
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
            m_sOutFileName = "";
            m_Info = null;
            m_Cfg = null;

            if (m_OutSHP != null)
            {
                m_OutSHP.Dispose();
                m_OutSHP = null;
            }

            m_nOutFldCount = 0;
            m_sOutFldNames = "";
            m_OutFldTypes = null;
            m_OutFldLens = null;
            m_OutFldDecs = null;
            m_OutFldIdxs = null;

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
            String FldVals;
            int PntLinCount, PntLinIdx, PntCount, PntIdx, i;
            double lat, lon;

            m_PntsX = new double[5];
            m_PntsY = new double[5];
            m_PntsZ = new double[5];
            m_Parts = new int[1];
            m_Parts[0] = 0;

            PntLinCount = m_Info.m_PntImgXs.Count;
            for (PntLinIdx = 0; PntLinIdx < PntLinCount; PntLinIdx++)
            {
                PntCount = m_Info.m_PntImgXs[PntLinIdx].Count;
                for (PntIdx = 0; PntIdx < PntCount; PntIdx++)
                {
                    FldVals = m_Info.m_PntLinIdxs[PntLinIdx].ToString() + "|" +
                              PntIdx.ToString() + "|" +
                              m_Info.m_PntLeftIdxs[PntLinIdx][PntIdx].ToString() + "|" +
                              m_Info.m_PntRightIdxs[PntLinIdx][PntIdx].ToString() + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntImgArea[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntOverB[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntOverF[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntOverL[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntOverR[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntOverLon[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntOverTrs[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntGSDA[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntGSDMin[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntGSDMax[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntGSDAvg[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntGSDStd[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntHMin[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntHMax[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntHAvg[PntLinIdx][PntIdx]) + "|" +
                              String.Format("{0:0.00}", m_Info.m_PntHStd[PntLinIdx][PntIdx]);
                              if (m_Cfg.m_Data.ObliqueStats)
                              {
                                  FldVals += "|";
                                  FldVals += String.Format("{0:0.00}", m_Info.m_PntOverOblF[PntLinIdx][PntIdx]) + "|";
                                  FldVals += String.Format("{0:0.00}", m_Info.m_PntOverOblL[PntLinIdx][PntIdx]);
                              }

                    for (i = 0; i < 4; i++)
                    {
                        Util.UTM2GEO(m_Info.m_nUTMZone,
                                     m_Info.m_nUTMNS,

                                     m_Info.m_PntImgXs[PntLinIdx][PntIdx][i],
                                     m_Info.m_PntImgYs[PntLinIdx][PntIdx][i],

                                     out lat,
                                     out lon);
                        m_PntsX[i] = lon;
                        m_PntsY[i] = lat;
                        m_PntsZ[i] = m_Info.m_PntImgZs[PntLinIdx][PntIdx][i];
                    }
                    m_PntsX[4] = m_PntsX[0];
                    m_PntsY[4] = m_PntsY[0];
                    m_PntsZ[4] = m_PntsZ[0];

                    Res = m_OutSHP.ShpPolyZFixedPtsCount_AddRecs(m_sOutFileName,

                                                                 1,
                                                                 5,
                                                                 m_PntsX,
                                                                 m_PntsY,
                                                                 m_PntsZ,
                                                                 m_nOutFldCount,
                                                                 m_OutFldIdxs,
                                                                 FldVals);
                    if (!Res)
                        return "Error Writing Shape File " + m_sOutFileName + ".";
                }
            }


            return "OK";
        }

        #endregion Private Write

        #endregion Private
    }
}
