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
    /// Interaction logic for Window_NoCode.xaml
    /// </summary>
    public partial class Window_NoCode : Window
    {
        #region DATA MEMBERS

        //Reference to main project controller
        private Controller controller;

        #endregion

        #region INITIALIZATION

        private Window_NoCode()
        {
            InitializeComponent();
        }

        public Window_NoCode(Controller controller_in)
        {
            InitializeComponent();

            controller = controller_in;

            txtbxMessages.Text = "Enter your email below.";
        }

        #endregion

        #region EVENT HANDLERS

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            controller.SetView(Controller.WindowView.KIOSK);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            LookupRegistrantByEmail();
        }

        private void wdwNoCode_KeyDown(object sender, KeyEventArgs e)
        {
            //DEV: Some kind of input verification might be useful here
            if (txtbxEmail.Text.Length > 0)
            {
                if (e.Key == Key.Enter)
                {
                    LookupRegistrantByEmail();

                    e.Handled = true;
                }
            }
        }

        #endregion

        private async void LookupRegistrantByEmail()
        {
            txtbxMessages.Text = "Looking up your registration info...";

            //Disable form controls during lookup
            btnSearch.Visibility = System.Windows.Visibility.Collapsed;
            btnSearch.IsEnabled = false;
            btnCancel.Visibility = System.Windows.Visibility.Collapsed;
            btnCancel.IsEnabled = false;

            try
            {
                controller.ActiveRegistrant = await controller.WebAPI.GetRegistrantByEmail(txtbxEmail.Text);

                controller.DisplayRegistrant();

                txtbxEmail.Clear();
                txtbxMessages.Text = "Enter your email below.";
            }
            catch (Exception ex)
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                    Environment.NewLine,
                    "An error occurred while looking up your registration info.",
                    "Please check your email and try again",
                    "If the problem persists, start over and click 'Register' to continue checking in.");
            }

            //Reenable form controls after lookup
            btnSearch.Visibility = System.Windows.Visibility.Visible;
            btnSearch.IsEnabled = true;
            btnCancel.Visibility = System.Windows.Visibility.Visible;
            btnCancel.IsEnabled = true;
        }
    }
}
