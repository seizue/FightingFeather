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

namespace FightingFeather
{
    public partial class ViewMuntonForm :MetroFramework.Forms.MetroForm
    {
        private string fileName;
        public ViewMuntonForm(string fileName)
        {
            InitializeComponent();

            this.fileName = fileName;
            LoadMuntonData();

            // Populate the ComboBox with options
            comboBox_Filter.Items.AddRange(new object[] { "PLASADA", "SUMMARY", });
        }

        private void LoadMuntonData()
        {
            try
            {
                // Load JSON file based on the filename
                string jsonFilePath = Path.Combine(Environment.CurrentDirectory, "TABLES", fileName);

                if (File.Exists(jsonFilePath))
                {
                    string jsonContent = File.ReadAllText(jsonFilePath);

                    // Deserialize JSON string into a list of dictionaries
                    List<Dictionary<string, string>> dataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonContent);

                    // Clear existing rows and columns in the DataGridView
                    DataMuntonGrid.Rows.Clear();
                    DataMuntonGrid.Columns.Clear();

                    // Add columns to the DataGridView based on the keys of the first dictionary
                    if (dataList.Count > 0)
                    {
                        foreach (string key in dataList[0].Keys)
                        {
                            // Add all columns except the "Date" column
                            if (key != "Date")
                            {
                                DataMuntonGrid.Columns.Add(key, key);
                            }
                        }

                        // Add rows to the DataGridView
                        foreach (Dictionary<string, string> data in dataList)
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(DataMuntonGrid);

                            // Iterate over the DataGridView columns and set cell values based on the JSON data
                            foreach (DataGridViewColumn column in DataMuntonGrid.Columns)
                            {
                                if (column.Name != "Date")
                                {
                                    if (data.ContainsKey(column.Name))
                                    {
                                        row.Cells[column.Index].Value = data[column.Name];
                                    }
                                    else
                                    {
                                        // Handle missing data here if necessary
                                        row.Cells[column.Index].Value = string.Empty;
                                    }
                                }
                            }

                            DataMuntonGrid.Rows.Add(row);
                        }
                    }

                    else
                    {
                        MessageBox.Show("No data found in the JSON file.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        private void button_Earnings_Click(object sender, EventArgs e)
        {

        }

        private void button_CashBreakDown_Click(object sender, EventArgs e)
        {

        }
    }
}

