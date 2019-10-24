using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Question3
{
    class Program
    {
        private static SqlConnection connection;
        private static int branch, shift;

        static void Main(string[] args)
        {
            string conn = @"Server = localhost; Database = Question2_database; Trusted_connection = Yes";

            try
            {
                connection = new SqlConnection(conn);
                connection.Open();
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Could not open database");
            }

            SqlCommand cmd = new SqlCommand();
            cmd.Connection = connection;
            cmd.CommandType = CommandType.Text;

            cmd.CommandText = "SELECT * FROM tblBranches";
            SqlDataReader dbReader = cmd.ExecuteReader();

            Console.WriteLine("Branches available:");
            while(dbReader.Read())
            {
                Console.WriteLine(dbReader[0].ToString() + "\t" + dbReader[1]);
            }

            dbReader.Close();


            cmd.CommandText = "SELECT * FROM tblShifts";
            dbReader = cmd.ExecuteReader();

            Console.WriteLine("\nShifts available:");
            while (dbReader.Read())
            {
                Console.WriteLine(dbReader[0].ToString() + "\t" + dbReader[1]);
            }

            dbReader.Close();

            
            Console.Write("Please enter the number (eg 1) for the branch to search with: ");
            branch = Convert.ToInt32(Console.ReadLine());

            Console.Write("\nPlease enter the number (eg 1) for the shift to search with: ");
            shift = Convert.ToInt32(Console.ReadLine());

            
            cmd.CommandText = "SELECT user_ID, user_Name, user_FullName, tblBranches.branch_description, tblShifts.shift_description FROM tblUsers INNER JOIN tblBranches on tblUsers.branch_ID = tblBranches.branch_ID INNER JOIN tblShifts on tblUsers.shift_ID = tblShifts.shift_ID WHERE ((tblUsers.branch_id = @branch) AND (tblUsers.shift_ID = @shift));";
            cmd.Parameters.AddWithValue("@branch", branch);
            cmd.Parameters.AddWithValue("@shift", shift);
            dbReader = cmd.ExecuteReader();

            Console.WriteLine("\nThe following was found in the database:");
            Console.WriteLine("{0,2}{1,15}{2,15}{3,10}{4,10}", "ID", "UserName", "FullName", "Branch", "Shift");
            while (dbReader.Read())
            {
                Console.WriteLine("{0,2}{1,15}{2,15}{3,10}{4,10}", dbReader[0].ToString(), dbReader[1],dbReader[2],dbReader[3],dbReader[4]);
                
            }

            dbReader.Close();

            Console.ReadLine();

        }
    }
}
