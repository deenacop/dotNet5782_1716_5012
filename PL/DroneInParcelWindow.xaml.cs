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
    /// Interaction logic for DroneInParcelWindow.xaml
    /// </summary>
    public partial class DroneInParcelWindow : Window
    {
        MenuWindow menuWindow;
        BlApi.IBL bL;
        DroneInParcel Drone;
        public DroneInParcelWindow(DroneInParcel drone, BlApi.IBL bl,MenuWindow menu)
        {
            InitializeComponent();
            bL = bl;
            DataContext = drone;
            Drone = drone;
            menuWindow = menu;
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(bL,bL.GetDrone(Drone.Id),menuWindow,1).Show();
        }
    }
}
