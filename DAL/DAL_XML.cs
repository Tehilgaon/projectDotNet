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
            if (!File.Exists(hostingUnitsPath))
                tool.SaveToXML<List<HostingUnit>>(hostingUnitList, hostingUnitsPath);
            if (!File.Exists(guestRequestsPath))
                tool.SaveToXML<List<GuestRequest>>(guestRequestsList, guestRequestsPath);
            if (!File.Exists(ordersPath))
                tool.SaveToXML<List<Order>>(orderList, ordersPath);


            guestRequestsList = tool.LoadFromXML<List<GuestRequest>>(guestRequestsPath);
            hostingUnitList = tool.LoadFromXML<List<HostingUnit>>(hostingUnitsPath);
            orderList = tool.LoadFromXML<List<Order>>(ordersPath);
            if(!File.Exists(configPath))
            {
                SaveConfigurationToXML(); 
            }
            else
            {
                configRoot = XElement.Load(configPath);
                BE.Configuration.GuestRequestKey = Convert.ToInt64(configRoot.Element("GuestRequestKey").Value);
                BE.Configuration.HostingUnitKey = Convert.ToInt64(configRoot.Element("HostingUnitKey").Value);
                BE.Configuration.OrderKey = Convert.ToInt64(configRoot.Element("OrderKey").Value);
                BE.Configuration.Mng = configRoot.Element("Mng").Value;
                BE.Configuration.Fee = Convert.ToInt32(configRoot.Element("Fee").Value);
                BE.Configuration.OrderValidity = Convert.ToInt32(configRoot.Element("OrderValidity").Value);
                BE.Configuration.SMTP_Server =  configRoot.Element("SMTP_Server").Value;
                BE.Configuration.MailSystem =  configRoot.Element("MailSystem").Value;
                BE.Configuration.Password =  configRoot.Element("Password").Value;
 
            }
        }
        static DAL_XML() { }

        #endregion

        void SaveConfigurationToXML()
        {
            configRoot = new XElement("config");
            try
            {
                configRoot.Add(new XElement("GuestRequestKey", BE.Configuration.GuestRequestKey),
                               new XElement("HostingUnitKey", BE.Configuration.HostingUnitKey),
                               new XElement("OrderKey", BE.Configuration.OrderKey),
                               new XElement("Mng", BE.Configuration.Mng),
                               new XElement("Fee", BE.Configuration.Fee),
                               new XElement("OrderValidity", BE.Configuration.OrderValidity),
                               new XElement("SMTP_Server", BE.Configuration.SMTP_Server),
                               new XElement("MailSystem", BE.Configuration.MailSystem),
                               new XElement("Password", BE.Configuration.Password));
                configRoot.Save(configPath);
            }
            catch (Exception ex)
            {
                throw new Exception("מצטערים, קרתה תקלה במערכת");
            }
        }

        //static readonly string ProjectPath = Directory.GetParent(Directory.GetParent(Environment.CurrentDirectory.ToString()).FullName).FullName;
        XElement configRoot;
        private readonly string guestRequestsPath =  "guestRequests.xml";
        private readonly string hostingUnitsPath =   "hostingUnits.xml";
        private readonly string ordersPath =  "orders.xml";
        private readonly string configPath =  "config.xml"; 
        List<GuestRequest> guestRequestsList = new List<GuestRequest>();
        List<HostingUnit> hostingUnitList = new List<HostingUnit>();
        List<Order> orderList = new List<Order>();
  
         

        #region HostingUnit
        public void addHostingUnit(HostingUnit hostingUnit)
        {
            hostingUnitList.Add(hostingUnit.Clone());
            tool.SaveToXML<List<HostingUnit>>(hostingUnitList, hostingUnitsPath);
            SaveConfigurationToXML();
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
            SaveConfigurationToXML();
        }
        public List<Order> getAllOrders(Func<Order, bool> predicate = null)
        {
            try
            {
                if (predicate != null)
                    return orderList.Where(predicate).ToList().Clone();
                Order[] OrderArr = new Order[orderList.Count];
                orderList.CopyTo(OrderArr);
                return OrderArr.ToList();
            }
            catch (Exception)
            { throw new Exception("מצטערים, קרתה שגיאה");  }
        }
        public void updateOrder(Order order)
        {
            int index = orderList.FindIndex(Item => Item.OrderKey == order.OrderKey);
            orderList[index] = order.Clone();
            tool.SaveToXML<List<Order>>(orderList, ordersPath);
        }

        #endregion

        #region GuestRequest
        public void addGuestRequest(GuestRequest guestRequest)
        { 
           guestRequestsList.Add(guestRequest.Clone());
           tool.SaveToXML<List<GuestRequest>>(guestRequestsList, guestRequestsPath);
            SaveConfigurationToXML();
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
