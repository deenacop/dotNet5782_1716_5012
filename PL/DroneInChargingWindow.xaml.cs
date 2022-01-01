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
        public DroneInCharging DroneInCharging { get; set; }//drone for binding
        BlApi.IBL bL;
        private bool _close { get; set; } = false;//for closing the window
        public DroneInChargingWindow(BlApi.IBL bl)
        {

            bL = bl;
            
            InitializeComponent(); ;

            DroneInChargingView = new();
            DroneInChargingLists = new();
            IEnumerable<DroneInCharging> tmp = bL.GetDroneInChargingList();
            foreach (var current in tmp)
                DroneInChargingLists.Add(current);
            DroneInChargingView.ItemsSource = DroneInChargingLists;
            DroneInChargingView.Items.Refresh();
        }
        

    }
}
