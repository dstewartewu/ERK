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
    /// <summary>
    /// Interaction logic for Window_NoCode.xaml
    /// </summary>
    public partial class Window_NoCode : Window
    {
        #region DATA MEMBERS

        //Reference to main project controller
        private Controller controller;

        #endregion

        #region INITIALIZATION

        private Window_NoCode()
        {
            InitializeComponent();
        }

        public Window_NoCode(Controller controller_in)
        {
            InitializeComponent();

            controller = controller_in;

            txtbxMessages.Text = "Enter your email below.";
        }

        #endregion

        #region EVENT HANDLERS

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            controller.SetView(Controller.WindowView.KIOSK);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            //PG: DEV: IMPLEMENT REGISTRANT SEARCH BY EMAIL
            if (txtbxEmail.Text.Length == 0)
            {
                MessageBox.Show("You didn't put anything in the search box.");
            }
            else
            {
                MessageBox.Show("Yay, it worked!" + Environment.NewLine + "Or it didn't. I don't know");
            }
        }

        #endregion
    }
}
