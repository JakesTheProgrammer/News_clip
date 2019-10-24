using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Windows;

namespace Question2
{
    class SqlDB
    {
        private static SqlConnection conn;
        private SqlDataReader dbReader;
        private SqlDataAdapter dataAdapter;

        public static bool OpenConnection()
        {
            bool status = false;

            string connection = @"Server=localhost;Database=Question2_database; Trusted_connection=Yes";

            try
            {
                conn = new SqlConnection(connection);
                conn.Open();
                status = true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Could not connect to the database. Error: " + ex.ToString());
                status = false;
            }

            return status;
        }

        public static void CloseConnection()
        {
            conn.Close();
        }

        public SqlConnection Connection
        {
            get
            {
                return conn;
            }
        }

        public bool ExecuteQuery(ref SqlCommand cmd)
        {
            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    OpenConnection();
                }

                cmd.Connection = conn;
                cmd.ExecuteNonQuery();

                return true;
            }
            catch (SqlException ex)
            {
                return false;
            }
        }

        public SqlDataReader DataReader(ref SqlCommand cmd)
        {
            if (conn.State != ConnectionState.Open)
            {
                OpenConnection();
            }

            cmd.Connection = conn;
            dbReader = cmd.ExecuteReader();
            return dbReader;
        }

        public DataTable DataAdapter(ref SqlCommand cmd)
        {
            if (conn.State != ConnectionState.Open)
            {
                OpenConnection();
            }

            cmd.Connection = conn;
            dataAdapter = new SqlDataAdapter();
            dataAdapter.SelectCommand = cmd;
            DataTable table = new DataTable();
            dataAdapter.Fill(table);
            return table;
        }
    }
}
