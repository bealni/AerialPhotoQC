using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using ClipperPath = System.Collections.Generic.List<Clipper.IntPoint>;
using ClipperPaths = System.Collections.Generic.List<System.Collections.Generic.List<Clipper.IntPoint>>;

namespace AerialPhotoQC
{
    public partial class ObliqueSimCtrl : UserControl
    {
        private const double DEG_TO_RAD = 0.01745329251994329576923690768489;

        private Cfg m_Cfg;

        private const int MOUSE_DELTA = 20;

        private bool m_bParamsExist;
        private double m_nWidthPix;
        private double m_nHeightPix;
        private double m_nWidth;
        private double m_nHeight;
        private double m_nFocalN;
        private double m_nFocalO;
        private double m_nZ;
        private double m_nBase;

        private double m_nMaxDX;
        private double m_nMaxDY;

        private double[] m_Xc;
        private double[] m_Yc;
        private double[, ,] m_Xs;
        private double[, ,] m_Ys;

        private Pen[] m_Pens;
        private Pen[] m_SelPens;
        private Font m_Font;

        private int[] m_CenterXs;
        private int[] m_CenterYs;

        private bool[] m_EOSelected;

        private int m_nSelOver;
        private bool[] m_OverExists;
        private int[] m_OverCoordsCount;
        private double[,] m_OverCoordsX;
        private double[,] m_OverCoordsY;

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Public ------------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Public

        /*=== ObliqueSimCtrl() ===*/
        public ObliqueSimCtrl()
        {
            InitializeComponent();
        }

        /*=== Open() ===*/
        public void Open(Cfg cfg)
        {
            m_Cfg = cfg;

            DoOpen();
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
        private void DoOpen()
        {
            m_bParamsExist = false;

            m_Xc = new double[2];
            m_Yc = new double[2];
            m_Xs = new double[2, 5, 4];
            m_Ys = new double[2, 5, 4];

            GSDTB.Text = m_Cfg.m_Data.GSD.ToString();
            NadirLonTB.Text = m_Cfg.m_Data.NadiralOver.ToString();

            m_Pens = new Pen[5];
            m_Pens[0] = Pens.Black;
            m_Pens[1] = Pens.Green;
            m_Pens[2] = Pens.Blue;
            m_Pens[3] = Pens.Green;
            m_Pens[4] = Pens.Blue;

            m_SelPens = new Pen[5];
            m_SelPens[0] = new Pen(Color.Black, 4f);
            m_SelPens[1] = new Pen(Color.Green, 4f);
            m_SelPens[2] = new Pen(Color.Blue, 4f);
            m_SelPens[3] = new Pen(Color.Green, 4f);
            m_SelPens[4] = new Pen(Color.Blue, 4f);

            m_Font = new Font(FontFamily.GenericSansSerif, 24f);

            m_CenterXs = new int[2];
            m_CenterYs = new int[2];

            m_EOSelected = new bool[2];
            m_EOSelected[1] = m_EOSelected[0] = false;

            m_nSelOver = -1;
            m_OverExists = new bool[3];
            m_OverExists[2] = m_OverExists[1] = m_OverExists[0] = false;
            m_OverCoordsCount = new int[3];
            m_OverCoordsCount[2] = m_OverCoordsCount[1] = m_OverCoordsCount[0] = 0;
            m_OverCoordsX = new double[4, 4];
            m_OverCoordsY = new double[4, 4];

            SelOverCB.SelectedIndex = 0;

            ApplyB.Focus();
            ApplyB.Select();
        }

        /*=== DoClose() ===*/
        private void DoClose()
        {
            int i;

            m_bParamsExist = false;

            m_Xc = null;
            m_Yc = null;
            m_Xs = null;
            m_Ys = null;

            GSDTB.Text = "";
            NadirLonTB.Text = "";

            for (i = 0; i < 5; i++)
            {
                m_Pens[i].Dispose();
                m_Pens[i] = null;
                m_SelPens[i].Dispose();
                m_SelPens[i] = null;
            }
            m_Pens = null;
            m_SelPens = null;

            m_Font.Dispose();
            m_Font = null;

            m_CenterXs = null;
            m_CenterYs = null;

            m_EOSelected = null;

            m_nSelOver = -1;
            m_OverExists = null;
            m_OverCoordsCount = null;
            m_OverCoordsX = null;
            m_OverCoordsY = null;

            SelOverCB.SelectedIndex = 0;

            ApplyB.Focus();
            ApplyB.Select();
        }

        #endregion Private Open/Close

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private Events ----------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private Events

        /*=== ApplyB_Click() ===*/
        private void ApplyB_Click(object sender, EventArgs e)
        {
            m_bParamsExist = false;
            DrawDP.Invalidate();

            if (!GetParams())
                return;

            m_bParamsExist = true;
            DrawDP.Invalidate();
        }

        /*=== SelOverCB_SelectedIndexChanged() ===*/
        private void SelOverCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            m_nSelOver = SelOverCB.SelectedIndex - 1;

            m_EOSelected[1] = m_EOSelected[0] = false;

            switch (m_nSelOver)
            {
                case 0:
                case 1:
                case 2:
                    m_EOSelected[1] = m_EOSelected[0] = true;
                    break;

                default:
                    break;
            }

            DrawDP.Invalidate();
        }

        /*=== DrawDP_Resize() ===*/
        private void DrawDP_Resize(object sender, EventArgs e)
        {
            DrawDP.Invalidate();
        }

        /*=== DrawDP_MouseMove() ===*/
        private void DrawDP_MouseMove(object sender, MouseEventArgs e)
        {
            int EOIdx;
            bool Found;

            if (!m_bParamsExist)
                return;

            Found = false;
            for (EOIdx = 0; EOIdx < 2; EOIdx++)
            {
                if (Math.Abs(e.X - m_CenterXs[EOIdx]) < MOUSE_DELTA &&
                    Math.Abs(e.Y - m_CenterYs[EOIdx]) < MOUSE_DELTA)
                {
                    Found = true;
                    break;
                }
            }

            if (Found)
            {
                if (DrawDP.Cursor != Cursors.Hand)
                    DrawDP.Cursor = Cursors.Hand;
            }
            else
            {
                if (DrawDP.Cursor != Cursors.Default)
                    DrawDP.Cursor = Cursors.Default;
            }
        }

        /*=== DrawDP_MouseDown() ===*/
        private void DrawDP_MouseDown(object sender, MouseEventArgs e)
        {
            int EOIdx;
            int FoundIdx;

            if (!m_bParamsExist)
                return;

            FoundIdx = -1;
            for (EOIdx = 0; EOIdx < 2; EOIdx++)
            {
                if (Math.Abs(e.X - m_CenterXs[EOIdx]) < MOUSE_DELTA &&
                    Math.Abs(e.Y - m_CenterYs[EOIdx]) < MOUSE_DELTA)
                {
                    FoundIdx = EOIdx;
                    break;
                }
            }

            m_EOSelected[1] = m_EOSelected[0] = false;
            m_nSelOver = -1;
            SelOverCB.SelectedIndex = 0;
            if (FoundIdx != -1)
                m_EOSelected[FoundIdx] = true;

            DrawDP.Invalidate();
        }

        /*=== DrawDP_Paint() ===*/
        private void DrawDP_Paint(object sender, PaintEventArgs e)
        {
            int xc, yc;
            int EOIdx, PhotoIdx, VtxIdx;
            int x, y, prevx, prevy, x0, y0;
            double fact;
            bool DrawSel;

            if (!m_bParamsExist)
                return;

            xc = DrawDP.Width / 2;
            yc = DrawDP.Height / 2;

            fact = Math.Min((double)(DrawDP.Width / 2 - 10) / m_nMaxDX,
                            (double)(DrawDP.Height / 2 - 10) / m_nMaxDY);

            for (EOIdx = 0; EOIdx < 2; EOIdx++)
            {
                x = xc + (int)(m_Xc[EOIdx] * fact);
                y = yc - (int)(m_Yc[EOIdx] * fact);
                if (m_EOSelected[EOIdx])
                    e.Graphics.DrawEllipse(m_SelPens[0], x - 4, y - 4, 9, 9);
                else
                    e.Graphics.DrawEllipse(m_Pens[0], x - 4, y - 4, 9, 9);
                m_CenterXs[EOIdx] = x;
                m_CenterYs[EOIdx] = y;
                for (PhotoIdx = 0; PhotoIdx < 5; PhotoIdx++)
                {
                    x = xc + (int)(m_Xs[EOIdx, PhotoIdx, 0] * fact);
                    y = yc - (int)(m_Ys[EOIdx, PhotoIdx, 0] * fact);
                    x0 = x;
                    y0 = y;
                    prevx = x;
                    prevy = y;
                    DrawSel = false;
                    for (VtxIdx = 1; VtxIdx < 4; VtxIdx++)
                    {
                        x = xc + (int)(m_Xs[EOIdx, PhotoIdx, VtxIdx] * fact);
                        y = yc - (int)(m_Ys[EOIdx, PhotoIdx, VtxIdx] * fact);
                        if (m_EOSelected[EOIdx])
                        {
                            if (m_nSelOver == -1)
                            {
                                DrawSel = true;
                                e.Graphics.DrawLine(m_SelPens[PhotoIdx], prevx, prevy, x, y);
                            }
                            else
                            {
                                if (m_nSelOver == 0)
                                {
                                    if (PhotoIdx == 0)
                                    {
                                        DrawSel = true;
                                        e.Graphics.DrawLine(m_SelPens[PhotoIdx], prevx, prevy, x, y);
                                    }
                                    else
                                        e.Graphics.DrawLine(m_Pens[PhotoIdx], prevx, prevy, x, y);
                                }
                                else
                                    if (m_nSelOver == 1)
                                    {
                                        if (PhotoIdx == 1)
                                        {
                                            DrawSel = true;
                                            e.Graphics.DrawLine(m_SelPens[PhotoIdx], prevx, prevy, x, y);
                                        }
                                        else
                                            e.Graphics.DrawLine(m_Pens[PhotoIdx], prevx, prevy, x, y);
                                    }
                                    else
                                    {
                                        if (PhotoIdx == 2)
                                        {
                                            DrawSel = true;
                                            e.Graphics.DrawLine(m_SelPens[PhotoIdx], prevx, prevy, x, y);
                                        }
                                        else
                                            e.Graphics.DrawLine(m_Pens[PhotoIdx], prevx, prevy, x, y);
                                    }
                            }
                        }
                        else
                            e.Graphics.DrawLine(m_Pens[PhotoIdx], prevx, prevy, x, y);

                        prevx = x;
                        prevy = y;
                    }
                    if (DrawSel)
                        e.Graphics.DrawLine(m_SelPens[PhotoIdx], prevx, prevy, x0, y0);
                    else
                        e.Graphics.DrawLine(m_Pens[PhotoIdx], prevx, prevy, x0, y0);
                }
            }

            if (m_nSelOver != -1)
            {
                if (m_OverExists[m_nSelOver])
                {
                    Point[] points = new Point[m_OverCoordsCount[m_nSelOver]];
                    for (VtxIdx = 0; VtxIdx < m_OverCoordsCount[m_nSelOver]; VtxIdx++)
                    {
                        x = xc + (int)(m_OverCoordsX[m_nSelOver, VtxIdx] * fact);
                        y = yc - (int)(m_OverCoordsY[m_nSelOver, VtxIdx] * fact);
                        points[VtxIdx] = new Point(x, y);
                    }
                    e.Graphics.FillPolygon(Brushes.Yellow, points);

                    if (m_nSelOver == 0)
                    {
                        for (EOIdx = 0; EOIdx < 2; EOIdx++)
                        {
                            x = xc + (int)(m_Xc[EOIdx] * fact);
                            y = yc - (int)(m_Yc[EOIdx] * fact);
                            if (m_EOSelected[EOIdx])
                                e.Graphics.DrawEllipse(m_SelPens[0], x - 4, y - 4, 9, 9);
                            else
                                e.Graphics.DrawEllipse(m_Pens[0], x - 4, y - 4, 9, 9);
                        }
                    }

                    String s;
                    if (m_nSelOver == 0)
                        s = NadirLonTB.Text + "%";
                    else
                        if (m_nSelOver == 1)
                            s = ObliqueLonTB.Text + "%";
                        else
                            s = ObliqueTrnsTB.Text + "%";

                    e.Graphics.DrawString(s, m_Font, Brushes.Black, 130f, 45f, StringFormat.GenericDefault);
                }
            }
        }

        #endregion Private Events

        /*=== GetParams() ===*/
        private bool GetParams()
        {
            if (!DoValidate())
                return false;

            DoCalculate();

            return true;
        }

        /*=== DoValidate() ===*/
        private bool DoValidate()
        {
            double GSD, Over, AreaCCD;

            if (!double.TryParse(GSDTB.Text, out GSD))
            {
                MessageBox.Show("GSD is not a valid number",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            if (!double.TryParse(NadirLonTB.Text, out Over))
            {
                MessageBox.Show("Nadiral Overlap is not a valid number",
                                "ERROR",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                return false;
            }

            m_nWidthPix = m_Cfg.m_Data.WidthPix;
            m_nHeightPix = m_Cfg.m_Data.HeightPix;
            m_nWidth = m_Cfg.m_Data.Width;
            m_nHeight = m_Cfg.m_Data.Height;
            m_nFocalN = m_Cfg.m_Data.FocalN;
            m_nFocalO = m_Cfg.m_Data.FocalO;

            AreaCCD = m_nWidthPix * m_nHeightPix;
            m_nZ = 0.01 * m_nFocalN * Math.Sqrt((AreaCCD * GSD * GSD) / (m_nWidth * m_nHeight));
            m_nBase = ((m_nHeight * m_nZ) * (1 - 0.01 * Over)) / m_nFocalN;

            return true;
        }

        /*=== DoCalculate() ===*/
        private void DoCalculate()
        {
            int i;
            double[] X1;
            double[] Y1;
            double[] X2;
            double[] Y2;
            double Over;

            m_Xc[0] = -0.5 * m_nBase;
            m_Xc[1] = 0.5 * m_nBase;
            m_Yc[1] = m_Yc[0] = 0.0;

            m_nMaxDX = double.MinValue;
            m_nMaxDY = double.MinValue;
            for (i = 0; i < 2; i++)
            {
                CalcNadir(i);
                CalcLeft(i);
                CalcTop(i);
                CalcRight(i);
                CalcBottom(i);
            }

            X1 = new double[4];
            Y1 = new double[4];
            X2 = new double[4];
            Y2 = new double[4];

            for (i = 0; i < 4; i++)
            {
                X1[i] = m_Xs[0, 0, i];
                Y1[i] = m_Ys[0, 0, i];
                X2[i] = m_Xs[1, 0, i];
                Y2[i] = m_Ys[1, 0, i];
            }
            Over = CalcOver(0, X1, Y1, X2, Y2);

            for (i = 0; i < 4; i++)
            {
                X1[i] = m_Xs[0, 1, i];
                Y1[i] = m_Ys[0, 1, i];
                X2[i] = m_Xs[1, 1, i];
                Y2[i] = m_Ys[1, 1, i];
            }
            Over = CalcOver(1, X1, Y1, X2, Y2);
            ObliqueLonTB.Text = String.Format("{0:0.00}", Over);

            for (i = 0; i < 4; i++)
            {
                X1[i] = m_Xs[0, 2, i];
                Y1[i] = m_Ys[0, 2, i];
                X2[i] = m_Xs[1, 2, i];
                Y2[i] = m_Ys[1, 2, i];
            }
            Over = CalcOver(2, X1, Y1, X2, Y2);
            ObliqueTrnsTB.Text = String.Format("{0:0.00}", Over);

            X1 = null;
            Y1 = null;
            X2 = null;
            Y2 = null;
        }

        /*=== CalcNadir() ===*/
        private void CalcNadir(int Idx)
        {
            double x, y, z;
            double X, Y;
            double W, H, F;

            W = 0.5 * m_nWidth * 0.001;
            H = 0.5 * m_nHeight * 0.001;
            F = m_nFocalN * 0.001;

            z = m_nZ - F;

            x = m_Xc[Idx] - H;
            y = m_Yc[Idx] - W;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 0, 0] = X;
            m_Ys[Idx, 0, 0] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = m_Xc[Idx] + H;
            y = m_Yc[Idx] - W;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 0, 1] = X;
            m_Ys[Idx, 0, 1] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = m_Xc[Idx] + H;
            y = m_Yc[Idx] + W;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 0, 2] = X;
            m_Ys[Idx, 0, 2] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = m_Xc[Idx] - H;
            y = m_Yc[Idx] + W;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 0, 3] = X;
            m_Ys[Idx, 0, 3] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);
        }

        /*=== CalcLeft() ===*/
        private void CalcLeft(int Idx)
        {
            double x, y, z;
            double X, Y;
            double W, H1, H2, F;
            double fact1 = Math.Cos(m_Cfg.m_Data.ObliqueAngle * DEG_TO_RAD);
            double fact2 = Math.Sin(m_Cfg.m_Data.ObliqueAngle * DEG_TO_RAD);
            double DX, DZ;

            W = 0.5 * m_nWidth * 0.001;
            H1 = 0.5 * m_nHeight * 0.001 * fact2;
            H2 = 0.5 * m_nHeight * 0.001 * fact1;
            F = m_nFocalO * 0.001;
            DX = -F * fact1;
            DZ = F * fact2;

            x = DX + m_Xc[Idx] - H1;
            y = m_Yc[Idx] - W;
            z = m_nZ - DZ + H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 1, 0] = X;
            m_Ys[Idx, 1, 0] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = DX + m_Xc[Idx] + H1;
            y = m_Yc[Idx] - W;
            z = m_nZ - DZ - H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 1, 1] = X;
            m_Ys[Idx, 1, 1] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = DX + m_Xc[Idx] + H1;
            y = m_Yc[Idx] + W;
            z = m_nZ - DZ - H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 1, 2] = X;
            m_Ys[Idx, 1, 2] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = DX + m_Xc[Idx] - H1;
            y = m_Yc[Idx] + W;
            z = m_nZ - DZ + H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 1, 3] = X;
            m_Ys[Idx, 1, 3] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);
        }

        /*=== CalcTop() ===*/
        private void CalcTop(int Idx)
        {
            double x, y, z;
            double X, Y;
            double W, H1, H2, F;
            double fact1 = Math.Cos(m_Cfg.m_Data.ObliqueAngle * DEG_TO_RAD);
            double fact2 = Math.Sin(m_Cfg.m_Data.ObliqueAngle * DEG_TO_RAD);
            double DY, DZ;

            W = 0.5 * m_nWidth * 0.001;
            H1 = 0.5 * m_nHeight * 0.001 * fact2;
            H2 = 0.5 * m_nHeight * 0.001 * fact1;
            F = m_nFocalO * 0.001;
            DY = F * fact1;
            DZ = F * fact2;

            x = m_Xc[Idx] - W;
            y = DY + m_Yc[Idx] + H1;
            z = m_nZ - DZ + H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 2, 0] = X;
            m_Ys[Idx, 2, 0] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = m_Xc[Idx] - W;
            y = DY + m_Yc[Idx] - H1;
            z = m_nZ - DZ - H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 2, 1] = X;
            m_Ys[Idx, 2, 1] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = m_Xc[Idx] + W;
            y = DY + m_Yc[Idx] - H1;
            z = m_nZ - DZ - H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 2, 2] = X;
            m_Ys[Idx, 2, 2] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = m_Xc[Idx] + W;
            y = DY + m_Yc[Idx] + H1;
            z = m_nZ - DZ + H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 2, 3] = X;
            m_Ys[Idx, 2, 3] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);
        }

        /*=== CalcRight() ===*/
        private void CalcRight(int Idx)
        {
            double x, y, z;
            double X, Y;
            double W, H1, H2, F;
            double fact1 = Math.Cos(m_Cfg.m_Data.ObliqueAngle * DEG_TO_RAD);
            double fact2 = Math.Sin(m_Cfg.m_Data.ObliqueAngle * DEG_TO_RAD);
            double DX, DZ;

            W = 0.5 * m_nWidth * 0.001;
            H1 = 0.5 * m_nHeight * 0.001 * fact2;
            H2 = 0.5 * m_nHeight * 0.001 * fact1;
            F = m_nFocalO * 0.001;
            DX = F * fact1;
            DZ = F * fact2;

            x = DX + m_Xc[Idx] - H1;
            y = m_Yc[Idx] - W;
            z = m_nZ - DZ - H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 3, 0] = X;
            m_Ys[Idx, 3, 0] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = DX + m_Xc[Idx] + H1;
            y = m_Yc[Idx] - W;
            z = m_nZ - DZ + H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 3, 1] = X;
            m_Ys[Idx, 3, 1] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = DX + m_Xc[Idx] + H1;
            y = m_Yc[Idx] + W;
            z = m_nZ - DZ + H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 3, 2] = X;
            m_Ys[Idx, 3, 2] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = DX + m_Xc[Idx] - H1;
            y = m_Yc[Idx] + W;
            z = m_nZ - DZ - H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 3, 3] = X;
            m_Ys[Idx, 3, 3] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);
        }

        /*=== CalcBottom() ===*/
        private void CalcBottom(int Idx)
        {
            double x, y, z;
            double X, Y;
            double W, H1, H2, F;
            double fact1 = Math.Cos(m_Cfg.m_Data.ObliqueAngle * DEG_TO_RAD);
            double fact2 = Math.Sin(m_Cfg.m_Data.ObliqueAngle * DEG_TO_RAD);
            double DY, DZ;

            W = 0.5 * m_nWidth * 0.001;
            H1 = 0.5 * m_nHeight * 0.001 * fact2;
            H2 = 0.5 * m_nHeight * 0.001 * fact1;
            F = m_nFocalO * 0.001;
            DY = -F * fact1;
            DZ = F * fact2;

            x = m_Xc[Idx] + W;
            y = DY + m_Yc[Idx] - H1;
            z = m_nZ - DZ + H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 4, 0] = X;
            m_Ys[Idx, 4, 0] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = m_Xc[Idx] + W;
            y = DY + m_Yc[Idx] + H1;
            z = m_nZ - DZ - H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 4, 1] = X;
            m_Ys[Idx, 4, 1] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = m_Xc[Idx] - W;
            y = DY + m_Yc[Idx] + H1;
            z = m_nZ - DZ - H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 4, 2] = X;
            m_Ys[Idx, 4, 2] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);

            x = m_Xc[Idx] - W;
            y = DY + m_Yc[Idx] - H1;
            z = m_nZ - DZ + H2;
            RayPlaneIntersect(m_Xc[Idx],
                              m_Yc[Idx],
                              m_nZ,
                              x,
                              y,
                              z,

                              out X,
                              out Y);
            m_Xs[Idx, 4, 3] = X;
            m_Ys[Idx, 4, 3] = Y;
            if (Math.Abs(X) > m_nMaxDX)
                m_nMaxDX = Math.Abs(X);
            if (Math.Abs(Y) > m_nMaxDY)
                m_nMaxDY = Math.Abs(Y);
        }

        /*=== RayPlaneIntersect() ===*/
        private void RayPlaneIntersect(double x1,
                                       double y1,
                                       double z1,
                                       double x2,
                                       double y2,
                                       double z2,

                                       out double x,
                                       out double y)
        {
            double vx, vy, vz, v;
            double nx, ny, nz, t;

            vx = x2 - x1;
            vy = y2 - y1;
            vz = z2 - z1;
            v = Math.Sqrt(vx * vx + vy * vy + vz * vz);
            nx = vx / v;
            ny = vy / v;
            nz = vz / v;

            t = -z1 / nz;

            x = x1 + t * nx;
            y = y1 + t * ny;
        }

        /*=== CalcOver() ===*/
        private double CalcOver(int OverIdx,

                                double[] X1,
                                double[] Y1,

                                double[] X2,
                                double[] Y2)
        {
            int i;
            double Over = 0.0;
            double MinX = double.MaxValue, MinY = double.MaxValue, Area, AreaC;
            ClipperPath Poly, PolyC;
            Clipper.Clipper c;
            ClipperPaths solution;

            for (i = 0; i < 4; i++)
            {
                if (X1[i] < MinX)
                    MinX = X1[i];
                if (Y1[i] < MinY)
                    MinY = Y1[i];
                if (X2[i] < MinX)
                    MinX = X2[i];
                if (Y2[i] < MinY)
                    MinY = Y2[i];
            }

            Poly = new ClipperPath();
            PolyC = new ClipperPath();
            for (i = 0; i < 4; i++)
            {
                Poly.Add(new Clipper.IntPoint((int)(10000.0 * (X1[i] - MinX)),
                                              (int)(10000.0 * (Y1[i] - MinY))));
                PolyC.Add(new Clipper.IntPoint((int)(10000.0 * (X2[i] - MinX)),
                                               (int)(10000.0 * (Y2[i] - MinY))));
            }
            Area = 1.0e-8 * Math.Abs(Clipper.Clipper.Area(Poly));

            c = new Clipper.Clipper();
            c.AddPath(Poly, Clipper.PolyType.ptClip, true);
            c.AddPath(PolyC, Clipper.PolyType.ptSubject, true);
            solution = new ClipperPaths();
            c.Execute(Clipper.ClipType.ctIntersection, solution, Clipper.PolyFillType.pftEvenOdd, Clipper.PolyFillType.pftEvenOdd);
            if (solution.Count == 1)
            {
                AreaC = Math.Abs(1.0e-8 * Clipper.Clipper.Area(solution[0]));
                Over = 100.0 * (AreaC / Area);

                if (solution[0].Count == 3 || solution[0].Count == 4)
                {
                    m_OverCoordsCount[OverIdx] = solution[0].Count;
                    m_OverExists[OverIdx] = true;
                    m_OverCoordsX[OverIdx, 0] = MinX + (double)(solution[0][0].X) * 0.0001;
                    m_OverCoordsY[OverIdx, 0] = MinY + (double)(solution[0][0].Y) * 0.0001;
                    m_OverCoordsX[OverIdx, 1] = MinX + (double)(solution[0][1].X) * 0.0001;
                    m_OverCoordsY[OverIdx, 1] = MinY + (double)(solution[0][1].Y) * 0.0001;
                    m_OverCoordsX[OverIdx, 2] = MinX + (double)(solution[0][2].X) * 0.0001;
                    m_OverCoordsY[OverIdx, 2] = MinY + (double)(solution[0][2].Y) * 0.0001;
                    if (m_OverCoordsCount[OverIdx] == 4)
                    {
                        m_OverCoordsX[OverIdx, 3] = MinX + (double)(solution[0][3].X) * 0.0001;
                        m_OverCoordsY[OverIdx, 3] = MinY + (double)(solution[0][3].Y) * 0.0001;
                    }
                }
                else
                    m_OverExists[OverIdx] = false;
            }
            else
                m_OverExists[OverIdx] = false;

            Poly.Clear();
            Poly = null;
            c.Clear();
            c = null;
            solution.Clear();
            solution = null;

return Over;
        }

        #endregion Private
    }
}
