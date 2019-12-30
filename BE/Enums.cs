using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class Enums
    {
        public enum HostingUnitType
        {
            צימר,
            אכסניה,
            בית_הארחה,
            בית_מלון,
            וילה,
            דירה,
            מאהל
        }
        public enum GuestRequestStatus
        {
            Active,
            NotActive
        }
        public enum Regions
        {
            
            South,
            North,
            Center,
            Jerusalem
        }
        public enum guestRequestStatus
        {
            open,
            closed,
            expired
        }
        public enum OrderStatus
        {
            NotMailed,
            Mailed,
            Canceled,
            Closed
        }
        public enum SubArea
        {
            Tel_Aviv,
            Tveria,
            Tzfat,
            Haifa,
            Jerusalem,
            Askelon,
            Asdod,
            Hadera,
            Beer_sheva,
            Eilat
        }
         
        public enum DataSourseType
        {
            List,
            XML
        }
    }


}