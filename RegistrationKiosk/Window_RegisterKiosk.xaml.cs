﻿using System;
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
    public partial class Window_RegisterKiosk : Window
    {
        #region DATA MEMBERS

        //Reference to main project controller
        private Controller controller;

        #endregion

        #region INITIALIZATION

        private Window_RegisterKiosk()
        {
            InitializeComponent();
        }

        public Window_RegisterKiosk(Controller controller_in)
        {
            InitializeComponent();

            controller = controller_in;

            txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                Environment.NewLine,
                "Kiosk is in offline mode.",
                "To connect to an event database, enter",
                "the kiosk registration code below.");
        }

        #endregion

        #region EVENT HANDLERS

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            controller.SetView(Controller.WindowView.START_MENU);
        }

        private void btnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            controller.Disconnect();
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterKiosk();
        }

        private void wdwRegisterKiosk_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && btnRegister.IsFocused)
            {
                RegisterKiosk();

                e.Handled = true;
            }
        }

        #endregion

        public void Connect()
        {
            lblOnline.Content = "ONLINE";

            if(controller.IsOnlineEnabled)
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Connected to \"" + controller.EventName + "\"",
                    "Click 'Disconnect' to return to offline mode.");
            }

            txtbxKioskCode.Clear();
        }

        public void Disconnect()
        {
            lblOnline.Content = "OFFLINE";

            txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                Environment.NewLine,
                "Kiosk is in offline mode.",
                "To connect to an event database, enter",
                "the kiosk registration code below.");
        }

        //PG: DEV: Run some kind of input validation here
        private void RegisterKiosk()
        {
            controller.Connect(txtbxKioskCode.Text);
        }

        public void SetMessage(String message)
        {
            txtbxMessages.Text = message;
        }

    }
}