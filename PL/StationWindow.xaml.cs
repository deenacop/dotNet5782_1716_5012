using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BO;
using System.Text.RegularExpressions;

namespace PL
{
    /// <summary>
    /// Interaction logic for StationWindow.xaml
    /// </summary>
    public partial class StationWindow : Window
    {
        BlApi.IBL bl;
        public BaseStation Station { get; set; }//drone for binding
        private bool _close { get; set; } = false;//for closing the window


        private MenuWindow stationListWindow;
        public StationWindow(BlApi.IBL bL, MenuWindow _stationListWindow)
        {

            bl = bL;
            Station = new();
            Station.Location = new();
            Station.DronesInCharging = new();
            DataContext = this;
            InitializeComponent();
            this.stationListWindow = _stationListWindow;
        }
        /// <summary>
        /// add butten
        /// </summary>
        /// <param name="sender">wanted drone</param>
        /// <param name="e">event</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if not filled the details
                if (txtNumOfSlots == null || txtID == null || txtName == null || txtLongitude == null || txtLatitude == null)
                {
                    MessageBox.Show("Missing drone details: ");
                }
                else
                {

                    //sending for add
                    bl.AddBaseStation(Station);
                    bool key = Station.NumOfAvailableChargingSlots > 0 ? true : false;
                    if (stationListWindow.stationToLists.ContainsKey(key))
                        stationListWindow.stationToLists[key].Add(bl.GetBaseStationList().Last());
                    else
                        stationListWindow.stationToLists.Add(key, bl.GetBaseStationList().Where(i => i.Id == Station.Id).ToList());

                    stationListWindow.SelectionAvailablity();
                    //success
                    MessageBox.Show("The drone has been added successfully :)\n" + Station.ToString());

                    _close = true;
                    try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
                }
            }
            catch (Exception ex)//failed
            {
                MessageBox.Show("Failed to add the drone: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }


        /// <summary>
        /// prevents from the user to enter letters in the id box
        /// </summary>
        /// <param name="sender">text box</param>
        /// <param name="e">event</param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        /// <summary>
        /// The function is prevent force closing
        /// </summary>
        /// <param name="sender">a window</param>
        /// <param name="e">wanted event</param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force close the window");
            }
        }

        /// <summary>
        /// Cancel button - closes a window
        /// </summary>
        /// <param name="sender">cancel butten</param>
        /// <param name="e">event</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }
    }
}
