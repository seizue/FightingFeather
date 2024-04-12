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
using System.Drawing.Printing;

namespace FightingFeather
{
    public partial class UserControl_Earnings : UserControl
    {
        private PrintDocument pd = new PrintDocument();
        private PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
        public UserControl_Earnings()
        {
            InitializeComponent();

            ReloadData();

            // Subscribe to the CellFormatting event
            GridPlasada_Earnings.CellFormatting += GridPlasada_Earnings_CellFormatting;
            GridPlasada_Earnings.CellPainting += GridPlasada_Earnings_CellPainting;
            GridPlasada_Earnings.SelectionChanged += GridPlasada_Earnings_SelectionChanged;

         

            foreach (DataGridViewRow row in GridPlasada_Earnings.Rows)
            {
                row.Height = 28;
            }

        }

        public void ReloadData()
        {
            GridPlasada_Earnings.Rows.Clear(); // Clear existing rows

            LoadJsonData(); // Reload data

            foreach (DataGridViewRow row in GridPlasada_Earnings.Rows) // Change back the custom cell height
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
                                    cell3.Value = obj["PAREHAS"];
                                   
                                }
                                else
                                {
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
                        cell4.Value = obj["LOGRO"]; 
                        row.Cells.Add(cell4);

                        DataGridViewTextBoxCell cell5 = new DataGridViewTextBoxCell();
                        cell5.Value = obj["FEE"];
                        row.Cells.Add(cell5);

                        DataGridViewTextBoxCell cell6 = new DataGridViewTextBoxCell();
                        cell6.Value = obj["TOTAL PLASADA"];
                        row.Cells.Add(cell6);

                        DataGridViewTextBoxCell cell7 = new DataGridViewTextBoxCell();
                        object rateValue = obj["RATE"];

                        if (rateValue == null || string.IsNullOrWhiteSpace(rateValue.ToString()))
                        {
                            cell7.Value = "-";
                        }
                        else
                        {
                            cell7.Value = rateValue;
                        }

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
          
        }


        private void GridPlasada_Earnings_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the cell value is 0, 0.00, empty, or null
            if (e.Value == null || e.Value.ToString() == "0" || e.Value.ToString() == "0.00" || string.IsNullOrEmpty(e.Value.ToString()))
            {
                // Replace the value with "-"
                e.Value = "-";
                e.FormattingApplied = true; // Set FormattingApplied to true to indicate that the formatting has been applied
            }

            // Check if the cell belongs to the "RATE" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_Earnings.Columns[e.ColumnIndex].Name == "RATE" && e.RowIndex >= 0)
            {
                // Get the value of the cell
                string rate = GridPlasada_Earnings.Rows[e.RowIndex].Cells["RATE"].Value?.ToString();

                // Check if the rate is "None"
                if (rate == "None")
                {
                    // Set the cell value to "-"
                    e.Value = "-";
                    e.FormattingApplied = true; // Indicate that the formatting is applied
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


        private void GridPlasada_Earnings_SelectionChanged(object sender, EventArgs e)
        {
            // Ensure that at least one row is selected
            if (GridPlasada_Earnings.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = GridPlasada_Earnings.SelectedRows[0];

                // Populate the textBox_FIGHT with the value from the "FIGHT" column
                textBox_FIGHT.Text = selectedRow.Cells["FIGHT"].Value.ToString();

                // Populate the textBox_NAME with the value from the "WINNER" column
                textBox_NAME.Text = selectedRow.Cells["WINNER"].Value.ToString();

                // Populate the textBox_BETPA with the value from the "BET" column
                textBox_BETPA.Text = selectedRow.Cells["BET"].Value.ToString();

                // Populate the textBox_FEE with the value from the "FEE" column
                textBox_FEE.Text = selectedRow.Cells["FEE"].Value.ToString();

                // Populate the textBox_CITYTAX with a constant value of 300
                textBox_CITYTAX.Text = "300";

                // Populate the label_RATE with the value from the "RATE" column 
                label_RATE.Text = selectedRow.Cells["RATE"].Value.ToString();

                // Populate the textBox_RATE_EARN with the value from the "RATE_EARNINGS" column
                object rateEarningsValue = selectedRow.Cells["RATE_EARNINGS"].Value;
                if (rateEarningsValue != null)
                {
                    string rateEarningsText = rateEarningsValue.ToString();
                    if (rateEarningsText != "0")
                    {
                        textBox_RATE_EARN.Text = rateEarningsText;
                    }
                    else
                    {
                        textBox_RATE_EARN.Text = "-";
                    }
                    // Now, we store the actual value without any conversion or representation
                    if (decimal.TryParse(rateEarningsText, out decimal rateEarnValue))
                    {
                        // Continue with calculations using rateEarnValue
                        // Parse bet and fee from the selected row cells
                        if (decimal.TryParse(selectedRow.Cells["BET"].Value?.ToString(), out decimal bet) &&
                            decimal.TryParse(selectedRow.Cells["FEE"].Value?.ToString(), out decimal fee))
                        {
                            // Calculate subtotal1 by subtracting the fee from the bet
                            decimal subtotal1 = bet - fee;
                            textBox_SUBTOTAL1.Text = subtotal1.ToString();

                            // Calculate subtotal2 by subtracting 300 from subtotal1
                            decimal defaultTax = 300;
                            decimal subtotal2 = subtotal1 - defaultTax;
                            textBox_SUBTOTAL2.Text = subtotal2.ToString();

                            // Calculate subtotal3 by adding rateEarnValue to it
                            decimal total = subtotal2 + rateEarnValue;
                            textBox_TOTAL.Text = total.ToString();
                        }
                        else
                        {
                            textBox_TOTAL.Text = "-"; // Or any appropriate default value
                        }
                    }
                    else
                    {
                        textBox_TOTAL.Text = "-"; // Or any appropriate default value
                    }
                }
                else
                {
                    textBox_TOTAL.Text = "-"; // Or any appropriate default value
                }

            }

        }

        private void metroTile_PrintReceipt_Click(object sender, EventArgs e)
        {

            PrintPreview();
        }

        private void PrintPreview()
        {
            pd.PrintPage += new PrintPageEventHandler(PrintPage);
            pd.QueryPageSettings += new QueryPageSettingsEventHandler(QueryPageSettings);

            printPreviewDialog.Document = pd;
            printPreviewDialog.ShowDialog();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {
            int paperWidth = e.PageSettings.PaperSize.Width;
            int paperHeight = e.PageSettings.PaperSize.Height;
            int panelWidth = Panel_PrintReceipt.Width;
            int panelHeight = Panel_PrintReceipt.Height;

            int centerX = (paperWidth - panelWidth) / 2;
            int centerY = (paperHeight - panelHeight) / 2;

            // Draw the panel content at the calculated center position
            using (Bitmap bmp = new Bitmap(panelWidth, panelHeight))
            {
                Panel_PrintReceipt.DrawToBitmap(bmp, new Rectangle(0, 0, panelWidth, panelHeight));
                e.Graphics.DrawImage(bmp, centerX, centerY);
            }
        }

        private void QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            // Adjust the paper size here
            e.PageSettings.PaperSize = new PaperSize("CustomSize", 400, 395); 
        }
    }

}

