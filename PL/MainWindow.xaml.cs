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
            passwordcode.Text = null;
            passwordBox.Password = null;
            username.Text = null;
            new GetPasswordWindow(bl).Show();
        }
        /// <summary>
        /// sent to a change password window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Label_MouseDoubleClick_1(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            passwordcode.Text = null;
            passwordBox.Password = null;
            username.Text = null;
            new ChangePasswordWindow(bl).Show();
        }
        /// <summary>
        /// button click=> sign in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            passwordcode.Text = null;
            passwordBox.Password = null;
            username.Text = null;
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
                if ((tmp.Password != passwordcode.Text && tmp.Password != passwordBox.Password) || tmp.IsRemoved)
                    throw new ItemNotExistException();
                passwordcode.Text = null;
                passwordBox.Password = null;
                username.Text = null;
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
        /// for showing the password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPassword_Checked(object sender, RoutedEventArgs e)
        {
            passwordcode.Text = passwordBox.Password;
            passwordBox.Visibility = Visibility.Collapsed;
            passwordcode.Visibility = Visibility.Visible;
        }
        /// <summary>
        /// for showing the password
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowPassword_Unchecked(object sender, RoutedEventArgs e)
        {
            passwordBox.Password = passwordcode.Text;
            passwordcode.Visibility = Visibility.Collapsed;
            passwordBox.Visibility = Visibility.Visible;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new Window1().Show();
        }
    }
   
}
