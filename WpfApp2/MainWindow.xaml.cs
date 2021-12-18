using System;
using System.Windows;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        BlApi.IBL bl;
        /// <summary>
        /// ctor for the window
        /// </summary>
        public MainWindow()
        {
            bl = BlFactory.GetBl();
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
