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
             
            guestRequestsList = LoadFromXML<List<GuestRequest>>(guestRequestsPath);
            
            orderList = LoadFromXML<List<Order>>(ordersPath);
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
        List<HostingUnit> hostingUnitList;
        List<Order> orderList = new List<Order>();
         

        #region HostingUnit
        public void addHostingUnit(HostingUnit hostingUnit)
        {
            DataSource.hostingUnits.Add(hostingUnit.Clone());
        }
        public List<HostingUnit> getAllHostingUnits(Func<HostingUnit, bool> predicate = null)
        {
            if (predicate != null)
                return DataSource.hostingUnits.Where(predicate).ToList().Clone();
            HostingUnit[] hostingUnitArr = new HostingUnit[DataSource.hostingUnits.Count];
            DataSource.hostingUnits.CopyTo(hostingUnitArr);
            return hostingUnitArr.ToList();
        }
        public void deleteHostingUnit(HostingUnit hostingUnit)
        {
            HostingUnit Unit = DataSource.hostingUnits.Where(Item => Item.HostingUnitKey == hostingUnit.HostingUnitKey).FirstOrDefault();
            DataSource.hostingUnits.Remove(Unit);
        }
        public void updateHostingUnit(HostingUnit hostingUnit)
        {
            int index = DataSource.hostingUnits.FindIndex(Item => Item.HostingUnitKey == hostingUnit.HostingUnitKey);
            DataSource.hostingUnits[index] = hostingUnit.Clone();
        }
        #endregion

        #region Order
        public void addOrder(Order order)
        {
            orderList.Add(order.Clone());
            SaveToXML<List<Order>>(orderList, ordersPath);
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
            SaveToXML<List<Order>>(orderList, ordersPath);
        }

        #endregion

        #region GuestRequest
        public void addGuestRequest(GuestRequest guestRequest)
        { 
           guestRequestsList.Add(guestRequest.Clone());
           SaveToXML<List<GuestRequest>>(guestRequestsList, guestRequestsPath);    
        }
        public void updateGuestRequest(GuestRequest guestRequest)
        {
            guestRequestsList.RemoveAll(Item => Item.GuestRequestKey == guestRequest.GuestRequestKey);
            SaveToXML<List<GuestRequest>>(guestRequestsList, guestRequestsPath);
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




        public static void SaveToXML<T>(T source, string path)
        {
            FileStream file = new FileStream(path, FileMode.Create);
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            xmlSerializer.Serialize(file, source); file.Close();
        }

        /// <summary>
        /// Load From XML tamplate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns></returns>
        public static T LoadFromXML<T>(string path)
        {
            FileStream file = new FileStream(path, FileMode.Open);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
            T result = (T)xmlSerializer.Deserialize(file);
            file.Close();
            return result;
        }


    }
}
