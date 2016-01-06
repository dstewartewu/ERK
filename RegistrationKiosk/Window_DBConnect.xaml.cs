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

namespace RegistrationKiosk
{
    public partial class Window_DBConnect : Window
    {
        #region DATA MEMBERS

        //Reference to admin window
        private Window_Admin admin;

        #endregion

        #region INITIALIZATION

        private Window_DBConnect()
        {
            InitializeComponent();
        }

        public Window_DBConnect(Window_Admin _admin)
        {
            InitializeComponent();

            admin = _admin;
        }

        #endregion

        #region EVENT HANDLERS

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This button is just for show ATM.");
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            admin.IsEnabled = true;
            admin.Visibility = System.Windows.Visibility.Visible;

            this.Visibility = System.Windows.Visibility.Hidden;
            this.IsEnabled = false;
        }

        #endregion
    }
}
