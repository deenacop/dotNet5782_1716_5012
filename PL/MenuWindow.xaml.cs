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
    /// for filtering stations
    /// </summary>
    public enum AvailablityStation { Available, Unavailable, All }

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        public ObservableCollection<CustomerToList> customerToLists { get; set; } //an ObservableCollection to present the list of customers
        public IEnumerable<BaseStationToList> stationToLists { get; set; }//a list to present the list of stations
        public IEnumerable<DroneToList> droneToLists { get; set; }//a list to present the list of drones
        public IEnumerable<ParcelToList> parcelToList { get; set; }//a list to present the list of parcels
        DispatcherTimer timer;//
        BlApi.IBL bL;
        double panelWidth;
        bool hidden;//menu is open or close
        private bool _close { get; set; } = false;//for closing the window
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="bl"></param>
        public MenuWindow(BlApi.IBL bl)
        {
            bL = bl;
            customerToLists = new(bL.GetListCustomer().OrderBy(item => item.Id));
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);//Gets or sets the period of time between timer ticks.
            timer.Tick += Timer_Tick;
            panelWidth = sidePanel.Width;
        }

        #region menu open and close
        /// <summary>
        /// Open the menu.
        /// If the user presses, the timer immediately starts running.
        /// Until it reaches its maximum size the Boolean variable does not change. 
        /// When the Boolean variable arrives, the variable changes and the clock stops.
        /// And vice versa for closing a menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// start the timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }
        #endregion

        #region logOut item selected
        /// <summary>
        /// user select log out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// <summary>
        /// user select drone view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void drone_Selected(object sender, RoutedEventArgs e)
        {
            //grouping by status
            CollectionView view;
            PropertyGroupDescription groupDescription;
            GroupingDrone(out view, out groupDescription);
            comboAvailableSlostSelector.ItemsSource = Enum.GetValues(typeof(AvailablityStation));
            comboAvailableSlostSelector.SelectedIndex = 2;
            droneGif.Visibility = Visibility.Collapsed;
            parcelLists.Visibility = Visibility.Collapsed;
            customerList.Visibility = Visibility.Collapsed;
            stationLists.Visibility = Visibility.Collapsed;
            droneLists.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// display grouping
        /// </summary>
        /// <param name="view"></param>
        /// <param name="groupDescription"></param>
        internal void GroupingDrone(out CollectionView view, out PropertyGroupDescription groupDescription)
        {
            droneToLists = bL.GetDroneList().OrderBy(i => i.Status).ThenBy(i => i.Id);//we want the items be sorted by ID
            DroneListView.ItemsSource = droneToLists;
            view = (CollectionView)CollectionViewSource.GetDefaultView(DroneListView.ItemsSource);
            groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
        }
        /// <summary>
        /// display an individual drone
        /// </summary>
        private void DroneListViewSelectedItem()
        {
            DroneToList drone = (DroneToList)DroneListView.SelectedItem;
            if (drone != null)
                new DroneWindow(bL, bL.GetDrone(drone.Id), this,this,this, 1).Show();
        }
        /// <summary>
        /// display an indevidual drone
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DroneListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DroneListViewSelectedItem();
        }

        #endregion

        #region customer item selected
        /// <summary>
        /// user select customer view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void customer_Selected(object sender, RoutedEventArgs e)
        {

            droneGif.Visibility = Visibility.Collapsed;
            droneLists.Visibility = Visibility.Collapsed;
            parcelLists.Visibility = Visibility.Collapsed;
            stationLists.Visibility = Visibility.Collapsed;
            customerList.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// display an individual customer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CustomerListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            CustomerToList customer = (CustomerToList)CustomerListView.SelectedItem;
            if (customer != null)
                new CustomerWindow(bL, bL.GetCustomer(customer.Id), this, 1).Show();
        }

        #endregion

        #region parcel item selected
        /// <summary>
        /// user select parcel view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        }
        /// <summary>
        /// grouping display
        /// </summary>
        internal void GroupingParcel()
        {
            parcelToList = bL.GetListParcel().OrderBy(i => i.Id);//we want that all the items will be sorted by ID
            ParcelListView.ItemsSource = parcelToList;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
        }
        /// <summary>
        /// display individual parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelToList parcel = (ParcelToList)ParcelListView.SelectedItem;
            if (parcel != null)
                new ParcelWindow(bL, bL.GetParcel(parcel.Id), 1, this).Show();
        }

        #endregion

        #region station item selected
        /// <summary>
        /// user select station view
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void station_Selected(object sender, RoutedEventArgs e)
        {
            stationToLists = from item in bL.GetBaseStationList()
                             orderby item.NumOfAvailableChargingSlots
                             select item;
            comboAvailableSlostSelector.ItemsSource = Enum.GetValues(typeof(AvailablityStation));
            comboAvailableSlostSelector.SelectedIndex = 2;
            StationListView.ItemsSource = stationToLists;
            droneGif.Visibility = Visibility.Collapsed;
            droneLists.Visibility = Visibility.Collapsed;
            parcelLists.Visibility = Visibility.Collapsed;
            customerList.Visibility = Visibility.Collapsed;
            stationLists.Visibility = Visibility.Visible;
            btnAdd.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// display an individual station
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StationListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BaseStationToList station = (BaseStationToList)StationListView.SelectedItem;
            if (station != null)
                new StationWindow(bL, bL.GetBaseStation(station.Id), this, 1).Show();
        }

        /// <summary>
        /// selected a filter 
        /// </summary>
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

        /// <summary>
        /// comboBox chaenged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                new DroneWindow(bL, this, this,this).Show();
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

