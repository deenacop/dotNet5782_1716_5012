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
        public SignUpWindow(BlApi.IBL bL)
        {
            bl = bL;
            user = new();
            user.IsManager = false;
            DataContext = this;
            InitializeComponent();
        }

        private void TextBox_OnlyLetters_PreviewKeyDown(object sender, KeyEventArgs e)
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

            //allow digits (without Shift or Alt)
            if (Char.IsLetter(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox
                            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;
        }
        private void password_PreviewKeyDown(object sender, KeyEventArgs e)
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

            //allow digits (without Shift or Alt)
            if (Char.IsDigit(c))
                if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                    return; //let this key be written inside the textbox
                            //forbid letters and signs (#,$, %, ...)
            e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
            return;
        }
        private void SendMail()
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(user.EmailAddress);
                mail.From = new MailAddress("wingsdronedeliverysystem@gmail.com");
                mail.Subject = "Welcome to our delivery by drone System!";
                mail.Body = "Hi " + user.Name + "\nRegistration for the system has been successfully completed.\nYour login information is: \nuser: " + user.Name + " password: " + user.Password;
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
        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (user.Password == null || user.Name == null || user.EmailAddress == null )
                {
                    MessageBox.Show("you must fill all fields");
                    return;
                }
                else
                {
                    if (user.Password.Length != 8)
                    {
                        MessageBox.Show("password should be with 8 digits");
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
      
            //Regex.IsMatch(user.Name, @"^[a-zA-Z]+$");
            
    }
}
