﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
using System.Text.RegularExpressions;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        BlApi.IBL bl;
        public BaseStation Station { get; set; }//drone for binding

        public BaseStationToList BaseStationToList { get; set; }

        private int Index;
        public int sizeH { get; set; }
        public int sizeW { get; set; }
        private bool _close { get; set; } = false;//for closing the window
        private MenuWindow stationListWindow;

        /// <summary>
        /// ctor for update
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="drone">selected drone</param>
        /// <param name="_droneListWindow">access to the window</param>
        /// <param name="_Index">the drone index</param>
        public StationWindow(BlApi.IBL bL, BaseStation station, MenuWindow _stationListWindow, int _Index)
        {
            bl = bL;
            Station = station;

            Index = _Index;
            if (_Index == 0)
            {
                Station.Location = new();
                sizeW = 340; sizeH = 370;
            }
            else
            {
                sizeH = 400; sizeW = 500;
            }
            DataContext = this;
            InitializeComponent();
            this.stationListWindow = _stationListWindow;
        }
        /// <summary>
        /// ctor for add
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="droneListWindow">access to the window</param>
        public StationWindow(BlApi.IBL bL, MenuWindow stationListWindow) : this(bL, new(), stationListWindow, 0)//sends to the other ctor
        {
            txtId.IsEnabled = true;
            //show the add grid
            AddGrid.Visibility = Visibility.Visible;
            UpdateGrid.Visibility = Visibility.Hidden;
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
                if (txtNumOfSlots == null || txtID == null || txtName == null || txtLongitude == null || txtLatitude == null)
                {
                    MessageBox.Show("Missing station details: ");
                }
                else
                {

                    //sending for add
                    bl.AddBaseStation(Station);
                    stationListWindow.stationToLists = bl.GetBaseStationList();
                    stationListWindow.SelectionAvailablity();
                    //success
                    MessageBox.Show("The station has been added successfully :)\n" + Station.ToString());

                    _close = true;
                    try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
                }
            }
            catch (AskRecoverExeption ex)
            {
                string message = ex.Message;
                string caption = "Confirmation";
                MessageBoxButton buttons = MessageBoxButton.YesNo;
                MessageBoxImage icon = MessageBoxImage.Question;
                if (MessageBox.Show(message, caption, buttons, icon) == MessageBoxResult.Yes)
                {
                    bl.StationRecover(Station);
                    stationListWindow.stationToLists = bl.GetBaseStationList();
                    stationListWindow.SelectionAvailablity();
                    //success
                    MessageBox.Show("The station has been recovered successfully :)\n" + Station.ToString());

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
                MessageBox.Show("Failed to add the station: " + ex.GetType().Name + "\n" + ex.Message);
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
                bl.UpdateStation(Station);
                stationListWindow.stationToLists = bl.GetBaseStationList();
                stationListWindow.SelectionAvailablity();
                MessageBox.Show("The station has been updated successfully :)\n" + Station.ToString());
                _close = true;
                Close();
            }
            catch (Exception ex)//faild
            {
                MessageBox.Show("Failed to update the station: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        /// <summary>
        /// view the drone in charging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown(object sender, RoutedEventArgs e)
        {
            if (Station.DronesInCharging.ToList().Count != 0)
                new DroneInChargingWindow(bl, Station.DronesInCharging.ToList()).Show();
            else
                MessageBox.Show("No drone are charging in this station ");
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.RemoveStation(Station);
                stationListWindow.stationToLists = bl.GetBaseStationList();
                stationListWindow.SelectionAvailablity();
                MessageBox.Show("The station has been removed successfully :)\n" + Station.ToString());
                _close = true;
                Close();
            }
            catch (Exception ex)//faild
            {
                MessageBox.Show("Failed to remove the station: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }
    }
}
