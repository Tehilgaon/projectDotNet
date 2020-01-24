using BL;
using BE;
using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq; 
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
using System.ComponentModel;
using System.Threading;
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
        public List<BankBranch> branchesList = new List<BankBranch>();
        

        List<GuestRequest> MgGuestRequestsList;
        List<HostingUnit> MgHostingUnitsList;
        List<Order> MgOrdersList;
        List<Host> MgHostList;

        XElement banks;
       
        DateTime today = DateTime.Today;
        public string guestMail, hostMail;

        public GuestRequest GuestRequest { get; set; }
        public HostingUnit HostingUnit { get; set; }
        public Order Order { get ; set ; }

        public List<string> Uris = new List<string>() {"https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcQqAfFteQzTFOMnS5TxhADG5iEUB76A1sgLZKtlcsx4UYRTpUpONQ&s",
        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcTUlP9896XWkF3fSVtJCZ-OJBwvq3hXSpSFmB86_LkP6lxunVKK&s",
        "https://encrypted-tbn0.gstatic.com/images?q=tbn:ANd9GcRHaIrCPsJnXsfoZeXaHGRRkh0g1BPcmo6Req6MYO2TbKwvfRxj&s",
        "http://www.lovehotel.live/photos/26aae4d5092f82c89d387d98aef0b19e.jpg"};

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                bL = MyBL.Instance;
                Guest_Zone();
                Host_Zone();
                Manager_Zone();
                LoadBanksList();
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
            this.GuestZone.tbxEnterMail.KeyDown += GuestOnKeyDownHandler;
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
            this.HostZone.tbxEnterMail.KeyDown += HostOnKeyDownHandler;
            this.HostZone.dataGrid.SelectionChanged += Unit_selectionChange;
            this.HostZone.dataGrid.AutoGeneratingColumn += HostingUnit_AutoGenerateColumns;
            this.HostZone.deleteButton.Click += HostingUnitDeleteButton_Click;
            this.HostZone.AddButton.Click += HostingUnitAdd_Click;
            this.HostZone.LogInButton.Click += HostLogInButton_Click;
            this.HostZone.updateButton.Click += UnitUpdateButton_Click;
            this.HostZone.watchOrdersButton.Click += WatchOrdersButton_Click;
            this.HostZone.dataGrid.MouseDoubleClick +=UnitUpdateButton_Click;
            this.HostZone.LogoutButton.Click += HostLogoutButton_Click;
            this.HostZone.watchDairy.Click += WatchDairy_Click;
             
        }
  
        private void Manager_Zone()
        {
            this.ManagerZone.tbkEnterMail.Text = "";
            this.ManagerZone.AddButton.Visibility = Visibility.Collapsed;
            this.ManagerZone.tbxEnterMail.KeyDown += ManagerOnKeyDownHandler;
            this.ManagerZone.LogInButton.Click += ManagerLogInButton_Click;
            this.ManagerZone.cbxfilter.SelectionChanged += Cbxfilter_SelectionChanged;
            this.ManagerZone.dataGrid.AutoGeneratingColumn += DataGrid_AutoGeneratingColumn;
            this.ManagerZone.cbxgroupBy.SelectionChanged += CbxgroupBy_SelectionChanged;
            this.ManagerZone.LogoutButton.Click += LogoutButton_Click;
            this.ManagerZone.tbxSearch.TextChanged += ManagerFilter;
            this.ManagerZone.dataGrid.SelectionChanged+= Manager_SelectionChanged;
            this.ManagerZone.dataGrid.MouseDoubleClick += ManagerZone_DoubleClickEdit;
            this.ManagerZone.cbxgroupBy.Text = "קבץ לפי";
        }






        #region GuestZone

        private void GuestOnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
                GuestLogInButton_Click(this, e);
        }

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
        public void GuestFilter(object sender, RoutedEventArgs e)
        {

            string newOld = null;
            string text = GuestZone.tbxSearch.Text;
            if (GuestZone.cbxNewOld.SelectedItem != null)
                newOld = ((ComboBoxItem)GuestZone.cbxNewOld.SelectedItem).Content.ToString();
            try
            {
                guestRequestsList = bL.GetAllGuestRequests(Item => Item.MailAddress == guestMail &&
               (Item.PrivateName.Contains(text) || Item.FamilyName.Contains(text) || text == "Search")).ToList();

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

        #endregion

        #region HostZone
        private void HostOnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
                HostLogoutButton_Click(this, e);
        }
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
                Order = new Order();
                Order.GuestRequestKey = guestRequest.GuestRequestKey;
                Order.HostingUnitKey = hostingUnit.HostingUnitKey;
                Order.GuestMail = guestRequest.MailAddress;
                bL.addOrder(Order);
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

        private void WatchDairy_Click(object sender, RoutedEventArgs e)
        {

            if (myDairy.Visibility == Visibility.Visible)
                myDairy.Visibility = Visibility.Collapsed;
            else
            {
                myDairy.Visibility = Visibility.Visible;

                /*var arrayDairy = tool.Flatten(HostingUnit);
                for (int i=0;i<arrayDairy.Count;i=i+2)
                { 
                     myDairy.BlackoutDates.Add(new CalendarDateRange(arrayDairy[i], arrayDairy[i+1]));
                }*/
                myDairy.BlackoutDates.Clear();
                for (DateTime day = DateTime.Today; day < today.AddYears(1); day = day.AddDays(1))
                {
                    if (HostingUnit[day] == true)
                        myDairy.BlackoutDates.Add(new CalendarDateRange(day));
                }
            }
             
        }

        #endregion

        #region ManagerZone

        private void ManagerOnKeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
                ManagerLogInButton_Click (this, e);
        }
        private void Cbxfilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] myKeys;
            this.ManagerZone.cbxgroupBy.Visibility = Visibility.Visible;
            this.ManagerZone.LgroupBy.Visibility = Visibility.Visible;
            string text = this.ManagerZone.tbxSearch.Text;
            switch (((ComboBoxItem)ManagerZone.cbxfilter.SelectedItem).Content.ToString())
            { 
                case "דרישות לקוח": 
                    myKeys=new string[] { "אזורי ביקוש", "מספר אנשים" };
                    this.ManagerZone.cbxgroupBy.ItemsSource = myKeys;
                    MgGuestRequestsList = bL.GetAllGuestRequests();
                    ManagerFilter(this, new RoutedEventArgs() );
                    break;
                case "יחידות אירוח":
                    
                    myKeys =new string[] { "אזורי אירוח", "סוגי אירוח" };
                    this.ManagerZone.cbxgroupBy.ItemsSource = myKeys;
                    MgHostingUnitsList = bL.getAllHostingUnits();
                    ManagerFilter(this, new RoutedEventArgs());
                    break;
                case "הזמנות": 
                    myKeys = new string[] { "סטטוס", "תאריך הזמנה" };
                    this.ManagerZone.cbxgroupBy.ItemsSource = myKeys;
                    MgOrdersList = bL.getAllOrders();
                    ManagerFilter(this, new RoutedEventArgs());
                    break;
                case "בעלי יחידות":
                    MgHostList = new List<Host>();
                    foreach (var item in bL.GroupHostByNumOfHostingUnit())
                        foreach (var host in item)
                            MgHostList.Add(host);
                    ManagerFilter(this, new RoutedEventArgs());
                    break;
                default: 
                    break;
                    
            }
        }
        private void Manager_SelectionChanged(object sender, SelectionChangedEventArgs e)
        { 
 
            string choice = ((ComboBoxItem)ManagerZone.cbxfilter.SelectedItem).Content.ToString();
            if (choice == "יחידות אירוח")
                HostingUnit = (sender as DataGrid).SelectedItem as HostingUnit;
            if (choice == "דרישות לקוח")
                GuestRequest = (sender as DataGrid).SelectedItem as GuestRequest;
            if (choice == "הזמנות")
                Order = (sender as DataGrid).SelectedItem as Order;
            
        }



        public void ManagerFilter(object sender, RoutedEventArgs e)
        {
            string text = this.ManagerZone.tbxSearch.Text;
            if (ManagerZone.cbxfilter.SelectedItem != null)
            {
                switch (((ComboBoxItem)ManagerZone.cbxfilter.SelectedItem).Content.ToString())
                {
                    case "דרישות לקוח":
                        ManagerZone.dataGrid.ItemsSource = MgGuestRequestsList.Where(item => (text == "" || text == "Search"
                        || item.MailAddress.Contains(text) || item.PrivateName.Contains(text) ||
                                item.FamilyName.Contains(text) || item.Status.Contains(text) || item.GuestRequestKey.Contains(text)));
                        break;
                    case "יחידות אירוח":
                        ManagerZone.dataGrid.ItemsSource = MgHostingUnitsList.Where(item => item.HostingUnitKey.Contains(text) ||
                        item.HostingUnitName.Contains(text) || item.HostingUnitType.Contains(text) || item.Host.HostKey.Contains(text) ||
                        item.Host.PrivateName.Contains(text) || item.Host.MailAddress.Contains(text) || text == "Search" || text == "");
                        break;
                    case "הזמנות":
                        ManagerZone.dataGrid.ItemsSource = MgOrdersList.Where(item => item.GuestRequestKey.Contains(text) ||
                        item.HostingUnitKey.Contains(text) || item.OrderKey.Contains(text) || item.OrderStatus.ToString().Contains(text) ||
                        text == "Search" || text == "");
                        break;
                    case "בעלי יחידות":
                        ManagerZone.dataGrid.ItemsSource = MgHostList.Where(item => item.HostKey.Contains(text) || item.MailAddress.Contains(text) ||
                         item.PhoneNumber.Contains(text) || item.PrivateName.Contains(text) || item.FamilyName.Contains(text) || text == "Search" || text == "");
                        break;
                    default:
                        break;
                }
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
            ManagerFilter(this, new RoutedEventArgs());

        }
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
            MgHostingUnitsList = new List<HostingUnit>();
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
            this.ManagerZone.dataGrid.ItemsSource = MgOrdersList.Where(item => item.OrderDate != default);
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
        private void ManagerZone_DoubleClickEdit(object sender, MouseButtonEventArgs e)
        {
            try
            {
                string choice = ((ComboBoxItem)ManagerZone.cbxfilter.SelectedItem).Content.ToString();
                if (choice == "יחידות אירוח")
                    new AddHostingUnit(HostingUnit).ShowDialog();
                if (choice == "דרישות לקוח")
                    GuestUpdateButton_Click(this, new RoutedEventArgs());
                //if (choice == "הזמנות")
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
        private void DataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (((ComboBoxItem)this.ManagerZone.cbxfilter.SelectedItem).Content.ToString())
            {
                case "דרישות לקוח":

                    switch (e.PropertyName)
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

                    switch (e.PropertyName)
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
                            e.Column.Header = "בעל היחידה";
                            break;
                        default:
                            HostingUnit_AutoGenerateColumns(sender, e);
                            break;
                    }
                    break;
                case "הזמנות":
                    OrdersGrid_AutoGeneratingColumn(sender, e);
                    break;
                case "בעלי יחידות":
                    HostGrid_AutoGeneratingColumn(sender, e);
                    break;
                default:
                    break;
            }
        }

        private void HostGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "HostKey":
                    e.Column.Header = "תעודת זהות";

                    break;
                case "PrivateName":
                    e.Column.Header = "שם פרטי";

                    break;
                case "FamilyName":
                    e.Column.Header = "שם משפחה";

                    break;
                case "PhoneNumber":
                    e.Column.Header = "פלאפון";

                    break;
                case "MailAddress":
                    e.Column.Header = "כתובת מייל";

                    break;
                case "CollectionClearance":
                    e.Column.Header = "אישור גביה";

                    break;
                case "Fee":
                    e.Column.Header = "עמלה";

                    break;
                case "Bankbranch":
                    e.Column.Header = "פרטי סניף";

                    break;
                case "BankAccountNumber":
                    e.Column.Header = "מספר חשבון";

                    break;


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
            //if (zone == this.HostZone)
               

        }

          

            public void LoadBanksList()
            {
                BackgroundWorker Worker = new BackgroundWorker();
                Worker.DoWork += Worker_DoWork;
                Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
                Worker.WorkerReportsProgress = true;
                Worker.RunWorkerAsync("argument");

            }

            private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
            {

                try
                {
                    banks = XElement.Load(e.Result.ToString());
                }
                catch
                {
                    MessageBox.Show("קרתה תקלה בטעינת הנתונים");
                }
                branchesList = (from bank in banks.Elements()
                                select
                             new BankBranch(
                                 Convert.ToInt32(bank.Element("קוד_בנק").Value),
                                 bank.Element("שם_בנק").Value,
                                 Convert.ToInt32(bank.Element("קוד_סניף").Value),
                                 bank.Element("כתובת_ה-ATM").Value,
                                 bank.Element("ישוב").Value
                                  )).ToList();


            }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        { if (!File.Exists(@"atm.xml")) {
                const string xmlLocalPath = @"atm.xml";

            WebClient wc = new WebClient();
            try
            {
                string xmlServerPath =
               @"http://www.boi.org.il/he/BankingSupervision/BanksAndBranchLocations/Lists/BoiBankBranchesDocs/atm.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
            }
            catch (Exception)
            {
                string xmlServerPath = @"http://www.jct.ac.il/~coshri/atm.xml";
                wc.DownloadFile(xmlServerPath, xmlLocalPath);
            }
            finally
            {
                wc.Dispose();
            }
          }
                e.Result = @"atm.xml";
        }

        
    }
}
