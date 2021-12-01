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
using IBL;
using IBL.BO;
namespace PL
{
    /// <summary>
    /// The Drone status(פנוי,תחוזקה,משלוח)
    /// </summary>
    public enum DroneStatus {Available, Maintenance, Delivery, All }
    /// <summary>
    /// The drone weights
    /// </summary>
    public enum WeightCategories { Light, Midium, Heavy, All };

    /// <summary>
    /// Interaction logic for DroneListWindow.xaml
    /// </summary>
    public partial class DroneListWindow : Window
    {
        IBL.IBL bL;
        public DroneListWindow(IBL.IBL blDrone)
        {
            bL = blDrone;
            InitializeComponent();
            ComboStatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            ComboWeightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            DroneListView.ItemsSource = bL.ListDroneDisplay();
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
                DroneListView.ItemsSource = bL.ListDroneDisplay();
            if (weight == WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = bL.ListDroneDisplay(i => i.DroneStatus == (IBL.BO.DroneStatus)status);
            if (weight != WeightCategories.All && status == DroneStatus.All)
                DroneListView.ItemsSource = bL.ListDroneDisplay(i => i.Weight == (IBL.BO.WeightCategories)weight);
            if (weight != WeightCategories.All && status != DroneStatus.All)
                DroneListView.ItemsSource = bL.ListDroneDisplay(i => i.DroneStatus == (IBL.BO.DroneStatus)status && i.Weight == (IBL.BO.WeightCategories)weight);

        }

        private void WieghtSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectionStatusAndWeight();
        }

        private void BTNAddDrone_Click(object sender, RoutedEventArgs e)
        {
            new AddDroneWindow(bL).Show();
        }
    }
}
