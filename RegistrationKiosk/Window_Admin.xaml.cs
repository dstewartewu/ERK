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
using System.IO;

namespace RegistrationKiosk {

    public partial class Window_Admin : Window
    {
        #region DATA MEMBERS

        //Reference to main project controller
        private Controller controller;

        #endregion

        #region INITIALIZATION

        public Window_Admin()
        {
            InitializeComponent();

            controller = new Controller(this);
            controller.SetView(Controller.WindowView.ADMIN);

            ResetAdminPanel();
        }

        private void ResetAdminPanel()
        {
            txtbxMessages.Text = String.Format("{1}{0}{2}",
                Environment.NewLine,
                "This kiosk is not yet registered.",
                "Click 'Register Kiosk' to begin.");

            btnRegisterKiosk.IsEnabled = true;
            btnRegisterKiosk.Visibility = System.Windows.Visibility.Visible;

            //Disable all other controls; registering the kiosk will re-enable.
            btnOpenKiosk.IsEnabled = false;
        }

        #endregion

        #region EVENT HANDLERS

        private void btnRegisterKiosk_Click(object sender, RoutedEventArgs e)
        {
            controller.SetView(Controller.WindowView.LOAD_EVENT);

            btnOpenKiosk.IsEnabled = true;
            btnOpenKiosk.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnOpenKiosk_Click(object sender, RoutedEventArgs e)
        {
            controller.SetView(Controller.WindowView.KIOSK);
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to quit?", "Quit", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        #endregion
    }
}
