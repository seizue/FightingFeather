using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;
using System.Data.SqlClient;
using Newtonsoft.Json;
using MetroFramework.Controls;
using System.Configuration;
using MetroFramework.Forms;
using System.Reflection;
using System.Drawing.Text;

namespace FightingFeather
{
    public partial class Main : MetroFramework.Forms.MetroForm
    {
        private Color defaultColor = Color.FromArgb(0, 0, 42); // Default color 
        private Color clickedColor = Color.FromArgb(193, 84, 55); // Color when the button is clicked

        private SQLiteConnection connection;
     
        private string connectionString = "Data Source = munton_posted.db;Version=3;";
        private int currentTableNumber = 0;

        private const string DatabaseFileName = "dofox.db";
        private string databasePath;
        private DataTable dataTable;

        private UserControl_Summa summa;
        private SettingsForm settingsForm;
        private ReminderForm reminderForm;

        public Main()
        {
            InitializeComponent();

            // Initialize the dataTable object
            dataTable = new DataTable();

            // Get the application directory and set up the database path
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            databasePath = Path.Combine(appDirectory, DatabaseFileName);

            connection = new SQLiteConnection("Data Source=munton_summa.db;Version=3;");
            connection.Open();
            
            InitializeDatabase();
            CreateSummaTables();
            UpdateFightIDs();
            RefreshGrid();
            RefreshCalculationDatagrid();
            LoadColumnVisibilitySettings();
            RevertColumnVisibilityChanges();

            // Set up the custom scroll bar
            FFCustomScroll();

            // Subscribe to the CellFormatting event
            GridPlasada_Entries.CellFormatting += GridPlasada_Entries_CellFormatting;

            // Subscribe to the CellClick event
            GridPlasada_Entries.CellClick += GridPlasada_Entries_CellClick;

            //Subscribe to the CellPainting event
            GridPlasada_Entries.CellPainting += GridPlasada_Entries_CellPainting;

            // Subscribe to the CellValueChanged event
            GridPlasada_Entries.CellValueChanged += GridPlasada_Entries_CellValueChanged;

            // Subscribe to the Shortcut ButtonEnter event
            userControl_Shortcut1.ButtonEnterClicked += UserControl_Shortcut1_ButtonEnterClicked;

            // Set raDateTimePicker1 to show the current date
            raDateTimePicker1.Value = DateTime.Today;

            ToolTip toolTip1 = new ToolTip();

            // Set the tooltip text for the button
            toolTip1.SetToolTip(button_Receipt, "Print Munton Receipt");
            toolTip1.SetToolTip(button_New, "Add New Entry");
            toolTip1.SetToolTip(button_Update, "Update Entry");
            toolTip1.SetToolTip(button_Delete, "Delete Entry");
            toolTip1.SetToolTip(raDateTimePicker1, "Select Date");
            toolTip1.SetToolTip(button_CreateNewPlasada, "Save Munton");

            // Custom cell height row
            foreach (DataGridViewRow row in GridPlasada_Entries.Rows)
            {
                row.Height = 30;
            }

            summa = new UserControl_Summa();
            settingsForm = new SettingsForm();
            settingsForm.ReminderToggleChanged += SettingsForm_ReminderToggleChanged;

            // Set the initial window state based on the saved value
            string savedWindowState = Properties.Settings.Default.MainFormWindowState;
            if (!string.IsNullOrEmpty(savedWindowState) && Enum.IsDefined(typeof(FormWindowState), savedWindowState))
            {
                // Parse the saved value and set the window state
                this.WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), savedWindowState);
            }

            // Set KeyPreview to true
            this.KeyPreview = true;

            // Wire up the KeyDown event handler
            this.KeyDown += Main_KeyDown;

            // Subscribe to the FormClosing event
            this.FormClosing += Main_FormClosing;

        }

        public void LoginComplete(LoginRegisterForm.UserType userType)
        {
            // Show or hide the admin button based on the user type
            button_Admin.Visible = userType == LoginRegisterForm.UserType.Admin;
            AdminDivider.Visible = userType == LoginRegisterForm.UserType.Admin;
        }

        public DateTime RaDateTimePickerValue
        {
            get { return raDateTimePicker1.Value; }
        }

        private void InitializeDatabase()
        {
            // Check if the database file exists, create it if not
            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile(databasePath);

                // Perform initial database setup
                using (var connection = new SQLiteConnection($"Data Source={databasePath}"))
                {
                    connection.Open();

                    // Query to create PLASADA table
                    string createPlasadaTableQuery = @"
                CREATE TABLE IF NOT EXISTS PLASADA (
                    FIGHT INTEGER PRIMARY KEY,
                    MERON TEXT,            
                    WALA TEXT,
                    [BET (M)] INTEGER,
                    [BET (W)] INTEGER,
                    [INITIAL BET DIFF] INTEGER,
                    PAREHAS INTEGER,
                    PAGO INTEGER,
                    WINNER TEXT,
                    [RATE AMOUNT] INTEGER,
                    RATE TEXT,
                    LOGRO INTEGER,
                    FEE REAL,
                    [TOTAL PLASADA] INTEGER,
                    [RATE EARNINGS] INTEGER,
                    [WINNERS EARN] INTEGER
                );";

                    // Execute queries to create tables
                    using (var command = new SQLiteCommand(createPlasadaTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                    }

                }
            }
        }


        private void CalculateParehasValues()
        {
            // Iterate through the rows of the DataGridView
            for (int i = 0; i < GridPlasada_Entries.Rows.Count; i++)
            {
                // Retrieve the PAGO value from the current row
                string pagoString = GridPlasada_Entries.Rows[i].Cells["PAGO"].Value?.ToString();

                // Skip calculation if PAGO is empty or null
                if (string.IsNullOrEmpty(pagoString))
                    continue;

                int pago = 0;
                if (int.TryParse(pagoString, out pago))
                {
                    // Retrieve the BET (W) value from the current row
                    int betW;
                    if (!int.TryParse(GridPlasada_Entries.Rows[i].Cells["BET_W"].Value.ToString(), out betW))
                    {
                        MessageBox.Show("Invalid value for BET (W) in row " + (i + 1));
                        return;
                    }

                    // Calculate the PAREHAS value based on the formula
                    int parehas = betW + pago;

                    // Set the calculated PAREHAS value to the corresponding cell in the DataGridView
                    GridPlasada_Entries.Rows[i].Cells["PAREHAS"].Value = parehas;
                }
                else
                {
                    return;
                }
            }
        }

        public decimal CalculateFee(decimal meronBet, decimal walaBet, string winner, decimal pago)
        {
            decimal fee = 0;

            if (winner == "M")
            {
                decimal sum = pago + walaBet; // Calculate the sum of pago and meronWala
                fee = sum * 0.10m; // Calculate 10% of the sum
            }

            else if (winner == "W")
            {
                fee = walaBet * 0.10m;
            }
            else if (winner == "Cancel" || winner == "Draw")
            {
                fee = 0;
            }

            return fee;
        }

        public void RefreshGrid()
        {
            // Clear the DataGridView
            GridPlasada_Entries.Rows.Clear();

            using (var connection = new SQLiteConnection($"Data Source={databasePath}"))
            {
                connection.Open();
                string selectQuery = "SELECT FIGHT, MERON, WALA, [BET (M)], [BET (W)], [INITIAL BET DIFF], PAREHAS, PAGO, WINNER, [RATE AMOUNT], RATE, LOGRO, FEE, [TOTAL PLASADA], [RATE EARNINGS], [WINNERS EARN] FROM PLASADA";

                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                PopulateGridRow(reader, connection);
                            }
                        }
                    }
                }
            }

            // Calculate the PAREHAS values after populating the DataGridView
            CalculateParehasValues();

            // Refresh the Calculations
            RefreshCalculationDatagrid();

            // Save to a backup json file
            SaveDataGridViewToJson();
        }


        public void PopulateGridRow(SQLiteDataReader reader, SQLiteConnection connection)
        {
            // Handle DBNull for nullable fields
            int initialBetDiff = Convert.IsDBNull(reader["INITIAL BET DIFF"]) ? 0 : Convert.ToInt32(reader["INITIAL BET DIFF"]);
            int pago = Convert.IsDBNull(reader["PAGO"]) ? 0 : Convert.ToInt32(reader["PAGO"]);
            int walaBet = Convert.IsDBNull(reader["BET (W)"]) ? 0 : Convert.ToInt32(reader["BET (W)"]);
            int rateAmount = Convert.IsDBNull(reader["RATE AMOUNT"]) ? 0 : Convert.ToInt32(reader["RATE AMOUNT"]);
            int rateResult = Convert.IsDBNull(reader["RATE EARNINGS"]) ? 0 : Convert.ToInt32(reader["RATE EARNINGS"]);
            string winner = Convert.IsDBNull(reader["WINNER"]) ? string.Empty : reader["WINNER"].ToString();


            // Call CalculateFee to get the fee value
            decimal fee = CalculateFee(Convert.ToDecimal(reader["BET (M)"]), Convert.ToDecimal(reader["BET (W)"]), reader["WINNER"].ToString(), pago);

            // Determine if fee is 0, then adjust totalPlasada accordingly
            decimal totalPlasada = fee != 0 ? fee + 300 : 0;

            // Calculate LOGRO
            int logro = initialBetDiff - (pago + rateAmount);

            // Update the LOGRO column in the database
            string updateLogroQuery = "UPDATE PLASADA SET LOGRO = @Logro WHERE FIGHT = @FightId";
            using (var updateCommand = new SQLiteCommand(updateLogroQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@Logro", logro);
                updateCommand.Parameters.AddWithValue("@FightId", reader["FIGHT"]);
                updateCommand.ExecuteNonQuery();
            }

            int betW = Convert.ToInt32(reader["BET (W)"]);

            // Calculate the PAREHAS value based on the formula
            int parehas = betW + pago;

            decimal winnersEarning = 0;

            if (winner == "M")
            {
                winnersEarning = parehas - totalPlasada + rateResult;
            }
            else if (winner == "W")
            {
                winnersEarning = walaBet - totalPlasada;
            }

            // Update the PAREHAS column in the database
            string updateParehasQuery = "UPDATE PLASADA SET PAREHAS = @Parehas WHERE FIGHT = @FightId";
            using (var updateCommand = new SQLiteCommand(updateParehasQuery, connection))
            {
                updateCommand.Parameters.AddWithValue("@Parehas", parehas);
                updateCommand.Parameters.AddWithValue("@FightId", reader["FIGHT"]);
                updateCommand.ExecuteNonQuery();
            }

            // Save calculated values back to the database
            string updateValuesQuery = "UPDATE PLASADA SET FEE = @Fee, [TOTAL PLASADA] = @TotalPlasada WHERE FIGHT = @FightId";
            using (var updateValuesCommand = new SQLiteCommand(updateValuesQuery, connection))
            {
                updateValuesCommand.Parameters.AddWithValue("@Fee", fee);
                updateValuesCommand.Parameters.AddWithValue("@TotalPlasada", totalPlasada);
                updateValuesCommand.Parameters.AddWithValue("@FightId", reader["FIGHT"]);
                updateValuesCommand.ExecuteNonQuery();
            }

            DataGridViewRow row = new DataGridViewRow();
            row.CreateCells(GridPlasada_Entries,
                reader["FIGHT"], reader["MERON"], reader["WALA"], reader["BET (M)"], reader["BET (W)"],
                reader["INITIAL BET DIFF"], reader["PAREHAS"], pago, reader["WINNER"],
                reader["RATE AMOUNT"], reader["RATE"], logro, fee,
                totalPlasada, reader["RATE EARNINGS"], winnersEarning);
            GridPlasada_Entries.Rows.Add(row);
        }



        public void SaveDataGridViewToJson()
        {
            // Create a list to store the data
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

            // Iterate through the rows of the DataGridView
            for (int i = 0; i < GridPlasada_Entries.Rows.Count - 1; i++)
            {
                DataGridViewRow row = GridPlasada_Entries.Rows[i];
                // Create a dictionary to store the data of each row
                Dictionary<string, object> rowData = new Dictionary<string, object>();

                // Iterate through the cells of the current row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    // Add cell value to the dictionary
                    rowData[cell.OwningColumn.HeaderText] = cell.Value;
                }

                // Add row data to the list
                data.Add(rowData);
            }


            // Convert the data to JSON
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

            // Specify the path to the JSON folder and the file name
            string jsonFolderPath = Path.Combine(Application.StartupPath, "JSON");
            string jsonFilePath = Path.Combine(jsonFolderPath, "receipt.json");

            try
            {
                // Create the JSON folder if it doesn't exist
                if (!Directory.Exists(jsonFolderPath))
                {
                    Directory.CreateDirectory(jsonFolderPath);
                }

                // Write the JSON data to the file
                File.WriteAllText(jsonFilePath, jsonData);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to JSON file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private bool IsNumeric(object value)
        {
            // Check if the value can be parsed as a number
            return double.TryParse(value.ToString(), out _);
        }


        public void CalculateAndDisplayFightTotal()
        {
            int totalFights = 0;

            // Iterate through the rows of the DataGridView
            foreach (DataGridViewRow row in GridPlasada_Entries.Rows)
            {
                // Check if the value in the WINNER column is not null or empty and not "CANCEL"
                if (row.Cells["FIGHT"].Value != null &&
                    !string.IsNullOrEmpty(row.Cells["FIGHT"].Value.ToString()) &&
                    !row.Cells["FIGHT"].Value.ToString().Equals("Cancel", StringComparison.OrdinalIgnoreCase))
                {
                    totalFights++;
                }
            }

            // Update the UserControl's textBox_TotalFight
            userControl_Summa1.UpdateFightTotal(totalFights);

            // Display the totalFights in labelValueFight
            labelValueFight.Text = totalFights.ToString();
        }


        public void CalculateAndDisplayDrawCancelTotal()
        {
            int totalDraws = 0;
            int totalCancels = 0;

            foreach (DataGridViewRow row in GridPlasada_Entries.Rows)
            {
                if (row.Cells["WINNER"].Value != null)
                {
                    string winnerValue = row.Cells["WINNER"].Value.ToString();
                    if (winnerValue.Equals("DRAW", StringComparison.OrdinalIgnoreCase))
                    {
                        totalDraws++;
                    }
                    else if (winnerValue.Equals("CANCEL", StringComparison.OrdinalIgnoreCase))
                    {
                        totalCancels++;
                    }
                }
            }

            // Update the UserControl's textBox_Draw
            userControl_Summa1.UpdateDrawCancelTotal(totalDraws);

            labelValueDraw.Text = totalDraws.ToString();
            label_ValueCancel.Text = totalCancels.ToString();
        }


        public void CalculateAndDisplayFeeTotal()
        {
            decimal totalFee = 0;

            foreach (DataGridViewRow row in GridPlasada_Entries.Rows)
            {
                if (row.Cells["FEE"].Value != null && decimal.TryParse(row.Cells["FEE"].Value.ToString(), out decimal fee))
                {
                    totalFee += fee;
                }
            }

            // Update the UserControl's textBox_Plasada
            userControl_Summa1.UpdateFeeTotal(totalFee.ToString());

            labelValueFee.Text = totalFee.ToString();
        }


        public void CalculateAndDisplayTotalPlasada()
        {
            decimal totalPlasada = 0;

            foreach (DataGridViewRow row in GridPlasada_Entries.Rows)
            {
                if (row.Cells["TOTAL_PLASADA"].Value != null && decimal.TryParse(row.Cells["TOTAL_PLASADA"].Value.ToString(), out decimal plasada))
                {
                    totalPlasada += plasada;
                }
            }

            // Update the UserControl's textBox_Total
            userControl_Summa1.UpdateTotal(totalPlasada.ToString());

            labelValuePlasada.Text = totalPlasada.ToString();
        }


        public void CalculateTotalCityTax()
        {
            int totalFights = 0;

            // Iterate through the rows of the DataGridView
            foreach (DataGridViewRow row in GridPlasada_Entries.Rows)
            {
                // Check if the value in the WINNER column is not null or empty and not "CANCEL"
                if (row.Cells["WINNER"].Value != null &&
                    !string.IsNullOrEmpty(row.Cells["WINNER"].Value.ToString()) &&
                    !row.Cells["WINNER"].Value.ToString().Equals("Cancel", StringComparison.OrdinalIgnoreCase))
                {
                    totalFights++;
                }
            }

            // Calculate total city tax
            int totalCityTax = totalFights * 300;

            // Update textBox_CityTax with the calculated total city tax
            userControl_Summa1.UpdateCityTax(totalCityTax.ToString());

            labelValueCityTax.Text = totalCityTax.ToString();
        }


        private void RefreshCalculationDatagrid()
        {
            CalculateAndDisplayFightTotal();
            CalculateAndDisplayDrawCancelTotal();
            CalculateAndDisplayFeeTotal();
            CalculateAndDisplayTotalPlasada();
            CalculateTotalCityTax();
        }

        private void UpdateFightIDs()
        {
            using (var connection = new SQLiteConnection($"Data Source={databasePath}"))
            {
                connection.Open();

                // Define the SQL command to update FIGHT IDs
                string updateQuery = "UPDATE PLASADA SET FIGHT = @NewFightID WHERE FIGHT = @OldFightID";

                // Loop through all rows in the DataGridView
                for (int i = 0; i < GridPlasada_Entries.Rows.Count; i++)
                {
                    // Update the FIGHT ID in the database table
                    using (var command = new SQLiteCommand(updateQuery, connection))
                    {
                        // Parameters for the update command
                        command.Parameters.AddWithValue("@NewFightID", i + 1);
                        command.Parameters.AddWithValue("@OldFightID", GridPlasada_Entries.Rows[i].Cells["FIGHT"].Value);

                        // Execute the update command
                        command.ExecuteNonQuery();
                    }
                }

                ResetAutoIncrement();
            }
        }


        private void ResetAutoIncrement()
        {
            using (var connection = new SQLiteConnection($"Data Source={databasePath}"))
            {
                try
                {
                    connection.Open();

                    // Check if the temporary table exists
                    string checkTempTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Temp_PLASADA'";
                    using (var command = new SQLiteCommand(checkTempTableQuery, connection))
                    {
                        var result = command.ExecuteScalar();
                        if (result == null || result.ToString() != "Temp_PLASADA")
                        {
                            // Create the temporary table if it doesn't exist
                            string createTempTableQuery = @"CREATE TABLE Temp_PLASADA (
                        FIGHT INTEGER PRIMARY KEY,
                        MERON TEXT,
                        WALA TEXT,
                        [BET (M)] INTEGER,                   
                        [BET (W)] INTEGER,
                        [INITIAL BET DIFF] INTEGER,
                        PAREHAS INTEGER,
                        PAGO INTEGER,
                        WINNER TEXT,
                        [RATE AMOUNT] INTEGER,
                        RATE TEXT,
                        LOGRO INTEGER,
                        FEE REAL,
                        [TOTAL PLASADA] INTEGER,
                        [RATE EARNINGS] INTEGER,
                        [WINNERS EARN] INTEGER
                    )";

                            using (var createCommand = new SQLiteCommand(createTempTableQuery, connection))
                            {
                                createCommand.ExecuteNonQuery();
                                Console.WriteLine("Temp_PLASADA table created.");
                            }
                        }
                    }

                    // Copy data from the original table to the temporary table
                    string copyDataQuery = "INSERT INTO Temp_PLASADA SELECT * FROM PLASADA";
                    using (var command = new SQLiteCommand(copyDataQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Data copied to Temp_PLASADA.");
                    }

                    // Drop the original table if it exists
                    string dropTableQuery = "DROP TABLE IF EXISTS PLASADA";
                    using (var command = new SQLiteCommand(dropTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("PLASADA table dropped.");
                    }

                    // Rename the temporary table to the original table name
                    string renameTableQuery = "ALTER TABLE Temp_PLASADA RENAME TO PLASADA";
                    using (var command = new SQLiteCommand(renameTableQuery, connection))
                    {
                        command.ExecuteNonQuery();
                        Console.WriteLine("Temp_PLASADA renamed to PLASADA.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("An error occurred: " + ex.Message);
                    // Handle the exception as needed
                }
            }
        }

        private void button_New_Click(object sender, EventArgs e)
        {
            EntryForm entryForm = new EntryForm(this, databasePath);

            // Hide the Save button in the EntryForm
            entryForm.ToggleUpdateButtonVisibility(false);

            entryForm.ShowDialog();

        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            // Check if there is a selected row
            if (GridPlasada_Entries.SelectedRows.Count > 0)
            {
                // Get the data of the selected row
                int selectedIndex = GridPlasada_Entries.SelectedRows[0].Index;
                int fightId = Convert.ToInt32(GridPlasada_Entries.Rows[selectedIndex].Cells["FIGHT"].Value);
                string meronName = GridPlasada_Entries.Rows[selectedIndex].Cells["MERON"].Value.ToString();
                int meronBet;
                if (!int.TryParse(GridPlasada_Entries.Rows[selectedIndex].Cells["BET_M"].Value.ToString(), out meronBet))
                {
                    MessageBox.Show("Invalid value for Meron Bet.");
                    return;
                }

                string walaName = GridPlasada_Entries.Rows[selectedIndex].Cells["WALA"].Value.ToString();
                int walaBet;
                if (!int.TryParse(GridPlasada_Entries.Rows[selectedIndex].Cells["BET_W"].Value.ToString(), out walaBet))
                {
                    MessageBox.Show("Invalid value for Wala Bet.");
                    return;
                }

                int pago; // Retrieve PAGO value from the selected row
                if (!int.TryParse(GridPlasada_Entries.Rows[selectedIndex].Cells["PAGO"].Value.ToString(), out pago))
                {
                    MessageBox.Show("Invalid value for PAGO.");
                    return;
                }

                string winner = GridPlasada_Entries.Rows[selectedIndex].Cells["WINNER"].Value.ToString();
                string rate = GridPlasada_Entries.Rows[selectedIndex].Cells["RATE"].Value.ToString();

                // Declare the rateamount variable outside the if block
                int rateamount = 0;

                // Check if the "RATE AMOUNT" cell is not null and not empty
                if (GridPlasada_Entries.Rows[selectedIndex].Cells["RATE_AMOUNT"].Value != null &&
                    !string.IsNullOrEmpty(GridPlasada_Entries.Rows[selectedIndex].Cells["RATE_AMOUNT"].Value.ToString()))
                {
                    // Parse RATE EARNINGS
                    if (!int.TryParse(GridPlasada_Entries.Rows[selectedIndex].Cells["RATE_AMOUNT"].Value.ToString(), out rateamount))
                    {
                        rateamount = 0;
                    }
                }

                // Create an instance of the EntryForm and load data for editing
                EntryForm entryForm = new EntryForm(this, databasePath);
                entryForm.LoadDataForEditing(fightId, meronName, meronBet, walaName, walaBet, pago, winner, rate, rateamount);

                // Hide the Save button in the EntryForm
                entryForm.ToggleSaveButtonVisibility(false);

                entryForm.ShowDialog();

                GridPlasada_Entries.CellFormatting += GridPlasada_Entries_CellFormatting;


            }
            else
            {
                MessageBox.Show("Please select a row to update.");
            }
        }

        private void button_Delete_Click(object sender, EventArgs e)
        {
            // Check if there is a selected row
            if (GridPlasada_Entries.SelectedRows.Count > 0)
            {
                // Get the index of the selected row
                int selectedIndex = GridPlasada_Entries.SelectedRows[0].Index;

                // Get the value of the "FIGHT" column from the selected row
                int fightId = Convert.ToInt32(GridPlasada_Entries.Rows[selectedIndex].Cells["FIGHT"].Value);

                // Delete the row from the database
                using (var connection = new SQLiteConnection($"Data Source={databasePath}"))
                {
                    connection.Open();
                    string deleteQuery = "DELETE FROM PLASADA WHERE FIGHT = @FightId";
                    using (var command = new SQLiteCommand(deleteQuery, connection))
                    {
                        command.Parameters.AddWithValue("@FightId", fightId);
                        command.ExecuteNonQuery();
                    }
                }


                // Remove the row from the DataGridView
                GridPlasada_Entries.Rows.RemoveAt(selectedIndex);

                ResetAutoIncrement();

                UpdateFightIDs();

                RefreshGrid();
                RefreshCalculationDatagrid();

                userControl_Earnings1.ReloadData();
                userControl_CashBreakDown1.ReloadData();

                GridPlasada_Entries.CellFormatting += GridPlasada_Entries_CellFormatting;

                MessageBox.Show("Successfully deleted.");

            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }

        private void button_Refresh_Click(object sender, EventArgs e)
        {
            // Clear existing rows in the DataGridView
            GridPlasada_Entries.Rows.Clear();

            // Refresh the grid with the updated data
            RefreshGrid();

            RefreshCalculationDatagrid();

            userControl_Earnings1.ReloadData();
            userControl_CashBreakDown1.ReloadData();

            // Display a message indicating successful refresh
            MessageBox.Show("Refresh successful!");
        }

        private void button_Export_Click(object sender, EventArgs e)
        {
            // Show a save file dialog to choose where to save the CSV file
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV (Comma delimited)|*.csv";
            saveFileDialog.Title = "Save CSV File";
            saveFileDialog.ShowDialog();

            // If the user didn't cancel the dialog and entered a filename
            if (saveFileDialog.FileName != "")
            {
                try
                {
                    // Create a new StreamWriter to write to the CSV file
                    using (StreamWriter sw = new StreamWriter(saveFileDialog.FileName))
                    {
                        // Write the column headers of visible columns to the file
                        foreach (DataGridViewColumn column in GridPlasada_Entries.Columns)
                        {
                            if (column.Visible)
                            {
                                sw.Write("\"" + column.HeaderText + "\",");
                            }
                        }
                        sw.WriteLine();

                        // Write each row of data to the file
                        foreach (DataGridViewRow row in GridPlasada_Entries.Rows)
                        {
                            foreach (DataGridViewCell cell in row.Cells)
                            {
                                if (cell.OwningColumn.Visible)
                                {
                                    // Check for null values and replace them with empty strings
                                    if (cell.Value != null)
                                    {
                                        string valueToWrite = cell.Value.ToString();

                                        // Prepend an apostrophe to force Excel to treat the value as text
                                        if (cell.OwningColumn.Name == "RATE")
                                        {
                                            valueToWrite = "'" + valueToWrite;
                                        }

                                        sw.Write("\"" + valueToWrite + "\"");
                                    }
                                    sw.Write(",");
                                }
                            }
                            sw.WriteLine();
                        }
                    }

                    MessageBox.Show("CSV file exported successfully!", "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting CSV file: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_CreateNewPlasada_Click(object sender, EventArgs e)
        {
            // Check if the Reminder toggle is off
            if (!Properties.Settings.Default.ReminderEnabled)
            {
                // Create an instance of ReminderForm and pass the value of raDateTimePicker1
                ReminderForm reminderForm = new ReminderForm(raDateTimePicker1.Value);

                // Show ReminderForm as a dialog
                DialogResult result = reminderForm.ShowDialog();

                // Check if the form was closed with OK result
                if (result == DialogResult.OK)
                {
                    // Execute the action if the user clicked "Continue" in ReminderForm
                    ExecutePlasadaCreation();

                    // Clear the DataGridView
                    GridPlasada_Entries.Rows.Clear();
                    DeleteAllRowsFromPlasadaTable();
                }
            }
            else
            {
                // If Reminder is enabled, directly execute the action
                ExecutePlasadaCreation();

                // Clear the DataGridView
                GridPlasada_Entries.Rows.Clear();
                DeleteAllRowsFromPlasadaTable();
            }
        }


        private void ExecutePlasadaCreation()
        {
            try
            {
                // Define your custom schema here
                string customSchema = @"
        (
            ""FIGHT"" INTEGER,
            ""MERON"" TEXT,
            ""WALA"" TEXT,
            ""BET (M)"" INTEGER,
            ""BET (W)"" INTEGER,
            ""INITIAL BET DIFF"" INTEGER,
            ""PAREHAS"" INTEGER,
            ""PAGO"" INTEGER,
            ""WINNER"" TEXT,
            ""RATE AMOUNT"" INTEGER,
            ""RATE"" TEXT,
            ""LOGRO"" INTEGER,
            ""FEE"" REAL,
            ""TOTAL PLASADA"" INTEGER,
            ""RATE EARNINGS"" INTEGER,
            ""WINNERS EARN"" INTEGER,
            ""DATE"" INTEGER,
            ""TotalFights"" INTEGER,
            ""TotalDraws"" INTEGER,
            ""TotalCancels"" INTEGER,
            ""TotalFee"" REAL,
            ""TotalCitytax"" INTEGER,
            ""OverAllTotalPlasada"" REAL
        )";

                currentTableNumber = GetNextTableNumber();
                DateTime currentDate = DateTime.Now.Date; // Get current date without time

                // Create custom table in your current database
                CreateCustomTable(currentTableNumber, customSchema, currentDate);

                // Save data to current database
                SaveDataToDatabase(currentDate);

                // Save table to JSON
                SaveTableToJson();

                // Create tables for summary
                CreateSummaTables();

                // Export data using SQLiteExport method in UserControl_Summa1
                userControl_Summa1.PerformSQLiteExport();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while creating Plasada tables: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void CreateSummaTables()
        {
            // Now, create tables in munton_summa.db
            string muntonDbConnectionString = "Data Source=munton_summa.db;Version=3;";

            try
            {
                using (SQLiteConnection muntonDbConnection = new SQLiteConnection(muntonDbConnectionString))
                {
                    muntonDbConnection.Open();

                    // Create Plasada_CashCount table if not exists
                    string createPlasadaCashCountTableQuery = @"CREATE TABLE IF NOT EXISTS Plasada_CashCount (
                                                         ""Amount"" INTEGER,
                                                         ""Quantity"" INTEGER,
                                                         ""TotalAmount"" INTEGER,
                                                         ""Date"" INTEGER
                                                         )";
                    using (SQLiteCommand createCommand = new SQLiteCommand(createPlasadaCashCountTableQuery, muntonDbConnection))
                    {
                        createCommand.ExecuteNonQuery();
                    }

                    // Create Plasada_Summary table if not exists
                    string createPlasadaSummaryTableQuery = @"CREATE TABLE IF NOT EXISTS Plasada_Summary (
                                                      ""ID"" INTEGER PRIMARY KEY AUTOINCREMENT,
                                                      ""Date"" TEXT,
                                                      ""Plasada"" INTEGER,
                                                      ""CityTax"" REAL,
                                                      ""Total"" REAL,
                                                      ""TotalFight"" INTEGER,
                                                      ""Draw"" NUMERIC,
                                                      ""Gate"" NUMERIC
                                                      )";
                    using (SQLiteCommand createCommand = new SQLiteCommand(createPlasadaSummaryTableQuery, muntonDbConnection))
                    {
                        createCommand.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while creating Summa tables: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void DeleteAllRowsFromPlasadaTable()
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection($"Data Source={databasePath};Version=3;"))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(connection))
                    {
                        // Delete all rows from the PLASADA table
                        command.CommandText = "DELETE FROM PLASADA";
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while deleting rows from the PLASADA table: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private int GetNextTableNumber()
        {
            // Fetch the highest existing table number from the database
            int nextTableNumber = 1;

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = "SELECT MAX(CAST(SUBSTR(tbl_name, LENGTH('MTN_ID_') + 1) AS INTEGER)) FROM sqlite_master WHERE tbl_name LIKE 'MTN_ID_%'";
                    var result = command.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        nextTableNumber = Convert.ToInt32(result) + 1;
                    }
                }
            }

            return nextTableNumber;
        }


        private void CreateCustomTable(int tableNumber, string schema, DateTime date)
        {
            string dateString = date.ToString("yyyyMMdd");
            string tableName = $"MTN_ID_{tableNumber:D8}_{dateString}";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = $"CREATE TABLE IF NOT EXISTS {tableName} {schema}";
                    command.ExecuteNonQuery();
                }
            }
        }


        private void InsertDataIntoTable(int fight, string meron, string wala, int betM, int betW, int initialBetDiff,
                                 int parehas, int pago, string winner, int rateAmount, string rate, int logro,
                                 double fee, int totalPlasada, int rateEarnings, int winnersEarn, int tableNumber, DateTime saveDate,
                                 int totalFights, int totalDraws, int totalCancels, double totalFee, int totalCityTax, double overallTotalPlasada)
        {
            string dateString = saveDate.ToString("yyyyMMdd");
            string tableName = $"MTN_ID_{tableNumber:D8}_{dateString}";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    command.CommandText = $@"INSERT INTO {tableName} (FIGHT, MERON, WALA, [BET (M)], [BET (W)], [INITIAL BET DIFF], PAREHAS, PAGO, WINNER,
                                [RATE AMOUNT], RATE, LOGRO, FEE, [TOTAL PLASADA], [RATE EARNINGS], [WINNERS EARN], [DATE], TotalFights, TotalDraws, TotalCancels, TotalFee, TotalCitytax, OverAllTotalPlasada)
                                VALUES (@fight, @meron, @wala, @betM, @betW, @initialBetDiff, @parehas, @pago, @winner,
                                @rateAmount, @rate, @logro, @fee, @totalPlasada, @rateEarnings, @winnersEarn, @saveDate, @totalFights, @totalDraws, @totalCancels, @totalFee, @totalCityTax, @overallTotalPlasada)";
                    command.Parameters.AddWithValue("@fight", fight);
                    command.Parameters.AddWithValue("@meron", meron);
                    command.Parameters.AddWithValue("@wala", wala);
                    command.Parameters.AddWithValue("@betM", betM);
                    command.Parameters.AddWithValue("@betW", betW);
                    command.Parameters.AddWithValue("@initialBetDiff", initialBetDiff);
                    command.Parameters.AddWithValue("@parehas", parehas);
                    command.Parameters.AddWithValue("@pago", pago);
                    command.Parameters.AddWithValue("@winner", winner);
                    command.Parameters.AddWithValue("@rateAmount", rateAmount);
                    command.Parameters.AddWithValue("@rate", rate);
                    command.Parameters.AddWithValue("@logro", logro);
                    command.Parameters.AddWithValue("@fee", fee);
                    command.Parameters.AddWithValue("@totalPlasada", totalPlasada);
                    command.Parameters.AddWithValue("@rateEarnings", rateEarnings);
                    command.Parameters.AddWithValue("@winnersEarn", winnersEarn);
                    command.Parameters.AddWithValue("@saveDate", saveDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@totalFights", totalFights);
                    command.Parameters.AddWithValue("@totalDraws", totalDraws);
                    command.Parameters.AddWithValue("@totalCancels", totalCancels);
                    command.Parameters.AddWithValue("@totalFee", totalFee);
                    command.Parameters.AddWithValue("@totalCityTax", totalCityTax);
                    command.Parameters.AddWithValue("@overallTotalPlasada", overallTotalPlasada);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void SaveDataToDatabase(DateTime saveDate)
        {
            // Track whether any data was successfully saved
            bool dataSaved = false;

            // Iterate through the rows of the DataGridView
            for (int i = 0; i < GridPlasada_Entries.Rows.Count - 1; i++)
            {
                DataGridViewRow row = GridPlasada_Entries.Rows[i];
                
                try
                {
                    // Assuming the columns are in the same order as defined in the custom schema
                    int fight = row.Cells["FIGHT"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["FIGHT"].Value) : 0;
                    string meron = row.Cells["MERON"].Value != DBNull.Value ? Convert.ToString(row.Cells["MERON"].Value) : string.Empty;
                    int betM = row.Cells["BET_M"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["BET_M"].Value) : 0;
                    string wala = row.Cells["WALA"].Value != DBNull.Value ? Convert.ToString(row.Cells["WALA"].Value) : string.Empty;

                    int betW = row.Cells["BET_W"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["BET_W"].Value) : 0;
                    int initialBetDiff = row.Cells["INITIAL_BET_DIF"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["INITIAL_BET_DIF"].Value) : 0;
                    int parehas = row.Cells["PAREHAS"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["PAREHAS"].Value) : 0;
                    int pago = row.Cells["PAGO"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["PAGO"].Value) : 0;
                    string winner = row.Cells["WINNER"].Value != DBNull.Value ? Convert.ToString(row.Cells["WINNER"].Value) : string.Empty;
                    int rateAmount = row.Cells["RATE_AMOUNT"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["RATE_AMOUNT"].Value) : 0;
                    string rate = row.Cells["RATE"].Value != DBNull.Value ? Convert.ToString(row.Cells["RATE"].Value) : string.Empty;
                    int logro = row.Cells["LOGRO"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["LOGRO"].Value) : 0;
                    double fee = row.Cells["FEE"].Value != DBNull.Value ? Convert.ToDouble(row.Cells["FEE"].Value) : 0;
                    int totalPlasada = row.Cells["TOTAL_PLASADA"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["TOTAL_PLASADA"].Value) : 0;
                    int rateEarnings = row.Cells["RATE_EARNINGS"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["RATE_EARNINGS"].Value) : 0;
                    int winnersEarn = row.Cells["WINNERS_EARN"].Value != DBNull.Value ? Convert.ToInt32(row.Cells["WINNERS_EARN"].Value) : 0;
                    int totalFights = Convert.ToInt32(labelValueFight.Text);
                    int totalDraws = Convert.ToInt32(labelValueDraw.Text);
                    int totalCancels = Convert.ToInt32(label_ValueCancel.Text);
                    double totalFee = Convert.ToDouble(labelValueFee.Text);
                    int totalCityTax = Convert.ToInt32(labelValueCityTax.Text);
                    double overallTotalPlasada = Convert.ToDouble(labelValuePlasada.Text);

                    InsertDataIntoTable(fight, meron, wala, betM, betW, initialBetDiff, parehas, pago, winner,
                                 rateAmount, rate, logro, fee, totalPlasada, rateEarnings, winnersEarn, currentTableNumber, saveDate, totalFights, totalDraws, totalCancels,
                                 totalFee,totalCityTax,overallTotalPlasada);

                    // Set dataSaved to true if data was successfully saved
                    dataSaved = true;

                }
                catch (FormatException ex)
                {
                    // Log the problematic row or specific values causing the exception
                    Console.WriteLine($"FormatException occurred: {ex.Message}");
                    Console.WriteLine("Problematic row:");
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        Console.WriteLine($"{cell.OwningColumn.Name}: {cell.Value}");
                    }
                }
            }

            // Display success message if data was saved
            if (dataSaved)
            {
                MessageBox.Show("Data saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("No data saved.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }



        private void SaveTableToJson()
        {
            try
            {
                // Create a list to store the data
                List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

                // Get the date from raDateTimePicker1
                DateTime date = raDateTimePicker1.Value.Date;

                // Additional data dictionary
                Dictionary<string, object> additionalData = new Dictionary<string, object>();
                additionalData["Date"] = date.ToString("yyyy-MM-dd");
                additionalData["totalFights"] = labelValueFight.Text;
                additionalData["totalDraws"] = labelValueDraw.Text;
                additionalData["totalCancels"] = label_ValueCancel.Text;
                additionalData["totalFee"] = labelValueFee.Text;
                additionalData["totalCityTax"] = labelValueCityTax.Text;
                additionalData["OverAllTotalPlasada"] = labelValuePlasada.Text;

                // Add additional data to the list
                data.Add(additionalData);

                // Iterate through the rows of the DataGridView
                for (int i = 0; i < GridPlasada_Entries.Rows.Count - 1; i++)
                {
                    DataGridViewRow row = GridPlasada_Entries.Rows[i];
                    // Create a dictionary to store the data of each row
                    Dictionary<string, object> rowData = new Dictionary<string, object>();

                    // Iterate through the cells of the current row
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        // Add cell value to the dictionary
                        rowData[cell.OwningColumn.HeaderText] = cell.Value;
                    }

                    // Add row data to the list
                    data.Add(rowData);
                }

                // Convert the data to JSON
                string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

                // Load the table number counter from the file
                int tableNumberCounter = LoadTableNumberCounter();

                // Generate the file name
                string tableNumber = tableNumberCounter.ToString().PadLeft(8, '0');
                string fileName = $"MTN_ID_{tableNumber}_{date:yyyyMMdd}.json";

                // Specify the path to the JSON folder
                string jsonFolderPath = Path.Combine(Application.StartupPath, "TABLES");

                // Combine folder path and file name
                string jsonFilePath = Path.Combine(jsonFolderPath, fileName);

                // Create the JSON folder if it doesn't exist
                if (!Directory.Exists(jsonFolderPath))
                {
                    Directory.CreateDirectory(jsonFolderPath);
                }

                // Write the JSON data to the file
                File.WriteAllText(jsonFilePath, jsonData);

                // Increment the table number counter for the next save
                tableNumberCounter++;

                // Save the updated table number counter to the file
                SaveTableNumberCounter(tableNumberCounter);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving data to JSON file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private int LoadTableNumberCounter()
        {
            string counterFilePath = Path.Combine(Application.StartupPath, "TABLES", "tableCounter.txt");

            // If the file exists, read the counter value
            if (File.Exists(counterFilePath))
            {
                string counterString = File.ReadAllText(counterFilePath);
                if (int.TryParse(counterString, out int counter))
                {
                    return counter;
                }
            }

            // If the file doesn't exist or the counter value couldn't be parsed, return 1
            return 1;
        }


        private void SaveTableNumberCounter(int counter)
        {
            string counterFilePath = Path.Combine(Application.StartupPath, "TABLES", "tableCounter.txt");
            File.WriteAllText(counterFilePath, counter.ToString());
        }



        private void SettingsForm_ReminderToggleChanged(object sender, bool isChecked)
        {
            // Show or hide the ReminderForm based on the toggle state received from the settings form
            if (isChecked)
            {
                // Create and show the ReminderForm only if it's not created yet or disposed
                if (reminderForm == null || reminderForm.IsDisposed)
                {
                    reminderForm = new ReminderForm(raDateTimePicker1.Value);
                    reminderForm.Show();
                }
                else
                {
                    reminderForm.Show();
                }
            }
            else
            {
                // Hide the ReminderForm if it's visible
                if (reminderForm != null && !reminderForm.IsDisposed && reminderForm.Visible)
                {
                    reminderForm.Hide();
                }
            }

        }


        private void label_Ernings_Click(object sender, EventArgs e)
        {

            userControl_Earnings1.Size = new Size(1055, 518);
            userControl_Earnings1.Visible = true;
            userControl_Earnings1.BringToFront();

            // Change the color of the labels and button
            label_Ernings.ForeColor = clickedColor;
            label_Entries.ForeColor = defaultColor;
            label_CashBreakDown.ForeColor = defaultColor;
            button_Home.ForeColor = clickedColor;
            button_Export.Visible = false;
            separatorRefresh.Visible = false;
            panel_Indicator.Location = new Point(311, 105);
            panel_Indicator.Size = new Size(65, 4);
            metroScrollBar1.Visible = false;

            userControl_Earnings1.ReloadData();
            userControl_CashBreakDown1.ReloadData();
        }
     

        private void label_Entries_Click(object sender, EventArgs e)
        {
          
            userControl_Earnings1.Size = new Size(0, 0);
            userControl_Earnings1.Visible = false;
            userControl_Earnings1.SendToBack();
            userControl_CashBreakDown1.SendToBack();
            
            // Change the color of the labels and button
            label_Ernings.ForeColor = defaultColor;
            label_Entries.ForeColor = clickedColor;
            label_CashBreakDown.ForeColor = defaultColor;
            button_Home.ForeColor = clickedColor;
            button_Export.Visible = true;
            separatorRefresh.Visible = true;
            panel_Indicator.Location = new Point(200, 105);
            panel_Indicator.Size = new Size(65, 4);

            RefreshGrid();
            RefreshCalculationDatagrid();

            userControl_Earnings1.ReloadData();
            userControl_CashBreakDown1.ReloadData();

            FFCustomScroll();

        }

        private void label_CashBreakDown_Click(object sender, EventArgs e)
        {
            userControl_CashBreakDown1.Visible = true;
            userControl_CashBreakDown1.BringToFront();
            userControl_Earnings1.SendToBack();

            label_CashBreakDown.ForeColor = clickedColor;
            label_Ernings.ForeColor= defaultColor;
            label_Entries.ForeColor= defaultColor;
            button_Export.Visible = false;
            separatorRefresh.Visible = false;
            panel_Indicator.Location = new Point(430, 105);
            panel_Indicator.Size = new Size(94, 4);
            metroScrollBar1.Visible = false;

            userControl_Earnings1.ReloadData();
            userControl_CashBreakDown1.ReloadData();
       
        }

        private void button_Shortcut_Click(object sender, EventArgs e)
        {
            userControl_Shortcut1.Visible = true;
            userControl_Shortcut1.BringToFront();

            button_Shortcut.ForeColor = clickedColor;
            button_Home.ForeColor = defaultColor;
            button_Inventory.ForeColor = defaultColor;
       
            userControl_Shortcut1.ReloadData();
        }

        private void button_Summa_Click(object sender, EventArgs e)
        {
            userControl_Summa1.Visible = true;
            userControl_Summa1.BringToFront();
            userControl_CashBreakDown1.Visible = false;
            userControl_Earnings1.Visible = false;

            button_Summa.ForeColor = clickedColor;
            button_Home.ForeColor = clickedColor;
            button_Plasada.ForeColor = defaultColor;

            RefreshGrid();
            RefreshCalculationDatagrid();

            // Get the selected date without the time component
            string selectedDate = raDateTimePicker1.Value.ToString("MM/dd/yyyy");

            // Pass the formatted date to the UserControl_Summa
            userControl_Summa1.SetDateText(selectedDate);
        }

        private void button_Plasada_Click(object sender, EventArgs e)
        {
            userControl_Summa1.Visible = false;
            userControl_CashBreakDown1.Visible = false;
            userControl_Earnings1.Visible = false;
            panel_Indicator.Location = new Point(200, 105);
            panel_Indicator.Size = new Size(65, 4);

            button_Export.Visible = true;
            separatorRefresh.Visible = true;

            button_Plasada.ForeColor = clickedColor;
            label_Entries.ForeColor = clickedColor;

            button_Summa.ForeColor= defaultColor;
            button_Inventory.ForeColor= defaultColor;
            label_CashBreakDown.ForeColor = defaultColor;
            label_Ernings.ForeColor = defaultColor;

            RefreshGrid();
            RefreshCalculationDatagrid();

            FFCustomScroll();

        }

        private void raDateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Get the selected date without the time component
            string selectedDate = raDateTimePicker1.Value.ToString("MM/dd/yyyy");

            // Pass the formatted date to the UserControl_Summa
            userControl_Summa1.SetDateText(selectedDate);
        }

        private void button_Home_Click(object sender, EventArgs e)
        {

            userControl_Shortcut1.Visible = false;
            userControl_Inventory1.Visible = false;
            userControl_Summa1.Visible = false;
            panel_Indicator.Location = new Point(200, 105);
            panel_Indicator.Size = new Size(65, 4);

            button_Export.Visible = true;
            separatorRefresh.Visible = true;

            GridPlasada_Entries.Visible = true;
            GridPlasada_Entries.BringToFront();
        
            button_Home.ForeColor= clickedColor;
            label_Entries.ForeColor = clickedColor;
            button_Plasada.ForeColor= clickedColor;
            label_CashBreakDown.ForeColor = defaultColor;
            label_Ernings.ForeColor = defaultColor;
            button_Summa.ForeColor = defaultColor;
            button_Inventory.ForeColor= defaultColor;
            button_Shortcut.ForeColor= defaultColor;

            RefreshGrid();
            RefreshCalculationDatagrid();

            FFCustomScroll();
        }



        // Declare a dictionary to store the initial visibility state of columns
        Dictionary<string, bool> columnVisibilityBackup = new Dictionary<string, bool>();

        // Handle MouseClick event for the DataGridView
        private void GridPlasada_Entries_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right) // Check if right mouse button is clicked
            {
                // Check if column header was clicked
                DataGridView.HitTestInfo hitTestInfo = GridPlasada_Entries.HitTest(e.X, e.Y);
                if (hitTestInfo.Type == DataGridViewHitTestType.ColumnHeader ||
                    hitTestInfo.Type == DataGridViewHitTestType.TopLeftHeader)
                {
                    // Create a context menu
                    ContextMenu menu = new ContextMenu();

                    // Add items on the menu
                    foreach (DataGridViewColumn column in GridPlasada_Entries.Columns)
                    {
                        // Exclude the FIGHT column
                        if (column.Name != "FIGHT")
                        {
                            MenuItem item = new MenuItem();

                            item.Text = column.HeaderText;
                            item.Checked = column.Visible;

                            // Check if the column visibility backup contains the column name
                            if (!columnVisibilityBackup.ContainsKey(column.Name))
                            {
                                // If not, add it with the current visibility state
                                columnVisibilityBackup[column.Name] = column.Visible;
                            }

                            // Add event if the item was clicked
                            item.Click += (obj, ea) =>
                            {
                                bool originalVisibility = columnVisibilityBackup[column.Name]; // Get the original visibility

                                column.Visible = !originalVisibility;

                                // Update the check
                                item.Checked = column.Visible;

                                // Save column visibility setting
                                Properties.Settings.Default[$"ColumnVisibility_{column.Name}"] = column.Visible;
                                Properties.Settings.Default.Save(); // Save settings

                                // Show the selection again
                                menu.Show(GridPlasada_Entries, e.Location);

                                if (column.Visible)
                                {
                                    column.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                }
                            };

                            menu.MenuItems.Add(item);
                        }
                    }

                    // Show the menu
                    menu.Show(GridPlasada_Entries, e.Location);
                }
            }
        }

        // Load column visibility settings from application settings
        private void LoadColumnVisibilitySettings()
        {
            foreach (DataGridViewColumn column in GridPlasada_Entries.Columns)
            {
                try
                {
                    // Check if the setting exists
                    if (Properties.Settings.Default.Properties[$"ColumnVisibility_{column.Name}"] != null)
                    {
                        // Set column visibility based on saved setting
                        column.Visible = (bool)Properties.Settings.Default[$"ColumnVisibility_{column.Name}"];
                    }
                }
                catch (SettingsPropertyNotFoundException ex)
                {
                    // Handle the case where the setting is not found
                    Console.WriteLine($"Setting for {column.Name} visibility not found: {ex.Message}");
                    // You can choose to set a default visibility here if needed
                }
            }
        }

        // Revert column visibility changes
        private void RevertColumnVisibilityChanges()
        {
            foreach (DataGridViewColumn column in GridPlasada_Entries.Columns)
            {
                if (columnVisibilityBackup.ContainsKey(column.Name))
                {
                    column.Visible = columnVisibilityBackup[column.Name]; // Restore original visibility
                }
            }
        }

        private void UserControl_Shortcut1_ButtonEnterClicked(object sender, EventArgs e)
        {
            // Call the RefreshGrid() method in your main form
            RefreshGrid();
        }

        private void button_Inventory_Click(object sender, EventArgs e)
        {
           
            userControl_Inventory1.Visible = true;   
            userControl_Shortcut1.Visible = false;
            userControl_Summa1.Visible = false;
          
            button_Inventory.ForeColor = clickedColor;
            button_Home.ForeColor = defaultColor;
            button_Shortcut.ForeColor = defaultColor;

            userControl_Inventory1.ReloadData();
        }

        private void button_Settings_Click(object sender, EventArgs e)
        {
            SettingsForm settingsForm = new SettingsForm();          
            settingsForm.ShowDialog();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Save the current window state whenever it changes
            Properties.Settings.Default.MainFormWindowState = this.WindowState.ToString();
            Properties.Settings.Default.Save();
        }

        private void button_Admin_Click(object sender, EventArgs e)
        {
            AdminForm adminForm = new AdminForm();
            adminForm.ShowDialog();     
        }

        private void button_DashBoard_Click(object sender, EventArgs e)
        {
            DashBoardForm dashboardForm = new DashBoardForm();
            dashboardForm.ShowDialog();
            dashboardForm.Reload();
        }

        private void Main_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if Ctrl + M keys are pressed simultaneously
            if (e.Control && e.KeyCode == Keys.M)
            {
                // Prevent the key from being processed further
                e.SuppressKeyPress = true;

                // Show the MuntonPrintForm
                MuntonPrintForm muntonPrintForm = new MuntonPrintForm();
                muntonPrintForm.ShowDialog();
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        private void Main_Load(object sender, EventArgs e)
        {
            this.ShadowType = MetroFormShadowType.None;
        }


        private void GridPlasada_Entries_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            // Check if the current cell contains text
            if (e.Value != null && e.Value.GetType() == typeof(string))
            {
                string originalText = (string)e.Value;
                if (!string.IsNullOrEmpty(originalText))
                {
                    // Convert the first letter to uppercase and the rest to lowercase
                    string formattedText = char.ToUpper(originalText[0]) + originalText.Substring(1).ToLower();
                    e.Value = formattedText;
                    // Set the cell style to display the text with the modified formatting
                    e.FormattingApplied = true;
                }
            }

            foreach (DataGridViewRow row in GridPlasada_Entries.Rows)
            {
                row.Height = 30;
            }


            // Check if the cell value is 0, 0.00, empty, or null
            if (e.RowIndex != GridPlasada_Entries.Rows.Count - 1 && (e.Value == null || e.Value.ToString() == "0" || e.Value.ToString() == "0.00" || string.IsNullOrEmpty(e.Value.ToString())))
            {
                // Replace the value with "-"
                e.Value = "-";
                e.FormattingApplied = true; // Set FormattingApplied to true to indicate that the formatting has been applied
            }

            if (e.Value != null && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Retrieve the column type
                var column = GridPlasada_Entries.Columns[e.ColumnIndex];

                // Check if the value is numeric
                if (IsNumeric(e.Value))
                {
                    // Format the value with commas for thousands separator
                    e.Value = string.Format("{0:#,0}", e.Value);
                    e.FormattingApplied = true; // Indicate that the formatting is applied
                }
            }

            // Check if the cell belongs to the "RATE" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].Name == "RATE" && e.RowIndex >= 0)
            {
                // Get the value of the cell
                string rate = GridPlasada_Entries.Rows[e.RowIndex].Cells["RATE"].Value?.ToString();

                // Check if the rate is "None"
                if (rate == "None")
                {
                    // Set the cell value to "-"
                    e.Value = "-";
                    e.FormattingApplied = true; // Indicate that the formatting is applied
                }
            }

            // Check if the column index is valid and the current cell being formatted is in the WINNER column
            if (e.ColumnIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].Name == "WINNER" && e.RowIndex >= 0)
            {
                // Get the value of the cell and ensure it's not null
                if (GridPlasada_Entries.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    string winner = GridPlasada_Entries.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                    // Set custom text color based on the value of the cell
                    if (winner == "W")
                    {
                        // Set the text color to a custom color for winner "W"
                        e.CellStyle.ForeColor = Color.Maroon; // Change this to your desired color
                        e.CellStyle.BackColor = Color.FromArgb(255, 243, 245);
                    }
                    else if (winner == "M")
                    {
                        // Set the text color to a custom color for winner "M"
                        e.CellStyle.ForeColor = Color.Green; // Change this to your desired color
                        e.CellStyle.BackColor = Color.FromArgb(239, 253, 244);
                    }
                    else if (winner == "Cancel" || winner == "Draw")
                    {
                        // Set the text color to a custom color for winner "Cancel" or "Draw"
                        e.CellStyle.ForeColor = Color.Gray; // Change this to your desired color
                    }
                    else
                    {
                        // Set the default text color for other values
                        e.CellStyle.ForeColor = GridPlasada_Entries.DefaultCellStyle.ForeColor;
                    }
                }
            }

            // Check if the cell belongs to the "INITIAL_BET_DIF" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].Name == "INITIAL_BET_DIF" && e.RowIndex >= 0)
            {
                // Set the font color for cells in the "INITIAL_BET_DIF" column
                e.CellStyle.ForeColor = Color.FromArgb(120, 30, 199);
            }

            // Check if the cell belongs to the "PAREHAS" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].Name == "PAREHAS" && e.RowIndex >= 0)
            {
                // Set the font color for cells in the "PAREHASF" column
                e.CellStyle.ForeColor = Color.FromArgb(99, 66, 0);
            }

            // Check if the cell belongs to the "RATE RESULT" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].Name == "RATE_EARNINGS" && e.RowIndex >= 0)
            {
                // Set the font color for cells in the "RATE RESULT" column
                e.CellStyle.ForeColor = Color.FromArgb(99, 66, 0);
            }

            // Check if the cell belongs to the "WINNERS EARNING" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].Name == "WINNERS_EARN" && e.RowIndex >= 0)
            {
                // Set the font color for cells in the "WINNERS EARNING" column
                e.CellStyle.ForeColor = Color.FromArgb(149, 32, 37);
            }

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewColumn column = GridPlasada_Entries.Columns[e.ColumnIndex];
                DataGridViewRow row = GridPlasada_Entries.Rows[e.RowIndex];

                // Check for MERON, WALA, WINNERS_EARN columns
                if ((column.Name == "MERON" || column.Name == "WALA" || column.Name == "WINNERS_EARN" || column.Name == "RATE_EARNINGS") && column.Name != "WINNER" && row.Cells["WINNER"].Value != null)
                {
                    string winner = row.Cells["WINNER"].Value.ToString();

                    // Determine the color based on the winner formula and if the value is greater than 0
                    if (winner == "M" && (column.Name == "MERON"))
                    {
                        // Set the color for MERON
                        row.Cells[column.Name].Style.BackColor = Color.FromArgb(239, 253, 244);
                    }
                    else if (winner == "W" && (column.Name == "WALA"))
                    {
                        // Set the color for WALA
                        row.Cells[column.Name].Style.BackColor = Color.FromArgb(255, 243, 245);
                    }
                }
            }

            // Check if the cell belongs to the "RATE" or "RATE AMOUNT" column headers
            if (e.RowIndex == -1 && (GridPlasada_Entries.Columns[e.ColumnIndex].Name == "RATE" || GridPlasada_Entries.Columns[e.ColumnIndex].Name == "RATE_AMOUNT"))
            {
                // Align the column header text to the middle-right
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }
        }

        private void GridPlasada_Entries_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.RowIndex == -1 &&
               (GridPlasada_Entries.Columns[e.ColumnIndex].Name == "FIGHT" ||
                GridPlasada_Entries.Columns[e.ColumnIndex].Name == "FEE" ||
                GridPlasada_Entries.Columns[e.ColumnIndex].Name == "WINNER" ||
                GridPlasada_Entries.Columns[e.ColumnIndex].Name == "LOGRO" ||
                GridPlasada_Entries.Columns[e.ColumnIndex].Name == "TOTAL_PLASADA" ||
                GridPlasada_Entries.Columns[e.ColumnIndex].Name == "RATE" ||
                GridPlasada_Entries.Columns[e.ColumnIndex].Name == "RATE_AMOUNT" ||
                GridPlasada_Entries.Columns[e.ColumnIndex].Name == "WINNERS_EARN"))

            {
                e.PaintBackground(e.CellBounds, true);

                using (StringFormat sf = new StringFormat())
                {
                    sf.Alignment = StringAlignment.Center;
                    sf.LineAlignment = StringAlignment.Center;

                    // Specify custom font
                    Font headerFont = new Font("Calibri", 8f, FontStyle.Bold);

                    using (Brush brush = new SolidBrush(e.CellStyle.ForeColor))
                    {
                        e.Graphics.DrawString(e.Value.ToString(), headerFont, brush, e.CellBounds, sf);
                    }
                }

                e.Handled = true;
            }

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].HeaderText == "RATE EARNINGS")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(236, 237, 240); // Change this to your desired color

                // Draw the divider line
                using (Pen dividerPen = new Pen(dividerColor, 1)) // Set the width of the divider
                {
                    // Calculate the position of the divider line
                    int x = e.CellBounds.Right - 1;
                    int y1 = e.CellBounds.Top;
                    int y2 = e.CellBounds.Bottom;

                    e.Graphics.DrawLine(dividerPen, x, y1, x, y2);
                }

                e.Handled = true;
            }

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].HeaderText == "WINNER")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(236, 237, 240); // Change this to your desired color

                // Draw the divider line
                using (Pen dividerPen = new Pen(dividerColor, 1)) // Set the width of the divider
                {
                    // Calculate the position of the divider line
                    int x = e.CellBounds.Right - 1;
                    int y1 = e.CellBounds.Top;
                    int y2 = e.CellBounds.Bottom;

                    e.Graphics.DrawLine(dividerPen, x, y1, x, y2);
                }

                e.Handled = true;
            }

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].HeaderText == "RATE")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(236, 237, 240); // Change this to your desired color

                // Draw the divider line
                using (Pen dividerPen = new Pen(dividerColor, 1)) // Set the width of the divider
                {
                    // Calculate the position of the divider line
                    int x = e.CellBounds.Right - 1;
                    int y1 = e.CellBounds.Top;
                    int y2 = e.CellBounds.Bottom;

                    e.Graphics.DrawLine(dividerPen, x, y1, x, y2);
                }

                e.Handled = true;
            }
        }

        private void GridPlasada_Entries_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the clicked cell is a full row
            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Select the entire row
                GridPlasada_Entries.Rows[e.RowIndex].Selected = true;
            }
        }


        // Event handler for the CellValueChanged event
        private void GridPlasada_Entries_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the changed cell is in the "WINNER" column
            if (e.ColumnIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].Name == "WINNER" && e.RowIndex >= 0)
            {
                // Get the value of the changed cell
                string winner = GridPlasada_Entries.Rows[e.RowIndex].Cells["WINNER"].Value?.ToString();

                // Check if the winner is "Cancel" or "Draw"
                if (winner == "Cancel" || winner == "Draw")
                {
                    // Set the corresponding cells to 0
                    GridPlasada_Entries.Rows[e.RowIndex].Cells["RATE_EARNINGS"].Value = 0;
                    GridPlasada_Entries.Rows[e.RowIndex].Cells["WINNERS_EARN"].Value = 0;
                }
            }
        }


        private void FFCustomScroll()
        {
            // Set up event handlers for the scrollbar
            metroScrollBar1.Scroll += (sender, e) =>
            {
                // Synchronize the DataGridView scroll position with the scrollbar value
                GridPlasada_Entries.FirstDisplayedScrollingRowIndex = metroScrollBar1.Value;
            };

            // Set the maximum value of the scrollbar to the total row count in the DataGridView
            metroScrollBar1.Minimum = 0;
            metroScrollBar1.Maximum = GridPlasada_Entries.RowCount - 1;
            metroScrollBar1.LargeChange = GridPlasada_Entries.DisplayedRowCount(false);
            metroScrollBar1.SmallChange = 1;

            // Set up event handlers for the DataGridView to update the scrollbar
            GridPlasada_Entries.Scroll += (sender, e) =>
            {
                metroScrollBar1.Value = GridPlasada_Entries.FirstDisplayedScrollingRowIndex;
            };
            GridPlasada_Entries.Resize += (sender, e) =>
            {
                metroScrollBar1.Height = GridPlasada_Entries.Height;
                metroScrollBar1.LargeChange = GridPlasada_Entries.DisplayedRowCount(false);
            };

            // Show or hide the scrollbar based on the row count and active cells
            UpdateScrollBarVisibility();
        }

        private void UpdateScrollBarVisibility()
        {
            // Determine if scroll bars should be visible
            bool showScrollBar = GridPlasada_Entries.RowCount > 12;

            // Show or hide the scrollbar accordingly
            metroScrollBar1.Visible = showScrollBar;
        }


        private void GridPlasada_Entries_SelectionChanged(object sender, EventArgs e)
        {
            UpdateScrollBarVisibility();

        }
    }
}
