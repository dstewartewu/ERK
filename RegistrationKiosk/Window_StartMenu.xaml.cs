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

    public partial class Window_StartMenu : Window
    {
        #region DATA MEMBERS

        //Reference to main project controller
        private Controller controller;

        private Boolean usingCustomEventname;

        #endregion

        #region INITIALIZATION

        public Window_StartMenu()
        {
            InitializeComponent();

            controller = new Controller(this);

            usingCustomEventname = false;

            btnNameMode.IsEnabled = false;

            txtbxMessages.Text = String.Format("{1}{0}{2}",
                Environment.NewLine,
                "Click 'Register Kiosk' to connect to an event database.",
                "Click 'Open Kiosk' to run the kiosk in offline mode.");
        }

        #endregion

        #region EVENT HANDLERS

        private void btnRegisterKiosk_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender, e);
            controller.SetView(Controller.WindowView.REGISTER_KIOSK);
        }

        private void btnOpenKiosk_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender, e);
            controller.SetView(Controller.WindowView.KIOSK);
        }

        private void btnNameMode_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender, e);
            if (controller.IsOnlineEnabled)
            {
                if (usingCustomEventname)
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}",
                                    Environment.NewLine,
                                    "Event \"" + controller.EventName + "\" loaded.",
                                    "This kiosk displays a generic event name.");

                    controller.UseCustomEventName(false);
                    usingCustomEventname = false;
                    btnNameMode.Content = "Default Name";
                }
                else
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}",
                                    Environment.NewLine,
                                    "Event \"" + controller.EventName + "\" loaded.",
                                    "This kiosk displays the event name.");

                    controller.UseCustomEventName(true);
                    usingCustomEventname = true;
                    btnNameMode.Content = "Custom Name";
                }
            }
            else
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}",
                                    Environment.NewLine,
                                    "Custom event names are disabled in offline mode.",
                                    "This kiosk displays a generic event name.");
            }
        }

        private void btnQuit_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender, e);
            if (MessageBox.Show("Are you sure you want to quit?", "Quit", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        #endregion

        public void Connect()
        {
            lblOnline.Content = "ONLINE";

            /* PG: If StartMenu.Connect() is called outside of Controller.Connect, then
             the following statement may not execute, causing unexpected behavior.
             StartMenu.Connect() should not be called outside this context. ?Any way to enforce that?*/
            if(controller.IsOnlineEnabled)
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Connected to \"" + controller.EventName + "\"",
                    "This kiosk displays the event name.");

                usingCustomEventname = true;
                btnNameMode.Content = "Custom Name";
                btnNameMode.IsEnabled = true;
            }
        }

        public void Disconnect()
        {
            lblOnline.Content = "OFFLINE";

            txtbxMessages.Text = String.Format("{1}{0}{2}",
                Environment.NewLine,
                "Click 'Register Kiosk' to connect to an event database.",
                "Click 'Open Kiosk' to run the kiosk in offline mode.");

            usingCustomEventname = false;
            btnNameMode.Content = "Default Name";
            btnNameMode.IsEnabled = false;
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
            b.Background = Brushes.White;
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
