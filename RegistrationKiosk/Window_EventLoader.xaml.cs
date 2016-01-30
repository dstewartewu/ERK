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
    public partial class Window_EventLoader : Window
    {
        #region DATA MEMBERS

        //Reference to main project controller
        private Controller controller;

        #endregion

        #region INITIALIZATION

        private Window_EventLoader()
        {
            InitializeComponent();
        }

        public Window_EventLoader(Controller controller_in)
        {
            InitializeComponent();

            controller = controller_in;

            txtbxMessages.Text = String.Format("{1}{0}{2}{0}{3}",
                Environment.NewLine,
                "No event loaded.",
                "To load an event, enter the API URL and",
                "Event Key below, then click 'Load'.");
        }

        #endregion

        #region EVENT HANDLERS

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("This button is just for show ATM." + Environment.NewLine + "'Open Kiosk' and 'Export to Excel' buttons have been enabled.");

            controller.SetView(Controller.WindowView.ADMIN);
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            controller.SetView(Controller.WindowView.ADMIN);
        }

        #endregion
    }
}
