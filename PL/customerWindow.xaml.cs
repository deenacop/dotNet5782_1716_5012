using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.ComponentModel;
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
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        BlApi.IBL bl;
        public Customer Customer { get; set; }//customer for binding
        private bool _close { get; set; } = false;//for closing the window
        
        private MenuWindow customerListWindow;//brings the menu window of the customer list
        
        private int Index;
        public int sizeH { get; set; }
        public int sizeW { get; set; }


        /// <summary>
        /// ctor for update
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="drone">selected drone</param>
        /// <param name="_droneListWindow">access to the window</param>
        /// <param name="_Index">the drone index</param>
        public CustomerWindow(BlApi.IBL bL, Customer customer, MenuWindow _customerListWindow, int _Index)
        {
            bl = bL;
            Customer = customer;

            Index = _Index;
            if (_Index == 0)
            {
                sizeW = 340; sizeH = 370;
            }
            else
            {
                sizeH = 400; sizeW = 500;
            }
            DataContext = this;
            InitializeComponent();
            this.customerListWindow = _customerListWindow;
            //show the update grid:
            //UpdateGrid.Visibility = Visibility.Visible;



        }
        /// <summary>
        /// ctor for add
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="droneListWindow">access to the window</param>
        public CustomerWindow(BlApi.IBL bL, MenuWindow customerListWindow) : this(bL, new(), customerListWindow, 0)//sends to the other ctor
        {
            bl = bL;
            Customer = new();
            Customer.Location = new();
            txtId.IsEnabled = true;
            AddGrid.Visibility = Visibility.Visible;
            UpdateGrid.Visibility = Visibility.Hidden;
       
        }

     

        /// <summary>
        /// Cancel button - closes a window
        /// </summary>
        /// <param name="sender">cancel butten</param>
        /// <param name="e">event</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

        /// <summary>
        /// Click event-update butten
        /// </summary>
        /// <param name="sender">update butten</param>
        /// <param name="e"> event</param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateCustomer(Customer);
                //stationListWindow.StationListView.Items.Refresh();
                MessageBox.Show("The station has been updated successfully :)\n" + Customer.ToString());
                
                _close = true;
                Close();
            }
            catch (Exception ex)//faild
            {
                MessageBox.Show("Failed to update the station: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }


        /// <summary>
        /// add butten
        /// </summary>
        /// <param name="sender">wanted customer</param>
        /// <param name="e">event</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if not filled the details
                if (txtPhone == null || txtID == null || txtName == null || txtLongitude == null || txtLatitude == null)
                {
                    MessageBox.Show("Missing customer details: ");
                }
                else
                {

                    //sending for add
                    bl.AddCustomer(Customer);
                    customerListWindow.customerToLists.Add(bl.GetListCustomer().Last());
                    //success
                    MessageBox.Show("The customer has been added successfully :)\n" + Customer.ToString());

                    _close = true;
                    try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
                }
            }
            catch (Exception ex)//failed
            {
                MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

       
        /// <summary>
        /// prevents from the user to enter letters in the id box
        /// </summary>
        /// <param name="sender">text box</param>
        /// <param name="e">event</param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// The function is prevent force closing
        /// </summary>
        /// <param name="sender">a window</param>
        /// <param name="e">wanted event</param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force close the window");
            }
        }
        private void Image_MouseDown_sent(object sender, MouseButtonEventArgs e)
        {
            if (Customer.FromCustomer.Count !=0)
                new ParcelByCustomerWindow(Customer.FromCustomer, bl, customerListWindow).Show();
            else
                MessageBox.Show("The cutomer didn't send any parcels!");
        }
        private void Image_MouseDown_received(object sender, MouseButtonEventArgs e)
        {
            if (Customer.ToCustomer.Count != 0)
                new ParcelByCustomerWindow(Customer.ToCustomer, bl, customerListWindow).Show();
            else
                MessageBox.Show("The cutomer didn't receive any parcels!");
        }
        
    }


}
