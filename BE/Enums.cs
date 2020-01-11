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
            
            דרום,
            צפון,
            מרכז,
            ירושלים
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
            תל_אביב,
            טבריה,
            צפת,
            חיפה,
            ירושלים,
            אשקלון,
            אשדוד,
            חדרה,
            באר_שבע,
            אילת
        }
         
        public enum DataSourseType
        {
            List,
            XML
        }
    }


}