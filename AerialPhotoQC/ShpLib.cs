using System;
using System.Runtime.InteropServices;

namespace AerialPhotoQC
{
    public class ShpLib
    {
        public const int SHPTYPE_POINT = 1;		    // supported in this version
        public const int SHPTYPE_POLYLINE = 3;	    // supported in this version
        public const int SHPTYPE_POLYGON = 5;	    // supported in this version
        public const int SHPTYPE_MULTIPOINT = 8;    // supported in this version

        public const int SHPTYPE_POINTZ = 11;
        public const int SHPTYPE_POLYLINEZ = 13;	// supported in this version
        public const int SHPTYPE_POLYGONZ = 15;		// supported in this version
        public const int SHPTYPE_MULTIPOINTZ = 18;

        public const int SHPTYPE_POINTM = 21;
        public const int SHPTYPE_POLYLINEM = 23;
        public const int SHPTYPE_POLYGONM = 25;
        public const int SHPTYPE_MULTIPOINTM = 28;

        public const int SHPTYPE_MULTIPATCH = 31;   // supported in this version

        public const int PARTTYPE_TRIANGLE_STRIP = 0;
        public const int PARTTYPE_TRIANGLE_FAN = 1;
        public const int PARTTYPE_OUTER_RING = 2;
        public const int PARTTYPE_INNER_RING = 3;
        public const int PARTTYPE_FIRST_RING = 4;
        public const int PARTTYPE_RING = 5;

        private IntPtr m_pNativeObject;

        [DllImport("ShpLib.dll")]
        static private extern IntPtr CreateClass();

        [DllImport("ShpLib.dll")]
        static private extern void DisposeClass(IntPtr pClassObject);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetGenInfo(IntPtr pClassObject,

                                                   String FileName,

                                                   ref int ShapeType,
                                                   ref int RecCount,
                                                   ref double MinX,
                                                   ref double MinY,
                                                   ref double MaxX,
                                                   ref double MaxY,

                                                   ref int FldCount,
                                                   byte[] FldNames,
                                                   byte[] FldTypes,
                                                   int[] FldLens,
                                                   int[] FldDecs);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetGenInfoZ(IntPtr pClassObject,

                                                    String FileName,

                                                    ref int ShapeType,
                                                    ref int RecCount,
                                                    ref double MinX,
                                                    ref double MinY,
                                                    ref double MinZ,
                                                    ref double MaxX,
                                                    ref double MaxY,
                                                    ref double MaxZ,

                                                    ref int FldCount,
                                                    byte[] FldNames,
                                                    byte[] FldTypes,
                                                    int[] FldLens,
                                                    int[] FldDecs);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetGenInfoM(IntPtr pClassObject,

                                                    String FileName,

                                                    ref int ShapeType,
                                                    ref int RecCount,
                                                    ref double MinX,
                                                    ref double MinY,
                                                    ref double MinM,
                                                    ref double MaxX,
                                                    ref double MaxY,
                                                    ref double MaxM,

                                                    ref int FldCount,
                                                    byte[] FldNames,
                                                    byte[] FldTypes,
                                                    int[] FldLens,
                                                    int[] FldDecs);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetGenInfoZM(IntPtr pClassObject,

                                                     String FileName,

                                                     ref int ShapeType,
                                                     ref int RecCount,
                                                     ref double MinX,
                                                     ref double MinY,
                                                     ref double MinZ,
                                                     ref double MinM,
                                                     ref double MaxX,
                                                     ref double MaxY,
                                                     ref double MaxZ,
                                                     ref double MaxM,

                                                     ref int FldCount,
                                                     byte[] FldNames,
                                                     byte[] FldTypes,
                                                     int[] FldLens,
                                                     int[] FldDecs);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_Start(IntPtr pClassObject,
												         String FileName,
                                                         double X1,
                                                         double Y1,
                                                         double X2,
                                                         double Y2,
                                                         bool DBF);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_First(IntPtr pClassObject,
												         ref int PntsCount,
												         ref int PartsCount);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_Next(IntPtr pClassObject,
												        ref int PntsCount,
												        ref int PartsCount);

        [DllImport("ShpLib.dll")]
        static private extern void Call_GetShpInfo_Geom(IntPtr pClassObject,
												        int PntsIdx,
												        int PartsIdx,
												        double[] PntsX,
												        double[] PntsY,
												        int[] Parts);

        [DllImport("ShpLib.dll")]
        /*
        static private extern String Call_GetShpInfo_DBF(IntPtr pClassObject,
												         int FldIdx);
        */
        static private extern void Call_GetShpInfo_DBF(IntPtr pClassObject,
                                                       int FldIdx,
                                                       byte[] Ret);

        [DllImport("ShpLib.dll")]
        static private extern void Call_GetShpInfo_Stop(IntPtr pClassObject);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_StartZ(IntPtr pClassObject,
												          String FileName,
												          double X1,
												          double Y1,
												          double Z1,
												          double X2,
												          double Y2,
												          double Z2,
												          bool DBF);
        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_FirstZ(IntPtr pClassObject,
                                                          ref int PntsCount,
                                                          ref int PartsCount);
        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_NextZ(IntPtr pClassObject,
                                                         ref int PntsCount,
                                                         ref int PartsCount);
        [DllImport("ShpLib.dll")]
        static private extern void Call_GetShpInfo_GeomZ(IntPtr pClassObject,
                                                        int PntsIdx,
                                                        int PartsIdx,
                                                        double[] PntsX,
                                                        double[] PntsY,
                                                        double[] PntsZ,
                                                        int[] Parts);
        [DllImport("ShpLib.dll")]
        static private extern void Call_GetShpInfo_StopZ(IntPtr pClassObject);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_StartM(IntPtr pClassObject,
                                                          String FileName,
                                                          double X1,
                                                          double Y1,
                                                          double M1,
                                                          double X2,
                                                          double Y2,
                                                          double M2,
                                                          bool DBF);
        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_FirstM(IntPtr pClassObject,
                                                          ref int PntsCount,
                                                          ref int PartsCount);
        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_NextM(IntPtr pClassObject,
                                                         ref int PntsCount,
                                                         ref int PartsCount);
        [DllImport("ShpLib.dll")]
        static private extern void Call_GetShpInfo_GeomM(IntPtr pClassObject,
                                                        int PntsIdx,
                                                        int PartsIdx,
                                                        double[] PntsX,
                                                        double[] PntsY,
                                                        double[] PntsM,
                                                        int[] Parts);
        [DllImport("ShpLib.dll")]
        static private extern void Call_GetShpInfo_StopM(IntPtr pClassObject);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_StartZM(IntPtr pClassObject,
                                                           String FileName,
                                                           double X1,
                                                           double Y1,
                                                           double Z1,
                                                           double M1,
                                                           double X2,
                                                           double Y2,
                                                           double Z2,
                                                           double M2,
                                                           bool DBF);
        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_FirstZM(IntPtr pClassObject,
                                                           ref int PntsCount,
                                                           ref int PartsCount);
        [DllImport("ShpLib.dll")]
        static private extern bool Call_GetShpInfo_NextZM(IntPtr pClassObject,
                                                          ref int PntsCount,
                                                          ref int PartsCount);
        [DllImport("ShpLib.dll")]
        static private extern void Call_GetShpInfo_GeomZM(IntPtr pClassObject,
                                                         int PntsIdx,
                                                         int PartsIdx,
                                                         double[] PntsX,
                                                         double[] PntsY,
                                                         double[] PntsZ,
                                                         double[] PntsM,
                                                         int[] Parts);
        [DllImport("ShpLib.dll")]
        static private extern void Call_GetShpInfo_StopZM(IntPtr pClassObject);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_OBIFLY_UpdateStatus(IntPtr pClassObject,

                                                            String FileName,
                                                            int RecCount,
                                                            int[] RecFIDs,
                                                            int FldIdx,
                                                            int[] NewStatus);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_CreateShape(IntPtr pClassObject,

                                                    String FilePath,

                                                    int ShapeType,
                                                    int DBFFieldCount,
                                                    String DBFFieldNames,
                                                    char[] pDBFFieldTypes,
                                                    int[] pDBFFieldLengths,
                                                    int[] pDBFFieldDecs);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_BeforeUpdate(IntPtr pClassObject,
            
                                                     String FilePath);

        [DllImport("ShpLib.dll")]
        static private extern void Call_AfterUpdate(IntPtr pClassObject);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_UpdateDBFRec(IntPtr pClassObject,

                                                     int RecIdx,
                                                     String FldVals);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_Polygon_AddRec(IntPtr pClassObject,

                                                       int PartCount,
                                                       int PtsCount,
                                                       int[] Parts,

                                                       double[] X,
                                                       double[] Y,

                                                       String FldVals);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_MultiPatch_AddRec(IntPtr pClassObject,

                                                          int PartCount,
                                                          int PtsCount,
                                                          int[] Parts,
                                                          int[] PartTypes,

                                                          double[] X,
                                                          double[] Y,
                                                          double[] Z,

                                                          String FldVals);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_ShpLine_AddRecs(IntPtr pClassObject,

                                                        String FileName,

                                                        int RecCount,
                                                        double[] X1,
                                                        double[] Y1,
                                                        double[] X2,
                                                        double[] Y2,

                                                        int FldCount,
                                                        int[] FldIdxs,
                                                        String FldVals,

                                                        ref int NewRecCount,
                                                        ref double NewMinX,
                                                        ref double NewMinY,
                                                        ref double NewMaxX,
                                                        ref double NewMaxY);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_ShpPnt_AddRecs(IntPtr pClassObject,

                                                       String FileName,

                                                       int RecCount,
                                                       double[] X,
                                                       double[] Y,

                                                       int FldCount,
                                                       int[] FldIdxs,
                                                       String FldVals,

                                                       ref int NewRecCount,
                                                       ref double NewMinX,
                                                       ref double NewMinY,
                                                       ref double NewMaxX,
                                                       ref double NewMaxY);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_ShpPntZ_AddRecs(IntPtr pClassObject,

                                                        String FileName,

                                                        int RecCount,
                                                        double[] X,
                                                        double[] Y,
                                                        double[] Z,

                                                        int FldCount,
                                                        int[] FldIdxs,
                                                        String FldVals);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_ShpPolyZFixedPtsCount_AddRecs(IntPtr pClassObject,

                                                                      String FileName,

                                                                      int RecCount,
                                                                      int PtsCount,
                                                                      double[] X,
                                                                      double[] Y,
                                                                      double[] Z,

                                                                      int FldCount,
                                                                      int[] FldIdxs,
                                                                      String FldVals);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_ShpLineZ_AddRecs(IntPtr pClassObject,

                                                         String FileName,

                                                         int RecCount,
                                                         int[] PtsCount,
                                                         double[] X,
                                                         double[] Y,
                                                         double[] Z,

                                                         int FldCount,
                                                         int[] FldIdxs,
                                                         String FldVals);

        // 2017-03-13
        [DllImport("ShpLib.dll")]
        static private extern bool Call_ShpPolyZSinglePart_AddRecs(IntPtr pClassObject,

                                                                   String FileName,

                                                                   int RecCount,
                                                                   int[] PtsCount,
                                                                   double[] X,
                                                                   double[] Y,
                                                                   double[] Z,

                                                                   int FldCount,
                                                                   int[] FldIdxs,
                                                                   String FldVals);

        // 2017-03-23
        [DllImport("ShpLib.dll")]
        static private extern bool Call_ShpPolyZMMultiPart_AddRecs(IntPtr pClassObject,

                                                                   String FileName,

                                                                   int RecCount,
                                                                   int[] PartsCount,
                                                                   int[] PtsCount,
                                                                   double[] X,
                                                                   double[] Y,
                                                                   double[] Z,
                                                                   double[] M,

                                                                   int FldCount,
                                                                   int[] FldIdxs,
                                                                   String FldVals);

        // 2016-06-12
        [DllImport("ShpLib.dll")]
        static private extern bool Call_SpIdx2D_Start(IntPtr pClassObject);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_SpIdx2D_InsertPoint(IntPtr pClassObject,
            
                                                            double X,
                                                            double Y,
                                                            int RecID);

        [DllImport("ShpLib.dll")]
        static private extern bool Call_SpIdx2D_InsertBox(IntPtr pClassObject,

                                                          double MinX,
                                                          double MinY,
                                                          double MaxX,
                                                          double MaxY,
                                                          int RecID);

        [DllImport("ShpLib.dll")]
        static private extern int Call_SpIdx2D_SearchStart(IntPtr pClassObject,
            
                                                           double MinX,
                                                           double MinY,
                                                           double MaxX,
                                                           double MaxY);
        [DllImport("ShpLib.dll")]
        static private extern int Call_SpIdx2D_GetFoundID(IntPtr pClassObject,
            
                                                          int Idx);

        [DllImport("ShpLib.dll")]
        static private extern void Call_SpIdx2D_SearchClear(IntPtr pClassObject);

        [DllImport("ShpLib.dll")]
        static private extern void Call_SpIdx2D_End(IntPtr pClassObject);

        /*=== Constructor ===*/
        public ShpLib()
        {
            this.m_pNativeObject = CreateClass();
        }

        /*=== Dispose() ===*/
        public void Dispose()
        {
            Dispose(true);
        }

        /*=== Dispose() ===*/
        protected virtual void Dispose(bool bDisposing)
        {
            if (this.m_pNativeObject != IntPtr.Zero)
            {
                DisposeClass(this.m_pNativeObject);
                this.m_pNativeObject = IntPtr.Zero;
            }

            if (bDisposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        /*=== Destructor ===*/
        ~ShpLib()
        {
            Dispose(false);
        }

        /*=== GetSHPTypeStr() ===*/
        public String GetSHPTypeStr(int Type)
        {
            switch (Type)
            {
                case SHPTYPE_POINT:
                    return "PUNTOS";

                case SHPTYPE_POLYLINE:
                    return "POLILINEA";

                case SHPTYPE_POLYGON:
                    return "POLIGONO";

                case SHPTYPE_MULTIPOINT:
                    return "PUNTOS MULTIPLES";

                case SHPTYPE_POINTZ:
                    return "PUNTOS Z";

                case SHPTYPE_POLYLINEZ:
                    return "POLILINEA Z";

                case SHPTYPE_POLYGONZ:
                    return "POLIGONO Z";

                case SHPTYPE_MULTIPOINTZ:
                    return "PUNTOS MULTIPLES Z";

                case SHPTYPE_POINTM:
                    return "PUNTOS M";

                case SHPTYPE_POLYLINEM:
                    return "POLILINEA M";

                case SHPTYPE_POLYGONM:
                    return "POLIGONO M";

                case SHPTYPE_MULTIPOINTM:
                    return "PUNTOS MULTIPLES M";

                case SHPTYPE_MULTIPATCH:
                    return "MULTIPATCH";

                default:
                    return "DESCONOCIDO";
            }
        }

        /*=== GetGenInfo() ===*/
        public bool GetGenInfo(String FileName,

                               ref int ShapeType,
                               ref int RecCount,
                               ref double MinX,
                               ref double MinY,
                               ref double MaxX,
                               ref double MaxY,

                               ref int FldCount,
                               String[] FldNames,
                               String[] FldTypes,
                               int[] FldLens,
                               int[] FldDecs)
        {
            int i, j, len;
            bool Res;
            byte[] cFldNames = new byte[10 * 256];
            byte[] cFldTypes = new byte[10 * 256];
            char[] s = new char[10];
            String S;

            Res = Call_GetGenInfo(this.m_pNativeObject,

                                  FileName,

                                  ref ShapeType,
                                  ref RecCount,
                                  ref MinX,
                                  ref MinY,
                                  ref MaxX,
                                  ref MaxY,

                                  ref FldCount,
                                  cFldNames,
                                  cFldTypes,
                                  FldLens,
                                  FldDecs);

            for (i = 0; i < FldCount; i++)
            {
                len = 0;
                for (j = 0; j < 10; j++)
                {
                    if (cFldNames[10 * i + j] == 32)
                        break;
                    s[j] = (char)cFldNames[10 * i + j];
                    ++len;
                }
                S = new String(s);
                FldNames[i] = S.Substring(0, len);

                s[0] = (char)cFldTypes[i];
                S = new String(s);
                FldTypes[i] = S.Substring(0, 1);
            }

            return Res;
        }

        /*=== GetGenInfoZ() ===*/
        public bool GetGenInfoZ(String FileName,

                                ref int ShapeType,
                                ref int RecCount,
                                ref double MinX,
                                ref double MinY,
                                ref double MinZ,
                                ref double MaxX,
                                ref double MaxY,
                                ref double MaxZ,

                                ref int FldCount,
                                String[] FldNames,
                                String[] FldTypes,
                                int[] FldLens,
                                int[] FldDecs)
        {
            int i, j, len;
            bool Res;
            byte[] cFldNames = new byte[10 * 256];
            byte[] cFldTypes = new byte[10 * 256];
            char[] s = new char[10];
            String S;

            Res = Call_GetGenInfoZ(this.m_pNativeObject,

                                   FileName,

                                   ref ShapeType,
                                   ref RecCount,
                                   ref MinX,
                                   ref MinY,
                                   ref MinZ,
                                   ref MaxX,
                                   ref MaxY,
                                   ref MaxZ,

                                   ref FldCount,
                                   cFldNames,
                                   cFldTypes,
                                   FldLens,
                                   FldDecs);

            for (i = 0; i < FldCount; i++)
            {
                len = 0;
                for (j = 0; j < 10; j++)
                {
                    if (cFldNames[10 * i + j] == 32)
                        break;
                    s[j] = (char)cFldNames[10 * i + j];
                    ++len;
                }
                S = new String(s);
                FldNames[i] = S.Substring(0, len);

                s[0] = (char)cFldTypes[i];
                S = new String(s);
                FldTypes[i] = S.Substring(0, 1);
            }

            return Res;
        }

        /*=== GetGenInfoM() ===*/
        public bool GetGenInfoM(String FileName,

                                ref int ShapeType,
                                ref int RecCount,
                                ref double MinX,
                                ref double MinY,
                                ref double MinM,
                                ref double MaxX,
                                ref double MaxY,
                                ref double MaxM,

                                ref int FldCount,
                                String[] FldNames,
                                String[] FldTypes,
                                int[] FldLens,
                                int[] FldDecs)
        {
            int i, j, len;
            bool Res;
            byte[] cFldNames = new byte[10 * 256];
            byte[] cFldTypes = new byte[10 * 256];
            char[] s = new char[10];
            String S;

            Res = Call_GetGenInfoM(this.m_pNativeObject,

                                   FileName,

                                   ref ShapeType,
                                   ref RecCount,
                                   ref MinX,
                                   ref MinY,
                                   ref MinM,
                                   ref MaxX,
                                   ref MaxY,
                                   ref MaxM,

                                   ref FldCount,
                                   cFldNames,
                                   cFldTypes,
                                   FldLens,
                                   FldDecs);

            for (i = 0; i < FldCount; i++)
            {
                len = 0;
                for (j = 0; j < 10; j++)
                {
                    if (cFldNames[10 * i + j] == 32)
                        break;
                    s[j] = (char)cFldNames[10 * i + j];
                    ++len;
                }
                S = new String(s);
                FldNames[i] = S.Substring(0, len);

                s[0] = (char)cFldTypes[i];
                S = new String(s);
                FldTypes[i] = S.Substring(0, 1);
            }

            return Res;
        }

        /*=== GetGenInfoZM() ===*/
        public bool GetGenInfoZM(String FileName,

                                 ref int ShapeType,
                                 ref int RecCount,
                                 ref double MinX,
                                 ref double MinY,
                                 ref double MinZ,
                                 ref double MinM,
                                 ref double MaxX,
                                 ref double MaxY,
                                 ref double MaxZ,
                                 ref double MaxM,

                                 ref int FldCount,
                                 String[] FldNames,
                                 String[] FldTypes,
                                 int[] FldLens,
                                 int[] FldDecs)
        {
            int i, j, len;
            bool Res;
            byte[] cFldNames = new byte[10 * 256];
            byte[] cFldTypes = new byte[10 * 256];
            char[] s = new char[10];
            String S;

            Res = Call_GetGenInfoZM(this.m_pNativeObject,

                                    FileName,

                                    ref ShapeType,
                                    ref RecCount,
                                    ref MinX,
                                    ref MinY,
                                    ref MinZ,
                                    ref MinM,
                                    ref MaxX,
                                    ref MaxY,
                                    ref MaxZ,
                                    ref MaxM,

                                    ref FldCount,
                                    cFldNames,
                                    cFldTypes,
                                    FldLens,
                                    FldDecs);

            for (i = 0; i < FldCount; i++)
            {
                len = 0;
                for (j = 0; j < 10; j++)
                {
                    if (cFldNames[10 * i + j] == 32)
                        break;
                    s[j] = (char)cFldNames[10 * i + j];
                    ++len;
                }
                S = new String(s);
                FldNames[i] = S.Substring(0, len);

                s[0] = (char)cFldTypes[i];
                S = new String(s);
                FldTypes[i] = S.Substring(0, 1);
            }

            return Res;
        }

        /*=== GetShpInfo_Start() ===*/
        public bool GetShpInfo_Start(String FileName,
                                     double X1,
                                     double Y1,
                                     double X2,
                                     double Y2,
                                     bool DBF)
        {
            return Call_GetShpInfo_Start(this.m_pNativeObject,
                                         FileName,
                                         X1,
                                         Y1,
                                         X2,
                                         Y2,
                                         DBF);
        }

        /*=== GetShpInfo_First() ===*/
        public bool GetShpInfo_First(ref int PntsCount,
                                     ref int PartsCount)
        {
            return Call_GetShpInfo_First(this.m_pNativeObject,
                                         ref PntsCount,
                                         ref PartsCount);
        }

        /*=== GetShpInfo_Next() ===*/
        public bool GetShpInfo_Next(ref int PntsCount,
                                    ref int PartsCount)
        {
            return Call_GetShpInfo_Next(this.m_pNativeObject,
                                        ref PntsCount,
                                        ref PartsCount);
        }

        /*=== GetShpInfo_Geom() ===*/
        public void GetShpInfo_Geom(int PntsIdx,
                                    int PartsIdx,
                                    double[] PntsX,
                                    double[] PntsY,
                                    int[] Parts)
        {
            Call_GetShpInfo_Geom(this.m_pNativeObject,
                                 PntsIdx,
                                 PartsIdx,
                                 PntsX,
                                 PntsY,
                                 Parts);
        }

        /*=== GetShpInfo_DBF() ===*/
        /*
        public String GetShpInfo_DBF(int FldIdx)
        {
            byte[] s = new byte[1024];
            char[] S;
            String Res;
            int i, Len;

            return Call_GetShpInfo_DBF(this.m_pNativeObject,
                                       FldIdx);
        }
        */
        public String GetShpInfo_DBF(int FldIdx)
        {
            byte[] s = new byte[1024];
            char[] S;
            String Res;
            int i, Len;

            Call_GetShpInfo_DBF(this.m_pNativeObject,
                                       FldIdx,
                                       s);

            Len = 0;
            while (true)
            {
                if (s[Len] == 0)
                    break;
                ++Len;
            }
            S = new char[Len];

            i = 0;
            while (true)
            {
                if (s[i] == 0)
                    break;
                S[i] = (char)s[i];
                ++i;
            }

            Res = new String(S);
            Res = Res.Trim();

            return Res;
        }

        /*=== GetShpInfo_Stop() ===*/
        public void GetShpInfo_Stop()
        {
            Call_GetShpInfo_Stop(this.m_pNativeObject);
        }

        /*=== GetShpInfo_StartZ() ===*/
        public bool GetShpInfo_StartZ(String FileName,
                                      double X1,
                                      double Y1,
                                      double Z1,
                                      double X2,
                                      double Y2,
                                      double Z2,
                                      bool DBF)
        {
            return Call_GetShpInfo_StartZ(this.m_pNativeObject,
                                          FileName,
                                          X1,
                                          Y1,
                                          Z1,
                                          X2,
                                          Y2,
                                          Z2,
                                          DBF);
        }

        /*=== GetShpInfo_FirstZ() ===*/
        public bool GetShpInfo_FirstZ(ref int PntsCount,
                                      ref int PartsCount)
        {
            return Call_GetShpInfo_FirstZ(this.m_pNativeObject,
                                          ref PntsCount,
                                          ref PartsCount);
        }

        /*=== GetShpInfo_NextZ() ===*/
        public bool GetShpInfo_NextZ(ref int PntsCount,
                                     ref int PartsCount)
        {
            return Call_GetShpInfo_NextZ(this.m_pNativeObject,
                                         ref PntsCount,
                                         ref PartsCount);
        }

        /*=== GetShpInfo_GeomZ() ===*/
        public void GetShpInfo_GeomZ(int PntsIdx,
                                     int PartsIdx,
                                     double[] PntsX,
                                     double[] PntsY,
                                     double[] PntsZ,
                                     int[] Parts)
        {
            Call_GetShpInfo_GeomZ(this.m_pNativeObject,
                                  PntsIdx,
                                  PartsIdx,
                                  PntsX,
                                  PntsY,
                                  PntsZ,
                                  Parts);
        }

        /*=== GetShpInfo_StopZ() ===*/
        public void GetShpInfo_StopZ()
        {
            Call_GetShpInfo_StopZ(this.m_pNativeObject);
        }

        /*=== GetShpInfo_StartM() ===*/
        public bool GetShpInfo_StartM(String FileName,
                                      double X1,
                                      double Y1,
                                      double M1,
                                      double X2,
                                      double Y2,
                                      double M2,
                                      bool DBF)
        {
            return Call_GetShpInfo_StartM(this.m_pNativeObject,
                                          FileName,
                                          X1,
                                          Y1,
                                          M1,
                                          X2,
                                          Y2,
                                          M2,
                                          DBF);
        }

        /*=== GetShpInfo_FirstM() ===*/
        public bool GetShpInfo_FirstM(ref int PntsCount,
                                      ref int PartsCount)
        {
            return Call_GetShpInfo_FirstM(this.m_pNativeObject,
                                          ref PntsCount,
                                          ref PartsCount);
        }

        /*=== GetShpInfo_NextM() ===*/
        public bool GetShpInfo_NextM(ref int PntsCount,
                                     ref int PartsCount)
        {
            return Call_GetShpInfo_NextM(this.m_pNativeObject,
                                         ref PntsCount,
                                         ref PartsCount);
        }

        /*=== GetShpInfo_GeomM() ===*/
        public void GetShpInfo_GeomM(int PntsIdx,
                                     int PartsIdx,
                                     double[] PntsX,
                                     double[] PntsY,
                                     double[] PntsM,
                                     int[] Parts)
        {
            Call_GetShpInfo_GeomM(this.m_pNativeObject,
                                  PntsIdx,
                                  PartsIdx,
                                  PntsX,
                                  PntsY,
                                  PntsM,
                                  Parts);
        }

        /*=== GetShpInfo_StopM() ===*/
        public void GetShpInfo_StopM()
        {
            Call_GetShpInfo_StopM(this.m_pNativeObject);
        }

        /*=== GetShpInfo_StartZM() ===*/
        public bool GetShpInfo_StartZM(String FileName,
                                       double X1,
                                       double Y1,
                                       double Z1,
                                       double M1,
                                       double X2,
                                       double Y2,
                                       double Z2,
                                       double M2,
                                       bool DBF)
        {
            return Call_GetShpInfo_StartZM(this.m_pNativeObject,
                                           FileName,
                                           X1,
                                           Y1,
                                           Z1,
                                           M1,
                                           X2,
                                           Y2,
                                           Z2,
                                           M2,
                                           DBF);
        }

        /*=== GetShpInfo_FirstZM() ===*/
        public bool GetShpInfo_FirstZM(ref int PntsCount,
                                       ref int PartsCount)
        {
            return Call_GetShpInfo_FirstZM(this.m_pNativeObject,
                                           ref PntsCount,
                                           ref PartsCount);
        }

        /*=== GetShpInfo_NextZM() ===*/
        public bool GetShpInfo_NextZM(ref int PntsCount,
                                      ref int PartsCount)
        {
            return Call_GetShpInfo_NextZM(this.m_pNativeObject,
                                          ref PntsCount,
                                          ref PartsCount);
        }

        /*=== GetShpInfo_GeomZM() ===*/
        public void GetShpInfo_GeomZM(int PntsIdx,
                                      int PartsIdx,
                                      double[] PntsX,
                                      double[] PntsY,
                                      double[] PntsZ,
                                      double[] PntsM,
                                      int[] Parts)
        {
            Call_GetShpInfo_GeomZM(this.m_pNativeObject,
                                   PntsIdx,
                                   PartsIdx,
                                   PntsX,
                                   PntsY,
                                   PntsZ,
                                   PntsM,
                                   Parts);
        }

        /*=== GetShpInfo_StopZM() ===*/
        public void GetShpInfo_StopZM()
        {
            Call_GetShpInfo_StopM(this.m_pNativeObject);
        }

        /*=== OBIFLY_UpdateStatus() ===*/
        public void OBIFLY_UpdateStatus(String FileName,
                                        int RecCount,
                                        int[] RecFIDs,
                                        int FldIdx,
                                        int[] NewStatus)
        {
            Call_OBIFLY_UpdateStatus(this.m_pNativeObject,

                                     FileName,
                                     RecCount,
                                     RecFIDs,
                                     FldIdx,
                                     NewStatus);
        }

        /*=== CreateShape() ===*/
        public bool CreateShape(String FilePath,

                                int ShapeType,
                                int DBFFieldCount,
                                String DBFFieldNames,
                                char[] pDBFFieldTypes,
                                int[] pDBFFieldLengths,
                                int[] pDBFFieldDecs)
        {
            return Call_CreateShape(this.m_pNativeObject,

                                    FilePath,

                                    ShapeType,
                                    DBFFieldCount,
                                    DBFFieldNames,
                                    pDBFFieldTypes,
                                    pDBFFieldLengths,
                                    pDBFFieldDecs);
        }

        /*=== BeforeUpdate() ===*/
        public bool BeforeUpdate(String FilePath)
        {
            return Call_BeforeUpdate(this.m_pNativeObject,

                                     FilePath);
        }

        /*=== AfterUpdate() ===*/
        public void AfterUpdate()
        {
            Call_AfterUpdate(this.m_pNativeObject);
        }

        /*=== UpdateDBFRec() ===*/
        public bool UpdateDBFRec(int RecIdx,
                                 String FldVals)
        {
            return Call_UpdateDBFRec(this.m_pNativeObject,

                                     RecIdx,
                                     FldVals);
        }

        /*=== Polygon_AddRec() ===*/
        public bool Polygon_AddRec(int PartCount,
                                   int PtsCount,
                                   int[] Parts,

                                   double[] X,
                                   double[] Y,

                                   String FldVals)
        {
            return Call_Polygon_AddRec(this.m_pNativeObject,

                                       PartCount,
                                       PtsCount,
                                       Parts,

                                       X,
                                       Y,

                                       FldVals);
        }

        /*=== MultiPatch_AddRec() ===*/
        public bool MultiPatch_AddRec(int PartCount,
                                      int PtsCount,
                                      int[] Parts,
                                      int[] PartTypes,

                                      double[] X,
                                      double[] Y,
                                      double[] Z,

                                      String FldVals)
        {
            return Call_MultiPatch_AddRec(this.m_pNativeObject,
                
                                          PartCount,
                                          PtsCount,
                                          Parts,
                                          PartTypes,

                                          X,
                                          Y,
                                          Z,

                                          FldVals);
        }

        /*=== ShpLine_AddRecs() ===*/
        public bool ShpLine_AddRecs(String FileName,

                                    int RecCount,
                                    double[] X1,
                                    double[] Y1,
                                    double[] X2,
                                    double[] Y2,

                                    int FldCount,
                                    int[] FldIdxs,
                                    String FldVals,

                                    ref int NewRecCount,
                                    ref double NewMinX,
                                    ref double NewMinY,
                                    ref double NewMaxX,
                                    ref double NewMaxY)
        {
            return Call_ShpLine_AddRecs(this.m_pNativeObject,

                                        FileName,

                                        RecCount,
                                        X1,
                                        Y1,
                                        X2,
                                        Y2,

                                        FldCount,
                                        FldIdxs,
                                        FldVals,

                                        ref NewRecCount,
                                        ref NewMinX,
                                        ref NewMinY,
                                        ref NewMaxX,
                                        ref NewMaxY);
        }

        /*=== ShpPnt_AddRecs() ===*/
        public bool ShpPnt_AddRecs(String FileName,

                                   int RecCount,
                                   double[] X,
                                   double[] Y,

                                   int FldCount,
                                   int[] FldIdxs,
                                   String FldVals,

                                   ref int NewRecCount,
                                   ref double NewMinX,
                                   ref double NewMinY,
                                   ref double NewMaxX,
                                   ref double NewMaxY)
        {
            return Call_ShpPnt_AddRecs(this.m_pNativeObject,

                                       FileName,

                                       RecCount,
                                       X,
                                       Y,

                                       FldCount,
                                       FldIdxs,
                                       FldVals,

                                       ref NewRecCount,
                                       ref NewMinX,
                                       ref NewMinY,
                                       ref NewMaxX,
                                       ref NewMaxY);
        }

        /*=== ShpPntZ_AddRecs() ===*/
        public bool ShpPntZ_AddRecs(String FileName,

                                    int RecCount,
                                    double[] X,
                                    double[] Y,
                                    double[] Z,

                                    int FldCount,
                                    int[] FldIdxs,
                                    String FldVals)
        {
            return Call_ShpPntZ_AddRecs(this.m_pNativeObject,

                                        FileName,

                                        RecCount,
                                        X,
                                        Y,
                                        Z,

                                        FldCount,
                                        FldIdxs,
                                        FldVals);
        }


        /*=== ShpPolyZFixedPtsCount_AddRecs() ===*/
        public bool ShpPolyZFixedPtsCount_AddRecs(String FileName,

                                                  int RecCount,
                                                  int PtsCount,
                                                  double[] X,
                                                  double[] Y,
                                                  double[] Z,

                                                  int FldCount,
                                                  int[] FldIdxs,
                                                  String FldVals)
        {
            return Call_ShpPolyZFixedPtsCount_AddRecs(this.m_pNativeObject,

                                                      FileName,

                                                      RecCount,
                                                      PtsCount,
                                                      X,
                                                      Y,
                                                      Z,

                                                      FldCount,
                                                      FldIdxs,
                                                      FldVals);
        }

        /*=== ShpLineZ_AddRecs() ===*/
        public bool ShpLineZ_AddRecs(String FileName,

                                     int RecCount,
                                     int[] PtsCount,
                                     double[] X,
                                     double[] Y,
                                     double[] Z,

                                     int FldCount,
                                     int[] FldIdxs,
                                     String FldVals)
        {
            return Call_ShpLineZ_AddRecs(this.m_pNativeObject,

                                         FileName,

                                         RecCount,
                                         PtsCount,
                                         X,
                                         Y,
                                         Z,

                                         FldCount,
                                         FldIdxs,
                                         FldVals);
        }

        // 2017-03-13
        /*=== ShpPolyZSinglePart_AddRecs() ===*/
        public bool ShpPolyZSinglePart_AddRecs(String FileName,

                                               int RecCount,
                                               int[] PtsCount,
                                               double[] X,
                                               double[] Y,
                                               double[] Z,

                                               int FldCount,
                                               int[] FldIdxs,
                                               String FldVals)
        {
            return Call_ShpPolyZSinglePart_AddRecs(this.m_pNativeObject,

                                                   FileName,

                                                   RecCount,
                                                   PtsCount,
                                                   X,
                                                   Y,
                                                   Z,

                                                   FldCount,
                                                   FldIdxs,
                                                   FldVals);
        }

        // 2017-03-23
        /*=== ShpPolyZMMultiPart_AddRecs() ===*/
        public bool ShpPolyZMMultiPart_AddRecs(String FileName,

                                               int RecCount,
                                               int[] PartsCount,
                                               int[] PtsCount,
                                               double[] X,
                                               double[] Y,
                                               double[] Z,
                                               double[] M,

                                               int FldCount,
                                               int[] FldIdxs,
                                               String FldVals)
        {
            return Call_ShpPolyZMMultiPart_AddRecs(this.m_pNativeObject,

                                                   FileName,

                                                   RecCount,
                                                   PartsCount,
                                                   PtsCount,
                                                   X,
                                                   Y,
                                                   Z,
                                                   M,

                                                   FldCount,
                                                   FldIdxs,
                                                   FldVals);
        }

        // 2016-06-12

        /*=== SpIdx2D_Start() ===*/
        public bool SpIdx2D_Start()
        {
            return Call_SpIdx2D_Start(this.m_pNativeObject);
        }

        /*=== SpIdx2D_InsertPoint() ===*/
        public bool SpIdx2D_InsertPoint(double X,
                                        double Y,
                                        int RecID)
        {
            return Call_SpIdx2D_InsertPoint(this.m_pNativeObject,

                                            X,
                                            Y,
                                            RecID);
        }

        /*=== SpIdx2D_InsertBox() ===*/
        public bool SpIdx2D_InsertBox(double MinX,
                                      double MinY,
                                      double MaxX,
                                      double MaxY,
                                      int RecID)
        {
            return Call_SpIdx2D_InsertBox(this.m_pNativeObject,

                                          MinX,
                                          MinY,
                                          MaxX,
                                          MaxY,
                                          RecID);
        }

        /*=== SpIdx2D_SearchStart() ===*/
        public int SpIdx2D_SearchStart(double MinX,
                                       double MinY,
                                       double MaxX,
                                       double MaxY)
        {
            return Call_SpIdx2D_SearchStart(this.m_pNativeObject,

                                            MinX,
                                            MinY,
                                            MaxX,
                                            MaxY);
        }

        /*=== SpIdx2D_GetFoundID() ===*/
        public int SpIdx2D_GetFoundID(int Idx)
        {
            return Call_SpIdx2D_GetFoundID(this.m_pNativeObject,
                
                                           Idx);
        }

        /*=== SpIdx2D_SearchClear() ===*/
        public void SpIdx2D_SearchClear()
        {
            Call_SpIdx2D_SearchClear(this.m_pNativeObject);
        }

        /*=== SpIdx2D_End() ===*/
        public void SpIdx2D_End()
        {
            Call_SpIdx2D_End(this.m_pNativeObject);
        }

    }
}
