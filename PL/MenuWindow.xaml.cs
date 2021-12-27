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
using System.Windows.Threading;

namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MenuWindow : Window
    {
        DispatcherTimer timer;//
        BlApi.IBL bL;
        double panelWidth;
        bool hidden;//menu is open or close
        private bool _close { get; set; } = false;//for closing the window

        public MenuWindow(BlApi.IBL bl)
        {

            bL = bl;
            InitializeComponent();
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);//Gets or sets the period of time between timer ticks.
            timer.Tick += Timer_Tick;
            panelWidth = sidePanel.Width;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (hidden)
            {
                sidePanel.Width += 1;
                if (sidePanel.Width >= panelWidth)
                {
                    timer.Stop();
                    hidden = false;
                }
            }
            else
            {
                sidePanel.Width -= 1;
                if (sidePanel.Width <= 35)
                {
                    timer.Stop();
                    hidden = true;
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            timer.Start();
        }

        private void dronsList_Click(object sender, RoutedEventArgs e)
        {
            new DroneListWindow(bL).Show();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void customer_Selected(object sender, RoutedEventArgs e)
        {
            adds.Visibility = Visibility.Visible;
        }

        private void drone_Selected(object sender, RoutedEventArgs e)
        {
            lists.Visibility = Visibility.Visible;
        }

        private void home_Selected(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }
    }
}

