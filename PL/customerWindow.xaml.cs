using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.RegularExpressions;

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
        public Customer Customer { get; set; }//drone for binding
        private bool _close { get; set; } = false;//for closing the window
        private MenuWindow droneListWindow;
        public CustomerWindow(BlApi.IBL bL, MenuWindow _droneListWindow)
        {
            bl = bL;
            Customer = new();
            Customer.Location = new();
            DataContext = this;
            InitializeComponent();
            this.droneListWindow = _droneListWindow;
        }
        /// <summary>
        /// add butten
        /// </summary>
        /// <param name="sender">wanted drone</param>
        /// <param name="e">event</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if not filled the details
                if (txtPhone == null || txtID == null || txtName == null || txtLongitude == null || txtLatitude == null)
                {
                    MessageBox.Show("Missing drone details: ");
                }
                else
                {

                    //sending for add
                    bl.AddCustomer(Customer);

                    //success
                    MessageBox.Show("The drone has been added successfully :)\n" + Customer.ToString());

                    _close = true;
                    try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
                }
            }
            catch (Exception ex)//failed
            {
                MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        private void txtID_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(txtID.Text, out int id);
            Customer.Id = id;
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
        ///// <summary>
        ///// The function is prevent force closing
        ///// </summary>
        ///// <param name="sender">a window</param>
        ///// <param name="e">wanted event</param>
        //private void Window_Closing(object sender, CancelEventArgs e)
        //{
        //    if (!_close)
        //    {
        //        e.Cancel = true;
        //        MessageBox.Show("You can't force close the window");
        //    }
        //}
        /// <summary>
        /// Cancel button - closes a window
        /// </summary>
        /// <param name="sender">cancel butten</param>
        /// <param name="e">event</param>
        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

    }
   
    
}
