using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace PL
{
    public class User : ValidationRule, IDataErrorInfo, INotifyPropertyChanged
    {
        public int MinimumCharacters { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string charString = value as string;

            if (charString.Length < MinimumCharacters)
                return new ValidationResult(false, $"User atleast {MinimumCharacters} characters.");

            return new ValidationResult(true, null);
        }
        public string UserName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        // מילון להעמסת השגיאות כאשר המפתח זה שם השדה של השגיאה והערך זה הודעת השגיאה
        public Dictionary<string, string> ErrorMessges { get; private set; } = new Dictionary<string, string>();

        private string password;
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }

        // זה קשור לממשק השגיאה הוא עוקב כל פעם בזאמל כל פעם שיש שינויים בשדות של המחלקה פה 
        public string this[string name]
        {
            get
            {
                //  אם התוצאה כלום אז הוא לא יהבהב אחרת כן
                string result = null;
                //כאן אני עשיתי תנאים על השדות
                switch (name)
                {
                    case "Password":
                        if (string.IsNullOrWhiteSpace(Password))
                        {
                            result = "Password cannet by empty";
                        }
                        else if (Password.Length < 8)
                        {
                            result = "Password must not be less than 8 digits";
                        }
                        break;
                    case "UserName":

                    default:
                        break;
                }
                return result;
                //// כאן אני מעמיס על המילון את הודעת השגיאה כאשר אם יש לו כבר מפתח
                //// .כזה אז תעמיס רק את ההודעה אחרת תוסיף גם את המפתח וגם את ההודעה
                if (ErrorMessges.ContainsKey(name))
                {
                    ErrorMessges[name] = result;
                }

                else if (result != null)
                {
                    ErrorMessges.Add(name, result);
                }

                // כאן תפעיל את האירוע שמודיע שהיה שינוי במילון ואז הזאמל ידפיס לך את ההודעה.
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ErrorMessges"));
                return result;
            }
        }
        public string Error
        {
            get
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Interaction logic for TTTTRRRRYYY.xaml
    /// </summary>
    public partial class TTTTRRRRYYY : Window
    {
        public TTTTRRRRYYY()
        {
            InitializeComponent();
        }
    }
}
