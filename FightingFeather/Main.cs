﻿using System;
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


namespace FightingFeather
{
    public partial class Main : MetroFramework.Forms.MetroForm
    {
        private Color defaultColor = Color.FromArgb(0, 0, 42); // Default color 
        private Color clickedColor = Color.FromArgb(193, 84, 55); // Color when the button is clicked

       private SQLiteConnection sqliteConnection;
        private const string DatabaseFileName = "dofox.db";
        private string databasePath;
        private SQLiteCommand sqliteCommand;
        private SQLiteDataAdapter sqliteDataAdapter;
        private DataTable dataTable;

        public Main()
        {
            InitializeComponent();

            // Initialize the dataTable object
            dataTable = new DataTable();

            // Get the application directory and set up the database path
            string appDirectory = AppDomain.CurrentDomain.BaseDirectory;
            databasePath = Path.Combine(appDirectory, DatabaseFileName);

            InitializeDatabase();
            UpdateFightIDs();
            RefreshGrid();

            // Subscribe to the CellFormatting event
            GridPlasada_Entries.CellFormatting += GridPlasada_Entries_CellFormatting;

            // Subscribe to the CellClick event
            GridPlasada_Entries.CellClick += GridPlasada_Entries_CellClick;

            //Subscribe to the CellPainting event
            GridPlasada_Entries.CellPainting += GridPlasada_Entries_CellPainting;

            // Subscribe to the CellValueChanged event
            GridPlasada_Entries.CellValueChanged += GridPlasada_Entries_CellValueChanged;

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
                    string createTableQuery = @"CREATE TABLE IF NOT EXISTS PLASADA (
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


                    using (var command = new SQLiteCommand(createTableQuery, connection))
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
                    // PAGO is not a valid integer
                    MessageBox.Show("Invalid value for PAGO in row " + (i + 1));
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
                            GridPlasada_Entries.Rows.Clear();

                            while (reader.Read())
                            {
                                // Handle DBNull for nullable fields
                                int initialBetDiff = Convert.IsDBNull(reader["INITIAL BET DIFF"]) ? 0 : Convert.ToInt32(reader["INITIAL BET DIFF"]);
                                int pago = Convert.IsDBNull(reader["PAGO"]) ? 0 : Convert.ToInt32(reader["PAGO"]);
                                int rateAmount = Convert.IsDBNull(reader["RATE AMOUNT"]) ? 0 : Convert.ToInt32(reader["RATE AMOUNT"]);

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
                                    totalPlasada, reader["RATE EARNINGS"], reader["WINNERS EARN"]);
                                GridPlasada_Entries.Rows.Add(row);
                            }
                        }
                    }
                }

                // Calculate the PAREHAS values after populating the DataGridView
                CalculateParehasValues();
            }
        }

        private bool IsNumeric(object value)
        {
            // Check if the value can be parsed as a number
            return double.TryParse(value.ToString(), out _);
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

            // Check if the cell value is 0 and if it's not a header cell
            if (e.Value != null && e.Value.ToString() == "0" && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Replace the cell value with "-"
                e.Value = "-";
                e.FormattingApplied = true; // Indicate that the formatting is applied
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

            // Check if the cell value is 0 and if it's not a header cell
            if (e.Value != null && e.Value.ToString() == "0" && e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // Replace the cell value with "-"
                e.Value = "-";
                e.FormattingApplied = true; // Indicate that the formatting is applied
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

            // Check if the cell belongs to the "LOGRO" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].Name == "LOGRO" && e.RowIndex >= 0)
            {
                // Set the font color for cells in the "LOGRO" column
                e.CellStyle.ForeColor = Color.FromArgb(99, 66, 0);
            }

            // Check if the cell belongs to the "FEE" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].Name == "FEE" && e.RowIndex >= 0)
            {
                // Set the font color for cells in the "FEE" column
                e.CellStyle.ForeColor = Color.FromArgb(99, 66, 0);
            }

            // Check if the cell belongs to the "TOTAL PLASAD" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].Name == "TOTAL_PLASADA" && e.RowIndex >= 0)
            {
                // Set the font color for cells in the "TOTAL PLASA" column
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
                e.CellStyle.ForeColor = Color.FromArgb(150, 21, 40);
            }

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewColumn column = GridPlasada_Entries.Columns[e.ColumnIndex];
                DataGridViewRow row = GridPlasada_Entries.Rows[e.RowIndex];

                // Check for MERON, WALA, WINNERS_EARN, and RATE_EARNINGS columns
                if ((column.Name == "MERON" || column.Name == "WALA" || column.Name == "WINNERS_EARN" || column.Name == "RATE_EARNINGS") && column.Name != "WINNER" && row.Cells["WINNER"].Value != null)
                {
                    string winner = row.Cells["WINNER"].Value.ToString();

                    // Determine the color based on the winner formula and if the value is greater than 0
                    if (winner == "M" && (column.Name == "MERON" || (column.Name == "WINNERS_EARN" && Convert.ToDecimal(row.Cells[column.Name].Value) > 0) || (column.Name == "RATE_EARNINGS" && Convert.ToDecimal(row.Cells[column.Name].Value) > 0)))
                    {
                        // Set the color for MERON, WINNERS_EARN, and RATE_EARNINGS when winner is "M" and the value is greater than 0
                        row.Cells[column.Name].Style.BackColor = Color.FromArgb(239, 253, 244);
                    }
                    else if (winner == "W" && (column.Name == "WALA" || (column.Name == "WINNERS_EARN" && Convert.ToDecimal(row.Cells[column.Name].Value) > 0) || (column.Name == "RATE_EARNINGS" && Convert.ToDecimal(row.Cells[column.Name].Value) > 0)))
                    {
                        // Set the color for WALA, WINNERS_EARN, and RATE_EARNINGS when winner is "W" and the value is greater than 0
                        row.Cells[column.Name].Style.BackColor = Color.FromArgb(255, 243, 245);
                    }
                }
            }



        }

        //Cell column border of GridPlasada datagrid
        private void GridPlasada_Entries_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].HeaderText == "INITIAL BET DIFF")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(236, 237, 240); // Change this to your desired color

                // Draw the divider line
                using (Pen dividerPen = new Pen(dividerColor, 3)) // Set the width of the divider
                {
                    // Calculate the position of the divider line
                    int x = e.CellBounds.Right - 1;
                    int y1 = e.CellBounds.Top;
                    int y2 = e.CellBounds.Bottom;

                    e.Graphics.DrawLine(dividerPen, x, y1, x, y2);
                }

                e.Handled = true;
            }

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].HeaderText == "BET (W)")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(236, 237, 240); // Change this to your desired color

                // Draw the divider line
                using (Pen dividerPen = new Pen(dividerColor, 3)) // Set the width of the divider
                {
                    // Calculate the position of the divider line
                    int x = e.CellBounds.Right - 1;
                    int y1 = e.CellBounds.Top;
                    int y2 = e.CellBounds.Bottom;

                    e.Graphics.DrawLine(dividerPen, x, y1, x, y2);
                }

                e.Handled = true;
            }

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].HeaderText == "PAREHAS")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(236, 237, 240); // Change this to your desired color

                // Draw the divider line
                using (Pen dividerPen = new Pen(dividerColor, 3)) // Set the width of the divider
                {
                    // Calculate the position of the divider line
                    int x = e.CellBounds.Right - 1;
                    int y1 = e.CellBounds.Top;
                    int y2 = e.CellBounds.Bottom;

                    e.Graphics.DrawLine(dividerPen, x, y1, x, y2);
                }

                e.Handled = true;
            }

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Entries.Columns[e.ColumnIndex].HeaderText == "RATE EARNINGS")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(236, 237, 240); // Change this to your desired color

                // Draw the divider line
                using (Pen dividerPen = new Pen(dividerColor, 3)) // Set the width of the divider
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

    }
}
