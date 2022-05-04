using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace AerialPhotoQC
{
    public class PRJInfo
    {
        /*-------------------------------------------------------------------*/
        /*-------------------------- Constants ------------------------------*/
        /*-------------------------------------------------------------------*/
        #region Constants

        // Error Codes
        public const int ERR_NONE = 0;
        public const int ERR_CANTREAD = 1;
        public const int ERR_INVALIDFORMAT = 2;
        public const int ERR_NO_CAMERA_FOUND = 3;
        public const int ERR_CAMERA_INFO_INCONSISTENCY = 4;

        // Tags
        private const String CAMERA_START = "$CAMERA";
        private const String CAMERA_TYPE = "$TYPE";
        private const String CAMERA_CCD_ORIENT = "$CCD_INTERIOR_ORIENTATION";
        private const String CAMERA_CCD_COLUMNS = "$CCD_COLUMNS";
        private const String CAMERA_CCD_ROWS = "$CCD_ROWS";
        private const String CAMERA_FOCAL_LENGTH = "$FOCAL_LENGTH";

        #endregion Constants

        /*-------------------------------------------------------------------*/
        /*-------------------------- Variables ------------------------------*/
        /*-------------------------------------------------------------------*/
        #region Variables

        // Cameras
        public String m_sCamID;
        public double[] m_CamMatrix;
        public double[] m_CamMatrixInv;
        public int m_nCamCols;
        public int m_nCamRows;
        public double m_nCamFocal;

        // Error reporting
        public String m_sExceptionStr;
        public int m_nErrLineNo;

        // Is Open
        public bool m_bIsOpen;

        #endregion Variables

        /*-------------------------------------------------------------------*/
        /*-------------------------- Public ---------------------------------*/
        /*-------------------------------------------------------------------*/
        #region Public

        /*=== PRJInfo() ===*/
        public PRJInfo()
        {
            Close();
        }

        /*=== Open() ===*/
        public int Open(String FileName)
        {
            return DoOpen(FileName);
        }

        /*=== Close() ===*/
        public void Close()
        {
            m_sCamID = "";
            m_CamMatrix = null;
            m_CamMatrixInv = null;
            m_nCamCols = 0;
            m_nCamRows = 0;
            m_nCamFocal = 0.0;

            // Is Open
            m_bIsOpen = false;

            // Error reporting
            m_sExceptionStr = "";
            m_nErrLineNo = 0;
        }

        /*=== GetErrStr() ===*/
        public String GetErrStr(int Err)
        {
            switch (Err)
            {
                case ERR_NONE:
                    return "OK";

                case ERR_CANTREAD:
                    return "Could not read input file.";
                case ERR_INVALIDFORMAT:
                    return "Input file has invalid format.";
                case ERR_NO_CAMERA_FOUND:
                    return "No Camera found.";
                case ERR_CAMERA_INFO_INCONSISTENCY:
                    return "Inconsistent Camera information.";

                default:
                    return "Unknown Error.";
            }
        }

        #endregion Public

        /*-------------------------------------------------------------------*/
        /*-------------------------- Private --------------------------------*/
        /*-------------------------------------------------------------------*/
        #region Private

        /*-------------------------------------------------------------------*/
        /*-------------------------- Private Open ---------------------------*/
        /*-------------------------------------------------------------------*/
        #region Private Open

        /*=== DoOpen() ===*/
        private int DoOpen(String FileName)
        {
            int Err;

            Close();

            Err = DoOpen_ReadCamera(FileName);
            if (Err != ERR_NONE)
                return Err;

            m_nErrLineNo = 0;
            m_bIsOpen = true;

            return ERR_NONE;
        }

        /*-------------------------------------------------------------------*/
        /*-------------------------- Private Open Camera --------------------*/
        /*-------------------------------------------------------------------*/
        #region Private Open Camera

        /*=== DoOpen_ReadCamera() ===*/
        private int DoOpen_ReadCamera(String FileName)
        {
            StreamReader SR;
            String S;
            bool Within, TypeFound, CCDOrientFound, CCDColsFound, CCDRawsFound, FocalFound;
            char[] Sep = new char[] { ' ', '\t' };
            string[] s;
            double m1, m2, m3, m4, m5, m6;
            int IVal;
            double DVal;

            try
            {
                SR = new StreamReader(FileName);

                Within = false;
                TypeFound = false;
                CCDOrientFound = false;
                CCDColsFound = false;
                CCDRawsFound = false;
                FocalFound = false;
                m_nErrLineNo = 0;
                while ((S = SR.ReadLine()) != null)
                {
                    ++m_nErrLineNo;
                    S = S.Trim();

                    if (!Within)
                    {
                        if (S == CAMERA_START)
                            Within = true;
                        continue;
                    }

                    if (S.StartsWith(CAMERA_TYPE))
                    {
                        s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                        if (s.Length != 3)
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }
                        TypeFound = true;
                        m_sCamID = s[2];
                        continue;
                    }

                    if (S.StartsWith(CAMERA_CCD_ORIENT))
                    {
                        if (!TypeFound)
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }

                        S = SR.ReadLine();
                        ++m_nErrLineNo;
                        s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                        if (s.Length != 3)
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }
                        if (!double.TryParse(s[0], out m1) ||
                            !double.TryParse(s[1], out m2) ||
                            !double.TryParse(s[2], out m3))
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }

                        S = SR.ReadLine();
                        ++m_nErrLineNo;
                        s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                        if (s.Length != 3)
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }
                        if (!double.TryParse(s[0], out m4) ||
                            !double.TryParse(s[1], out m5) ||
                            !double.TryParse(s[2], out m6))
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }

                        m_CamMatrix = new double[6] { m1, m2, m3, m4, m5, m6 };
                        m_CamMatrixInv = new double[6] { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 };
                        InverseAffineMatrix(m_CamMatrix,
                                            m_CamMatrixInv);                                                             
                        CCDOrientFound = true;
                        continue;
                    }

                    if (S.StartsWith(CAMERA_CCD_COLUMNS))
                    {
                        if (!CCDOrientFound)
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }
                        s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                        if (s.Length != 3)
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }
                        if (!int.TryParse(s[2], out IVal))
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }

                        CCDColsFound = true;                        
                        m_nCamCols = IVal;
                        continue;
                    }

                    if (S.StartsWith(CAMERA_CCD_ROWS))
                    {
                        if (!CCDColsFound)
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }
                        s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                        if (s.Length != 3)
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }
                        if (!int.TryParse(s[2], out IVal))
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }

                        CCDRawsFound = true;
                        m_nCamRows = IVal;
                        continue;
                    }

                    if (S.StartsWith(CAMERA_FOCAL_LENGTH))
                    {
                        if (!CCDRawsFound)
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }
                        s = S.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
                        if (s.Length != 3)
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }
                        if (!double.TryParse(s[2], out DVal))
                        {
                            SR.Close();
                            SR.Dispose();
                            SR = null;
                            return ERR_INVALIDFORMAT;
                        }

                        FocalFound = true;
                        m_nCamFocal = DVal;
                        break;
                    }
                }

                SR.Close();
                SR.Dispose();
                SR = null;
            }
            catch (Exception ex)
            {
                m_sExceptionStr = ex.Message;
                return ERR_CANTREAD;
            }

            if (!FocalFound)

            if (m_sCamID == "")
            {
                return ERR_NO_CAMERA_FOUND;
            }
            
            return ERR_NONE;
        }

        /*=== InverseAffineMatrix() ===*/
        private void InverseAffineMatrix(double[] Dir,
                                         double[] Inv)
        {
            double mult;

            mult = 1.0 / (Dir[0] * Dir[4] - Dir[1] * Dir[3]);

            Inv[0] = Dir[4] * mult;
            Inv[1] = -Dir[1] * mult;
            Inv[2] = (-Dir[2] * Dir[4] + Dir[1] * Dir[5]) * mult;

            Inv[3] = -Dir[3] * mult;
            Inv[4] = Dir[0] * mult;
            Inv[5] = (Dir[2] * Dir[3] - Dir[0] * Dir[5]) * mult;
        }

        #endregion Private Open Cameras

        #endregion Private Open

        #endregion Private
    }
}
