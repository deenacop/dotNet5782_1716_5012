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
using BO;

namespace PL
{

    /// <summary>
    /// Interaction logic for DroneInChargingWindow.xaml
    /// </summary>
    public partial class DroneInChargingWindow : Window
    {
        public List<DroneInCharging> DroneInChargingLists;

        BlApi.IBL bL;
        private bool _close { get; set; } = false;//for closing the window
        public DroneInChargingWindow(BlApi.IBL bl, List<DroneInCharging> droneInChargingLists)
        {
            InitializeComponent();
            bL = bl;
            DroneInChargingLists = new List<DroneInCharging>();
            DroneInChargingLists = droneInChargingLists;
            DroneInChargingView.ItemsSource = DroneInChargingLists;
        }

    }
}
