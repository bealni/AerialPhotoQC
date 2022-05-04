using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AerialPhotoQC
{
    public class MonoModel
    {
        /*-------------------------------------------------------------------*/
        /*-------------------------- Variables ------------------------------*/
        /*-------------------------------------------------------------------*/
        #region Variables

        // Camera
        public double[] m_CamMatrix;
        public double[] m_CamMatrixInv;
        public int m_nCamCols;
        public int m_nCamRows;
        public double m_nCamFocal;
        public double m_nSensorPixSize;
        public double m_nSensorPixSizeInv;

        // Photo
        public double[] m_PhotoProjCenter;
        public double[] m_PhotoRotMatrix;
        public double[] m_PhotoRotMatrixInv;

        // Is Open
        public bool m_bIsOpen;

        #endregion Variables

        /*-------------------------------------------------------------------*/
        /*-------------------------- Public ---------------------------------*/
        /*-------------------------------------------------------------------*/
        #region Public

        /*=== MonoModel() ===*/
        public MonoModel()
        {
            Close();
        }

        /*=== Open() ===*/
        public void Open(double[] CamMatrix,
                         double[] CamMatrixInv,
                         int CamCols,
                         int CamRows,
                         double CamFocal)
        {
            int i;

            Close();

            // Camera
            m_CamMatrix = new double[6];
            m_CamMatrixInv = new double[6];
            for (i = 0; i < 6; i++)
            {
                m_CamMatrix[i] = CamMatrix[i];
                m_CamMatrixInv[i] = CamMatrixInv[i];
                if (!(i == 2 || i == 5) && m_CamMatrixInv[i] != 0)
                    m_nSensorPixSize = Math.Abs(m_CamMatrixInv[i]);
            }
            m_nCamCols = CamCols;
            m_nCamRows = CamRows;
            m_nCamFocal = CamFocal;
            m_nSensorPixSizeInv = 1.0 * m_nSensorPixSize;

            m_PhotoProjCenter = new double[3] { 0.0, 0.0, 0.0 };
            m_PhotoRotMatrix = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
            m_PhotoRotMatrixInv = new double[9] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };

            m_bIsOpen = true;
        }

        /*=== Close() ===*/
        public void Close()
        {
            m_bIsOpen = false;

            // Camera
            m_CamMatrix = null;
            m_CamMatrixInv = null;
            m_nCamCols = 0;
            m_nCamRows = 0;
            m_nCamFocal = 0.0;

            // Photo
            m_PhotoProjCenter = null;
            m_PhotoRotMatrix = null;
            m_PhotoRotMatrixInv = null;
        }

        /*=== SetImgPos() ===*/
        public void SetImgPos(double X,
                              double Y,
                              double Z)
        {
            m_PhotoProjCenter[0] = X;
            m_PhotoProjCenter[1] = Y;
            m_PhotoProjCenter[2] = Z;
        }

        /*=== SetImgParams() ===*/
        public void SetImgParams(double X,
                                 double Y,
                                 double Z,

                                 double KappaRad)
        {
            double sinkp, coskp;

            m_PhotoProjCenter[0] = X;
            m_PhotoProjCenter[1] = Y;
            m_PhotoProjCenter[2] = Z;

            sinkp = Math.Sin(KappaRad);
            coskp = Math.Cos(KappaRad);

            m_PhotoRotMatrix[0] = /* cosph = 1 */ coskp;
            m_PhotoRotMatrix[1] = /* cosom = 1 */ sinkp /* + sinom = 0 * sinph = 0 * coskp */;
            m_PhotoRotMatrix[2] = 0.0 /* sinom = 0 * sinkp - cosom = 1 * sinph = 0 * coskp */;

            m_PhotoRotMatrix[3] = - /* cosph = 1 */ sinkp;
            m_PhotoRotMatrix[4] = /* cosom = 1 */ coskp /* - sinom = 0 * sinph = 0 * sinkp */;
            m_PhotoRotMatrix[5] = 0.0 /* sinom = 0 * coskp + cosom = 1 * sinph = 0 * sinkp */;

            m_PhotoRotMatrix[6] = 0.0 /* sinph = 0 */;
            m_PhotoRotMatrix[7] = 0.0 /* -sinom = 0 * cosph = 1 */;
            m_PhotoRotMatrix[8] = 1.0 /* cosom = 1 * cosph = 1 */;

            m_PhotoRotMatrixInv[0] = m_PhotoRotMatrix[0];
            m_PhotoRotMatrixInv[1] = m_PhotoRotMatrix[3];
            m_PhotoRotMatrixInv[2] = m_PhotoRotMatrix[6];

            m_PhotoRotMatrixInv[3] = m_PhotoRotMatrix[1];
            m_PhotoRotMatrixInv[4] = m_PhotoRotMatrix[4];
            m_PhotoRotMatrixInv[5] = m_PhotoRotMatrix[7];

            m_PhotoRotMatrixInv[6] = m_PhotoRotMatrix[2];
            m_PhotoRotMatrixInv[7] = m_PhotoRotMatrix[5];
            m_PhotoRotMatrixInv[8] = m_PhotoRotMatrix[8];

        }

        /*=== WrldToImg() ===*/
        public void WrldToImg(double X,
                              double Y,
                              double Z,

                              out double Xpix,
                              out double Ypix)
        {
            double Xmm, Ymm;

            WrldToImgMM(X,
                        Y,
                        Z,

                        out Xmm,
                        out Ymm);

            MMToPix(Xmm,
                    Ymm,

                    out Xpix,
                    out Ypix);
        }

        /*=== ImgToWrld_ByZ() ===*/
        public void ImgToWrld_ByZ(double Xpix,
                                  double Ypix,
                                  double Z,

                                  out double X,
                                  out double Y,
                                  out double GSD)
        {
            double Xmm, Ymm;
            double DistToSensor;
            double dx, dy, dz, DistToGround;

            PixToMM(Xpix,
                    Ypix,

                    out Xmm,
                    out Ymm);

            DoImgToWrld_ByZMM(Xmm,
                              Ymm,
                              Z,

                              out X,
                              out Y,
                              out DistToSensor);

            dx = m_PhotoProjCenter[0] - X;
            dy = m_PhotoProjCenter[1] - Y;
            dz = m_PhotoProjCenter[2] - Z;
            DistToGround = 100.0 * Math.Sqrt(dx * dx + dy * dy + dz * dz);
            GSD = (DistToGround / DistToSensor) * m_nSensorPixSizeInv;
        }

        #endregion Public

        /*-------------------------------------------------------------------*/
        /*-------------------------- Private --------------------------------*/
        /*-------------------------------------------------------------------*/
        #region Private

        /*=== PixToMM() ===*/
        private void PixToMM(double Xpix,
                             double Ypix,

                             out double Xmm,
                             out double Ymm)
        {
            Xmm = m_CamMatrixInv[0] * Xpix + m_CamMatrixInv[1] * Ypix + m_CamMatrixInv[2];
            Ymm = m_CamMatrixInv[3] * Xpix + m_CamMatrixInv[4] * Ypix + m_CamMatrixInv[5];
        }

        /*=== MMToPix() ===*/
        private void MMToPix(double Xmm,
                             double Ymm,

                             out double Xpix,
                             out double Ypix)
        {
            Xpix = m_CamMatrix[0] * Xmm + m_CamMatrix[1] * Ymm + m_CamMatrix[2];
            Ypix = m_CamMatrix[3] * Xmm + m_CamMatrix[4] * Ymm + m_CamMatrix[5];
        }

        /*=== WrldToImgMM() ===*/
        private void WrldToImgMM(double X,
                                 double Y,
                                 double Z,

                                 out double ImgX,
                                 out double ImgY)
        {
            double ImgZ;

            Rotate(X - m_PhotoProjCenter[0],
                   Y - m_PhotoProjCenter[1],
                   Z - m_PhotoProjCenter[2],
                   m_PhotoRotMatrix,

                   out ImgX,
                   out ImgY,
                   out ImgZ);

            ImgX /= -ImgZ;
            ImgY /= -ImgZ;

            ImgX *= m_nCamFocal;
            ImgY *= m_nCamFocal;
        }

        /*=== Rotate() ===*/
        private void Rotate(double x,
                            double y,
                            double z,
                            double[] RM,

                            out double xr,
                            out double yr,
                            out double zr)
        {
            xr = RM[0] * x + RM[1] * y + RM[2] * z;
            yr = RM[3] * x + RM[4] * y + RM[5] * z;
            zr = RM[6] * x + RM[7] * y + RM[8] * z;
        }

        /*=== DoImgToWrld_ByZMM() ===*/
        private void DoImgToWrld_ByZMM(double Xmm,
                                       double Ymm,
                                       double Z,

                                       out double X,
                                       out double Y,
                                       out double DistToSensor)
        {
            double x, y, z, d;
            double xr, yr, zr;
            double RayX0, RayY0, RayZ0;
            double RayX1, RayY1, RayZ1;
            double fact;

            x = Xmm;
            y = Ymm;
            z = -m_nCamFocal;
            d = Math.Sqrt(x * x + y * y + z * z);
            DistToSensor = d;
            x /= d;
            y /= d;
            z /= d;

            Rotate(x,
                   y,
                   z,
                   m_PhotoRotMatrixInv,

                   out xr,
                   out yr,
                   out zr);

            RayX0 = 0.0;
            RayY0 = 0.0;
            RayZ0 = 0.0;
            RayX1 = xr;
            RayY1 = yr;
            RayZ1 = zr;

            fact = (Z - m_PhotoProjCenter[2] - RayZ0) / (RayZ1 - RayZ0);
            X = RayX0 + fact * (RayX1 - RayX0);
            Y = RayY0 + fact * (RayY1 - RayY0);
            X += m_PhotoProjCenter[0];
            Y += m_PhotoProjCenter[1];
        }

        #endregion Private
    }
}
