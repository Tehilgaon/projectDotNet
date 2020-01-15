using BE;
using DAL;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Data;  

namespace BL
{
    public class MyBL: IBL
    {
        static IDAL myDAL;

        #region Singleton
        private static readonly MyBL instance = new MyBL();

        public static MyBL Instance
        {
            get { return instance; }
        }

        static MyBL()
        {
            // string TypeDAL = ConfigurationSettings.AppSettings.Get("TypeDS");
            string TypeDAL = Configuration.TypeDAL;
            // string TypeDAL = "List";
            myDAL = factoryDAL.getDAL(TypeDAL);
        }
        private MyBL() { }
        #endregion

        #region HostingUnit
        public void addHostingUnit(HostingUnit hostingUnit)
        {
            if (hostingUnit.HostingUnitName == null ||hostingUnit.HostingUnitType == null)//||
                //hostingUnit.Host.PrivateName==null)//||hostingUnit.Host.MailAddress==null)
                throw new Exception("חובה למלא את כל הפרטים");
            myDAL.addHostingUnit(hostingUnit);
        }
        
        public List<HostingUnit> getAllHostingUnits(Func<HostingUnit, bool> p=null)
        {
            return myDAL.getAllHostingUnits(p);
        }

        public void updateHostingUnit(HostingUnit hostingUnit)
        {
            HostingUnit oldHostingUnit = getAllHostingUnits(Item => Item.HostingUnitKey == hostingUnit.HostingUnitKey).FirstOrDefault();
            if (hostingUnit.Host.CollectionClearance==false && oldHostingUnit.Host.CollectionClearance==true &&
                getAllOrders(Item => Item.HostingUnitKey == hostingUnit.HostingUnitKey && Item.OrderStatus == Enums.OrderStatus.Mailed).Count!=0)
                throw new Exception("לא ניתן לבטל הרשאה לחיוב חשבון בנק בשל הזמנות פתוחות");  
            myDAL.updateHostingUnit(hostingUnit);
        }

        public void deleteHostingUnit(HostingUnit hostingUnit)
        { 
            if (getAllOrders(Item => Item.HostingUnitKey == hostingUnit.HostingUnitKey  &&
            (Item.OrderStatus == Enums.OrderStatus.Mailed || Item.OrderStatus == Enums.OrderStatus.NotMailed)).Count!=0)
                throw new Exception("לא ניתן למחוק יחידת אירוח בשל הזמנות פתוחות");
            myDAL.deleteHostingUnit(hostingUnit);
        }
        #endregion

        #region GuestRequest
        public void addGuestRequest(GuestRequest guestRequest)
        {   if (guestRequest.PrivateName == null || guestRequest.FamilyName == null || guestRequest.MailAddress == null
                || guestRequest.EntryDate == default || guestRequest.ReleaseDate == default || guestRequest.Area == null)
                throw new Exception("חובה למלא את כל הפרטים"); 
            if((guestRequest.ReleaseDate-guestRequest.EntryDate).Days<1) 
                throw new Exception("לא ניתן לבצע הזמנה לפחות מיום אחד");
            myDAL.addGuestRequest(guestRequest);
        }

        public void updateGuestRequest(GuestRequest guestRequest)
        {
            myDAL.updateGuestRequest(guestRequest);
        }

        public List<GuestRequest> GetAllGuestRequests(Func<GuestRequest, bool> predicate=null)
        {
            return myDAL.GetAllGuestRequests(predicate);
        }
        #endregion

        #region Order
        public void addOrder(Order order)
        {
            HostingUnit hostingUnit = getAllHostingUnits(Item => Item.HostingUnitKey == order.HostingUnitKey).FirstOrDefault();
            GuestRequest guestRequest = GetAllGuestRequests(Item => Item.GuestRequestKey == order.GuestRequestKey).FirstOrDefault();
            if (hostingUnit == null) throw new KeyNotFoundException("לא קיימת יחידת אירוח עבור הזמנה זו");
            if (guestRequest == null) throw new KeyNotFoundException("לא קיימת דרישת לקוח עבור הזמנה זו"); 
            if (getAllOrders(Item=>Item.OrderKey==order.OrderKey).Count!=0)
                throw new InvalidExpressionException( "הזמנה כבר קיימת במערכת");
            if(ifAvailable(hostingUnit,guestRequest.EntryDate,guestRequest.ReleaseDate)==null)
                throw new Exception("התאריכים המבוקשים אינם זמינים");  
            myDAL.addOrder(order);
        }
         

        public List<Order> getAllOrders(Func<Order, bool> predicate=null)
        {
            return myDAL.getAllOrders(predicate);
        }
        public void updateOrder(Order order)
        {
            Order oldOrder  = getAllOrders(Item => Item.OrderKey == order.OrderKey).FirstOrDefault();
            HostingUnit hostingUnit = getAllHostingUnits(Item => Item.HostingUnitKey == order.HostingUnitKey).FirstOrDefault();
            GuestRequest guestRequest = GetAllGuestRequests(Item => Item.GuestRequestKey == order.GuestRequestKey).FirstOrDefault();
            if (hostingUnit == null || guestRequest == null || oldOrder == null || oldOrder.OrderStatus == Enums.OrderStatus.Canceled ||
                oldOrder.OrderStatus == Enums.OrderStatus.Mailed && order.OrderStatus == Enums.OrderStatus.NotMailed ||
                oldOrder.OrderStatus == Enums.OrderStatus.NotMailed && order.OrderStatus == Enums.OrderStatus.Closed)
                throw new InvalidExpressionException("פרטי הזמנה אינם נכונים");
            if (oldOrder.OrderStatus == order.OrderStatus)  return;
            if (oldOrder.OrderStatus == Enums.OrderStatus.Closed)
                throw new Exception(" לא ניתן לשנות הזמנה שנסגרה"); 
            
            if (oldOrder.OrderStatus == Enums.OrderStatus.NotMailed&&order.OrderStatus==Enums.OrderStatus.Mailed)
            {
                if (hostingUnit.Host.CollectionClearance==false)
                    throw new Exception("חובה לחתום על הרשאה לחיוב חשבון בנק לפני שליחת מייל ללקוח");
                 //Sending an email
            }
            if (oldOrder.OrderStatus==Enums.OrderStatus.Mailed&&order.OrderStatus==Enums.OrderStatus.Closed)
            {
                hostingUnit.Fee+= DaysBetween(guestRequest.EntryDate, guestRequest.ReleaseDate) * Configuration.Fee;  
                hostingUnit = updateDairy(hostingUnit, guestRequest);
                updateHostingUnit(hostingUnit);
                guestRequest.Status = Enums.GuestRequestStatus.NotActive.ToString();
                updateGuestRequest(guestRequest);
                var orders = getAllOrders(Item => Item.GuestRequestKey == guestRequest.GuestRequestKey&& Item.OrderKey!=order.OrderKey).ToList();
                foreach (var Item in orders)
                {
                    Item.OrderStatus = Enums.OrderStatus.Canceled;
                    updateOrder(Item);
                }    
            } 
            myDAL.updateOrder(order);
        }
        #endregion


        public List<BankBranch> GetAllBranches()
        {
            return myDAL.GetAllBranches();
        }
        public List<HostingUnit> AllAvailable(DateTime FirstDate, int NumOfDays)
        {
            return getAllHostingUnits().Where(hostingUnit => 
            ifAvailable(hostingUnit, FirstDate, FirstDate.AddDays(NumOfDays)) != null).ToList().Clone();
        }
        public HostingUnit ifAvailable(HostingUnit hostingUnit, DateTime EntryDate, DateTime ReleaseDate )
        {
            DateTime day = EntryDate;
            while (day.Day != ReleaseDate.Day)
            {
                if (hostingUnit[day])
                    return null;
                day = day.AddDays(1);
            }
            return hostingUnit;
        }
        public int DaysBetween(DateTime D1,DateTime D2=default)
        {
            if (D2 == default)
                return (DateTime.Now - D1).Days;
            return (D2 - D1).Days;
        }
        public List<Order> AllOrdersSince(TimeSpan Time)
        {
            var orders = from order in getAllOrders()
                         where (DateTime.Now - order.OrderDate >= Time)
                         select order;
            return orders.ToList();
        }
        public int AllOrdersOfGuestRequest(string guestRequestKey,Enums.OrderStatus status)
        {
            return getAllOrders(Item => Item.GuestRequestKey == guestRequestKey&& Item.OrderStatus == status).Count;
        }
        public int AllOrdersOfHostingUnit(string hostingUnitKey, Enums.OrderStatus status)
        {
            return getAllOrders(Item => Item.HostingUnitKey == hostingUnitKey && Item.OrderStatus == status).Count;
        }
        public List<IGrouping<string ,GuestRequest>> GroupGuestRequestByRegion()
        {
            return (from guestRequest in GetAllGuestRequests()
                                    group guestRequest by guestRequest.Area).ToList();
        }
        public IGrouping<int, GuestRequest> GroupGuestRequestByNumOfGuests()
        {
            return (IGrouping<int, GuestRequest>) from guestRequest in GetAllGuestRequests()
                    group guestRequest by guestRequest.Children + guestRequest.Adults;
        }
        public  IGrouping<int, Host> GroupHostByNumOfHostingUnit()
        {             
            List<Host> hosts = new List<Host>();
            foreach (var Item in getAllHostingUnits())
                hosts.Add(Item.Host);
            hosts.Distinct();
            return (IGrouping<int, Host>)hosts.GroupBy(Item => getAllHostingUnits(x => x.Host == Item).Count);
        }  
        public IGrouping<Enums.Regions, HostingUnit> GroupHostingUnitByRegion()
        {
            return (IGrouping <Enums.Regions, HostingUnit >) getAllHostingUnits().GroupBy(hostingUnit => hostingUnit.Area);
        }
        public HostingUnit updateDairy(HostingUnit hostingUnit,GuestRequest guestRequest)
        {
            DateTime date = guestRequest.EntryDate;
            while(date.Day!= guestRequest.ReleaseDate.Day)
            {
                hostingUnit[date] = true;
                /*if (date.Year==DateTime.Today.Year)
                    hostingUnit.YearlyOccupied++;
                else
                    hostingUnit.YearlyOccupied = 1;*/
                date = date.AddDays(1);
            }
            return hostingUnit;
        }
         
        


    }
}
