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
    /// Interaction logic for UserMainWindow.xaml
    /// </summary>
    public partial class UserMainWindow : Window
    {
        BlApi.IBL bL;
        public Parcel addParcel { get; set; }
        User user;
        private bool _close { get; set; } = false;//for closing the window

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
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {

            _close = true;
            Close();
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

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
                    //success
                    MessageBox.Show("The parcel has been added successfully :)\n" + addParcel.ToString());
                }  
                catch (Exception ex)//failed
                {
                    MessageBox.Show("Failed to add the parcel: " + ex.GetType().Name + "\n" + ex.Message);
                }
            }

        }
    }
}