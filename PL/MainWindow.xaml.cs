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
    public partial class MainWindow
    {
        private MyBL bL;
        List<GuestRequest> guestRequestsList;
        List<HostingUnit> hostingUnitsList;
        List<Order> ordersList;
        GuestRequest guestRequest;
        HostingUnit hostingUnit;
        Order order;

        List<GuestRequest> MgGuestRequestsList;
        List<HostingUnit> MgHostingUnitsList;
        List<Order> MgOrdersList;



        public MainWindow()
        {
            try
            {
                InitializeComponent();
                bL = MyBL.Instance;
                Guest_Zone();
                Host_Zone();
                Manager_Zone();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void Guest_Zone()
        {
            this.GuestZone.tbkEnterMail.Text = "התחבר כאורח";
            this.GuestZone.AddButton.Click += GuestAddButton_Click;
            this.GuestZone.LogInButton.Click += GuestLogInButton_Click;
            this.GuestZone.dataGrid.SelectionChanged += Guest_selectionChange;
            this.GuestZone.dataGrid.MouseDoubleClick += GuestUpdateButton_Click; 
            this.GuestZone.dataGrid.AutoGeneratingColumn += Guest_AutoGenerateColumns;
            this.GuestZone.tbxSearch.TextChanged += GuestFilter;
            this.GuestZone.cbxNewOld.SelectionChanged += GuestFilter;
            
        }
         
        private void Host_Zone()
        {
            this.HostZone.tbkEnterMail.Text = "התחבר כבעל יחידת אירוח";
            this.HostZone.AddButton.Content = "הוסף יחידה";
            this.HostZone.dataGrid.SelectionChanged += Unit_selectionChange;
            this.HostZone.dataGrid.AutoGeneratingColumn += HostingUnit_AutoGenerateColumns;
            this.HostZone.deleteButton.Click += HostingUnitDeleteButton_Click;
            this.HostZone.AddButton.Click += HostingUnitAdd_Click;
            this.HostZone.LogInButton.Click += HostLogInButton_Click;
            this.HostZone.updateButton.Click += updateUnitButton_Click;
            this.HostZone.watchOrdersButton.Click += WatchOrdersButton_Click;

        }
        private void Manager_Zone()
        {
            this.ManagerZone.tbkEnterMail.Text = "";
            this.ManagerZone.AddButton.Visibility = Visibility.Collapsed;
            this.ManagerZone.LogInButton.Click += ManagerLogInButton_Click;
            this.ManagerZone.cbxfilter.SelectionChanged += Cbxfilter_SelectionChanged;
            this.ManagerZone.dataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
        }

       

       






        

        #region GuestZone
        private void Guest_selectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != null)
            {
                Type type = e.OriginalSource.GetType();
                if (type == typeof(DataGrid))
                    guestRequest = (sender as DataGrid).SelectedItem as GuestRequest; 
            }

        } 
        private void GuestAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (new AddGuestRequest().ShowDialog() == true) 
                MessageBox.Show("בקשתך נוספה");
                  
        }
        private void GuestUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            new AddGuestRequest(guestRequest).ShowDialog();
        }

        private void GuestLogInButton_Click(object sender, RoutedEventArgs e)
        {
            GuestZone.spFilter.Visibility = Visibility.Visible;
            GuestFilter(this, new RoutedEventArgs());

        }

        #endregion

        #region HostZone
        private void HostingUnitAdd_Click(object sender, RoutedEventArgs e)
        {
            if (new AddHostingUnit().ShowDialog() == true)
            {
                MessageBox.Show("היחידה נוספה בהצלחה");
            }

            else
                MessageBox.Show("no");
        }
        private void HostLogInButton_Click(object sender, RoutedEventArgs e)
        {
                HostZone.dataGrid.Visibility = Visibility.Visible;
                HostingUnitFilter(this, new RoutedEventArgs());
            
        }
        private void Unit_selectionChange(object sender, SelectionChangedEventArgs e)
        { 
            hostingUnit = (sender as DataGrid).SelectedItem as HostingUnit;
            this.HostZone.deleteButton.Visibility = Visibility.Visible;
            this.HostZone.updateButton.Visibility = Visibility.Visible;
            this.HostZone.watchOrdersButton.Visibility = Visibility.Visible;
        }
        private void updateUnitButton_Click(object sender, RoutedEventArgs e)
        {
            if (new AddHostingUnit(hostingUnit).ShowDialog() == true)
                MessageBox.Show("פרטי יחידה עודכנו בהצלחה");

        }
        private void WatchOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Orders WindOrder = new Orders();
                WindOrder.CurrentHostingUnit = hostingUnit;
                guestRequestsList = bL.GetAllGuestRequests(Item => Item.Area == hostingUnit.Area
                 && Item.Type == hostingUnit.HostingUnitType && Item.Status == Enums.GuestRequestStatus.Active.ToString()
                 && bL.ifAvailable(hostingUnit, Item.EntryDate, Item.ReleaseDate) != null);
                foreach (var item in guestRequestsList)
                {
                    creatOrder(item, hostingUnit);
                }
                ordersList = bL.getAllOrders(Item => Item.HostingUnitKey == hostingUnit.HostingUnitKey);
                if (ordersList.Count != 0)
                {
                    WindOrder.OrdersGrid.ItemsSource = ordersList;
                } 
                WindOrder.ShowDialog();
                HostingUnitFilter(this, new RoutedEventArgs());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void creatOrder(GuestRequest guestRequest, HostingUnit hostingUnit)
        {
            if (bL.getAllOrders(item => item.GuestRequestKey == guestRequest.GuestRequestKey && item.HostingUnitKey == hostingUnit.HostingUnitKey).Count == 0)
            {
                order = new Order();
                order.GuestRequestKey = guestRequest.GuestRequestKey;
                order.HostingUnitKey = hostingUnit.HostingUnitKey;
                bL.addOrder(order);
            }
        }

        private void HostingUnitDeleteButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MessageBox.Show("האם אתה בטוח שברצונך למחוק את היחידה",
                "מחיקת יחידה", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    bL.deleteHostingUnit(hostingUnit);
                    HostingUnitFilter(this, new RoutedEventArgs());
                    this.HostZone.deleteButton.Visibility = Visibility.Collapsed;
                    this.HostZone.updateButton.Visibility = Visibility.Collapsed;
                    this.HostZone.watchOrdersButton.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region ManagerZone
        private void Cbxfilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            switch (((ComboBoxItem)ManagerZone.cbxfilter.SelectedItem).Content.ToString())
            {
                 
                case "דרישות לקוח":   
                    MgGuestRequestsList = bL.GetAllGuestRequests();
                    ManagerZone.dataGrid.ItemsSource = MgGuestRequestsList;
                    break;
                case "יחידות אירוח":
                    MgHostingUnitsList = bL.getAllHostingUnits();  
                    ManagerZone.dataGrid.ItemsSource = MgHostingUnitsList;
                    break;
                case "הזמנות":
                    MgOrdersList = bL.getAllOrders(); 
                    ManagerZone.dataGrid.ItemsSource = MgOrdersList;
                    break;

                   
                    
            }
        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (((ComboBoxItem)this.ManagerZone.cbxfilter.SelectedItem).Content.ToString())
            {
                case "דרישות לקוח":
                    Guest_AutoGenerateColumns(sender, e);
                    break;
                case "יחידות אירוח":
                    HostingUnit_AutoGenerateColumns(sender, e);
                    break;
                default:
                    OrdersGrid_AutoGeneratingColumn(sender, e);
                    break;
            }
        }
        private void ManagerLogInButton_Click(object sender, RoutedEventArgs e)
        {
            if (ManagerZone.tbxEnterMail.Text == Configuration.Password.ToString())
                this.ManagerZone.cbxfilter.Visibility = Visibility.Visible;
        }

        #endregion
        public void OrdersGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "OrderStatus":
                    e.Column.Header = "סטטוס הזמנה";
                    e.Column.IsReadOnly = true;
                    break;
                case "OrderKey":
                    e.Column.Header = "מספר הזמנה";
                    e.Column.IsReadOnly = true;
                    break;
                case "CreateDate":
                    e.Column.Header = "תאריך יצירת ההזמנה";
                    e.Column.IsReadOnly = true;
                    break;
                case "OrderDate":
                    e.Column.Header = "תאריך סגירת ההזמנה";
                    e.Column.IsReadOnly = true;
                    break;
                case "HostingUnitKey":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
                case "GuestRequestKey":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        public void Guest_AutoGenerateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "PrivateName":
                    e.Column.Header = "שם פרטי";
                    e.Column.IsReadOnly = true;
                    break;
                case "FamilyName":
                    e.Column.Header = "שם משפחה";
                    e.Column.IsReadOnly = true;
                    break;
                case "MailAddress":
                    e.Column.Header = "כתובת מייל";
                    e.Column.IsReadOnly = true;
                    break;
                case "EntryDate":
                    e.Column.Header = "תאריך צ'יק-אין";
                    e.Column.IsReadOnly = true;
                    break;
                case "ReleaseDate":
                    e.Column.Header = "תאריך צ'יק אאוט";
                    e.Column.IsReadOnly = true;
                    break;
                case "Area":
                    e.Column.Header = "איזור רצוי";
                    e.Column.IsReadOnly = true;
                    break;
               
                case "Type":
                    e.Column.Header = "סוג אירוח";
                    e.Column.IsReadOnly = true;
                    break;
                case "Adults":
                    e.Column.Header = "מבוגרים";
                    e.Column.IsReadOnly = true;
                    break;
                case "Children":
                    e.Column.Header = "ילדים";
                    e.Column.IsReadOnly = true;
                    break;
                default: 
                    e.Column.Visibility = Visibility.Collapsed;
                    break;

            }
        }
        public void HostingUnit_AutoGenerateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "HostingUnitName":
                    e.Column.Header = "שם יחידה";
                    e.Column.IsReadOnly = true;
                    break;
                case "HostingUnitType":
                    e.Column.Header = "סוג אירוח";
                    e.Column.IsReadOnly = true;
                    break;
                case "Area":
                    e.Column.Header = "איזור";
                    e.Column.IsReadOnly = true;
                    break;
                case "Fee":
                    e.Column.Header = "עמלה";
                    e.Column.IsReadOnly = true;
                    break;
                default:
                    e.Column.Visibility = Visibility.Collapsed;
                    break;     

            }
        }

        public void GuestFilter(object sender, RoutedEventArgs e)
        {

            string newOld=null; 
            string text = GuestZone.tbxSearch.Text;
            if(GuestZone.cbxNewOld.SelectedItem!=null) 
                 newOld = ((ComboBoxItem)GuestZone.cbxNewOld.SelectedItem).Content.ToString();   
            try
            {
                guestRequestsList =  bL.GetAllGuestRequests(Item => Item.MailAddress == GuestZone.tbxEnterMail.Text &&
                (Item.PrivateName.Contains(text)||Item.FamilyName.Contains(text))).ToList(); 
                if (newOld != null)
                {
                    guestRequestsList = guestRequestsList.OrderByDescending(item => item.RegistrationDate).ToList();
                    if (newOld == "מהישן לחדש")
                        guestRequestsList.Reverse();
                }
                this.GuestZone.dataGrid.ItemsSource = guestRequestsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "לא נמצאו הזמנות", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }

        }
        public void HostingUnitFilter(object sender, RoutedEventArgs e)
        { 
            try
            {
                hostingUnitsList = bL.getAllHostingUnits(Item => Item.Host.MailAddress == HostZone.tbxEnterMail.Text);
                this.HostZone.dataGrid.ItemsSource = hostingUnitsList;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "לא נמצאו הזמנות", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }

        }





    } 
}
