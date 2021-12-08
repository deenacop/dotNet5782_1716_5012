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
using System.Collections.Specialized;

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
        public ObservableCollection<DroneToList> droneToLists;
        public DroneListWindow(IBL.IBL bl)
        {
            bL = bl;
            InitializeComponent();
            droneToLists = new ObservableCollection<DroneToList>();
            InitDrones();
            ComboStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            ComboWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            DroneListView.ItemsSource = droneToLists;
            ComboStatusSelector.SelectedIndex = 3;
            droneToLists.CollectionChanged += DroneListView_CollectionChanged;
        }

        private void DroneListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectionStatusAndWeight();
        }

        private void InitDrones()
        {
            List<DroneToList> temp = bL.GetDroneList().ToList();
            foreach (var item in temp)
            {
                droneToLists.Add(item);
            }
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
                DroneListView.ItemsSource = droneToLists;
            if (weight == WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.ToList().FindAll(i => i.Status == (IBL.BO.DroneStatus)status);
            if (weight != WeightCategories.All && status == DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.ToList().FindAll(i => i.Weight == (IBL.BO.WeightCategories)weight);
            if (weight != WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.ToList().FindAll(i => i.Status == (IBL.BO.DroneStatus)status && i.Weight == (IBL.BO.WeightCategories)weight);
        }

        private void WieghtSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionStatusAndWeight();
        }

        private void BTNAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new SingleDroneWindow(bL, this).Show();
        }
        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // ListViewItem item = (ListViewItem)sender;
            DroneToList drone = (DroneToList)DroneListView.SelectedItem;
            if (drone != null)
                new SingleDroneWindow(bL, bL.GetDrone(drone.Id), this, DroneListView.SelectedIndex).Show();
            //    if (new SingleDroneWindow(bL, bL.GetDrone(drone.Id)).ShowDialog() ?? false)
            DroneListView.ItemsSource = bL.GetDroneList();
        }

        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
        }
    }
}