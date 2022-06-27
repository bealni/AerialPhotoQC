using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Newtonsoft.Json;

namespace AerialPhotoQC
{
    public class Cfg
    {
        public Cfg_Data m_Data;

        public String m_sFileName;

        /*=== Cfg() ===*/
        public Cfg()
        {
            m_Data = null;

            Close();
        }

        /*=== Open() ===*/
        public String Open(String FileName, bool IsNew)
        {
            Close();

            try
            {
                if (IsNew)
                {
                    m_Data = new Cfg_Data();
                    m_Data.OpenNew();
                    String json = JsonConvert.SerializeObject(m_Data, Formatting.Indented);
                    StreamWriter SW = new System.IO.StreamWriter(FileName);
                    SW.Write(json);
                    SW.Close();
                    SW.Dispose();
                }
                else
                {
                    StreamReader SR = new StreamReader(FileName);
                    String json = SR.ReadToEnd();
                    SR.Close();
                    SR = null;
                    m_Data = JsonConvert.DeserializeObject<Cfg_Data>(json);
                }
            }
            catch (Exception ex)
            {
                Close();
                return "Could not open\n\"" +
                       FileName +
                       "\".\nException:\n" + ex.Message;
            }

            m_sFileName = FileName;

            return "OK";
        }

        /*=== Write() ===*/
        public String Write()
        {
            if (m_Data == null)
                return "Cfg.Write(): Cfg object is not open.";

            try
            {
                String json = JsonConvert.SerializeObject(m_Data, Formatting.Indented);
                StreamWriter SW = new System.IO.StreamWriter(m_sFileName);
                SW.Write(json);
                SW.Close();
                SW.Dispose();
            }
            catch (Exception ex)
            {
                Close();
                return "Could not write\n\"" +
                       m_sFileName +
                       "\".\nException:\n" + ex.Message;
            }

            return "OK";
        }

        /*=== Close() ===*/
        public void Close()
        {
            if (m_Data != null)
            {
                m_Data.Close();
                m_Data = null;
            }

            m_sFileName = "";
        }
    }
}
