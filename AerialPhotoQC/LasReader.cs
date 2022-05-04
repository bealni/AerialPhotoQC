using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AerialPhotoQC
{
    public class LasReader
    {
        public const int ERR_NONE = 0;
        public const int ERR_STS_CANT_OPEN = 1;
        public const int ERR_STS_INVALID_FORMAT = 2;
        public const int ERR_PROJID4_IS_NULL = 3;
        public const int ERR_PROJID4_LEN_IS_NOT_8 = 4;
        public const int ERR_SYSID_IS_NULL = 5;
        public const int ERR_SYSID_LEN_IS_NOT_32 = 6;
        public const int ERR_GENSOFT_IS_NULL = 7;
        public const int ERR_GENSOFT_LEN_IS_NOT_32 = 8;
        public const int ERR_CANT_OPEN_UPDATE = 9;
        public const int ERR_NOT_OPEN = 10;

        private LasReader_12 m_LR12;
        private LasReader_14 m_LR14;

        /*=== LasReader ===*/
        public LasReader(String FileName)
        {
            int Status;
            bool Open14 = false;

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
            UInt64[] PntByRetCount;         // [15], as according to version 1.4
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
            UInt64 WDPRecStart;             // VersMin >= 3
            
            // VersMin >= 4
            UInt64 EVLRecStart;
            UInt32 EVLRecCount;

            int VarRecsLen;
            byte[] VarRecsBuf = null;
            int ExtVarRecsLen;
            byte[] ExtVarRecsBuf = null;

            ProjID4 = new byte[8];
            SysID = new byte[32];
            GenSoft = new byte[32];
            PntByRetCount = new UInt64[15];

            m_LR12 = new LasReader_12(FileName);
            Status = m_LR12.GetStatus();
            if (Status != ERR_NONE)
                Open14 = true;

            if (!Open14)
            {
                Status = m_LR12.GetHeader_VLRecsLengths(out VarRecsLen,
                                                        out ExtVarRecsLen);
                if (Status != ERR_NONE)
                    Open14 = true;
                else
                {
                    if (VarRecsLen > 0)
                        VarRecsBuf = new byte[VarRecsLen];
                    if (ExtVarRecsLen > 0)
                        ExtVarRecsBuf = new byte[ExtVarRecsLen];
                    Status = m_LR12.GetHeader_RowInfo(out FileSrcID,
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
                                                      out WDPRecStart,      // VersMin >= 3

                                                      // VersMin >= 4
                                                      out EVLRecStart,
                                                      out EVLRecCount,

                                                      VarRecsBuf,
                                                      ExtVarRecsBuf);

                    if (Status != ERR_NONE)
                    {
                        Open14 = true;
                        VarRecsBuf = null;
                        ExtVarRecsBuf = null;
                    }
                    else
                    if (!(VersMaj == 1 && VersMin < 4))
                    {
                        Open14 = true;
                        VarRecsBuf = null;
                        ExtVarRecsBuf = null;
                    }
                }
            }

            if (Open14)
            {
                m_LR12.Free();
                m_LR12 = null;

                m_LR14 = new LasReader_14(FileName);
                Status = m_LR14.GetStatus();
                if (Status != ERR_NONE)
                {
                    m_LR14.Free();
                    m_LR14 = null;
                    return;
                }

                Status = m_LR14.GetHeader_VLRecsLengths(out VarRecsLen,
                                                        out ExtVarRecsLen);
                if (Status != ERR_NONE)
                {
                    m_LR14.Free();
                    m_LR14 = null;
                    return;
                }

                if (VarRecsLen > 0)
                    VarRecsBuf = new byte[VarRecsLen];
                if (ExtVarRecsLen > 0)
                    ExtVarRecsBuf = new byte[ExtVarRecsLen];

                Status = m_LR14.GetHeader_RowInfo(out FileSrcID,
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

                if (Status != ERR_NONE)
                {
                    m_LR14.Free();
                    m_LR14 = null;
                    VarRecsBuf = null;
                    ExtVarRecsBuf = null;
                    return;
                }
                if (!(VersMaj == 1 && VersMin == 4))
                {
                    m_LR14.Free();
                    m_LR14 = null;
                    VarRecsBuf = null;
                    ExtVarRecsBuf = null;
                    return;
                }
            }
        }

        /*=== Free() ===*/
        public void Free()
        {
            if (m_LR12 != null)
            {
                m_LR12.Free();
                m_LR12 = null;
            }
            if (m_LR14 != null)
            {
                m_LR14.Free();
                m_LR14 = null;
            }
        }

        /*=== GetStatus() ===*/
        public int GetStatus()
        {
            if (m_LR12 != null)
                return m_LR12.GetStatus();
            else
            if (m_LR14 != null)
                return m_LR14.GetStatus();
            else
                return ERR_NOT_OPEN;
        }

        /*=== GetErrStr() ===*/
        public String GetErrStr(int Err)
        {
            if (m_LR12 != null)
                return m_LR12.GetErrStr(Err);
            else
            if (m_LR14 != null)
                return m_LR14.GetErrStr(Err);
            else
                return "LAS is not Open.";
        }

        /*=== SetOffset() ===*/
        public int SetOffset(double OffsetX,
                             double OffsetY,
                             double OffsetZ)
        {
            if (m_LR12 != null)
                return m_LR12.SetOffset(OffsetX,
                                        OffsetY,
                                        OffsetZ);
            else
            if (m_LR14 != null)
                return m_LR14.SetOffset(OffsetX,
                                        OffsetY,
                                        OffsetZ);
            else
                return ERR_NOT_OPEN;
        }

        /*=== GetHeader_VersMaj() ===*/
        public byte GetHeader_VersMaj()
        {
            if (m_LR12 != null)
                return m_LR12.GetHeader_VersMaj();
            else
            if (m_LR14 != null)
                return m_LR14.GetHeader_VersMaj();
            else
                return 0;
        }

        /*=== GetHeader_VersMin() ===*/
        public byte GetHeader_VersMin()
        {
            if (m_LR12 != null)
                return m_LR12.GetHeader_VersMin();
            else
            if (m_LR14 != null)
                return m_LR14.GetHeader_VersMin();
            else
                return 0;
        }

        /*=== GetHeader_SysID() ===*/
        public string GetHeader_SysID()
        {
            if (m_LR12 != null)
                return m_LR12.GetHeader_SysID();
            else
            if (m_LR14 != null)
                return m_LR14.GetHeader_SysID();
            else
                return "";
        }

        /*=== GetHeader_GenSoft() ===*/
        public string GetHeader_GenSoft()
        {
            if (m_LR12 != null)
                return m_LR12.GetHeader_GenSoft();
            else
            if (m_LR14 != null)
                return m_LR14.GetHeader_GenSoft();
            else
                return "";
        }

        /*=== GetHeader_DateTime() ===*/
        public DateTime GetHeader_DateTime()
        {
            if (m_LR12 != null)
                return m_LR12.GetHeader_DateTime();
            else
            if (m_LR14 != null)
                return m_LR14.GetHeader_DateTime();
            else
                return new DateTime(0, 0, 0);
        }

        /*=== GetHeader_PntCount() ===*/
        public UInt64 GetHeader_PntCount()
        {
            if (m_LR12 != null)
                return m_LR12.GetHeader_PntCount();
            else
            if (m_LR14 != null)
                return m_LR14.GetHeader_PntCount();
            else
                return 0;
        }

        /*=== GetHeader_ScalesAndOffsets() ===*/
        public void GetHeader_ScalesAndOffsets(out double XScale,
                                               out double YScale,
                                               out double ZScale,
                                               out double XOffset,
                                               out double YOffset,
                                               out double ZOffset)
        {
            XScale = 0.0;
            YScale = 0.0;
            ZScale = 0.0;
            XOffset = 0.0;
            YOffset = 0.0;
            ZOffset = 0.0;

            if (m_LR12 != null)
                m_LR12.GetHeader_ScalesAndOffsets(out XScale,
                                                  out YScale,
                                                  out ZScale,
                                                  out XOffset,
                                                  out YOffset,
                                                  out ZOffset);
            else
            if (m_LR14 != null)
                m_LR14.GetHeader_ScalesAndOffsets(out XScale,
                                                  out YScale,
                                                  out ZScale,
                                                  out XOffset,
                                                  out YOffset,
                                                  out ZOffset);
        }

        /*=== GetHeader_BB2D() ===*/
        public void GetHeader_BB2D(out double XMin,
                                   out double XMax,
                                   out double YMin,
                                   out double YMax)
        {
            XMin = 0.0;
            XMax = 0.0;
            YMin = 0.0;
            YMax = 0.0;

            if (m_LR12 != null)
                m_LR12.GetHeader_BB2D(out XMin,
                                      out XMax,
                                      out YMin,
                                      out YMax);
            else
            if (m_LR14 != null)
                m_LR14.GetHeader_BB2D(out XMin,
                                      out XMax,
                                      out YMin,
                                      out YMax);
        }

        /*=== GetHeader_BB() ===*/
        public void GetHeader_BB(out double XMin,
                                 out double XMax,
                                 out double YMin,
                                 out double YMax,
                                 out double ZMin,
                                 out double ZMax)
        {
            XMin = 0;
            XMax = 0;
            YMin = 0;
            YMax = 0;
            ZMin = 0;
            ZMax = 0;

            if (m_LR12 != null)
                m_LR12.GetHeader_BB(out XMin,
                                    out XMax,
                                    out YMin,
                                    out YMax,
                                    out ZMin,
                                    out ZMax);
            else
            if (m_LR14 != null)
                m_LR14.GetHeader_BB(out XMin,
                                    out XMax,
                                    out YMin,
                                    out YMax,
                                    out ZMin,
                                    out ZMax);
        }

        /*=== GetHeader_VLRecsLengths() ===*/
        public int GetHeader_VLRecsLengths(out int VarRecsLen,
                                           out int ExtVarRecsLen)
        {
            VarRecsLen = 0;
            ExtVarRecsLen = 0;

            if (m_LR12 != null)
                return m_LR12.GetHeader_VLRecsLengths(out VarRecsLen,
                                                      out ExtVarRecsLen);
            else
            if (m_LR14 != null)
                return m_LR14.GetHeader_VLRecsLengths(out VarRecsLen,
                                                      out ExtVarRecsLen);
            else
                return ERR_NOT_OPEN;
        }

        /*=== GetHeader_RowInfo() ===*/
        public int GetHeader_RowInfo(out UInt16 FileSrcID,          // VersMin >= 1
                                     out UInt16 GlbEnc,             // VersMin >= 2
                                     out UInt32 ProjID1,
                                     out UInt16 ProjID2,
                                     out UInt16 ProjID3,
                                     byte[] ProjID4,                // [8]
                                     out byte VersMaj,
                                     out byte VersMin,
                                     byte[] SysID,                  // [32]
                                     byte[] GenSoft,                // [32]
                                     out UInt16 CrtDayOfYear,
                                     out UInt16 CrtYear,
                                     out UInt16 HdrSize,
                                     out UInt32 OffsetToPntData,
                                     out UInt32 VarRecsCount,
                                     out byte PntFmtID,
                                     out UInt16 PntDataRecLen,
                                     out UInt64 PntDataRecCount,
                                     UInt64[] PntByRetCount,        // VersMin >= 4, [15], else just [5] used
                                     out double XScale,
                                     out double YScale,
                                     out double ZScale,
                                     out double XOffset,
                                     out double YOffset,
                                     out double ZOffset,
                                     out double XMin,
                                     out double XMax,
                                     out double YMin,
                                     out double YMax,
                                     out double ZMin,
                                     out double ZMax,
                                     out UInt64 WDPRecStart,        // VersMin >= 3

                                     // VersMin >= 4
                                     out UInt64 EVLRecStart,
                                     out UInt32 EVLRecCount,

                                     byte[] VarRecsBuf,
                                     byte[] ExtVarRecsBuf)
        {
            FileSrcID = 0;
            GlbEnc = 0;
            ProjID1 = 0;
            ProjID2 = 0;
            ProjID3 = 0;
            VersMaj = 0;
            VersMin = 0;
            CrtDayOfYear = 0;
            CrtYear = 0;
            HdrSize = 0;
            OffsetToPntData = 0;
            VarRecsCount = 0;
            PntFmtID = 0;
            PntDataRecLen = 0;
            PntDataRecCount = 0;
            XScale = 0;
            YScale = 0;
            ZScale = 0;
            XOffset = 0;
            YOffset = 0;
            ZOffset = 0;
            XMin = 0;
            XMax = 0;
            YMin = 0;
            YMax = 0;
            ZMin = 0;
            ZMax = 0;
            WDPRecStart = 0;        // VersMin >= 3

            // VersMin >= 4
            EVLRecStart = 0;
            EVLRecCount = 0;

            if (m_LR12 != null)
                return m_LR12.GetHeader_RowInfo(out FileSrcID,          // VersMin >= 1
                                                out GlbEnc,             // VersMin >= 2
                                                out ProjID1,
                                                out ProjID2,
                                                out ProjID3,
                                                ProjID4,                // [8]
                                                out VersMaj,
                                                out VersMin,
                                                SysID,                  // [32]
                                                GenSoft,                // [32]
                                                out CrtDayOfYear,
                                                out CrtYear,
                                                out HdrSize,
                                                out OffsetToPntData,
                                                out VarRecsCount,
                                                out PntFmtID,
                                                out PntDataRecLen,
                                                out PntDataRecCount,
                                                PntByRetCount,         // VersMin >= 4, [15], else just [5] used
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
                                                out WDPRecStart,       // VersMin >= 3

                                                // VersMin >= 4
                                                out EVLRecStart,
                                                out EVLRecCount,

                                                VarRecsBuf,
                                                ExtVarRecsBuf);
            else
            if (m_LR14 != null)
                return m_LR14.GetHeader_RowInfo(out FileSrcID,          // VersMin >= 1
                                                out GlbEnc,             // VersMin >= 2
                                                out ProjID1,
                                                out ProjID2,
                                                out ProjID3,
                                                ProjID4,                // [8]
                                                out VersMaj,
                                                out VersMin,
                                                SysID,                  // [32]
                                                GenSoft,                // [32]
                                                out CrtDayOfYear,
                                                out CrtYear,
                                                out HdrSize,
                                                out OffsetToPntData,
                                                out VarRecsCount,
                                                out PntFmtID,
                                                out PntDataRecLen,
                                                out PntDataRecCount,
                                                PntByRetCount,         // VersMin >= 4, [15], else just [5] used
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
                                                out WDPRecStart,       // VersMin >= 3

                                                // VersMin >= 4
                                                out EVLRecStart,
                                                out EVLRecCount,

                                                VarRecsBuf,
                                                ExtVarRecsBuf);
            else
                return ERR_NOT_OPEN;
        }

        /*=== GetFirstPoint() ===*/
        public bool GetFirstPoint(out Int32 X,
                                  out Int32 Y,
                                  out Int32 Z)
        {
            X = 0;
            Y = 0;
            Z = 0;

            if (m_LR12 != null)
                return m_LR12.GetFirstPoint(out X,
                                            out Y,
                                            out Z);
            else
            if (m_LR14 != null)
                return m_LR14.GetFirstPoint(out X,
                                            out Y,
                                            out Z);
            else
                return false;
        }

        /*=== GetFirstPoint() ===*/
        public bool GetFirstPoint(out Int32 X,
                                  out Int32 Y,
                                  out Int32 Z,
                                  out UInt16 Intensity,
                                  out byte RetNum,
                                  out byte NumRet,
                                  out byte ScanDir,
                                  out byte ScanAngle,
                                  out UInt16 Red,
                                  out UInt16 Green,
                                  out UInt16 Blue)
        {
            X = 0;
            Y = 0;
            Z = 0;
            Intensity = 0;
            RetNum = 0;
            NumRet = 0;
            ScanDir = 0;
            ScanAngle = 0;
            Red = 0;
            Green = 0;
            Blue = 0;

            if (m_LR12 != null)
                return m_LR12.GetFirstPoint(out X,
                                            out Y,
                                            out Z,
                                            out Intensity,
                                            out RetNum,
                                            out NumRet,
                                            out ScanDir,
                                            out ScanAngle,
                                            out Red,
                                            out Green,
                                            out Blue);
            else
            if (m_LR14 != null)
                return m_LR14.GetFirstPoint(out X,
                                            out Y,
                                            out Z,
                                            out Intensity,
                                            out RetNum,
                                            out NumRet,
                                            out ScanDir,
                                            out ScanAngle,
                                            out Red,
                                            out Green,
                                            out Blue);
            else
                return false;
        }

        /*=== GetFirstPoint() ===*/
        public bool GetFirstPoint(out Int32 X,
                                  out Int32 Y,
                                  out Int32 Z,
                                  out UInt16 Intensity,
                                  out byte RetNum,
                                  out byte NumRet,
                                  out byte ScanDir,
                                  out byte EdgeOfFlightLine,
                                  out byte Classification,
                                  out byte ScanAngle,
                                  out byte UserData,
                                  out UInt16 PointSourceID,
                                  out double GPSTime,
                                  out UInt16 Red,
                                  out UInt16 Green,
                                  out UInt16 Blue,

                                  // 1.4
                                  out byte ClassificationFlag,
                                  out byte ScannerChannel,
                                  out Int16 ScanAngle_14)
        {
            X = 0;
            Y = 0;
            Z = 0;
            Intensity = 0;
            RetNum = 0;
            NumRet = 0;
            ScanDir = 0;
            EdgeOfFlightLine = 0;
            Classification = 0;
            ScanAngle = 0;
            UserData = 0;
            PointSourceID = 0;
            GPSTime = 0.0;
            Red = 0;
            Green = 0;
            Blue = 0;

            // 1.4
            ClassificationFlag = 0;
            ScannerChannel = 0;
            ScanAngle_14 = 0;

            if (m_LR12 != null)
                return m_LR12.GetFirstPoint(out X,
                                            out Y,
                                            out Z,
                                            out Intensity,
                                            out RetNum,
                                            out NumRet,
                                            out ScanDir,
                                            out EdgeOfFlightLine,
                                            out Classification,
                                            out ScanAngle,
                                            out UserData,
                                            out PointSourceID,
                                            out GPSTime,
                                            out Red,
                                            out Green,
                                            out Blue);
            else
            if (m_LR14 != null)
                return m_LR14.GetFirstPoint(out X,
                                            out Y,
                                            out Z,
                                            out Intensity,
                                            out RetNum,
                                            out NumRet,
                                            out ScanDir,
                                            out EdgeOfFlightLine,
                                            out Classification,
                                            out ScanAngle,
                                            out UserData,
                                            out PointSourceID,
                                            out GPSTime,
                                            out Red,
                                            out Green,
                                            out Blue,
                                            
                                            out ClassificationFlag,
                                            out ScannerChannel,
                                            out ScanAngle_14);
            else
                return false;
        }

        /*=== GetNextPoint() ===*/
        public bool GetNextPoint(out Int32 X,
                                 out Int32 Y,
                                 out Int32 Z)
        {
            X = 0;
            Y = 0;
            Z = 0;

            if (m_LR12 != null)
                return m_LR12.GetNextPoint(out X,
                                           out Y,
                                           out Z);
            else
            if (m_LR14 != null)
                return m_LR14.GetNextPoint(out X,
                                           out Y,
                                           out Z);
            else
                return false;
        }

        /*=== GetNextPoint() ===*/
        public bool GetNextPoint(out Int32 X,
                                 out Int32 Y,
                                 out Int32 Z,
                                 out UInt16 Intensity,
                                 out byte RetNum,
                                 out byte NumRet,
                                 out byte ScanDir,
                                 out byte ScanAngle,
                                 out UInt16 Red,
                                 out UInt16 Green,
                                 out UInt16 Blue)
        {
            X = 0;
            Y = 0;
            Z = 0;
            Intensity = 0;
            RetNum = 0;
            NumRet = 0;
            ScanDir = 0;
            ScanAngle = 0;
            Red = 0;
            Green = 0;
            Blue = 0;

            if (m_LR12 != null)
                return m_LR12.GetNextPoint(out X,
                                           out Y,
                                           out Z,
                                           out Intensity,
                                           out RetNum,
                                           out NumRet,
                                           out ScanDir,
                                           out ScanAngle,
                                           out Red,
                                           out Green,
                                           out Blue);
            else
            if (m_LR14 != null)
                return m_LR14.GetNextPoint(out X,
                                           out Y,
                                           out Z,
                                           out Intensity,
                                           out RetNum,
                                           out NumRet,
                                           out ScanDir,
                                           out ScanAngle,
                                           out Red,
                                           out Green,
                                           out Blue);
            else
                return false;
        }

        /*=== GetNextPoint() ===*/
        public bool GetNextPoint(out Int32 X,
                                 out Int32 Y,
                                 out Int32 Z,
                                 out UInt16 Intensity,
                                 out byte RetNum,
                                 out byte NumRet,
                                 out byte ScanDir,
                                 out byte EdgeOfFlightLine,
                                 out byte Classification,
                                 out byte ScanAngle,
                                 out byte UserData,
                                 out UInt16 PointSourceID,
                                 out double GPSTime,
                                 out UInt16 Red,
                                 out UInt16 Green,
                                 out UInt16 Blue,

                                  // 1.4
                                  out byte ClassificationFlag,
                                  out byte ScannerChannel,
                                  out Int16 ScanAngle_14)
        {
            X = 0;
            Y = 0;
            Z = 0;
            Intensity = 0;
            RetNum = 0;
            NumRet = 0;
            ScanDir = 0;
            EdgeOfFlightLine = 0;
            Classification = 0;
            ScanAngle = 0;
            UserData = 0;
            PointSourceID = 0;
            GPSTime = 0.0;
            Red = 0;
            Green = 0;
            Blue = 0;

            // 1.4
            ClassificationFlag = 0;
            ScannerChannel = 0;
            ScanAngle_14 = 0;

            if (m_LR12 != null)
                return m_LR12.GetNextPoint(out X,
                                           out Y,
                                           out Z,
                                           out Intensity,
                                           out RetNum,
                                           out NumRet,
                                           out ScanDir,
                                           out EdgeOfFlightLine,
                                           out Classification,
                                           out ScanAngle,
                                           out UserData,
                                           out PointSourceID,
                                           out GPSTime,
                                           out Red,
                                           out Green,
                                           out Blue);
            else
            if (m_LR14 != null)
                return m_LR14.GetNextPoint(out X,
                                           out Y,
                                           out Z,
                                           out Intensity,
                                           out RetNum,
                                           out NumRet,
                                           out ScanDir,
                                           out EdgeOfFlightLine,
                                           out Classification,
                                           out ScanAngle,
                                           out UserData,
                                           out PointSourceID,
                                           out GPSTime,
                                           out Red,
                                           out Green,
                                           out Blue,

                                           out ClassificationFlag,
                                           out ScannerChannel,
                                           out ScanAngle_14);
            else
                return false;
        }

        /*=== GetLastPoint() ===*/
        public void GetLastPoint()
        {
            if (m_LR12 != null)
                m_LR12.GetLastPoint();
            else
            if (m_LR14 != null)
                m_LR14.GetLastPoint();
        }
    }
}
