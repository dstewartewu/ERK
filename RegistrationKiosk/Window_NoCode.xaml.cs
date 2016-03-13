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

        //For temporarily storing style values
        Style tempStyle;

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

            txtbxMessages.Text = "This feature is disabled in offline mode.";

            txtbxEmail.IsEnabled = false;
            btnSearch.IsEnabled = false;
        }

        #endregion

        #region EVENT HANDLERS

        #region CONTROLS

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender, e);
            controller.SetView(Controller.WindowView.KIOSK);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender, e);
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

        #region STYLE

        //This is the handler for mouse hover of any button.
        private void BtnMouseHover(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;                                           //Create the button from the passed-in object.
            Style style = this.FindResource("ButtonFocusVisual") as Style;      //Initialize the style from the App.xaml file with the label "ButtonFocusVisual".
            tempStyle = b.Style;                                                //Save the current button state for use when the mouse is no longer hovering over it.
            b.Style = style;                                                    //Apply the new style for the hover effect.
        }

        //This is the handler for when the mouse hover ends on any button.
        private void BtnMouseLeave(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;                                          //Create the button from the passed-in object.
            b.Style = tempStyle;                                                //Use the saved variable from BtnMouseHover to return the button's visual state.
        }

        //This is the handler for click on any button.
        private void BtnClick(object sender, RoutedEventArgs e)
        {
            Button b = (Button)sender;                                          //Create the button from the passed-in object.
            Style style = this.FindResource("ButtonFocusVisual") as Style;      //Initialize the style from the App.xaml file with the label "ButtonFocusVisual".
            b.Style = style;                                                    //Apply the new style for the click effect.
        }

        //This is the handler for the enable/disable change on any button.
        private void EnableBtnChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Button b = (Button)sender;                                          //Create the button from the passed-in object.
            Style style = this.FindResource("ButtonFocusVisual") as Style;      //Initialize the style from the App.xaml file with the label "ButtonFocusVisual".
            b.Style = style;                                                    //Apply the new style for the desired effect.
        }

        #endregion

        #endregion

        public void Connect()
        {
            lblOnline.Content = "ONLINE";

            txtbxMessages.Text = String.Format("{1}{0}{2}",
                Environment.NewLine,
                "Enter your email below and click",
                "'Search' to find your registration.");

            txtbxEmail.IsEnabled = true;
            btnSearch.IsEnabled = true;
        }

        public void Disconnect()
        {
            lblOnline.Content = "OFFLINE";

            txtbxMessages.Text = "This feature is disabled in offline mode.";

            txtbxEmail.IsEnabled = false;
            btnSearch.IsEnabled = false;
        }

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
            catch(System.Net.Http.HttpRequestException http_ex)
            {
                controller.Disconnect();

                txtbxMessages.Text = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Online connection lost. Returning to offline mode.",
                    "Please check your internet connection and try again.");

                controller.LogError("Online connection lost.", http_ex.Message);

                btnSearch.Visibility = System.Windows.Visibility.Visible;
                btnCancel.Visibility = System.Windows.Visibility.Visible;
                btnCancel.IsEnabled = true;

                return;
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
