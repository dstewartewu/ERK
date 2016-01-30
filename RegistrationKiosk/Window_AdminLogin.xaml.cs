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
    /// <summary>
    /// Interaction logic for AdminLogin.xaml
    /// </summary>
    public partial class Window_AdminLogin : Window
    {
        #region DATA MEMBERS

        //Reference to main project controller
        private Controller controller;

        /* True if this is the initial login (quit/cancel button quits application)
         * False otherwise (quit/cancel button returns to kiosk screen) */
        Boolean initLogin;

        #endregion

        #region INITIALIZATION

        private Window_AdminLogin()
        {
            InitializeComponent();
            controller = null;
            initLogin = true;
        }

        public Window_AdminLogin(Controller controller_in)
        {
            InitializeComponent();

            controller = controller_in;

            initLogin = true;

            lblMessages.Content = "Welcome!" + Environment.NewLine + "Enter the password to begin."; 
        }

        #endregion

        #region EVENT HANDLERS

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            AttemptLogin();
        }

        private void btnQuitCancel_Click(object sender, RoutedEventArgs e)
        {
            if(initLogin)
            {
                if (MessageBox.Show("Are you sure you want to quit?", "Quit", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    Application.Current.Shutdown();
                }
            }
            else
            {
                controller.SetView(Controller.WindowView.KIOSK);
            }
        }

        private void wdwAdminLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                AttemptLogin();
            }
        }

        #endregion

        private void AttemptLogin()
        {
            //PG: DEV: AUTHENTICATE PASSWORD
            /* PG: DEV: if(controller.AttemptLogin()) //If login is successful (AttemptLogin() must return Boolean)
            */

            //PG: PLACEHOLDER FOR AUTHENTICATION
            if (passbxPassword.Password.Length == 0)
            {
                MessageBox.Show("No password entered" + Environment.NewLine + "Development password is any non-empty string.");

                passbxPassword.Focus();
                passbxPassword.SelectAll();
            }
            else
            {
                controller.SetView(Controller.WindowView.ADMIN);

                //Switch function of btnQuitCancel to Cancel
                initLogin = false;
                btnQuitCancel.Content = "Cancel";
                lblMessages.Content = "Enter password to" + Environment.NewLine + "return to admin panel.";
                passbxPassword.Clear();
            }
            //PG: END PLACEHOLDER
        }
    }
}
