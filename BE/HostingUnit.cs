using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 

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

        //private string review;
        //facilities


        public string HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey = value; }
        public string HostingUnitName { get => hostingUnitName; set => hostingUnitName = value; }   
        public Host Host { get => host; set => host = value; }
        public bool[,] Diary { get => diary; set => diary = value; } 
        public string HostingUnitType { get => hostingUnitType; set => hostingUnitType = value; }
        public string Area { get => area; set => area = value; }

        

        public HostingUnit() 
        {
            HostingUnitKey = Configuration.HostingUnitKey++.ToString();
            Diary = new bool[12, 31];
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
