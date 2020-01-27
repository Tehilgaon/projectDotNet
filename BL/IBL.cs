using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE;

namespace BL
{
    public interface IBL
    {
        #region HostingUnit        
        void addHostingUnit(HostingUnit hostingUnit); 
        List<HostingUnit> getAllHostingUnits(Func<HostingUnit, bool> predicate = null);
        void updateHostingUnit(HostingUnit hostingUnit);
        void deleteHostingUnit(HostingUnit hostingUnit);
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

        //List<BankBranch> GetAllBranches(); 
        List<HostingUnit> AllAvailable(DateTime FirstDate, int NumOfDays); 
        HostingUnit ifAvailable(HostingUnit hostingUnit, DateTime EntryDate, DateTime ReleaseDate);  
        int DaysBetween(DateTime D1, DateTime D2 = default); 
        List<Order> AllOrdersSince(TimeSpan Time); 
        int AllOrdersOfGuestRequest(string guestRequestKey, Enums.OrderStatus status);  
        int AllOrdersOfHostingUnit(string hostingUnitKey, Enums.OrderStatus status); 
        List<IGrouping<string, GuestRequest>> GroupGuestRequestByRegion();  
        List<IGrouping<int, GuestRequest>> GroupGuestRequestByNumOfGuests(); 
        List<IGrouping<int, Host>> GroupHostByNumOfHostingUnit();  
        List<IGrouping<string, HostingUnit>> GroupHostingUnitByRegion();
        List<IGrouping<string, HostingUnit>> GroupHostingUnitsByType();
        List<IGrouping<Enums.OrderStatus, Order>> GroupOrdersByStatus(); 
        List<IGrouping<DateTime, Order>> GroupOrderByDate();
         

    }
}