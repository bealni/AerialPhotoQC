using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AerialPhotoQC
{
    public class LasReader_14
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

        private const byte ID_L = 76;
        private const byte ID_A = 65;
        private const byte ID_S = 83;
        private const byte ID_F = 70;

        private const int HEADER_SIZE = 375;
        private const long OFFSETS_OFFSET = 155L;

        private String m_sFileName;
        private int m_nStatus;
        BinaryReader m_BR;

        // Row Header Info
        private UInt16 m_nHDR_FileSrcID;                        // VersMin >= 1
        private UInt16 m_nHDR_GlbEnc;                           // VersMin >= 2
        private UInt32 m_nHDR_ProjID1;
        private UInt16 m_nHDR_ProjID2;
        private UInt16 m_nHDR_ProjID3;
        private byte[] m_nHDR_ProjID4;                          // [8]
        private byte m_nHDR_VersMaj;
        private byte m_nHDR_VersMin;
        private byte[] m_nHDR_SysID;                            // [32]
        private byte[] m_nHDR_GenSoft;                          // [32]
        private UInt16 m_nHDR_CrtDayOfYear;
        private UInt16 m_nHDR_CrtYear;
        private UInt16 m_nHDR_HdrSize;
        private UInt32 m_nHDR_OffsetToPntData;
        private UInt32 m_nHDR_VarRecsCount;
        private byte m_nHDR_PntFmtID;
        private UInt16 m_nHDR_PntDataRecLen;
        private UInt64 m_nHDR_PntDataRecCount;
        private UInt64[] m_nHDR_PntByRetCount;                  // VersMin >= 4, [15], else just [5] used
        private double m_nHDR_XScale;
        private double m_nHDR_YScale;
        private double m_nHDR_ZScale;
        private double m_nHDR_XOffset;
        private double m_nHDR_YOffset;
        private double m_nHDR_ZOffset;
        private double m_nHDR_XMin;
        private double m_nHDR_XMax;
        private double m_nHDR_YMin;
        private double m_nHDR_YMax;
        private double m_nHDR_ZMin;
        private double m_nHDR_ZMax;
        private UInt64 m_nHDR_WDPRecStart;                      // VersMin >= 3

        // VersMin >= 4
        private UInt64 m_nHDR_EVLRecStart;
        private UInt32 m_nHDR_EVLRecCount;

        // Derivated Header Info
        private string m_sHDR_SysID;
        private string m_sHDR_GenSoft;
        private DateTime m_dtHDR_DateTime;

        // Before and After Point Records Block
        public int m_nVarRecsLen;
        public byte[] m_VarRecsBuf;
        public int m_nExtVarRecsLen;
        public byte[] m_ExtVarRecsBuf;

        private int m_nPntRecIdx;

        /*=== Constructor ===*/
        public LasReader_14(String FileName)
        {
            m_sFileName = FileName;
            m_BR = null;
            m_nStatus = ReadHeader();
        }

        /*=== Free() ===*/
        public void Free()
        {
            m_nHDR_ProjID4 = null;
            m_nHDR_SysID = null;
            m_nHDR_GenSoft = null;
            m_nHDR_PntByRetCount = null;
        }

        /*=== GetStatus() ===*/
        public int GetStatus()
        {
            return m_nStatus;
        }

        /*=== GetErrStr() ===*/
        public String GetErrStr(int Err)
        {
            switch (Err)
            {
                case ERR_NONE:
                    return "OK";
                case ERR_STS_CANT_OPEN:
                    return "Could not open.";
                case ERR_STS_INVALID_FORMAT:
                    return "Invalid format.";
                case ERR_PROJID4_IS_NULL:
                    return "PROJID4 is NULL.";
                case ERR_PROJID4_LEN_IS_NOT_8:
                    return "PROJID4 length is not 8.";
                case ERR_SYSID_IS_NULL:
                    return "SYSID is NULL.";
                case ERR_SYSID_LEN_IS_NOT_32:
                    return "SYSID length is not 32.";
                case ERR_GENSOFT_IS_NULL:
                    return "GENSOFT is NULL.";
                case ERR_GENSOFT_LEN_IS_NOT_32:
                    return "GENSOFT length is not 32.";
                case ERR_CANT_OPEN_UPDATE:
                    return "Could not open for update.";
                case ERR_NOT_OPEN:
                    return "LAS is not Open.";
                default:
                    return "Unknown Error.";
            }
        }

        /*=== SetOffset() ===*/
        public int SetOffset(double OffsetX,
                             double OffsetY,
                             double OffsetZ)
        {
            long offset, offs;

            if (m_nStatus != ERR_NONE)
                return m_nStatus;

            BinaryWriter BW = new BinaryWriter(File.Open(m_sFileName, FileMode.Open));

            offs = OFFSETS_OFFSET;

            offset = BW.BaseStream.Seek(offs, SeekOrigin.Begin);
            if (offset != offs)
            {
                BW.Close();
                BW.Dispose();
                BW = null;
                return ERR_CANT_OPEN_UPDATE;
            }

            m_nHDR_XMin += (OffsetX - m_nHDR_XOffset);
            m_nHDR_XMax += (OffsetX - m_nHDR_XOffset);
            m_nHDR_YMin += (OffsetY - m_nHDR_YOffset);
            m_nHDR_YMax += (OffsetY - m_nHDR_YOffset);
            m_nHDR_ZMin += (OffsetZ - m_nHDR_ZOffset);
            m_nHDR_ZMax += (OffsetZ - m_nHDR_ZOffset);
            m_nHDR_XOffset = OffsetX;
            m_nHDR_YOffset = OffsetY;
            m_nHDR_ZOffset = OffsetZ;

            BW.Write(m_nHDR_XOffset);
            BW.Write(m_nHDR_YOffset);
            BW.Write(m_nHDR_ZOffset);
            BW.Write(m_nHDR_XMax);
            BW.Write(m_nHDR_XMin);
            BW.Write(m_nHDR_YMax);
            BW.Write(m_nHDR_YMin);
            BW.Write(m_nHDR_ZMax);
            BW.Write(m_nHDR_ZMin);

            BW.Close();
            BW.Dispose();
            BW = null;

            return ERR_NONE;
        }

        /*=== GetHeader_VersMaj() ===*/
        public byte GetHeader_VersMaj()
        {
            if (m_nStatus != ERR_NONE)
                return 0;

            return m_nHDR_VersMaj;
        }

        /*=== GetHeader_VersMin() ===*/
        public byte GetHeader_VersMin()
        {
            if (m_nStatus != ERR_NONE)
                return 0;

            return m_nHDR_VersMin;
        }

        /*=== GetHeader_SysID() ===*/
        public string GetHeader_SysID()
        {
            if (m_nStatus != ERR_NONE)
                return "";

            return m_sHDR_SysID;
        }

        /*=== GetHeader_GenSoft() ===*/
        public string GetHeader_GenSoft()
        {
            if (m_nStatus != ERR_NONE)
                return "";

            return m_sHDR_GenSoft;
        }

        /*=== GetHeader_DateTime() ===*/
        public DateTime GetHeader_DateTime()
        {
            if (m_nStatus != ERR_NONE)
                return new DateTime(0, 0, 0);

            return m_dtHDR_DateTime;
        }

        /*=== GetHeader_PntCount() ===*/
        public UInt64 GetHeader_PntCount()
        {
            if (m_nStatus != ERR_NONE)
                return 0;

            return m_nHDR_PntDataRecCount;
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

            if (m_nStatus != ERR_NONE)
                return;

            XScale = m_nHDR_XScale;
            YScale = m_nHDR_YScale;
            ZScale = m_nHDR_ZScale;
            XOffset = m_nHDR_XOffset;
            YOffset = m_nHDR_YOffset;
            ZOffset = m_nHDR_ZOffset;
        }

        /*=== GetHeader_BB2D() ===*/
        public void GetHeader_BB2D(out double XMin,
                                   out double XMax,
                                   out double YMin,
                                   out double YMax)
        {
            double ZMin, ZMax;

            GetHeader_BB(out XMin,
                         out XMax,
                         out YMin,
                         out YMax,
                         out ZMin,
                         out ZMax);
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

            if (m_nStatus != ERR_NONE)
                return;

            XMin = m_nHDR_XMin;
            XMax = m_nHDR_XMax;
            YMin = m_nHDR_YMin;
            YMax = m_nHDR_YMax;
            ZMin = m_nHDR_ZMin;
            ZMax = m_nHDR_ZMax;
        }

        /*=== GetHeader_VLRecsLengths() ===*/
        public int GetHeader_VLRecsLengths(out int VarRecsLen,
                                           out int ExtVarRecsLen)
        {
            VarRecsLen = 0;
            ExtVarRecsLen = 0;

            if (m_nStatus != ERR_NONE)
                return m_nStatus;

            VarRecsLen = m_nVarRecsLen;
            ExtVarRecsLen = m_nExtVarRecsLen;

            return ERR_NONE;
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
            int i;

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

            if (m_nStatus != ERR_NONE)
                return m_nStatus;

            if (ProjID4 == null)
                return ERR_PROJID4_IS_NULL;
            if (ProjID4.Length != 8)
                return ERR_PROJID4_LEN_IS_NOT_8;

            if (SysID == null)
                return ERR_SYSID_IS_NULL;
            if (SysID.Length != 32)
                return ERR_SYSID_LEN_IS_NOT_32;

            if (GenSoft == null)
                return ERR_GENSOFT_IS_NULL;
            if (GenSoft.Length != 32)
                return ERR_GENSOFT_LEN_IS_NOT_32;

            FileSrcID = m_nHDR_FileSrcID;
            GlbEnc = m_nHDR_GlbEnc;
            ProjID1 = m_nHDR_ProjID1;
            ProjID2 = m_nHDR_ProjID2;
            ProjID3 = m_nHDR_ProjID3;
            for (i = 0; i < 8; i++)
                ProjID4[i] = m_nHDR_ProjID4[i];
            VersMaj = m_nHDR_VersMaj;
            VersMin = m_nHDR_VersMin;
            for (i = 0; i < 32; i++)
                SysID[i] = m_nHDR_SysID[i];
            for (i = 0; i < 32; i++)
                GenSoft[i] = m_nHDR_GenSoft[i];
            CrtDayOfYear = m_nHDR_CrtDayOfYear;
            CrtYear = m_nHDR_CrtYear;
            HdrSize = m_nHDR_HdrSize;
            OffsetToPntData = m_nHDR_OffsetToPntData;
            VarRecsCount = m_nHDR_VarRecsCount;
            PntFmtID = m_nHDR_PntFmtID;
            PntDataRecLen = m_nHDR_PntDataRecLen;
            PntDataRecCount = m_nHDR_PntDataRecCount;
            for (i = 0; i < 15; i++)
                PntByRetCount[i] = m_nHDR_PntByRetCount[i];
            XScale = m_nHDR_XScale;
            YScale = m_nHDR_YScale;
            ZScale = m_nHDR_ZScale;
            XOffset = m_nHDR_XOffset;
            YOffset = m_nHDR_YOffset;
            ZOffset = m_nHDR_ZOffset;
            XMin = m_nHDR_XMin;
            XMax = m_nHDR_XMax;
            YMin = m_nHDR_YMin;
            YMax = m_nHDR_YMax;
            ZMin = m_nHDR_ZMin;
            ZMax = m_nHDR_ZMax;

            WDPRecStart = m_nHDR_WDPRecStart;       // VersMin >= 3

            // VersMin >= 4
            EVLRecStart = m_nHDR_EVLRecStart;
            EVLRecCount = m_nHDR_EVLRecCount;

            if (m_nVarRecsLen > 0 && VarRecsBuf != null)
            {
                for (i = 0; i < m_nVarRecsLen; i++)
                    VarRecsBuf[i] = m_VarRecsBuf[i];
            }
            if (m_nExtVarRecsLen > 0 && ExtVarRecsBuf != null)
            {
                for (i = 0; i < m_nExtVarRecsLen; i++)
                    ExtVarRecsBuf[i] = m_ExtVarRecsBuf[i];
            }

            return ERR_NONE;
        }

        /*=== GetFirstPoint() ===*/
        public bool GetFirstPoint(out Int32 X,
                                  out Int32 Y,
                                  out Int32 Z)
        {
            long offset;

            X = 0;
            Y = 0;
            Z = 0;

            if (m_nStatus != ERR_NONE)
                return false;
            if (m_nHDR_PntDataRecCount < 1)
                return false;

            try
            {
                m_BR = new BinaryReader(File.Open(m_sFileName, FileMode.Open));
            }
            catch (Exception)
            {
                return false;
            }
            if (m_BR == null)
                return false;

            offset = m_BR.BaseStream.Seek((long)m_nHDR_OffsetToPntData, SeekOrigin.Begin);
            if (offset != (long)m_nHDR_OffsetToPntData)
            {
                m_BR.Close();
                m_BR.Dispose();
                m_BR = null;
                return false;
            }

            m_nPntRecIdx = 0;
            return GetNextPoint(out X,
                                out Y,
                                out Z);
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
            long offset;

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

            if (m_nStatus != ERR_NONE)
                return false;
            if (m_nHDR_PntDataRecCount < 1)
                return false;

            try
            {
                m_BR = new BinaryReader(File.Open(m_sFileName, FileMode.Open));
            }
            catch (Exception)
            {
                return false;
            }
            if (m_BR == null)
                return false;

            offset = m_BR.BaseStream.Seek((long)m_nHDR_OffsetToPntData, SeekOrigin.Begin);
            if (offset != (long)m_nHDR_OffsetToPntData)
            {
                m_BR.Close();
                m_BR.Dispose();
                m_BR = null;
                return false;
            }

            m_nPntRecIdx = 0;
            return GetNextPoint(out X,
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
            long offset;

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

            if (m_nStatus != ERR_NONE)
                return false;
            if (m_nHDR_PntDataRecCount < 1)
                return false;

            try
            {
                m_BR = new BinaryReader(File.Open(m_sFileName, FileMode.Open));
            }
            catch (Exception)
            {
                return false;
            }
            if (m_BR == null)
                return false;

            offset = m_BR.BaseStream.Seek((long)m_nHDR_OffsetToPntData, SeekOrigin.Begin);
            if (offset != (long)m_nHDR_OffsetToPntData)
            {
                m_BR.Close();
                m_BR.Dispose();
                m_BR = null;
                return false;
            }

            m_nPntRecIdx = 0;
            return GetNextPoint(out X,
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

                                // 1.4
                                out ClassificationFlag,
                                out ScannerChannel,
                                out ScanAngle_14);
        }

        /*=== GetNextPoint() ===*/
        public bool GetNextPoint(out Int32 X,
                                 out Int32 Y,
                                 out Int32 Z)
        {
            X = 0;
            Y = 0;
            Z = 0;

            if (m_nPntRecIdx == (int)m_nHDR_PntDataRecCount)
                return false;

            switch (m_nHDR_PntFmtID)
            {
                case 0:
                    return GetNextPoint_Type0(out X,
                                              out Y,
                                              out Z);
                case 1:
                    return GetNextPoint_Type1(out X,
                                              out Y,
                                              out Z);
                case 2:
                    return GetNextPoint_Type2(out X,
                                              out Y,
                                              out Z);
                case 3:
                    return GetNextPoint_Type3(out X,
                                              out Y,
                                              out Z);
                case 4:
                    return GetNextPoint_Type4(out X,
                                              out Y,
                                              out Z);
                case 5:
                    return GetNextPoint_Type5(out X,
                                              out Y,
                                              out Z);
                case 6:
                    return GetNextPoint_Type6(out X,
                                              out Y,
                                              out Z);

                case 7:
                    return GetNextPoint_Type7(out X,
                                              out Y,
                                              out Z);

                case 8:
                    return GetNextPoint_Type8(out X,
                                              out Y,
                                              out Z);

                case 9:
                    return GetNextPoint_Type9(out X,
                                              out Y,
                                              out Z);

                case 10:
                    return GetNextPoint_Type10(out X,
                                               out Y,
                                               out Z);

                default:
                    return false;
            }
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

            if (m_nPntRecIdx == (int)m_nHDR_PntDataRecCount)
                return false;

            switch (m_nHDR_PntFmtID)
            {
                case 0:
                    return GetNextPoint_Type0(out X,
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
                case 1:
                    return GetNextPoint_Type1(out X,
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
                case 2:
                    return GetNextPoint_Type2(out X,
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
                case 3:
                    return GetNextPoint_Type3(out X,
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
                case 4:
                    return GetNextPoint_Type4(out X,
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
                case 5:
                    return GetNextPoint_Type5(out X,
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

                case 6:
                    return GetNextPoint_Type6(out X,
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

                case 7:
                    return GetNextPoint_Type7(out X,
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

                case 8:
                    return GetNextPoint_Type8(out X,
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

                case 9:
                    return GetNextPoint_Type9(out X,
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

                case 10:
                    return GetNextPoint_Type10(out X,
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

                default:
                    return false;
            }
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

            if (m_nPntRecIdx == (int)m_nHDR_PntDataRecCount)
                return false;

            switch (m_nHDR_PntFmtID)
            {
                case 0:
                    return GetNextPoint_Type0(out X,
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

                case 1:
                    return GetNextPoint_Type1(out X,
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

                case 2:
                    return GetNextPoint_Type2(out X,
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

                case 3:
                    return GetNextPoint_Type3(out X,
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

                case 4:
                    return GetNextPoint_Type4(out X,
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

                case 5:
                    return GetNextPoint_Type5(out X,
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

                case 6:
                    return GetNextPoint_Type6(out X,
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

                                              // 1.4
                                              out ClassificationFlag,
                                              out ScannerChannel,
                                              out ScanAngle_14);

                case 7:
                    return GetNextPoint_Type7(out X,
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

                                              // 1.4
                                              out ClassificationFlag,
                                              out ScannerChannel,
                                              out ScanAngle_14);

                case 8:
                    return GetNextPoint_Type8(out X,
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

                                              // 1.4
                                              out ClassificationFlag,
                                              out ScannerChannel,
                                              out ScanAngle_14);

                case 9:
                    return GetNextPoint_Type9(out X,
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

                                              // 1.4
                                              out ClassificationFlag,
                                              out ScannerChannel,
                                              out ScanAngle_14);

                case 10:
                    return GetNextPoint_Type10(out X,
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

                                              // 1.4
                                              out ClassificationFlag,
                                              out ScannerChannel,
                                              out ScanAngle_14);

                default:
                    return false;
            }
        }

        /*=== GetLastPoint() ===*/
        public void GetLastPoint()
        {
            if (m_BR != null)
            {
                m_BR.Close();
                m_BR.Dispose();
                m_BR = null;
            }
        }

        /*=== GetNextPoint_Type0() ===*/
        private bool GetNextPoint_Type0(out Int32 X,
                                        out Int32 Y,
                                        out Int32 Z)
        {
            // byte Flags;
            long Offset = 8;


            X = 0;
            Y = 0;
            Z = 0;

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            if (m_nPntRecIdx < (int)m_nHDR_PntDataRecCount - 1)
                m_BR.BaseStream.Seek(Offset, SeekOrigin.Current);
            /* 2 + 1 + 1 + 1 + 1 + 2 = 8
            Intensity = m_BR.ReadUInt16();
            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);
            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            */
            // Not Present in Type 0
            /*
            GPSTime = m_BR.ReadDouble();
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type0() ===*/
        private bool GetNextPoint_Type0(out Int32 X,
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
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            // Not Present in Type 0
            /*
            GPSTime = m_BR.ReadDouble();
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type0() ===*/
        public bool GetNextPoint_Type0(out Int32 X,
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
                                       out UInt16 Blue)
        {
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            Classification = m_BR.ReadByte();
            ScanAngle = m_BR.ReadByte();
            UserData = m_BR.ReadByte();
            PointSourceID = m_BR.ReadUInt16();
            // Not Present in Type 0
            /*
            GPSTime = m_BR.ReadDouble();
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type1() ===*/
        private bool GetNextPoint_Type1(out Int32 X,
                                        out Int32 Y,
                                        out Int32 Z)
        {
            // byte Flags;
            long Offset = 16;

            X = 0;
            Y = 0;
            Z = 0;

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            if (m_nPntRecIdx < (int)m_nHDR_PntDataRecCount - 1)
                m_BR.BaseStream.Seek(Offset, SeekOrigin.Current);
            /* 2 + 1 + 1 + 1 + 1 + 2 + 8 = 16
            Intensity = m_BR.ReadUInt16();
            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);
            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            */
            // Not Present in Type 1
            /*
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type1() ===*/
        private bool GetNextPoint_Type1(out Int32 X,
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
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            // Not Present in Type 1
            /*
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type1() ===*/
        private bool GetNextPoint_Type1(out Int32 X,
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
                                        out UInt16 Blue)
        {
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            Classification = m_BR.ReadByte();
            ScanAngle = m_BR.ReadByte();
            UserData = m_BR.ReadByte();
            PointSourceID = m_BR.ReadUInt16();
            GPSTime = m_BR.ReadDouble();
            // Not Present in Type 1
            /*
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type2() ===*/
        private bool GetNextPoint_Type2(out Int32 X,
                                        out Int32 Y,
                                        out Int32 Z)
        {
            // byte Flags;
            long Offset = 14;

            X = 0;
            Y = 0;
            Z = 0;

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            if (m_nPntRecIdx < (int)m_nHDR_PntDataRecCount - 1)
                m_BR.BaseStream.Seek(Offset, SeekOrigin.Current);
            /* 2 + 1 + 1 + 1 + 1 + 2 + 2 + 2 + 2 = 14
            Intensity = m_BR.ReadUInt16();
            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);
            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type2() ===*/
        private bool GetNextPoint_Type2(out Int32 X,
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
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type2() ===*/
        private bool GetNextPoint_Type2(out Int32 X,
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
                                        out UInt16 Blue)
        {
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            Classification = m_BR.ReadByte();
            ScanAngle = m_BR.ReadByte();
            UserData = m_BR.ReadByte();
            PointSourceID = m_BR.ReadUInt16();
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type3() ===*/
        private bool GetNextPoint_Type3(out Int32 X,
                                        out Int32 Y,
                                        out Int32 Z)
        {
            // byte Flags;
            long Offset = 22;

            X = 0;
            Y = 0;
            Z = 0;

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            if (m_nPntRecIdx < (int)m_nHDR_PntDataRecCount - 1)
                m_BR.BaseStream.Seek(Offset, SeekOrigin.Current);
            /* 2 + 1 + 1 + 1 + 1 + 2 + 8 + 2 + 2 + 2 = 22
            Intensity = m_BR.ReadUInt16();
            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);
            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type3() ===*/
        private bool GetNextPoint_Type3(out Int32 X,
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
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type3() ===*/
        private bool GetNextPoint_Type3(out Int32 X,
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
                                        out UInt16 Blue)
        {
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            EdgeOfFlightLine = (byte)((Flags & 0x80) >> 7);
            Classification = m_BR.ReadByte();
            ScanAngle = m_BR.ReadByte();
            UserData = m_BR.ReadByte();
            PointSourceID = m_BR.ReadUInt16();
            GPSTime = m_BR.ReadDouble();
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type4() ===*/
        private bool GetNextPoint_Type4(out Int32 X,
                                        out Int32 Y,
                                        out Int32 Z)
        {
            // byte Flags;
            long Offset = 45;

            X = 0;
            Y = 0;
            Z = 0;

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            if (m_nPntRecIdx < (int)m_nHDR_PntDataRecCount - 1)
                m_BR.BaseStream.Seek(Offset, SeekOrigin.Current);
            /* 2 + 1 + 1 + 1 + 1 + 2 + 8 + 1 + 8 + 4 + 4 + 4 + 4 + 4 = 45
            Intensity = m_BR.ReadUInt16();
            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);
            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            */
            // Not Present in Type 4
            /*
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */
            /*
            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type4() ===*/
        private bool GetNextPoint_Type4(out Int32 X,
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
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            // Not Present in Type 4
            /*
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */
            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type4() ===*/
        private bool GetNextPoint_Type4(out Int32 X,
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
                                        out UInt16 Blue)
        {
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            Classification = m_BR.ReadByte();
            ScanAngle = m_BR.ReadByte();
            UserData = m_BR.ReadByte();
            PointSourceID = m_BR.ReadUInt16();
            GPSTime = m_BR.ReadDouble();
            // Not Present in Type 4
            /*
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */
            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)

            ++m_nPntRecIdx;

            return true;

        }

        /*=== GetNextPoint_Type5() ===*/
        private bool GetNextPoint_Type5(out Int32 X,
                                        out Int32 Y,
                                        out Int32 Z)
        {
            // byte Flags;
            long Offset = 51;

            X = 0;
            Y = 0;
            Z = 0;

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            if (m_nPntRecIdx < (int)m_nHDR_PntDataRecCount - 1)
                m_BR.BaseStream.Seek(Offset, SeekOrigin.Current);
            /* 2 + 1 + 1 + 1 + 1 + 2 + 8 + 2 + 2 + 2 + 1 + 8 + 4 + 4 + 4 + 4 + 4 = 51
            Intensity = m_BR.ReadUInt16();
            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);
            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type5() ===*/
        private bool GetNextPoint_Type5(out Int32 X,
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
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            m_BR.ReadByte();                            // Classification
            ScanAngle = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type5() ===*/
        private bool GetNextPoint_Type5(out Int32 X,
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
                                        out UInt16 Blue)
        {
            byte Flags;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags = m_BR.ReadByte();
            RetNum = (byte)(Flags & 0x07);
            NumRet = (byte)((Flags & 0x38) >> 3);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ScanDir = (byte)((Flags & 0x40) >> 6);
            Classification = m_BR.ReadByte();
            ScanAngle = m_BR.ReadByte();
            UserData = m_BR.ReadByte();
            PointSourceID = m_BR.ReadUInt16();
            GPSTime = m_BR.ReadDouble();
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type6() ===*/
        private bool GetNextPoint_Type6(out Int32 X,
                                        out Int32 Y,
                                        out Int32 Z)
        {
            long Offset = 18;

            X = 0;
            Y = 0;
            Z = 0;

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            if (m_nPntRecIdx < (int)m_nHDR_PntDataRecCount - 1)
                m_BR.BaseStream.Seek(Offset, SeekOrigin.Current);
            /* 2 + 1 + 1 + 1 + 1 + 2 + 2 + 8 = 18 
            Intensity = m_BR.ReadUInt16();
            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);
            ClassificationFlags = (byte)(Flags2 & 0x0F);
            ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);
            Classification = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            ScanAngle = m_BR.ReadInt16();
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type6() ===*/
        private bool GetNextPoint_Type6(out Int32 X,
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
            byte Flags1, Flags2;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            // ClassificationFlags = (byte)(Flags2 & 0x0F);
            // ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            // EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);

            m_BR.ReadByte();                            // Classification
            m_BR.ReadByte();                            // User Data
            ScanAngle = (byte)((int)((double)m_BR.ReadInt16() * 0.006));
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type6() ===*/
        private bool GetNextPoint_Type6(out Int32 X,
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
            byte Flags1, Flags2;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ClassificationFlag = (byte)(Flags2 & 0x0F);
            ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);

            Classification = m_BR.ReadByte();
            UserData = m_BR.ReadByte();
            ScanAngle_14 = m_BR.ReadInt16();
            ScanAngle = (byte)((int)((double)ScanAngle_14 * 0.006));
            PointSourceID = m_BR.ReadUInt16();
            GPSTime = m_BR.ReadDouble();

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type7() ===*/
        private bool GetNextPoint_Type7(out Int32 X,
                                        out Int32 Y,
                                        out Int32 Z)
        {
            long Offset = 24;

            X = 0;
            Y = 0;
            Z = 0;

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            if (m_nPntRecIdx < (int)m_nHDR_PntDataRecCount - 1)
                m_BR.BaseStream.Seek(Offset, SeekOrigin.Current);

            /* 2 + 1 + 1 + 1 + 1 + 2 + 2 + 8 + 2 + 2 + 2 = 24
            Intensity = m_BR.ReadUInt16();
            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);
            ClassificationFlags = (byte)(Flags2 & 0x0F);
            ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);
            Classification = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            ScanAngle = m_BR.ReadInt16();
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type7() ===*/
        private bool GetNextPoint_Type7(out Int32 X,
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
            byte Flags1, Flags2;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            // ClassificationFlags = (byte)(Flags2 & 0x0F);
            // ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            // EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);

            m_BR.ReadByte();                            // Classification
            m_BR.ReadByte();                            // User Data
            ScanAngle = (byte)((int)((double)m_BR.ReadInt16() * 0.006));
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time

            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type7() ===*/
        private bool GetNextPoint_Type7(out Int32 X,
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
            byte Flags1, Flags2;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ClassificationFlag = (byte)(Flags2 & 0x0F);
            ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);

            Classification = m_BR.ReadByte();
            UserData = m_BR.ReadByte();
            ScanAngle_14 = m_BR.ReadInt16();
            ScanAngle = (byte)((int)((double)ScanAngle_14 * 0.006));
            PointSourceID = m_BR.ReadUInt16();
            GPSTime = m_BR.ReadDouble();

            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type8() ===*/
        private bool GetNextPoint_Type8(out Int32 X,
                                        out Int32 Y,
                                        out Int32 Z)
        {
            long Offset = 26;

            X = 0;
            Y = 0;
            Z = 0;

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            if (m_nPntRecIdx < (int)m_nHDR_PntDataRecCount - 1)
                m_BR.BaseStream.Seek(Offset, SeekOrigin.Current);

            /* 2 + 1 + 1 + 1 + 1 + 2 + 2 + 8 + 2 + 2 + 2 + 2 = 26
            Intensity = m_BR.ReadUInt16();
            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);
            ClassificationFlags = (byte)(Flags2 & 0x0F);
            ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);
            Classification = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            ScanAngle = m_BR.ReadInt16();
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            NIR = m_BR.ReadUInt16();
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type8() ===*/
        private bool GetNextPoint_Type8(out Int32 X,
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
            byte Flags1, Flags2;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            // ClassificationFlags = (byte)(Flags2 & 0x0F);
            // ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            // EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);

            m_BR.ReadByte();                            // Classification
            m_BR.ReadByte();                            // User Data
            ScanAngle = (byte)((int)((double)m_BR.ReadInt16() * 0.006));
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time

            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();

            m_BR.ReadUInt16();                          // NIR

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type8() ===*/
        private bool GetNextPoint_Type8(out Int32 X,
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
            byte Flags1, Flags2;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ClassificationFlag = (byte)(Flags2 & 0x0F);
            ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);

            Classification = m_BR.ReadByte();
            UserData = m_BR.ReadByte();
            ScanAngle_14 = m_BR.ReadInt16();
            ScanAngle = (byte)((int)((double)ScanAngle_14 * 0.006));
            PointSourceID = m_BR.ReadUInt16();
            GPSTime = m_BR.ReadDouble();

            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();

            m_BR.ReadUInt16();                          // NIR

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type9() ===*/
        private bool GetNextPoint_Type9(out Int32 X,
                                        out Int32 Y,
                                        out Int32 Z)
        {
            long Offset = 47;

            X = 0;
            Y = 0;
            Z = 0;

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            if (m_nPntRecIdx < (int)m_nHDR_PntDataRecCount - 1)
                m_BR.BaseStream.Seek(Offset, SeekOrigin.Current);
            /* 2 + 1 + 1 + 1 + 1 + 2 + 2 + 8 +
               1 + 8 + 4 + 4 + 4 + 4 + 4 = 47
            Intensity = m_BR.ReadUInt16();
            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);
            ClassificationFlags = (byte)(Flags2 & 0x0F);
            ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);
            Classification = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            ScanAngle = m_BR.ReadInt16();
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            */
            /*
            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type9() ===*/
        private bool GetNextPoint_Type9(out Int32 X,
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
            byte Flags1, Flags2;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            // ClassificationFlags = (byte)(Flags2 & 0x0F);
            // ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            // EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);

            m_BR.ReadByte();                            // Classification
            m_BR.ReadByte();                            // User Data
            ScanAngle = (byte)((int)((double)m_BR.ReadInt16() * 0.006));
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time

            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type9() ===*/
        private bool GetNextPoint_Type9(out Int32 X,
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
            byte Flags1, Flags2;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ClassificationFlag = (byte)(Flags2 & 0x0F);
            ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);

            Classification = m_BR.ReadByte();
            UserData = m_BR.ReadByte();
            ScanAngle_14 = m_BR.ReadInt16();
            ScanAngle = (byte)((int)((double)ScanAngle_14 * 0.006));
            PointSourceID = m_BR.ReadUInt16();
            GPSTime = m_BR.ReadDouble();

            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type10() ===*/
        private bool GetNextPoint_Type10(out Int32 X,
                                         out Int32 Y,
                                         out Int32 Z)
        {
            long Offset = 55;

            X = 0;
            Y = 0;
            Z = 0;

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            if (m_nPntRecIdx < (int)m_nHDR_PntDataRecCount - 1)
                m_BR.BaseStream.Seek(Offset, SeekOrigin.Current);

            /* 2 + 1 + 1 + 1 + 1 + 2 + 2 + 8 + 2 + 2 + 2 + 2 +
               1 + 8 + 4 + 4 + 4 + 4 + 4 = 55
            Intensity = m_BR.ReadUInt16();
            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);
            ClassificationFlags = (byte)(Flags2 & 0x0F);
            ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);
            Classification = m_BR.ReadByte();
            m_BR.ReadByte();                            // User Data
            ScanAngle = m_BR.ReadInt16();
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time
            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();
            NIR = m_BR.ReadUInt16();
            */
            /*
            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)
            */

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type10() ===*/
        private bool GetNextPoint_Type10(out Int32 X,
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
            byte Flags1, Flags2;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            // ClassificationFlags = (byte)(Flags2 & 0x0F);
            // ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            // EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);

            m_BR.ReadByte();                            // Classification
            m_BR.ReadByte();                            // User Data
            ScanAngle = (byte)((int)((double)m_BR.ReadInt16() * 0.006));
            m_BR.ReadUInt16();                          // Point Source ID
            m_BR.ReadDouble();                          // GPS Time

            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();

            m_BR.ReadUInt16();                          // NIR

            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)

            ++m_nPntRecIdx;

            return true;
        }

        /*=== GetNextPoint_Type10() ===*/
        private bool GetNextPoint_Type10(out Int32 X,
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
            byte Flags1, Flags2;

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

            X = m_BR.ReadInt32();
            Y = m_BR.ReadInt32();
            Z = m_BR.ReadInt32();
            Intensity = m_BR.ReadUInt16();

            if (Intensity == 0)
                Intensity = (UInt16)(1 + new Random().Next(9));

            Flags1 = m_BR.ReadByte();
            Flags2 = m_BR.ReadByte();
            RetNum = (byte)(Flags1 & 0x0F);
            NumRet = (byte)((Flags1 & 0xF0) >> 4);

            if (RetNum == 0)
                RetNum = (byte)1;
            if (NumRet == 0)
                NumRet = (byte)1;

            ClassificationFlag = (byte)(Flags2 & 0x0F);
            ScannerChannel = (byte)((Flags2 & 0x30) >> 4);
            ScanDir = (byte)((Flags2 & 0x40) >> 6);
            EdgeOfFlightLine = (byte)((Flags2 & 0x80) >> 7);

            Classification = m_BR.ReadByte();
            UserData = m_BR.ReadByte();
            ScanAngle_14 = m_BR.ReadInt16();
            ScanAngle = (byte)((int)((double)ScanAngle_14 * 0.006));
            PointSourceID = m_BR.ReadUInt16();
            GPSTime = m_BR.ReadDouble();

            Red = m_BR.ReadUInt16();
            Green = m_BR.ReadUInt16();
            Blue = m_BR.ReadUInt16();

            m_BR.ReadUInt16();                          // NIR

            m_BR.ReadByte();                            // Wave Packet Descriptor Index 
            m_BR.ReadUInt64();                          // Byte offset to waveform data
            m_BR.ReadUInt32();                          // Waveform packet size in bytes 
            m_BR.ReadSingle();                          // Return Point Waveform Location
            m_BR.ReadSingle();                          // X(t)
            m_BR.ReadSingle();                          // Y(t)
            m_BR.ReadSingle();                          // Z(t)

            ++m_nPntRecIdx;

            return true;
        }

        /*=== ReadHeader() ===*/
        private int ReadHeader()
        {
            byte[] ID;
            UInt16 FileSrcID = 0;                       // VersMin >= 1
            UInt16 GlbEnc = 0;                          // VersMin >= 2
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
            UInt32 LegacyPntDataRecCount;
            UInt64 PntDataRecCount = 0;
            UInt64[] PntByRetCount = new UInt64[15];     // VersMin >= 4, [15], else just [5] used
            double XScale, YScale, ZScale;
            double XOffset, YOffset, ZOffset;
            double XMin, XMax, YMin, YMax, ZMin, ZMax;
            UInt64 WDPRecStart = 0;                     // VersMin >= 3

            // VersMin >= 4
            UInt64 EVLRecStart = 0;
            UInt32 EVLRecCount = 0;

            int i, SysIDLen, GenSoftLen;

            m_nHDR_ProjID4 = null;
            m_nHDR_SysID = null;
            m_nHDR_GenSoft = null;
            m_nHDR_PntByRetCount = null;

            try
            {
                m_BR = new BinaryReader(File.Open(m_sFileName, FileMode.Open));
            }
            catch (Exception)
            {
                return ERR_STS_CANT_OPEN;
            }
            if (m_BR == null)
                return ERR_STS_CANT_OPEN;

            try
            {
                ID = m_BR.ReadBytes(4);
                if (ID == null)
                {
                    m_BR.Close();
                    m_BR.Dispose();
                    m_BR = null;
                    return ERR_STS_INVALID_FORMAT;
                }
                if (ID.Length != 4)
                {
                    m_BR.Close();
                    m_BR.Dispose();
                    m_BR = null;
                    return ERR_STS_INVALID_FORMAT;
                }
                if (ID[0] != ID_L ||
                    ID[1] != ID_A ||
                    ID[2] != ID_S ||
                    ID[3] != ID_F)
                {
                    m_BR.Close();
                    m_BR.Dispose();
                    m_BR = null;
                    return ERR_STS_INVALID_FORMAT;
                }
                ID = null;

                FileSrcID = m_BR.ReadUInt16();
                GlbEnc = m_BR.ReadUInt16();

                ProjID1 = m_BR.ReadUInt32();
                ProjID2 = m_BR.ReadUInt16();
                ProjID3 = m_BR.ReadUInt16();
                ProjID4 = m_BR.ReadBytes(8);
                if (ProjID4 == null)
                {
                    m_BR.Close();
                    m_BR.Dispose();
                    m_BR = null;
                    return ERR_STS_INVALID_FORMAT;
                }
                if (ProjID4.Length != 8)
                {
                    m_BR.Close();
                    m_BR.Dispose();
                    m_BR = null;
                    return ERR_STS_INVALID_FORMAT;
                }
                VersMaj = m_BR.ReadByte();
                VersMin = m_BR.ReadByte();
                if (VersMin != 4)
                {
                    m_BR.Close();
                    m_BR.Dispose();
                    m_BR = null;
                    return ERR_STS_INVALID_FORMAT;
                }
                SysID = m_BR.ReadBytes(32);
                if (SysID == null)
                {
                    m_BR.Close();
                    m_BR.Dispose();
                    m_BR = null;
                    return ERR_STS_INVALID_FORMAT;
                }
                if (SysID.Length != 32)
                {
                    m_BR.Close();
                    m_BR.Dispose();
                    m_BR = null;
                    return ERR_STS_INVALID_FORMAT;
                }
                GenSoft = m_BR.ReadBytes(32);
                if (GenSoft == null)
                {
                    m_BR.Close();
                    m_BR.Dispose();
                    m_BR = null;
                    return ERR_STS_INVALID_FORMAT;
                }
                if (GenSoft.Length != 32)
                {
                    m_BR.Close();
                    m_BR.Dispose();
                    m_BR = null;
                    return ERR_STS_INVALID_FORMAT;
                }
                SysIDLen = 32;
                for (i = 31; i >= 0; i--)
                if (SysID[i] == 0)
                    --SysIDLen;
                else
                    break;
                GenSoftLen = 32;
                for (i = 31; i >= 0; i--)
                if (GenSoft[i] == 0)
                    --GenSoftLen;
                else
                    break;
                CrtDayOfYear = m_BR.ReadUInt16();
                CrtYear = m_BR.ReadUInt16();
                HdrSize = m_BR.ReadUInt16();
                OffsetToPntData = m_BR.ReadUInt32();
                VarRecsCount = m_BR.ReadUInt32();
                PntFmtID = m_BR.ReadByte();
                PntDataRecLen = m_BR.ReadUInt16();
                LegacyPntDataRecCount = m_BR.ReadUInt32();
                
                // Legacy
                for (i = 0; i < 5; i++)
                    m_BR.ReadUInt32();

                XScale = m_BR.ReadDouble();
                YScale = m_BR.ReadDouble();
                ZScale = m_BR.ReadDouble();
                XOffset = m_BR.ReadDouble();
                YOffset = m_BR.ReadDouble();
                ZOffset = m_BR.ReadDouble();
                XMax = m_BR.ReadDouble();
                XMin = m_BR.ReadDouble();
                YMax = m_BR.ReadDouble();
                YMin = m_BR.ReadDouble();
                ZMax = m_BR.ReadDouble();
                ZMin = m_BR.ReadDouble();

                // VersMin >= 3
                if (VersMin >= 3)
                {
                    WDPRecStart = m_BR.ReadUInt64();
                }

                // VersMin >= 4
                if (VersMin >= 4)
                {
                    EVLRecStart = m_BR.ReadUInt64();
                    EVLRecCount = m_BR.ReadUInt32();
                    PntDataRecCount = m_BR.ReadUInt64();
                    for (i = 0; i < 15; i++)
                        PntByRetCount[i] = m_BR.ReadUInt64();

                }
            }
            catch (Exception)
            {
                m_BR.Close();
                m_BR.Dispose();
                m_BR = null;
                return ERR_STS_INVALID_FORMAT;
            }

            // Row Header Info
            m_nHDR_FileSrcID = FileSrcID;
            m_nHDR_GlbEnc = GlbEnc;
            m_nHDR_ProjID1 = ProjID1;
            m_nHDR_ProjID2 = ProjID2;
            m_nHDR_ProjID3 = ProjID3;
            m_nHDR_ProjID4 = new byte[8];
            for (i = 0; i < 8; i++)
                m_nHDR_ProjID4[i] = ProjID4[i];
            ProjID4 = null;
            m_nHDR_VersMaj = VersMaj;
            m_nHDR_VersMin = VersMin;
            m_nHDR_SysID = new byte[32];
            m_nHDR_GenSoft = new byte[32];
            for (i = 0; i < 32; i++)
            {
                m_nHDR_SysID[i] = SysID[i];
                m_nHDR_GenSoft[i] = GenSoft[i];
            }
            SysID = null;
            GenSoft = null;
            m_nHDR_CrtDayOfYear = CrtDayOfYear;
            m_nHDR_CrtYear = CrtYear;
            m_nHDR_HdrSize = HdrSize;
            m_nHDR_OffsetToPntData = OffsetToPntData;
            m_nHDR_VarRecsCount = VarRecsCount;
            m_nHDR_PntFmtID = PntFmtID;
            m_nHDR_PntDataRecLen = PntDataRecLen;
            m_nHDR_PntDataRecCount = PntDataRecCount;
            m_nHDR_PntByRetCount = new UInt64[15];
            for (i = 0; i < 15; i++)
                m_nHDR_PntByRetCount[i] = PntByRetCount[i];
            m_nHDR_XScale = XScale;
            m_nHDR_YScale = YScale;
            m_nHDR_ZScale = ZScale;
            m_nHDR_XOffset = XOffset;
            m_nHDR_YOffset = YOffset;
            m_nHDR_ZOffset = ZOffset;
            m_nHDR_XMin = XMin;
            m_nHDR_XMax = XMax;
            m_nHDR_YMin = YMin;
            m_nHDR_YMax = YMax;
            m_nHDR_ZMin = ZMin;
            m_nHDR_ZMax = ZMax;

            m_nHDR_WDPRecStart = WDPRecStart;     // VersMin >= 3

            // VersMin >= 4
            m_nHDR_EVLRecStart = EVLRecStart;
            m_nHDR_EVLRecCount = EVLRecCount;

            // Derivated Header Info
            m_sHDR_SysID = System.Text.Encoding.Default.GetString(m_nHDR_SysID, 0, SysIDLen);
            m_sHDR_GenSoft = System.Text.Encoding.Default.GetString(m_nHDR_GenSoft, 0, GenSoftLen);
            m_dtHDR_DateTime = new DateTime(m_nHDR_CrtYear, 1, 1);
            m_dtHDR_DateTime = m_dtHDR_DateTime.AddDays((double)(m_nHDR_CrtDayOfYear - 1));

            // Here: Var Recs!!!
            m_nVarRecsLen = (int)OffsetToPntData - HEADER_SIZE;
            if (m_nVarRecsLen > 0)
            {
                m_VarRecsBuf = new byte[m_nVarRecsLen];
                byte[] bts = m_BR.ReadBytes(m_nVarRecsLen);
                for (i = 0; i < m_nVarRecsLen; i++)
                    m_VarRecsBuf[i] = bts[i];
            }
            else
                m_VarRecsBuf = null;

            m_nExtVarRecsLen = (int)m_BR.BaseStream.Length - (int)m_nHDR_OffsetToPntData - (int)m_nHDR_PntDataRecCount * m_nHDR_PntDataRecLen;
            if (m_nExtVarRecsLen > 0)
            {
                long offs = (long)((int)m_nHDR_OffsetToPntData + (int)m_nHDR_PntDataRecCount * m_nHDR_PntDataRecLen);
                long offset = m_BR.BaseStream.Seek(offs, SeekOrigin.Begin);
                if (offset != offs)
                {
                    m_BR.Close();
                    m_BR.Dispose();
                    m_BR = null;
                    return ERR_STS_INVALID_FORMAT;
                }

                m_ExtVarRecsBuf = new byte[m_nExtVarRecsLen];
                byte[] bts = m_BR.ReadBytes(m_nExtVarRecsLen);
                for (i = 0; i < m_nExtVarRecsLen; i++)
                    m_ExtVarRecsBuf[i] = bts[i];
            }
            else
                m_ExtVarRecsBuf = null;

            m_BR.Close();
            m_BR.Dispose();
            m_BR = null;

            return ERR_NONE;
        }
    }
}
