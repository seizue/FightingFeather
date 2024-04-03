using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace FightingFeather
{
    public partial class UserControl_Summa : UserControl
    {

        private string jsonFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "JSON", "cashCount.json");
      
        public UserControl_Summa()
        {
            InitializeComponent();
          

            GridPlasada_Summary.CellPainting += GridPlasada_Summary_CellPainting;
            GridPlasada_Summary.CellEndEdit += GridPlasada_Summary_CellEndEdit;
            GridPlasada_Summary.CellValueChanged += GridPlasada_Summary_CellValueChanged;

            ReloadData();
        }

        public void ReloadData()
        {
            PopulateAmountColumn();

            foreach (DataGridViewRow row in GridPlasada_Summary.Rows)
            {
                row.Height = 32;
            }

            LoadDataFromFile();
        }

        //Trigger to save the data values from textBoxes
        public void PerformSQLiteExport()
        {
            SaveToSQLite();
            SaveToJSON();
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

        private void button_ClearCashCount_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in GridPlasada_Summary.Rows)
            {
                row.Height = 32;
            }

            foreach (DataGridViewRow row in GridPlasada_Summary.Rows)
            {
                // Set the value of QUANTITY column to null
                row.Cells["QUANTITY"].Value = null;

                // Set the value of TOTAL_AMOUNT column to empty string
                row.Cells["TOTAL_AMOUNT"].Value = "";

                // You can also apply any other custom clearing logic here if needed
            }

            // Clear the TextBox for the overall total
            textBox_Total.Text = "-";

            // Clear the JSON file
            if (File.Exists(jsonFilePath))
            {
                File.Delete(jsonFilePath);
            }
        }
    
        private void CalculateTotalSum()
        {
            decimal totalSum = 0;

            foreach (DataGridViewRow row in GridPlasada_Summary.Rows)
            {
                if (!row.IsNewRow && row.Cells["TOTAL_AMOUNT"].Value != null && decimal.TryParse(row.Cells["TOTAL_AMOUNT"].Value.ToString(), out decimal amount))
                {
                    totalSum += amount;
                }
            }

            // Update the total sum in the last row of the "TOTAL_AMOUNT" column
            int lastRowIndex = GridPlasada_Summary.Rows.Count - 1;
            DataGridViewCell lastTotalAmountCell = GridPlasada_Summary.Rows[lastRowIndex].Cells["TOTAL_AMOUNT"];
            lastTotalAmountCell.Value = totalSum.ToString(); // Update the value
            lastTotalAmountCell.Style.ForeColor = Color.Red; // Set the foreground color to red
        }


        public void SetDateText(string text)
        {
            textBox_Date.Text = text;
        }

        public void UpdateFeeTotal(string total)
        {
            textBox_Plasada.Text = total;
        }

        public void UpdateCityTax(string cityTaxValue)
        {

            textBox_CityTax.Text = cityTaxValue;
        }

        public void UpdateTotal(string total)
        {
            textBox_Total.Text = total;
        }

        public void UpdateFightTotal(int totalFights)
        {
            textBox_TotalFight.Text = $"{totalFights}";
        }

        public void UpdateDrawCancelTotal(int totalDraws)
        {
            textBox_Draw.Text = $"{totalDraws}";
        }


        private void button_Export_Click(object sender, EventArgs e)
        {
            // Collect data from text boxes
            string date = textBox_Date.Text;
            string plasada = textBox_Plasada.Text;
            string cityTax = textBox_CityTax.Text;
            string total = textBox_Total.Text;
            string totalFight = textBox_TotalFight.Text;
            string draw = textBox_Draw.Text;
            string gate = textBox_Gate.Text;

            // Format the date as yyyyMMdd
            string formattedDate = DateTime.Parse(date).ToString("yyyyMMdd");

            // Create CSV content
            StringBuilder csvContent = new StringBuilder();
            csvContent.AppendLine("Date,Plasada,CityTax,Total,TotalFight,Draw,Gate");
            csvContent.AppendLine($"{date},{plasada},{cityTax},{total},{totalFight},{draw},{gate}");

            // Prompt user to select file destination
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                // Construct the file name based on Plasada_Summary and date
                string fileName = $"Plasada_Summary_{formattedDate}.csv";
                saveFileDialog.FileName = fileName;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog.FileName;

                    try
                    {
                        // Write CSV content to file
                        File.WriteAllText(filePath, csvContent.ToString());

                        MessageBox.Show("Data exported to CSV successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while exporting data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }

        }

        private void SaveToSQLite()
        {
            // Collect data from text boxes
            string date = textBox_Date.Text;
            string plasada = textBox_Plasada.Text;
            string cityTax = textBox_CityTax.Text;
            string total = textBox_Total.Text;
            string totalFight = textBox_TotalFight.Text;
            string draw = textBox_Draw.Text;
            string gate = textBox_Gate.Text;

            try
            {
                // Insert data into SQLite database
                string connectionString = "Data Source=munton_summa.db;Version=3;";
                string insertQuery = @"INSERT INTO Plasada_Summary 
                               (Date, Plasada, CityTax, Total, TotalFight, Draw, Gate) 
                               VALUES (@Date, @Plasada, @CityTax, @Total, @TotalFight, @Draw, @Gate)";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    using (SQLiteCommand command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Date", date);
                        command.Parameters.AddWithValue("@Plasada", plasada);
                        command.Parameters.AddWithValue("@CityTax", cityTax);
                        command.Parameters.AddWithValue("@Total", total);
                        command.Parameters.AddWithValue("@TotalFight", totalFight);
                        command.Parameters.AddWithValue("@Draw", draw);
                        command.Parameters.AddWithValue("@Gate", gate);

                        command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the data to the munton_summa: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Method for exporting data to JSON
        private void SaveToJSON()
        {
            // Collect data from text boxes
            string date = textBox_Date.Text;
            string plasada = textBox_Plasada.Text;
            string cityTax = textBox_CityTax.Text;
            string total = textBox_Total.Text;
            string totalFight = textBox_TotalFight.Text;
            string draw = textBox_Draw.Text;
            string gate = textBox_Gate.Text;

            // Create data object
            var newData = new
            {
                Date = date,
                Plasada = plasada,
                CityTax = cityTax,
                Total = total,
                TotalFight = totalFight,
                Draw = draw,
                Gate = gate
            };

            // Specify the folder path using the jsonFilePath variable
            string folderPath = Path.GetDirectoryName(jsonFilePath);

            // Create the folder if it doesn't exist
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // Create the folder if it doesn't exist
            CreateFolder(folderPath);

            // Combine the folder path and the file name "summa.json"
            string filePath = Path.Combine(folderPath, "MSu", "summa.json");

            try
            {
                List<object> existingData;

                // Check if the file exists and contains valid JSON content
                if (File.Exists(filePath))
                {
                    // Read existing JSON content
                    string jsonContent = File.ReadAllText(filePath);

                    // Deserialize the existing JSON content into a list of objects
                    existingData = JsonConvert.DeserializeObject<List<object>>(jsonContent);

                    // Check if existingData is null (invalid JSON format)
                    if (existingData == null)
                    {
                        // Create a new list to hold data
                        existingData = new List<object>();
                    }
                }
                else
                {
                    // Create a new list to hold data
                    existingData = new List<object>();
                }

                // Add new data to the list
                existingData.Add(newData);

                // Serialize the updated list back to JSON
                string updatedJsonContent = JsonConvert.SerializeObject(existingData, Formatting.Indented);

                // Write updated JSON content to file
                File.WriteAllText(filePath, updatedJsonContent);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while saving the data to summa.json: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateFolder(string folderPath)
        {
            try
            {
                // Combine the folder path and the folder name "MSu"
                string msuFolderPath = Path.Combine(folderPath, "MSu");

                // Check if the folder already exists
                if (!Directory.Exists(msuFolderPath))
                {
                    // Create the folder
                    Directory.CreateDirectory(msuFolderPath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while creating the folder: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    CalculateTotalSum(); // Recalculate total sum after editing
                }
                else
                {
                    MessageBox.Show("Invalid quantity or amount. Please enter valid decimal numbers.");
                    // Clear the total amount cell if parsing fails
                    totalAmountCell.Value = null;
                }
            }

        }

        private void GridPlasada_Summary_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the edited cell is in the "TOTAL_AMOUNT" column
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Summary.Columns[e.ColumnIndex].HeaderText == "TOTAL_AMOUNT")
            {
                // Recalculate total sum after any edit in the "TOTAL_AMOUNT" column
                CalculateTotalSum();
            }
        }

        private void GridPlasada_Summary_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0)
            {
                // Check if the entered value is not null or empty
                if (!string.IsNullOrEmpty(e.FormattedValue?.ToString()))
                {
                    // Check if the entered value is a valid number
                    if (!int.TryParse(e.FormattedValue.ToString(), out _))
                    {
                        // If the entered value is not a valid number, cancel the event
                        GridPlasada_Summary.Rows[e.RowIndex].ErrorText = "Only numeric values are allowed.";
                        e.Cancel = true;
                    }
                    else
                    {
                        // Clear any error message if the entered value is a valid number
                        GridPlasada_Summary.Rows[e.RowIndex].ErrorText = "";
                    }
                }
            }
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
    }
}

