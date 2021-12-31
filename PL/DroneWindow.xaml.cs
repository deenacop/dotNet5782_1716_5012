﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for DroneWindow.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        BlApi.IBL bl;
        public Drone Drone { get; set; }//drone for binding
        private bool _close { get; set; } = false;//for closing the window

        FilterByWeightAndStatus weightAndStatus;

        private MenuWindow droneListWindow;//brings the menu window of the drone list

        private int Index;

        public int sizeH { get; set; }
        public int sizeW { get; set; }
        #region update
        /// <summary>
        /// Click event-update butten
        /// </summary>
        /// <param name="sender">update butten</param>
        /// <param name="e"> event</param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // var item = droneListWindow.droneToLists.Where(i => i.Key.Status == Drone.Status && i.Key.Weight == Drone.Weight).first();
                bl.UpdateDrone(Drone);
                droneListWindow.DroneListView.Items.Refresh();
                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                Close();
            }
            catch (Exception ex)//faild
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        // <summary>
        /// Click event - update- send drone to charge
        /// </summary>
        /// <param name="sender">menu item</param>
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
        /// <summary>
        /// Click event - update- release drone frome charge
        /// </summary>
        /// <param name="sender">menu item</param>
        /// <param name="e">event</param>
        private void btnReleasingDroneFromBaseStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //send to the bl function
                bl.ReleasingDroneFromBaseStation(Drone);
                droneListWindow.DroneListView.Items.Refresh();
                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)//faild
            {
                MessageBox.Show("Failed to send the drone to charge: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }
        /// <summary>
        /// Click event - update - collect parcel
        /// </summary>
        /// <param name="sender">menu item</param>
        /// <param name="e">event</param>
        private void btnCollectionParcelByDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //send to the bl function
                bl.CollectionParcelByDrone(Drone);
                droneListWindow.DroneListView.Items.Refresh();
                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)//faild
            {
                MessageBox.Show("Failed to send the drone to collect a parcel: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }
        /// <summary>
        /// Click event- update - assign parcel to a drone
        /// </summary>
        /// <param name="sender">menu item</param>
        /// <param name="e">event</param>
        private void btnAssignParcelToDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //send to the bl function
                bl.AssignParcelToDrone(Drone);
                droneListWindow.DroneListView.Items.Refresh();
                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)//faild
            {
                MessageBox.Show("Failed to assign the drone to charge: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }
        /// <summary>
        /// Click event - update - deliver parcel
        /// </summary>
        /// <param name="sender">menu item</param>
        /// <param name="e">event</param>
        private void btnDeliveryParcelByDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //send to the bl function
                bl.DeliveryParcelByDrone(Drone);
                droneListWindow.DroneListView.Items.Refresh();
                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)//faild
            {
                MessageBox.Show("Failed to delivery the parcel by the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        /// <summary>
        /// Button for display parcel if exist. 
        /// </summary>
        /// <param name="sender">the butten</param>
        /// <param name="e">event</param>
        private void showParcel_Click(object sender, RoutedEventArgs e)
        {
            if (Drone.Status == BO.DroneStatus.Delivery)//if there is a parcel
            {
                //parcelDetails.Visibility = Visibility.Visible;
                //btnReciver.Visibility = Visibility.Visible;
                //btnSender.Visibility = Visibility.Visible;
            }
            else//if there is no parcel a message will be displayed to the user
            {
                MessageBox.Show("There is no parcel in transfer");

            }
        }
        /// <summary>
        /// Button for display sender
        /// </summary>
        /// <param name="sender">the butten</param>
        /// <param name="e">event</param>
        //private void showSender_Click(object sender, RoutedEventArgs e)
        //{
        //    senderDetails.Visibility = Visibility.Visible;
        //}
        /// <summary>
        /// Button for display reciver
        /// </summary>
        /// <param name="sender">the butten</param>
        /// <param name="e">event</param>
        //private void showreciver_Click(object sender, RoutedEventArgs e)
        //{
        //    recieverDetails.Visibility = Visibility.Visible;
        //}

        #endregion
        /// <summary>
        /// ctor for update
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="drone">selected drone</param>
        /// <param name="_droneListWindow">access to the window</param>
        /// <param name="_Index">the drone index</param>
        public DroneWindow(BlApi.IBL bL, Drone drone, MenuWindow _droneListWindow, int _Index)
        {
            bl = bL;
            Drone = drone;
            weightAndStatus = new();
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
            this.droneListWindow = _droneListWindow;
            //show the update grid:
            //UpdateGrid.Visibility = Visibility.Visible;
            comboWeight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            comboStationSelector.ItemsSource = bl.GetBaseStationList().Select(s => s.Id);

        }
        /// <summary>
        /// ctor for add
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="droneListWindow">access to the window</param>
        public DroneWindow(BlApi.IBL bL, MenuWindow droneListWindow) : this(bL, new(), droneListWindow, 0)//sends to the other ctor
        {
            comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            txtId.IsEnabled = true;
            //show the add grid
            AddGrid.Visibility = Visibility.Visible;
            //UpdateGrid.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Collapsed;
            droneOptions.Visibility = Visibility.Collapsed;
        }
        /// <summary>
        /// for changing the id txt box
        /// </summary>
        /// <param name="sender">wanted drone</param>
        /// <param name="e">event</param>
        //private void TxtId_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    int.TryParse(txtId.Text, out int id);
        //    Drone.Id = id;
        //}

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
                if (comboStationSelector.SelectedItem == null || txtID == null || txtMODEL == null || comboWeightSelector == null)
                {
                    MessageBox.Show("Missing drone details: ");
                }
                else
                {
                    
                    //sending for add
                    bl.AddDrone(Drone, (int)comboStationSelector.SelectedItem);
                    weightAndStatus.Weight = /*(WeightCategories)*/Drone.Weight;
                    weightAndStatus.Status = Drone.Status;
                    if (droneListWindow.droneToLists.ContainsKey(weightAndStatus))
                        droneListWindow.droneToLists[weightAndStatus].Add(bl.GetDroneList().First(i => i.Id == Drone.Id));
                    else
                        droneListWindow.droneToLists.Add(weightAndStatus,bl.GetDroneList().Where(i=>i.Id == Drone.Id).ToList());
                    
                    droneListWindow.SelectionStatusAndWeight();
                    //success
                    MessageBox.Show("The drone has been added successfully :)\n" + Drone.ToString());

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
        /// <param name="sender">the window</param>
        /// <param name="e">event</param>
        private void Window_Loaded(object sender, RoutedEventArgs e) 
        { 
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
        /// open the menu
        /// </summary>
        /// <param name="sender">menu</param>
        /// <param name="e">event</param>
        private void MenuItem_Click(object sender, RoutedEventArgs e) 
        { 
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
