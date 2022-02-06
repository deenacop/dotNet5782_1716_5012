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
    /// Interaction logic for ParcelByCustomerWindow.xaml
    /// To view a package at a customer
    /// </summary>
    public partial class ParcelByCustomerWindow : Window
    {

        BlApi.IBL bL;
        List<ParcelByCustomer> ParcelList;
        private bool _close { get; set; } = false;//for closing the window
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="parcelList"></param>
        /// <param name="bl"></param>
        /// <param name="menu"></param>
        public ParcelByCustomerWindow(List<ParcelByCustomer> parcelList, BlApi.IBL bl, MenuWindow menu)
        {
            InitializeComponent();
            bL = bl;
            ParcelList = new List<ParcelByCustomer>();
            ParcelList = parcelList;
            ParcelByCustomerView.ItemsSource = ParcelList;
        }
    }
}

