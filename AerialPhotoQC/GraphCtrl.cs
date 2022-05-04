using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AerialPhotoQC
{
    public partial class GraphCtrl : UserControl
    {
        private const float TITLE_FONT_SIZE = 18.0f;
        private const float TITLE_Y = 10.0f;
        private const int BOX_LEFT = 35;
        private const int BOX_TOP = 50;
        private const int BOX_RIGHT = 20;
        private const int BOX_BOTTOM = 50;
        private const float BOX_FONT_SIZE = 8.0f;
        private const float LEGEND_FONT_SIZE = 12.0f;

        private String m_sTitleX;
        private int m_nStepX;

        private double m_nMin;
        private double m_nMax;
        private double m_nAvg;
        private double m_nMed;
        private double m_nStd;
        private double m_nPerc20Sigmas;
        private double m_nPerc25Sigmas;
        private double m_nPerc30Sigmas;
        private List<double> m_HistX;
        private List<double> m_HistY;

        private double m_nMaxY;

        private bool m_bInfoExists;

        private Font m_TitleFont;
        private Pen m_BoxPen;
        private Font m_BoxFont;
        private Font m_LegendFont;
        private Pen m_GraphPen;
        private Pen m_MedPen;
        private Pen m_AvgPen;
        private Pen m_20SPen;
        private Pen m_25SPen;
        private Pen m_30SPen;

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Public ------------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Public

        /*=== GraphCtrl() ===*/
        public GraphCtrl()
        {
            InitializeComponent();

            m_HistX = null;
            m_HistY = null;

            m_TitleFont = null;
            m_BoxPen = null;
            m_BoxFont = null;
            m_LegendFont = null;
            m_GraphPen = null;
            m_MedPen = null;
            m_AvgPen = null;
            m_20SPen = null;
            m_25SPen = null;
            m_30SPen = null;

            CleanInfo();
        }

        /*=== SetInfo() ===*/
        public void SetInfo(String TitleX,
                            int StepX,
                            
                            double Min,
                            double Max,
                            double Avg,
                            double Med,
                            double Std,
                            double Perc20Sigmas,
                            double Perc25Sigmas,
                            double Perc30Sigmas,
                            List<double> HistX,
                            List<double> HistY)
        {
            int i, Count;

            CleanInfo();

            m_sTitleX = TitleX;
            m_nStepX = StepX;

            m_nMin = Min;
            m_nMax = Max;
            m_nAvg = Avg;
            m_nMed = Med;
            m_nStd = Std;
            m_nPerc20Sigmas = Perc20Sigmas;
            m_nPerc25Sigmas = Perc25Sigmas;
            m_nPerc30Sigmas = Perc30Sigmas;

            Count = HistX.Count;

            m_HistX = new List<double>();
            for (i = 0; i < Count; i++)
                m_HistX.Add(HistX[i]);
            m_HistY = new List<double>();
            m_nMaxY = double.MinValue;
            for (i = 0; i < Count; i++)
            {
                m_HistY.Add(HistY[i]);
                if (HistY[i] > m_nMaxY)
                    m_nMaxY = HistY[i];
            }

            CreateFontsAndPens();

            m_bInfoExists = true;

            MainDP.Invalidate();
        }

        /*=== CleanInfo() ===*/
        public void CleanInfo()
        {
            ClearFontsAndPens();

            m_bInfoExists = false;

            m_sTitleX = "";
            m_nStepX = 0;

            m_nMin = 0.0;
            m_nMax = 0.0;
            m_nAvg = 0.0;
            m_nMed = 0.0;
            m_nStd = 0.0;
            m_nPerc20Sigmas = 0.0;
            m_nPerc25Sigmas = 0.0;
            m_nPerc30Sigmas = 0.0;
            if (m_HistX != null)
            {
                m_HistX.Clear();
                m_HistX = null;
            }
            if (m_HistY != null)
            {
                m_HistY.Clear();
                m_HistY = null;
            }
            m_nMaxY = 0.0;

            MainDP.Invalidate();
        }

        #endregion Public

        /*-----------------------------------------------------------------------------*/
        /*--------------------------------- Private -----------------------------------*/
        /*-----------------------------------------------------------------------------*/
        #region Private

        /*=== MainDP_Resize() ===*/
        private void MainDP_Resize(object sender, EventArgs e)
        {
            MainDP.Invalidate();
        }

        /*=== MainDP_Paint() ===*/
        private void MainDP_Paint(object sender, PaintEventArgs e)
        {
            Graphics g;
            int W, H;

            if (!m_bInfoExists)
                return;

            g = e.Graphics;
            W = MainDP.Width;
            H = MainDP.Height;

            DrawTitle(g,
                      W,
                      H);
            DrawBox(g,
                    W,
                    H);
            DrawGraph(g,
                      W,
                      H);
        }

        /*=== DrawTitle() ===*/
        private void DrawTitle(Graphics g,
                               int W,
                               int H)
        {
            SizeF Size;
            PointF Pos;

            Size = g.MeasureString(m_sTitleX, m_TitleFont);

            Pos = new PointF(0.5f * ((float)W - Size.Width), TITLE_Y);
            g.DrawString(m_sTitleX, m_TitleFont, Brushes.Blue, Pos);
        }

        /*=== DrawBox() ===*/
        private void DrawBox(Graphics g,
                             int W,
                             int H)
        {
            int x, y, w, h;
            double ScaleX, ScaleY;
            int min;
            double X;
            int y1, y2;

            x = BOX_LEFT;
            y = BOX_TOP;
            w = W - BOX_LEFT - BOX_RIGHT;
            h = H - BOX_TOP - BOX_BOTTOM;

            y1 = H - BOX_BOTTOM;
            y2 = BOX_TOP;

            ScaleX = (double)w / (m_nMax - m_nMin);
            ScaleY = (double)h / m_nMaxY;

            g.DrawRectangle(m_BoxPen, x, y, w, h);

            g.DrawString("0.00",
                         m_BoxFont,
                         Brushes.DarkGray,
                         new PointF(5f, (float)(H - BOX_BOTTOM - 5)));
            g.DrawString(String.Format("{0:0.00}", m_nMaxY),
                         m_BoxFont,
                         Brushes.DarkGray,
                         new PointF(5f, (float)(BOX_TOP - 5)));

            x = BOX_LEFT;
            g.DrawString(String.Format("{0:0.00}", m_nMin),
                         m_BoxFont,
                         Brushes.DarkGray,
                         x,
                         BOX_TOP - 20);
            x = W - BOX_RIGHT;
            g.DrawString(String.Format("{0:0.00}", m_nMax),
                         m_BoxFont,
                         Brushes.DarkGray,
                         x - 30,
                         BOX_TOP - 20);

            min = (int)(Math.Ceiling(m_nMin));
            while (min % m_nStepX != 0)
                ++min;

            X = (double)min;
            while (true)
            {
                x = BOX_LEFT + (int)((X - m_nMin) * ScaleX);
                g.DrawLine(m_BoxPen, x, y1, x, y2);

                g.DrawString(String.Format("{0:0}", X),
                             m_BoxFont,
                             Brushes.DarkGray,
                             x - 5,
                             H - BOX_BOTTOM + 5);

                X += (double)m_nStepX;
                if (X > m_nMax)
                    break;
            }
        }

        /*=== DrawGraph() ===*/
        private void DrawGraph(Graphics g,
                               int W,
                               int H)
        {
            int i, Count, w, h;
            double ScaleX, ScaleY;
            int x, y, prevx = 0, prevy = 0;
            int y1, y2;

            w = W - BOX_LEFT - BOX_RIGHT;
            h = H - BOX_TOP - BOX_BOTTOM;

            y1 = H - BOX_BOTTOM;
            y2 = BOX_TOP;

            ScaleX = (double)w / (m_nMax - m_nMin);
            ScaleY = (double)h / m_nMaxY;

            Count = m_HistX.Count;
            for (i = 0; i < Count; i++)
            {
                x = BOX_LEFT + (int)((m_HistX[i] - m_nMin) * ScaleX);
                y = H - BOX_BOTTOM - (int)(m_HistY[i] * ScaleY);

                if (i > 0)
                    g.DrawLine(m_GraphPen, prevx, prevy, x, y);

                prevx = x;
                prevy = y;
            }

            x = BOX_LEFT + (int)((m_nMed - m_nMin) * ScaleX);
            g.DrawLine(m_MedPen, x, y1, x, y2);

            x = BOX_LEFT + (int)((m_nAvg - m_nMin) * ScaleX);
            g.DrawLine(m_AvgPen, x, y1, x, y2);
            g.DrawString(String.Format("{0:0.00}", m_nAvg),
                         m_LegendFont,
                         Brushes.Black,
                         x - 15,
                         H - BOX_BOTTOM + 20);

            x = BOX_LEFT + (int)((m_nAvg - 2.0 * m_nStd - m_nMin) * ScaleX);
            g.DrawLine(m_20SPen, x, y1, x, y2);
            g.DrawString(String.Format("{0:0.00}", m_nAvg - 2.0 * m_nStd),
                         m_LegendFont,
                         Brushes.DarkGreen,
                         x - 15,
                         H - BOX_BOTTOM + 20);
            x = BOX_LEFT + (int)((m_nAvg + 2.0 * m_nStd - m_nMin) * ScaleX);
            g.DrawLine(m_20SPen, x, y1, x, y2);
            g.DrawString(String.Format("{0:0.00}", m_nAvg + 2.0 * m_nStd),
                         m_LegendFont,
                         Brushes.DarkGreen,
                         x - 15,
                         H - BOX_BOTTOM + 20);

            x = BOX_LEFT + (int)((m_nAvg - 2.5 * m_nStd - m_nMin) * ScaleX);
            g.DrawLine(m_25SPen, x, y1, x, y2);
            g.DrawString(String.Format("{0:0.00}", m_nAvg - 2.5 * m_nStd),
                         m_LegendFont,
                         Brushes.Blue,
                         x - 15,
                         H - BOX_BOTTOM + 20);
            x = BOX_LEFT + (int)((m_nAvg + 2.5 * m_nStd - m_nMin) * ScaleX);
            g.DrawLine(m_25SPen, x, y1, x, y2);
            g.DrawString(String.Format("{0:0.00}", m_nAvg + 2.5 * m_nStd),
                         m_LegendFont,
                         Brushes.Blue,
                         x - 15,
                         H - BOX_BOTTOM + 20);

            x = BOX_LEFT + (int)((m_nAvg - 3.0 * m_nStd - m_nMin) * ScaleX);
            g.DrawLine(m_30SPen, x, y1, x, y2);
            g.DrawString(String.Format("{0:0.00}", m_nAvg - 3.0 * m_nStd),
                         m_LegendFont,
                         Brushes.DarkRed,
                         x - 15,
                         H - BOX_BOTTOM + 20);
            x = BOX_LEFT + (int)((m_nAvg + 3.0 * m_nStd - m_nMin) * ScaleX);
            g.DrawLine(m_30SPen, x, y1, x, y2);
            g.DrawString(String.Format("{0:0.00}", m_nAvg + 3.0 * m_nStd),
                         m_LegendFont,
                         Brushes.DarkRed,
                         x - 15,
                         H - BOX_BOTTOM + 20);

            x = BOX_LEFT + 5;
            y = BOX_TOP + 5;
            g.DrawString(String.Format("2.0σ = {0:0.00}%", m_nPerc20Sigmas),
                         m_LegendFont,
                         Brushes.DarkGreen,
                         x,
                         y);

            y += 20;
            g.DrawString(String.Format("2.5σ = {0:0.00}%", m_nPerc25Sigmas),
                         m_LegendFont,
                         Brushes.Blue,
                         x,
                         y);

            y += 20;
            g.DrawString(String.Format("3.0σ = {0:0.00}%", m_nPerc30Sigmas),
                         m_LegendFont,
                         Brushes.DarkRed,
                         x,
                         y);

            // σ
        }
        /*=== CreateFontsAndPens() ===*/
        private void CreateFontsAndPens()
        {
            m_TitleFont = new Font(FontFamily.GenericSansSerif, TITLE_FONT_SIZE);
            m_BoxPen = new Pen(Color.DarkGray, 1f);
            m_BoxFont = new Font(FontFamily.GenericSansSerif, BOX_FONT_SIZE);
            m_LegendFont = new Font(FontFamily.GenericSansSerif, LEGEND_FONT_SIZE);
            m_GraphPen = new Pen(Color.Black, 1f);
            m_MedPen = new Pen(Color.Black, 1f);
            m_AvgPen = new Pen(Color.Black, 2f);
            m_20SPen = new Pen(Color.DarkGreen, 2f);
            m_25SPen = new Pen(Color.Blue, 2f);
            m_30SPen = new Pen(Color.DarkRed, 2f);
        }

        /*=== ClearFontsAndPens() ===*/
        private void ClearFontsAndPens()
        {
            if (m_TitleFont != null)
            {
                m_TitleFont.Dispose();
                m_TitleFont = null;
            }
            if (m_BoxPen != null)
            {
                m_BoxPen.Dispose();
                m_BoxPen = null;
            }
            if (m_BoxFont != null)
            {
                m_BoxFont.Dispose();
                m_BoxFont = null;
            }
            if (m_LegendFont != null)
            {
                m_LegendFont.Dispose();
                m_LegendFont = null;
            }
            if (m_GraphPen != null)
            {
                m_GraphPen.Dispose();
                m_GraphPen = null;
            }
            if (m_MedPen != null)
            {
                m_MedPen.Dispose();
                m_MedPen = null;
            }
            if (m_AvgPen != null)
            {
                m_AvgPen.Dispose();
                m_AvgPen = null;
            }
            if (m_20SPen != null)
            {
                m_20SPen.Dispose();
                m_20SPen = null;
            }
            if (m_25SPen != null)
            {
                m_25SPen.Dispose();
                m_25SPen = null;
            }
            if (m_30SPen != null)
            {
                m_30SPen.Dispose();
                m_30SPen = null;
            }
        }

        #endregion Private
    }
}
