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
                this.GuestZone.LogInButton.Click += GuestLogInButton_Click;



                this.HostZone.tbkEnterMail.Text = "התחבר כבעל יחידת אירוח";
                this.HostZone.AddButton.Content = "הוסף יחידה";
                this.HostZone.AddButton.Click +=  HostingUnitAdd_Click;
                this.HostZone.LogInButton.Click += HostLogInButton_Click;

                this.ManagerZone.tbkEnterMail.Text = "";
                this.ManagerZone.cbxfilter.Visibility = Visibility.Visible;
                //this.ManagerZone.cbxfilter.
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
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
                GuestZone.dataGrid.ItemsSource = guestRequestsList; 
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

    }
}
