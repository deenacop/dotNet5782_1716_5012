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
                User tmp = bl.GetUser(email.Text);
                SendMail();
                Close();
            }
            catch (Exception)
            {
                MessageBox.Show("invalid mail address");
            }
        }
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
        private void TextBox_Email_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox text = sender as TextBox;
            if (text == null) return;
            if (e == null) return;

            //allow get out of the text box
            if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
            {
                e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            }

            //allow list of system keys 
            if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
                e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
             || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right)
                return;

            char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);

            //allow control system keys
            if (Char.IsControl(c)) return;

            //allow dot
            if (e.Key == Key.OemPeriod)
            {
                return;
            }

            //allow @
            if (e.Key == Key.Attn)
            {
                return;
            }


            //allow digits (without Shift or Alt)
            if (Char.IsLetterOrDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox
                            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;
        }
    }
}
