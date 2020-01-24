using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;



namespace BE
{
    [Serializable]
    public class HostingUnit
    {
        private string hostingUnitKey;
        private Host host;
        private string hostingUnitName;
        private string hostingUnitType;
        private bool[,] diary;
        private string area;
        private string subArea;
        private double ratingPercentage;
        private bool pool;
        private bool jacuzzi;
        private bool garden;
        private bool parking;
        private bool baby_bed;
        private double fee;
        private int yearlyOccupied;




        public string HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey = value; }
        public string HostingUnitName { get => hostingUnitName;
            set
            {
                Regex r = new Regex("^[ a-zA-Zא-ת]{2,15}$");
                if (!r.IsMatch(value))
                    throw new Exception("Name should contain only letters, Between 2-15.");
                hostingUnitName = value;
            }
        }
        public Host Host
        {
            get => host;
            set
            {
                try { host = value; }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        [XmlIgnore]
        public bool[,] Diary { get => diary; set => diary = value; }

        public string HostingUnitType { get => hostingUnitType; set => hostingUnitType = value; }
        public string Area { get => area; set => area = value; }
        public string SubArea { get => subArea; set => subArea = value; }
        public double Fee { get => fee; set
            {
                fee = value;
                Host.Fee += value;
            }
        }
        public int YearlyOccupied { get => yearlyOccupied; set => yearlyOccupied = value; }
        public bool Pool { get => pool; set => pool = value; }
        public bool Jacuzzi { get => jacuzzi; set => jacuzzi = value; }
        public bool Garden { get => garden; set => garden = value; }
        public bool Parking { get => parking; set => parking = value; }
        public bool Baby_bed { get => baby_bed; set => baby_bed = value; }

        [XmlArray("Diary")]
        public bool [] DairySer
       {
            get
            {
                return Diary.Flatten();
            }
            set
            {
                Diary = value.Expand(12);
            }
      }

        public HostingUnit() 
        {
            HostingUnitKey = Configuration.HostingUnitKey++.ToString();
            Diary = new bool[12, 31];
            Host = new Host();
        }
         

        public override string ToString()
        {
            return "\tמספר זיהוי:"+HostingUnitKey + "\tשם :" + HostingUnitName +
                "\tסוג האירוח:" + HostingUnitType.ToString() + "\tפרטי בעלים:" + Host ;
        }
        

        public bool this[DateTime date]
        {
            get
            {
                return Diary[date.Month - 1, date.Day - 1];
            }
            set
            {
                Diary[date.Month - 1, date.Day - 1] = value;
            }
        }
       
    }
}
