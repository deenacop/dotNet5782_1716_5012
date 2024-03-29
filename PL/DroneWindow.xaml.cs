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
using BlApi;
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

        //FilterByWeightAndStatus weightAndStatus;

        private MenuWindow droneListWindow;//brings the menu window of the drone list

        private MenuWindow stationListWindow;

        private MenuWindow parcelListWindow;


        private int Index;

        public int sizeH { get; set; }//hight window
        public int sizeW { get; set; }//width window

        BackgroundWorker AutoRun;
        private void UpdatedTask() => AutoRun.ReportProgress(0); // invokes report progress action for thread updating
        private bool chekEnd() => AutoRun.CancellationPending; // returns whether to cancel yet or not

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
               
                bl.UpdateDrone(Drone);
                droneListWindow.droneToLists = bl.GetDroneList();
                CollectionView view;
                PropertyGroupDescription groupDescription;
                droneListWindow.GroupingDrone(out view, out groupDescription);
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
                droneListWindow.droneToLists = bl.GetDroneList();
                CollectionView view;
                PropertyGroupDescription groupDescription;
                droneListWindow.GroupingDrone(out view, out groupDescription);//grouping the list
                droneListWindow.DroneListView.Items.Refresh();
                droneListWindow.stationToLists = bl.GetBaseStationList();
                droneListWindow.SelectionAvailablity();
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
                droneListWindow.droneToLists = bl.GetDroneList();
                droneListWindow.stationToLists = bl.GetBaseStationList();
                droneListWindow.SelectionAvailablity();
                CollectionView view;
                PropertyGroupDescription groupDescription;
                droneListWindow.GroupingDrone(out view, out groupDescription);
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
                droneListWindow.droneToLists = bl.GetDroneList();
                CollectionView view;
                PropertyGroupDescription groupDescription;
                droneListWindow.GroupingDrone(out view, out groupDescription);//grouping the list
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
                CollectionView view;
                PropertyGroupDescription groupDescription;
                droneListWindow.GroupingDrone(out view, out groupDescription);
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
                droneListWindow.droneToLists = bl.GetDroneList();
                CollectionView view;
                PropertyGroupDescription groupDescription;
                droneListWindow.GroupingDrone(out view, out groupDescription);
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
        private void Image_MouseDown(object sender, RoutedEventArgs e)
        {
            if (Drone.Status == BO.DroneStatus.Delivery)//if there is a parcel
            {
                new ParcelInTransferWindow(Drone.Parcel, bl, droneListWindow).Show();
            }
            else//if there is no parcel a message will be displayed to the user
            {
                MessageBox.Show("There is no parcel in transfer");

            }
        }

        /// <summary>
        /// ctor for update
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="drone">selected drone</param>
        /// <param name="_droneListWindow">access to the window</param>
        /// <param name="_Index">the drone index</param>
        public DroneWindow(BlApi.IBL bL, Drone drone, MenuWindow _droneListWindow, MenuWindow _stationListWindow, MenuWindow _menuWindow , int _Index)
        {
            bl = bL;
            Drone = drone;
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
            this.stationListWindow = _stationListWindow;
            this.parcelListWindow = _menuWindow;
            comboWeight.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            comboStationSelector.ItemsSource = bl.GetBaseStationList().Select(s => s.Id);

        }
        #endregion

        #region add
        /// <summary>
        /// ctor for add
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="droneListWindow">access to the window</param>
        public DroneWindow(BlApi.IBL bL, MenuWindow droneListWindow, MenuWindow stationListWindow, MenuWindow parcelListWindow) : this(bL, new(), droneListWindow, stationListWindow, parcelListWindow, 0)//sends to the other ctor
        {
            comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            txtId.IsEnabled = true;
            //show the add grid
            AddGrid.Visibility = Visibility.Visible;
            UpdateGrid.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Collapsed;
            droneOptions.Visibility = Visibility.Collapsed;
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
                if (comboStationSelector.SelectedItem == null || txtID == null || txtMODEL == null || comboWeightSelector == null)
                {
                    MessageBox.Show("Missing drone details: ");
                }
                else
                {
                    //sending for add
                    bl.AddDrone(Drone, (int)comboStationSelector.SelectedItem);
                    CollectionView view;
                    PropertyGroupDescription groupDescription;
                    droneListWindow.GroupingDrone(out view, out groupDescription);
                    droneListWindow.droneToLists = bl.GetDroneList();
                    stationListWindow.stationToLists = bl.GetBaseStationList();
                    stationListWindow.SelectionAvailablity();//to update the stations list
                    //succe
                    MessageBox.Show("The drone has been added successfully :)\n" + Drone.ToString());

                    _close = true;
                    try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
                }
            }
            catch (AskRecoverExeption ex)//to recover the deleted drone
            {
                string message = ex.Message;
                string caption = "Confirmation";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    bl.DroneRecover(Drone, (int)comboStationSelector.SelectedItem);
                    CollectionView view;
                    PropertyGroupDescription groupDescription;
                    droneListWindow.GroupingDrone(out view, out groupDescription);
                    droneListWindow.droneToLists = bl.GetDroneList();
                    stationListWindow.stationToLists = bl.GetBaseStationList();//to update the stations list
                    stationListWindow.SelectionAvailablity();
                    MessageBox.Show("The drone has been recovered successfully :)\n" + Drone.ToString());
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
        #endregion
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
        /// to show the detailes of the drone with out changing anything
        /// </summary>
        /// <param name="bL"></param>
        /// <param name="drone"></param>
        public DroneWindow(IBL bL, Drone drone)
        {
            bl = bL;
            Drone = drone;
            sizeH = 400; sizeW = 500;
            DataContext = this;
            InitializeComponent();
            comboWeight.SelectedItem = drone.Weight;
            comboWeight.IsEnabled = false;
            TXTModel.IsEnabled = false;
            btnUpdate.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// removes the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.RemoveDrone(Drone);
                droneListWindow.GroupingDrone(out CollectionView view, out PropertyGroupDescription groupDescription);
                stationListWindow.stationToLists = bl.GetBaseStationList();
                stationListWindow.SelectionAvailablity();//to update the stations list
                MessageBox.Show("The drone has been removed successfully :)\n" + Drone.ToString());
                _close = true;
                Close();
            }
            catch (Exception ex)//faild
            {
                MessageBox.Show("Failed to remove the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        /// <summary>
        /// simulation of the drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Automatic_Click(object sender, RoutedEventArgs e)
        {
            AutoRun = new() { WorkerReportsProgress = true, WorkerSupportsCancellation = true };
            AutoRun.DoWork += AutoRun_DoWork;
            AutoRun.ProgressChanged += AutoRun_ProgressChanged;
            AutoRun.RunWorkerCompleted += AutoRun_RunWorkerCompleted;
            AutoRun.RunWorkerAsync(Drone.Id);
        }

        /// <summary>
        /// stop the simulation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Regular_Click(object sender, RoutedEventArgs e)
        {
            AutoRun?.CancelAsync();
        }

        /// <summary>
        /// updates the drone and the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoRun_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            Drone = bl.GetDrone(Drone.Id);
            UpdateGrid.DataContext = Drone;
            CollectionView view;
            PropertyGroupDescription groupDescription;
            droneListWindow.GroupingDrone(out view, out groupDescription);
            parcelListWindow.GroupingParcel();
            stationListWindow.stationToLists = bl.GetBaseStationList();
            stationListWindow.SelectionAvailablity();//to update the stations list
        }

        /// <summary>
        /// to start the simulation
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoRun_DoWork(object sender, DoWorkEventArgs e)
        {
            bl.StartSimulation(Drone.Id, chekEnd, UpdatedTask);
        }

        /// <summary>
        /// message box that tells the user that the simulation has finished
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutoRun_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("Drone management moved to manual");
        }
    }
}
