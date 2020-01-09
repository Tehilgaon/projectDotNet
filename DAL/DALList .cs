using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;
using DS;

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

        private DALList() { }
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

        
    }
}
