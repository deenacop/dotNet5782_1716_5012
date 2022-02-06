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
    /// Interaction logic for ParcelInTransferWindow.xaml
    /// To view a package in delivery
    /// </summary>
    public partial class ParcelInTransferWindow : Window
    {
        BlApi.IBL bL;
        MenuWindow menuWindow;
        ParcelInTransfer parcelInTransfer;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="parcelIn"></param>
        /// <param name="bl"></param>
        /// <param name="menu"></param>
        public ParcelInTransferWindow(ParcelInTransfer parcelIn, BlApi.IBL bl, MenuWindow menu)
        {
            InitializeComponent();
            parcelInTransfer = parcelIn;
            DataContext = parcelIn;
            bL = bl;
            menuWindow = menu;
        }
        /// <summary>
        /// view the sender customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown_sender(object sender, MouseButtonEventArgs e)
        {
            new CustomerInParcelWindow(parcelInTransfer.SenderCustomer, bL, menuWindow).Show();
        }
        /// <summary>
        /// view the rerciever customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown_receiver(object sender, MouseButtonEventArgs e)
        {
            new CustomerInParcelWindow(parcelInTransfer.ReceiverCustomer, bL, menuWindow).Show();
        }
    }
}
