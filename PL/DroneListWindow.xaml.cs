using System;
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
using System.Collections.ObjectModel;
using IBL;
using IBL.BO;
namespace PL
{
    /// <summary>
    /// The Drone status(פנוי,תחוזקה,משלוח)
    /// </summary>
    public enum DroneStatus { Available, Maintenance, Delivery, All }
    /// <summary>
    /// The drone weights
    /// </summary>
    public enum WeightCategories { Light, Medium, Heavy, All };

    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL.IBL bL;
        public DroneListWindow(IBL.IBL bl)
        {
            bL = bl;
            InitializeComponent();
            ComboStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            ComboWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            DroneListView.ItemsSource = bL.GetDroneList();
            ComboStatusSelector.SelectedIndex = 3;
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionStatusAndWeight();
        }

        private void SelectionStatusAndWeight()
        {
            DroneStatus status = (DroneStatus)ComboStatusSelector.SelectedItem;
            if (ComboWeightSelector.SelectedIndex == -1)
            {
                ComboWeightSelector.SelectedIndex = 3;
            }
            WeightCategories weight = (WeightCategories)ComboWeightSelector.SelectedItem;
            DroneListView.ItemsSource = null;
            if (weight == WeightCategories.All && status == DroneStatus.All)
                DroneListView.ItemsSource = bL.GetDroneList();
            if (weight == WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = bL.GetDroneList(i => i.Status == (IBL.BO.DroneStatus)status);
            if (weight != WeightCategories.All && status == DroneStatus.All)
                DroneListView.ItemsSource = bL.GetDroneList(i => i.Weight == (IBL.BO.WeightCategories)weight);
            if (weight != WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = bL.GetDroneList(i => i.Status == (IBL.BO.DroneStatus)status && i.Weight == (IBL.BO.WeightCategories)weight);
        }

        private void WieghtSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionStatusAndWeight();
        }

        private void BTNAddDrone_Click(object sender, RoutedEventArgs e)
        {
            if (new SingleDroneWindow(bL).ShowDialog() ?? false)
                DroneListView.ItemsSource = bL.GetDroneList();
        }

        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ListViewItem item = (ListViewItem)sender;
            DroneToList drone = (DroneToList)DroneListView.SelectedItem;
            if (new SingleDroneWindow(bL, bL.GetDrone(drone.Id)).ShowDialog() ?? false)
                DroneListView.ItemsSource = bL.GetDroneList();
        }

        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            DroneToList drone = item.DataContext as DroneToList;
            if (new SingleDroneWindow(bL, bL.GetDrone(drone.Id)).ShowDialog() ?? false)
                DroneListView.ItemsSource = bL.GetDroneList();
        }
    }
}
