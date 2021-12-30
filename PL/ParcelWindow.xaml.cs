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
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        BlApi.IBL bl;
        public Parcel Parcel { get; set; }//drone for binding
        private bool _close { get; set; } = false;//for closing the window
        FilterByPriorityAndStatus PriorityAndStatus;
        private MenuWindow menuWindow;
        private int Index;
        public int sizeH { get; set; }
        public int sizeW { get; set; }
        public ParcelWindow(BlApi.IBL bL, Parcel parcel, MenuWindow _menuWindow, int _Index)
        {
            bl = bL;
            Parcel = parcel;
            Parcel.SenderCustomer = new();
            Parcel.TargetidCustomer = new();
            PriorityAndStatus = new();
            Index = _Index;
            if (_Index == 0)
            {
                sizeW = 340; sizeH = 370;
            }
            else
            {
                sizeH = 400; sizeW = 500;
            }
            DataContext = this;
            InitializeComponent();
            this.menuWindow = _menuWindow;
            //show the update grid:
            comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            comboPrioritySelector.ItemsSource = Enum.GetValues(typeof(BO.Priorities));

        }
        /// <summary>
        /// ctor for add
        /// </summary>
        /// <param name="bL">BL object</param>
        /// <param name="droneListWindow">access to the window</param>
        public ParcelWindow(BlApi.IBL bL, MenuWindow menuWindow) : this(bL, new(), menuWindow, 0)//sends to the other ctor
        {
            comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));
            txtSender.IsEnabled = true;
            txtReciver.IsEnabled = true;
            //show the add grid
            txtReciver.ItemsSource = bl.GetListCustomer().Select(i => i.Id);
            txtSender.ItemsSource = bl.GetListCustomer().Select(i => i.Id);
            AddGrid.Visibility = Visibility.Visible;
            //UpdateGrid.Visibility = Visibility.Hidden;
            //btnUpdate.Visibility = Visibility.Collapsed;
            //droneOptions.Visibility = Visibility.Collapsed;
        }


        /// <summary>
        /// add butten
        /// </summary>
        /// <param name="sender">wanted drone</param>
        /// <param name="e">event</param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if not filled the details
                if (comboPrioritySelector.SelectedItem == null || txtReciver == null || txtSender == null || comboWeightSelector == null)
                {
                    MessageBox.Show("Missing drone details: ");
                }
                else
                {
                    Parcel.SenderCustomer.Id = (int)txtSender.SelectedItem;
                    Parcel.TargetidCustomer.Id = (int)txtReciver.SelectedItem;
                    //sending for add
                    bl.AddParcel(Parcel);
                    PriorityAndStatus.Priority = Parcel.Priority;
                    PriorityAndStatus.Status = BO.ParcelStatus.Defined;
                    if (menuWindow.parcelToList.ContainsKey(PriorityAndStatus))
                        menuWindow.parcelToList[PriorityAndStatus].Add(bl.GetListParcel().Last());
                    else
                        menuWindow.parcelToList.Add(PriorityAndStatus, bl.GetListParcel().Where(i => i.Id == bl.GetListParcel().Last().Id).ToList());

                    menuWindow.SelectionStatusAndPriority();
                    //success
                    MessageBox.Show("The parcel has been added successfully :)\n" + Parcel.ToString());

                    _close = true;
                    try { DialogResult = true; } catch (InvalidOperationException) { Close(); }
                }
            }
            catch (Exception ex)//failed
            {
                MessageBox.Show("Failed to add the parcel: " + ex.GetType().Name + "\n" + ex.Message);
            }
        }

        /// <summary>
        /// load the window
        /// </summary>
        /// <param name="sender">the window</param>
        /// <param name="e">event</param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        /// <summary>
        /// The function is prevent force closing
        /// </summary>
        /// <param name="sender">a window</param>
        /// <param name="e">wanted event</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_close)
            {
                e.Cancel = true;
                MessageBox.Show("You can't force close the window");
            }
        }

        /// <summary>
        /// open the menu
        /// </summary>
        /// <param name="sender">menu</param>
        /// <param name="e">event</param>
        //private void MenuItem_Click(object sender, RoutedEventArgs e)
        //{
        //}

        /// <summary>
        /// Cancel button - closes a window
        /// </summary>
        /// <param name="sender">cancel butten</param>
        /// <param name="e">event</param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _close = true;
            Close();
        }
    }
}

