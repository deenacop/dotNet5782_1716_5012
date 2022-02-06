using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        BlApi.IBL bL;
        private string email;

        public ChangePasswordWindow(BlApi.IBL bl)
        {
            InitializeComponent();
            bL = bl;
        }

        /// <summary>
        /// save the details and send a massage
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxImage icon = MessageBoxImage.Error;
                if (mailAddress.Text == "" || oldPassword.Text == "" || newPassword.Text == "")
                {
                    MessageBox.Show("you must fill all fields", "Confirmation", MessageBoxButton.OK, icon);
                    return;
                }
                if (!IsValidMailFormat(mailAddress.Text))
                {
                    MessageBox.Show("wrong mail format", "Confirmation", MessageBoxButton.OK, icon);
                    return;
                }
                if (oldPassword.Text != bL.GetUser(mailAddress.Text).Password)
                {
                    MessageBox.Show("worng password", "Confirmation", MessageBoxButton.OK, icon);
                    return;
                }

                else
                {
                    SendMail();
                    bL.updateUser(mailAddress.Text, newPassword.Text);
                    Close();
                }
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
                mail.To.Add(mailAddress.Text);
                mail.From = new MailAddress("drone.strike.delivery@gmail.com");
                mail.Subject = "Get a new Password";
                mail.Body = "Hi " + bL.GetUser(mailAddress.Text).Name + "\nYour new password is: " + newPassword.Text;
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

    }
}