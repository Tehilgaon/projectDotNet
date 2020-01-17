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
    /// Interaction logic for AddHostingUnit.xaml
    /// </summary>
    public partial class AddHostingUnit  
    {
        BL.MyBL bl;
        BE.HostingUnit hostingUnit;
        DateTime CurrentDate = DateTime.UtcNow;
        public AddHostingUnit()
        {
            InitializeComponent();
            bl = MyBL.Instance;
            UnitButtom.Click += AddButton_Click;
            cbxArea.ItemsSource = Enum.GetValues(typeof(BE.Enums.Regions));
            cbxUnitType.ItemsSource = Enum.GetValues(typeof(BE.Enums.HostingUnitType));
            this.cbxBankNum.ItemsSource = from item in bl.GetAllBranches() select item.BankNumber;
            this.cbxBranchNum.ItemsSource = from item in bl.GetAllBranches() select item.BranchNumber;

            hostingUnit = new HostingUnit();
            newOfSameHost();
            DataContext = hostingUnit; 
        }

        /*private void TbxEmail_LostFocus(object sender, RoutedEventArgs e)
        {
            HostingUnit unit = bl.getAllHostingUnits(item => item.Host.MailAddress == (sender as TextBox).Text as string).FirstOrDefault();
            if (unit != null)
            {
                string messegeBody = "?האם אלו פרטיך " + " " + hostingUnit.Host.PrivateName+ " "+hostingUnit.Host.FamilyName;
                MessageBoxResult result = MessageBox.Show(messegeBody, "זיהוי", MessageBoxButton.YesNo,
                                                          MessageBoxImage.Question, MessageBoxResult.No, MessageBoxOptions.RightAlign);
                if (result == MessageBoxResult.Yes)
                {
                    Host host = hostingUnit.Host;
                    tbxPrivateName. = host.PrivateName;
                    tbxFamilyName.Text = host.FamilyName;

                }
            }

        }*/
        void newOfSameHost()
        {
            HostingUnit SameHost = bl.getAllHostingUnits(Item => Item.Host.MailAddress == ((MainWindow)System.Windows.Application.Current.MainWindow).hostMail).FirstOrDefault();
            if (SameHost != null) 
                hostingUnit.Host = SameHost.Host; 
        }

        public AddHostingUnit(HostingUnit unit)
        {
            InitializeComponent();
            bl = MyBL.Instance;
            hostingUnit = unit;
            UnitButtom.Content = "שמור";
            UnitButtom.Click += UpdateButton_Click;

            this.cbxArea.ItemsSource = Enum.GetNames(typeof(BE.Enums.Regions));
            this.cbxUnitType.ItemsSource = Enum.GetNames(typeof(BE.Enums.HostingUnitType));
            this.cbxBankNum.ItemsSource = from item in bl.GetAllBranches() select item.BankNumber;
            this.cbxBranchNum.ItemsSource = from item in bl.GetAllBranches() select item.BranchNumber;
            tbxEmail.IsEnabled = false;
            cbxArea.IsEnabled = false; 
            cbxUnitType.IsEnabled = false;
            tbxEmail.IsEnabled = false;
            cbxBankNum.IsEnabled = false;
            cbxBranchNum.IsEnabled = false;
            tbxaccountNum.IsEnabled = false;
            iDHostKey.IsEnabled = false;

            cbxArea.SelectedItem = hostingUnit.Area;
            DataContext = hostingUnit;

        }


        private void Window_Loaded(object sender, RoutedEventArgs e) { }
         

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.addHostingUnit(hostingUnit);
                DialogResult = true;
                this.Close();
                
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            } 
        }
        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.updateHostingUnit(hostingUnit);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }


    }
}
