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
            hostingUnit = new HostingUnit();
            newOfSameHost();
            DataContext = hostingUnit;
        }


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
            tbxEmail.IsEnabled = false;
            cbxArea.IsEnabled = false;
            cbxUnitType.IsEnabled = false;
            tbxEmail.IsEnabled = false;
            tbxBankNum.IsEnabled = false;
            tbxBranchNum.IsEnabled = false;
            tbxaccountNum.IsEnabled = false;
            iDHostKey.IsEnabled = false;

            cbxArea.SelectedItem = hostingUnit.Area;
            DataContext = hostingUnit;

        }
         

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (ValidBankDetails())
                { 
                    bl.addHostingUnit(hostingUnit);
                    DialogResult = true;
                    this.Close();
                }
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
        public bool ValidBankDetails()
        {
            BankBranch branch = ((MainWindow)System.Windows.Application.Current.MainWindow).branchesList.Where
                (item => item.BankNumber == hostingUnit.Host.Bankbranch.BankNumber &&
                item.BranchNumber == hostingUnit.Host.Bankbranch.BranchNumber).FirstOrDefault();
            if (branch != null)
            {
                hostingUnit.Host.Bankbranch.BankName = branch.BankName;
                hostingUnit.Host.Bankbranch.BranchAddress = branch.BranchAddress;
                hostingUnit.Host.Bankbranch.BranchCity = branch.BranchCity;  
                return true; 
            }
            tbxBankNum.Clear();
            tbxBranchNum.Clear();
            throw new Exception(" פרטי בנק שגוים");   
        }


    }
}
