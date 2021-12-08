using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class SingleDroneWindow : Window
    {
        IBL.IBL bl;
        public Drone Drone { get; set; }//drone for binding
        private bool _close { get; set; } = false;//for closing the window

        private DroneListWindow droneListWindow;

        private int Index;
        /// <summary>
        /// ctor of the update
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="drone">selected drone</param>
        /// <param name="_droneListWindow"></param>
        /// <param name="_Index">the drone index</param>
        public SingleDroneWindow(IBL.IBL bL, Drone drone, DroneListWindow _droneListWindow, int _Index)
        {
            bl = bL;
            Drone = drone;
            DataContext = this;
           
            Index = _Index;
            InitializeComponent();
            this.droneListWindow = _droneListWindow;
            UpdateGrid.Visibility = Visibility.Visible;
            comboWeight.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            comboStationSelector.ItemsSource = bl.GetBaseStationList().Select(s => s.Id);
        }
        /// <summary>
        /// ctor of add
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="droneListWindow"></param>
        public SingleDroneWindow(IBL.IBL bL, DroneListWindow droneListWindow) : this(bL, new(), droneListWindow, 0)
        {
            comboWeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            txtId.IsEnabled = true;
            AddGrid.Visibility = Visibility.Visible;
            UpdateGrid.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Collapsed;
            droneOptions.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// for changing the id txt box
        /// </summary>
        /// <param name="sender">wanted drone</param>
        /// <param name="e">wanted event</param>
        private void TxtId_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(txtId.Text, out int id);
            Drone.Id = id;
        }

        /// <summary>
        /// add butten
        /// </summary>
        /// <param name="sender">wanted drone</param>
        /// <param name="e">wanted event</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if not filled the details
                if (comboStationSelector.SelectedItem == null || txtID == null || txtMODEL==null || comboWeightSelector==null )
                {
                    MessageBox.Show("Missing drone details: ");
                }
                else
                {
                    //sending foe add
                    bl.AddDrone(Drone, (int)comboStationSelector.SelectedItem);
                    //success
                    MessageBox.Show("The drone has been added successfully :)\n" + Drone.ToString());
                    droneListWindow.droneToLists.Add(bl.GetDroneList().First(i => i.Id == Drone.Id));
                    //close window
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
        /// load the window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">wanted event</param>
        private void Window_Loaded(object sender, RoutedEventArgs e) { }

        /// <summary>
        /// The function is prevent force closing
        /// </summary>
        /// <param name="sender"> window</param>
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
        /// The update butten
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">wanted event</param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateDrone(Drone);
                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                DroneToList droneToList = droneListWindow.droneToLists[Index];
                droneToList.Model = Drone.Model;
                droneListWindow.droneToLists[Index] = droneToList;
                droneListWindow.DroneListView.Items.Refresh();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }
        /// <summary>
        /// update- send drone to charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">wanted event</param>
        private void btnSendDroneToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //send to the bl function
                bl.SendDroneToCharge(Drone);
                droneListWindow.DroneListView.Items.Refresh();
                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                //success
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)//faild
            {
                _close = true;
                MessageBox.Show("Failed to send the drone to charge: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        private void btnReleasingDroneFromBaseStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.ReleasingDroneFromBaseStation(Drone);
                droneListWindow.DroneListView.Items.Refresh();

                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send the drone to charge: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        private void btnCollectionParcelByDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.CollectionParcelByDrone(Drone);
                droneListWindow.DroneListView.Items.Refresh();

                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to send the drone to collect a parcel: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        private void btnAssignParcelToDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AssignParcelToDrone(Drone);
                droneListWindow.DroneListView.Items.Refresh();

                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to assign the drone to charge: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        private void btnDeliveryParcelByDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.DeliveryParcelByDrone(Drone);
                droneListWindow.DroneListView.Items.Refresh();

                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delivery the parcel by the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }
        /// <summary>
        /// prevents from the user to enter letters in the id box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">wanted event</param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        /// <summary>
        /// open the menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)  {      }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void showParcel_Click(object sender, RoutedEventArgs e)
        {
            if (Drone.Status == IBL.BO.DroneStatus.Delivery)
            {
                parcelDetails.Visibility = Visibility.Visible;
                btnReciver.Visibility = Visibility.Visible;
                btnSender.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("There is no parcel in transfer" );

            }
        }
        private void showSender_Click(object sender, RoutedEventArgs e)
        {
            senderDetails.Visibility = Visibility.Visible;
        }
        private void showreciver_Click(object sender, RoutedEventArgs e)
        {
            recieverDetails.Visibility = Visibility.Visible;
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

        private void btnCancel_Click_1(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }
    }
}