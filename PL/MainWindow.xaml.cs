using BL;
using BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MyBL bL;
        public MainWindow()
        {
            InitializeComponent();
            bL = MyBL.Instance;



            try
            {
                Order order1 = new Order()
                {
                    HostingUnitKey = "1000000",
                    GuestRequestKey = "1000000",
                    OrderDate = new DateTime(2020, 2, 3),
                };
                Order order2 = new Order()
                {
                    HostingUnitKey = "1000001",
                    GuestRequestKey = "1000001",
                    OrderDate = new DateTime(2020, 3, 3),
                };
                Order order3 = new Order()
                {
                    HostingUnitKey = "1000001",
                    GuestRequestKey = "1000002",
                    OrderDate = new DateTime(2020, 3, 3),
                };
                Order order4 = new Order()
                {
                    HostingUnitKey = "1000003",
                    GuestRequestKey = "1000002",
                    OrderDate = new DateTime(2020, 6, 12),
                };
                Order order5 = new Order()
                {
                    HostingUnitKey = "1000004",
                    GuestRequestKey = "1000002",
                    OrderDate = new DateTime(2020, 5, 5),
                };
                Order order6 = new Order()
                {
                    HostingUnitKey = "1000001",
                    GuestRequestKey = "1000000",
                    OrderDate = new DateTime(2020, 9, 9),
                };

                bL.addOrder(order1);
                //bL.addOrder(order1);  //Exception "already Exist"
                bL.addOrder(order2);


                //order1.OrderStatus = Enums.OrderStatus.Mailed;
                //bL.updateOrder(order1);   //Exception "collection clearance  unsigned"
                order2.OrderStatus = Enums.OrderStatus.Mailed;
                bL.updateOrder(order2);
                order2.OrderStatus = Enums.OrderStatus.Closed;
                bL.updateOrder(order2);
                //bL.addOrder(order3); //Exception "Days not available"

                 



                HostingUnit unit1 = new HostingUnit()
                {
                    HostingUnitName = "malon",
                    HostingUnitType = Enums.HostingUnitType.צימר,
                    Area = Enums.Regions.Jerusalem,
                    Host = new Host()
                    {
                        HostKey = "123456789",
                        CollectionClearance = false,
                        PrivateName = "Dani",
                        FamilyName = "cohen",
                        MailAddress = "Dani@gmail.com",
                        PhoneNumber = "0505050505",
                        BankAccountNumber = "123123",
                        Bankbranch = bL.GetAllBranches()[0],
                    }
                };
                HostingUnit unit2 = new HostingUnit()
                {
                    HostingUnitName = "mySelf",
                    HostingUnitType = Enums.HostingUnitType.מאהל,
                    Area = Enums.Regions.North,
                    Host = new Host()
                    {
                        HostKey = "315136952",
                        CollectionClearance = true,
                        PrivateName = "sara",
                        FamilyName = "levi",
                        MailAddress = "SLevi@gmail.com",
                        PhoneNumber = "0505770505",
                        BankAccountNumber = "111111",
                        Bankbranch = bL.GetAllBranches()[3],
                    }
                };
                bL.addHostingUnit(unit1);
                bL.addHostingUnit(unit2);

                unit1.Host.CollectionClearance = true;
                bL.updateHostingUnit(unit1);
                
                bL.addOrder(order4);
                order4.OrderStatus = Enums.OrderStatus.Mailed;
                bL.updateOrder(order4);
                //unit1.Host.CollectionClearance = false;
                //bL.updateHostingUnit(unit1);  //Exception can not cancel collection clearance
                
                HostingUnit unit3 = bL.getAllHostingUnits(item => item.HostingUnitKey == order2.HostingUnitKey).FirstOrDefault();
                //bL.deleteHostingUnit(unit3);

                //unit3 = bL.getAllHostingUnits(item => item.HostingUnitKey == order4.HostingUnitKey).FirstOrDefault();
                //bL.deleteHostingUnit(unit3); //Exception "can not be deleted- open orders

                //lb_HostingUnits.DataContext = bL.AllAvailable(new DateTime(2020, 3, 3), 2);

                bL.addOrder(order5);
                order5.OrderStatus = Enums.OrderStatus.Mailed;
                bL.updateOrder(order5);

                //tbx_exceptions.Text = bL.AllOrdersOfGuestRequest("1000002", Enums.OrderStatus.Mailed).ToString();

                bL.addOrder(order6);
                order6.OrderStatus = Enums.OrderStatus.Mailed;
                bL.updateOrder(order6);
                order6.OrderStatus = Enums.OrderStatus.Closed;
                //bL.updateOrder(order6);

                //tbx_exceptions.Text = bL.AllOrdersOfHostingUnit("1000001", Enums.OrderStatus.Closed).ToString();

                //lb_HostingUnits.DataContext = bL.GroupGuestRequestByRegion();

                tbx_exceptions.Text = unit1.ToString();
            }
            catch (Exception e)
            {
                tbx_exceptions.Text = e.Message;
            }



            }
    }
}
