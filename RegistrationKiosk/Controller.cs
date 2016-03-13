using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationKiosk
{
    public class Controller
    {
        #region DATA MEMBERS

        public enum WindowView { START_MENU, KIOSK, REGISTER_KIOSK, NO_CODE }
        public enum RegistrantMode { RESET, STUDENT, EMPLOYEE, GENERAL, REGISTER }
        public enum ClassStanding { SELECT, FRESHMAN, SOPHOMORE, JUNIOR, SENIOR, POSTBACH, GRADUATE, ALUMNUS }
        private WindowView currentView;

        private Window_StartMenu startMenu;
        private Window_Kiosk kiosk;
        private Window_RegisterKiosk registerKiosk;
        private Window_NoCode noCode;

        private String api_url;
        private WebAPI webAPI;
        private Registrant activeRegistrant;

        private const String ERROR_LOG_PATH = "ERROR_LOG.txt";

        #endregion

        #region INITIALIZATION

        private Controller() { }

        public Controller(Window_StartMenu admin_in)
        {
            startMenu = admin_in;
            kiosk = new Window_Kiosk(this);
            registerKiosk = new Window_RegisterKiosk(this);
            noCode = new Window_NoCode(this);

            api_url = "http://www.timjunger.com/";
            webAPI = null;
            activeRegistrant = null;

            currentView = WindowView.START_MENU;

            //Hide and disable all windows at program start
            startMenu.Visibility = System.Windows.Visibility.Hidden;
            startMenu.IsEnabled = false;
            kiosk.Visibility = System.Windows.Visibility.Hidden;
            kiosk.IsEnabled = false;
            registerKiosk.Visibility = System.Windows.Visibility.Hidden;
            registerKiosk.IsEnabled = false;
            noCode.Visibility = System.Windows.Visibility.Hidden;
            noCode.IsEnabled = false;

            //Display initial window (admin)
            SetView(WindowView.START_MENU);
        }

        #endregion

        #region PROPERTIES

        public Registrant ActiveRegistrant
        {
            get { return activeRegistrant; }
            set { activeRegistrant = value; }
        }

        public String EventName
        {
            get
            {
                if (webAPI != null)
                {
                    return webAPI.Event.EventName;
                }
                else
                {
                    return "[OFFLINE]";
                }
            }
        }

        public Boolean IsOnlineEnabled
        {
            get
            {
                return webAPI != null;
            }
        }

        public WebAPI WebAPI
        {
            get { return webAPI; }
        }

        #endregion

        #region VIEW CONTROL

        public void SetView(WindowView view_in)
        {
            //Enable and show window specified by paramters view_in
            switch (view_in)
            {
                case (WindowView.START_MENU):
                    startMenu.IsEnabled = true;
                    startMenu.Visibility = System.Windows.Visibility.Visible;
                    startMenu.Focus();
                    break;
                case (WindowView.KIOSK):
                    kiosk.IsEnabled = true;
                    kiosk.Visibility = System.Windows.Visibility.Visible;
                    if(IsOnlineEnabled)
                    {
                        kiosk.txtbxEnterCode.Focus();
                    }
                    else
                    {
                        kiosk.cmbRegistrantType.Focus();
                    }
                    break;
                case (WindowView.REGISTER_KIOSK):
                    registerKiosk.IsEnabled = true;
                    registerKiosk.Visibility = System.Windows.Visibility.Visible;
                    registerKiosk.txtbxKioskCode.Focus();
                    break;
                case (WindowView.NO_CODE):
                    noCode.IsEnabled = true;
                    noCode.Visibility = System.Windows.Visibility.Visible;
                    noCode.txtbxEmail.Focus();
                    break;
                default:
                    break;
            }

            if(view_in != currentView)
            {
                //Hide and disable current window
                switch (currentView)
                {
                    case (WindowView.START_MENU):
                        startMenu.Visibility = System.Windows.Visibility.Hidden;
                        startMenu.IsEnabled = false;
                        break;
                    case (WindowView.KIOSK):
                        kiosk.Visibility = System.Windows.Visibility.Hidden;
                        kiosk.IsEnabled = false;
                        break;
                    case (WindowView.REGISTER_KIOSK):
                        registerKiosk.Visibility = System.Windows.Visibility.Hidden;
                        registerKiosk.IsEnabled = false;
                        break;
                    case (WindowView.NO_CODE):
                        noCode.Visibility = System.Windows.Visibility.Hidden;
                        noCode.IsEnabled = false;
                        break;
                    default:
                        break;
                }

                currentView = view_in;
            }
        }

        public void DisplayRegistrant()
        {
            SetView(WindowView.KIOSK);

            kiosk.DisplayRegistrant();
        }
        
        #endregion

        public void ClearRegistrant()
        {
            if (activeRegistrant != null)
                activeRegistrant = null;
        }

        public async void Connect(String kioskRegistration)
        {
            registerKiosk.SetMessage("Attempting to register kiosk...");

            try
            {
                webAPI = await WebAPI.CreateWebAPI(api_url, kioskRegistration);

                if(webAPI == null)
                {
                    registerKiosk.SetMessage(String.Format("{1}{0}{2}{0}{3}",
                        Environment.NewLine,
                        "Kiosk registration not found.",
                        "Please check your registration code and try again."));
                }
                else
                {
                    registerKiosk.SetMessage(String.Format("{1}{0}{2}",
                        Environment.NewLine,
                        "Kiosk registration successful!",
                        "Event \"" + EventName + "\" loaded."));

                    startMenu.Connect();
                    kiosk.Connect();
                    registerKiosk.Connect();
                    noCode.Connect();

                    SetView(WindowView.START_MENU);
                }
            }
            catch(System.Net.Http.HttpRequestException http_ex)
            {
                registerKiosk.SetMessage(String.Format("{1}{0}{2}",
                    Environment.NewLine,
                    "Online connection failed.",
                    "Please check your internet connection and try again."));

                LogError("Online connection failed.", http_ex.Message);
            }
            catch(Exception e)
            {
                registerKiosk.SetMessage(String.Format("{1}{0}{2}{0}{3}{0}{4}",
                    Environment.NewLine, 
                    "An error has occurred.",
                    "Please check your registration code and try again.",
                    "If errors continue, contact the system administrator",
                    "and check ERROR_LOG.txt in the kiosk program folder."));

                LogError(e.Message);
            }
        }

        public void Disconnect()
        {
            webAPI = null;
            ClearRegistrant();

            startMenu.Disconnect();
            kiosk.Disconnect();
            registerKiosk.Disconnect();
            noCode.Disconnect();
        }

        public void LogError(params String[] errorMessage)
        {
            if(!File.Exists(ERROR_LOG_PATH))
            {
                using(StreamWriter sw = File.CreateText(ERROR_LOG_PATH))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    foreach(String line in errorMessage)
                    {
                        sw.WriteLine(line);
                    }

                    sw.WriteLine();
                }
            }
            else
            {
                using(StreamWriter sw = File.AppendText(ERROR_LOG_PATH))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    foreach(String line in errorMessage)
                    {
                        sw.WriteLine(line);
                    }

                    sw.WriteLine();
                }
            }
        }

        public void UseCustomEventName(Boolean useCustom)
        {
            if(useCustom)
            {
                kiosk.SetCustomEventName(EventName);
            }
            else
            {
                kiosk.SetCustomEventName(null);
            }
        }
    }
}
