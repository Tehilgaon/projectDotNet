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

        public AddGuestRequest()
        {
            InitializeComponent();
            bl = MyBL.Instance;
            cbxArea.ItemsSource = Enum.GetNames(typeof(BE.Enums.Regions));
            //AreaComboBox.SelectedValue = Enums.Regions.North.ToString();
            cbxsubArea.ItemsSource = Enum.GetValues(typeof(BE.Enums.SubArea));
            cbxHostingType.ItemsSource = Enum.GetValues(typeof(BE.Enums.HostingUnitType));
            guestRequest = new BE.GuestRequest();
            DataContext = guestRequest;
          

        }
        public AddGuestRequest(GuestRequest request)
        {
            InitializeComponent();
            bl = MyBL.Instance;  
            guestRequest = request;
            tbxPrivateName.IsEnabled = false;
            tbxFamilyName.IsEnabled = false;
            tbxEmail.IsEnabled = false;
            cbxArea.IsEnabled = false;
            cbxsubArea.IsEnabled = false;
            cbxHostingType.IsEnabled = false;

            cbxArea.ItemsSource = Enum.GetValues(typeof(BE.Enums.Regions));
            cbxsubArea.ItemsSource = Enum.GetValues(typeof(BE.Enums.SubArea));
            cbxHostingType.ItemsSource = Enum.GetValues(typeof(BE.Enums.HostingUnitType));

            cbxArea.SelectedItem = request.Area;
            cbxsubArea.SelectedValue = request.SubArea;
            cbxHostingType.SelectedValue = request.Type;

            AddGuestButton.Content = "שמור";
            AddGuestButton.Click += UpdateButton_Click; 
            DataContext = guestRequest;
        }

         

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.addGuestRequest(guestRequest);
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

      
    }
}
