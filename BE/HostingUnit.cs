using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;


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
        private bool childrensAttractions;
        private double fee;
        private int yearlyOccupied;


        //private string review;
        //facilities


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
        public bool[,] Diary { get => diary; set => diary = value; } 
        public string HostingUnitType { get => hostingUnitType; set => hostingUnitType = value; }
        public string Area { get => area; set => area = value; }
        public string SubArea { get => subArea; set => subArea = value; }
        public double Fee { get => fee; set => fee = value; }
        public int YearlyOccupied { get => yearlyOccupied; set => yearlyOccupied = value; }

        public HostingUnit() 
        {
            HostingUnitKey = Configuration.HostingUnitKey++.ToString();
            Diary = new bool[12, 31];
            Host = new Host();
        }
         

        public override string ToString()
        {
            return ",  מספר זיהוי:"+HostingUnitKey + ",  שם :" + HostingUnitName +
                " ,סוג האירוח:" + HostingUnitType.ToString() + ",  פרטי בעלים:" + Host ;
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
