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
        public int StationID { get; set; }

        public SingleDroneWindow(IBL.IBL bL, Drone drone)
        {
            bl = bL;
            DataContext = this;
            Drone = drone;
            InitializeComponent();
            comboWeightSelector.ItemsSource = (IBL.BO.WeightCategories[])Enum.GetValues(typeof(IBL.BO.WeightCategories));
            comboStationSelector.ItemsSource = bl.GetBaseStationList().Select(s => s.StationID);
        }

        public SingleDroneWindow(IBL.IBL bL) : this(bL, new())
        {
            txtId.IsEnabled = true;
            pnlStationId.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
            btnUpdate.Visibility = Visibility.Collapsed;
            btnSendDroneToCharge.Visibility = Visibility.Collapsed;
            btnReleasingDroneFromBaseStation.Visibility = Visibility.Collapsed;
        }

        private void TxtId_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(txtId.Text, out int id);
            Drone.Id = id;
        }

        bool _close = false;
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.AddDrone(Drone, StationID);
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
    }
}
