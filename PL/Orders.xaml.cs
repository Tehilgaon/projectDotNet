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
using System.Windows.Shapes;
using BL;
using BE;

namespace PL
{
    /// <summary>
    /// Interaction logic for Orders.xaml
    /// </summary>
    public partial class Orders 
    {
        private MyBL bL;
        GuestRequest guestRequest;
        Order currentOrder;
        public Orders()
        {
            InitializeComponent();
            bL = MyBL.Instance;
            NewOrdersGrid.SelectionChanged += LbxNewOrders_SelectionChanged;
            buttonCreateOrder.Click += createOrder_button;
        }

        private void createOrder_button(object sender, RoutedEventArgs e)
        {
            try
            {
                currentOrder = new Order();
                currentOrder.GuestRequestKey = guestRequest.GuestRequestKey;
                //currentOrder.HostingUnitKey=
                bL.addOrder(currentOrder);
                DialogResult = true;
                MessageBox.Show("נוצרה הזמנה עבור" + "  " + guestRequest.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        } 
        private void LbxNewOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            guestRequest= (sender as DataGrid).SelectedItem as GuestRequest;
            buttonCreateOrder.Visibility = Visibility.Visible;

        }

    }
}
