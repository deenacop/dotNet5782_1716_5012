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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BL;
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL.IBL bl;
        /// <summary>
        /// ctor for the window
        /// </summary>
        public MainWindow()
        {
            bl = new BL.BL();
            InitializeComponent();
        }
        /// <summary>
        /// Click event- a button to go to the List View window is pressed
        /// </summary>
        /// <param name="sender">the button for switching to the drones window </param>
        /// <param name="e">event</param>
        private void btnShowListDrone_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bl).Show();
        }

    }
}
