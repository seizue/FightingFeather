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

namespace FightingFeather
{
    public partial class UserControl_Inventory : UserControl
    {
     
        public UserControl_Inventory()
        {
            InitializeComponent();
            LoadJSONFilesIntoDataGrid();
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

                foreach (string jsonFile in jsonFiles)
                {
                    string fileName = Path.GetFileName(jsonFile);

                    // Add a new row to the DataGridView
                    int rowIndex = postedMunton.Rows.Add();

                    // Specify the "MUNTON" column index or name where you want to display the file name
                    int muntonColumnIndex = postedMunton.Columns["MUNTON"].Index;

                    // Set the file name to the specified column in the newly added row
                    postedMunton.Rows[rowIndex].Cells[muntonColumnIndex].Value = fileName;
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
