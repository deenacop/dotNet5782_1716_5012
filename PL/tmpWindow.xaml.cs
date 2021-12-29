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

namespace PL
{
    /// <summary>
    /// Interaction logic for tmpWindow.xaml
    /// </summary>

     
    
    public partial class tmpWindow : Window
    {
        public int sizeH { get; set; }
        public int sizeW { get; set; }

        public tmpWindow(string str)
        {
            if(str=="add")
            {
                sizeW = 340; sizeH = 370;
            }
            if(str=="update")
            {
                sizeH = 400; sizeW = 400;
            }
            DataContext = this;
            InitializeComponent();
        }


    }
}
