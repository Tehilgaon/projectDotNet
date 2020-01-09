﻿using System;
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
    public partial class AddGuestRequest : Window
    {
        BL.MyBL bl;
        BE.GuestRequest guestRequest;
        public AddGuestRequest(GuestRequest request=null)
        {
            InitializeComponent();   
            bl = MyBL.Instance; 
            if (request!=null)
            {
                privateNameTextBox.IsEnabled = false;
                familyNameTextBox.IsEnabled = false;
                EmailTextBox.IsEnabled = false;
                AreaComboBox.IsEnabled = false;
                subAreaComboBox.IsEnabled = false;
                HostingTypeComboBox.IsEnabled = false;
            } 
            else
            {
                AreaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.Regions));
                subAreaComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.SubArea));
                HostingTypeComboBox.ItemsSource = Enum.GetValues(typeof(BE.Enums.HostingUnitType));
                guestRequest = new BE.GuestRequest();
            }
           
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
    }
}