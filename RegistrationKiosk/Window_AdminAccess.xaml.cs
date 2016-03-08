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
            BtnClick(sender, e);        //Handles appearance of button on click
            AttemptLogin();
        }

        private void btnQuitCancel_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender, e);        //Handles appearance of button on click
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


        //Handlers for button appearance.
        Style tempStyle;    //Temproary style variable

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

    }
}
