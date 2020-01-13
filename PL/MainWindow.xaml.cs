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

        

        public MainWindow()
        {
            try
            {
                InitializeComponent();
                bL = MyBL.Instance;

                this.GuestZone.tbkEnterMail.Text = "התחבר כאורח";
                this.GuestZone.AddButton.Click += GuestAddButton_Click;
                this.GuestZone.dataGrid.MouseDoubleClick += GuestUpdateButton_Click;
                this.GuestZone.dataGrid.SelectionChanged += Guest_selectionChange;
                this.GuestZone.dataGrid.AutoGeneratingColumn += Guest_AutoGenerateColumns;   
                this.GuestZone.LogInButton.Click += GuestLogInButton_Click;



                this.HostZone.tbkEnterMail.Text = "התחבר כבעל יחידת אירוח";
                this.HostZone.AddButton.Content = "הוסף יחידה";
                this.HostZone.deleteButton.Visibility = Visibility.Visible;
                this.HostZone.deleteButton.Click += DeleteButton_Click;
                this.HostZone.AddButton.Click +=  HostingUnitAdd_Click;
                this.HostZone.LogInButton.Click += HostLogInButton_Click;
               

                this.ManagerZone.tbkEnterMail.Text = ""; 
                this.ManagerZone.AddButton.Visibility = Visibility.Collapsed;
                this.ManagerZone.LogInButton.Click += LogInButton_Click;
                this.ManagerZone.cbxfilter.SelectionChanged += Cbxfilter_SelectionChanged;
                
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }


        }

        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
             if(ManagerZone.tbxEnterMail.Text==Configuration.Password.ToString())
                this.ManagerZone.cbxfilter.Visibility = Visibility.Visible;
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("האם אתה בטוח שברצונך למחוק את היחידה",
            "מחיקת יחידה", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {

            }
            else
            {

            }   
        }

        private void Guest_AutoGenerateColumns(object sender, DataGridAutoGeneratingColumnEventArgs e)
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
             case "SubArea":
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
            case "RegistrationDate":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
            case "GuestRequestKey":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;
            case "Status":
                    e.Column.Visibility = Visibility.Collapsed;
                    break;

            }
        }

        private void Guest_selectionChange(object sender, SelectionChangedEventArgs e)
        {
            guestRequest = (sender as DataGrid).SelectedItem as GuestRequest;
          
        }
        private void GuestAddButton_Click(object sender, RoutedEventArgs e)
        {
            if (new AddGuestRequest().ShowDialog() == true)
            {
                MessageBox.Show("בקשתך נוספה");
               // GuestZone.dataGrid.Items.Add(this.)
            }


        } 
        private void GuestUpdateButton_Click(object sender, RoutedEventArgs e) 
        {
            new AddGuestRequest(guestRequest).ShowDialog();    
        }
        private void GuestLogInButton_Click(object sender, RoutedEventArgs e)
        {
            guestRequestsList = bL.GetAllGuestRequests(Item => Item.MailAddress == GuestZone.tbxEnterMail.Text);
            if (guestRequestsList.Count == 0)
                MessageBox.Show("לא נמצאו הזמנות");
            else
            {
                GuestZone.dataGrid.ItemsSource = guestRequestsList;
                GuestZone.SpLogin.Visibility = Visibility.Collapsed;
            }

        }



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
            hostingUnitsList = bL.getAllHostingUnits(Item => Item.Host.MailAddress == HostZone.tbxEnterMail.Text);
            if (hostingUnitsList.Count == 0)
                MessageBox.Show("לא נמצאו יחידות אירוח");
            else
            {
                HostZone.dataGrid.Visibility = Visibility.Collapsed;
                HostZone.UnitsGrid.Visibility = Visibility.Visible;
                for (int i = 0; i < hostingUnitsList.Count; i++)
                {
                    hostingUnitUC hostingUnit = new hostingUnitUC(hostingUnitsList[i]);
                    HostZone.UnitsGrid.Children.Add(hostingUnit);
                    Grid.SetRow(hostingUnit, i);
 
                }
            } 
        }

        private void Cbxfilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ComboBoxItem)ManagerZone.cbxfilter.SelectedItem).Content.ToString())
            {
                case "דרישות לקוח":
                    guestRequestsList = bL.GetAllGuestRequests();
                    ManagerZone.dataGrid.ItemsSource = guestRequestsList;
                    break;
                case "יחידות אירוח":
                    hostingUnitsList = bL.getAllHostingUnits();
                    ManagerZone.dataGrid.ItemsSource = hostingUnitsList;
                    break;
                case "הזמנות":
                    ordersList = bL.getAllOrders();
                    ManagerZone.dataGrid.ItemsSource = ordersList;
                    break;
            }
        }


    }
}
