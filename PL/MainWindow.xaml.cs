using System;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using BlApi;
using BO;

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
        // public override ValidationResult validate(Object value, CultureInfo culterInfo)
        //{
        //    string charString = value as string;
        //    if (charString.Length < 0)
        //        return new ValidationResult(false, $"email address");
        //}
        /// <summary>
        /// send to a get password window 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new GetPasswordWindow(bl).Show();
        }
        /// <summary>
        /// sent to a change password window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            new ChangePasswordWindow(bl).Show();
        }
        /// <summary>
        /// button click=> sign in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new SignUpWindow(bl).Show();
        }
        /// <summary>
        /// for sign in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                User tmp = bl.GetUser(username.Text);
                if (tmp.Password != passwordcode.Text||tmp.IsRemoved)
                    throw new ItemNotExistException();
                if (tmp.IsManager == true)
                    new MenuWindow(bl).Show();
                else
                    new UserMainWindow(bl,tmp).Show();
                passwordcode.Clear();//clears the text box
                username.Clear();//clears the text box
            }
            catch (Exception)
            {
                MessageBox.Show("you enter a non correct Username or password");
            }
        }
        /// <summary>
        /// close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Close();
        }
    }
}
