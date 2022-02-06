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
using BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerInParcelWindow.xaml
    /// </summary>
    public partial class CustomerInParcelWindow : Window
    {
        MenuWindow menuWindow;
        BlApi.IBL bL;
        CustomerInParcel customer;
        public CustomerInParcelWindow(CustomerInParcel customerIn,BlApi.IBL bl, MenuWindow menu)
        {
            InitializeComponent();
            customer = customerIn;
            DataContext = customerIn;
            bL = bl;
            menuWindow = menu;
        }

        /// <summary>
        /// sends to the current customer window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new CustomerWindow(bL, bL.GetCustomer(customer.Id)).Show();
        }
    }
}
