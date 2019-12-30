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
    public partial class MainWindow : Window
    {
        private MyBL bL;
        public MainWindow()
        {
            InitializeComponent();
            bL = MyBL.Instance;
            /*bL.addHostingUnit(new HostingUnit()
            {
                HostingUnitName = "myMama",
                HostingUnitType = Enums.HostingUnitType.וילה,
                Area = Enums.Regions.North,
                Host = new Host()
                {
                    CollectionClearance = true,
                    PrivatrName = "maya",
                    BankAccountNumber = "123456",
                    MailAddress = "maya@gmail.com",
                },

            });*/
            HostingUnit hu = new HostingUnit();


            hu.HostingUnitName = "ThePlace";
            hu.HostingUnitType = Enums.HostingUnitType.צימר;
            hu.Area = Enums.Regions.Jerusalem;
            hu.Host = new Host()
            {
                CollectionClearance = true,
                PrivatrName = "dani",
                BankAccountNumber = "654321",
                MailAddress = "dani@gmail.com",
            };

            
            bL.addHostingUnit(hu);
            hu.HostingUnitName = "great";
            bL.updateHostingUnit(hu);
            lb_HostingUnits.DataContext = bL.getAllHostingUnits();

 
            

            }
    }
}
