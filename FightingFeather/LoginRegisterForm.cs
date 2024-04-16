using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.IO;

namespace FightingFeather
{
    public partial class LoginRegisterForm : MetroFramework.Forms.MetroForm
    {
        private Main mainForm;
 
        private string databaseName = "FFkey.db";
        private string connectionString;
        public LoginRegisterForm(Main mainForm)
        {
            connectionString = $"Data Source={databaseName};Version=3;";
            this.mainForm = mainForm;

            InitializeComponent();

            if (!File.Exists(databaseName))
            {
                CreateDatabase();
            }
            CreateTablesIfNotExist();

            // Subscribe to the CheckedChanged event of the ShowPasswordCheckBox
            ShowPasswordCheckBox.CheckedChanged += ShowPasswordCheckBox_CheckedChanged;
        }

        private void label_Login_Click(object sender, EventArgs e)
        {
            label_Login.ForeColor = Color.FromArgb(13, 64, 113);
            label_Help.ForeColor = Color.DimGray;
            panel_Indicator.Location = new System.Drawing.Point(29, 50);
            panel_Help.Visible = false;
        }

        private void label_Help_Click(object sender, EventArgs e)
        {
            label_Help.ForeColor = Color.FromArgb(13, 64, 113);
            label_Login.ForeColor = Color.DimGray;
            panel_Indicator.Location = new System.Drawing.Point(127, 51);
            panel_Help.Visible = true;
        }

        private void textBox_Search_ButtonClick(object sender, EventArgs e)
        {
            textBox_Username.Text = "";
        }

        private void textBox_Password_ButtonClick(object sender, EventArgs e)
        {
            textBox_Password.Text = "";
        }

        private bool IsPasswordValid(string username, string password)
        {
            // Define the connection string for SQLite
            string connectionString = "Data Source=FFkey.db;Version=3;";

            // Define the query to check if the username and password match
            string query = "SELECT COUNT(*) FROM AccountRegistered WHERE Username = @Username AND Password = @Password AND Status != 'SUSPENDED'";

            // Create a connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Create a command to execute the query
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Add parameters to the query
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        // Execute the query and get the result
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // If count > 0, it means there is a match and the account is not suspended
                        // Otherwise, the credentials are invalid or the account is suspended
                        return count > 0;
                    }
                }
                finally
                {
                    // Ensure the connection is always closed even if an exception occurs
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
            }
        }


        private bool IsAdminPasswordValid(string username, string password)
        {
            // Define the connection string for SQLite
            string connectionString = "Data Source=FFkey.db;Version=3;";

            // Define the query to check if the username and password match for Admin
            string query = "SELECT COUNT(*) FROM Admin WHERE Username = @Username AND Password = @Password";

            // Create a connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();

                    // Create a command to execute the query
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Add parameters to the query
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        // Execute the query and get the result
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // If count > 0, it means there is a match, otherwise, the credentials are invalid
                        return count > 0;
                    }
                }
                finally
                {
                    // Ensure the connection is always closed even if an exception occurs
                    if (connection.State != ConnectionState.Closed)
                        connection.Close();
                }
            }
        }


        public enum UserType
        {
            Admin,
            NormalUser
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            // Retrieve username and password input
            string username = textBox_Username.Text;
            string password = textBox_Password.Text;

            // Check if username and password are correct for normal users (SQLite)
            if (IsPasswordValid(username, password))
            {
                // Successful login for normal user
                MessageBox.Show("Login successful!");

                // Hide the LoginForm
                this.Hide();

                // Show the MainForm
                Main mainForm = new Main();
                mainForm.Show();
                mainForm.LoginComplete(UserType.NormalUser);

                // Optionally, you can clear the input fields
                textBox_Username.Clear();
                textBox_Password.Clear();
            }

            // Check if username and password are correct for admin (SQLite) or using hardcoded credentials
            else if (IsAdminPasswordValid(username, password))
            {
                // Successful login for admin
                MessageBox.Show("Admin login successful!");

                // Hide the LoginForm
                this.Hide();

                // Show the MainForm
                Main mainForm = new Main();
                mainForm.Show();
                mainForm.LoginComplete(UserType.Admin);

                // Optionally, you can clear the input fields
                textBox_Username.Clear();
                textBox_Password.Clear();
            }
            else
            {
                // Failed login
                MessageBox.Show("Invalid username or password. Please try again.");

                // You can optionally clear the input fields to allow the user to retry.
                textBox_Username.Clear();
                textBox_Password.Clear();

                // Set focus back to the username field for convenience.
                textBox_Username.Focus();
            }
        }




        private void fogotPass_Link_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact your system administrator if you forgot the password!");
        }

        private void textBox_Password_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key is pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Trigger the button click event
                button_Login.PerformClick();
            }
        }

        private void textBox_Username_KeyDown(object sender, KeyEventArgs e)
        {
            // Check if the Enter key is pressed
            if (e.KeyCode == Keys.Enter)
            {
                // Prevent the key from being processed by the textbox
                e.SuppressKeyPress = true;

                // Set focus to the next textbox
                textBox_Password.Focus();
            }
        }
        
        private void ShowPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            // Toggle the UseSystemPasswordChar property of the password textbox
            textBox_Password.UseSystemPasswordChar = !ShowPasswordCheckBox.Checked;

            // If system password characters are used, set PasswordChar to an empty string to show the password as plain text
            if (ShowPasswordCheckBox.Checked)
            {
                textBox_Password.PasswordChar = '\0'; // Set PasswordChar to null character
            }
            else
            {
                // If system password characters are not used, set PasswordChar to * to hide the password
                textBox_Password.PasswordChar = '*';
            }
        }


        private void LoginRegisterForm_FormClosing(object sender, FormClosingEventArgs e)
        {
          Application.Exit();
        }

        private void LoginRegisterForm_Load(object sender, EventArgs e)
        {
            if (!File.Exists(databaseName))
            {
                CreateDatabase();
            }
            CreateTablesIfNotExist();
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
                string checkFLMTableExists = "SELECT count(*) FROM sqlite_master WHERE type='table' AND name='FLM'";

                SQLiteCommand command1 = new SQLiteCommand(checkAccountRegisteredTableExists, connection);
                int accountRegisteredTableExists = Convert.ToInt32(command1.ExecuteScalar());

                SQLiteCommand command2 = new SQLiteCommand(checkAdminTableExists, connection);
                int adminTableExists = Convert.ToInt32(command2.ExecuteScalar());

                SQLiteCommand command3 = new SQLiteCommand(checkFLMTableExists, connection);
                int flmTableExists = Convert.ToInt32(command3.ExecuteScalar());

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
                    INSERT INTO Admin (Username, Password) VALUES ('admin', '888534Admin__');
                    INSERT INTO Admin (Username, Password) VALUES ('master', '888534master17__!&');
                    ";
                        SQLiteCommand insertCommand = new SQLiteCommand(insertAdminData, connection);
                        insertCommand.ExecuteNonQuery();
                    }
                }

                if (flmTableExists == 0)
                {
                    // If FLM table doesn't exist, create it
                    string createFLMTable = @"CREATE TABLE FLM (
                       ID INTEGER PRIMARY KEY AUTOINCREMENT,
                       ExperienceDays INTEGER,
                       ExpirationDate DATE,
                       LicenseCode TEXT,
                       LicenseKey TEXT,
                       LicenseType TEXT,
                       LicenseStatus TEXT,
                       CreatedDate DATE
                   )";
                    SQLiteCommand createCommand3 = new SQLiteCommand(createFLMTable, connection);
                    createCommand3.ExecuteNonQuery();
                }
            }
        }

    }
}
