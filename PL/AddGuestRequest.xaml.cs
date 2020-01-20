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
        List<CheckBox> CheckBoxOptions;
        

        public AddGuestRequest()
        {
            InitializeComponent();
            bl = MyBL.Instance;
            cbxArea.ItemsSource = Enum.GetNames(typeof(BE.Enums.Regions));  
            cbxHostingType.ItemsSource = Enum.GetValues(typeof(BE.Enums.HostingUnitType));
            guestRequest = new BE.GuestRequest(); 
            
            newOfSameGuest();
            DataContext = guestRequest; 
        }
        public AddGuestRequest(GuestRequest request)
        {
            if (request != null)
            {
                InitializeComponent();
                bl = MyBL.Instance;
                guestRequest = request;
                this.Title = "עידכון בקשה";
                this.cbxArea.ItemsSource = Enum.GetNames(typeof(BE.Enums.Regions));  
                this.cbxHostingType.ItemsSource = Enum.GetNames(typeof(BE.Enums.HostingUnitType));
                
                tbxPrivateName.IsEnabled = false;
                tbxFamilyName.IsEnabled = false;
                tbxEmail.IsEnabled = false;
                cbxArea.IsEnabled = false; 
                cbxHostingType.IsEnabled = false;
                this.cbxArea.SelectedItem = request.Area; 
                this.cbxHostingType.SelectedValue = request.Type;

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
                //DialogResult = true;
                this.Close();
                MessageBox.Show("פרטיך עודכנו בהצלחה");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

         
        void newOfSameGuest() 
        {
            GuestRequest SameGuest = bl.GetAllGuestRequests(Item => Item.MailAddress == ((MainWindow)System.Windows.Application.Current.MainWindow).guestMail).FirstOrDefault();
            if (SameGuest != null)
            {
                guestRequest.PrivateName = SameGuest.PrivateName;
                guestRequest.FamilyName = SameGuest.FamilyName;
                guestRequest.MailAddress = SameGuest.MailAddress;
            }
        }
        

         

      
    }
}
