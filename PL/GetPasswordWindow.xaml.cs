using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
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
    /// Interaction logic for GetPasswordWindow.xaml
    /// </summary>
    public partial class GetPasswordWindow : Window
    {
        BlApi.IBL bl;
        public GetPasswordWindow(BlApi.IBL bL)
        {
            bl = bL;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!IsValidMailFormat(email.Text))
                {
                    MessageBox.Show("wrong mail format", "Confirmation", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                User tmp = bl.GetUser(email.Text);
                SendMail();
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("invalid mail address");
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
                User tmp = bl.GetUser(email.Text);
                MailMessage mail = new MailMessage();
                mail.To.Add(tmp.EmailAddress);
                mail.From = new MailAddress("drone.strike.delivery@gmail.com");
                mail.Subject = "Password Recovery";
                mail.Body = "Hi " + tmp.Name + "\nYour password is: " + tmp.Password;
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
