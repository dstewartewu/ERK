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

        private Controller.RegistrantMode currentMode;

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
            MockCheckIn();
        }

        private void btnNoCode_Click(object sender, RoutedEventArgs e)
        {
            controller.SetView(Controller.WindowView.NO_CODE);
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            SetMode(Controller.RegistrantMode.REGISTER);

            //Display instructions for registration
            lblMessages.Content = String.Format("{1}{0}{2}",
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
            if (wdwMain.IsLoaded && cmbRegistrantType.SelectedIndex != (int)currentMode)
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

        private void txtbxEnterCode_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                MockCheckIn();
            }
        }

        private void txtbxEnterCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (wdwMain.IsLoaded)
            {
                if (Regex.IsMatch(txtbxEnterCode.Text, "\\D"))
                {
                    btnEnterCode.IsEnabled = false;

                    lblMessages.Content = "Registration codes contain numbers only.";
                }
                else if (txtbxEnterCode.Text.Length == 6)
                {
                    btnEnterCode.IsEnabled = true;

                    lblMessages.Content = "Click 'Enter Code' to continue.";
                }
                else if (txtbxEnterCode.Text.Length > 6)
                {
                    btnEnterCode.IsEnabled = false;

                    lblMessages.Content = String.Format("{1}{0}{2}",
                        Environment.NewLine,
                        "Registration codes are only six digits long.",
                        "Please check your code and try again.");
                }
                else
                {
                    btnEnterCode.IsEnabled = false;
                }
            }

            e.Handled = true;
        }

        private void txtbxFirstName_TextChanged(object sender, TextChangedEventArgs e)
        {
            //PG: DEV: Verify user input; restrict certain character sets
        }

        private void wdwMain_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.F1)
            {
                controller.SetView(Controller.WindowView.ADMIN_LOGIN);
            }
        }

        #endregion

        private void SetMode(Controller.RegistrantMode mode)
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
                lblMessages.Content = String.Format("{1}{0}{2}{0}{3}{0}{4}",
                                                Environment.NewLine,
                                                "Welcome!",
                                                "Scan or enter your 6-digit code if you pre-registered online.",
                                                "Don't have your code? Click 'No Code' to check in by email.",
                                                "Otherwise, click 'Register' to check in and receive a name tag.");

                //Set RESET mode
                currentMode = Controller.RegistrantMode.RESET;

                //Hide 'start over' button
                btnStartOver.Visibility = System.Windows.Visibility.Hidden;
                btnStartOver.IsEnabled = false;

                //Show left column elements
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

                //Hide and reset right column elements
                lblRegistrantType.Visibility = System.Windows.Visibility.Hidden;
                cmbRegistrantType.Visibility = System.Windows.Visibility.Hidden;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.RESET;
                cmbRegistrantType.IsEnabled = false;
                lblFirstName.Visibility = System.Windows.Visibility.Hidden;
                txtbxFirstName.Visibility = System.Windows.Visibility.Hidden;
                txtbxFirstName.Clear();
                txtbxFirstName.IsEnabled = false;
                lblLastName.Visibility = System.Windows.Visibility.Hidden;
                txtbxLastName.Visibility = System.Windows.Visibility.Hidden;
                txtbxLastName.Clear();
                txtbxLastName.IsEnabled = false;
                lblSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                lblSchoolOrganization.Content = "School";
                txtbxSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrganization.Clear();
                txtbxSchoolOrganization.IsEnabled = false;
                lblClassStandingJobTitle.Visibility = System.Windows.Visibility.Hidden;
                lblClassStandingJobTitle.Content = "Class standing";
                cmbClassStanding.Visibility = System.Windows.Visibility.Hidden;
                cmbClassStanding.SelectedIndex = (int)Controller.RegistrantMode.RESET;
                cmbClassStanding.IsEnabled = false;
                txtbxJobTitle.Visibility = System.Windows.Visibility.Hidden;
                txtbxJobTitle.Clear();
                txtbxJobTitle.IsEnabled = false;

                btnCheckIn.IsEnabled = false;

                return;
            }
            #endregion

            //Else prepare kiosk for check-in
            #region PREPARE FOR CHECK-IN

            else if (currentMode == (int)Controller.RegistrantMode.RESET)
            {
                //Hide left column elements
                txtbxEnterCode.Visibility = System.Windows.Visibility.Hidden;
                txtbxEnterCode.Clear();
                txtbxEnterCode.IsEnabled = false;
                btnEnterCode.Visibility = System.Windows.Visibility.Hidden;
                btnEnterCode.IsEnabled = false;
                btnNoCode.Visibility = System.Windows.Visibility.Hidden;
                btnNoCode.IsEnabled = false;
                btnRegister.Visibility = System.Windows.Visibility.Hidden;
                btnRegister.IsEnabled = false;

                //Show 'Start Over' button
                btnStartOver.IsEnabled = true;
                btnStartOver.Visibility = System.Windows.Visibility.Visible;
            }
            #endregion

            //Registrant is a student
            #region STUDENT

            if (mode == Controller.RegistrantMode.STUDENT)
            {
                //Set STUDENT mode
                currentMode = Controller.RegistrantMode.STUDENT;

                //Show registrat type selector
                lblRegistrantType.Visibility = System.Windows.Visibility.Visible;
                cmbRegistrantType.IsEnabled = true;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.STUDENT; //WATCH THIS LINE FOR BUGS!
                cmbRegistrantType.Visibility = System.Windows.Visibility.Visible;

                //Show right column elements related to student registration; hide others
                lblFirstName.Visibility = System.Windows.Visibility.Visible;
                txtbxFirstName.IsEnabled = true;
                txtbxFirstName.Visibility = System.Windows.Visibility.Visible;
                lblLastName.Visibility = System.Windows.Visibility.Visible;
                txtbxLastName.IsEnabled = true;
                txtbxLastName.Visibility = System.Windows.Visibility.Visible;
                lblSchoolOrganization.Content = "School";
                lblSchoolOrganization.Visibility = System.Windows.Visibility.Visible;
                txtbxSchoolOrganization.IsEnabled = true;
                txtbxSchoolOrganization.Visibility = System.Windows.Visibility.Visible;
                lblClassStandingJobTitle.Content = "Class standing";
                lblClassStandingJobTitle.Visibility = System.Windows.Visibility.Visible;
                txtbxJobTitle.Visibility = System.Windows.Visibility.Hidden;
                txtbxJobTitle.IsEnabled = false;
                cmbClassStanding.IsEnabled = true;
                cmbClassStanding.Visibility = System.Windows.Visibility.Visible;

                btnCheckIn.IsEnabled = true;

                return;
            }

            #endregion //STUDENT

            //Registrant is an employer or employee
            #region EMPLOYER

            if (mode == Controller.RegistrantMode.EMPLOYER)
            {
                //Set EMPLOYER mode
                currentMode = Controller.RegistrantMode.EMPLOYER;

                //Show registrat type selector
                lblRegistrantType.Visibility = System.Windows.Visibility.Visible;
                cmbRegistrantType.IsEnabled = true;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.EMPLOYER; //WATCH THIS LINE FOR BUGS!
                cmbRegistrantType.Visibility = System.Windows.Visibility.Visible;

                //Show right column elements related to employer/employee registration; hide others
                lblFirstName.Visibility = System.Windows.Visibility.Visible;
                txtbxFirstName.IsEnabled = true;
                txtbxFirstName.Visibility = System.Windows.Visibility.Visible;
                lblLastName.Visibility = System.Windows.Visibility.Visible;
                txtbxLastName.IsEnabled = true;
                txtbxLastName.Visibility = System.Windows.Visibility.Visible;
                lblSchoolOrganization.Content = "Organization";
                lblSchoolOrganization.Visibility = System.Windows.Visibility.Visible;
                txtbxSchoolOrganization.IsEnabled = true;
                txtbxSchoolOrganization.Visibility = System.Windows.Visibility.Visible;
                lblClassStandingJobTitle.Content = "Job title";
                lblClassStandingJobTitle.Visibility = System.Windows.Visibility.Visible;
                txtbxJobTitle.Visibility = System.Windows.Visibility.Visible;
                txtbxJobTitle.IsEnabled = true;
                cmbClassStanding.Visibility = System.Windows.Visibility.Hidden;
                cmbClassStanding.IsEnabled = false;

                btnCheckIn.IsEnabled = true;

                return;
            }

            #endregion //EMPLOYER

            //Registrant is general
            #region GENERAL

            if (mode == Controller.RegistrantMode.GENERAL)
            {
                //Set GENERAL mode
                currentMode = Controller.RegistrantMode.GENERAL;

                //Show registrat type selector
                lblRegistrantType.Visibility = System.Windows.Visibility.Visible;
                cmbRegistrantType.IsEnabled = true;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.GENERAL; //WATCH THIS LINE FOR BUGS!
                cmbRegistrantType.Visibility = System.Windows.Visibility.Visible;

                //Show right column elements related to general registration
                lblFirstName.Visibility = System.Windows.Visibility.Visible;
                txtbxFirstName.IsEnabled = true;
                txtbxFirstName.Visibility = System.Windows.Visibility.Visible;
                lblLastName.Visibility = System.Windows.Visibility.Visible;
                txtbxLastName.IsEnabled = true;
                txtbxLastName.Visibility = System.Windows.Visibility.Visible;
                lblSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrganization.IsEnabled = false;
                lblClassStandingJobTitle.Visibility = System.Windows.Visibility.Hidden;
                txtbxJobTitle.Visibility = System.Windows.Visibility.Hidden;
                txtbxJobTitle.IsEnabled = false;
                cmbClassStanding.Visibility = System.Windows.Visibility.Hidden;
                cmbClassStanding.IsEnabled = false;

                btnCheckIn.IsEnabled = true;

                return;
            }

            #endregion //GENERAL

            //User is new registrant
            #region REGISTER

            if (mode == Controller.RegistrantMode.REGISTER)
            {
                //Set REGISTER mode
                currentMode = Controller.RegistrantMode.REGISTER;

                //Show registrat type selector
                lblRegistrantType.Visibility = System.Windows.Visibility.Visible;
                cmbRegistrantType.IsEnabled = true;
                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.RESET; //WATCH THIS LINE FOR BUGS!
                cmbRegistrantType.Visibility = System.Windows.Visibility.Visible;

                //Hide all other right column elements until registrant type is selected
                lblFirstName.Visibility = System.Windows.Visibility.Hidden;
                txtbxFirstName.IsEnabled = false;
                txtbxFirstName.Visibility = System.Windows.Visibility.Hidden;
                lblLastName.Visibility = System.Windows.Visibility.Hidden;
                txtbxLastName.IsEnabled = false;
                txtbxLastName.Visibility = System.Windows.Visibility.Hidden;
                lblSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrganization.Visibility = System.Windows.Visibility.Hidden;
                txtbxSchoolOrganization.IsEnabled = false;
                lblClassStandingJobTitle.Visibility = System.Windows.Visibility.Hidden;
                txtbxJobTitle.Visibility = System.Windows.Visibility.Hidden;
                txtbxJobTitle.IsEnabled = false;
                cmbClassStanding.IsEnabled = false;
                cmbClassStanding.Visibility = System.Windows.Visibility.Hidden;

                btnCheckIn.IsEnabled = false;

                return;
            }

            #endregion //REGISTER
        }

        /* Phillip: Some of these are here to shush the compiler while I rework the kiosk interface
           Some of these are testing/mock-up methods */
        #region DUMMY/MOCK-UP VARIABLES AND METHOD STUBS

        //This method is a mock-up for demonstrating different use cases
        private void MockCheckIn()
        {
            if (txtbxEnterCode.Text == "111111")
            {
                SetMode(Controller.RegistrantMode.STUDENT);

                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.STUDENT;
                txtbxFirstName.Text = "Hiro";
                txtbxLastName.Text = "Hamada";
                txtbxSchoolOrganization.Text = "SFIT";
                cmbClassStanding.SelectedIndex = (int)Controller.ClassStanding.SENIOR;

                lblMessages.Content = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Please verify your registration info.",
                    "When you are ready, click 'Check In' to finish.");
            }
            else if (txtbxEnterCode.Text == "222222")
            {
                SetMode(Controller.RegistrantMode.EMPLOYER);

                cmbRegistrantType.SelectedIndex = (int)Controller.RegistrantMode.EMPLOYER;
                txtbxFirstName.Text = "Jean-Baptiste";
                txtbxLastName.Text = "Zorg";
                txtbxSchoolOrganization.Text = "Zorg Co.";
                txtbxJobTitle.Text = "CEO";

                lblMessages.Content = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Please verify your registration info.",
                    "When you are ready, click 'Check In' to finish.");
            }
            else if (txtbxEnterCode.Text == "333333")
            {
                SetMode(Controller.RegistrantMode.GENERAL);

                txtbxFirstName.Text = "Guy";
                txtbxLastName.Text = "Personson";

                lblMessages.Content = String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Please verify your registration info.",
                    "When you are ready, click 'Check In' to finish.");
            }
            else
            {
                lblMessages.Content = String.Format("{1}{0}{2}{0}{3}",
                    Environment.NewLine,
                    "Registrant not found!",
                    "Please check your registration number and try again.",
                    "If the problem persists, click 'Register' to continue.");
            }
        }

        #endregion
    }
}
