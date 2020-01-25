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
using System.ComponentModel;
using BL;
using BE;


namespace PL
{
    /// <summary>
    /// Interaction logic for Email.xaml
    /// </summary>
    public partial class Email 
    {
        private MyBL bL;
        public Order currentOrder;
        HostingUnit hostingUnit;
        public Email(Order order)
        {  
            InitializeComponent();
            bL = MyBL.Instance;
            sendButton.Click += SendButton_Click;
            currentOrder = order;
            hostingUnit = bL.getAllHostingUnits(item => item.HostingUnitKey == currentOrder.HostingUnitKey).FirstOrDefault();  
            DataContext = hostingUnit;
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            progressBar.IsIndeterminate = true;
            BackgroundWorker MailWorker = new BackgroundWorker();
            MailWorker.DoWork += (se, args) =>
            { 
                bL.updateOrder(currentOrder); 
            };
            MailWorker.RunWorkerCompleted += (se, args) =>
            {
                DialogResult = true;
                this.Close();
                MessageBox.Show("המייל נשלח בהצלחה");
                
            };
            MailWorker.RunWorkerAsync();
        }
    }
}
