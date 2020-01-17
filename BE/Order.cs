using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE
{
    [Serializable]
    public class Order
    {
        private string hostingUnitKey;
        private string guestRequestKey;
        private string orderKey;
        private Enums.OrderStatus orderStatus;
        private DateTime createDate;
        private DateTime orderDate;
        private string guestMail;

        public string HostingUnitKey { get => hostingUnitKey; set => hostingUnitKey = value; }
        public string GuestRequestKey { get => guestRequestKey; set => guestRequestKey = value; }
        public string OrderKey { get => orderKey; }
        public DateTime CreateDate { get => createDate; }
        public DateTime OrderDate { get => orderDate ; set => orderDate = value; }
        public Enums.OrderStatus OrderStatus { get => orderStatus; set => orderStatus = value; }
        public string GuestMail { get => guestMail; set => guestMail = value; }

        public override string ToString()
        {
            return ",  מספר הזמנה:  " + OrderKey + ",  מספר יחידת אירוח:  " + HostingUnitKey + ",  מספר דרישת לקוח:  " + GuestRequestKey +
                ",  סטטוס הזמנה:  " + OrderStatus.ToString() + ",  תאריך הזמנה:  " + orderDate.ToString("MM/dd/yyyy ");
        }
        public Order() 
        {
            orderKey = Configuration.OrderKey++.ToString();
            createDate = DateTime.Now;
            orderStatus = Enums.OrderStatus.NotMailed;
        }
         
    }
}
