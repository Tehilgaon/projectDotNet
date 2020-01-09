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
        GuestRequest guestRequest;
        public MainWindow()
        {
            try
            {
                InitializeComponent();
                bL = MyBL.Instance;

                this.GuestZone.tbkEnterMail.Text = "התחבר להצגת בקשות קודמות";
                this.GuestZone.AddButton.Click += AddGuestRequestButton_Click;
                this.GuestZone.dataGrid.MouseDoubleClick += UpdateGuestRequestButton_Click;
                this.GuestZone.dataGrid.SelectionChanged += Guest_selectionChange;

                this.HostZone.tbkEnterMail.Text = "כניסה לאיזור האישי";
                this.HostZone.AddButton.Content = "הוסף יחידה";
                this.HostZone.AddButton.Click += AddHostingUnit_Click;
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
        private void AddGuestRequestButton_Click(object sender, RoutedEventArgs e)
        {
            if (new AddGuestRequest().ShowDialog() == true)
                MessageBox.Show("בקשתך נוספה");

        }

        private void UpdateGuestRequestButton_Click (object sender, RoutedEventArgs e) 
        {
            MessageBox.Show("checking");
            new AddGuestRequest(guestRequest).ShowDialog(); 
                
        }



            private void AddHostingUnit_Click(object sender, RoutedEventArgs e)
             {
            new AddHostingUnit().ShowDialog();
        }



    }
}
