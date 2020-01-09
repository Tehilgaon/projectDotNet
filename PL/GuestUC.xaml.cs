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
using BL;
using BE;

namespace PL
{
    /// <summary>
    /// Interaction logic for GuestUC.xaml
    /// </summary>
    public partial class GuestUC : UserControl
    {
        private MyBL bL;
        public GuestUC()
        {
            InitializeComponent();
            bL = MyBL.Instance;
        }

         
        private void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            List<GuestRequest> guestRequest =bL.GetAllGuestRequests(Item => Item.MailAddress == tbxEnterMail.Text);
            if (guestRequest.Count == 0)
                MessageBox.Show("לא נמצאו הזמנות");
            else
                dataGrid.ItemsSource = guestRequest;
             


        }
    }
}
