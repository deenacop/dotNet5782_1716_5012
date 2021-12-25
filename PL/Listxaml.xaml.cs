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
using BO;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace PL
{
    /// <summary>
    /// Interaction logic for Listxaml.xaml
    /// </summary>
    /// <summary>
    /// The Drone status(פנוי,תחוזקה,משלוח)
    /// </summary>
    public enum DroneStatus { Available, Maintenance, Delivery, All }
    /// <summary>
    /// The drone weights
    /// </summary>
    public enum WeightCategories { Light, Medium, Heavy, All };
    public partial class Listxaml : Window
    {
        BlApi.IBL bL;

        public ObservableCollection<DroneToList> droneToLists;
        public Listxaml(BlApi.IBL bl)
        {
            InitializeComponent();
            bL = bl;
            droneToLists = new ObservableCollection<DroneToList>();
            InitDrones();
            //for the options in the combo text
            ComboStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            //for the options in the combo text
            ComboWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            DroneListView.ItemsSource = droneToLists;
            //Default - all
            ComboStatusSelector.SelectedIndex = 3;
            //Event registration
            droneToLists.CollectionChanged += DroneListView_CollectionChanged;
        }

        /// <summary>
        /// Function that updates in case of a change in the list by resending for filtering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            SelectionStatusAndWeight();
        }
        /// <summary>
        /// convert to ObservableCollection
        /// </summary>
        private void InitDrones()
        {
            List<DroneToList> temp = bL.GetDroneList().ToList();
            foreach (var item in temp)
            {
                droneToLists.Add(item);
            }
        }
        /// <summary>
        /// Filter by status
        /// </summary>
        /// <param name="sender">wanded item from the combo box</param>
        /// <param name="e">event</param>
        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionStatusAndWeight();
        }
        /// <summary>
        /// Filter according to the two filters. Weight and status
        /// </summary>
        private void SelectionStatusAndWeight()
        {
            DroneStatus status = (DroneStatus)ComboStatusSelector.SelectedItem;
            //Sent before the second is updated anג its null
            if (ComboWeightSelector.SelectedIndex == -1)
            {
                ComboWeightSelector.SelectedIndex = 3;
            }
            WeightCategories weight = (WeightCategories)ComboWeightSelector.SelectedItem;
            DroneListView.ItemsSource = null;
            if (weight == WeightCategories.All && status == DroneStatus.All)
                DroneListView.ItemsSource = droneToLists;
            if (weight == WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.ToList().FindAll(i => i.Status == (BO.DroneStatus)status);
            if (weight != WeightCategories.All && status == DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.ToList().FindAll(i => i.Weight == (BO.WeightCategories)weight);
            if (weight != WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.ToList().FindAll(i => i.Status == (BO.DroneStatus)status && i.Weight == (BO.WeightCategories)weight);
        }
        /// <summary>
        /// Filter by wieght
        /// </summary>
        /// <param name="sender">wanded item from the combo box</param>
        /// <param name="e">event</param>
        private void WieghtSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionStatusAndWeight();
        }
        /// <summary>
        /// Click event. Add button
        /// </summary>
        /// <param name="sender">Add button</param>
        /// <param name="e">event</param>
        private void BTNAddDrone_Click(object sender, RoutedEventArgs e)
        {
            //Sends to the add window 
            new SingleDroneWindow(bL, this).Show();
        }
        /// <summary>
        /// chosen drone
        /// </summary>
        /// <param name="sender">item from the list view</param>
        /// <param name="e">event</param>
        private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneListViewSelectedItem();
        }
        /// <summary>
        /// Double click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">event</param>
        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneListViewSelectedItem();
        }

        private void DroneListViewSelectedItem()
        {
            DroneToList drone = (DroneToList)DroneListView.SelectedItem;
            if (drone != null)
                new SingleDroneWindow(bL, bL.GetDrone(drone.Id), this, DroneListView.SelectedIndex).Show();
            DroneListView.ItemsSource = bL.GetDroneList();
        }
    }

}
