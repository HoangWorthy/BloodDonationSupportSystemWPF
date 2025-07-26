using DAL.Entities;
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

namespace Blood_Donation_Support_System_WPF
{
    /// <summary>
    /// Interaction logic for BloodRequestDetailWindow.xaml
    /// </summary>
    public partial class BloodRequestDetailWindow : Window
    {
        public BloodRequestDetailWindow(BloodRequest request)
        {
            InitializeComponent(); // ✅ should be first
            this.DataContext = request;
        }


        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
