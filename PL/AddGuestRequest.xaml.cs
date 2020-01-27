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
    /// Interaction logic for AddGuestRequest.xaml
    /// </summary>
    public partial class AddGuestRequest  
    {
        BL.MyBL bl;
        BE.GuestRequest guestRequest;
        DateTime date=DateTime.Now;
        string aYearFNow=DateTime.Today.AddYears(1).ToShortDateString() ;
        
        

        public AddGuestRequest()
        {
            InitializeComponent();
            bl = MyBL.Instance;
            cbxArea.ItemsSource = Enum.GetNames(typeof(BE.Enums.Regions));  
            cbxHostingType.ItemsSource = Enum.GetValues(typeof(BE.Enums.HostingUnitType));
            guestRequest = new GuestRequest();
            NewRequestOfSameGuest();

            DataContext = guestRequest; 
        }
        public AddGuestRequest(GuestRequest request)
        {
            if (request != null)
            {
                InitializeComponent();
                bl = MyBL.Instance;
                guestRequest = request;
                this.Title = "בקשת לקוח";
                this.cbxArea.ItemsSource = Enum.GetNames(typeof(BE.Enums.Regions));  
                this.cbxHostingType.ItemsSource = Enum.GetNames(typeof(BE.Enums.HostingUnitType)); 
                this.cbxArea.SelectedItem = request.Area; 
                this.cbxHostingType.SelectedValue = request.Type;
                IsRequestStillEnable();
                AddGuestButton.Content = "שמור";
                AddGuestButton.Click += UpdateButton_Click; 
                DataContext = guestRequest;
            }
        }

         

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.addGuestRequest(guestRequest);
                ((MainWindow)System.Windows.Application.Current.MainWindow).GuestFilter(this, new RoutedEventArgs());
                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.updateGuestRequest(guestRequest);
                ((MainWindow)System.Windows.Application.Current.MainWindow).GuestFilter(this, new RoutedEventArgs());
                //DialogResult = true;
                this.Close();
                MessageBox.Show("פרטיך עודכנו בהצלחה");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


         /// <summary>
         /// while inside the private-zone, displying the basic details on the Add-window
         /// </summary>
        void NewRequestOfSameGuest() 
        {
            GuestRequest SameGuest = bl.GetAllGuestRequests(Item => Item.MailAddress == ((MainWindow)System.Windows.
            Application.Current.MainWindow).guestMail).FirstOrDefault();
            if (SameGuest != null)
            {
                guestRequest.PrivateName = SameGuest.PrivateName;
                guestRequest.FamilyName = SameGuest.FamilyName;
                guestRequest.MailAddress = SameGuest.MailAddress;
            }
        }

        /// <summary>
        /// changing is permitted only if an order to this request was not created yet.
        /// </summary>
        void IsRequestStillEnable()
        {
            if (bl.getAllOrders(item => item.GuestRequestKey == guestRequest.GuestRequestKey).Count != 0)
            {
                tbxPrivateName.IsEnabled = false;
                tbxFamilyName.IsEnabled = false;
                tbxEmail.IsEnabled = false;
                cbxArea.IsEnabled = false;
                cbxHostingType.IsEnabled = false;
                EntryDatePicker.IsEnabled = false;
                ReleaseDatePicker.IsEnabled = false;
                tbxAdults.IsEnabled = false;
                tbxChildren.IsEnabled = false;
                cbxpool.IsEnabled = false;
                cbxjacuzzi.IsEnabled = false;
                cbxparking.IsEnabled = false;
                cbxbabybed.IsEnabled = false;
                cbxgarden.IsEnabled = false;
            }
        }
        

         

      
    }
}
