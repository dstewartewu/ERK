using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistrationKiosk
{
    public class Controller
    {
        #region DATA MEMBERS

        //private enum ConnectionStatus { DISCONNECTED, CONNECTED, LOST }
        public enum WindowView { ADMIN, ADMIN_LOGIN, KIOSK, LOAD_EVENT, NO_CODE }
        public enum RegistrantMode { RESET, STUDENT, EMPLOYER, GENERAL, REGISTER }
        public enum ClassStanding { SELECT, FRESHMAN, SOPHOMORE, JUNIOR, SENIOR, POSTBAC, GRADUATE, ALUMNUS }
        private WindowView currentView;

        //These windows references are used to set which window the user can see at any given time
        private Window_Admin admin;
        private Window_AdminLogin login;
        private Window_Kiosk kiosk;
        private Window_EventLoader loadEvent;
        private Window_NoCode noCode;

        private Registrant currentRegistrant;
        private String api_url;
        private String eventKey;

        #endregion

        #region INITIALIZATION

        private Controller()
        {
            admin = null;
            login = null;
            kiosk = null;
            loadEvent = null;
            noCode = null;

            currentView = WindowView.ADMIN;
        }

        public Controller(Window_Admin admin_in)
        {
            admin = admin_in;
            login = new Window_AdminLogin(this);
            kiosk = new Window_Kiosk(this);
            loadEvent = new Window_EventLoader(this);
            noCode = new Window_NoCode(this);

            currentView = WindowView.ADMIN_LOGIN;

            //Hide and disable all windows at program start
            admin.Visibility = System.Windows.Visibility.Hidden;
            admin.IsEnabled = false;
            login.Visibility = System.Windows.Visibility.Hidden;
            login.IsEnabled = false;
            kiosk.Visibility = System.Windows.Visibility.Hidden;
            kiosk.IsEnabled = false;
            loadEvent.Visibility = System.Windows.Visibility.Hidden;
            loadEvent.IsEnabled = false;
            noCode.Visibility = System.Windows.Visibility.Hidden;
            noCode.IsEnabled = false;
        }

        #endregion

        #region VIEW CONTROL

        public void SetView(WindowView view_in)
        {
            //Enable and show window specified by paramters view_in
            switch (view_in)
            {
                case (WindowView.ADMIN):
                    admin.IsEnabled = true;
                    admin.Visibility = System.Windows.Visibility.Visible;
                    break;
                case (WindowView.ADMIN_LOGIN):
                    login.IsEnabled = true;
                    login.Visibility = System.Windows.Visibility.Visible;
                    break;
                case (WindowView.KIOSK):
                    kiosk.IsEnabled = true;
                    kiosk.Visibility = System.Windows.Visibility.Visible;
                    break;
                case (WindowView.LOAD_EVENT):

                    loadEvent.IsEnabled = true;
                    loadEvent.Visibility = System.Windows.Visibility.Visible;
                    break;
                case (WindowView.NO_CODE):
                    noCode.IsEnabled = true;
                    noCode.Visibility = System.Windows.Visibility.Visible;
                    break;
                default:
                    break;
            }

            //Hide and disable current window
            switch (currentView)
            {
                case (WindowView.ADMIN):
                    admin.Visibility = System.Windows.Visibility.Hidden;
                    admin.IsEnabled = false;
                    break;
                case (WindowView.ADMIN_LOGIN):
                    login.Visibility = System.Windows.Visibility.Hidden;
                    login.IsEnabled = false;
                    break;
                case (WindowView.KIOSK):
                    kiosk.Visibility = System.Windows.Visibility.Hidden;
                    kiosk.IsEnabled = false;
                    break;
                case (WindowView.LOAD_EVENT):
                    loadEvent.Visibility = System.Windows.Visibility.Hidden;
                    loadEvent.IsEnabled = false;
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

        #endregion

        /* Use this method to verify that api_url targets the correct API, and
         * verify valid eventKey.
         */
        private Boolean VerifyAccessCredentials()
        {
            //PG: DEV

            return false;
        }
    }
}
