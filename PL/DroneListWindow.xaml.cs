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
    //public enum DroneStatus { Available, Maintenance, Delivery, All }
    ///// <summary>
    ///// The drone weights
    ///// </summary>
    //public enum WeightCategories { Light, Medium, Heavy, All };

    /// <summary>
    /// A class by which the items in the list are sorted. The key to the dictionary is an object of the class
    /// </summary>
    //public struct FilterByWeightAndStatus
    //{//Type of structure because you want to avoid reference
    //    public BO.WeightCategories Weight { get; set; }   
    //    public BO.DroneStatus Status { get; set; }
    //}

    public partial class DroneListWindow : Window
    {
        BlApi.IBL bL;
        public Dictionary<FilterByWeightAndStatus, List<DroneToList>> droneToLists;
        public DroneListWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            //bL = bl;
            //droneToLists = new Dictionary<FilterByWeightAndStatus, List<DroneToList>>();
            //InitDrones();//Sends to a function that will populate the dictionary
            //DroneListView.ItemsSource = droneToLists.Values.SelectMany(i=>i); 
            ////for the options in the combo text
            //ComboStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            ////for the options in the combo text
            //ComboWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));         
            ////Default - all
            //ComboStatusSelector.SelectedIndex = 3;
        }
        ///// <summary>
        ///// populate the ObservableCollection
        ///// </summary>
        //private void InitDrones()
        //{//The function executes a query,
        // //the query divides into groups according to the key.
        // //Recall that the key consists of a class that has weight and status.
        // //After creating a group, it converts it to a dictionary with a key and a list (its value)
        //    droneToLists = (from item in bL.GetDroneList()
        //     group item by new FilterByWeightAndStatus()
        //     {
        //         Weight = item.Weight,
        //         Status = item.Status
        //     }).ToDictionary(i => i.Key, i => i.ToList());
        //}
        /// <summary>
        /// Function that updates in case of a change in the list by resending for filtering
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void DroneListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        //{
        //    SelectionStatusAndWeight();
        //}
        
        ///// <summary>
        ///// Filter by status
        ///// </summary>
        ///// <param name="sender">wanded item from the combo box</param>
        ///// <param name="e">event</param>
        //private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    SelectionStatusAndWeight();
        //}
        ///// <summary>
        ///// Filter according to the two filters. Weight and status
        ///// </summary>
        //internal void SelectionStatusAndWeight()
        //{
        //    DroneStatus status = (DroneStatus)ComboStatusSelector.SelectedItem;
        //    //Sent before the second is updated anג its null
        //    if (ComboWeightSelector.SelectedIndex == -1)
        //    {
        //        ComboWeightSelector.SelectedIndex = 3;
        //    }
        //    WeightCategories weight = (WeightCategories)ComboWeightSelector.SelectedItem;
        //    DroneListView.ItemsSource = null;
        //    if (weight == WeightCategories.All && status == DroneStatus.All)
        //        DroneListView.ItemsSource = droneToLists.Values.SelectMany(i=>i);
        //    if (weight == WeightCategories.All && status != DroneStatus.All)
        //        DroneListView.ItemsSource = droneToLists.Where(i => i.Key.Status == (BO.DroneStatus)status).SelectMany(i=>i.Value);
        //    if (weight != WeightCategories.All && status == DroneStatus.All)
        //        DroneListView.ItemsSource = droneToLists.Where(i => i.Key.Weight == (BO.WeightCategories)weight).SelectMany(i => i.Value);
        //    if (weight != WeightCategories.All && status != DroneStatus.All)
        //        DroneListView.ItemsSource = droneToLists.Where(i => i.Key.Status == (BO.DroneStatus)status && i.Key.Weight == (BO.WeightCategories)weight).SelectMany(i => i.Value);
        //}
        ///// <summary>
        ///// Filter by wieght
        ///// </summary>
        ///// <param name="sender">wanded item from the combo box</param>
        ///// <param name="e">event</param>
        //private void WieghtSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    SelectionStatusAndWeight();
        //}
        /// <summary>
        /// Click event. Add button
        /// </summary>
        /// <param name="sender">Add button</param>
        /// <param name="e">event</param>
        //private void BTNAddDrone_Click(object sender, RoutedEventArgs e)
        //{
        //    //Sends to the add window 
        //    new DroneWindow(bL, this).Show();
        //}
        /// <summary>
        /// chosen drone
        /// </summary>
        /// <param name="sender">item from the list view</param>
        /// <param name="e">event</param>
        //private void DroneListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    DroneListViewSelectedItem();
        //}
        /// <summary>
        /// Double click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e">event</param>
//        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
//        {
//            DroneListViewSelectedItem();
//        }

//        private void DroneListViewSelectedItem()
//        {
//            DroneToList drone = (DroneToList)DroneListView.SelectedItem;
//            if (drone != null)
//                new DroneWindow(bL, bL.GetDrone(drone.Id), this, DroneListView.SelectedIndex).Show();
//            DroneListView.ItemsSource = bL.GetDroneList();
//        }
   }
}

