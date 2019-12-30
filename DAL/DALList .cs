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
        public void deleteHostingUnit(string Key)
        {
            HostingUnit hostingUnit=getAllHostingUnits(Item=>Item.HostingUnitKey==Key).FirstOrDefault();
            DataSource.hostingUnits.Remove(hostingUnit);   
        }
        public void updateHostingUnit(HostingUnit hostingUnit)
        {
            int index=DataSource.hostingUnits.FindIndex(Item => Item.HostingUnitKey == hostingUnit.HostingUnitKey);
            DataSource.hostingUnits.Insert(index ,hostingUnit.Clone());   
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
            DataSource.orders.Where(Item => Item.OrderKey == order.OrderKey).Select(Item => Item = order.Clone());
        }

        #endregion

        #region GuestRequest
        public void addGuestRequest(GuestRequest guestRequest)
        {
            DataSource.guestRequests.Add(guestRequest.Clone());
        }
        public void updateGuestRequest(GuestRequest guestRequest)
        {
            DataSource.guestRequests.Where(Item=>Item.GuestRequestKey==guestRequest.GuestRequestKey).Select(Item=>Item=guestRequest.Clone());
        }
        public List<GuestRequest> GetAllGuestRequest()
        {
            return DataSource.guestRequests.Select(Item => Item).ToList().Clone();
        }
        public List<GuestRequest> GetAllGuestRequests(Func<GuestRequest, bool> predicate=null)
        {
            if(predicate!=null)
                return DataSource.guestRequests.Where(predicate).ToList().Clone();
            return DataSource.guestRequests.Select(Item => Item).ToList().Clone();

        }

        #endregion

        public List<BankAccount> GetAllBranches()
        {
            List<BankAccount> bankAccounts = new List<BankAccount>()
            {
                new BankAccount(12, "Leumi", 200, "Gilo 15", "jerusalem"),
                new BankAccount(12, "Leumi",200, "Gilo 15", "jerusalem"   ),
                new BankAccount(81, "hapoalim",150, "Herzel 30", "Tel Aviv" ),
                new BankAccount(47, "Mizrahi", 411, "Vered 4", "Ashkelon" ),
                new BankAccount(81, "hapoalim", 140, "Ben Guryon 21", "Lod" )
            };
            return bankAccounts;
        }

        
    }
}
