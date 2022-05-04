using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AerialPhotoQC
{
    public class DTMInfo
    {
        private double NO_VALUE_LIMIT = -1.0e+10;

        public String m_sLasFileName;
        private double m_nInMinX;
        private double m_nInMinY;
        private double m_nInMaxX;
        private double m_nInMaxY;

        public int m_nUTMZone;
        public int m_nUTMNS;

        public double m_nMinX;
        public double m_nMinY;
        public double m_nMaxX;
        public double m_nMaxY;
        public double m_nStep;
        public int m_nCountX;
        public int m_nCountY;
        public double[,] m_Zs;

        private InputCtrl m_Ctrl;
        private System.Windows.Forms.Label m_Lbl;

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Public ------------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Public

        /*=== DTMInfo() ===*/
        public DTMInfo()
        {
            DoClose();
        }

        /*=== Open() ===*/
        public String Open(String LasFileName,

                           double MinX,
                           double MinY,
                           double MaxX,
                           double MaxY,

                           int UTMZone,
                           int UTMNS,
            
                           InputCtrl Ctrl,
                           System.Windows.Forms.Label Lbl)
        {
            m_sLasFileName = LasFileName;

            m_nInMinX = MinX;
            m_nInMinY = MinY;
            m_nInMaxX = MaxX;
            m_nInMaxY = MaxY;

            m_nUTMZone = UTMZone;
            m_nUTMNS = UTMNS;

            m_Ctrl = Ctrl;
            m_Lbl = Lbl;

            return DoOpen();
        }

        /*=== Close() ===*/
        public void Close()
        {
            DoClose();
        }

        /*=== GetZ() ===*/
        public double GetZ(double X,
                           double Y)
        {
            int idxx, idxy;

            idxx = (int)(Math.Floor((X - m_nMinX) / m_nStep));
            if (idxx < 0)
                idxx = 0;
            else
            if (idxx > m_nCountX - 1)
                idxx = m_nCountX - 1;

            idxy = (int)(Math.Floor((Y - m_nMinY) / m_nStep));
            if (idxy < 0)
                idxy = 0;
            else
            if (idxy > m_nCountY - 1)
                idxy = m_nCountY - 1;

            return m_Zs[idxx, idxy];
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

            Res = DoOpen_Prepare();
            if (Res != "OK")
            {
                DoClose();
                return Res;
            }

            return "OK";
        }

        /*=== DoOpen_Prepare() ===*/
        private String DoOpen_Prepare()
        {
            LasReader LR;

            int Status;

            UInt16 FileSrcID;
            UInt16 GlbEnc;
            UInt32 ProjID1;
            UInt16 ProjID2;
            UInt16 ProjID3;
            byte[] ProjID4;
            byte VersMaj;
            byte VersMin;
            byte[] SysID;
            byte[] GenSoft;
            UInt16 CrtDayOfYear;
            UInt16 CrtYear;
            UInt16 HdrSize;
            UInt32 OffsetToPntData;
            UInt32 VarRecsCount;
            byte PntFmtID;
            UInt16 PntDataRecLen;
            UInt64 PntDataRecCount;
            UInt64[] PntByRetCount;      // [15], as according to version 1.4
            double XScale;
            double YScale;
            double ZScale;
            double XOffset;
            double YOffset;
            double ZOffset;
            double XMin;
            double XMax;
            double YMin;
            double YMax;
            double ZMin;
            double ZMax;
            UInt64 WDPRecStart;      // VersMin >= 3

            // VersMin >= 4
            UInt64 EVLRecStart;
            UInt32 EVLRecCount;

            // Before and After Point Records Block
            int VarRecsLen;
            byte[] VarRecsBuf = null;
            int ExtVarRecsLen;
            byte[] ExtVarRecsBuf = null;

            double x1, y1, x2, y2, x3, y3, x4, y4;
            double UTMMinX;
            double UTMMinY;
            double UTMMaxX;
            double UTMMaxY;
            double UTMStep;
            int idxx, idxy;
            String Res;
            String DTMFileName;

            LR = new LasReader(m_sLasFileName);
            Status = LR.GetStatus();
            if (Status != LasReader.ERR_NONE)
                return "Error Opening Las # " + Status.ToString();
            Status = LR.GetHeader_VLRecsLengths(out VarRecsLen,
                                                  out ExtVarRecsLen);
            if (Status != LasReader.ERR_NONE)
                return "Error Opening Las # " + Status.ToString();

            if (VarRecsLen > 0)
                VarRecsBuf = new byte[VarRecsLen];
            if (ExtVarRecsLen > 0)
                ExtVarRecsBuf = new byte[ExtVarRecsLen];

            ProjID4 = new byte[8];
            SysID = new byte[32];
            GenSoft = new byte[32];
            PntByRetCount = new UInt64[15];

            Status = LR.GetHeader_RowInfo(out FileSrcID,
                                          out GlbEnc,
                                          out ProjID1,
                                          out ProjID2,
                                          out ProjID3,
                                          ProjID4,
                                          out VersMaj,
                                          out VersMin,
                                          SysID,
                                          GenSoft,
                                          out CrtDayOfYear,
                                          out CrtYear,
                                          out HdrSize,
                                          out OffsetToPntData,
                                          out VarRecsCount,
                                          out PntFmtID,
                                          out PntDataRecLen,
                                          out PntDataRecCount,
                                          PntByRetCount,
                                          out XScale,
                                          out YScale,
                                          out ZScale,
                                          out XOffset,
                                          out YOffset,
                                          out ZOffset,
                                          out XMin,
                                          out XMax,
                                          out YMin,
                                          out YMax,
                                          out ZMin,
                                          out ZMax,
                                          out WDPRecStart,        // VersMin >= 3
                                          
                                          // VersMin >= 4
                                          out EVLRecStart,
                                          out EVLRecCount,

                                          VarRecsBuf,
                                          ExtVarRecsBuf);
            if (Status != LasReader.ERR_NONE)
            {
                ProjID4 = null;
                SysID = null;
                GenSoft = null;
                PntByRetCount = null;
                VarRecsBuf = null;
                ExtVarRecsBuf = null;
                LR.Free();
                LR = null;
                return "Error Opening Las # " + Status.ToString();
            }

            Util.GEO2UTM(m_nUTMZone,
                         m_nUTMNS,

                         YMin,
                         XMin,

                         out x1,
                         out y1);
            Util.GEO2UTM(m_nUTMZone,
                         m_nUTMNS,

                         YMin,
                         XMax,

                         out x2,
                         out y2);
            Util.GEO2UTM(m_nUTMZone,
                         m_nUTMNS,

                         YMax,
                         XMax,

                         out x3,
                         out y3);
            Util.GEO2UTM(m_nUTMZone,
                         m_nUTMNS,

                         YMax,
                         XMin,

                         out x4,
                         out y4);

            UTMMinX = Math.Min(x1, Math.Min(x2, Math.Min(x3, x4)));
            UTMMinY = Math.Min(y1, Math.Min(y2, Math.Min(y3, y4)));
            UTMMaxX = Math.Max(x1, Math.Max(x2, Math.Max(x3, x4)));
            UTMMaxY = Math.Max(y1, Math.Max(y2, Math.Max(y3, y4)));
            UTMStep = Math.Sqrt(((UTMMaxX - UTMMinX) * (UTMMaxY - UTMMinY)) / (double)PntDataRecCount);

            m_nStep = (double)((int)Math.Ceiling(0.1 * UTMStep)) * 10.0;
            m_nMinX = Math.Floor(m_nInMinX);
            m_nMinY = Math.Floor(m_nInMinY);
            m_nMaxX = Math.Ceiling(m_nInMaxX);
            m_nMaxY = Math.Ceiling(m_nInMaxY);

            m_nCountX = (int)(Math.Ceiling((m_nMaxX - m_nMinX) / m_nStep)) + 1;
            m_nCountY = (int)(Math.Ceiling((m_nMaxY - m_nMinY) / m_nStep)) + 1;

            m_nMaxX = m_nMinX + m_nStep * (double)(m_nCountX - 1);
            m_nMaxY = m_nMinY + m_nStep * (double)(m_nCountY - 1);

            m_Zs = new double[m_nCountX, m_nCountY];
            for (idxx = 0; idxx < m_nCountX; idxx++)
            {
                for (idxy = 0; idxy < m_nCountY; idxy++)
                    m_Zs[idxx, idxy] = double.MinValue;
            }

            DTMFileName = Path.GetDirectoryName(m_sLasFileName) + "\\" +
                          Path.GetFileNameWithoutExtension(m_sLasFileName) + ".dtm";
            if (File.Exists(DTMFileName))
            {
                if (DoOpen_ReadZs())
                {
                    ProjID4 = null;
                    SysID = null;
                    GenSoft = null;
                    PntByRetCount = null;
                    VarRecsBuf = null;
                    ExtVarRecsBuf = null;
                    LR.Free();
                    LR = null;

                    return "OK";
                }
            }

            Res = DoOpen_FillZs(LR,
                                XOffset,
                                YOffset,
                                ZOffset,
                                XScale,
                                YScale,
                                ZScale,
                                (int)PntDataRecCount);
            if (Res != "OK")
            {
                ProjID4 = null;
                SysID = null;
                GenSoft = null;
                PntByRetCount = null;
                VarRecsBuf = null;
                ExtVarRecsBuf = null;
                LR.Free();
                LR = null;

                return Res;
            }

            ProjID4 = null;
            SysID = null;
            GenSoft = null;
            PntByRetCount = null;
            VarRecsBuf = null;
            ExtVarRecsBuf = null;
            LR.Free();
            LR = null;

            return "OK";
        }

        /*=== DoOpen_FillZs() ===*/
        private String DoOpen_FillZs(LasReader LR,
                                     double XOffset,
                                     double YOffset,
                                     double ZOffset,
                                     double XScale,
                                     double YScale,
                                     double ZScale,
                                     int RecCount)
        {
            bool Res;
            int iX, iY, iZ;
            double X, Y, Z;
            double x, y;
            int idxx, idxy;
            bool FirstGot = false;
            int RecIdx;
            String Total = ((int)((double)RecCount / 1000000.0)).ToString() + "M";

            RecIdx = 0;
            while (true)
            {
                if (!FirstGot)
                {
                    Res = LR.GetFirstPoint(out iX,
                                           out iY,
                                           out iZ);
                    if (!Res)
                        return "Error Reading DTM.";
                    FirstGot = true;
                }
                else
                {
                    Res = LR.GetNextPoint(out iX,
                                          out iY,
                                          out iZ);
                    if (!Res)
                        break;
                }
                ++RecIdx;
                if (RecIdx % 1000000 == 0)
                {
                    m_Lbl.Text = "Reading DTM: " + (RecIdx / 1000000).ToString() + " of " + Total;
                    System.Windows.Forms.Application.DoEvents();
                    m_Ctrl.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                }

                X = (double)(iX) * XScale + XOffset;
                Y = (double)(iY) * YScale + YOffset;
                Z = (double)(iZ) * ZScale + ZOffset;

                Util.GEO2UTM(m_nUTMZone,
                             m_nUTMNS,

                             Y,
                             X,

                             out x,
                             out y);
                if (x < m_nMinX || x > m_nMaxX + m_nStep)
                    continue;
                if (y < m_nMinY || y > m_nMaxY + m_nStep)
                    continue;

                idxx = (int)(Math.Floor((x - m_nMinX) / m_nStep));
                if (idxx < 0 || idxx > m_nCountX - 1)
                    continue;
                idxy = (int)(Math.Floor((y - m_nMinY) / m_nStep));
                if (idxy < 0 || idxy > m_nCountY - 1)
                    continue;

                m_Zs[idxx, idxy] = Z;
            }

            DoOpen_FillNoData();

            DoOpen_WriteZs();

            return "OK";
        }

        /*=== DoClose() ===*/
        private void DoClose()
        {
            m_sLasFileName = "";
            m_nInMinX = 0.0;
            m_nInMinY = 0.0;
            m_nInMaxX = 0.0;
            m_nInMaxY = 0.0;

            m_nUTMZone = 0;
            m_nUTMNS = 0;

            m_nMinX = 0.0;
            m_nMinY = 0.0;
            m_nMaxX = 0.0;
            m_nMaxY = 0.0;
            m_nStep = 0.0;
            m_nCountX = 0;
            m_nCountY = 0;
            m_Zs = null;

            m_Ctrl = null;
            m_Lbl = null;
        }

        #endregion Private Open/Close

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Read/Write Zs ---------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Read/Write Zs

        /*=== DoOpen_FillNoData() ===*/
        private void DoOpen_FillNoData()
        {
            int idxx, idxy;
            for (idxx = 0; idxx < m_nCountX; idxx++)
            {
                for (idxy = 0; idxy < m_nCountY; idxy++)
                {
                    if (m_Zs[idxx, idxy] < NO_VALUE_LIMIT)
                        DoOpen_FillNoData(idxx, idxy);
                }
            }
        }

        /*=== DoOpen_FillNoData() ===*/
        private void DoOpen_FillNoData(int IdxX,
                                       int IdxY)
        {
            int d, dx, dy;
            int idxx, idxy;
            bool Found;

            d = 1;
            while (true)
            {
                idxy = IdxY - d;
                if (idxy >= 0 && idxy <= m_nCountY - 1)
                {
                    Found = false;
                    for (dx = -d; dx <= d; dx++)
                    {
                        idxx = IdxX + dx;
                        if (idxx < 0 || idxx > m_nCountX)
                            continue;

                        if (m_Zs[idxx, idxy] > NO_VALUE_LIMIT)
                        {
                            m_Zs[IdxX, IdxY] = m_Zs[idxx, idxy];
                            Found = true;
                            break;
                        }
                    }
                    if (Found)
                        break;
                }

                idxy = IdxY + d;
                if (idxy >= 0 && idxy <= m_nCountY - 1)
                {
                    Found = false;
                    for (dx = -d; dx <= d; dx++)
                    {
                        idxx = IdxX + dx;
                        if (idxx < 0 || idxx > m_nCountX)
                            continue;

                        if (m_Zs[idxx, idxy] > NO_VALUE_LIMIT)
                        {
                            m_Zs[IdxX, IdxY] = m_Zs[idxx, idxy];
                            Found = true;
                            break;
                        }
                    }
                    if (Found)
                        break;
                }

                idxx = IdxX - d;
                if (idxx >= 0 && idxx <= m_nCountX - 1)
                {
                    Found = false;
                    for (dy = -d; dy <= d; dy++)
                    {
                        idxy = IdxY + dy;
                        if (idxy < 0 || idxy > m_nCountY)
                            continue;

                        if (m_Zs[idxx, idxy] > NO_VALUE_LIMIT)
                        {
                            m_Zs[IdxX, IdxY] = m_Zs[idxx, idxy];
                            Found = true;
                            break;
                        }
                    }
                    if (Found)
                        break;
                }

                idxx = IdxX + d;
                if (idxx >= 0 && idxx <= m_nCountX - 1)
                {
                    Found = false;
                    for (dy = -d; dy <= d; dy++)
                    {
                        idxy = IdxY + dy;
                        if (idxy < 0 || idxy > m_nCountY)
                            continue;

                        if (m_Zs[idxx, idxy] > NO_VALUE_LIMIT)
                        {
                            m_Zs[IdxX, IdxY] = m_Zs[idxx, idxy];
                            Found = true;
                            break;
                        }
                    }
                    if (Found)
                        break;
                }

                ++d;
            }
        }

        /*=== DoOpen_ReadZs() ===*/
        private bool DoOpen_ReadZs()
        {
            String DTMFileName = Path.GetDirectoryName(m_sLasFileName) + "\\" +
                                 Path.GetFileNameWithoutExtension(m_sLasFileName) + ".dtm";
            BinaryReader BR = null;
            double MinX;
            double MinY;
            double MaxX;
            double MaxY;
            double Step;
            int CountX;
            int CountY;
            int idxx, idxy;

            m_Lbl.Text = "Reading DTM...";
            System.Windows.Forms.Application.DoEvents();
            m_Ctrl.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                BR = new BinaryReader(File.Open(DTMFileName, FileMode.Open));

                MinX = BR.ReadDouble();
                MinY = BR.ReadDouble();
                MaxX = BR.ReadDouble();
                MaxY = BR.ReadDouble();
                Step = BR.ReadDouble();
                CountX = BR.ReadInt32();
                CountY = BR.ReadInt32();

                if (!((int)MinX == (int)m_nMinX &&
                      (int)MinY == (int)m_nMinY &&
                      (int)MaxX == (int)m_nMaxX &&
                      (int)MaxY == (int)m_nMaxY &&
                      (int)Step == (int)m_nStep &&
                      CountX == m_nCountX &&
                      CountY == m_nCountY))
                {
                    BR.Close();
                    BR.Dispose();
                    BR = null;

                    if (File.Exists(DTMFileName))
                        File.Delete(DTMFileName);

                    return false;
                }

                for (idxx = 0; idxx < m_nCountX; idxx++)
                {
                    for (idxy = 0; idxy < m_nCountY; idxy++)
                        m_Zs[idxx, idxy] = BR.ReadDouble();
                }

                BR.Close();
                BR.Dispose();
                BR = null;
            }
            catch (Exception)
            {
                if (BR != null)
                {
                    BR.Close();
                    BR.Dispose();
                    BR = null;
                }

                if (File.Exists(DTMFileName))
                    File.Delete(DTMFileName);

                return false;
            }

            return true;
        }

        /*=== DoOpen_WriteZs() ===*/
        private bool DoOpen_WriteZs()
        {
            String DTMFileName = Path.GetDirectoryName(m_sLasFileName) + "\\" +
                                 Path.GetFileNameWithoutExtension(m_sLasFileName) + ".dtm";
            BinaryWriter BW = null;
            int idxx, idxy;

            m_Lbl.Text = "Writing DTM...";
            System.Windows.Forms.Application.DoEvents();
            m_Ctrl.Cursor = System.Windows.Forms.Cursors.WaitCursor;
            try
            {
                BW = new BinaryWriter(File.Open(DTMFileName, FileMode.Create));

                BW.Write(m_nMinX);
                BW.Write(m_nMinY);
                BW.Write(m_nMaxX);
                BW.Write(m_nMaxY);
                BW.Write(m_nStep);
                BW.Write(m_nCountX);
                BW.Write(m_nCountY);
                for (idxx = 0; idxx < m_nCountX; idxx++)
                {
                    for (idxy = 0; idxy < m_nCountY; idxy++)
                        BW.Write(m_Zs[idxx, idxy]);
                }

                BW.Close();
                BW.Dispose();
                BW = null;
            }
            catch (Exception)
            {
                if (BW != null)
                {
                    BW.Close();
                    BW.Dispose();
                    BW = null;
                }

                if (File.Exists(DTMFileName))
                    File.Delete(DTMFileName);

                return false;
            }

            return true;
        }

        #endregion Private Read/Write Zs

        #endregion Private
    }
}
