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
using System.Windows.Threading;
using System.Windows.Navigation;
using System.Diagnostics;

namespace PL
{
    /// <summary>
    /// Interaction logic for UserMainWindow.xaml
    /// </summary>
    public partial class UserMainWindow : Window
    {
        BlApi.IBL bL;
        public Parcel addParcel { get; set; }

        public Customer Customer { get; set; }//customer for binding

        User user;
        IEnumerable<ParcelByCustomer> ParcelListTo;
        IEnumerable<ParcelByCustomer> ParcelListFrom;
        private bool _close { get; set; } = false;//for closing the window
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="bl"></param>
        /// <param name="User"></param>
        public UserMainWindow(BlApi.IBL bl, User User)
        {
            user = User;
            bL = bl;
            addParcel = new();
            addParcel.SenderCustomer = new();
            addParcel.TargetidCustomer = new();
            InitializeComponent();
            DataContext = this;
            txtReciver.ItemsSource = bl.GetListCustomer().Select(i => i.Id);
            comboPrioritySelector.ItemsSource = Enum.GetValues(typeof(BO.Priorities));
            comboWeightSelector.ItemsSource = Enum.GetValues(typeof(BO.WeightCategories));

            ParcelListTo = bL.GetCustomer(User.Id).ToCustomer.ToList();
            ParcelListFrom = bL.GetCustomer(User.Id).FromCustomer.ToList();
            ParcelByCustomerView.ItemsSource = ParcelListFrom;
            ParcelByCustomerView1.ItemsSource = ParcelListTo;
            GroupingParcel();
        }
        /// <summary>
        /// grouping the parcels of the user by its status
        /// </summary>
        internal void GroupingParcel()
        {
            ParcelListTo = bL.GetCustomer(user.Id).ToCustomer.ToList(); ;//we want that all the items will be sorted by ID
            ParcelListFrom = bL.GetCustomer(user.Id).FromCustomer.ToList();
            ParcelByCustomerView1.ItemsSource = ParcelListTo;
            ParcelByCustomerView.ItemsSource = ParcelListFrom;
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelByCustomerView1.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
            view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelByCustomerView.ItemsSource);
            groupDescription = new PropertyGroupDescription("Status");
            view.GroupDescriptions.Add(groupDescription);
        }
        /// <summary>
        /// log out
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {

            _close = true;
            Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        /// <summary>
        /// add a parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (comboPrioritySelector.SelectedItem == null || txtReciver == null || comboWeightSelector == null)
            {
                MessageBox.Show("Missing drone details: ");
            }
            else
            {
                try
                {
                    addParcel.SenderCustomer.Id = user.Id;
                    //sending for add
                    bL.AddParcel(addParcel);
                    txtReciver.SelectedItem = null;
                    comboWeightSelector.SelectedItem = null;
                    comboPrioritySelector.SelectedItem = null;
                    ParcelListTo = bL.GetCustomer(user.Id).ToCustomer.ToList();
                    ParcelListFrom = bL.GetCustomer(user.Id).FromCustomer.ToList();
                    ParcelByCustomerView.ItemsSource = ParcelListFrom;
                    ParcelByCustomerView1.ItemsSource = ParcelListTo;
                    //success
                    MessageBox.Show("The parcel has been added successfully :)\n" + addParcel.ToString());
                    addParcel.Id = 0;
                }
                catch (Exception ex)//failed
                {
                    MessageBox.Show("Failed to add the parcel: " + ex.GetType().Name + "\n" + ex.Message);
                }
            }
        }

        /// <summary>
        /// show an individual prcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ParcelByCustomer parcelFrom = (ParcelByCustomer)ParcelByCustomerView.SelectedItem;
            ParcelByCustomer parcelTo = (ParcelByCustomer)ParcelByCustomerView1.SelectedItem;
            if (parcelFrom != null)
                new ParcelWindow(bL, bL.GetParcel(parcelFrom.Id), 1).Show();
            if (parcelTo != null)
                new ParcelWindow(bL, bL.GetParcel(parcelTo.Id), 1).Show();
        }

    }
}

