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
    /// </summary>
    public partial class ParcelInTransferWindow : Window
    {
        ParcelInTransfer parcelInTransfer; 
        public ParcelInTransferWindow(ParcelInTransfer parcelIn)
        {
            InitializeComponent();
            parcelInTransfer = parcelIn;
            DataContext = parcelIn;
        }
        

        private void Image_MouseDown_sender(object sender, MouseButtonEventArgs e)
        {
            new CustomerInParcelWindow(parcelInTransfer.SenderCustomer).Show();
        }

        private void Image_MouseDown_receiver(object sender, MouseButtonEventArgs e)
        {
            new CustomerInParcelWindow(parcelInTransfer.ReceiverCustomer).Show();
        }
    }
}
