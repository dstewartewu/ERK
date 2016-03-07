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
            controller.SetView(Controller.WindowView.REGISTER_KIOSK);
        }

        private void btnOpenKiosk_Click(object sender, RoutedEventArgs e)
        {
            controller.SetView(Controller.WindowView.KIOSK);
        }

        private void btnNameMode_Click(object sender, RoutedEventArgs e)
        {
            if(controller.IsOnlineEnabled)
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
    }
}
