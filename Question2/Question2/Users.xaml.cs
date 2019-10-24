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

namespace Question2
{
    /// <summary>
    /// Interaction logic for Users.xaml
    /// </summary>
    public partial class Users : Window
    {
        private string userName, fullName, temp;
        private int branch, shift, ID;

        private bool check_edit = false;

        private SqlDB db; 

        public Users()
        {
            InitializeComponent();
            db = new SqlDB();
            if (SqlDB.OpenConnection())
            {
                fillBranchCombo();
                fillShiftCombo();
                genDataGridUsers();
            }
        }

        private void BtnAdd_User_Click(object sender, RoutedEventArgs e)
        {
            if (!(String.IsNullOrEmpty(txtUserName.Text)) || !(String.IsNullOrEmpty(txtFullName.Text)))
            {
                userName = txtUserName.Text;
                fullName = txtFullName.Text;
            }
            else
                MessageBox.Show("Please make sure that all boxes have either a value, word or sentence", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            temp = comboBranches.Items[comboBranches.SelectedIndex].ToString();
            branch = int.Parse(temp.Substring(0, temp.IndexOf(" ")));

            temp = comboShifts.Items[comboShifts.SelectedIndex].ToString();
            shift = int.Parse(temp.Substring(0, temp.IndexOf(" ")));

            if (check_edit == false)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db.Connection;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "INSERT INTO tblUsers VALUES (@user_Name, @user_FullName, @branch_ID, @shift_ID)";
                cmd.Parameters.AddWithValue("@user_Name", userName);
                cmd.Parameters.AddWithValue("@user_FullName", fullName);
                cmd.Parameters.AddWithValue("@branch_ID", branch);
                cmd.Parameters.AddWithValue("@shift_ID", shift);

                if (db.ExecuteQuery(ref cmd) == false)
                    MessageBox.Show("Could not insert into table, please check all values", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("New data have been inserted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                genDataGridUsers();
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db.Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "update_users";
                cmd.Parameters.AddWithValue("@pID", ID);
                cmd.Parameters.AddWithValue("@pUserName", txtUserName.Text);
                cmd.Parameters.AddWithValue("@pFullName", txtFullName.Text);
                cmd.Parameters.AddWithValue("@pbranch", branch);
                cmd.Parameters.AddWithValue("@pshift", shift);

                if (db.ExecuteQuery(ref cmd) == false)
                    MessageBox.Show("Could not update the specific row from the database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("data has been successfully updated from the database", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                genDataGridUsers();
                check_edit = false;
            }

            clear_all_fields();
        }

        private void BtnEdit_User_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)dbGrid_Users.SelectedItem;
            ID = Convert.ToInt32(dataRowView.Row[0]);

            txtUserName.Text = dataRowView.Row[1].ToString();
            txtFullName.Text = dataRowView.Row[2].ToString();

            check_edit = true;
        }

        private void BtnDelete_User_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)dbGrid_Users.SelectedItem;
            ID = Convert.ToInt32(dataRowView.Row[0]);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "DELETE FROM tblUsers WHERE user_ID = '@id'";
            cmd.Parameters.AddWithValue("@id", ID);

            if (MessageBox.Show("Are you sure that you want to delete this row from the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (db.ExecuteQuery(ref cmd) == false)
                    MessageBox.Show("Could not delete the specific row from the database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("data has been successfully deleted from the database", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            genDataGridUsers();
        }

        private void BtnHelp_users_Click(object sender, RoutedEventArgs e)
        {
            string msg = "To add a new branch please fill in the description box and press the add button";
            msg += "\nPlease press the row in the gird when you want to either edit or delete the data. The row you select will change colour";
            msg += "\nAfter selecting the row, you can either press the delete button or edit button.";
            msg += "\nIf you select the edit button, the data of the row will appear in the boxes on the left hand of the screen. Edit the values and press the add button to save your updated data";
            MessageBox.Show(msg, "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        void fillBranchCombo()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db.Connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "show_branches";

            SqlDataReader reader = cmd.ExecuteReader();
            while(reader.Read())
            {
                comboBranches.Items.Add(reader[0].ToString() + " " + reader[1].ToString());
            }

            reader.Close();
        }

        void fillShiftCombo()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db.Connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "show_shifts";

            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboShifts.Items.Add(reader[0].ToString() + " " + reader[1].ToString());
            }

            reader.Close();
        }

        void genDataGridUsers()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db.Connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "show_users";

            dbGrid_Users.ItemsSource = db.DataAdapter(ref cmd).AsDataView();
        }

        void clear_all_fields()
        {
            txtUserName.Clear();
            txtFullName.Clear();
            comboBranches.SelectedIndex = 0;
            comboShifts.SelectedIndex = 0;
        }
    }
}
