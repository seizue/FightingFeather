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

                int activeSellCount = 0;

                foreach (string jsonFile in jsonFiles)
                {
                    activeSellCount++;

                    string fileName = Path.GetFileName(jsonFile);
                    string[] fileNameParts = fileName.Split('_');

                    // Assuming the date part is always at the last index of fileNameParts
                    string datePart = fileNameParts[fileNameParts.Length - 1];

                    // Format date as YYYY-MM-DD
                    string formattedDate = $"{datePart.Substring(0, 4)}-{datePart.Substring(4, 2)}-{datePart.Substring(6, 2)}";

                    // Add a new row to the DataGridView
                    int rowIndex = postedMunton.Rows.Add();

                    // Specify the "MUNTON" column index or name where you want to display the file name
                    int muntonColumnIndex = postedMunton.Columns["MUNTON"].Index;
                    // Set the file name to the specified column in the newly added row
                    postedMunton.Rows[rowIndex].Cells[muntonColumnIndex].Value = fileName;

                    // Set the ITEM_NO column to the sequential number
                    int itemNoColumnIndex = postedMunton.Columns["ITEM_NO"].Index;
                    postedMunton.Rows[rowIndex].Cells[itemNoColumnIndex].Value = activeSellCount;

                    // Set the date to the "DATE" column
                    int dateColumnIndex = postedMunton.Columns["DATE"].Index; // Assuming "DATE" is the column name
                    postedMunton.Rows[rowIndex].Cells[dateColumnIndex].Value = formattedDate;

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
            
        }

     

    }
}
