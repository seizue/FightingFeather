using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FightingFeather
{
    public partial class UserControl_CashBreakDown : UserControl
    {
        public UserControl_CashBreakDown()
        {
            InitializeComponent();

            LoadJsonData();

            GridPlasada_CashBreakDown.CellFormatting += GridPlasada_CashBreakDown_CellFormatting;

            foreach (DataGridViewRow row in GridPlasada_CashBreakDown.Rows)
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
                        cell2.Value = obj["WINNER"];
                        row.Cells.Add(cell2);

                        DataGridViewTextBoxCell cell3 = new DataGridViewTextBoxCell();
                        cell3.Value = obj["PAREHAS"];
                        row.Cells.Add(cell3);

                        DataGridViewTextBoxCell cell5 = new DataGridViewTextBoxCell();
                        cell5.Value = obj["RATE"];
                        row.Cells.Add(cell5);

                        DataGridViewTextBoxCell cell6 = new DataGridViewTextBoxCell();
                        cell6.Value = obj["RATE EARNINGS"];
                        row.Cells.Add(cell6);

                        DataGridViewTextBoxCell cell7 = new DataGridViewTextBoxCell();
                        cell7.Value = obj["TOTAL PLASADA"];
                        row.Cells.Add(cell7);

                        DataGridViewTextBoxCell cell8 = new DataGridViewTextBoxCell();
                        cell8.Value = obj["WINNERS EARNING"];
                        row.Cells.Add(cell8);


                        // Add the row to the DataGridView
                        GridPlasada_CashBreakDown.Rows.Add(row);


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


        private void GridPlasada_CashBreakDown_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null && e.Value.ToString() == "0")
            {
                e.Value = "-";
            }

            // Check if the cell belongs to the "PAREHAS" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_CashBreakDown.Columns[e.ColumnIndex].Name == "PARADA" && e.RowIndex >= 0)
            {
                // Set the font color for cells in the "PAREHASF" column
                e.CellStyle.ForeColor = Color.FromArgb(99, 66, 0);
            }

            // Change the foreground color for the "RATE EARNINGS" column
            if (e.ColumnIndex >= 0 && GridPlasada_CashBreakDown.Columns[e.ColumnIndex].Name == "RATE_EARNINGS" && e.RowIndex >= 0)
            {
                // Set the foreground color for "RATE EARNINGS" cells
                e.CellStyle.ForeColor = Color.FromArgb(153, 105, 28);
            }

            if (e.ColumnIndex >= 0 && GridPlasada_CashBreakDown.Columns[e.ColumnIndex].Name == "TOTAL" && e.RowIndex >= 0)
            {
                // Set the foreground color for "WINNERS EARNING" cells
                e.CellStyle.ForeColor = Color.Maroon;
            }



            // Check if the column index is valid and the current cell being formatted is in the WINNER column
            if (e.ColumnIndex >= 0 && GridPlasada_CashBreakDown.Columns[e.ColumnIndex].Name == "WINNER" && e.RowIndex >= 0)
            {
                // Get the value of the cell and ensure it's not null
                if (GridPlasada_CashBreakDown.Rows[e.RowIndex].Cells[e.ColumnIndex].Value != null)
                {
                    string winner = GridPlasada_CashBreakDown.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString();

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
                        e.CellStyle.ForeColor = GridPlasada_CashBreakDown.DefaultCellStyle.ForeColor;
                    }
                }
            }

        }


    }
}
