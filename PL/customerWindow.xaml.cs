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
        public Customer Customer { get; set; } = new();//customer for binding
        private bool _close { get; set; } = false;//for closing the window

        private MenuWindow customerListWindow;//brings the menu window of the customer list

        private int Index;
        public int sizeH { get; set; }//hight of the window
        public int sizeW { get; set; }//width of the window

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="customer"></param>
        public CustomerWindow(BlApi.IBL bL, Customer customer)
        {
            bl = bL;
            Customer = customer;
            sizeH = 400; sizeW = 500;
            DataContext = this;
            InitializeComponent();
            TXTPhone.IsEnabled = false;
            TXTName.IsEnabled = false;
            btnUpdate.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// ctor for update
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="drone">selected drone</param>
        /// <param name="_droneListWindow">access to the window</param>
        /// <param name="_Index">the drone index</param>
        public CustomerWindow(BlApi.IBL bL, Customer customer, MenuWindow _customerListWindow, int _Index)//update ctor
        {
            bl = bL;
            Customer = customer;
            Index = _Index;
            if (_Index == 0)
            {
                Customer.Location = new();
                sizeW = 340; sizeH = 370;
            }
            else
            {
                sizeH = 400; sizeW = 500;
            }
            DataContext = this;
            InitializeComponent();
            this.customerListWindow = _customerListWindow;
        }
        /// <summary>
        /// ctor for add
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="droneListWindow">access to the window</param>
        public CustomerWindow(BlApi.IBL bL, MenuWindow customerListWindow) : this(bL, new(), customerListWindow, 0)//sends to the add ctor
        {
            bl = bL;
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
                customerListWindow.customerToLists = new(bl.GetListCustomer());//updates the pl list of customers
                customerListWindow.CustomerListView.ItemsSource = customerListWindow.customerToLists;//updates the view list
                MessageBox.Show("The customer has been updated successfully :)\n" + Customer.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)//faild
            {
                MessageBox.Show("Failed to update the customer: " + ex.GetType().Name + "\n" + ex.Message);
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
                    customerListWindow.customerToLists.Add(bl.GetListCustomer(c => c.Id == Customer.Id).Last());
                    customerListWindow.customerToLists = new(bl.GetListCustomer().OrderBy(item => item.Id));//Sort the list by ID
                    customerListWindow.CustomerListView.ItemsSource = customerListWindow.customerToLists;//updates the view list
                    //success
                    MessageBox.Show("The customer has been added successfully :)\n" + Customer.ToString());
                    _close = true;
                    try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
                }
            }
            catch (AskRecoverExeption ex)//if the customer is recovering
            {
                string message = ex.Message;
                string caption = "Confirmation";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    bl.CustonerRecover(Customer);
                    customerListWindow.customerToLists.Add(bl.GetListCustomer(c => c.Id == Customer.Id).Last());//adds
                    customerListWindow.customerToLists.OrderBy(item => item.Id);// Sort the list by ID
                    customerListWindow.CustomerListView.ItemsSource = customerListWindow.customerToLists;//updates the view list

                    //success
                    MessageBox.Show("The customer has been recovered successfully :)\n" + Customer.ToString());
                    _close = true;
                    try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
                }
                else
                {
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

        /// <summary>
        /// shows the parcel that is by the customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown_sent(object sender, MouseButtonEventArgs e)
        {
            if (Customer.FromCustomer.ToList().Count != 0)
                new ParcelByCustomerWindow(Customer.FromCustomer.ToList(), bl, customerListWindow).Show();
            else
                MessageBox.Show("The cutomer didn't send any parcels!");
        }

        /// <summary>
        /// shows the parcel that is sent to the customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown_received(object sender, MouseButtonEventArgs e)
        {
            if (Customer.ToCustomer.ToList().Count != 0)
                new ParcelByCustomerWindow(Customer.ToCustomer.ToList(), bl, customerListWindow).Show();
            else
                MessageBox.Show("The cutomer didn't receive any parcels!");
        }

        /// <summary>
        /// remove button for removing a customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.RemoveCustomer(Customer);
                customerListWindow.customerToLists = new(bl.GetListCustomer());
                customerListWindow.customerToLists.OrderBy(item => item.Id);// Sort the list by ID
                customerListWindow.CustomerListView.ItemsSource = customerListWindow.customerToLists;//updates the view list
                MessageBox.Show("The customer has been removed successfully :)\n" + Customer.ToString());
                _close = true;
                Close();
            }
            catch (Exception ex)//faild
            {
                MessageBox.Show("Failed to remove the customer: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }
    }
}
