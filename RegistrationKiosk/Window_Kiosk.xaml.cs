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
            CheckIn();
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
                    case (int)Controller.RegistrantMode.EMPLOYEE:
                        SetMode(Controller.RegistrantMode.EMPLOYEE);
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
            if (txtbxEnterCode.Text.Length >= 6)
            {
                txtbxMessages.Text = "Registration codes are 6 digits only.";

                e.Handled = true;

                return;
            }

            if(Regex.IsMatch(e.Text, "\\D"))
            {
                e.Handled = true;

                txtbxMessages.Text = "Numbers only, please.";
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

        private void txtbxFirstName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(txtbxFirstName.Text.Length >= 64)
            {
                txtbxMessages.Text = "First and last names are limited to 64 characters.";

                e.Handled = true;

                return;
            }

            if(!Regex.IsMatch(e.Text, "[-'\\p{L} ]+"))
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Only letters, hyphens, apostrophes, and spaces",
                    "are permitted in first and last names.");

                e.Handled = true;

                return;
            }

            if(txtbxFirstName.Text.Length >= 16)
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                    Environment.NewLine,
                    "First or last names longer than 16 letters tend to display poorly on name tags.",
                    "If possible, you may want to consider using a shorter name.",
                    "Sorry for the inconvenience!");
            }
        }

        private void txtbxLastName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (txtbxLastName.Text.Length >= 64)
            {
                txtbxMessages.Text = "First and last names are limited to 64 characters.";

                e.Handled = true;

                return;
            }

            if (!Regex.IsMatch(e.Text, "[-'\\p{L} ]+"))
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Only letters, hyphens, apostrophes, and spaces",
                    "are permitted in first and last names.");

                e.Handled = true;

                return;
            }

            if (txtbxLastName.Text.Length >= 16)
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                    Environment.NewLine,
                    "First or last names longer than 16 letters tend to display poorly on name tags.",
                    "If possible, you may want to consider using a shorter name.",
                    "Sorry for the inconvenience!");
            }
        }

        private void txtbxMajorOrPosition_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
            {
                if (txtbxMajorOrPosition.Text.Length >= 64)
                {
                    txtbxMessages.Text = "Majors are limited to 64 characters.";

                    e.Handled = true;

                    return;
                }

                if (!Regex.IsMatch(e.Text, "[-\\p{L} ]+"))
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}",
                        Environment.NewLine,
                        "Only letters, hyphens, and spaces",
                        "are permitted in major names.");

                    e.Handled = true;

                    return;
                }

                if (txtbxMajorOrPosition.Text.Length >= 16)
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                        Environment.NewLine,
                        "Majors names longer than 16 letters tend to display poorly on name tags.",
                        "Consider shortening or abbreviating the name of your major.",
                        "Sorry for the inconvenience!");
                }
            }
            else if (cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.EMPLOYEE)
            {
                if (txtbxMajorOrPosition.Text.Length >= 64)
                {
                    txtbxMessages.Text = "Job titles are limited to 64 characters.";

                    e.Handled = true;

                    return;
                }

                if (Regex.IsMatch(e.Text, "\\b[-\\p{L} ]+\\b"))
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}",
                        Environment.NewLine,
                        "Only letters, hyphens, and spaces",
                        "are supported in job titles.");

                    e.Handled = true;

                    return;
                }

                if (txtbxMajorOrPosition.Text.Length >= 16)
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                        "Job titles longer than 16 letters tend to display poorly on name tags.",
                        "If possible, consider shortening or abbreviating your job title.",
                        "Sorry for the inconvenience!");
                }
            }
        }

        private void txtbxSchoolOrOrganization_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
            {
                if (txtbxSchoolOrOrganization.Text.Length >= 64)
                {
                    txtbxMessages.Text = "School names are limited to 64 characters.";

                    e.Handled = true;

                    return;
                }

                if (!Regex.IsMatch(e.Text, "[-\\p{L} ]+"))
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}",
                        Environment.NewLine,
                        "Only letters, hyphens, and spaces",
                        "are permitted in school names.");

                    e.Handled = true;

                    return;
                }

                if (txtbxSchoolOrOrganization.Text.Length >= 16)
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                        Environment.NewLine,
                        "Majors names longer than 16 letters tend to display poorly on name tags.",
                        "Consider shortening or abbreviating the name of your major.",
                        "Sorry for the inconvenience!");
                }
            }
            else if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.EMPLOYEE)
            {
                if (txtbxSchoolOrOrganization.Text.Length >= 64)
                {
                    txtbxMessages.Text = "Organization names are limited to 64 characters.";

                    e.Handled = true;

                    return;
                }

                if (Regex.IsMatch(e.Text, "\\b[-\\p{L} ]+\\b"))
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}",
                        Environment.NewLine,
                        "Only letters, hyphens, and spaces",
                        "are supported in organization names.");

                    e.Handled = true;

                    return;
                }

                if (txtbxSchoolOrOrganization.Text.Length >= 16)
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                        "Organization names longer than 16 letters tend to display poorly on name tags.",
                        "If possible, consider shortening or abbreviating the name of your organization.",
                        "Sorry for the inconvenience!");
                }
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
                else if(btnCheckIn.IsFocused)
                {
                    CheckIn();
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

        private async void CheckIn()
        {
            if(IsReadyForCheckIn())
            {
                //Hide and disable 'Start Over' and 'Check In' buttons
                rctStartOver.Visibility = System.Windows.Visibility.Hidden;
                btnStartOver.Visibility = System.Windows.Visibility.Hidden;
                btnStartOver.IsEnabled = false;
                rctCheckInFinish.Visibility = System.Windows.Visibility.Hidden;
                btnCheckIn.Visibility = System.Windows.Visibility.Hidden;
                btnCheckIn.IsEnabled = false;

                //Display check-in confirmation message
                txtbxMessages.Text = String.Format("{1}{0}{2}",
                Environment.NewLine,
                "Check-in complete!",
                "Your name tag is printing.");

                //Update stuff

                //Print stuff

                await Task.Delay(5000);

                SetMode(Controller.RegistrantMode.RESET);
            }
        }

        public void DisplayRegistrant()
        {
            txtbxFirstName.Text = controller.ActiveRegistrant.FirstName;
            txtbxLastName.Text = controller.ActiveRegistrant.LastName;

            switch (controller.ActiveRegistrant.RegistrantType)
            {
                case "Student":
                    SetMode(Controller.RegistrantMode.STUDENT);
                    txtbxSchoolOrOrganization.Text = controller.ActiveRegistrant.College;
                    txtbxMajorOrPosition.Text = controller.ActiveRegistrant.Major;

                    switch (controller.ActiveRegistrant.ClassStanding)
                    {
                        case "Freshman":
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.FRESHMAN;
                            break;
                        case "Junior":
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.JUNIOR;
                            break;
                        case "Sophomore":
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.SOPHOMORE;
                            break;
                        case "Senior":
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.SENIOR;
                            break;
                        case "PostBach":
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.POSTBACH;
                            break;
                        case "Graduate":
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.GRADUATE;
                            break;
                        case "Alumnus":
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.ALUMNUS;
                            break;
                        default:
                            cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.SELECT;
                            break;
                    }

                    break;

                case "Employee":
                    SetMode(Controller.RegistrantMode.EMPLOYEE);
                    txtbxSchoolOrOrganization.Text = controller.ActiveRegistrant.Company;
                    txtbxMajorOrPosition.Text = controller.ActiveRegistrant.Position;

                    break;

                case "General":
                    SetMode(Controller.RegistrantMode.GENERAL);

                    break;

                default:
                    SetMode(Controller.RegistrantMode.REGISTER);

                    break;
            }

            /*Placeholder //PICKUP
            if (IsReadyToCheckIn())
            */
            txtbxMessages.Text = String.Format("{1}{0}{2}",
                Environment.NewLine,
                "Registration found.",
                "Please verify your info and click 'Check In' to finish.");
        }

        //DEV: PG: This method should highlight fields that have errors when they are found.
        private Boolean IsReadyForCheckIn()
        {
            Boolean isReady = true;
            Boolean firstNameError = false; //Used to avoid redunant name-related error messages
            Boolean fieldsLeftBlank = false;

            //Check registrant type selection
            if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.RESET)
            {
                isReady = false;

                txtbxMessages.Clear();
                txtbxMessages.Text = "You must select a registrant type.";
            }

            if(cmbRegistrantType.SelectedIndex != (int)Controller.RegistrantMode.EMPLOYEE)
            {
                //Check first name
                if (txtbxFirstName.Text.Length == 0)
                {
                    isReady = false;
                    fieldsLeftBlank = true;
                }
                else if (Regex.IsMatch(txtbxFirstName.Text, @"[^-'\p{L} ]"))
                {
                    if (isReady)
                    {
                        isReady = false;
                        
                        txtbxMessages.Clear();
                        txtbxMessages.Text = "Names may only contain letters, apostrophes, hypens, and spaces.";

                        firstNameError = true;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            "Names may only contain letters, apostrophes, hypens, and spaces.");
                    }
                }

                //Check last name
                if (txtbxLastName.Text.Length == 0)
                {
                    isReady = false;
                    fieldsLeftBlank = true;
                }
                else if (Regex.IsMatch(txtbxLastName.Text, @"[^-'\p{L} ]"))
                {
                    if (isReady)
                    {
                        isReady = false;

                        if (!firstNameError)
                        {
                            txtbxMessages.Clear();
                            txtbxMessages.Text = "Names may only contain letters, apostrophes, hypens, and spaces.";
                        }
                    }
                    else
                    {
                        if (!firstNameError)
                        {
                            txtbxMessages.Text = String.Format("{1}{0}{2}",
                                Environment.NewLine,
                                txtbxMessages.Text,
                                "Names may only contain letters, apostrophes, hypens, and spaces.");
                        }
                    }
                }

                //Check School name
                if (cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
                {
                    if (txtbxSchoolOrOrganization.Text.Length == 0)
                    {
                        isReady = false;
                        fieldsLeftBlank = true;
                    }
                    else if (Regex.IsMatch(txtbxSchoolOrOrganization.Text, @"[^-\p{L} ]"))
                    {
                        if (isReady)
                        {
                            isReady = false;

                            txtbxMessages.Clear();
                            txtbxMessages.Text = "School names may only contain letters, hypens, and spaces.";
                        }
                        else
                        {
                            txtbxMessages.Text = String.Format("{1}{0}{2}",
                                Environment.NewLine,
                                txtbxMessages.Text,
                                "School names may only contain letters, hypens, and spaces.");
                        }
                    }
                }

                //Check Major
                if (cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
                {
                    if (txtbxMajorOrPosition.Text.Length == 0)
                    {
                        isReady = false;
                        fieldsLeftBlank = true;
                    }
                    else if (Regex.IsMatch(txtbxMajorOrPosition.Text, @"[^-\p{L} ]"))
                    {
                        if (isReady)
                        {
                            isReady = false;

                            txtbxMessages.Clear();
                            txtbxMessages.Text = "Major names may only contain letters, hypens, and spaces.";
                        }
                        else
                        {
                            txtbxMessages.Text = String.Format("{1}{0}{2}",
                                Environment.NewLine,
                                txtbxMessages.Text,
                                "Major names may only contain letters, hypens, and spaces.");
                        }
                    }
                }

                //Check class standing selection
                if (cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
                {
                    if (cmbClassStanding.SelectedIndex == (int)Controller.ClassStanding.SELECT && !txtbxMessages.Text.Contains("class standing"))
                    {
                        if(isReady)
                        {
                            isReady = false;

                            txtbxMessages.Clear();
                            txtbxMessages.Text = "Please select a class standing.";
                        }
                        else
                        {
                            txtbxMessages.Text = String.Format("{1}{0}{2}",
                                    Environment.NewLine,
                                    txtbxMessages.Text,
                                    "Please select a class standing.");
                        }
                    }
                }

                if(fieldsLeftBlank)
                {
                    if(isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = "Blank fields detected. Please fill in any missing info.";
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                                Environment.NewLine,
                                txtbxMessages.Text,
                                "Blank fields detected. Please fill in any missing info.");
                    }
                }
            }

            return isReady;
        }

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

        private async void LookupRegistrant()
        {
            if (IsValidRegistrationCode())
            {
                txtbxMessages.Text = "Looking up your registration info...";

                //Disable form controls during lookup
                btnEnterCode.Visibility = System.Windows.Visibility.Collapsed;
                btnEnterCode.IsEnabled = false;
                btnNoCode.Visibility = System.Windows.Visibility.Collapsed;
                btnEnterCode.IsEnabled = false;
                btnRegister.Visibility = System.Windows.Visibility.Collapsed;
                btnRegister.IsEnabled = false;

                try
                {
                    if((controller.ActiveRegistrant = await controller.WebAPI.GetRegistrantByCode(txtbxEnterCode.Text)) == null)
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                            Environment.NewLine,
                            "Your registration info was not found.",
                            "Please check your code and try again.",
                            "If the problem persists, start over and click 'Register' to continue checking in.");
                    }
                    else
                    {
                        DisplayRegistrant();

                        return;
                    }
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

            //Reenable form controls after lookup
            btnEnterCode.Visibility = System.Windows.Visibility.Visible;
            btnEnterCode.IsEnabled = true;
            btnNoCode.Visibility = System.Windows.Visibility.Visible;
            btnEnterCode.IsEnabled = true;
            btnRegister.Visibility = System.Windows.Visibility.Visible;
            btnRegister.IsEnabled = true;
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
                //Clear controller.activeRegistrant, if any
                controller.ClearRegistrant();

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

                //Hide 'Start Over' button
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
                rctSchoolOrOrganization.Visibility = System.Windows.Visibility.Hidden;
                lblSchoolOrOrganization.Visibility = System.Windows.Visibility.Hidden;
                lblSchoolOrOrganization.Content = "School";
                txtbxSchoolOrOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrOrganization.Clear();
                txtbxSchoolOrOrganization.IsEnabled = false;
                rctMajorOrPosition.Visibility = System.Windows.Visibility.Hidden;
                lblMajorOrPosition.Visibility = System.Windows.Visibility.Hidden;
                lblMajorOrPosition.Content = "Major";
                txtbxMajorOrPosition.Visibility = System.Windows.Visibility.Hidden;
                txtbxMajorOrPosition.Clear();
                txtbxMajorOrPosition.IsEnabled = false;
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
                txtbxMessages.Text = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Welcome!",
                    "Please fill in all the fields below.");

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

                rctSchoolOrOrganization.Visibility = System.Windows.Visibility.Visible;
                lblSchoolOrOrganization.Content = "School";
                lblSchoolOrOrganization.Visibility = System.Windows.Visibility.Visible;
                txtbxSchoolOrOrganization.IsEnabled = true;
                txtbxSchoolOrOrganization.Visibility = System.Windows.Visibility.Visible;

                rctMajorOrPosition.Visibility = System.Windows.Visibility.Visible;
                lblMajorOrPosition.Content = "Major";
                lblMajorOrPosition.Visibility = System.Windows.Visibility.Visible;
                txtbxMajorOrPosition.IsEnabled = true;
                txtbxMajorOrPosition.Visibility = System.Windows.Visibility.Visible;

                rctClassStanding.Visibility = System.Windows.Visibility.Visible;
                lblClassStanding.Visibility = System.Windows.Visibility.Visible;
                cmbClassStanding.IsEnabled = true;
                cmbClassStanding.Visibility = System.Windows.Visibility.Visible;

                return;
            }

            #endregion //STUDENT

            //Registrant is an employer or employee
            #region EMPLOYER

            if (mode == Controller.RegistrantMode.EMPLOYEE)
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                    Environment.NewLine,
                    "Welcome!",
                    "Fill in the fields below and click 'Check In' to receive a name tag.",
                    "All fields are optional.");

                //Set grdInputFields for EMPLOYER parameters
                //Watch for bugs! See declaration of selectionLocked for cautionary info
                selectionLocked = true;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.EMPLOYEE;
                selectionLocked = false;

                rctFirstName.Visibility = System.Windows.Visibility.Visible;
                lblFirstName.Visibility = System.Windows.Visibility.Visible;
                txtbxFirstName.Visibility = System.Windows.Visibility.Visible;
                txtbxFirstName.IsEnabled = true;
                rctLastName.Visibility = System.Windows.Visibility.Visible;
                lblLastName.Visibility = System.Windows.Visibility.Visible;
                txtbxLastName.Visibility = System.Windows.Visibility.Visible;
                txtbxLastName.IsEnabled = true;

                rctSchoolOrOrganization.Visibility = System.Windows.Visibility.Visible;
                lblSchoolOrOrganization.Content = "Organization";
                lblSchoolOrOrganization.Visibility = System.Windows.Visibility.Visible;
                txtbxSchoolOrOrganization.IsEnabled = true;
                txtbxSchoolOrOrganization.Visibility = System.Windows.Visibility.Visible;

                rctMajorOrPosition.Visibility = System.Windows.Visibility.Visible;
                lblMajorOrPosition.Content = "Job Title";
                lblMajorOrPosition.Visibility = System.Windows.Visibility.Visible;
                txtbxMajorOrPosition.IsEnabled = true;
                txtbxMajorOrPosition.Visibility = System.Windows.Visibility.Visible;

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
                txtbxMessages.Text = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Welcome!",
                    "Please enter your first and last name.");

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

                rctSchoolOrOrganization.Visibility = System.Windows.Visibility.Hidden;
                lblSchoolOrOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrOrganization.IsEnabled = false;

                rctMajorOrPosition.Visibility = System.Windows.Visibility.Hidden;
                lblMajorOrPosition.Visibility = System.Windows.Visibility.Hidden;
                txtbxMajorOrPosition.Visibility = System.Windows.Visibility.Hidden;
                txtbxMajorOrPosition.IsEnabled = false;

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

                rctSchoolOrOrganization.Visibility = System.Windows.Visibility.Hidden;
                lblSchoolOrOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrOrganization.IsEnabled = false;

                rctMajorOrPosition.Visibility = System.Windows.Visibility.Hidden;
                lblMajorOrPosition.Visibility = System.Windows.Visibility.Hidden;
                txtbxMajorOrPosition.Visibility = System.Windows.Visibility.Hidden;
                txtbxMajorOrPosition.IsEnabled = false;

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