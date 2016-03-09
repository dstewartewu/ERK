using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
            BtnClick(sender, e);
            CheckIn();
        }

        private void btnEnterCode_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender, e);
            LookupRegistrant();
        }

        private void btnNoCode_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender, e);
            controller.SetView(Controller.WindowView.NO_CODE);
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender, e);
            SetMode(Controller.RegistrantMode.REGISTER);

            //Display instructions for registration
            txtbxMessages.Text = String.Format("{1}{0}{2}",
                Environment.NewLine,
                "Fill out the form below to continue.",
                "Start by selecting a registrant type.");
        }

        private void btnStartOver_Click(object sender, RoutedEventArgs e)
        {
            BtnClick(sender, e);
            SetMode(Controller.RegistrantMode.RESET);
        }

        private void cmbRegistrantType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (wdwKiosk.IsLoaded && !selectionLocked)
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

            if(Regex.IsMatch(e.Text, @"[^-'\p{L} ]+"))
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Only letters, hyphens, apostrophes, and spaces",
                    "are permitted in first and last names.");

                e.Handled = true;

                return;
            }

            if(txtbxFirstName.Text.Length > 16)
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

            if (Regex.IsMatch(e.Text, @"[^-'\p{L} ]+"))
            {
                txtbxMessages.Text = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Only letters, hyphens, apostrophes, and spaces",
                    "are permitted in first and last names.");

                e.Handled = true;

                return;
            }

            if (txtbxLastName.Text.Length > 16)
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

                if (Regex.IsMatch(e.Text, @"[^-\p{L} ]+"))
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}",
                        Environment.NewLine,
                        "Only letters, hyphens, and spaces",
                        "are permitted in major names.");

                    e.Handled = true;

                    return;
                }

                if (txtbxMajorOrPosition.Text.Length > 16)
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

                if (Regex.IsMatch(e.Text, "[^-\\p{L} ]+"))
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
                        Environment.NewLine,
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

                if (Regex.IsMatch(e.Text, @"[^-\p{L} ]+"))
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}",
                        Environment.NewLine,
                        "Only letters, hyphens, and spaces",
                        "are permitted in school names.");

                    e.Handled = true;

                    return;
                }

                if (txtbxSchoolOrOrganization.Text.Length > 16)
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

                if (Regex.IsMatch(e.Text, "[^-\\p{L} ]+"))
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
                controller.SetView(Controller.WindowView.START_MENU);
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
                //Hide and disable 'Start Over' and 'Check In' buttons, and all input fields
                rctStartOver.Visibility = System.Windows.Visibility.Hidden;
                btnStartOver.Visibility = System.Windows.Visibility.Hidden;
                btnStartOver.IsEnabled = false;
                rctCheckInFinish.Visibility = System.Windows.Visibility.Hidden;
                btnCheckIn.Visibility = System.Windows.Visibility.Hidden;
                btnCheckIn.IsEnabled = false;
                cmbRegistrantType.IsEnabled = false;
                txtbxFirstName.IsEnabled = false;
                txtbxLastName.IsEnabled = false;
                txtbxSchoolOrOrganization.IsEnabled = false;
                txtbxMajorOrPosition.IsEnabled = false;
                cmbClassStanding.IsEnabled = false;

                //Display check-in confirmation message
                txtbxMessages.Text = "Checking in, please wait...";

                //Update or add registrant
                try
                {
                    //If this is a new registrant, create and add to event database
                    if(controller.ActiveRegistrant == null)
                    {
                        controller.ActiveRegistrant = new Registrant();

                        if(controller.IsOnlineEnabled)
                        {
                            controller.ActiveRegistrant.EventNumber = controller.WebAPI.Event.EventNumber;
                        }

                        switch(cmbRegistrantType.SelectedIndex)
                        {
                            case (int)Controller.RegistrantMode.STUDENT:
                                controller.ActiveRegistrant.RegistrantType = "Student";
                                break;
                            case (int)Controller.RegistrantMode.EMPLOYEE:
                                controller.ActiveRegistrant.RegistrantType = "Employee";
                                break;
                            default:
                                controller.ActiveRegistrant.RegistrantType = "General";
                                break;
                        }

                        controller.ActiveRegistrant.FirstName = txtbxFirstName.Text;
                        controller.ActiveRegistrant.LastName = txtbxLastName.Text;

                        if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
                        {
                            controller.ActiveRegistrant.College = txtbxSchoolOrOrganization.Text;
                            controller.ActiveRegistrant.Major = txtbxMajorOrPosition.Text;

                            switch(cmbClassStanding.SelectedIndex)
                            {
                                case (int)Controller.ClassStanding.FRESHMAN:
                                    controller.ActiveRegistrant.ClassStanding = "Freshman";
                                    break;

                                case (int)Controller.ClassStanding.SOPHOMORE:
                                    controller.ActiveRegistrant.ClassStanding = "Sophomore";
                                    break;

                                case (int)Controller.ClassStanding.JUNIOR:
                                    controller.ActiveRegistrant.ClassStanding = "Junior";
                                    break;

                                case (int)Controller.ClassStanding.SENIOR:
                                    controller.ActiveRegistrant.ClassStanding = "Senior";
                                    break;

                                case (int)Controller.ClassStanding.POSTBACH:
                                    controller.ActiveRegistrant.ClassStanding = "PostBac";
                                    break;

                                case (int)Controller.ClassStanding.GRADUATE:
                                    controller.ActiveRegistrant.ClassStanding = "Graduate";
                                    break;

                                case (int)Controller.ClassStanding.ALUMNUS:
                                    controller.ActiveRegistrant.ClassStanding = "Alumnus";
                                    break;

                                default:
                                    controller.ActiveRegistrant.ClassStanding = "None";
                                    break;
                            }
                        }
                        else if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.EMPLOYEE)
                        {
                            controller.ActiveRegistrant.Company = txtbxSchoolOrOrganization.Text;
                            controller.ActiveRegistrant.Position = txtbxMajorOrPosition.Text;
                        }

                        if(controller.IsOnlineEnabled)
                        {
                            controller.ActiveRegistrant.CheckInTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            await controller.WebAPI.AddStudent(controller.ActiveRegistrant);
                        }
                    }
                    //If this registrant already exists, update their database entry
                    else
                    {
                        switch (cmbRegistrantType.SelectedIndex)
                        {
                            case (int)Controller.RegistrantMode.STUDENT:
                                controller.ActiveRegistrant.RegistrantType = "Student";
                                controller.ActiveRegistrant.FirstName = txtbxFirstName.Text;
                                controller.ActiveRegistrant.LastName = txtbxLastName.Text;
                                controller.ActiveRegistrant.College = txtbxSchoolOrOrganization.Text;
                                controller.ActiveRegistrant.Major = txtbxMajorOrPosition.Text;

                                switch (cmbClassStanding.SelectedIndex)
                                {
                                    case (int)Controller.ClassStanding.FRESHMAN:
                                        controller.ActiveRegistrant.ClassStanding = "Freshman";
                                        break;

                                    case (int)Controller.ClassStanding.SOPHOMORE:
                                        controller.ActiveRegistrant.ClassStanding = "Sophomore";
                                        break;

                                    case (int)Controller.ClassStanding.JUNIOR:
                                        controller.ActiveRegistrant.ClassStanding = "Junior";
                                        break;

                                    case (int)Controller.ClassStanding.SENIOR:
                                        controller.ActiveRegistrant.ClassStanding = "Senior";
                                        break;

                                    case (int)Controller.ClassStanding.POSTBACH:
                                        controller.ActiveRegistrant.ClassStanding = "PostBac";
                                        break;

                                    case (int)Controller.ClassStanding.GRADUATE:
                                        controller.ActiveRegistrant.ClassStanding = "Graduate";
                                        break;

                                    case (int)Controller.ClassStanding.ALUMNUS:
                                        controller.ActiveRegistrant.ClassStanding = "Alumnus";
                                        break;

                                    default:
                                        controller.ActiveRegistrant.ClassStanding = "None";
                                        break;
                                }

                                controller.ActiveRegistrant.Company = null;
                                controller.ActiveRegistrant.Position = null;

                                break;

                            case (int)Controller.RegistrantMode.EMPLOYEE:
                                controller.ActiveRegistrant.RegistrantType = "Employee";

                                if(txtbxFirstName.Text.Length != 0)
                                {
                                    controller.ActiveRegistrant.FirstName = txtbxFirstName.Text;
                                }

                                if(txtbxLastName.Text.Length != 0)
                                {
                                    controller.ActiveRegistrant.LastName = txtbxLastName.Text;
                                }

                                if(txtbxSchoolOrOrganization.Text.Length != 0)
                                {
                                    controller.ActiveRegistrant.Company = txtbxSchoolOrOrganization.Text;
                                }

                                if(txtbxMajorOrPosition.Text.Length != 0)
                                {
                                    controller.ActiveRegistrant.Position = txtbxMajorOrPosition.Text;
                                }

                                controller.ActiveRegistrant.College = null;
                                controller.ActiveRegistrant.Major = null;
                                controller.ActiveRegistrant.ClassStanding = null;

                                break;

                            default:
                                controller.ActiveRegistrant.RegistrantType = "General";
                                controller.ActiveRegistrant.FirstName = txtbxFirstName.Text;
                                controller.ActiveRegistrant.LastName = txtbxLastName.Text;
                                controller.ActiveRegistrant.College = null;
                                controller.ActiveRegistrant.Major = null;
                                controller.ActiveRegistrant.ClassStanding = null;
                                controller.ActiveRegistrant.Company = null;
                                controller.ActiveRegistrant.Position = null;
                                break;
                        }

                        if(controller.IsOnlineEnabled)
                        {
                            controller.ActiveRegistrant.CheckInTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            await controller.WebAPI.UpdateRegistrant(controller.ActiveRegistrant);
                        }
                    }
                }
                catch(Exception e)
                {
                    controller.LogError("An error occurred while adding/updating " + controller.ActiveRegistrant.FirstName + " " + controller.ActiveRegistrant.LastName + " to the event database.");

                    Thread.Sleep(3500);
                }

                //Display check-in confirmation message
                txtbxMessages.Text = String.Format("{1}{0}{2}",
                Environment.NewLine,
                "Check-in complete!",
                "Your name tag is printing.");

                //Print registrant name tag
                try
                {
                    Printer.Print(controller.ActiveRegistrant);
                }
                catch(Exception e)
                {
                    controller.LogError("An error occurred while printing the name tag of " + controller.ActiveRegistrant.FirstName + " " + controller.ActiveRegistrant.LastName + ".");
                }

                await Task.Delay(3500);

                SetMode(Controller.RegistrantMode.RESET);
            }
        }

        public void Connect()
        {
            lblOnline.Content = "ONLINE";

            if(controller.IsOnlineEnabled)
            {
                SetCustomEventName(controller.EventName);
            }

            SetMode(Controller.RegistrantMode.RESET);
        }

        public void Disconnect()
        {
            lblOnline.Content = "OFFLINE";

            SetCustomEventName(null);

            SetMode(Controller.RegistrantMode.RESET);
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

        private Boolean IsReadyForCheckIn()
        {
            Boolean isReady = true;
            String errorMessage;

            //Check registrant type selection
            if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.RESET)
            {
                txtbxMessages.Clear();
                txtbxMessages.Text = "You must select a registrant type.";

                return false;
            }

            //Check first name
            if(txtbxFirstName.Text.Length == 0)
            {
                if(cmbRegistrantType.SelectedIndex != (int)Controller.RegistrantMode.EMPLOYEE)
                {
                    isReady = false;

                    txtbxMessages.Clear();
                    txtbxMessages.Text = "First name can't be left blank.";
                }
            }
            else if(Regex.IsMatch(txtbxFirstName.Text, @"[^\p{L}'\- ]+"))
            {
                isReady = false;

                txtbxMessages.Clear();
                txtbxMessages.Text = "First name may only contain letters, apostrohpes, hyphens, and spaces.";
            }
            else if(txtbxFirstName.Text.Length > 64)
            {
                isReady = false;

                txtbxMessages.Clear();
                txtbxMessages.Text = "First name can't exceed 64 characters.";
            }

            //Check last name
            if(txtbxLastName.Text.Length == 0)
            {
                if(cmbRegistrantType.SelectedIndex != (int)Controller.RegistrantMode.EMPLOYEE)
                {
                    errorMessage = "Last name can't be left blank.";

                    if(isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = errorMessage;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            errorMessage);
                    }
                }
            }
            else if(Regex.IsMatch(txtbxLastName.Text, @"[^\p{L}'\- ]+"))
            {
                errorMessage = "Last name may only contain letters, apostrohpes, hyphens, and spaces.";

                if (isReady)
                {
                    isReady = false;

                    txtbxMessages.Clear();
                    txtbxMessages.Text = errorMessage;
                }
                else
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}",
                        Environment.NewLine,
                        txtbxMessages.Text,
                        errorMessage);
                }
            }
            else if (txtbxLastName.Text.Length > 64)
            {
                errorMessage = "Last name can't exceed 64 characters.";

                if (isReady)
                {
                    isReady = false;

                    txtbxMessages.Clear();
                    txtbxMessages.Text = errorMessage;
                }
                else
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}",
                        Environment.NewLine,
                        txtbxMessages.Text,
                        errorMessage);
                }
            }

            //Check school or organization name
            if(txtbxSchoolOrOrganization.Text.Length == 0)
            {
                if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
                {
                    errorMessage = "School name can't be left blank.";

                    if(isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = errorMessage;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            errorMessage);
                    }
                }
            }
            else if(Regex.IsMatch(txtbxSchoolOrOrganization.Text, @"[^\p{L}'\- ]+"))
            {
                if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
                {
                    errorMessage = "School names may only contain letters, hyphens, and spaces.";

                    if (isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = errorMessage;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            errorMessage);
                    }
                }
                else if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.EMPLOYEE)
                {
                    errorMessage = "Organization names may only contain letters, hyphens, and spaces.";

                    if (isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = errorMessage;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            errorMessage);
                    }
                }
            }
            else if (txtbxSchoolOrOrganization.Text.Length > 64)
            {
                if (cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
                {
                    errorMessage = "School names can't exceed 64 characters.";

                    if (isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = errorMessage;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            errorMessage);
                    }
                }
                else if (cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.EMPLOYEE)
                {
                    errorMessage = "Organization names can't exceed 64 characters.";

                    if (isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = errorMessage;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            errorMessage);
                    }
                }
            }

            //Check major or job title
            if(txtbxMajorOrPosition.Text.Length == 0)
            {
                if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
                {
                    errorMessage = "Major can't be left blank.";

                    if(isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = errorMessage;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            errorMessage);
                    }
                }
            }
            else if(Regex.IsMatch(txtbxMajorOrPosition.Text, @"[^\p{L}'\- ]+"))
            {
                if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
                {
                    errorMessage = "Majors may only contain letters, hyphens, and spaces.";

                    if (isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = errorMessage;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            errorMessage);
                    }
                }
                else if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.EMPLOYEE)
                {
                    errorMessage = "Job titles may only contain letters, hyphens, and spaces.";

                    if (isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = errorMessage;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            errorMessage);
                    }
                }
            }
            else if (txtbxMajorOrPosition.Text.Length > 64)
            {
                if (cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT)
                {
                    errorMessage = "Major can't exceed 64 characters.";

                    if (isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = errorMessage;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            errorMessage);
                    }
                }
                else if (cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.EMPLOYEE)
                {
                    errorMessage = "Job title can't exceed 64 characters.";

                    if (isReady)
                    {
                        isReady = false;

                        txtbxMessages.Clear();
                        txtbxMessages.Text = errorMessage;
                    }
                    else
                    {
                        txtbxMessages.Text = String.Format("{1}{0}{2}",
                            Environment.NewLine,
                            txtbxMessages.Text,
                            errorMessage);
                    }
                }
            }

            //Check class standing, if student
            if(cmbRegistrantType.SelectedIndex == (int)Controller.RegistrantMode.STUDENT && cmbClassStanding.SelectedIndex == (int)Controller.ClassStanding.SELECT)
            {
                errorMessage = "You must select a class standing.";

                if (isReady)
                {
                    isReady = false;

                    txtbxMessages.Clear();
                    txtbxMessages.Text = errorMessage;
                }
                else
                {
                    txtbxMessages.Text = String.Format("{1}{0}{2}",
                        Environment.NewLine,
                        txtbxMessages.Text,
                        errorMessage);
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

                        controller.ClearRegistrant();
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

                    controller.ClearRegistrant();
                }
            }
            else
            {
                txtbxMessages.Text += String.Format("{0}{1}{0}{2}",
                        Environment.NewLine,
                        "Please check your registration code and try again.",
                        "If the problem persists, start over and click 'Register' to continue checking in.");

                controller.ClearRegistrant();
            }

            //Reenable form controls after lookup
            btnEnterCode.Visibility = System.Windows.Visibility.Visible;
            btnEnterCode.IsEnabled = true;
            btnNoCode.Visibility = System.Windows.Visibility.Visible;
            btnEnterCode.IsEnabled = true;
            btnRegister.Visibility = System.Windows.Visibility.Visible;
            btnRegister.IsEnabled = true;
        }

        public void SetCustomEventName(String eventName)
        {
            if(eventName == null)
            {
                txtbxHeader.Text = "Event Check-In";
            }
            else
            {
                txtbxHeader.Text = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    eventName,
                    "Check-In");
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
                //Clear controller.activeRegistrant, if any
                controller.ClearRegistrant();

                if(controller.IsOnlineEnabled)
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
                    cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.SELECT;
                    cmbClassStanding.IsEnabled = false;

                    //Hide 'Check In' button
                    rctCheckInFinish.Visibility = System.Windows.Visibility.Hidden;
                    btnCheckIn.Visibility = System.Windows.Visibility.Hidden;
                    btnCheckIn.IsEnabled = false;

                    return;
                }
                else
                {
                    txtbxFirstName.Clear();
                    txtbxLastName.Clear();
                    txtbxSchoolOrOrganization.Clear();
                    txtbxMajorOrPosition.Clear();
                    cmbRegistrantType.Focus();
                    mode = Controller.RegistrantMode.REGISTER;
                }
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
                //Set welcome message and starting instructions
                txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                    Environment.NewLine,
                    "Welcome!",
                    "Are you a student, employer, or general attendee?",
                    "Please make a selection below.");

                //Watch for bugs! See declaration of selectionLocked for cautionary info
                selectionLocked = true;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.RESET;
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
                cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.SELECT;

                return;
            }

            #endregion //REGISTER
        }

        //Handlers for button appearance.
        Style tempStyle;    //Temproary style variable
        Style tempCBStyle;

        //This is the handler for mouse hover of any button.
        private void BtnMouseHover(object sender, MouseEventArgs e)
        {
            Button b = (Button)sender;                                          //Create the button from the passed-in object.
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