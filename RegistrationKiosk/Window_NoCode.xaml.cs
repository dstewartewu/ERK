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
                if((controller.ActiveRegistrant = await controller.WebAPI.GetRegistrantByEmail(txtbxEmail.Text)) == null)
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}{0}{4}",
                        Environment.NewLine,
                        "Your registration info was not found.",
                        "Please check your email and try again.",
                        "If the problem persists, start over and",
                        "click 'Register' to continue checking in.");
                }
                else
                {
                    controller.DisplayRegistrant();

                    //Reset prompt
                    txtbxMessages.Text = "Enter your email below.";
                    txtbxEmail.Clear();
                }
            }
            catch (Exception ex)
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                    Environment.NewLine,
                    "An error occurred. Please try again.",
                    "If the problem persists, start over and",
                    "click 'Register' to continue checking in.");

                controller.LogError(ex.Message);
            }

            //Reenable form controls after lookup
            btnSearch.Visibility = System.Windows.Visibility.Visible;
            btnSearch.IsEnabled = true;
            btnCancel.Visibility = System.Windows.Visibility.Visible;
            btnCancel.IsEnabled = true;
        }
    }
}
