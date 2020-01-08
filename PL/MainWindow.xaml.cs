﻿using BL;
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
             
            this.GuestRequestZone.AddButton.Click+= AddGuestRequestButton_Click;

        }

        private void AddGuestRequestButton_Click(object sender, RoutedEventArgs e)
        {
            new AddGuestRequest().ShowDialog();
        }



    }
}
