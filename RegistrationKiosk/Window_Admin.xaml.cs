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
            lblMessages.Content = String.Format("{1}{0}{2}",
                Environment.NewLine,
                "No event loaded.",
                "Click 'Load Event' to begin.");

            //Enable 'Load Event' button.
            btnLoadEvent.IsEnabled = true;
            btnLoadEvent.Visibility = System.Windows.Visibility.Visible;

            //Disable all other controls; loading an event will re-enable.
            btnOpenKiosk.IsEnabled = false;
            btnExport.IsEnabled = false;
            btnSearch.IsEnabled = false;
            datagrdSearchResults.IsEnabled = false;
        }

        #endregion

        #region EVENT HANDLERS

        private void btnConnectDB_Click(object sender, RoutedEventArgs e)
        {
            controller.SetView(Controller.WindowView.LOAD_EVENT);

            btnOpenKiosk.IsEnabled = true;
            btnOpenKiosk.Visibility = System.Windows.Visibility.Visible;

            btnExport.IsEnabled = true;
            btnExport.Visibility = System.Windows.Visibility.Visible;
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("DEV: This button does nothing. NOTHING.");
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

        #region OLD CODE
        /*
        private Window_Main main = null;

        //===========================================================================
        #region Window Initialize
        //===========================================================================
        public Window_Admin(Window_Main main) {
            this.main = main;
            InitializeComponent();
            pass_Admin.Focus();
        }
        #endregion
        //===========================================================================
        #region Window Events
        //===========================================================================

        /// <summary>
        /// Click event for Cancel button.
        /// </summary>
        private void btn_AdminCancel_Click(object sender, RoutedEventArgs e) {
            main.IsEnabled = true;
            this.Close();
        }

        /// <summary>
        /// Click event for Okay button.
        /// </summary>
        private void btn_AdminOk_Click(object sender, RoutedEventArgs e) {
            // Check password
            if (main.GetSecurity().CheckAdminPassword(pass_Admin.Password)) {
                main.IsEnabled = true;
                //main.GotoAdminPage(); //PHILLIP: THE 'ADMIN PAGE' HAS BEEN REMOVED FROM THE MAIN KIOSK; IT MAY BE IMPLEMENTED AS A SEPARATE WINDOW INSTEAD. FOR NOW, I'M COMMENTING THIS OUT TO SHUT THE COMPILER UP
                this.Close();
            } else {
                MessageBox.Show("Invalid Password!");
                pass_Admin.Focus();
                pass_Admin.Password = "";
            }
        }

        /// <summary>
        /// KeyDown event for password boxes.
        /// </summary>
        private void pass_Admin_PressEnter(object sender, KeyEventArgs e) {
            if (e.Key == Key.Return) {
                btn_AdminOk_Click(sender, e);
            }
        }

        #endregion
        //===========================================================================
        */
        #endregion
    }
}
