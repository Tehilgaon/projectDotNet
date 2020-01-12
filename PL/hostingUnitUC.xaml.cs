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
using BE;
using BL;
namespace PL
{
    /// <summary>
    /// Interaction logic for hostingUnitUC.xaml
    /// </summary>
    public partial class hostingUnitUC : UserControl
    {
        private MyBL bL;
        List<Order> ordersList;
        List<GuestRequest> guestRequestsList;
        Orders currentOrders;
        public HostingUnit CurrentHostingUnit { get; set; }
        public hostingUnitUC(HostingUnit hostingUnit)
        {
            InitializeComponent();
            bL = MyBL.Instance;
            labelUnitAddress.DataContext = hostingUnit.SubArea + " " + hostingUnit.Area;
            this.CurrentHostingUnit = hostingUnit;
            this.HostingUnitGrid.DataContext = CurrentHostingUnit;
        }

        private void updateUnitButton_Click(object sender, RoutedEventArgs e)
        {
            if (new AddHostingUnit(CurrentHostingUnit).ShowDialog() == true)
                MessageBox.Show("פרטי יחידה עודכנו בהצלחה");

        }

        private void WatchOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                currentOrders = new Orders();
                ordersList = bL.getAllOrders(Item => Item.HostingUnitKey == CurrentHostingUnit.HostingUnitKey);
                guestRequestsList = bL.GetAllGuestRequests(Item => Item.Area == CurrentHostingUnit.Area
                  && Item.Type == CurrentHostingUnit.HostingUnitType && bL.ifAvailable(CurrentHostingUnit, Item.EntryDate, Item.ReleaseDate) != null);
                if (ordersList.Count != 0)
                {
                    currentOrders.OrdersGrid.ItemsSource = ordersList;
                }
                if(guestRequestsList.Count!=0)
                {
                    currentOrders.NewOrdersGrid.ItemsSource = guestRequestsList;
                }
                currentOrders.ShowDialog();

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
