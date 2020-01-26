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
using System.ComponentModel;

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
            OrdersGrid.MouseDoubleClick += OrdersGrid_MouseDoubleClick;
            tbxSearch.PreviewKeyDown += TbxSearch_PreviewKeyDown;
            IconMail.Click += IconMail_Click;
            IconClose.Click += IconClose_Click;
            tbxSearch.TextChanged += OrderFilter;
            cbxOrderStatus.SelectionChanged += OrderFilter;
            
        }

        private void TbxSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (tbxSearch.Text == "Search")
                tbxSearch.Clear();
        }

        private void OrdersGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            guestRequest = bL.GetAllGuestRequests(item => item.GuestRequestKey == currentOrder.GuestRequestKey).FirstOrDefault();
            new AddGuestRequest(guestRequest).ShowDialog();
        }

        private void IconClose_Click(object sender, RoutedEventArgs e)
        {
            currentOrder.OrderStatus = Enums.OrderStatus.Closed;
            bL.updateOrder(currentOrder);
            OrderFilter(this, new RoutedEventArgs());
            MessageBox.Show("ההזמנה נסגרה בהצלחה");
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
            try
            {
                Email emailwind = new Email(currentOrder); 
                if(emailwind.ShowDialog() == true)
                {
                   OrderFilter(this, new RoutedEventArgs());
                   MessageBox.Show("המייל נשלח בהצלחה");    
                }    
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
             
        }

        
        /*private void createOrder_button(object sender, RoutedEventArgs e)
        {
            try
            {
                ((MainWindow)System.Windows.Application.Current.MainWindow).creatOrder(guestRequest, CurrentHostingUnit);  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }*/

        public void OrderFilter(object sender, RoutedEventArgs e)
        {
            string orderStatus = null;
            string ordersSince= null;
            TimeSpan span;
            string text = tbxSearch.Text;
            if (cbxOrderStatus.SelectedItem != null)
                orderStatus = ((ComboBoxItem)cbxOrderStatus.SelectedItem).Content.ToString();
            if(cbxAllOrderSince.SelectedItem!=null)
                ordersSince= ((ComboBoxItem)cbxOrderStatus.SelectedItem).Content.ToString();
            try
            {
                if (ordersSince != null||ordersSince!="All")
                {
                    span = ordersSince == "This week" ? new TimeSpan(7, 0, 0, 0) : ordersSince == "This month" ? new TimeSpan(30, 0, 0, 0) : new TimeSpan(372, 0, 0, 0);
                    ordersList = bL.AllOrdersSince(span);
                }
                else { ordersList = bL.getAllOrders(); }      
                ordersList = ordersList.Where(item => item.HostingUnitKey == CurrentHostingUnit.HostingUnitKey &&
                (item.OrderStatus.ToString().Contains(text) || item.OrderKey.Contains(text)||text=="Search"||text=="") && 
                (item.OrderStatus.ToString() == orderStatus || orderStatus == null||orderStatus=="All")).ToList();
                
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
