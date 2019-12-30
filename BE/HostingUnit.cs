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
        private Enums.HostingUnitType hostingUnitType;
        private bool[,] diary;
        private Enums.Regions area;
        private string address;
        private double ratingPercentage;
        private string review;
        //facilities


        public string HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey = value; }
        public string HostingUnitName { get => hostingUnitName; set => hostingUnitName = value; }   
        public Host Host { get => host; set => host = value; }
        public bool[,] Diary { get => diary; set => diary = value; } 
        public Enums.HostingUnitType HostingUnitType { get => hostingUnitType; set => hostingUnitType = value; }
        public Enums.Regions Area { get => area; set => area = value; }

        

        public HostingUnit() 
        {
            HostingUnitKey = Configuration.HostingUnitKey++.ToString();
            Diary = new bool[12, 31];
        }
         

        public override string ToString()
        {
            return this.hostingUnitName + " " + HostingUnitType+" "+ HostingUnitKey;
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
