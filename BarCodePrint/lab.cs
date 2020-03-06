using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;

namespace BarCodePrint
{
    [Serializable]
    public class lab
    {
        public List<survey> surveys;

        public lab()
        {
            surveys = new List<survey>();
        }

        public int AddSurvey(string name, int prefix, int max)
        {
            foreach (survey sur in this.surveys)
            {
                if (sur.name == name) return 0; //Такое имя уже есть
            }

            survey LocSurvey = new survey();
            {
                LocSurvey.name = name;
                LocSurvey.prefix = prefix;
                LocSurvey.position = 0;
                LocSurvey.max = max;
            }
            this.surveys.Add(LocSurvey);
            return 1; //все штатно
        }

        public List<string> GetNameSurvey()
        {
            List<string> list = new List<string>();
            foreach (survey sur in this.surveys)
            {
                list.Add(sur.name);
            }
            return list;
        }

        public survey GetSurvey(string name)
        {
            foreach (survey sur in this.surveys)
            {
                if (sur.name == name) return sur;
            }
            return null;
        }

        public void SaveToXML(string filename)
        {
            XmlSerializer xml = new XmlSerializer(this.GetType());

            FileStream f = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.Read);
            xml.Serialize(f, this);
            f.Close();
        }

        public lab LoadFromXML(string filename)
        {
            lab clab = new lab();
            try
            {
                XmlSerializer xml = new XmlSerializer(typeof(lab));
                FileStream f = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
                clab = xml.Deserialize(f) as lab;
                f.Close();
            }
            catch
            {
                MessageBox.Show("Не найден конфиг файл, будет создан новый!");          
            }
            return clab;
        }
    }

    [Serializable]
    public class survey
    {
        public string name;
        public int prefix;
        public int position;
        public int max;
    }
}
