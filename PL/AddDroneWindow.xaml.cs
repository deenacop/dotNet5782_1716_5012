using System;
using System.Collections.Generic;
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
    public partial class AddDroneWindow : Window
    {
        IBL.IBL bl;
        DroneToList newDrone = new();
        int StationID;

        public AddDroneWindow(IBL.IBL bL)
        {
            bl = bL;
            InitializeComponent();
            comboWeightSelector.ItemsSource = Enum.GetValues(typeof(IBL.BO.WeightCategories));
            List<int> ListstationID = new();
            foreach (BaseStationToList current in bl.ListBaseStationDisplay())
            {
                ListstationID.Add(current.StationID);
            }

            comboStationSelector.ItemsSource = ListstationID;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            newDrone.Weight = (IBL.BO.WeightCategories)comboWeightSelector.SelectedItem;
        }

        private void comboStationSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            StationID = (int)comboStationSelector.SelectedItem;
        }

        private void TXTID_TextChanged(object sender, TextChangedEventArgs e)
        {
            int.TryParse(TXTID.Text, out int ID);
            newDrone.DroneID = ID;
        }

        //private void TXTModel_TextChanged(object sender, TextChangedEventArgs e)
        //{
            
        //}

        private void BTNAdd_Click(object sender, RoutedEventArgs e)
        {
            bl.AddDrone(newDrone, StationID);
            //bl.ListDroneDisplay();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            newDrone.Model = TXTModel.Text;
        }
    }
}
