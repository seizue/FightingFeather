using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FightingFeather
{
    public partial class AdminForm : MetroFramework.Forms.MetroForm
    {
        private SQLiteConnection sqliteConnection;

        public AdminForm()
        {
            InitializeComponent();

            // Set raDateTimePicker1 to show the current date
            raDateTimePicker1.Value = DateTime.Today;

            // Populate the ComboBox with options
            comboBox_Status.Items.AddRange(new object[] { "N/A", "ACTIVE", "SUSPENDED" });

            sqliteConnection = new SQLiteConnection("Data Source=FFKey.db;Version=3;");
            sqliteConnection.Open();

            // Subscribe to the CellFormatting event
            Grid_RegisterUsers.CellFormatting += Grid_RegisterUsers_CellFormatting;
        }


        private void label_RegisterUsers_Click(object sender, EventArgs e)
        {
            label_RegisterUsers.ForeColor = Color.FromArgb(193, 84, 55);
            label_AdminSettings.ForeColor = Color.FromArgb(64, 64, 64);
            panel_Indicator.Location = new Point(260, 66);
            panel_Indicator.Size = new Size(65, 4);
        }

        private void label_AdminSettings_Click(object sender, EventArgs e)
        {
            label_AdminSettings.ForeColor = Color.FromArgb(193, 84, 55);
            label_RegisterUsers.ForeColor = Color.FromArgb(64, 64, 64);
            panel_Indicator.Location = new Point(375, 66);
            panel_Indicator.Size = new Size(121, 4);
        }

        private void button_AddNewUser_Click(object sender, EventArgs e)
        {
            // Get the maximum ID from the database and increment it by 1
            string generatedId = GetNextIdFromDatabase();

            // Display the generated ID in the textBox_ID
            textBox_ID.Text = generatedId;

            panel_BGAddNewUser.Visible = true;
            label_AdminSettings.Enabled = false;
            button_License.Enabled = false;
            button_SaveUpdate.Enabled = false;
        }

        private string GetNextIdFromDatabase()
        {
            string nextId = "01"; // Default starting ID
            string query = "SELECT MAX(ID) FROM AccountRegistered";

            using (SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection))
            {
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    int maxId;
                    if (int.TryParse(result.ToString(), out maxId))
                    {
                        // Increment the maximum ID by 1 and format it with leading zeros
                        nextId = (maxId + 1).ToString("00");
                    }
                }
            }

            return nextId;
        }

        private void button_CloseRegForm_Click(object sender, EventArgs e)
        {
            // Clear textboxes
            textBox_ID.Text = "";
            textBox_Name.Text = "";
            comboBox_Status.SelectedIndex = -1;
            textBox_Username.Text = "";
            textBox_Password.Text = "";

            panel_BGAddNewUser.Visible=false;
            label_AdminSettings.Enabled = true;
            button_License.Enabled = true;
        }

        private void button_SaveNewUser_Click(object sender, EventArgs e)
        {
            string id = textBox_ID.Text;
            string name = textBox_Name.Text;
            string status = comboBox_Status.SelectedItem?.ToString(); // Get selected item or null
            string username = textBox_Username.Text;
            string password = textBox_Password.Text;
            string date = DateTime.Now.ToString("yyyy-MM-dd");

            // Check if status is null
            if (status == null)
            {
                MessageBox.Show("Please select a status.", "Status Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // Exit the method without saving
            }

            string insertQuery = "INSERT INTO AccountRegistered (ID, Name, Username, Password, Status, Date) VALUES (@ID, @Name, @Username, @Password, @Status, @Date)";

            using (SQLiteCommand cmd = new SQLiteCommand(insertQuery, sqliteConnection))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Status", status);
                cmd.Parameters.AddWithValue("@Date", date);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("User saved successfully!");

                    LoadDataToGrid();

                    // Clear textboxes after saving
                    textBox_ID.Text = "";
                    textBox_Name.Text = "";
                    comboBox_Status.SelectedIndex = -1;
                    textBox_Username.Text = "";
                    textBox_Password.Text = "";

                    panel_BGAddNewUser.Visible = false;
                    label_AdminSettings.Enabled = true;
                    button_License.Enabled = true;
                }
                else
                {
                    MessageBox.Show("Failed to save user.");
                }
            }
        }

        private void button_UpdateUser_Click(object sender, EventArgs e)
        {
            if (Grid_RegisterUsers.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = Grid_RegisterUsers.SelectedRows[0];

                // Get the ID of the selected row
                string idToUpdate = selectedRow.Cells["ID"].Value.ToString();

                // Fetch user data based on the ID
                FetchUserDataAndUpdateTextBoxes(idToUpdate);

                // Show the update form
                panel_BGAddNewUser.Visible = true;
                button_SaveUpdate.Visible = true;
                label_AdminSettings.Enabled = false;
                button_License.Enabled = false;
                label_Dis.Text = "UPDATE FORM";
            }
            else
            {
                MessageBox.Show("Please select a row to update.", "No Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void FetchUserDataAndUpdateTextBoxes(string userId)
        {
            string query = "SELECT * FROM AccountRegistered WHERE ID = @ID";
            using (SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection))
            {
                cmd.Parameters.AddWithValue("@ID", userId);
                using (SQLiteDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Populate textboxes with user data
                        textBox_ID.Text = reader["ID"].ToString();
                        textBox_Name.Text = reader["Name"].ToString();
                        comboBox_Status.SelectedItem = reader["Status"].ToString();
                        textBox_Username.Text = reader["Username"].ToString();
                        textBox_Password.Text = reader["Password"].ToString();
                    }
                }
            }
        }

        private void button_SaveUpdate_Click(object sender, EventArgs e)
        {
            string id = textBox_ID.Text;
            string name = textBox_Name.Text;
            string status = comboBox_Status.SelectedItem?.ToString(); // Get selected item or null
            string username = textBox_Username.Text;
            string password = textBox_Password.Text;

            string updateQuery = "UPDATE AccountRegistered SET Name = @Name, Username = @Username, Password = @Password, Status = @Status WHERE ID = @ID";

            using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, sqliteConnection))
            {
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@Name", name);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@Status", status);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("User updated successfully!");
                    LoadDataToGrid();
                }
                else
                {
                    MessageBox.Show("Failed to update user.");
                }
            }
        }

        private void button_ClearFields_Click(object sender, EventArgs e)
        {
           
            // Clear textboxes
            textBox_ID.Text = "";
            textBox_Name.Text = "";
            comboBox_Status.SelectedIndex = -1;
            textBox_Username.Text = "";
            textBox_Password.Text = "";

            MessageBox.Show("Clear fields successfully!");
        }

        private void AdminForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            sqliteConnection.Close();

        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            LoadDataToGrid();
        }

        private void LoadDataToGrid()
        {
            string query = "SELECT ID, Name, Username, Password, Status, Date FROM AccountRegistered";

            using (SQLiteCommand cmd = new SQLiteCommand(query, sqliteConnection))
            {
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    // Clear existing data
                    Grid_RegisterUsers.Rows.Clear();

                    // Populate the DataGridView with the retrieved data
                    foreach (DataRow row in dt.Rows)
                    {
                        Grid_RegisterUsers.Rows.Add(row.ItemArray);
                    }
                }
            }
        }

        private void Grid_RegisterUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current cell belongs to the Password column and its value is not null
            if (Grid_RegisterUsers.Columns[e.ColumnIndex].Name == "PASSWORD" && e.Value != null)
            {
                // Replace the actual password value with asterisks
                e.Value = new string('*', e.Value.ToString().Length);
            }
        }

        private void button_CheckPass_Click(object sender, EventArgs e)
        {

        }

        private void button_ChangePass_Click(object sender, EventArgs e)
        {

        }

        private void button_Export_Click(object sender, EventArgs e)
        {

        }

        private void button_FIND_Click(object sender, EventArgs e)
        {

        }

        private void button_CLEAR_Click(object sender, EventArgs e)
        {

        }
    }
}
