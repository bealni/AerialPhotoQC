using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AerialPhotoQC
{
    class Util
    {
        public const double _PI_DIV_180 = 0.017453292519943295769236907684883; // PI / 180.0
        public const double _180_DIV_PI = 57.295779513082320876798154814114;   // 180.0 / PI

        private const double UTM_A = 6378137.00;                                // Semimajor Axis
        private const double UTM_E = 0.081819191042832221962301696167414;	// Eccentricity
        private const double UTM_E2 = 0.0066943800229034765012105137949192;     // Eccentricity ^ 2
        private const double UTM_E4 = 4.4814723891049150564916514506058e-5;     // Eccentricity ^ 4
        private const double UTM_E6 = 3.0000679234817458810564743813201e-7;     // Eccentricity ^ 6

        private const double UTM_EP = 0.082094438151933797612484334094693;		// Second Eccentricity
        private const double UTM_EP2 = 0.0067394967754816834794891652395675;    // Second Eccentricity ^ 2

        private const double UTM_K0 = 0.9996;                                   // Scale Factor along the Central Meridian
        private const double UTM_E0 = 500000.00;                                // False Easting, Meters
        private const double UTM_N0N = 0.00;                                    // False Northing, North, Meters
        private const double UTM_N0S = 10000000.00;                             // False Northing, South, Meters

        private const double UTM_E1 = 0.0016792203945127758389885689586122;     // (1 - sqrt(1 - e^2)) / (1 + sqrt(1 - e^2))
        private const double UTM_E12 = 2.8197811333476425290230697153483e-6;	// UTM_E1 ^ 2
        private const double UTM_E13 = 4.7350339871797104629737508024224e-9;	// UTM_E1 ^ 3
        private const double UTM_E14 = 7.9511656399833153778561691381511e-12;   // UTM_E1 ^ 3

        /*---------------------------------------------------------------------------------------*/
        /*----------------------------------- General -------------------------------------------*/
        /*---------------------------------------------------------------------------------------*/
        #region General

        /*=== Norm2D() ===*/
        public static double Norm2D(double x,
                                    double y)
        {
            return Math.Sqrt(x * x + y * y);
        }

        /*=== Dot2D() ===*/
        public static double Dot2D(double x1,
                                   double y1,
                                   double x2,
                                   double y2)
        {
            return (x1 * x2 + y1 * y2);
        }

        /*=== Cross2D() ===*/
        public static double Cross2D(double x1,
                                     double y1,
                                     double x2,
                                     double y2)
        {
            return (x1 * y2 - y1 * x2);
        }

        /*=== DistPntToLin2D() ===*/
        public static double DistPntToLin2D(double x1,
                                            double y1,
                                            double x2,
                                            double y2,
                                            double px,
                                            double py)
        {
            double vx, vy, vpx, vpy;

            vx = x2 - x1;
            vy = y2 - y1;
            vpx = px - x1;
            vpy = py - y1;

            return (Math.Abs(Cross2D(vx, vy, vpx, vpy)) / Norm2D(vx, vy));
        }

        /*=== PointInPoly() ===*/
        public static bool PointInPoly(List<double> PolyX,
                                       List<double> PolyY,
                                       double X,
                                       double Y)
        {
            int i, j, Count;
            bool OddInt;

            Count = PolyX.Count;
            j = Count - 1;
            OddInt = false;
            for (i = 0; i < Count; i++)
            {
                if ((PolyY[i] < Y && PolyY[j] >= Y) ||
                    (PolyY[j] < Y && PolyY[i] >= Y))
                {
                    if (PolyX[i] + (Y - PolyY[i]) / (PolyY[j] - PolyY[i]) * (PolyX[j] - PolyX[i]) < X)
                    {
                        OddInt = !OddInt;
                    }
                }
                j = i;
            }

            return OddInt;
        }

        /*=== PointPolyLineDist() ===*/
        public static double PointPolyLineDist(double Xp,
                                               double Yp,
                                               List<double> PolyX,
                                               List<double> PolyY)
        {
            int Count, i;
            double Xs1, Ys1, Xs2, Ys2;
            double Dist, MinDist;

            Count = PolyX.Count;
            MinDist = 1.0e+100;
            for (i = 0; i < Count - 1; i++)
            {
                Xs1 = PolyX[i];
                Ys1 = PolyY[i];
                Xs2 = PolyX[i + 1];
                Ys2 = PolyY[i + 1];
                Dist = PointSegDist(Xp,
                                    Yp,
                                    Xs1,
                                    Ys1,
                                    Xs2,
                                    Ys2);
                if (Dist < MinDist)
                {
                    MinDist = Dist;
                }
            }

            return MinDist;
        }

        /*=== PointSegDist() ===*/
        public static double PointSegDist(double Xp,
                                          double Yp,
                                          double Xs1,
                                          double Ys1,
                                          double Xs2,
                                          double Ys2)
        {
            double vx, vy;
            double wx, wy;
            double c1, c2, b;
            double px, py;
            double dx, dy;
            double Dist;

            vx = Xs2 - Xs1;
            vy = Ys2 - Ys1;
            wx = Xp - Xs1;
            wy = Yp - Ys1;

            c1 = wx * vx + wy * vy;
            if (c1 <= 0.0)
            {
                Dist = Math.Sqrt(wx * wx + wy * wy);                
            }
            else
            {
                c2 = vx * vx + vy * vy;
                if (c2 <= c1)
                {
                    dx = Xp - Xs2;
                    dy = Yp - Ys2;
                    Dist = Math.Sqrt(dx * dx + dy * dy);
                }
                else
                {
                    b = c1 / c2;
                    px = Xs1 + b * vx;
                    py = Ys1 + b * vy;
                    dx = Xp - px;
                    dy = Yp - py;
                    Dist = Math.Sqrt(dx * dx + dy * dy);
                }
            }

            return Dist;
        }


        /*=== SegmentsIntersect() ===*/
        public static bool SegmentsIntersect(double p0X,
                                             double p0Y,
                                             double p1X,
                                             double p1Y,
                                             double q0X,
                                             double q0Y,
                                             double q1X,
                                             double q1Y)
        {
            double ux, uy, vx, vy, wx, wy;
            double d, s1, s2;

            ux = p1X - p0X;
            uy = p1Y - p0Y;
            vx = q1X - q0X;
            vy = q1Y - q0Y;
            wx = p0X - q0X;
            wy = p0Y - q0Y;

            d = (ux * vy - uy * vx);
            s1 = (vx * wy - vy * wx) / d;

            ux = q1X - q0X;
            uy = q1Y - q0Y;
            vx = p1X - p0X;
            vy = p1Y - p0Y;
            wx = q0X - p0X;
            wy = q0Y - p0Y;

            d = (ux * vy - uy * vx);
            s2 = (vx * wy - vy * wx) / d;

            return (s1 >= 0.0 && s1 <= 1.0 && s2 >= 0.0 && s2 <= 1.0);
        }

        #endregion General

        /*---------------------------------------------------------------------------------------*/
        /*----------------------------------- UTM/GEO Conversions -------------------------------*/
        /*---------------------------------------------------------------------------------------*/
        #region UTM/GEO Conversions

        /*=== GEO2UTMZone() ===*/
        public static void GEO2UTMZone(double lat,
                                       double lon,
                                       out int ZoneNo,
                                       out int NorthSouth)
        {
            double Lon;

            Lon = (lon + 180.0) - ((int)((lon + 180.0) / 360.0)) * 360.0 - 180.0; // -180.00 .. 179.9;
            ZoneNo = ((int)((Lon + 180.0) / 6.0)) + 1;
            if (lat >= 0.0)
                NorthSouth = 0;
            else
                NorthSouth = 1;
        }

        /*=== GEO2UTM() ===*/
        public static void GEO2UTM(int ZoneNo,
                                   int NorthSouth,
                                   double lat,
                                   double lon,
                                   out double x,
                                   out double y)
        {
            double k0, fi0, lambda0, E0, N0;
            double fi_rad, lambda_rad, fi0_rad, lambda0_rad;
            double N, M, M0, A, T, C;

            GetUTMConstants(ZoneNo,
                            NorthSouth,
                            lat,
                            lon,
                            out k0,
                            out fi0,
                            out lambda0,
                            out E0,
                            out N0);

            fi_rad = _PI_DIV_180 * lat;
            lambda_rad = _PI_DIV_180 * lon;
            fi0_rad = _PI_DIV_180 * fi0;
            lambda0_rad = _PI_DIV_180 * lambda0;

            M = UTM_A *
                (
                    (1 - UTM_E2 / 4 - 3 * UTM_E4 / 64 - 5 * UTM_E6 / 256) * fi_rad -
                    (3 * UTM_E2 / 8 + 3 * UTM_E4 / 32 + 45 * UTM_E6 / 1024) * Math.Sin(2 * fi_rad) +
                    (15 * UTM_E4 / 256 + 45 * UTM_E6 / 1024) * Math.Sin(4 * fi_rad) -
                    (35 * UTM_E6 / 3072) * Math.Sin(6 * fi_rad)
                );

            M0 = UTM_A *
                (
                    (1 - UTM_E2 / 4 - 3 * UTM_E4 / 64 - 5 * UTM_E6 / 256) * fi0_rad -
                    (3 * UTM_E2 / 8 + 3 * UTM_E4 / 32 + 45 * UTM_E6 / 1024) * Math.Sin(2 * fi0_rad) +
                    (15 * UTM_E4 / 256 + 45 * UTM_E6 / 1024) * Math.Sin(4 * fi0_rad) -
                    (35 * UTM_E6 / 3072) * Math.Sin(6 * fi0_rad)
                );

            T = (Math.Tan(fi_rad));
            T = T * T;

            C = UTM_EP * Math.Cos(fi_rad);
            C = C * C;

            A = (lambda_rad - lambda0_rad) * Math.Cos(fi_rad);

            N = UTM_E * Math.Sin(fi_rad);
            N = N * N;
            N = UTM_A / Math.Sqrt(1 - N);

            x = k0 * N *
                (A +
                    (1 - T + C) * (A * A * A) / 6 +
                    (5 - 18 * T + (T * T) + 72 * C - 58 * UTM_EP2) * (A * A * A * A * A) / 120
                ) +
                E0;

            y = k0 *
                (M - M0 +
                    N * Math.Tan(fi_rad) *
                    ((A * A) / 2 +
                        (5 - T + 9 * C + 4 * (C * C)) * (A * A * A * A) / 24 +
                        (61 - 58 * T + (T * T) + 600 * C - 330 * UTM_EP2) * (A * A * A * A * A * A) / 720
                    )
                ) +
                N0;
        }

        /*=== UTM2GEO() ===*/
        public static void UTM2GEO(int ZoneNo,
                                   int NorthSouth,
                                   double x,
                                   double y,
                                   out double lat,
                                   out double lon)
        {
            double k0, fi0, lambda0, E0, N0;
            double fi_rad, lambda_rad, fi0_rad, lambda0_rad;
            double M0, M, mu, fi1, R1, sinfi12, C1, cosfi1, cosfi12, T1, tanfi1, N1, D;


            GetUTMConstantsFixed(ZoneNo,
                                 NorthSouth,
                                 out k0,
                                 out fi0,
                                 out lambda0,
                                 out E0,
                                 out N0);

            fi0_rad = _PI_DIV_180 * fi0;
            lambda0_rad = _PI_DIV_180 * lambda0;

            M0 = UTM_A *
            (
                (1 - UTM_E2 / 4 - 3 * UTM_E4 / 64 - 5 * UTM_E6 / 256) * fi0_rad -
                (3 * UTM_E2 / 8 + 3 * UTM_E4 / 32 + 45 * UTM_E6 / 1024) * Math.Sin(2 * fi0_rad) +
                (15 * UTM_E4 / 256 + 45 * UTM_E6 / 1024) * Math.Sin(4 * fi0_rad) -
                (35 * UTM_E6 / 3072) * Math.Sin(6 * fi0_rad)
            );

            M = M0 + (y - N0) / k0;

            mu = M / (UTM_A * (1 - UTM_E2 / 4 - 3 * UTM_E4 / 64 - 5 * UTM_E6 / 256));

            fi1 = mu +
                    (3 * UTM_E1 / 2 - 27 * UTM_E13 / 32) * Math.Sin(2 * mu) +
                    (21 * UTM_E12 / 16 - 55 * UTM_E14 / 32) * Math.Sin(4 * mu) +
                    (151 * UTM_E13 / 96) * Math.Sin(6 * mu) +
                    (1097 * UTM_E14 / 512) * Math.Sin(8 * mu);

            sinfi12 = Math.Sin(fi1);
            sinfi12 = sinfi12 * sinfi12;

            R1 = (UTM_A * (1 - UTM_E2)) /
            Math.Pow((1 - UTM_E2 * sinfi12), 1.5);

            cosfi1 = Math.Cos(fi1);
            cosfi12 = cosfi1 * cosfi1;

            tanfi1 = Math.Tan(fi1);

            C1 = UTM_EP2 * cosfi12;
            T1 = tanfi1 * tanfi1;
            N1 = UTM_A / Math.Sqrt(1 - UTM_E2 * sinfi12);

            D = (x - E0) / (N1 * k0);

            fi_rad = fi1 -
                    ((N1 * tanfi1) / R1) *
                    (
                        (D * D) / 2 -
                        (5 + 3 * T1 + 10 * C1 - 4 * (C1 * C1) - 9 * UTM_EP2) * (D * D * D * D) / 24 +
                        (61 + 90 * T1 + 298 * C1 + 45 * (T1 * T1) - 252 * UTM_EP2 - 3 * (C1 * C1)) * (D * D * D * D * D * D) / 720
                    );

            lambda_rad = lambda0_rad +
                        (1 / cosfi1) *
                        (
                            D - (1 + 2 * T1 + C1) * (D * D * D) / 6 +
                            (5 - 2 * C1 + 28 * T1 - 3 * (C1 * C1) + 8 * UTM_EP2 + 24 * (T1 * T1)) * (D * D * D * D * D) / 120
                        );

            lat = _180_DIV_PI * fi_rad;
            lon = _180_DIV_PI * lambda_rad;
        }

        /*=== Cnv_ft2m() ===*/
        public static double Cnv_ft2m(double ft)
        {
            return 0.3048 * ft;
        }
        /*=== Cnv_m2ft() ===*/
        public static double Cnv_m2ft(double m)
        {
            return 3.2808398950131233595800524934383 * m;
        }
        /*=== Cnv_kt2ms() ===*/
        public static double Cnv_kt2ms(double kt)
        {
            return 0.514444 * kt;
        }
        /*=== Cnv_ms2kt() ===*/
        public static double Cnv_ms2kt(double ms)
        {
            return 1.9438461717893492780555317974357 * ms;
        }
        /*=== Cnv_m2nm() ===*/
        public static double Cnv_m2nm(double m)
        {
            return 5.3995680345572354211663066954644e-4 * m;
        }
        /*=== Cnv_nm2m() ===*/
        public static double Cnv_nm2m(double nm)
        {
            return 1852 * nm;
        }

        /*=== GetUTMConstantsFixed() ===*/
        private static void GetUTMConstantsFixed(int ZoneNo,
                                                 int NorthSouth,
                                                 out double k0,
                                                 out double fi0,
                                                 out double lambda0,
                                                 out double E0,
                                                 out double N0)
        {
            k0 = UTM_K0;
            fi0 = 0.00;

            lambda0 = 3.00 + 6.00 * (ZoneNo - 1) - 180.00;

            E0 = UTM_E0;

            if (NorthSouth == 0)
                N0 = UTM_N0N;
            else
                N0 = UTM_N0S;
        }

        /*=== GetUTMConstants() ===*/
        private static void GetUTMConstants(int ZoneNo,
                                            int NorthSouth,
                                            double lat,
                                            double lon,
                                            out double k0,
                                            out double fi0,
                                            out double lambda0,
                                            out double E0,
                                            out double N0)
        {
            k0 = UTM_K0;
            fi0 = 0.00;

            lambda0 = 3.00 + 6.00 * (ZoneNo - 1) - 180.00;

            E0 = UTM_E0;

            if (NorthSouth == 0)
                N0 = UTM_N0N;
            else
                N0 = UTM_N0S;
        }

        #endregion UTM/GEO Conversions
    }
}
