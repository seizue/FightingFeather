using Newtonsoft.Json.Linq;
using System;
using System.Drawing;
using System.IO;
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
           
        }

        public void ReloadData()
        {
            GridPlasada_Inspection.Rows.Clear(); // Clear existing rows
            LoadJsonData(); // Reload data
       
            foreach (DataGridViewRow row in GridPlasada_Inspection.Rows) // Change back the custom cell height
            {
                row.Height = 28;
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
            string fightValue = currentRow.Cells["FIGHT"].Value?.ToString();
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
        }
    }
    


}


