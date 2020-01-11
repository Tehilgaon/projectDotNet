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
    public partial class AddHostingUnit : Window
    {
        BL.MyBL bl;
        BE.HostingUnit hostingUnit;
        public AddHostingUnit()
        {
            InitializeComponent();
            bl = MyBL.Instance;
            UnitButtom.Click += AddButton_Click;
            cbxArea.ItemsSource = Enum.GetValues(typeof(BE.Enums.Regions));
            cbxSubArea.ItemsSource= Enum.GetValues(typeof(BE.Enums.SubArea));
            cbxUnitType.ItemsSource = Enum.GetValues(typeof(BE.Enums.HostingUnitType));
            hostingUnit = new HostingUnit();
            DataContext = hostingUnit; 
        }
        public AddHostingUnit(HostingUnit unit)
        {
            InitializeComponent();
            bl = MyBL.Instance;
            hostingUnit = unit;
            UnitButtom.Content = "שמור";
            UnitButtom.Click += UpdateButton_Click;
             
            tbxEmail.IsEnabled = false;
            cbxArea.IsEnabled = false;
            cbxSubArea.IsEnabled = false;
            cbxUnitType.IsEnabled = false;
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
