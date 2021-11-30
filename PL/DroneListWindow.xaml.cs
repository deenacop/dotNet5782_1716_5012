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
            ComboStatusSelector.SelectedIndex = 0;
            ComboWeightSelector.SelectedIndex = 0;
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = NewMethod();
            DroneListView.ItemsSource = bL.ListDroneDisplay().Where(i => i.DroneStatus == status);
        }

        private void NewMethod()
        {
            DroneStatus status = (DroneStatus)ComboStatusSelector.SelectedItem;
            WeightCategories weight = (WeightCategories)ComboWeightSelector.SelectedItem;
            DroneListView.ItemsSource = null;
            DroneListView.ItemsSource = bL.ListDroneDisplay().Where(i => i.DroneStatus == status && );

        }

        private void WieghtSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            WeightCategories weight = (WeightCategories)ComboWeightSelector.SelectedItem;
            DroneListView.ItemsSource = null;
            DroneListView.ItemsSource = bL.ListDroneDisplay().Where(i => i.Weight==weight);
        }

    }
}
