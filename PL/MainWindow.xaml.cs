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
using System.Collections.ObjectModel;
using System.IO;


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

        public string guestMail, hostMail;

        public GuestRequest GuestRequest { get; set; }
        public HostingUnit HostingUnit { get; set; }

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
            this.GuestZone.AddButton.Content = "הוסף בקשה";
            this.GuestZone.AddButton.Click += GuestAddButton_Click;
            this.GuestZone.LogInButton.Click += GuestLogInButton_Click;
            this.GuestZone.dataGrid.SelectionChanged += Guest_selectionChange;
            this.GuestZone.dataGrid.MouseDoubleClick += GuestUpdateButton_Click;
            this.GuestZone.dataGrid.AutoGeneratingColumn += Guest_AutoGenerateColumns;
            this.GuestZone.tbxSearch.TextChanged += GuestFilter;
            this.GuestZone.cbxNewOld.SelectionChanged += GuestFilter;
            this.GuestZone.LogoutButton.Click += GuestLogoutButton_Click;

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
            this.HostZone.updateButton.Click += UnitUpdateButton_Click;
            this.HostZone.watchOrdersButton.Click += WatchOrdersButton_Click;
            this.HostZone.dataGrid.MouseDoubleClick +=UnitUpdateButton_Click;
            this.HostZone.LogoutButton.Click += HostLogoutButton_Click;
        }

        

        private void Manager_Zone()
        {
            this.ManagerZone.tbkEnterMail.Text = "";
            this.ManagerZone.AddButton.Visibility = Visibility.Collapsed;
            this.ManagerZone.LogInButton.Click += ManagerLogInButton_Click;
            this.ManagerZone.cbxfilter.SelectionChanged += Cbxfilter_SelectionChanged;
            this.ManagerZone.dataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            this.ManagerZone.cbxgroupBy.SelectionChanged += CbxgroupBy_SelectionChanged;
            this.ManagerZone.LogoutButton.Click += LogoutButton_Click;
        }

         


        #region GuestZone
        private void Guest_selectionChange(object sender, SelectionChangedEventArgs e)
        {
            if (e.OriginalSource != null)
            {
                Type type = e.OriginalSource.GetType();
                if (type == typeof(DataGrid))
                    GuestRequest = (sender as DataGrid).SelectedItem as GuestRequest; 
            }

        } 
        private void GuestAddButton_Click(object sender, RoutedEventArgs e)
        { 
            if (new AddGuestRequest().ShowDialog() == true) 
                MessageBox.Show("בקשתך נוספה");
                  
        }
        private void GuestUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            new AddGuestRequest(GuestRequest).ShowDialog();
        }

        private void GuestLogInButton_Click(object sender, RoutedEventArgs e)
        {
            guestMail = this.GuestZone.tbxEnterMail.Text;
            GuestFilter(this, new RoutedEventArgs());
            if (guestRequestsList.Count != 0)
                logIn(this.GuestZone);             
        }
        private void GuestLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            guestMail = null;
            logOut(this.GuestZone);
        }

        #endregion

        #region HostZone
        private void HostingUnitAdd_Click(object sender, RoutedEventArgs e)
        {
            if (new AddHostingUnit().ShowDialog() == true) 
                MessageBox.Show("היחידה נוספה בהצלחה");
      
        }
        private void HostLogInButton_Click(object sender, RoutedEventArgs e)
        {
            hostMail = this.HostZone.tbxEnterMail.Text;
            HostingUnitFilter(this, new RoutedEventArgs());
            if (hostingUnitsList.Count != 0)
                logIn(this.HostZone);
        }
        private void Unit_selectionChange(object sender, SelectionChangedEventArgs e)
        { 
            HostingUnit = (sender as DataGrid).SelectedItem as HostingUnit;
            if (HostingUnit != null)
                this.HostZone.spUnitbuttons.Visibility = Visibility.Visible;
            else
                this.HostZone.spUnitbuttons.Visibility = Visibility.Collapsed;
        }
        private void UnitUpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (new AddHostingUnit(HostingUnit).ShowDialog() == true)
                MessageBox.Show("פרטי יחידה עודכנו בהצלחה");

        }
        private void WatchOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Orders WindOrder = new Orders();
                WindOrder.CurrentHostingUnit = HostingUnit;
                guestRequestsList = bL.GetAllGuestRequests(Item => Item.Area == HostingUnit.Area
                 && Item.Type == HostingUnit.HostingUnitType && Item.Status == Enums.GuestRequestStatus.Active.ToString()
                 && bL.ifAvailable(HostingUnit, Item.EntryDate, Item.ReleaseDate) != null);
                foreach (var item in guestRequestsList)
                {
                    creatOrder(item, HostingUnit);
                }
                ordersList = bL.getAllOrders(Item => Item.HostingUnitKey == HostingUnit.HostingUnitKey);
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
                order.GuestMail = guestRequest.MailAddress;
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
                    bL.deleteHostingUnit(HostingUnit);
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
        private void HostLogoutButton_Click(object sender, RoutedEventArgs e)
        {
            hostMail = null;
            logOut(this.HostZone);
        }

        #endregion

        #region ManagerZone
        private void Cbxfilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] myKeys;
            this.ManagerZone.cbxgroupBy.Visibility = Visibility.Visible;
            this.ManagerZone.LgroupBy.Visibility = Visibility.Visible;
            switch (((ComboBoxItem)ManagerZone.cbxfilter.SelectedItem).Content.ToString())
            { 
                case "דרישות לקוח":   
                    MgGuestRequestsList = bL.GetAllGuestRequests();
                    myKeys=new string[] { "אזורי ביקוש", "מספר אנשים" };
                    this.ManagerZone.cbxgroupBy.ItemsSource = myKeys;
                    ManagerZone.dataGrid.ItemsSource = MgGuestRequestsList; 
                    break;
                case "יחידות אירוח":
                    MgHostingUnitsList = bL.getAllHostingUnits();
                    myKeys =new string[] { "אזורי אירוח", "סוגי אירוח" };
                    this.ManagerZone.cbxgroupBy.ItemsSource = myKeys;
                    ManagerZone.dataGrid.ItemsSource = MgHostingUnitsList;
                    break;
                case "הזמנות":
                    MgOrdersList = bL.getAllOrders();
                    myKeys = new string[] { "סטטוס", "תאריך הזמנה" };
                    this.ManagerZone.cbxgroupBy.ItemsSource = myKeys;
                    ManagerZone.dataGrid.ItemsSource = MgOrdersList;
                    break;
                default:
                    
                    break;
                    
            }
        }

        private void CbxgroupBy_SelectionChanged(object sender, SelectionChangedEventArgs e) 
        {  
            switch (this.ManagerZone.cbxgroupBy.SelectedItem) 
            {
                case "מספר אנשים":
                    showGroupingGuestByNumOfPeople();
                    break;
                case "אזורי ביקוש":
                    showGroupingGuestByAreas();
                    break;
                case "אזורי אירוח":
                    showGroupingUnitsByAreas();
                    break;
                case "סוגי אירוח":
                    showGroupingUnitsByType();
                    break;
                case "תאריך הזמנה":
                    showGroupingOrderByOrderDate();
                    break;
                case "סטטוס":
                   showGroupingOrderByStatus();
                    break;
                default:
                    break;




            }

        }

        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (((ComboBoxItem)this.ManagerZone.cbxfilter.SelectedItem).Content.ToString())
            {
                case "דרישות לקוח":
                    
                    switch(e.PropertyName)
                    {
                        case "RegistrationDate":
                            e.Column.Header = "תאריך הרשמה";
                            e.Column.Visibility = Visibility.Visible;
                            break;
                        case "GuestRequestKey":
                            e.Column.Header = "מספר סידורי";
                            e.Column.Visibility = Visibility.Visible;
                            break;
                        case "Status":
                            e.Column.Header = "סטטוס";
                            e.Column.Visibility = Visibility.Visible;
                            break;
                        default:
                            Guest_AutoGenerateColumns(sender, e);
                            break;
                    } 
                    break;
                 case "יחידות אירוח":

                    switch(e.PropertyName)
                    {
                        case "YearlyOccupied":
                            e.Column.Header = "תפוסה שנתית";
                            e.Column.Visibility = Visibility.Visible;
                            break;
                        case "HostingUnitKey":
                            e.Column.Header = "מספר סידורי";
                            e.Column.Visibility = Visibility.Visible;
                            break;
                        case "Host":
                            e.Column.Header = "Host";
                            break;
                        default:
                            HostingUnit_AutoGenerateColumns(sender, e);
                            break;
                    }
                    break;
                case "הזמנות":
                     OrdersGrid_AutoGeneratingColumn(sender, e);
                    break;
                default: 
                    break;
            }
        }
        private void ManagerLogInButton_Click(object sender, RoutedEventArgs e)
        {

            if (ManagerZone.tbxEnterMail.Text == Configuration.Mng.ToString()) 
                logIn(this.ManagerZone); 
        }
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            logOut(this.ManagerZone);
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
                case "GuestMail":
                    e.Column.Header = "מייל הלקוח";
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
                guestRequestsList =  bL.GetAllGuestRequests(Item => Item.MailAddress == guestMail &&
                (Item.PrivateName.Contains(text)||Item.FamilyName.Contains(text)||text=="Search")).ToList(); 
                 
                if (newOld != null)
                {
                    guestRequestsList = guestRequestsList.OrderByDescending(item => item.RegistrationDate).ToList();
                    if (newOld == "הישנים יותר")
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
                hostingUnitsList = bL.getAllHostingUnits(Item => Item.Host.MailAddress == hostMail);
                
                this.HostZone.dataGrid.ItemsSource = hostingUnitsList;  
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "לא נמצאו הזמנות", MessageBoxButton.OK,
                                MessageBoxImage.Error, MessageBoxResult.Cancel, MessageBoxOptions.RightAlign);
            }

        }
        private void logOut(GuestUC zone)
        {
            zone.dataGrid.ItemsSource = null;
            zone.dataGrid.Columns.Clear();
            zone.SpLogin.Visibility = Visibility.Visible;
            zone.LogoutButton.Visibility = zone.dataGrid.Visibility=zone.spFilter.Visibility=Visibility.Collapsed;
            
        }
        private void logIn(GuestUC zone)
        {
            zone.spFilter.Visibility = zone.dataGrid.Visibility = zone.LogoutButton.Visibility = Visibility.Visible;
            zone.tbxEnterMail.Clear();
            zone.SpLogin.Visibility = Visibility.Collapsed;
            if (zone == this.ManagerZone)
                zone.cbxfilter.Visibility = Visibility.Visible;
            if (zone == this.GuestZone)
                zone.cbxNewOld.Visibility = Visibility.Visible;

        }


        /*void showGroupingGuestByAreas()
        {
            ManagerZone.groupGrid.Visibility = Visibility.Visible;
            ManagerZone.groupGrid.Columns.Clear(); 
             
            ObservableCollection< GroupInfoCollection < GuestRequest> >l= new ObservableCollection<GuestRequest>();
            foreach(var item in bL.GroupGuestRequestByRegion())
            {
                 
            }
             
        }*/

        void showGroupingGuestByAreas()
        {
            MgGuestRequestsList = new List<GuestRequest>();
            foreach (var item in bL.GroupGuestRequestByRegion()) 
                foreach (var value in item)
                  MgGuestRequestsList.Add(value);
            this.ManagerZone.dataGrid.ItemsSource = MgGuestRequestsList;
        }
        void showGroupingGuestByNumOfPeople()
        {
            MgGuestRequestsList = new List<GuestRequest>();
            foreach (var item in bL.GroupGuestRequestByNumOfGuests())
                foreach (var value in item)
                    MgGuestRequestsList.Add(value);
            this.ManagerZone.dataGrid.ItemsSource = MgGuestRequestsList;
        }
        void showGroupingUnitsByAreas()
        {
            MgHostingUnitsList= new List<HostingUnit>();
            foreach (var item in bL.GroupHostingUnitByRegion())
                foreach (var value in item)
                    MgHostingUnitsList.Add(value);
            this.ManagerZone.dataGrid.ItemsSource = MgHostingUnitsList;
        }
        void showGroupingUnitsByType()
        {
            MgHostingUnitsList = new List<HostingUnit>();
            foreach (var item in bL.GroupHostingUnitsByType())
                foreach (var value in item)
                    MgHostingUnitsList.Add(value);
            this.ManagerZone.dataGrid.ItemsSource = MgHostingUnitsList;
        }
        void showGroupingOrderByStatus()
        {
            MgOrdersList = new List<Order>();
            foreach (var item in bL.GroupOrdersByStatus())
                foreach (var value in item)
                    MgOrdersList.Add(value);
            this.ManagerZone.dataGrid.ItemsSource = MgOrdersList;
        }
        void showGroupingOrderByOrderDate()
        {
            MgOrdersList = new List<Order>();
            foreach (var item in bL.GroupOrderByDate())
                foreach (var value in item)
                    MgOrdersList.Add(value);
            this.ManagerZone.dataGrid.ItemsSource = MgOrdersList.Where(item=>item.OrderDate!=default);
        }






    }
}
