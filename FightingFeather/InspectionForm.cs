using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace FightingFeather
{
    public partial class InspectionForm : MetroFramework.Forms.MetroForm
    {
        public InspectionForm()
        {
            InitializeComponent();

            ReloadData();

            // Populate the ComboBox with options
            comboBox_Filter.Items.AddRange(new object[] { "Default", "Show Duplicate Rows", "Show Duplicate Names"});

            // Set the default filter
            comboBox_Filter.SelectedIndex = 0;
        }

        public void ReloadData()
        {
            GridPlasada_Inspection.Rows.Clear(); // Clear existing rows
            LoadJsonData(); // Reload data
       
            foreach (DataGridViewRow row in GridPlasada_Inspection.Rows) // Change back the custom cell height
            {
                row.Height = 25;
            }       

        }

        private void LoadJsonData()
        {
            string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JSON", "receipt.json");
            Console.WriteLine("Attempting to load JSON file from path: " + jsonFilePath);

            if (File.Exists(jsonFilePath))
            {
                string jsonText = File.ReadAllText(jsonFilePath);

                try
                {
                    JArray jsonArray = JArray.Parse(jsonText);


                    foreach (JObject obj in jsonArray)
                    {
                        obj["WALA"] = obj.ContainsKey("WALA") ? char.ToUpper(obj["WALA"].ToString()[0]) + obj["WALA"].ToString().Substring(1) : "";
                        obj["MERON"] = obj.ContainsKey("MERON") ? char.ToUpper(obj["MERON"].ToString()[0]) + obj["MERON"].ToString().Substring(1) : "";

                        // Check if the "WINNER" key exists and has a non-null or non-empty value
                        if (obj.ContainsKey("WINNER") && !string.IsNullOrEmpty(obj["WINNER"].ToString()))
                        {
                            // Skip this row as it already has a value in the "WINNER" column
                            continue;
                        }

                        DataGridViewRow row = new DataGridViewRow();

                        row.Cells.Add(new DataGridViewTextBoxCell { Value = obj["FIGHT"] });
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = obj["MERON"] });
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = obj["WALA"] });
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = obj["BET (M)"] });
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = obj["BET (W)"] });
                        row.Cells.Add(new DataGridViewTextBoxCell { Value = obj["INITIAL BET DIFF"] });

                        GridPlasada_Inspection.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading JSON data: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
                }
            }
        }


        private void GridPlasada_Inspection_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) // Skip header cells
                return;

            DataGridViewRow currentRow = GridPlasada_Inspection.Rows[e.RowIndex];
            string meronValue = currentRow.Cells["MERON"].Value?.ToString();
            string walaValue = currentRow.Cells["WALA"].Value?.ToString();

            bool sameName = false;
            bool exactlySameName = false;

            // Iterate through all rows
            foreach (DataGridViewRow otherRow in GridPlasada_Inspection.Rows)
            {
                if (otherRow.Index != e.RowIndex) // Skip the current row
                {
                    string otherMeronValue = otherRow.Cells["MERON"].Value?.ToString();
                    string otherWalaValue = otherRow.Cells["WALA"].Value?.ToString();

                    // Check if MERON and WALA columns have the same name
                    if ((meronValue == otherMeronValue && meronValue != "") || (walaValue == otherWalaValue && walaValue != ""))
                    {
                        sameName = true;

                        // Check if both MERON and WALA have the same name
                        if (meronValue == otherMeronValue && walaValue == otherWalaValue)
                        {
                            exactlySameName = true;
                            break; // No need to check other rows
                        }
                    }
                }
            }

            // Apply colors based on the conditions
            if (exactlySameName)
            {
                // Set the cell color to RGB(255, 243, 245)
                Console.WriteLine($"Changing cell in row {e.RowIndex}, column FIGHT color to RGB(242, 236, 236).");
                currentRow.Cells["FIGHT"].Style.BackColor = Color.FromArgb(242, 236, 236);
                currentRow.Cells["FIGHT"].Style.ForeColor = Color.FromArgb(149, 32, 37);
            }
            else if (sameName)
            {
                // Set the cell color to RGB(239, 253, 244)
                Console.WriteLine($"Changing cell in row {e.RowIndex}, column FIGHT color to RGB(246, 243, 243).");
                currentRow.Cells["FIGHT"].Style.BackColor = Color.FromArgb(246, 243, 243);
                currentRow.Cells["FIGHT"].Style.ForeColor = Color.FromArgb(26, 47, 47);
            }
        }

        private void InspectionForm_Load(object sender, EventArgs e)
        {
            GridPlasada_Inspection.CellFormatting += GridPlasada_Inspection_CellFormatting;

            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                // Maximize the window without covering the taskbar
                this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void ApplyFilter(string filter)
        {
            // Check for uncommitted new row
            if (GridPlasada_Inspection.Rows.Count > 0 && GridPlasada_Inspection.Rows[GridPlasada_Inspection.Rows.Count - 1].IsNewRow)
            {
                GridPlasada_Inspection.EndEdit();
            }

            GridPlasada_Inspection.SuspendLayout(); // Suspend layout to improve performance

            foreach (DataGridViewRow row in GridPlasada_Inspection.Rows)
            {
                if (!row.IsNewRow) // Skip processing the new row
                {
                    switch (filter)
                    {
                        case "Show Duplicate Rows":
                            if (row.Cells["FIGHT"].Style.BackColor != Color.FromArgb(242, 236, 236))
                            {
                                row.Visible = false;
                            }
                            else
                            {
                                row.Visible = true;
                            }
                            break;
                        case "Show Duplicate Names":
                            if (row.Cells["FIGHT"].Style.BackColor != Color.FromArgb(246, 243, 243))
                            {
                                row.Visible = false;
                            }
                            else
                            {
                                row.Visible = true;
                            }
                            break;
                        default:
                            row.Visible = true; // Show all rows by default
                            break;
                    }
                }
            }

            GridPlasada_Inspection.ResumeLayout(); // Resume layout after filtering
        }

        private void comboBox_Filter_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedFilter = comboBox_Filter.SelectedItem?.ToString();
            ApplyFilter(selectedFilter);
        }

        private void button_Search_Click(object sender, EventArgs e)
        {
            string nameSearchKeyword = textBox_NameSearch.Text.Trim();
            string betSearchKeyword = textBox_BetSearch.Text.Trim();

            GridPlasada_Inspection.SuspendLayout(); // Suspend layout to improve performance

            bool foundMatch = false; // Flag to track if any match is found

            foreach (DataGridViewRow row in GridPlasada_Inspection.Rows)
            {
                if (!row.IsNewRow) // Skip processing the new row
                {
                    bool nameMatch = string.IsNullOrEmpty(nameSearchKeyword) || row.Cells["MERON"].Value.ToString().Contains(nameSearchKeyword) || row.Cells["WALA"].Value.ToString().Contains(nameSearchKeyword);
                    bool betMatch = string.IsNullOrEmpty(betSearchKeyword) || row.Cells["BET_M"].Value.ToString().Contains(betSearchKeyword) || row.Cells["BET_W"].Value.ToString().Contains(betSearchKeyword);

                    row.Visible = nameMatch && betMatch;

                    if (row.Visible)
                    {
                        foundMatch = true; // Set flag if any match is found
                    }
                }
            }

            GridPlasada_Inspection.ResumeLayout(); // Resume layout after filtering

            // Show message box if no match is found
            if (!foundMatch)
            {
                MessageBox.Show("No matching results found.", "Search Results", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }

}


