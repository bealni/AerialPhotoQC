using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AerialPhotoQC
{
    public class Cfg_Data
    {
        private const String TOKEN = "Aerial Photo Quality Control Configuration File";

        private const double DEF_GSD = 8;
        private const double DEF_NADIRAL_OVER = 70;

        private const double DEF_WIDTH_PIX = 14204;
        private const double DEF_HEIGHT_PIX = 10652;
        private const double DEF_WIDTH_MM = 53.41;
        private const double DEF_HEIGHT_MM = 39.95;
        private const double DEF_FOCAL_N = 52;
        private const double DEF_FOCAL_O = 69;
        private const double DEF_OBLIQUE_ANGLE = 45;

        private const bool DEF_OBLIQUE_STATS = true;
        private const double DEF_OL_B = 0.642995246080177;
        private const double DEF_OL_C = 35.6998867702733;
        private const double DEF_OF_A = 0.001069959510154;
        private const double DEF_OF_B = 0.569895638010965;
        private const double DEF_OF_C = 32.3121795573913;

        public String Token;

        public double GSD;
        public double NadiralOver;

        public double WidthPix;
        public double HeightPix;
        public double Width;
        public double Height;
        public double FocalN;
        public double FocalO;
        public double ObliqueAngle;

        public bool ObliqueStats;
        public double OL_B;
        public double OL_C;
        public double OF_A;
        public double OF_B;
        public double OF_C;

        /*=== Cfg_Data() ===*/
        public Cfg_Data()
        {
            Close();
        }

        /*=== OpenNew() ===*/
        public void OpenNew()
        {
            Close();

            Token = TOKEN;

            GSD = DEF_GSD;
            NadiralOver = DEF_NADIRAL_OVER;

            WidthPix = DEF_WIDTH_PIX;
            HeightPix = DEF_HEIGHT_PIX;
            Width = DEF_WIDTH_MM;
            Height = DEF_HEIGHT_MM;
            FocalN = DEF_FOCAL_N;
            FocalO = DEF_FOCAL_O;
            ObliqueAngle = DEF_OBLIQUE_ANGLE;

            ObliqueStats = DEF_OBLIQUE_STATS;
            OL_B = DEF_OL_B;
            OL_C = DEF_OL_C;
            OF_A = DEF_OF_A;
            OF_B = DEF_OF_B;
            OF_C = DEF_OF_C;
        }

        /*=== Close() ===*/
        public void Close()
        {
            Token = "";

            GSD = 0.0;
            NadiralOver = 0.0;

            WidthPix = 0.0;
            HeightPix = 0.0;
            Width = 0.0;
            Height = 0.0;
            FocalN = 0.0;
            FocalO = 0.0;
            ObliqueAngle = 0.0;

            ObliqueStats = DEF_OBLIQUE_STATS;
            OL_B = DEF_OL_B;
            OL_C = DEF_OL_C;
            OF_A = DEF_OF_A;
            OF_B = DEF_OF_B;
            OF_C = DEF_OF_C;
        }
    }
}
