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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Mail;
using System.ComponentModel;
using BL;
using BE;

namespace PL
{
    /// <summary>
    /// Interaction logic for GuestUC.xaml, User Control for all zones
    /// </summary>
    public partial class GuestUC : UserControl
    {
        private MyBL bL;
        public GuestUC()
        {
            InitializeComponent();
            bL = MyBL.Instance;
            tbxEnterMail.PreviewMouseDown += TbxEnterMail_PreviewMouseDown;
            tbxEnterMail.LostFocus += TbxEnterMail_LostFocus;
            tbxSearch.PreviewKeyDown += TbxSearch_PreviewKeyDown;   
        }

        
        private void TbxSearch_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (tbxSearch.Text == "Search")
                tbxSearch.Clear();   
        }

        private void TbxEnterMail_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                MailAddress email = new MailAddress(tbxEnterMail.Text);
            }
            catch(Exception)
            {
                if (tbxEnterMail.Text != ""&&tbxEnterMail.Text!=BE.Configuration.Mng.ToString())
                {
                    tbxEnterMail.Text = "Email is incorrect";
                    tbxEnterMail.Foreground = Brushes.Red;
                }    
            }   
        }

        private void TbxEnterMail_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tbxEnterMail.Text == "Enter your email" || tbxEnterMail.Text == "Email is incorrect")
            {
                tbxEnterMail.Clear();
                tbxEnterMail.Foreground = Brushes.Gray;
            }
        }

  

    }
}
