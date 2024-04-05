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

namespace FightingFeather
{
    public partial class AdminForm : MetroFramework.Forms.MetroForm
    {
        private SQLiteConnection sqliteConnection;
        private string databaseName = "FFkey.db";
        private string connectionString;

        private string securityKey = "1727";

        public AdminForm()
        {
            InitializeComponent();
            connectionString = $"Data Source={databaseName};Version=3;";

            // Set raDateTimePicker1 to show the current date
            raDateTimePicker1.Value = DateTime.Today;

            sqliteConnection = new SQLiteConnection("Data Source=FFKey.db;Version=3;");
            sqliteConnection.Open();

            // Populate the ComboBox with options
            comboBox_Status.Items.AddRange(new object[] { "N/A", "ACTIVE", "SUSPENDED" });

            // Subscribe to the CellFormatting event
            Grid_RegisterUsers.CellFormatting += Grid_RegisterUsers_CellFormatting;

            //Subscribe to the CellPainting event
            Grid_RegisterUsers.CellPainting += Grid_RegisterUsers_CellPainting;

        }


        private void label_RegisterUsers_Click(object sender, EventArgs e)
        {
            label_RegisterUsers.ForeColor = Color.FromArgb(193, 84, 55);
            label_AdminSettings.ForeColor = Color.FromArgb(64, 64, 64);
            panel_Indicator.Location = new Point(260, 66);
            panel_Indicator.Size = new Size(65, 4);

           
            panel_BGAddNewUser.Visible = false;
            panel_AdvanceSettings.Visible = false;
        }

        private void label_AdminSettings_Click(object sender, EventArgs e)
        {
            label_AdminSettings.ForeColor = Color.FromArgb(193, 84, 55);
            label_RegisterUsers.ForeColor = Color.FromArgb(64, 64, 64);
            panel_Indicator.Location = new Point(375, 66);
            panel_Indicator.Size = new Size(121, 4);

            panel_AdvanceSettings.Visible = true;
            panel_BGAddNewUser.Visible = true;
           
        }

        private void button_AddNewUser_Click(object sender, EventArgs e)
        {
            // Get the maximum ID from the database and increment it by 1
            string generatedId = GetNextIdFromDatabase();

            // Display the generated ID in the textBox_ID
            textBox_ID.Text = generatedId;

            panel_BGAddNewUser.Visible = true;
            label_AdminSettings.Enabled = false;
            label_RegisterUsers.Enabled = false;
            button_License.Enabled = false;
            button_SaveUpdate.Visible = false;
                    
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
            label_RegisterUsers.Enabled = true;
            button_License.Enabled = true;
            button_SaveUpdate.Visible = false;
            button_AddNewUser.Visible = true;
            label_Dis.Text = "REGISTRATION FORM";
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
                button_SaveUpdate.Enabled = true;
                label_AdminSettings.Enabled = false;
                label_RegisterUsers.Enabled = false;
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
                    panel_BGAddNewUser.Visible = false;
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
            // Close the SQLite connection if it's open
            if (sqliteConnection.State != ConnectionState.Closed)
            {
                sqliteConnection.Close();
            }
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {
            if (!File.Exists(databaseName))
            {
                CreateDatabase();
            }
            CreateTablesIfNotExist();
            LoadDataToGrid();
        }

        private void CreateDatabase()
        {
            SQLiteConnection.CreateFile(databaseName);
        }

        private void CreateTablesIfNotExist()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string checkAccountRegisteredTableExists = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='AccountRegistered'";
                string checkAdminTableExists = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='Admin'";

                SQLiteCommand command1 = new SQLiteCommand(checkAccountRegisteredTableExists, connection);
                int accountRegisteredTableExists = Convert.ToInt32(command1.ExecuteScalar());

                SQLiteCommand command2 = new SQLiteCommand(checkAdminTableExists, connection);
                int adminTableExists = Convert.ToInt32(command2.ExecuteScalar());

                if (accountRegisteredTableExists == 0)
                {
                    string createAccountRegisteredTable = @"CREATE TABLE AccountRegistered (
                                                    ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                                    Name TEXT,
                                                    Username TEXT,
                                                    Password TEXT,
                                                    Status TEXT,
                                                    Date INTEGER
                                                )";
                    SQLiteCommand createCommand1 = new SQLiteCommand(createAccountRegisteredTable, connection);
                    createCommand1.ExecuteNonQuery();
                }

                if (adminTableExists == 0)
                {
                    // If Admin table doesn't exist, create it
                    string createAdminTable = @"CREATE TABLE Admin (
                                ID INTEGER PRIMARY KEY AUTOINCREMENT,
                                Username TEXT,
                                Password TEXT
                            )";
                    SQLiteCommand createCommand2 = new SQLiteCommand(createAdminTable, connection);
                    createCommand2.ExecuteNonQuery();
                }
                else
                {
                    // Check if Admin table is empty
                    string checkAdminEmpty = "SELECT count(*) FROM Admin";
                    SQLiteCommand checkAdminCommand = new SQLiteCommand(checkAdminEmpty, connection);
                    int adminRowCount = Convert.ToInt32(checkAdminCommand.ExecuteScalar());

                    if (adminRowCount == 0)
                    {
                        // If Admin table exists but is empty, insert new records
                        string insertAdminData = @"
        INSERT INTO Admin (Username, Password) VALUES ('admin', 'admin');
        INSERT INTO Admin (Username, Password) VALUES ('master', '888534master__17!&');
        ";
                        SQLiteCommand insertCommand = new SQLiteCommand(insertAdminData, connection);
                        insertCommand.ExecuteNonQuery();
                    }
                }
            }
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
  

        private void button_CheckPass_Click(object sender, EventArgs e)
        {
            if (Grid_RegisterUsers.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = Grid_RegisterUsers.SelectedRows[0];

                // Get the value of the Password column in the selected row
                string password = selectedRow.Cells["PASSWORD"].Value?.ToString();

                if (!string.IsNullOrEmpty(password))
                {
                    // Display the password
                    MessageBox.Show($"Password: {password}", "Password", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("No password found for the selected row.", "No Password", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to view the password.", "No Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button_ChangePass_Click(object sender, EventArgs e)
        {
            if (Grid_RegisterUsers.SelectedRows.Count > 0)
            {
                // Get the selected row
                DataGridViewRow selectedRow = Grid_RegisterUsers.SelectedRows[0];

                // Get the ID of the selected user
                string userId = selectedRow.Cells["ID"].Value?.ToString();

                if (!string.IsNullOrEmpty(userId))
                {
                    // Prompt user to enter new password
                    string newPassword = PromptForNewPassword();

                    if (!string.IsNullOrEmpty(newPassword))
                    {
                        // Update password in the database
                        UpdatePasswordInDatabase(userId, newPassword);
                    }
                }
                else
                {
                    MessageBox.Show("No user ID found for the selected row.", "No User ID", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select a row to change the password.", "No Row Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string PromptForNewPassword()
        {
            // Create the form
            using (var form = new Form())
            {
                // Set form properties
                form.Text = "Enter New Password";
                form.FormBorderStyle = FormBorderStyle.FixedDialog; // Remove minimize and maximize buttons
                form.StartPosition = FormStartPosition.CenterScreen; // Center the form
                form.ClientSize = new Size(200, 70); // Set custom size
                form.MaximizeBox = false; // Disable maximize button
                form.MinimizeBox = false; // Disable minimize button

                // Create the password input textbox
                var passwordBox = new TextBox
                {
                    PasswordChar = '*',
                    Dock = DockStyle.Top,
                    MaxLength = 100,
                    Margin = new Padding(10)
                };

                // Create the OK button
                var okButton = new Button
                {
                    Text = "OK",
                    Dock = DockStyle.Bottom,
                    BackColor = Color.WhiteSmoke,
                    ForeColor = Color.DimGray
                };

                // Handle button click event
                okButton.Click += (sender, e) => form.DialogResult = DialogResult.OK;

                // Add controls to the form
                form.Controls.Add(passwordBox);
                form.Controls.Add(okButton);

                // Show the form
                if (form.ShowDialog() == DialogResult.OK)
                {
                    return passwordBox.Text;
                }
                else
                {
                    return null;
                }
            }
        }


        private void UpdatePasswordInDatabase(string userId, string newPassword)
        {
            string updateQuery = "UPDATE AccountRegistered SET Password = @Password WHERE ID = @ID";

            using (SQLiteCommand cmd = new SQLiteCommand(updateQuery, sqliteConnection))
            {
                cmd.Parameters.AddWithValue("@ID", userId);
                cmd.Parameters.AddWithValue("@Password", newPassword);

                int result = cmd.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Password changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Refresh the grid
                    LoadDataToGrid();
                }
                else
                {
                    MessageBox.Show("Failed to change password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


        private void button_Export_Click(object sender, EventArgs e)
        {
            // Show the save file dialog
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog.FileName = "FF_Accounts.csv"; // Default file name
            saveFileDialog.Title = "Export to CSV";
            saveFileDialog.ShowDialog();

            // If the file name is not an empty string, proceed to export
            if (saveFileDialog.FileName != "")
            {
                try
                {
                    // Open the file stream for writing
                    using (StreamWriter streamWriter = new StreamWriter(saveFileDialog.FileName))
                    {
                        // Write the column headers
                        for (int i = 0; i < Grid_RegisterUsers.Columns.Count; i++)
                        {
                            streamWriter.Write(Grid_RegisterUsers.Columns[i].HeaderText);
                            if (i < Grid_RegisterUsers.Columns.Count - 1)
                                streamWriter.Write(",");
                        }
                        streamWriter.WriteLine();

                        // Write the data rows
                        foreach (DataGridViewRow row in Grid_RegisterUsers.Rows)
                        {
                            for (int i = 0; i < Grid_RegisterUsers.Columns.Count; i++)
                            {
                                if (row.Cells[i].Value != null)
                                    streamWriter.Write(row.Cells[i].Value.ToString());
                                else
                                    streamWriter.Write("");
                                if (i < Grid_RegisterUsers.Columns.Count - 1)
                                    streamWriter.Write(",");
                            }
                            streamWriter.WriteLine();
                        }
                    }

                    MessageBox.Show("Data exported successfully!", "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error exporting data: " + ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button_FIND_Click(object sender, EventArgs e)
        {
            SearchAccountsGrid();
        }

        private void SearchAccountsGrid()
        {
            // Get the search term from the TextBox
            string searchTerm = textBox_Search.Text.Trim();

            // If the search term is empty, return
            if (string.IsNullOrEmpty(searchTerm))
            {
                MessageBox.Show("Please enter a search term.", "Empty Search Term", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Store the visibility state of columns
            Dictionary<string, bool> columnVisibility = new Dictionary<string, bool>();
            foreach (DataGridViewColumn column in Grid_RegisterUsers.Columns)
            {
                columnVisibility[column.Name] = column.Visible;
            }

            // Show all columns to ensure searching works correctly
            foreach (DataGridViewColumn column in Grid_RegisterUsers.Columns)
            {
                column.Visible = true;
            }

            // Flag to check if any match is found
            bool matchFound = false;

            // Iterate through the rows of the DataGridView
            foreach (DataGridViewRow row in Grid_RegisterUsers.Rows)
            {
                // Flag to check if the row contains a match
                bool rowContainsMatch = false;

                // Iterate through the specific columns (ID, NAME, USERNAME)
                for (int columnIndex = 0; columnIndex < Grid_RegisterUsers.Columns.Count; columnIndex++)
                {
                    // Check if the current column is one of the specified columns
                    if (Grid_RegisterUsers.Columns[columnIndex].Name == "ID" ||
                        Grid_RegisterUsers.Columns[columnIndex].Name == "NAME" ||
                        Grid_RegisterUsers.Columns[columnIndex].Name == "USERNAME")
                    {
                        // Check if the cell value contains the search term (case-insensitive)
                        if (row.Cells[columnIndex].Value != null && row.Cells[columnIndex].Value.ToString().IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            // Select the row containing the matched cell
                            row.Selected = true;
                            // Scroll the DataGridView to the selected row if it's visible
                            if (row.Visible)
                            {
                                Grid_RegisterUsers.FirstDisplayedScrollingRowIndex = row.Index;
                            }
                            // Set the flag indicating a match is found
                            matchFound = true;
                            // Set the flag indicating the row contains a match
                            rowContainsMatch = true;
                            // Exit the column iteration loop
                            break;
                        }
                    }
                }

                // If the row doesn't contain a match, hide it
                row.Visible = rowContainsMatch;
            }

            // Restore the original visibility state of columns
            foreach (DataGridViewColumn column in Grid_RegisterUsers.Columns)
            {
                column.Visible = columnVisibility[column.Name];
            }

            // If no match is found, display a message
            if (!matchFound)
            {
                MessageBox.Show("Search term not found.", "No Match", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button_CLEAR_Click(object sender, EventArgs e)
        {
            // Clear the search textbox
            textBox_Search.Clear();

            // Reload the data into the DataGridView to revert to the default state
            LoadDataToGrid();
        }

        private void textBox_Search_ClearClicked()
        {
            // Clear the search textbox
            textBox_Search.Clear();

            // Reload the data into the DataGridView to revert to the default state
            LoadDataToGrid();
        }

        private void textBox_Search_ButtonClick(object sender, EventArgs e)
        {
            SearchAccountsGrid();
        }

        private void button_SecKey_Click(object sender, EventArgs e)
        {
            string enteredKey = textBox_SecKey.Text;

            // Check if the entered key matches the predefined security key
            if (enteredKey == securityKey)
            {
                MessageBox.Show("Security key matched. Access granted.");

                // Enable controls if security key is successful
                textBox_NewPass.Enabled = true;
                textBox_ConfirmPass.Enabled = true;
                button_SaveNewPassword.Enabled = true;
                button_SaveNewPassword.BackColor = Color.FromArgb(0, 120, 215);
                button_SaveNewPassword.FlatAppearance.BorderColor = Color.FromArgb(141, 195, 237);
                button_SaveNewPassword.ForeColor = Color.White;
            }
            else
            {
                MessageBox.Show("Incorrect security key. Access denied.");

            }

        }

        private void button_SaveNewPassword_Click(object sender, EventArgs e)
        {

        }

        private void Grid_RegisterUsers_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            // Check if the current cell belongs to the Password column and its value is not null
            if (Grid_RegisterUsers.Columns[e.ColumnIndex].Name == "PASSWORD" && e.Value != null)
            {
                // Replace the actual password value with asterisks
                e.Value = new string('*', e.Value.ToString().Length);
            }

            // Check if the current cell belongs to the column "NAME"
            if (Grid_RegisterUsers.Columns[e.ColumnIndex].Name == "NAME")
            {
                // Apply the padding to the header cell of the "NAME" column
                Grid_RegisterUsers.Columns[e.ColumnIndex].HeaderCell.Style.Padding = new Padding(10, 4, 0, 4);
            }

            // Check if the current cell belongs to the "STATUS" column
            if (Grid_RegisterUsers.Columns[e.ColumnIndex].Name == "STATUS")
            {
                // Check the cell value and set the forecolor accordingly
                if (e.Value != null && e.Value.ToString() == "ACTIVE")
                {
                    e.CellStyle.ForeColor = Color.SeaGreen;
                }
                else if (e.Value != null && e.Value.ToString() == "SUSPENDED")
                {
                    e.CellStyle.ForeColor = Color.Salmon;
                }
            }
        }

        private void Grid_RegisterUsers_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && Grid_RegisterUsers.Columns[e.ColumnIndex].HeaderText == "NAME")
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

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && Grid_RegisterUsers.Columns[e.ColumnIndex].HeaderText == "USERNAME")
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

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && Grid_RegisterUsers.Columns[e.ColumnIndex].HeaderText == "PASSWORD")
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

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && Grid_RegisterUsers.Columns[e.ColumnIndex].HeaderText == "STATUS")
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
