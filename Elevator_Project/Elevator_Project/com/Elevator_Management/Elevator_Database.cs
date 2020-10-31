using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Data;

namespace Elevator_Project
{
     class Elevator_Database
    {
        
        // Function to retun current floor of the Elevator from Database
        internal string GetCurrentFloor() {
            string Floor = "No Current Floor Records Available";
            var DBPath = Application.StartupPath + "\\Elevator_DB_2000.mdb";
            OleDbConnection conn = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + DBPath);
            try
            {
                // Get Current Floor Number From Database
                string queryString = "SELECT Floor_number FROM Floors WHERE Floor_id=(SELECT Floor_id FROM Current_Floor WHERE Current_Floor_id=(SELECT MAX(Current_Floor_id) FROM Current_Floor))";

                OleDbCommand command = new OleDbCommand(queryString, conn);
                conn.Open();
                OleDbDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Floor = reader.GetInt32(0).ToString();
                }
                
                reader.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!!! Something is wrong with Database"+ ex);
            }

            return Floor;
        }

        // Function to Set current floor of the Elevator
        internal void SetCurrentFloor(int State) {
            if (State == 1 || State == 2)
            {
                SetFloorToDatabase(State);
            }
            else {
                MessageBox.Show("Error!!! You only can set Elevator to floor 1 or 2");
            }
        }
        
        
        // Function to Set the Current Floor of the Elevator in the database
        private void SetFloorToDatabase(int State)
        {
            var DBPath = Application.StartupPath + "\\Elevator_DB_2000.mdb";
            OleDbConnection conn = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + DBPath);

            try
            {
                // Set Floor State to the Database with current date and time
                string date_time = DateTime.Now.ToString("dd-MM-yyyy 'at' h:mm:ss tt");
                string queryString = "INSERT INTO Current_Floor(Floor_id, Date_Time) SELECT Floors.Floor_id, '"+ date_time + "' FROM Floors WHERE Floors.Floor_number=?";
                
                OleDbCommand command = new OleDbCommand(queryString, conn);
                command.Parameters.AddWithValue("@a",State);

                conn.Open();
                command.ExecuteNonQuery();
                //MessageBox.Show("Arrived on Floor "+State);
                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!!! Something is wrong with Database" + ex);
            }
        }


        // Function to showing tables of database in the form gridview
        internal void GetTablesData()
        {
            var DBPath = Application.StartupPath + "\\Elevator_DB_2000.mdb";
            OleDbConnection conn = new OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + DBPath);

            try
            {
                conn.Open();
                // Get Tables Data from Database
                string queryString = "SELECT * FROM Current_Floor";
                OleDbDataAdapter DbAdapter = new OleDbDataAdapter(queryString, conn);
                //OleDbCommand command = new OleDbCommand(queryString, conn);
                DataTable DbTable = new DataTable();
                DbAdapter.Fill(DbTable);
                // block the un necessary columns from the table
                Main_Form.Self.database_grid_view.AutoGenerateColumns = false;
                // add the column of the above sql query to out form data view grid table
                Main_Form.Self.database_grid_view.DataSource = DbTable;

                // chenge the floor_id to floor_number in datagridviewTable
                foreach (DataGridViewRow DataRow in Main_Form.Self.database_grid_view.Rows)
                {
                    foreach (DataGridViewCell DataCell in DataRow.Cells)
                    {
                        if (DataCell.Value.Equals(7))
                        {
                            DataCell.Value = 1;
                        }
                        else if (DataCell.Value.Equals(8))
                        {
                            DataCell.Value = 2;
                        }
                    }
                }
                // deselct default and then select last row of the table which indicate last log of the elevator
                Main_Form.Self.database_grid_view.CurrentCell.Selected = false;
                Main_Form.Self.database_grid_view.Rows[Main_Form.Self.database_grid_view.Rows.Count - 1].Selected = true;

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error!!! Something is wrong Loading data from the database" + ex);
            }
        }

    }
}
