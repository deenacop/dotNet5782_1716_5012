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
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
       public User user { get; set; }
        BlApi.IBL bL;
        public ChangePasswordWindow(BlApi.IBL bl)
        {
            bL = bl;
            user = new();
            DataContext = this;
            InitializeComponent();
        }

   
        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (user.EmailAddress == "" || user.Password == ""|| newPassword.Text=="")
                {
                    MessageBox.Show("you must fill all fields");
                }
                if (newPassword.Text.Length != 6)
                {
                    MessageBox.Show("password should be with 6 digits");
                }
                if (newPassword.Text == bL.GetUser(user.EmailAddress).Password)
                {
                    MessageBox.Show("Choose a password that does not exist in the system");
                }
                else
                {
                    SendMail();
                    bL.updateUser(user.EmailAddress,newPassword.Text);
                    Close();
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
        private void SendMail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(user.EmailAddress);
                mail.From = new MailAddress("wingsdronedeliverysystem@gmail.com");
                mail.Subject = "Get a new Password";
                mail.Body = "Hi " + bL.GetUser(user.EmailAddress).Name + "\nYour new password is: " + newPassword.Text;
                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential("wingsdronedeliverysystem@gmail.com", "Wings1234"),
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


