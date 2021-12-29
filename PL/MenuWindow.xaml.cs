using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using BO;

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
    /// Delivery priorities
    /// </summary>
    public enum Priorities { Normal, Fast, Urgent, All };

    /// <summary>
    /// The parcel status (הוגדר,שויך,נאסף עי רחפן,סופק ללקוח)
    /// </summary>
    public enum ParcelStatus { Defined, Associated, PickedUp, Delivered, All }
    /// <summary>
    /// The station status af availability (for the comboBox)
    /// </summary>
    public enum AvailablityStation { Available, Unavailable, All }
    /// <summary>
    /// A class by which the items in the list are sorted. The key to the dictionary is an object of the class
    /// </summary>
    public struct FilterByWeightAndStatus
    {//Type of structure because you want to avoid reference
        public BO.WeightCategories Weight { get; set; }
        public BO.DroneStatus Status { get; set; }
    }
    public struct FilterByPriorityAndStatus
    {//Type of structure because you want to avoid reference
        public BO.Priorities Priority { get; set; }
        public BO.ParcelStatus Status { get; set; }
    }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public Dictionary<FilterByWeightAndStatus, List<DroneToList>> droneToLists;//a dictionary to present the list of drones
        public Dictionary<int, List<BaseStationToList>> sortedStationToLists;//a dictionary to present the sorted list of stations
        public Dictionary<FilterByPriorityAndStatus, List<ParcelToList>> parcelToList;//a dictionary to present the list of parcels
        public ObservableCollection<CustomerToList> customerToLists;//an ObservableCollection to present the list of customers
        public Dictionary<bool, List<BaseStationToList>> stationToLists;//a dictionary to present the list of stations
        DispatcherTimer timer;//
        BlApi.IBL bL;
        double panelWidth;
        bool hidden;//menu is open or close

        private bool _close { get; set; } = false;//for closing the window

        public MenuWindow(BlApi.IBL bl)
        {

            bL = bl;
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);//Gets or sets the period of time between timer ticks.
            timer.Tick += Timer_Tick;
            panelWidth = sidePanel.Width;
        }

        #region menu open and close
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                sidePanel.Width += 1;
                if (sidePanel.Width >= panelWidth)
                {
                    timer.Stop();
                    hidden = false;
                }
            }
            else
            {
                sidePanel.Width -= 1;
                if (sidePanel.Width <= 35)
                {
                    timer.Stop();
                    hidden = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }
        #endregion

        #region home item selected
        private void home_Selected(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
        #endregion

        #region drone item selected
        private void drone_Selected(object sender, RoutedEventArgs e)
        {
            droneToLists = new Dictionary<FilterByWeightAndStatus, List<DroneToList>>();
            InitDrones();//Sends to a function that will populate the dictionary
            //for the options in the combo text
            DroneListView.ItemsSource = droneToLists.Values.SelectMany(i => i);

            ComboStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            //for the options in the combo text
            ComboWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            //Default - all
            ComboStatusSelector.SelectedIndex = 3;
            parcelLists.Visibility = Visibility.Collapsed;
            customerList.Visibility = Visibility.Collapsed;
            stationLists.Visibility = Visibility.Collapsed;
            droneLists.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
            
        }



        /// <summary>
        /// populate the ObservableCollection
        /// </summary>
        private void InitDrones()
        {//The function executes a query,
         //the query divides into groups according to the key.
         //Recall that the key consists of a class that has weight and status.
         //After creating a group, it converts it to a dictionary with a key and a list (its value)
            droneToLists = (from item in bL.GetDroneList()
                            group item by new FilterByWeightAndStatus()
                            {
                                Weight = item.Weight,
                                Status = item.Status
                            }).ToDictionary(i => i.Key, i => i.ToList());
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
        internal void SelectionStatusAndWeight()
        {
            DroneStatus status = (DroneStatus)ComboStatusSelector.SelectedItem;
            //Sent before the second is updated anג its null
            if (ComboWeightSelector.SelectedIndex == -1)
            {
                ComboWeightSelector.SelectedIndex = 3;
            }
            WeightCategories weight = (WeightCategories)ComboWeightSelector.SelectedItem;
            DroneListView.ItemsSource = null;
            if (status == DroneStatus.All && weight == WeightCategories.All)
                DroneListView.ItemsSource = from item in droneToLists.Values.SelectMany(i => i)
                                            orderby ( item.Weight, item.Status)
                                            select item;
            if (weight == WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.Where(i => i.Key.Status == (BO.DroneStatus)status).SelectMany(i => i.Value);
            if (weight != WeightCategories.All && status == DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.Where(i => i.Key.Weight == (BO.WeightCategories)weight).SelectMany(i => i.Value);
            if (weight != WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = droneToLists.Where(i => i.Key.Status == (BO.DroneStatus)status && i.Key.Weight == (BO.WeightCategories)weight).SelectMany(i => i.Value);
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
        private void DroneListViewSelectedItem()
        {
            DroneToList drone = (DroneToList)DroneListView.SelectedItem;
            if (drone != null)
                new DroneWindow(bL, bL.GetDrone(drone.Id), this, DroneListView.SelectedIndex).Show();
            // DroneListView.ItemsSource = bL.GetDroneList();
        }
        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneListViewSelectedItem();
        }

        #endregion

        #region customer item selected
        private void customer_Selected(object sender, RoutedEventArgs e)
        {
            customerToLists = new ObservableCollection<CustomerToList>();
            IEnumerable<CustomerToList> tmp = bL.GetListCustomer();
            foreach (var current in tmp)
                customerToLists.Add(current);
            CustomerListView.ItemsSource = customerToLists;
            customerToLists.CollectionChanged += CustomerToLists_CollectionChanged;
            droneLists.Visibility = Visibility.Collapsed;
            parcelLists.Visibility = Visibility.Collapsed;
            stationLists.Visibility = Visibility.Collapsed;
            customerList.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
        }
        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList customer = (CustomerToList)CustomerListView.SelectedItem;
            if (customer != null)
                new CustomerWindow().Show();
            // DroneListView.ItemsSource = bL.GetDroneList();
        }

        private void CustomerToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            CustomerListView.Items.Refresh();
        }
        #endregion

        #region parcel item selected
        private void parcel_Selected(object sender, RoutedEventArgs e)
        {
            parcelToList = new Dictionary<FilterByPriorityAndStatus, List<ParcelToList>>();
            InitParcels();//Sends to a function that will populate the dictionary
            ParcelListView.ItemsSource = parcelToList.Values.SelectMany(i => i);
            //for the options in the combo text
            comboStatusSelector.ItemsSource = Enum.GetValues(typeof(ParcelStatus));
            //for the options in the combo text
            comboPrioritySelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            //Default - all
            comboStatusSelector.SelectedIndex = 4;
            droneLists.Visibility = Visibility.Collapsed;
            stationLists.Visibility = Visibility.Collapsed;
            customerList.Visibility = Visibility.Collapsed;
            parcelLists.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// populate the ObservableCollection
        /// </summary>
        private void InitParcels()
        {//The function executes a query,
         //the query divides into groups according to the key.
         //Recall that the key consists of a class that has weight and status.
         //After creating a group, it converts it to a dictionary with a key and a list (its value)
            parcelToList = (from item in bL.GetListParcel()
                            group item by new FilterByPriorityAndStatus()
                            {
                                Priority = item.Priority,
                                Status = item.Status
                            }).ToDictionary(i => i.Key, i => i.ToList());
        }
        /// <summary>
        /// Filter according to the two filters. Weight and status
        /// </summary>
        internal void SelectionStatusAndPriority()
        {
            ParcelStatus status = (ParcelStatus)comboStatusSelector.SelectedItem;
            //Sent before the second is updated anג its null
            if (comboPrioritySelector.SelectedIndex == -1)
            {
                comboPrioritySelector.SelectedIndex = 3;
            }
            Priorities priority = (Priorities)comboPrioritySelector.SelectedItem;
            DroneListView.ItemsSource = null;
            if (priority == Priorities.All && status == ParcelStatus.All)
                ParcelListView.ItemsSource = parcelToList.Values.SelectMany(i => i);
            if (priority == Priorities.All && status != ParcelStatus.All)
                ParcelListView.ItemsSource = parcelToList.Where(i => i.Key.Status == (BO.ParcelStatus)status).SelectMany(i => i.Value);
            if (priority != Priorities.All && status == ParcelStatus.All)
                ParcelListView.ItemsSource = parcelToList.Where(i => i.Key.Priority == (BO.Priorities)priority).SelectMany(i => i.Value);
            if (priority != Priorities.All && status != ParcelStatus.All)
                ParcelListView.ItemsSource = parcelToList.Where(i => i.Key.Status == (BO.ParcelStatus)status && i.Key.Priority == (BO.Priorities)priority).SelectMany(i => i.Value);
        }
        /// <summary>
        /// Filter by wieght
        /// </summary>
        /// <param name="sender">wanded item from the combo box</param>
        /// <param name="e">event</param>
        private void comboStatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionStatusAndPriority();

        }

        private void comboPrioritySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionStatusAndPriority();
        }

        private void ParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList parcel = (ParcelToList)ParcelListView.SelectedItem;
            if (parcel != null)
                new ParcelWindow().Show();
            // DroneListView.ItemsSource = bL.GetDroneList();
        }

        #endregion

        #region station item selected
        private void station_Selected(object sender, RoutedEventArgs e)
        {
            stationToLists = new Dictionary<bool, List<BaseStationToList>>();
            stationToLists = (from item in bL.GetBaseStationList()
                              group item by (item.NumOfAvailableChargingSlots > 0 ? true : false)).ToDictionary(i => i.Key, i => i.ToList());
            comboAvailableSlostSelector.ItemsSource = Enum.GetValues(typeof(AvailablityStation));
            StationListView.ItemsSource = stationToLists.Values.SelectMany(i => i);
            comboAvailableSlostSelector.SelectedIndex = 2;
            droneLists.Visibility = Visibility.Collapsed;
            parcelLists.Visibility = Visibility.Collapsed;
            customerList.Visibility = Visibility.Collapsed;
            stationLists.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
        }

        private void StationToLists_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            StationListView.Items.Refresh();
        }

        private void StationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseStationToList station = (BaseStationToList)StationListView.SelectedItem;
            if (station != null)
                new StationWindow().Show();
            // DroneListView.ItemsSource = bL.GetDroneList();
        }
        private void comboAvailableSlostSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            AvailablityStation availablity = (AvailablityStation)comboAvailableSlostSelector.SelectedItem;
            StationListView.ItemsSource = null;
            if (availablity == AvailablityStation.All)
                StationListView.ItemsSource = from item in stationToLists.Values.SelectMany(i => i)
                                              orderby (item.NumOfAvailableChargingSlots)
                                              select item;
            if (availablity == AvailablityStation.Available)
                StationListView.ItemsSource = stationToLists.Where(i => i.Key == true).SelectMany(i => i.Value);
            if (availablity == AvailablityStation.Unavailable)
                StationListView.ItemsSource = stationToLists.Where(i => i.Key == false).SelectMany(i => i.Value);
        }
        #endregion

        /// <summary>
        /// an add button sent each time to add another item. According to the SelectedItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {//Sends to the add window 
            //check which grid is currently open
            if (menuListView.SelectedItem == drone)
            {
                new tmpWindow("add").Show();
            }
            if (menuListView.SelectedItem == customer)
            {
                new CustomerWindow().Show();
            }
            if (menuListView.SelectedItem == parcel)
            {
                new ParcelWindow().Show();
            }
            if (menuListView.SelectedItem == station)
            {
                new StationWindow().Show();
            }
        }
    }
}

