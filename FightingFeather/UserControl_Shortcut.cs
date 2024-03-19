using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Drawing.Text;

namespace FightingFeather
{
    public partial class UserControl_Shortcut : UserControl
    {
        public event EventHandler ButtonEnterClicked;

        public UserControl_Shortcut()
        {
            InitializeComponent();
            
            // Attach event handlers to calculate bet difference when bet values change
            textBox_MeronBet.TextChanged += CalculateBetDifference;
            textBox_WalaBet.TextChanged += CalculateBetDifference;
            textBox_Pago.TextChanged += CalculateParehas;

            // Populate the ComboBox with options
            comboBox_Winner.Items.AddRange(new object[] { "M", "W", "Cancel", "Draw" });
            comboBox_Rate.Items.AddRange(new object[] { "8/10", "3/4", "7/10", "N/A" });

            // Create a new System.Windows.Forms.ToolTip instance
            System.Windows.Forms.ToolTip toolTip = new System.Windows.Forms.ToolTip();

            // Set tooltip for the PictureBox
            toolTip.SetToolTip(pictureBox1, "Entries that don't have a Winner yet are displayed here.");

            ReloadData();
        }


        public void ReloadData()
        {
            GridPlasada_Shortcut.Rows.Clear(); // Clear existing rows

            PopulateGridPlasadaShortcut(); // Reload data
        }

        private void CalculateBetDifference(object sender, EventArgs e)
        {

            // Ensure both text boxes have valid integer values
            int meronBet, walaBet;
            if (int.TryParse(textBox_MeronBet.Text, out meronBet) && int.TryParse(textBox_WalaBet.Text, out walaBet))
            {
                // Calculate the bet difference
                int betDiff = meronBet - walaBet;
                textBox_BetDiff.Text = betDiff.ToString();
            }
            else
            {
                // Handle invalid input or non-integer values
                textBox_BetDiff.Text = "-";
            }
        }
        private void CalculateParehas(object sender, EventArgs e)
        {
            // Define variables to store the parsed values
            int walaBet, pago;

            // Parse the values from the text boxes
            if (int.TryParse(textBox_WalaBet.Text, out walaBet) && int.TryParse(textBox_Pago.Text, out pago))
            {
                // Perform the addition
                int parehas = walaBet + pago;

                // Update the textBox_Parehas with the calculated result
                textBox_Parehas.Text = parehas.ToString();
            }
            else
            {
                // Handle cases where parsing fails (e.g., invalid input)
                // Handle invalid input or non-integer values
                textBox_Parehas.Text = "-";
            }
        }

        private bool ValidateInput()
        {
            // Check if the required fields are filled and if the input data is valid
            if (string.IsNullOrWhiteSpace(textBox_MeronBet.Text) ||
                string.IsNullOrWhiteSpace(textBox_WalaBet.Text) ||
                string.IsNullOrWhiteSpace(textBox_MeronName.Text) ||
                string.IsNullOrWhiteSpace(textBox_WalaName.Text))
            {
                return false;
            }

            // Additional validation logic can be added here as per your requirements

            return true;
        }

        private void SaveDataToDatabase()
        {        
            try
            {
                // Example connection string for SQLite
                string connectionString = "Data Source=dofox.db;Version=3;";

                // Create an SQLite connection object
                SQLiteConnection connection = new SQLiteConnection(connectionString);
                {
                    connection.Open();

                    // Define the SQL command to insert data into the database
                    string insertQuery = "INSERT INTO PLASADA (MERON, [BET (M)], WALA, [BET (W)], [INITIAL BET DIFF], PAGO, WINNER, RATE)" +
                                         "VALUES (@MeronName, @MeronBet, @WalaName, @WalaBet, @InitialBetDiff, @Pago, @Winner, @Rate)";

                    // Create a SQLiteCommand object
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        // Set parameter values
                        command.Parameters.AddWithValue("@MeronName", textBox_MeronName.Text);
                        command.Parameters.AddWithValue("@MeronBet", textBox_MeronBet.Text);
                        command.Parameters.AddWithValue("@WalaName", textBox_WalaName.Text);
                        command.Parameters.AddWithValue("@WalaBet", textBox_WalaBet.Text);
                        command.Parameters.AddWithValue("@InitialBetDiff", textBox_BetDiff.Text); 
                        command.Parameters.AddWithValue("@Pago", textBox_Pago.Text); 
                        command.Parameters.AddWithValue("@Winner", comboBox_Winner.SelectedItem); 
                        command.Parameters.AddWithValue("@Rate", comboBox_Rate.SelectedItem);

                        // Execute the command
                        command.ExecuteNonQuery();
                    }

                }
                
                GridPlasada_Shortcut.Rows.Clear();

                PopulateGridPlasadaShortcut();

                MessageBox.Show("Data saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void PopulateGridPlasadaShortcut()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JSON", "receipt.json");
            Console.WriteLine("Attempting to load JSON file from path: " + jsonFilePath);

            if (File.Exists(jsonFilePath))
            {
                string jsonText = File.ReadAllText(jsonFilePath);


                try
                {
                    JArray jsonArray = JArray.Parse(jsonText);

                    // Assuming jsonArray contains an array of objects
                    foreach (JObject obj in jsonArray)
                    {
                        // Check if the "WINNER" key exists and has a non-null or non-empty value
                        if (obj.ContainsKey("WINNER") && !string.IsNullOrEmpty(obj["WINNER"].ToString()))
                        {
                            // Skip this row as it already has a value in the "WINNER" column
                            continue;
                        }

                        // Convert WALA to uppercase
                        if (obj.ContainsKey("WALA"))
                        {
                            string walaValue = obj["WALA"].ToString();
                            obj["WALA"] = char.ToUpper(walaValue[0]) + walaValue.Substring(1);
                        }

                        // Convert MERON to uppercase
                        if (obj.ContainsKey("MERON"))
                        {
                            string meronValue = obj["MERON"].ToString();
                            obj["MERON"] = char.ToUpper(meronValue[0]) + meronValue.Substring(1);
                        }

                        // Create a new row
                        DataGridViewRow row = new DataGridViewRow();

                        // Add cells based on the columns you want to display
                        DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();
                        cell1.Value = obj["FIGHT"];
                        row.Cells.Add(cell1);

                        DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
                        cell2.Value = obj["MERON"];
                        row.Cells.Add(cell2);

                        DataGridViewTextBoxCell cell3 = new DataGridViewTextBoxCell();
                        cell3.Value = obj["BET (M)"];
                        row.Cells.Add(cell3);

                        DataGridViewTextBoxCell cell4 = new DataGridViewTextBoxCell();
                        cell4.Value = obj["WALA"];
                        row.Cells.Add(cell4);

                        DataGridViewTextBoxCell cell5 = new DataGridViewTextBoxCell();
                        cell5.Value = obj["BET (W)"];
                        row.Cells.Add(cell5);

                        // Add the row to the DataGridView
                        GridPlasada_Shortcut.Rows.Add(row);
                    }

                }

                catch (Exception ex)
                {
                    MessageBox.Show("Error loading JSON data: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
                }
            }         
        }

        private void ClearInputFields()
        {
            textBox_MeronName.Clear();
            textBox_MeronBet.Clear();
            textBox_WalaName.Clear();
            textBox_WalaBet.Clear();
            textBox_BetDiff.Clear();
            textBox_Pago.Clear();
            textBox_Parehas.Clear();
            comboBox_Winner.Items.Clear();
            comboBox_Rate.Items.Clear();
            textBox_EnterAmountRate.Clear();
        }

        private void button_Enter_Click(object sender, EventArgs e)
        {

            // Validate user input
            if (ValidateInput())
            {
                // Save data to the database if input is valid
                SaveDataToDatabase();

                // Raise the event
                ButtonEnterClicked?.Invoke(this, EventArgs.Empty);

                GridPlasada_Shortcut.Rows.Clear();
                PopulateGridPlasadaShortcut();

                ClearInputFields();

            }
            else
            {
                MessageBox.Show("Please fill in all required fields!");
            }
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            // Check if there is a selected row
            if (GridPlasada_Shortcut.SelectedRows.Count > 0)
            {
                // Get the index of the selected row
                int selectedIndex = GridPlasada_Shortcut.SelectedRows[0].Index;

                // Get the FIGHT ID of the selected row
                int fightId = Convert.ToInt32(GridPlasada_Shortcut.Rows[selectedIndex].Cells["FIGHT"].Value);

                // Populate the input fields with the data from the selected row
                PopulateInputFields(selectedIndex);

                button_SaveUpdate.Visible = true;
                button_Enter.Visible = false;

                // Set the new location of the button
                button_SaveUpdate.Location = new Point(55, 168);

            }
            else
            {
                MessageBox.Show("Please select a row to update.");

                // Hide the "SaveUpdate" button if no row is selected
                button_SaveUpdate.Visible = false;
            }
        }

        private void button_SaveUpdate_Click(object sender, EventArgs e)
        {
          

            // Check if the user input is valid
            if (ValidateInput())
            {
                // Get the FIGHT ID of the selected row
                int selectedIndex = GridPlasada_Shortcut.SelectedRows[0].Index;
                int fightId = Convert.ToInt32(GridPlasada_Shortcut.Rows[selectedIndex].Cells["FIGHT"].Value);

                GridPlasada_Shortcut.Rows.Clear();

                // Update data in the database
                UpdateDataToDatabase(fightId);

                // Raise the event
                ButtonEnterClicked?.Invoke(this, EventArgs.Empty);

                GridPlasada_Shortcut.Rows.Clear();
                PopulateGridPlasadaShortcut();

                ClearInputFields();

                button_SaveUpdate.Visible = false;
                button_Enter.Visible = true;

            }
            else
            {
                MessageBox.Show("Please fill in all required fields!");
            }
        }

        private void PopulateInputFields(int selectedIndex)
        {
            // Get the values from the selected row
            string meron = GridPlasada_Shortcut.Rows[selectedIndex].Cells["MERON"].Value.ToString();
            string meronBet = GridPlasada_Shortcut.Rows[selectedIndex].Cells["BET_M"].Value.ToString();
            string wala = GridPlasada_Shortcut.Rows[selectedIndex].Cells["WALA"].Value.ToString();
            string walaBet = GridPlasada_Shortcut.Rows[selectedIndex].Cells["BET_W"].Value.ToString();

            // Update the input fields with the values from the selected row
            textBox_MeronName.Text = meron;
            textBox_MeronBet.Text = meronBet;
            textBox_WalaName.Text = wala;
            textBox_WalaBet.Text = walaBet;
          
        }

        private void UpdateDataToDatabase(int fightId)
        {
            try
            {
                // Example connection string for SQLite
                string connectionString = "Data Source=dofox.db;Version=3;";

                // Create an SQLite connection object
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Define the SQL command to update data in the database
                    string updateQuery = "UPDATE PLASADA SET MERON = @MeronName, [BET (M)] = @MeronBet, " +
                                         "WALA = @WalaName, [BET (W)] = @WalaBet, " +
                                         "[INITIAL BET DIFF] = @InitialBetDiff, PAGO = @Pago, " +
                                         "WINNER = @Winner, RATE = @Rate WHERE FIGHT = @FightId";

                    // Create a SQLiteCommand object
                    using (var command = new SQLiteCommand(updateQuery, connection))
                    {
                        // Set parameter values
                        command.Parameters.AddWithValue("@MeronName", textBox_MeronName.Text);
                        command.Parameters.AddWithValue("@MeronBet", textBox_MeronBet.Text);
                        command.Parameters.AddWithValue("@WalaName", textBox_WalaName.Text);
                        command.Parameters.AddWithValue("@WalaBet", textBox_WalaBet.Text);
                        command.Parameters.AddWithValue("@InitialBetDiff", textBox_BetDiff.Text);
                        command.Parameters.AddWithValue("@Pago", textBox_Pago.Text);
                        command.Parameters.AddWithValue("@Winner", comboBox_Winner.SelectedItem);
                        command.Parameters.AddWithValue("@Rate", comboBox_Rate.SelectedItem);
                        command.Parameters.AddWithValue("@FightId", fightId);

                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                }

                GridPlasada_Shortcut.Rows.Clear();

                PopulateGridPlasadaShortcut();

                MessageBox.Show("Data updated successfully!");
      
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }


        private void button_Delete_Click(object sender, EventArgs e)
        {

            // Check if there is a selected row
            if (GridPlasada_Shortcut.SelectedRows.Count > 0)
            {
                // Get the index of the selected row
                int selectedIndex = GridPlasada_Shortcut.SelectedRows[0].Index;

                // Get the FIGHT ID of the selected row
                int fightId = Convert.ToInt32(GridPlasada_Shortcut.Rows[selectedIndex].Cells["FIGHT"].Value);

                // Delete the row from the database
                DeleteRowFromDatabase(fightId);

                // Remove the row from the DataGridView
                GridPlasada_Shortcut.Rows.RemoveAt(selectedIndex);             

                // Update the IDs in the database
                UpdateIDsInDatabase(fightId);

                // Raise the event
                ButtonEnterClicked?.Invoke(this, EventArgs.Empty);

                GridPlasada_Shortcut.Rows.Clear();
                PopulateGridPlasadaShortcut();


                MessageBox.Show("Row deleted successfully.");
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void UpdateIDsInDatabase(int deletedFightId)
        {
            try
            {
                // Example connection string for SQLite
                string connectionString = "Data Source=dofox.db;Version=3;";

                // Create an SQLite connection object
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Define the SQL command to update the IDs in the database
                    string updateQuery = "UPDATE PLASADA SET FIGHT = FIGHT - 1 WHERE FIGHT > @DeletedFightId";

                    // Create a SQLiteCommand object
                    using (var command = new SQLiteCommand(updateQuery, connection))
                    {
                        // Set the parameter value
                        command.Parameters.AddWithValue("@DeletedFightId", deletedFightId);

                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while updating IDs: " + ex.Message);
            }
        }


        private void DeleteRowFromDatabase(int fightId)
        {
            try
            {
                // Example connection string for SQLite
                string connectionString = "Data Source=dofox.db;Version=3;";

                // Create an SQLite connection object
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Define the SQL command to delete the row from the database
                    string deleteQuery = "DELETE FROM PLASADA WHERE FIGHT = @FightId";

                    // Create a SQLiteCommand object
                    using (var command = new SQLiteCommand(deleteQuery, connection))
                    {
                        // Set the parameter value
                        command.Parameters.AddWithValue("@FightId", fightId);

                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while deleting the row: " + ex.Message);
            }
        }

        private void button_ClearFields_Click(object sender, EventArgs e)
        {
            ClearInputFields();

            MessageBox.Show("Successfully cleared the fields!");

        }


        private void textBox_MeronBet_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a digit or a control key (like backspace or delete)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // If the key is not a digit or a control key, suppress the key press event
                e.Handled = true;
            }
        }

        private void textBox_WalaBet_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a digit or a control key (like backspace or delete)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // If the key is not a digit or a control key, suppress the key press event
                e.Handled = true;
            }
        }

        private void textBox_Pago_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a digit or a control key (like backspace or delete)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // If the key is not a digit or a control key, suppress the key press event
                e.Handled = true;
            }
        }

        private void textBox_EnterAmountRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is a digit or a control key (like backspace or delete)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                // If the key is not a digit or a control key, suppress the key press event
                e.Handled = true;
            }
        }

        private void button_Inspection_Click(object sender, EventArgs e)
        {
            // Create an instance of the InspectionForm class
            InspectionForm inspectionForm = new InspectionForm();

            // Show the form
            inspectionForm.Show();

            inspectionForm.ReloadData();
        }
    }

}
