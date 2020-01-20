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
    internal class DALList : IDAL
    {
        #region Singleton
        private static readonly DALList instance = new DALList();
        public static DALList Instance
        {
            get { return instance; }
        }

        private DALList() {
            
        }
        static DALList() { }

        #endregion
        
        #region HostingUnit
        public void addHostingUnit(HostingUnit hostingUnit)
        {
            DataSource.hostingUnits.Add(hostingUnit.Clone());
        } 
        public List<HostingUnit> getAllHostingUnits(Func<HostingUnit, bool> predicate = null)
        {
            if(predicate!=null) 
                return DataSource.hostingUnits.Where(predicate).ToList().Clone();
            HostingUnit[] hostingUnitArr = new HostingUnit[DataSource.hostingUnits.Count];
            DataSource.hostingUnits.CopyTo(hostingUnitArr);
            return hostingUnitArr.ToList();
        }
        public void deleteHostingUnit(HostingUnit hostingUnit)
        {
            HostingUnit Unit= DataSource.hostingUnits.Where(Item => Item.HostingUnitKey == hostingUnit.HostingUnitKey).FirstOrDefault();
            DataSource.hostingUnits.Remove(Unit);   
        }
        public void updateHostingUnit(HostingUnit hostingUnit)
        {
            int index=DataSource.hostingUnits.FindIndex(Item => Item.HostingUnitKey == hostingUnit.HostingUnitKey);
            DataSource.hostingUnits[index] = hostingUnit.Clone();  
        }   
        #endregion

        #region Order
        public void addOrder(Order order)
        {
            DataSource.orders.Add(order.Clone());
        }
        public List<Order> getAllOrders(Func<Order, bool> predicate=null)
        {
            if(predicate!=null) 
                return DataSource.orders.Where(predicate).ToList().Clone();
            Order[] OrderArr = new Order[DataSource.orders.Count];
            DataSource.orders.CopyTo(OrderArr);
            return OrderArr.ToList();

        } 
        public void updateOrder(Order order)
        {
            int index = DataSource.orders.FindIndex(Item => Item.OrderKey == order.OrderKey);
            DataSource.orders[index] = order.Clone();
        }

        #endregion

        #region GuestRequest
        public void addGuestRequest(GuestRequest guestRequest)
        {
             
            /*try
            {
                XElement Request = new XElement("guestRequest");
                Request.Add(new XElement("guestRequestKey", guestRequest.GuestRequestKey),
                      new XElement("PrivateName", guestRequest.PrivateName),
                      new XElement("FamilyName", guestRequest.FamilyName),
                      new XElement("entryDate", guestRequest.EntryDate.ToShortDateString()),
                      new XElement("releaseDate", guestRequest.ReleaseDate.ToShortDateString()),
                      new XElement("RegistrationDate", guestRequest.RegistrationDate.ToString()),
                      new XElement("MailAddress", guestRequest.MailAddress),
                      new XElement("Area", guestRequest.Area),
                      new XElement("Adults", guestRequest.Adults),
                      new XElement("Children", guestRequest.Children),
                      new XElement("Type", guestRequest.Type),
                      new XElement("Status", guestRequest.Status));
               // new XElement("NumOfDrivingLessons", guestRequest.), 
               guestRequestRoot.Add(Request);
               guestRequestRoot.Save(guestRequestsPath);
                
            }
            catch (Exception)
            { }*/
            DataSource.guestRequests.Add(guestRequest.Clone());
        }
        public void updateGuestRequest(GuestRequest guestRequest)
        {
            DataSource.guestRequests.RemoveAll(Item => Item.GuestRequestKey == guestRequest.GuestRequestKey);
            addGuestRequest(guestRequest); 
        }
        public List<GuestRequest> GetAllGuestRequest()
        {
            return DataSource.guestRequests.Select(Item => Item).ToList().Clone();
        }
        public List<GuestRequest> GetAllGuestRequests(Func<GuestRequest, bool> predicate=null)
        {
            if(predicate!=null)
                return DataSource.guestRequests.Where(predicate).ToList().Clone();
            return DataSource.guestRequests.Select (Item => Item).ToList().Clone();

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
