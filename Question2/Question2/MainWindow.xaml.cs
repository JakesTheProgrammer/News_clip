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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Question2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private SqlDB dB;

        public MainWindow()
        {
            InitializeComponent();
            dB = new SqlDB();
            if (SqlDB.OpenConnection())
                db_status.Content = "Database connection status: ONLINE";
            else
                db_status.Content = "Database connection status: OFFLINE";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Users user_screen = new Users();
            user_screen.ShowDialog();
            
        }

        private void BtnBranches_Click(object sender, RoutedEventArgs e)
        {
            Branches branches_screen = new Branches();
            branches_screen.ShowDialog();
        }

        private void BtnShifts_Click(object sender, RoutedEventArgs e)
        {
            Shifts shifts_screen = new Shifts();
            shifts_screen.ShowDialog();
        }
    }
}
