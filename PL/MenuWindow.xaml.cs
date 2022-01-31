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
        //public Dictionary<FilterByWeightAndStatus, List<DroneToList>> droneToLists;//a dictionary to present the list of drones
        //public Dictionary<FilterByPriorityAndStatus, List<ParcelToList>> parcelToList;//a dictionary to present the list of parcels
        public ObservableCollection<CustomerToList> customerToLists { get; set; } //an ObservableCollection to present the list of customers
        public IEnumerable<BaseStationToList> stationToLists { get; set; }//a list to present the list of stations
        public IEnumerable<DroneToList> droneToLists { get; set; }//a list to present the list of drones
        public IEnumerable<ParcelToList> parcelToList { get; set; }//a list to present the list of parcels
        DispatcherTimer timer;//
        BlApi.IBL bL;
        double panelWidth;
        bool hidden;//menu is open or close

        private bool _close { get; set; } = false;//for closing the window

        public MenuWindow(BlApi.IBL bl)
        {

            bL = bl;
            customerToLists = new(bL.GetListCustomer());
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

        #region logOut item selected
        private void logOut_Selected(object sender, RoutedEventArgs e)
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
            //grouping by status
            CollectionView view;
            PropertyGroupDescription groupDescription;
            GroupingDrone(out view, out groupDescription);
           
            droneGif.Visibility = Visibility.Collapsed;
            parcelLists.Visibility = Visibility.Collapsed;
            customerList.Visibility = Visibility.Collapsed;
            stationLists.Visibility = Visibility.Collapsed;
            droneLists.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
            //btnRemove.Visibility = Visibility.Visible;
        }

        internal void GroupingDrone(out CollectionView view, out PropertyGroupDescription groupDescription)
        {
            droneToLists = bL.GetDroneList().OrderBy(i => i.Status).ThenBy(i=>i.Id);//we want the items be sorted by ID
            DroneListView.ItemsSource = droneToLists;
            view = (CollectionView)CollectionViewSource.GetDefaultView(DroneListView.ItemsSource);
            groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void DroneListViewSelectedItem()
        {
            DroneToList drone = (DroneToList)DroneListView.SelectedItem;
            if (drone != null)
                new DroneWindow(bL, bL.GetDrone(drone.Id), this, 1).Show();
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
            droneGif.Visibility = Visibility.Collapsed;
            droneLists.Visibility = Visibility.Collapsed;
            parcelLists.Visibility = Visibility.Collapsed;
            stationLists.Visibility = Visibility.Collapsed;
            customerList.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
            // btnRemove.Visibility = Visibility.Visible;
        }
        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList customer = (CustomerToList)CustomerListView.SelectedItem;
            if (customer != null)
                new CustomerWindow(bL, bL.GetCustomer(customer.Id), this, 1).Show();
        }

        #endregion

        #region parcel item selected
        private void parcel_Selected(object sender, RoutedEventArgs e)
        {        
            //grouping by status
            GroupingParcel();
            droneGif.Visibility = Visibility.Collapsed;
            droneLists.Visibility = Visibility.Collapsed;
            stationLists.Visibility = Visibility.Collapsed;
            customerList.Visibility = Visibility.Collapsed;
            parcelLists.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
            // btnRemove.Visibility = Visibility.Visible;
        }

        internal void GroupingParcel()
        {
            parcelToList = bL.GetListParcel().OrderBy(i => i.Id);//we want that all the items will be sorted by ID
            ParcelListView.ItemsSource = parcelToList;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
        }

        private void ParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList parcel = (ParcelToList)ParcelListView.SelectedItem;
            if (parcel != null)
                new ParcelWindow(bL, bL.GetParcel(parcel.Id), this, 1).Show();
            // DroneListView.ItemsSource = bL.GetDroneList();
        }

        #endregion

        #region station item selected
        private void station_Selected(object sender, RoutedEventArgs e)
        {
            stationToLists = from item in bL.GetBaseStationList()
                             orderby item.NumOfAvailableChargingSlots
                             select item;
            comboAvailableSlostSelector.ItemsSource = Enum.GetValues(typeof(AvailablityStation));
            comboAvailableSlostSelector.SelectedIndex = 2;
            droneGif.Visibility = Visibility.Collapsed;
            droneLists.Visibility = Visibility.Collapsed;
            parcelLists.Visibility = Visibility.Collapsed;
            customerList.Visibility = Visibility.Collapsed;
            stationLists.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
            //btnRemove.Visibility = Visibility.Visible;
        }

        private void StationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseStationToList station = (BaseStationToList)StationListView.SelectedItem;
            if (station != null)
                new StationWindow(bL, bL.GetBaseStation(station.Id), this, 1).Show();
        }



        internal void SelectionAvailablity()
        {
            if (comboAvailableSlostSelector.SelectedIndex == -1)
                comboAvailableSlostSelector.SelectedIndex = 2;
            StationListView.ItemsSource = (AvailablityStation)comboAvailableSlostSelector.SelectedItem switch
            {
                AvailablityStation.All => stationToLists,
                AvailablityStation.Available => stationToLists.Where(s => s.NumOfAvailableChargingSlots > 0),
                AvailablityStation.Unavailable =>
                stationToLists.Where(s => s.NumOfAvailableChargingSlots == 0),
                _ => null
            };
        }
        private void comboAvailableSlostSelector_SelectionChanged(object sender, SelectionChangedEventArgs e) =>
            SelectionAvailablity();
        #endregion

        /// <summary>
        /// an add button sent each time to add another item. According to the SelectedItem
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {//Sends to the remove window 
            //check which grid is currently open
            if (menuListView.SelectedItem == drone)
            {
                new DroneWindow(bL, this).Show();
            }
            if (menuListView.SelectedItem == customer)
            {
                new CustomerWindow(bL, this).Show();
            }
            if (menuListView.SelectedItem == parcel)
            {
                new ParcelWindow(bL, this).Show();
            }
            if (menuListView.SelectedItem == station)
            {
                new StationWindow(bL, this).Show();
            }
        }     
       
    }
}

