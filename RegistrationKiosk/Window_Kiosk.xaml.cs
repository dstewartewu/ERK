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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace RegistrationKiosk {

    public partial class Window_Kiosk : Window
    {
        #region CLASS DATA MEMBERS

        //Reference to main project controller
        private Controller controller;

        /* The cmbRegistrantType_SelectionChanged() event calls SetMode().
         * SetMode() can trigger the cmbRegistrantType_SelectionChanged event, causing
         * both methods to call each other in an infinite loop.
         * The boolean variable selectionLocked is used to prevent cmbRegistrantType_SelectionChanged() from
         * firing when SetMode() changes the selected index in cmbRegistrantType. */
        private Boolean selectionLocked;

        #endregion

        #region INITIALIZATION

        private Window_Kiosk()
        {
            InitializeComponent();
        }

        public Window_Kiosk(Controller controller_in)
        {
            InitializeComponent();

            controller = controller_in;
        }

        private void wdwMain_Loaded(object sender, RoutedEventArgs e)
        {
            SetMode(Controller.RegistrantMode.RESET);
        }

        #endregion

        #region EVENT HANDLERS

        private void btnCheckIn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Congrats, you're in.");

            SetMode(Controller.RegistrantMode.RESET);
        }

        private void btnEnterCode_Click(object sender, RoutedEventArgs e)
        {
            LookupRegistrant();
        }

        private void btnNoCode_Click(object sender, RoutedEventArgs e)
        {
            controller.SetView(Controller.WindowView.NO_CODE);
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            SetMode(Controller.RegistrantMode.REGISTER);

            //Display instructions for registration
            txtbxMessages.Text = String.Format("{1}{0}{2}",
                Environment.NewLine,
                "Fill out the form below to continue.",
                "Start by selecting a registrant type.");
        }

        private void btnStartOver_Click(object sender, RoutedEventArgs e)
        {
            SetMode(Controller.RegistrantMode.RESET);
        }

        private void cmbRegistrantType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wdwMain.IsLoaded && !selectionLocked)
            {
                switch (cmbRegistrantType.SelectedIndex)
                {
                    case (int)Controller.RegistrantMode.STUDENT:
                        SetMode(Controller.RegistrantMode.STUDENT);
                        break;
                    case (int)Controller.RegistrantMode.EMPLOYER:
                        SetMode(Controller.RegistrantMode.EMPLOYER);
                        break;
                    case (int)Controller.RegistrantMode.GENERAL:
                        SetMode(Controller.RegistrantMode.GENERAL);
                        break;
                    default:
                        SetMode(Controller.RegistrantMode.REGISTER);
                        break;
                }
            }

            e.Handled = true;
        }

        private void txtbxEnterCode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(Regex.IsMatch(e.Text, "\\D"))
            {
                e.Handled = true;

                txtbxMessages.Text = "Numbers only, please.";
            }

            if(txtbxEnterCode.Text.Length == 6)
            {
                e.Handled = true;

                txtbxMessages.Text = "Registration codes are 6 digits.";
            }
        }

        private void txtbxEnterCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(IsValidRegistrationCode())
            {
                btnEnterCode.IsEnabled = true;
            }
            else
            {
                btnEnterCode.IsEnabled = false;
            }
        }

        private void wdwMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                SetMode(Controller.RegistrantMode.RESET);
                controller.SetView(Controller.WindowView.ADMIN_LOGIN);
                e.Handled = true;
                return;
            }

            if (e.Key == Key.Enter)
            {
                if (txtbxEnterCode.IsFocused)
                {
                    e.Handled = true;

                    LookupRegistrant();
                }
                else
                {
                    TraversalRequest tRequest = new TraversalRequest(FocusNavigationDirection.Next);
                    UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;

                    if (keyboardFocus != null)
                    {
                        keyboardFocus.MoveFocus(tRequest);
                    }

                    e.Handled = true;
                }
            }
        }

        #endregion

        private Boolean IsValidRegistrationCode()
        {
            if (Regex.IsMatch(txtbxEnterCode.Text, "\\D"))
            {
                txtbxMessages.Text = "Registration codes contain numbers only.";

                return false;
            }

            if (txtbxEnterCode.Text.Length == 6)
            {
                txtbxMessages.Text = "Press Enter or click 'Enter Code' to continue.";

                return true;
            }
            else
            {
                return false;
            }
        }

        private void DisplayRegistrant()
        {
            txtbxFirstName.Text = controller.ActiveRegistrant.FirstName;
            txtbxLastName.Text = controller.ActiveRegistrant.LastName;

            switch (controller.ActiveRegistrant.RegistrantType)
            {
                case "Student":
                    SetMode(Controller.RegistrantMode.STUDENT);
                    txtbxSchoolOrganization.Text = controller.ActiveRegistrant.College;
                    txtbxMajorTitle.Text = controller.ActiveRegistrant.Major;

                    switch (controller.ActiveRegistrant.ClassStanding)
                    {
                        case "Freshman":
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.FRESHMAN;
                            break;
                        case "Junior":
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.JUNIOR;
                            break;
                        case "Senior":
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.SENIOR;
                            break;
                        default:
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.SELECT;
                            break;
                    }

                    break;

                default:
                    break;
            }
        }

        private async void LookupRegistrant()
        {
            if (IsValidRegistrationCode())
            {
<<<<<<< HEAD
                txtbxMessages.Text = "Looking up your registration info...";

                try
                {
                    controller.ActiveRegistrant = await controller.WebAPI.GetRegistrantByCode(Int32.Parse(txtbxEnterCode.Text));
=======
                controller.ActiveRegistrant = RegAdapter.GetRegistrant(await controller.WebAPI.GetRegistrantByCode(txtbxEnterCode.Text));
>>>>>>> 1880b6bb4d5b4aa599492812de6d44938963221c

                    DisplayRegistrant();
                }
                catch (Exception ex)
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                        Environment.NewLine,
                        "An error occurred while looking up your registration info.",
                        "Please check your registration code and try again",
                        "If the problem persists, start over and click 'Register' to continue checking in.");
                }
            }
            else
            {
                txtbxMessages.Text += String.Format("{0}{1}{0}{2}",
                        Environment.NewLine,
                        "Please check your registration code and try again.",
                        "If the problem persists, start over and click 'Register' to continue checking in.");
            }
        }

        public void SetMode(Controller.RegistrantMode mode)
        {
            if (mode < Controller.RegistrantMode.RESET || mode > Controller.RegistrantMode.REGISTER)
            {
                return;
            }

            //Reset the form to the intial state
            #region RESET

            if (mode == Controller.RegistrantMode.RESET)
            {
                //Set welcome message and starting instructions
                txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}{0}{4}",
                                                Environment.NewLine,
                                                "Enter your 6-digit code if you pre-registered online.",
                                                "Don't have your code? Click 'No Code' to check in by email.",
                                                "Otherwise, click 'Register' to check in.",
                                                "Employers, click 'Register' to receive a name tag.");

                //Show grdCheckInStart elements
                rctCheckInStart.Visibility = System.Windows.Visibility.Visible;
                txtbxEnterCode.IsEnabled = true;
                txtbxEnterCode.Clear();
                txtbxEnterCode.Visibility = System.Windows.Visibility.Visible;
                txtbxEnterCode.Focus();
                btnEnterCode.IsEnabled = false;
                btnEnterCode.Visibility = System.Windows.Visibility.Visible;
                btnNoCode.IsEnabled = true;
                btnNoCode.Visibility = System.Windows.Visibility.Visible;
                btnRegister.IsEnabled = true;
                btnRegister.Visibility = System.Windows.Visibility.Visible;

                //Hide 'start over' button
                rctStartOver.Visibility = System.Windows.Visibility.Hidden;
                btnStartOver.Visibility = System.Windows.Visibility.Hidden;
                btnStartOver.IsEnabled = false;

                //Hide and reset grdInputFields elements
                rctRegistrantType.Visibility = System.Windows.Visibility.Hidden;
                lblRegistrantType.Visibility = System.Windows.Visibility.Hidden;
                cmbRegistrantType.Visibility = System.Windows.Visibility.Hidden;

                selectionLocked = true;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.RESET;
                selectionLocked = false;

                cmbRegistrantType.IsEnabled = false;
                rctFirstName.Visibility = System.Windows.Visibility.Hidden;
                lblFirstName.Visibility = System.Windows.Visibility.Hidden;
                txtbxFirstName.Visibility = System.Windows.Visibility.Hidden;
                txtbxFirstName.Clear();
                txtbxFirstName.IsEnabled = false;
                rctLastName.Visibility = System.Windows.Visibility.Hidden;
                lblLastName.Visibility = System.Windows.Visibility.Hidden;
                txtbxLastName.Visibility = System.Windows.Visibility.Hidden;
                txtbxLastName.Clear();
                txtbxLastName.IsEnabled = false;
                rctSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                lblSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                lblSchoolOrganization.Content = "School";
                txtbxSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrganization.Clear();
                txtbxSchoolOrganization.IsEnabled = false;
                rctMajorTitle.Visibility = System.Windows.Visibility.Hidden;
                lblMajorTitle.Visibility = System.Windows.Visibility.Hidden;
                lblMajorTitle.Content = "Major";
                txtbxMajorTitle.Visibility = System.Windows.Visibility.Hidden;
                txtbxMajorTitle.Clear();
                txtbxMajorTitle.IsEnabled = false;
                rctClassStanding.Visibility = System.Windows.Visibility.Hidden;
                lblClassStanding.Visibility = System.Windows.Visibility.Hidden;
                cmbClassStanding.Visibility = System.Windows.Visibility.Hidden;
                cmbClassStanding.SelectedIndex = (int)Controller.RegistrantMode.RESET;
                cmbClassStanding.IsEnabled = false;

                //Hide 'Check In' button
                rctCheckInFinish.Visibility = System.Windows.Visibility.Hidden;
                btnCheckIn.Visibility = System.Windows.Visibility.Hidden;
                btnCheckIn.IsEnabled = false;

                return;
            }
            #endregion

            //If not RESET, prepare kiosk for check-in
            #region PREPARE FOR CHECK-IN

            //Show 'Start Over' button
            rctStartOver.Visibility = System.Windows.Visibility.Visible;
            btnStartOver.IsEnabled = true;
            btnStartOver.Visibility = System.Windows.Visibility.Visible;

            //Show 'Check In' button
            rctCheckInFinish.Visibility = System.Windows.Visibility.Visible;
            btnCheckIn.IsEnabled = true;
            btnCheckIn.Visibility = System.Windows.Visibility.Visible;
            btnCheckIn.IsEnabled = false;

            //Hide grdCheckInStart elements
            txtbxEnterCode.Visibility = System.Windows.Visibility.Collapsed;
            txtbxEnterCode.Clear();
            txtbxEnterCode.IsEnabled = false;
            btnEnterCode.Visibility = System.Windows.Visibility.Collapsed;
            btnEnterCode.IsEnabled = false;
            btnNoCode.Visibility = System.Windows.Visibility.Collapsed;
            btnNoCode.IsEnabled = false;
            btnRegister.Visibility = System.Windows.Visibility.Collapsed;
            btnRegister.IsEnabled = false;

            //Show registration type selector
            rctRegistrantType.Visibility = System.Windows.Visibility.Visible;
            lblRegistrantType.Visibility = System.Windows.Visibility.Visible;
            cmbRegistrantType.IsEnabled = true;
            cmbRegistrantType.Visibility = System.Windows.Visibility.Visible;

            #endregion

            //Registrant is a student
            #region STUDENT

            if (mode == Controller.RegistrantMode.STUDENT)
            {
                //Set grdInputFields for STUDENT parameters
                //Watch for bugs! See declaration of selectionLocked for cautionary info
                selectionLocked = true;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.STUDENT;
                selectionLocked = false;

                rctFirstName.Visibility = System.Windows.Visibility.Visible;
                lblFirstName.Visibility = System.Windows.Visibility.Visible;
                txtbxFirstName.Visibility = System.Windows.Visibility.Visible;
                txtbxFirstName.IsEnabled = true;
                rctLastName.Visibility = System.Windows.Visibility.Visible;
                lblLastName.Visibility = System.Windows.Visibility.Visible;
                txtbxLastName.Visibility = System.Windows.Visibility.Visible;
                txtbxLastName.IsEnabled = true;

                rctSchoolOrganization.Visibility = System.Windows.Visibility.Visible;
                lblSchoolOrganization.Content = "School";
                lblSchoolOrganization.Visibility = System.Windows.Visibility.Visible;
                txtbxSchoolOrganization.IsEnabled = true;
                txtbxSchoolOrganization.Visibility = System.Windows.Visibility.Visible;

                rctMajorTitle.Visibility = System.Windows.Visibility.Visible;
                lblMajorTitle.Content = "Major";
                lblMajorTitle.Visibility = System.Windows.Visibility.Visible;
                txtbxMajorTitle.IsEnabled = true;
                txtbxMajorTitle.Visibility = System.Windows.Visibility.Visible;

                rctClassStanding.Visibility = System.Windows.Visibility.Visible;
                lblClassStanding.Visibility = System.Windows.Visibility.Visible;
                cmbClassStanding.IsEnabled = true;
                cmbClassStanding.Visibility = System.Windows.Visibility.Visible;

                return;
            }

            #endregion //STUDENT

            //Registrant is an employer or employee
            #region EMPLOYER

            if (mode == Controller.RegistrantMode.EMPLOYER)
            {
                //Set grdInputFields for EMPLOYER parameters
                //Watch for bugs! See declaration of selectionLocked for cautionary info
                selectionLocked = true;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.EMPLOYER;
                selectionLocked = false;

                rctFirstName.Visibility = System.Windows.Visibility.Visible;
                lblFirstName.Visibility = System.Windows.Visibility.Visible;
                txtbxFirstName.Visibility = System.Windows.Visibility.Visible;
                txtbxFirstName.IsEnabled = true;
                rctLastName.Visibility = System.Windows.Visibility.Visible;
                lblLastName.Visibility = System.Windows.Visibility.Visible;
                txtbxLastName.Visibility = System.Windows.Visibility.Visible;
                txtbxLastName.IsEnabled = true;

                rctSchoolOrganization.Visibility = System.Windows.Visibility.Visible;
                lblSchoolOrganization.Content = "Organization";
                lblSchoolOrganization.Visibility = System.Windows.Visibility.Visible;
                txtbxSchoolOrganization.IsEnabled = true;
                txtbxSchoolOrganization.Visibility = System.Windows.Visibility.Visible;

                rctMajorTitle.Visibility = System.Windows.Visibility.Visible;
                lblMajorTitle.Content = "Job Title";
                lblMajorTitle.Visibility = System.Windows.Visibility.Visible;
                txtbxMajorTitle.IsEnabled = true;
                txtbxMajorTitle.Visibility = System.Windows.Visibility.Visible;

                rctClassStanding.Visibility = System.Windows.Visibility.Hidden;
                lblClassStanding.Visibility = System.Windows.Visibility.Hidden;
                cmbClassStanding.Visibility = System.Windows.Visibility.Hidden;
                cmbClassStanding.IsEnabled = false;

                return;
            }

            #endregion //EMPLOYER

            //Registrant is general
            #region GENERAL

            if (mode == Controller.RegistrantMode.GENERAL)
            {
                //Set grdInputFields for GENERAL parameters
                //Watch for bugs! See declaration of selectionLocked for cautionary info
                selectionLocked = true;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.GENERAL;
                selectionLocked = false;

                rctFirstName.Visibility = System.Windows.Visibility.Visible;
                lblFirstName.Visibility = System.Windows.Visibility.Visible;
                txtbxFirstName.Visibility = System.Windows.Visibility.Visible;
                txtbxFirstName.IsEnabled = true;
                rctLastName.Visibility = System.Windows.Visibility.Visible;
                lblLastName.Visibility = System.Windows.Visibility.Visible;
                txtbxLastName.Visibility = System.Windows.Visibility.Visible;
                txtbxLastName.IsEnabled = true;

                rctSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                lblSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrganization.IsEnabled = false;

                rctMajorTitle.Visibility = System.Windows.Visibility.Hidden;
                lblMajorTitle.Visibility = System.Windows.Visibility.Hidden;
                txtbxMajorTitle.Visibility = System.Windows.Visibility.Hidden;
                txtbxMajorTitle.IsEnabled = false;

                rctClassStanding.Visibility = System.Windows.Visibility.Hidden;
                lblClassStanding.Visibility = System.Windows.Visibility.Hidden;
                cmbClassStanding.Visibility = System.Windows.Visibility.Hidden;
                cmbClassStanding.IsEnabled = false;

                return;
            }

            #endregion //GENERAL

            //User is new registrant
            #region REGISTER

            if (mode == Controller.RegistrantMode.REGISTER)
            {
                //Set REGISTER mode
                //Watch for bugs! See declaration of selectionLocked for cautionary info
                selectionLocked = true;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.REGISTER;
                selectionLocked = false;

                //Hide all other grdInputFields elements until registrant type is selected
                rctFirstName.Visibility = System.Windows.Visibility.Hidden;
                lblFirstName.Visibility = System.Windows.Visibility.Hidden;
                txtbxFirstName.IsEnabled = false;
                txtbxFirstName.Visibility = System.Windows.Visibility.Hidden;

                rctLastName.Visibility = System.Windows.Visibility.Hidden;
                lblLastName.Visibility = System.Windows.Visibility.Hidden;
                txtbxLastName.IsEnabled = false;
                txtbxLastName.Visibility = System.Windows.Visibility.Hidden;

                rctSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                lblSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrganization.IsEnabled = false;

                rctMajorTitle.Visibility = System.Windows.Visibility.Hidden;
                lblMajorTitle.Visibility = System.Windows.Visibility.Hidden;
                txtbxMajorTitle.Visibility = System.Windows.Visibility.Hidden;
                txtbxMajorTitle.IsEnabled = false;

                rctClassStanding.Visibility = System.Windows.Visibility.Hidden;
                lblClassStanding.Visibility = System.Windows.Visibility.Hidden;
                cmbClassStanding.IsEnabled = false;
                cmbClassStanding.Visibility = System.Windows.Visibility.Hidden;

                return;
            }

            #endregion //REGISTER
        }

        
    }
}