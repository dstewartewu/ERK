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

        #endregion

        #region INITIALIZATION

        private Window_AdminLogin()
        {
            InitializeComponent();
            controller = null;
        }

        public Window_AdminLogin(Controller controller_in)
        {
            InitializeComponent();

            controller = controller_in;
            txtbxMessages.Text = "Enter the registration code for this" + Environment.NewLine + "kiosk to return to the admin panel."; 
        }

        #endregion

        #region EVENT HANDLERS

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            AttemptLogin();
        }

        private void btnQuitCancel_Click(object sender, RoutedEventArgs e)
        {
            controller.SetView(Controller.WindowView.KIOSK);
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
            //PG: PLACEHOLDER FOR AUTHENTICATION
            if (passbxPassword.Password.Length == 0)
            {
                MessageBox.Show("No password entered" + Environment.NewLine + "Development password is any non-empty string.");

                passbxPassword.Focus();
                passbxPassword.SelectAll();
            }
            else
            {
                controller.SetView(Controller.WindowView.START_MENU);

                txtbxMessages.Text = "Enter kiosk code to return to admin panel.";
                passbxPassword.Clear();
            }
            //PG: END PLACEHOLDER
        }
    }
}
