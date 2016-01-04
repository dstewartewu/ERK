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

        //Reference to main admin window
        Window_Admin admin;

        //Reference to kiosk window
        Window_Kiosk kiosk;

        /* True if this is the initial login (quit/cancel button quit application)
         * False otherwise (quit/cancel button returns to kiosk screen) */
        Boolean initLogin;

        #endregion

        #region INITIALIZATION

        private Window_AdminLogin()
        {
            InitializeComponent();

            initLogin = true;
        }

        public Window_AdminLogin(Window_Admin _admin, Window_Kiosk _kiosk)
        {
            InitializeComponent();

            admin = _admin;
            kiosk = _kiosk;
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
                Application.Current.Shutdown();

                //PG: DEV: ADD EXIT VERIFICATION WINDOW
            }
            else
            {
                kiosk.IsEnabled = true;
                kiosk.Visibility = System.Windows.Visibility.Visible;

                this.Visibility = System.Windows.Visibility.Hidden;
                this.IsEnabled = false;
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
            //PG: DEV: AUTHENTICATE USERNAME & LOGIN

            //PG: PLACEHOLDER FOR AUTHENTICATION
            if (passbxPassword.Password.Length == 0)
            {
                MessageBox.Show("No password entered" + Environment.NewLine + "Development password is any non-empty string.");

                passbxPassword.Focus();
                passbxPassword.SelectAll();
            }
            else
            {
                admin.IsEnabled = true;
                admin.Visibility = System.Windows.Visibility.Visible;

                this.Visibility = System.Windows.Visibility.Hidden;
                this.IsEnabled = false;

                //Switch function of btnQuitCancel to Cancel
                initLogin = false;
                btnQuitCancel.Content = "Cancel";
            }
            //PG: END PLACEHOLDER
        }
    }
}
