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
            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            DroneListView.ItemsSource = bL.ListDroneDisplay();
            StatusSelector.SelectedIndex = 0;
        }

        private void StatusSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DroneStatus status = (DroneStatus)StatusSelector.SelectedItem;
            DroneListView.ItemsSource = null;
            DroneListView.ItemsSource= bL.ListDroneDisplay().Where(i=>i.DroneStatus== status);
        }
    }
}
