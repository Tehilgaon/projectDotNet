using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace DAL
{
    public interface IDAL
    {
        #region HostingUnit        
        void addHostingUnit(HostingUnit hostingUnit); 
        List<HostingUnit> getAllHostingUnits(Func<HostingUnit, bool> predicate = null);
        void deleteHostingUnit(string Key);
        void updateHostingUnit(HostingUnit hostingUnit);
        #endregion

        #region GuestRequest
        void addGuestRequest(GuestRequest guestRequest);
        void updateGuestRequest(GuestRequest guestRequest); 
        List<GuestRequest> GetAllGuestRequests(Func<GuestRequest, bool> predicate=null);
        #endregion

        #region Order
        void addOrder(Order order); 
        List<Order> getAllOrders(Func<Order, bool> predicate=null);
        void updateOrder(Order order);
        #endregion

        List<BankBranch> GetAllBranches();
    }
}
