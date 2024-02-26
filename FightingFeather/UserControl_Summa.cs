using Newtonsoft.Json;
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

namespace FightingFeather
{
    public partial class UserControl_Summa : UserControl
    {

        private string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JSON", "summary.json");

        public UserControl_Summa()
        {
            InitializeComponent();
            PopulateAmountColumn();

            GridPlasada_Summary.CellPainting += GridPlasada_Summary_CellPainting;
            GridPlasada_Summary.CellEndEdit += GridPlasada_Summary_CellEndEdit;

            foreach (DataGridViewRow row in GridPlasada_Summary.Rows)
            {
                row.Height = 32;
            }

            LoadDataFromFile();
        }



        private void PopulateAmountColumn()
        {
            // Define the values to be added to the "AMOUNT" column
            int[] amounts = { 1, 5, 10, 20, 50, 100, 200, 500, 1000 };

            // Loop through each amount and add it to the "AMOUNT" column
            foreach (int amount in amounts)
            {
                // Add a new row and set the value for the "AMOUNT" column
                int rowIndex = GridPlasada_Summary.Rows.Add();
                GridPlasada_Summary.Rows[rowIndex].Cells["AMOUNT"].Value = amount;
            }
        }


        private void GridPlasada_Summary_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == GridPlasada_Summary.Columns["QUANTITY"].Index && e.RowIndex >= 0)
            {
                DataGridViewCell quantityCell = GridPlasada_Summary.Rows[e.RowIndex].Cells["QUANTITY"];
                DataGridViewCell amountCell = GridPlasada_Summary.Rows[e.RowIndex].Cells["AMOUNT"];
                DataGridViewCell totalAmountCell = GridPlasada_Summary.Rows[e.RowIndex].Cells["TOTAL_AMOUNT"];

                if (decimal.TryParse(quantityCell.Value?.ToString(), out decimal quantity) && decimal.TryParse(amountCell.Value?.ToString(), out decimal amount))
                {
                    decimal totalAmount = quantity * amount;
                    totalAmountCell.Value = totalAmount.ToString(); // Convert totalAmount to string for display
                    SaveDataToFile();
                }
                else
                {
                    MessageBox.Show("Invalid quantity or amount. Please enter valid decimal numbers.");
                }
            }
        }

        private void LoadDataFromFile()
        {
            if (File.Exists(jsonFilePath))
            {
                string jsonData = File.ReadAllText(jsonFilePath);
                List<Dictionary<string, object>> data = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonData);

                decimal totalSum = 0; // Variable to calculate the total sum

                foreach (var entry in data)
                {
                    decimal amount = Convert.ToDecimal(entry["Amount"]);
                    decimal quantity = Convert.ToDecimal(entry["Quantity"]);

                    // Find the row index based on the "Amount" value
                    int rowIndex = FindRowIndexByAmount((int)amount);
                    if (rowIndex != -1)
                    {
                        // Update the "Quantity" and "Total Amount" cells in the same row
                        DataGridViewCell quantityCell = GridPlasada_Summary.Rows[rowIndex].Cells["QUANTITY"];
                        DataGridViewCell totalAmountCell = GridPlasada_Summary.Rows[rowIndex].Cells["TOTAL_AMOUNT"];

                        quantityCell.Value = quantity; // Update Quantity
                        decimal totalAmount = amount * quantity; // Recalculate Total Amount
                        totalAmountCell.Value = totalAmount.ToString(); // Convert totalAmount to string for display

                        totalSum += totalAmount; // Update the total sum
                    }
                    else
                    {
                        // Handle the case where the corresponding "Amount" row is not found
                        // You can choose to add a new row or handle the error according to your requirements
                    }
                }

                // Set the total sum only in the last row of the "TOTAL_AMOUNT" column
                int lastRowIndex = GridPlasada_Summary.Rows.Count - 1;
                DataGridViewCell lastTotalAmountCell = GridPlasada_Summary.Rows[lastRowIndex].Cells["TOTAL_AMOUNT"];
                if (lastTotalAmountCell.Value == null || lastTotalAmountCell.Value.ToString() == "")
                {
                    lastTotalAmountCell.Value = totalSum.ToString();
                    lastTotalAmountCell.Style.ForeColor = Color.Red; // Set the foreground color to red

                    // Display the total sum in the textBox_Total
                    textBox_Total.Text = totalSum.ToString();
                }
            }
        }





        private void SaveDataToFile()
        {
            List<Dictionary<string, object>> data = new List<Dictionary<string, object>>();

            // Iterate through each row except the last one
            for (int i = 0; i < GridPlasada_Summary.Rows.Count - 1; i++)
            {
                DataGridViewRow row = GridPlasada_Summary.Rows[i];

                if (!row.IsNewRow && row.Cells["AMOUNT"].Value != null && row.Cells["QUANTITY"].Value != null && row.Cells["TOTAL_AMOUNT"].Value != null)
                {
                    var entry = new Dictionary<string, object>
            {
                { "Amount", row.Cells["AMOUNT"].Value },
                { "Quantity", row.Cells["QUANTITY"].Value },
                { "TotalAmount", row.Cells["TOTAL_AMOUNT"].Value }
            };
                    data.Add(entry);
                }
            }

            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);
            File.WriteAllText(jsonFilePath, jsonData);
        }






        private int FindRowIndexByAmount(int amount)
        {
            foreach (DataGridViewRow row in GridPlasada_Summary.Rows)
            {
                if (Convert.ToInt32(row.Cells["AMOUNT"].Value) == amount)
                {
                    return row.Index;
                }
            }
            return -1; // Return -1 if the row is not found
        }


        private void GridPlasada_Summary_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {


            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Summary.Columns[e.ColumnIndex].HeaderText == "AMOUNT")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(220, 220, 224); // Change this to your desired color

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


            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Summary.Columns[e.ColumnIndex].HeaderText == "QUANTITY")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(220, 220, 224); // Change this to your desired color

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

        private void button_ClearCashCount_Click(object sender, EventArgs e)
        {

            foreach (DataGridViewRow row in GridPlasada_Summary.Rows)
            {
                row.Height = 32;
            }

            // Clear the DataGridView
            GridPlasada_Summary.Rows.Clear();

            // Clear the TextBox for the overall total
            textBox_Total.Text = "";

            // Clear the JSON file
            if (File.Exists(jsonFilePath))
            {
                File.Delete(jsonFilePath);
            }
        }
    }


}

