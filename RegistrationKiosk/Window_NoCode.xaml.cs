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

        //Reference to kiosk
        Window_Kiosk kiosk;

        #endregion

        #region INITIALIZATION

        public Window_NoCode()
        {
            InitializeComponent();
        }

        public Window_NoCode(Window_Kiosk _kiosk)
        {
            InitializeComponent();

            kiosk = _kiosk;

            lblMessages.Content = "Enter your email below.";
        }

        #endregion

        #region EVENT HANDLERS

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            kiosk.IsEnabled = true;
            kiosk.Visibility = System.Windows.Visibility.Visible;

            this.Visibility = System.Windows.Visibility.Hidden;
            this.IsEnabled = false;
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
