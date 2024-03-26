using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Configuration;
using System.Text.RegularExpressions;

namespace FightingFeather
{
    public partial class UserControl_Inventory : UserControl
    {
     
        public UserControl_Inventory()
        {
            InitializeComponent();
            
            ReloadData();
        }

        public void ReloadData()
        {
            postedMunton.Rows.Clear(); // Clear existing rows

            if (postedMunton.Columns.Contains("MUNTON"))
            {
                postedMunton.Columns["MUNTON"].Width = 290; // Set the desired width
            }

            foreach (DataGridViewRow row in postedMunton.Rows)
            {
                row.Height = 28;
            }

            LoadJSONFilesIntoDataGrid(); // Reload data
        }

        private void LoadJSONFilesIntoDataGrid()
        {
            try
            {
                string executableFolderPath = Environment.CurrentDirectory;

                // Combine the executable folder path with the "TABLES" folder name
                string tablesFolderPath = Path.Combine(executableFolderPath, "TABLES");

                // Check if the "TABLES" folder exists; if not, create it
                if (!Directory.Exists(tablesFolderPath))
                {
                    Directory.CreateDirectory(tablesFolderPath);
                }

                // Get the list of JSON files in the folder
                string[] jsonFiles = Directory.GetFiles(tablesFolderPath, "*.json");

                // Clear existing rows in postedMunton DataGridView
                postedMunton.Rows.Clear();

                // Iterate through each JSON file
                foreach (string jsonFile in jsonFiles)
                {
                    // Read the JSON file and parse its content
                    string jsonContent = File.ReadAllText(jsonFile);

                    // Deserialize the JSON content as a JArray (JSON array)
                    JArray jsonArray = JArray.Parse(jsonContent);

                    // Add a new row to the DataGridView
                    int rowIndex = postedMunton.Rows.Add();

                    // Specify the "MUNTON" column index or name where you want to display the file name
                    int muntonColumnIndex = postedMunton.Columns["MUNTON"].Index;
                    // Set the file name to the specified column in the newly added row
                    postedMunton.Rows[rowIndex].Cells[muntonColumnIndex].Value = Path.GetFileName(jsonFile);

                    // Set the date to the "DATE" column
                    int dateColumnIndex = postedMunton.Columns["DATE"].Index; // Assuming "DATE" is the column name
                    postedMunton.Rows[rowIndex].Cells[dateColumnIndex].Value = jsonArray.Last()["Date"]; // Assuming the key for the date is "Date"

                    // Set the padding for the header of the "DATE" column
                    postedMunton.Columns[dateColumnIndex].HeaderCell.Style.Padding = new Padding(10, 4, 0, 4);

                    // Set the value in the TOTAL_ENTRY column of the DataGridView
                    int totalEntryColumnIndex = postedMunton.Columns["TOTAL_ENTRY"].Index; // Assuming "TOTAL_ENTRY" is the column name
                    postedMunton.Rows[rowIndex].Cells[totalEntryColumnIndex].Value = jsonArray.Last()["MERON"]; // Set MERON value to TOTAL_ENTRY column

                    int totalEarnings= postedMunton.Columns["TOTAL_EARNING"].Index;
                    postedMunton.Rows[rowIndex].Cells[totalEarnings].Value = jsonArray.Last()["WINNERS EARNING"]; 

                 
                }
            }
            catch (DirectoryNotFoundException)
            {
                MessageBox.Show("The 'TABLES' folder does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error displaying JSON files in DataGrid: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void button_ViewMunton_Click(object sender, EventArgs e)
        {
            // Check if any row is selected
            if (postedMunton.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = postedMunton.SelectedRows[0];

                // Get the filename from the selected row
                string fileName = selectedRow.Cells["MUNTON"].Value.ToString();

                // Pass the filename to the ViewMuntonForm
                using (ViewMuntonForm viewMuntonForm = new ViewMuntonForm(fileName))
                {
                    viewMuntonForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Please select a row to view Munton data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            string searchText = textBox_Search.Text.Trim();

            // If search text is empty, show all rows and do nothing
            if (string.IsNullOrEmpty(searchText))
            {
                ShowAllRows();
                return;
            }

            bool found = false; // Flag to track if any match is found

            // Loop through the rows in the DataGridView
            foreach (DataGridViewRow row in postedMunton.Rows)
            {
                // Skip new rows in edit mode (uncommitted rows)
                if (row.IsNewRow)
                    continue;

                // Get the filename from the MUNTON column
                string fileName = row.Cells["MUNTON"].Value.ToString();

                // Extract the part between "MTN_ID_" and the first underscore before the date
                int startIndex = fileName.IndexOf("MTN_ID_") + "MTN_ID_".Length;
                int endIndex = fileName.IndexOf('_', startIndex);
                string searchablePart = fileName.Substring(startIndex, endIndex - startIndex);

                // Check if the searchable part contains the search text
                if (searchablePart.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    row.Visible = true; // Show the row
                    found = true; // Update the flag
                }
                else
                {
                    row.Visible = false; // Hide the row if no match
                }
            }

            // If no match is found, display a message
            if (!found)
            {
                MessageBox.Show("No matching records found.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // Method to show all rows in the DataGridView
        private void ShowAllRows()
        {
            foreach (DataGridViewRow row in postedMunton.Rows)
            {
                // Skip new rows in edit mode (uncommitted rows)
                if (!row.IsNewRow)
                    row.Visible = true;
            }
        }

        private void raDateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            // Get the selected date from raDateTimePicker1
            DateTime selectedDate = raDateTimePicker1.Value.Date;

            // Loop through the rows in the DataGridView
            foreach (DataGridViewRow row in postedMunton.Rows)
            {
                // Skip new rows in edit mode (uncommitted rows)
                if (row.IsNewRow)
                    continue;

                // Get the date value from the "DATE" column
                DateTime rowDate;
                if (DateTime.TryParse(row.Cells["DATE"].Value.ToString(), out rowDate))
                {
                    // Compare the date value with the selected date
                    if (rowDate.Date == selectedDate)
                    {
                        // Show the row if it matches the selected date
                        row.Visible = true;
                    }
                    else
                    {
                        // Hide the row if it doesn't match the selected date
                        row.Visible = false;
                    }
                }
                else
                {
                    row.Visible = true;
                }
            }
        }

        private void button_ExportMunton_Click(object sender, EventArgs e)
        {
            // Check if there are rows in the DataGridView
            if (postedMunton.Rows.Count == 0)
            {
                MessageBox.Show("There are no records to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Create a SaveFileDialog to specify the CSV file location
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.Title = "Export Munton Data";
            saveFileDialog.FileName = "MuntonData.csv";
            saveFileDialog.RestoreDirectory = true;

            // Show the SaveFileDialog
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Create a StreamWriter to write to the CSV file
                    using (StreamWriter writer = new StreamWriter(saveFileDialog.FileName))
                    {
                        // Write header row
                        for (int i = 0; i < postedMunton.Columns.Count; i++)
                        {
                            writer.Write(postedMunton.Columns[i].HeaderText);
                            if (i < postedMunton.Columns.Count - 1)
                                writer.Write(",");
                        }
                        writer.WriteLine();

                        // Write data rows
                        foreach (DataGridViewRow row in postedMunton.Rows)
                        {
                            // Skip new rows in edit mode (uncommitted rows)
                            if (row.IsNewRow)
                                continue;

                            for (int i = 0; i < postedMunton.Columns.Count; i++)
                            {
                                if (row.Cells[i].Value != null)
                                {
                                    writer.Write(row.Cells[i].Value.ToString());
                                }
                                if (i < postedMunton.Columns.Count - 1)
                                    writer.Write(",");
                            }
                            writer.WriteLine();
                        }
                    }

                    MessageBox.Show("Data exported successfully.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error exporting data: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
