using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FightingFeather
{
    public partial class UserControl_Earnings : UserControl
    {
        public UserControl_Earnings()
        {
            InitializeComponent();

            // Subscribe to the CellFormatting event
            GridPlasada_Earnings.CellFormatting += GridPlasada_Earnings_CellFormatting;
            GridPlasada_Earnings.CellPainting += GridPlasada_Earnings_CellPainting;

            LoadJsonData();

            foreach (DataGridViewRow row in GridPlasada_Earnings.Rows)
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

                    // Assuming jsonArray contains an array of objects
                    foreach (JObject obj in jsonArray)
                    {
                        // Create a new row
                        DataGridViewRow row = new DataGridViewRow();

                        // Add cells based on the columns you want to display
                        DataGridViewTextBoxCell cell1 = new DataGridViewTextBoxCell();
                        cell1.Value = obj["FIGHT"]; // Replace "ColumnName1" with the actual name
                        row.Cells.Add(cell1);

                        DataGridViewTextBoxCell cell2 = new DataGridViewTextBoxCell();
                        // Determine the value for column MERON or WALA based on the value of column WINNER
                        switch (obj["WINNER"].ToString())
                        {
                            case "M":
                                cell2.Value = obj["MERON"];
                                cell2.Style.BackColor = Color.FromArgb(239, 253, 244);
                                break;
                            case "W":
                                cell2.Value = obj["WALA"];
                                cell2.Style.BackColor = Color.FromArgb(255, 243, 245);
                                break;
                            default:
                                cell2.Value = ""; // Handle other cases if needed
                                break;
                        }
                        row.Cells.Add(cell2);

                        DataGridViewTextBoxCell cell3 = new DataGridViewTextBoxCell();
                        // Determine the value for column BET based on the value of column WINNER
                        switch (obj["WINNER"].ToString())
                        {
                            case "M":
                                if (obj["BET (M)"] != null)
                                {
                                    cell3.Value = obj["BET (M)"];
                                }
                                else
                                {
                                    // Handle case where "BET (M)" key is missing or null
                                    cell3.Value = 0; // or any default value you want to set
                                }
                                break;
                            case "W":
                                if (obj["BET (W)"] != null)
                                {
                                    cell3.Value = obj["BET (W)"];
                                }
                                else
                                {
                                    // Handle case where "BET (W)" key is missing or null
                                    cell3.Value = 0; // or any default value you want to set
                                }
                                break;
                            case "Cancel":
                            case "None":
                                cell3.Value = 0; // or any default value you want to set
                                break;
                            default:
                                cell3.Value = ""; // Handle other cases if needed
                                break;
                        }
                        row.Cells.Add(cell3);

                        DataGridViewTextBoxCell cell4 = new DataGridViewTextBoxCell();
                        cell4.Value = obj["PAREHAS"]; 
                        row.Cells.Add(cell4);

                        DataGridViewTextBoxCell cell5 = new DataGridViewTextBoxCell();
                        cell5.Value = obj["LOGRO"]; 
                        row.Cells.Add(cell5);

                        DataGridViewTextBoxCell cell6 = new DataGridViewTextBoxCell();
                        cell6.Value = obj["FEE"];
                        row.Cells.Add(cell6);

                        DataGridViewTextBoxCell cell7 = new DataGridViewTextBoxCell();
                        cell7.Value = obj["TOTAL PLASADA"];
                        row.Cells.Add(cell7);

                        DataGridViewTextBoxCell cell8 = new DataGridViewTextBoxCell();
                        cell8.Value = obj["RATE EARNINGS"];
                        row.Cells.Add(cell8);

                        DataGridViewTextBoxCell cell9 = new DataGridViewTextBoxCell();
                        cell9.Value = obj["WINNERS EARNING"];
                        row.Cells.Add(cell9);


                        // Add the row to the DataGridView
                        GridPlasada_Earnings.Rows.Add(row);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error loading JSON data: " + ex.Message + "\nStack Trace: " + ex.StackTrace);
                }

            }
            else
            {
               
            }
        }






        private void GridPlasada_Earnings_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null && e.Value.ToString() == "0")
            {
                e.Value = "-";
            }

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

            // Check if the cell belongs to the "PAREHAS" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_Earnings.Columns[e.ColumnIndex].Name == "PAREHAS" && e.RowIndex >= 0)
            {
                // Set the font color for cells in the "PAREHASF" column
                e.CellStyle.ForeColor = Color.FromArgb(99, 66, 0);
            }

            // Change the foreground color for the "RATE EARNINGS" column
            if (e.ColumnIndex >= 0 && GridPlasada_Earnings.Columns[e.ColumnIndex].Name == "RATE_EARNINGS" && e.RowIndex >= 0)
            {
                // Set the foreground color for "RATE EARNINGS" cells
                e.CellStyle.ForeColor = Color.FromArgb(153, 105, 28);
            }

            if (e.ColumnIndex >= 0 && GridPlasada_Earnings.Columns[e.ColumnIndex].Name == "WINNERS_EARN" && e.RowIndex >= 0)
            {
                // Set the foreground color for "WINNERS EARNING" cells
                e.CellStyle.ForeColor = Color.Maroon;
            }

        }


        private void GridPlasada_Earnings_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Earnings.Columns[e.ColumnIndex].HeaderText == "RATE EARNINGS")
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
    }
}
