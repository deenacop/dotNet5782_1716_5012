﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using IBL.BO;

namespace PL
{
    /// <summary>
    /// Interaction logic for AddDroneWindow.xaml
    /// </summary>
    public partial class SingleDroneWindow : Window
    {
        IBL.IBL bl;
        public Drone Drone { get; set; }
        private bool _close { get; set; } = false;
        public SingleDroneWindow(IBL.IBL bL, Drone drone)
        {
            bl = bL;
            DataContext = this;
            Drone = drone;
            InitializeComponent();
            UpdateGrid.Visibility = Visibility.Visible;
            comboWeight.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            comboStationSelector.ItemsSource = bl.GetBaseStationList().Select(s => s.Id);
        }

        public SingleDroneWindow(IBL.IBL bL) : this(bL, new())
        {
            comboWeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            txtId.IsEnabled = true;
            AddGrid.Visibility = Visibility.Visible;
            UpdateGrid.Visibility = Visibility.Hidden;
            btnUpdate.Visibility = Visibility.Collapsed;
            droneOptions.Visibility = Visibility.Collapsed;
        }

        private void TxtId_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(txtId.Text, out int id);
            Drone.Id = id;
        }

     
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddDrone(Drone, (int)comboStationSelector.SelectedItem);
                MessageBox.Show("The drone has been added successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force close the window");
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateDrone(Drone);
                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        private void btnSendDroneToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.SendDroneToCharge(Drone);
                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)
            {
                _close = true;
                MessageBox.Show("Failed to send the drone to charge: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        private void btnReleasingDroneFromBaseStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.ReleasingDroneFromBaseStation(Drone, 50);
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
                MessageBox.Show("The drone has been updated successfully :)\n" + Drone.ToString());
                _close = true;
                try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delivery the parcel by the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        private void ExitClick(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        private void showParcel_Click(object sender, RoutedEventArgs e)
        {
            parcelLabel.Visibility = Visibility.Visible;
            parcelDetails.Visibility = Visibility.Visible;
        }

        private void showSender_Click(object sender, RoutedEventArgs e)
        {
            senderLAbel.Visibility = Visibility.Visible;
            senderDetails.Visibility = Visibility.Visible;

        }

        private void showReciever_Click(object sender, RoutedEventArgs e)
        {
            recieverLAbel.Visibility = Visibility.Visible;
            recieverDetails.Visibility = Visibility.Visible;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }
    }
}
