using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Data.Common;
using Newtonsoft.Json;

namespace FightingFeather
{
    public partial class ViewMuntonForm :MetroFramework.Forms.MetroForm
    {
        private string fileName;

        private Color defaultColor = Color.FromArgb(0, 0, 42); // Default color 
        private Color clickedColor = Color.FromArgb(193, 84, 55); // Color when the button is clicked

        public ViewMuntonForm(string fileName)
        {
            InitializeComponent();

            this.fileName = fileName;
            Reload();

            // Populate the ComboBox with options
            comboBox_Filter.Items.AddRange(new object[] { "PLASADA", "SUMMARY", });
        
        }

        private void Reload()
        {
            LoadMuntonData();

            foreach (DataGridViewRow row in DataMuntonGrid.Rows)
            {
                row.Height = 28;
            }
        }

        // load the backup Munton as json a file
        private void LoadMuntonData()
        {
            try
            {
                // Load JSON file based on the filename
                string jsonFilePath = Path.Combine(Environment.CurrentDirectory, "TABLES", fileName + ".json");

                if (File.Exists(jsonFilePath))
                {
                    string jsonContent = File.ReadAllText(jsonFilePath);

                    // Deserialize JSON string into a list of dictionaries
                    List<Dictionary<string, string>> dataList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonContent);

                    // Clear existing rows and columns in the DataGridView
                    DataMuntonGrid.Rows.Clear();
                    DataMuntonGrid.Columns.Clear();

                    // Add columns to the DataGridView for the fight details
                    DataMuntonGrid.Columns.Add("FIGHT", "FIGHT");
                    DataMuntonGrid.Columns.Add("MERON", "MERON");
                    DataMuntonGrid.Columns.Add("WALA", "WALA");
                    DataMuntonGrid.Columns.Add("BET (M)", "BET (M)");
                    DataMuntonGrid.Columns.Add("BET (W)", "BET (W)");
                    DataMuntonGrid.Columns.Add("INITIAL BET DIFF", "INITIAL BET DIFF");
                    DataMuntonGrid.Columns.Add("PAREHAS", "PAREHAS");
                    DataMuntonGrid.Columns.Add("PAGO", "PAGO");
                    DataMuntonGrid.Columns.Add("WINNER", "WINNER");
                    DataMuntonGrid.Columns.Add("RATE AMOUNT", "RATE AMOUNT");
                    DataMuntonGrid.Columns.Add("RATE", "RATE");
                    DataMuntonGrid.Columns.Add("LOGRO", "LOGRO");
                    DataMuntonGrid.Columns.Add("FEE", "FEE");
                    DataMuntonGrid.Columns.Add("PLASADA", "PLASADA");
                    DataMuntonGrid.Columns.Add("RATE EARNINGS", "RATE EARNINGS");
                    DataMuntonGrid.Columns.Add("WINNERS EARNING", "WINNERS EARNING");

                    // Add rows to the DataGridView for each fight detail
                    foreach (Dictionary<string, string> data in dataList)
                    {
                        // Skip the summary data
                        if (!data.ContainsKey("FIGHT"))
                            continue;

                        DataGridViewRow row = new DataGridViewRow();
                        row.CreateCells(DataMuntonGrid);

                        // Set cell values for each column
                        row.Cells[0].Value = data["FIGHT"];
                        row.Cells[1].Value = data["MERON"];
                        row.Cells[2].Value = data["WALA"];
                        row.Cells[3].Value = data["BET (M)"];
                        row.Cells[4].Value = data["BET (W)"];
                        row.Cells[5].Value = data["INITIAL BET DIFF"];
                        row.Cells[6].Value = data["PAREHAS"];
                        row.Cells[7].Value = data["PAGO"];
                        row.Cells[8].Value = data["WINNER"];
                        row.Cells[9].Value = data["RATE AMOUNT"];
                        row.Cells[10].Value = data["RATE"];
                        row.Cells[11].Value = data["LOGRO"];
                        row.Cells[12].Value = data["FEE"];
                        row.Cells[13].Value = data["PLASADA"];
                        row.Cells[14].Value = data["RATE EARNINGS"];
                        row.Cells[15].Value = data["WINNERS EARNING"];

                        DataMuntonGrid.Rows.Add(row);
                    }
                }
                else
                {
                    MessageBox.Show("Munton data file not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close(); // Close the form if file not found
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Munton data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Handle the error as needed
            }
        }


        private void textBox_Search_ButtonClick(object sender, EventArgs e)
        {
            string searchText = textBox_Search.Text.Trim();

            // If search text is empty, show all rows and do nothing
            if (string.IsNullOrEmpty(searchText))
            {
                ShowAllRows();
                return;
            }

            // Convert search text to lowercase for case-insensitive search
            string searchTextLower = searchText.ToLower();

            // Loop through the rows in the DataGridView
            foreach (DataGridViewRow row in DataMuntonGrid.Rows)
            {
                // Skip new rows in edit mode (uncommitted rows)
                if (row.IsNewRow)
                    continue;

                bool rowVisible = false;

                // Loop through the cells in the current row
                foreach (DataGridViewCell cell in row.Cells)
                {
                    // Check if the cell value contains the search text (case-insensitive)
                    if (cell.Value != null && cell.Value.ToString().ToLower().Contains(searchTextLower))
                    {
                        rowVisible = true; // Set the row visibility to true if the search text is found in any cell
                        break;
                    }
                }

                // Set the row visibility based on the search result
                row.Visible = rowVisible;
            }
        }

        private void ShowAllRows()
        {
            foreach (DataGridViewRow row in DataMuntonGrid.Rows)
            {
                // Skip new rows in edit mode (uncommitted rows)
                if (!row.IsNewRow)
                    row.Visible = true;
            }
        }

        private void textBox_Search_ClearClicked()
        {
            textBox_Search.Text = ""; // Clear the search text

            // Show all rows
            ShowAllRows();
        }

        private void button_Earnings_Click(object sender, EventArgs e)
        {
            button_Earnings.ForeColor = clickedColor;
            button_Home.ForeColor = defaultColor;
            button_CashBreakDown.ForeColor = defaultColor;

            // First, ensure all columns are hidden
            foreach (DataGridViewColumn column in DataMuntonGrid.Columns)
            {
                column.Visible = false;
            }

            // Then, show the columns you want to display in the specified order
            string[] columnsToShow = { "FIGHT", "WINNER", "BET", "LOGRO", "FEE", "TOTAL PLASADA", "RATE", "RATE EARNINGS", "WINNERS EARNING" };

            foreach (string columnName in columnsToShow)
            {
                if (DataMuntonGrid.Columns.Contains(columnName))
                {
                    DataMuntonGrid.Columns[columnName].Visible = true;
                }
            }

            // Check if the "BET" column exists; if not, add it
            string betColumnName = "BET";
            if (!DataMuntonGrid.Columns.Contains(betColumnName))
            {
                DataGridViewTextBoxColumn betColumn = new DataGridViewTextBoxColumn();
                betColumn.Name = betColumnName;
                DataMuntonGrid.Columns.Add(betColumn);
            }

            // Set the display index of the "BET" column next to the "WINNER" column
            if (DataMuntonGrid.Columns.Contains("WINNER") && DataMuntonGrid.Columns.Contains("BET"))
            {
                DataMuntonGrid.Columns["BET"].DisplayIndex = DataMuntonGrid.Columns["WINNER"].DisplayIndex + 1;
            }

            // Now, handle conditional formatting for the "WINNER" column and populate the "BET" column based on "WINNER" value
            foreach (DataGridViewRow row in DataMuntonGrid.Rows)
            {
                if (row.Cells["WINNER"].Value != null)
                {
                    string winnerValue = row.Cells["WINNER"].Value.ToString();
                    string correspondingColumnWinner = winnerValue == "M" ? "MERON" : winnerValue == "W" ? "WALA" : "";

                    if (!string.IsNullOrEmpty(correspondingColumnWinner) && DataMuntonGrid.Columns.Contains(correspondingColumnWinner))
                    {
                        object winnerValueObject = row.Cells[correspondingColumnWinner].Value;
                        if (winnerValueObject != null)
                        {
                            row.Cells["WINNER"].Value = winnerValueObject.ToString();
                        }
                    }

                    string correspondingColumnBet = winnerValue == "M" ? "PAREHAS" : winnerValue == "W" ? "BET (W)" : "";
                    if (!string.IsNullOrEmpty(correspondingColumnBet) && DataMuntonGrid.Columns.Contains(correspondingColumnBet))
                    {
                        object betValueObject = row.Cells[correspondingColumnBet].Value;
                        if (betValueObject != null)
                        {
                            row.Cells["BET"].Value = betValueObject.ToString();
                        }
                    }

                    // Set background color based on the winner value for the "WINNER" column
                    if (winnerValue == "M")
                    {
                        row.Cells["WINNER"].Style.BackColor = Color.FromArgb(239, 253, 244);
                    }
                    else if (winnerValue == "W")
                    {
                        row.Cells["WINNER"].Style.BackColor = Color.FromArgb(255, 243, 245);
                    }
                }
            }

        }


        private void button_CashBreakDown_Click(object sender, EventArgs e)
        {
            button_CashBreakDown.ForeColor = clickedColor;
            button_Home.ForeColor = defaultColor;
            button_Earnings.ForeColor = defaultColor;

            // First, ensure all columns are hidden
            foreach (DataGridViewColumn column in DataMuntonGrid.Columns)
            {
                column.Visible = false;
            }

            // Then, show the columns you want to display in the specified order
            string[] columnsToShow = { "FIGHT", "WINNER", "PAREHAS", "RATE", "RATE EARNINGS", "TOTAL PLASADA", "WINNERS EARNING" };

            // Set the display index of PAREHAS next to WINNER
            if (DataMuntonGrid.Columns.Contains("WINNER") && DataMuntonGrid.Columns.Contains("PAREHAS"))
            {
                int winnerDisplayIndex = DataMuntonGrid.Columns["WINNER"].DisplayIndex;
                DataMuntonGrid.Columns["PAREHAS"].DisplayIndex = winnerDisplayIndex + 1;
            }

            foreach (string columnName in columnsToShow)
            {
                if (DataMuntonGrid.Columns.Contains(columnName))
                {
                    DataMuntonGrid.Columns[columnName].Visible = true;
                }
            }
        }


        private void button_Home_Click(object sender, EventArgs e)
        {
            Reload();
            button_Home.ForeColor = clickedColor;
            button_Earnings.ForeColor = defaultColor;
            button_CashBreakDown.ForeColor = defaultColor;
        }


        private void button_ExportMunton_Click(object sender, EventArgs e)
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
                        foreach (DataGridViewColumn column in DataMuntonGrid.Columns)
                        {
                            if (column.Visible)
                            {
                                sw.Write("\"" + column.HeaderText + "\",");
                            }
                        }
                        sw.WriteLine();

                        // Write each row of data to the file
                        foreach (DataGridViewRow row in DataMuntonGrid.Rows)
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


        private void DataMuntonGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the cell value is 0, 0.00, or empty
            if (e.Value != null && (e.Value.ToString() == "0" || e.Value.ToString() == "0.00" || string.IsNullOrEmpty(e.Value.ToString())))
            {
                // Replace the value with "-"
                e.Value = "-";
                e.FormattingApplied = true; // Set FormattingApplied to true to indicate that the formatting has been applied
            }

            // Check if the cell value is a string and not empty
            if (e.Value != null && e.Value.GetType() == typeof(string) && !string.IsNullOrEmpty(e.Value.ToString()))
            {
                // Capitalize the first letter of the cell value
                e.Value = char.ToUpper(e.Value.ToString()[0]) + e.Value.ToString().Substring(1);
                e.FormattingApplied = true; // Set FormattingApplied to true to indicate that the formatting has been applied
            }


            // Set text color for "INITIAL BET DIFF" column
            if (DataMuntonGrid.Columns[e.ColumnIndex].Name == "INITIAL BET DIFF")
            {
                e.CellStyle.ForeColor = Color.FromArgb(120, 30, 199);
              
            }

            // Set text color for "PAREHAS" column
            if (DataMuntonGrid.Columns[e.ColumnIndex].Name == "PAREHAS")
            {
                e.CellStyle.ForeColor = Color.FromArgb(99, 66, 0);
              
            }

            // Set text color for "RATE EARNINGS" column
            if (DataMuntonGrid.Columns[e.ColumnIndex].Name == "RATE EARNINGS")
            {
                e.CellStyle.ForeColor = Color.FromArgb(99, 66, 0);
               
            }

            // Check if the cell is in the "WINNER" column
            if (DataMuntonGrid.Columns[e.ColumnIndex].Name == "WINNERS EARNING")
            {
                // Set the text color to maroon
                e.CellStyle.ForeColor = Color.Maroon;
            }

            // Check if the column index is valid and the current cell being formatted is in the WINNER column
            if (e.ColumnIndex >= 0 && DataMuntonGrid.Columns[e.ColumnIndex].Name == "WINNER" && e.RowIndex >= 0)
            {
                // Get the value of the cell and ensure it's not null
                if (DataMuntonGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    string winner = DataMuntonGrid.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

                    // Set custom text color and alignment based on the value of the cell
                    if (winner == "W" || winner == "M" || winner == "Cancel" || winner == "Draw")
                    {
                        // Set the text color based on the winner value
                        if (winner == "W")
                        {
                            e.CellStyle.ForeColor = Color.Maroon; // Change this to your desired color
                            e.CellStyle.BackColor = Color.FromArgb(255, 243, 245);
                        }
                        else if (winner == "M")
                        {
                            e.CellStyle.ForeColor = Color.Green; // Change this to your desired color
                            e.CellStyle.BackColor = Color.FromArgb(239, 253, 244);
                        }
                        else if (winner == "Cancel" || winner == "Draw")
                        {
                            e.CellStyle.ForeColor = Color.Gray; // Change this to your desired color
                        }

                        // Center-align the content of the cell
                        e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    }
                }
            }

            // Check if the current cell being formatted is in the RATE AMOUNT column
            if (e.ColumnIndex >= 0 && DataMuntonGrid.Columns[e.ColumnIndex].Name == "RATE AMOUNT" && e.RowIndex >= 0)
            {
                // Center-align the content of the cell
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                DataGridViewColumn column = DataMuntonGrid.Columns[e.ColumnIndex];
                DataGridViewRow row = DataMuntonGrid.Rows[e.RowIndex];

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


            if (e.ColumnIndex >= 0 && DataMuntonGrid.Columns[e.ColumnIndex].Name == "WINNERS EARNING")
            {
                // Center-align the content of the cell
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

                // Check if the current cell is the header cell
                if (e.RowIndex == -1)
                {
                    // Center-align the header text
                    DataMuntonGrid.Columns[e.ColumnIndex].HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void DataMuntonGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && DataMuntonGrid.Columns[e.ColumnIndex].HeaderText == "RATE EARNINGS")
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

            // Check if the current cell being painted is the header cell of the "WINNERS EARNING" column
            if (e.ColumnIndex >= 0 && DataMuntonGrid.Columns[e.ColumnIndex].Name == "WINNERS EARNING" && e.RowIndex == -1)
            {
                // Center-align the header text
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            // Check if the current cell being painted is the header cell of the "WINNERS EARNING" column
            if (e.ColumnIndex >= 0 && DataMuntonGrid.Columns[e.ColumnIndex].Name == "WINNER" && e.RowIndex == -1)
            {
                // Center-align the header text
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (e.ColumnIndex >= 0 && DataMuntonGrid.Columns[e.ColumnIndex].Name == "RATE AMOUNT" && e.RowIndex == -1)
            {
                // Center-align the header text
                e.CellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

        }
    }
}

