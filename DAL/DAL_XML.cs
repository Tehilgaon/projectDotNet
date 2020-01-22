using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;
using System.Xml.Serialization;
using System.Xml.Linq;
using System.IO;


namespace DAL
{
    class DAL_XML : IDAL
    {

        #region Singleton
        private static readonly DAL_XML instance = new DAL_XML();
        public static DAL_XML Instance
        {
            get { return instance; }
        }

        

        private DAL_XML()
        {
          
            guestRequestsList = tool.LoadFromXML<List<GuestRequest>>(guestRequestsPath);
            hostingUnitList = tool.LoadFromXML<List<HostingUnit>>(hostingUnitsPath);
            orderList = tool.LoadFromXML<List<Order>>(ordersPath);
        }
        static DAL_XML() { }

        #endregion


        //static readonly string ProjectPath = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory.ToString()).FullName).FullName;
        XElement configRoot;
        private readonly string guestRequestsPath =  "guestRequests.xml";
        private readonly string hostingUnitsPath =   "hostingUnits.xml";
        private readonly string ordersPath =  "orders.xml";
        private readonly string configPath =  "config.xml";
        XElement guestRequestRoot;
        List<GuestRequest> guestRequestsList = new List<GuestRequest>();
        List<HostingUnit> hostingUnitList = new List<HostingUnit>();
        List<Order> orderList = new List<Order>();
  
         

        #region HostingUnit
        public void addHostingUnit(HostingUnit hostingUnit)
        {
            hostingUnitList.Add(hostingUnit.Clone());
            tool.SaveToXML<List<HostingUnit>>(hostingUnitList, hostingUnitsPath);
        }
        public List<HostingUnit> getAllHostingUnits(Func<HostingUnit, bool> predicate = null)
        {
            if (predicate != null)
                return hostingUnitList.Where(predicate).ToList().Clone();
            HostingUnit[] hostingUnitArr = new HostingUnit[hostingUnitList.Count];
            hostingUnitList.CopyTo(hostingUnitArr);
            return hostingUnitArr.ToList();
        }
        public void deleteHostingUnit(HostingUnit hostingUnit)
        {
            HostingUnit Unit = hostingUnitList.Where(Item => Item.HostingUnitKey == hostingUnit.HostingUnitKey).FirstOrDefault();
            hostingUnitList.Remove(Unit);
            tool.SaveToXML<List<HostingUnit>>(hostingUnitList, hostingUnitsPath); 
        }
        public void updateHostingUnit(HostingUnit hostingUnit)
        {
            int index = hostingUnitList.FindIndex(Item => Item.HostingUnitKey == hostingUnit.HostingUnitKey);
            hostingUnitList[index] = hostingUnit.Clone();
            tool.SaveToXML<List<HostingUnit>>(hostingUnitList, hostingUnitsPath);
        }
        #endregion

        #region Order
        public void addOrder(Order order)
        {
            orderList.Add(order.Clone());
            tool.SaveToXML<List<Order>>(orderList, ordersPath);
        }
        public List<Order> getAllOrders(Func<Order, bool> predicate = null)
        {
            if (predicate != null)
                return orderList.Where(predicate).ToList().Clone();
            Order[] OrderArr = new Order[orderList.Count];
            orderList.CopyTo(OrderArr);
            return OrderArr.ToList();

        }
        public void updateOrder(Order order)
        {
            int index = DataSource.orders.FindIndex(Item => Item.OrderKey == order.OrderKey);
            orderList[index] = order.Clone();
            tool.SaveToXML<List<Order>>(orderList, ordersPath);
        }

        #endregion

        #region GuestRequest
        public void addGuestRequest(GuestRequest guestRequest)
        { 
           guestRequestsList.Add(guestRequest.Clone());
           tool.SaveToXML<List<GuestRequest>>(guestRequestsList, guestRequestsPath);    
        }
        public void updateGuestRequest(GuestRequest guestRequest)
        {
            guestRequestsList.RemoveAll(Item => Item.GuestRequestKey == guestRequest.GuestRequestKey);
            tool.SaveToXML<List<GuestRequest>>(guestRequestsList, guestRequestsPath);
            addGuestRequest(guestRequest);   
        }
        
        public List<GuestRequest> GetAllGuestRequests(Func<GuestRequest, bool> predicate = null)
        {
            if (predicate != null)
                return guestRequestsList.Where(predicate).ToList().Clone();
            return guestRequestsList.Select(Item => Item).ToList().Clone(); 
        }

        #endregion

        public List<BankBranch> GetAllBranches()
        {
            return DataSource.GetAllBranches();
        }



         

    }
}
