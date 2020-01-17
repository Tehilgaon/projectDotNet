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

        List<Order> ordersList;
        List<GuestRequest> guestRequestsList=new List<GuestRequest>();
        




        public HostingUnit CurrentHostingUnit { get; set; }
        public Orders()
        {
            InitializeComponent();
            bL = MyBL.Instance;
            OrdersGrid.AutoGeneratingColumn += ((MainWindow)System.Windows.Application.Current.MainWindow).OrdersGrid_AutoGeneratingColumn;
            OrdersGrid.SelectionChanged += OrdersGrid_SelectionChanged;
            IconMail.Click += IconMail_Click;
            IconClose.Click += IconClose_Click;
            tbxSearch.TextChanged += OrderFilter;
            cbxOrderStatus.SelectionChanged += OrderFilter;
        }

        private void IconClose_Click(object sender, RoutedEventArgs e)
        {
            currentOrder.OrderStatus = Enums.OrderStatus.Closed;
            bL.updateOrder(currentOrder);
            OrderFilter(this, new RoutedEventArgs());
        }
    
         
        private void OrdersGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != null)
            {
                Type type = e.OriginalSource.GetType();
                if (type == typeof(DataGrid))
                {
                    currentOrder = (sender as DataGrid).SelectedItem as Order;
                    if (currentOrder != null)
                    {
                        IconMail.IsEnabled = false;
                        IconClose.IsEnabled = false;
                        if(currentOrder.OrderStatus == Enums.OrderStatus.NotMailed) 
                            IconMail.IsEnabled = true;
                        if (currentOrder.OrderStatus == Enums.OrderStatus.Mailed)
                        {
                            IconClose.IsEnabled = true;
                        }
                    }
                }
            }
            
            
        }
  
        private void IconMail_Click(object sender, RoutedEventArgs e)
        {
            IconMail.IsEnabled = false;
            currentOrder.OrderStatus = Enums.OrderStatus.Mailed;
            currentOrder.OrderDate = DateTime.Now;
            bL.updateOrder(currentOrder); 
            OrderFilter(this, new RoutedEventArgs()); 
        }

        
        private void createOrder_button(object sender, RoutedEventArgs e)
        {
            try
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).creatOrder(guestRequest, CurrentHostingUnit);  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
        public void OrderFilter(object sender, RoutedEventArgs e)
        {
            string orderStatus = null;
            string text = tbxSearch.Text;
            if (cbxOrderStatus.SelectedItem != null)
                orderStatus = ((ComboBoxItem)cbxOrderStatus.SelectedItem).Content.ToString();
            try
            {
                ordersList = bL.getAllOrders(item => item.HostingUnitKey == CurrentHostingUnit.HostingUnitKey &&
                (item.OrderStatus.ToString().Contains(text) || item.OrderKey.Contains(text)) && (item.OrderStatus.ToString() == orderStatus || orderStatus == null||orderStatus=="All"));
                
                OrdersGrid.ItemsSource = ordersList;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "לא נמצאו הזמנות", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }



        }


    }
}
