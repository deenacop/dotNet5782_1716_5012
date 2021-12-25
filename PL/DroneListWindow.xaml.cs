using BO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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

    public class FilterByWeightAndStatus
    {
        public BO.WeightCategories Weight { get; set; }   
        public BO.DroneStatus Status { get; set; }
    }
    public partial class DroneListWindow : Window
    {
        BlApi.IBL bL;

        public ObservableCollection<IGrouping<FilterByWeightAndStatus, DroneToList>> droneToLists;//public ObservableCollection 
        public DroneListWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            bL = bl;
            droneToLists = new ObservableCollection<IGrouping<FilterByWeightAndStatus, DroneToList>>();
            InitDrones();//Sends to a function that will populate the ObservableCollection
            DroneListView.ItemsSource = droneToLists.SelectMany(i=>i); 
            //for the options in the combo text
            ComboStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            //for the options in the combo text
            ComboWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
           
            //Default - all
            ComboStatusSelector.SelectedIndex = 3;
            //Event registration
            droneToLists.CollectionChanged += DroneListView_CollectionChanged;
        }
        /// <summary>
        /// populate the ObservableCollection
        /// </summary>
        private void InitDrones()
        {//The function executes a query,
         //the query divides into groups according to the key.
         //Recall that the key consists of a class that has weight and status.
         //After creating a group, it converts it to a list (currently on var defenition and cannot enter to the ObservableCollection).
         //After that puts on the ObservableCollection
            (from item in bL.GetDroneList()
             group item by new FilterByWeightAndStatus()
             {
                 Weight = item.Weight,
                 Status = item.Status
             }).ToList().ForEach(i => droneToLists.Add(i));
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
                DroneListView.ItemsSource = droneToLists.SelectMany(i=>i);
            if (weight == WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.Where(i => i.Key.Status == (BO.DroneStatus)status)/*.SelectMany(i=>i)*/;
            if (weight != WeightCategories.All && status == DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.Where(i => i.Key.Weight == (BO.WeightCategories)weight).SelectMany(i => i);
            if (weight != WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.Where(i => i.Key.Status == (BO.DroneStatus)status && i.Key.Weight == (BO.WeightCategories)weight).SelectMany(i => i);
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
            new DroneWindow(bL, this).Show();
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
                new DroneWindow(bL, bL.GetDrone(drone.Id), this, DroneListView.SelectedIndex).Show();
            DroneListView.ItemsSource = bL.GetDroneList();
        }
    }
}

