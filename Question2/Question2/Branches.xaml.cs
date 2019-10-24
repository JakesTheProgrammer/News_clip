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
    /// Interaction logic for Branches.xaml
    /// </summary>
    public partial class Branches : Window
    {
        private SqlDB db;

        private string branch_description;
        private bool check_edit = false;
        private int ID;

        public Branches()
        {
            InitializeComponent();

            db = new SqlDB();
            if (SqlDB.OpenConnection())
            {
                genDataGridBranches();
            }
        }

        private void BtnAdd_Branch_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBranch_Descrip.Text))
            {
                branch_description = txtBranch_Descrip.Text;
                
            }
            else
                MessageBox.Show("Please enter a word or sentence in the description box", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            if (check_edit == false)// used to see if the user wants to edit a row or to insert new values
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db.Connection;
                cmd.CommandType = CommandType.Text;

                cmd.CommandText = "INSERT INTO tblBranches VALUES (@branch_description)";
                cmd.Parameters.AddWithValue("@branch_description", branch_description);

                if (db.ExecuteQuery(ref cmd) == false)
                    MessageBox.Show("Could not insert into table, please check all values", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("New data have been inserted", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                genDataGridBranches();
            }
            else
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = db.Connection;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "update_branches";
                cmd.Parameters.AddWithValue("@pID", ID);
                cmd.Parameters.AddWithValue("@pDesc", txtBranch_Descrip.Text);

                if (db.ExecuteQuery(ref cmd) == false)
                    MessageBox.Show("Could not update the specific row from the database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("data has been successfully updated from the database", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                genDataGridBranches();
                check_edit = false;
            }

            txtBranch_Descrip.Clear();
            branch_description = "";
        }

        private void BtnEdit_Branch_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)dbGrid_Branch.SelectedItem;
            ID = Convert.ToInt32(dataRowView.Row[0]);
            string desc = Convert.ToString(dataRowView.Row[1]);

            txtBranch_Descrip.Text = desc;

            check_edit = true;
        }

        private void BtnDelete_Branch_Click(object sender, RoutedEventArgs e)
        {
            DataRowView dataRowView = (DataRowView)dbGrid_Branch.SelectedItem;
            ID = Convert.ToInt32(dataRowView.Row[0]);

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db.Connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "DELETE FROM tblBranches WHERE branch_ID = @id";
            cmd.Parameters.AddWithValue("@id", ID);

            if (MessageBox.Show("Are you sure that you want to delete this row from the database?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (db.ExecuteQuery(ref cmd) == false)
                    MessageBox.Show("Could not delete the specific row from the database", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                else
                    MessageBox.Show("data has been successfully deleted from the database", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            genDataGridBranches();
        }

        void genDataGridBranches()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = db.Connection;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "show_branches";

            dbGrid_Branch.ItemsSource = db.DataAdapter(ref cmd).AsDataView();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string msg = "To add a new branch please fill in the description box and press the add button";
            msg += "\nPlease press the row in the gird when you want to either edit or delete the data. The row you select will change colour";
            msg += "\nAfter selecting the row, you can either press the delete button or edit button.";
            msg += "\nIf you select the edit button, the data of the row will appear in the boxes on the left hand of the screen. Edit the values and press the add button to save your updated data";
            MessageBox.Show(msg, "Help", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
