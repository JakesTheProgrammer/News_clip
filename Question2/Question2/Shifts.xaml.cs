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
using System.Data;
using System.Data.SqlClient;
using Question2;

namespace Question2
{
    /// <summary>
    /// Interaction logic for Shifts.xaml
    /// </summary>
    public partial class Shifts : Window
    {
        private SqlDB db;

        private string shift_description;
        private int ID;
        private bool check_edit = false;

        public Shifts()
        {
            InitializeComponent();
            db = new SqlDB();
            if (SqlDB.OpenConnection())
            {
                genDataGridShifts();
            }
        }

        private void BtnDelete_Shift_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)dbGrid_Shifts.SelectedItem;
            ID = Convert.ToInt32(dataRowView.Row[0]);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "DELETE FROM tblShifts WHERE shift_ID = @id";
            cmd.Parameters.AddWithValue("@id", ID);

            if (MessageBox.Show("Are you sure that you want to delete this row from the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (db.ExecuteQuery(ref cmd) == false)
                    MessageBox.Show("Could not delete the specific row from the database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("data has been successfully deleted from the database", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            genDataGridShifts();
        }

        private void BtnEdit_Shift_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)dbGrid_Shifts.SelectedItem;
            ID = Convert.ToInt32(dataRowView.Row[0]);
            string desc = Convert.ToString(dataRowView.Row[1]);

            txtShift_desc.Text = desc;

            check_edit = true;
        }

        private void BtnAdd_Shift_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtShift_desc.Text))
            {
                shift_description = txtShift_desc.Text;
                
            }
            else
                MessageBox.Show("Please enter a word or sentence in the description box", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            if (check_edit == false) // used to see if the user wants to edit a row or to insert new values
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db.Connection;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "INSERT INTO tblShifts VALUES (@shift_description)";
                cmd.Parameters.AddWithValue("@shift_description", shift_description);

                if (db.ExecuteQuery(ref cmd) == false)
                    MessageBox.Show("Could not insert into table, please check all values", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("New data have been inserted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                genDataGridShifts();
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db.Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "update_shifts";
                cmd.Parameters.AddWithValue("@pID", ID);
                cmd.Parameters.AddWithValue("@pDesc", txtShift_desc.Text);

                if (db.ExecuteQuery(ref cmd) == false)
                    MessageBox.Show("Could not update the specific row from the database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("data has been successfully updated from the database", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                genDataGridShifts();
                check_edit = false;
            }

            txtShift_desc.Clear();
        }

        void genDataGridShifts()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db.Connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "show_shifts";

            dbGrid_Shifts.ItemsSource =db.DataAdapter(ref cmd).AsDataView();
        }

        private void BtnHelp_shift_Click(object sender, RoutedEventArgs e)
        {
            string msg = "To add a new branch please fill in the description box and press the add button";
            msg += "\nPlease press the row in the gird when you want to either edit or delete the data. The row you select will change colour";
            msg += "\nAfter selecting the row, you can either press the delete button or edit button.";
            msg += "\nIf you select the edit button, the data of the row will appear in the boxes on the left hand of the screen. Edit the values and press the add button to save your updated data";
            MessageBox.Show(msg, "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
