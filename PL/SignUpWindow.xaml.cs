using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SignUpWindow.xaml
    /// </summary>
    public partial class SignUpWindow : Window
    {
        BlApi.IBL bl;
        public User user { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="bL"></param>
        public SignUpWindow(BlApi.IBL bL)
        {
            bl = bL;
            user = new();
            user.Location = new();
            user.IsManager = false;//We do not want to allow anyone who enters the app to register as a manager.
                                   //If you want to register as a manager you must get a special permit from usת
                                   //and we will register you by hand.
            DataContext = this;
            InitializeComponent();
        }

        /// <summary>
        /// save details and sign in
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Click(object sender, RoutedEventArgs e)//
        {
            try
            {
                if (user.Password == null || user.Name == null || user.EmailAddress == null)
                {
                    MessageBox.Show("you must fill all fields");
                    return;
                }
                else
                {
                    if (!IsValidMailFormat(user.EmailAddress))
                    {
                        MessageBox.Show("wrong mail format", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    else
                    {
                        _ = new MailAddress(user.EmailAddress);
                        bl.AddUser(user);
                        SendMail();
                        Close();
                    }
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("invalid mail format");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// checks the mails format
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private bool IsValidMailFormat(string email)
        {
            if (email.EndsWith("@gmail.com") || email.EndsWith("@g.jct.ac.il"))
                return true;
            if (email.Contains(" "))
                return false;
            return false;
        }

        /// <summary>
        /// function for sending a mail to a user
        /// </summary>
        private void SendMail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(user.EmailAddress);
                mail.From = new MailAddress("drone.strike.delivery@gmail.com");
                mail.Subject = "Welcome to our delivery by drone System!";
                mail.Body = "Hi " + user.Name + "\nRegistration for the system has been successfully completed.\nYour login information is: \nuser: " + user.Name + " password: " + user.Password;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("drone.strike.delivery@gmail.com", "shiradeena"),
                    EnableSsl = true
                };

                smtp.Send(mail);
            }
            catch (Exception ex)
            {
                //תפיסה וטיפול בשגיאות
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// prevents from the user to enter letters in the id box
        /// </summary>
        /// <param name="sender">text box</param>
        /// <param name="e">event</param>
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            System.Text.RegularExpressions.Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}