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
using Newtonsoft.Json;

namespace FightingFeather
{
    public partial class LoginRegisterForm : MetroFramework.Forms.MetroForm
    {
        private Main mainForm;
 
        private string databaseName = "FFkey.db";
        private string connectionString;
        public LoginRegisterForm(Main mainForm)
        {
            
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fighting Feather");
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            databaseName = Path.Combine(appDataPath, "FFkey.db");
            connectionString = $"Data Source={databaseName};Version=3;";
            this.mainForm = mainForm;

            InitializeComponent();

            if (!File.Exists(databaseName))
            {
                CreateDatabase();
            }

            // Ensure tables are created after the database is initialized
            CreateTablesIfNotExist();
            CreateDirectoryFLM();

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
            // Use the updated connection string pointing to the correct database location
            Console.WriteLine($"Using connection string: {connectionString}");

            // Define the query to check if the username and password match
            string query = "SELECT COUNT(*) FROM AccountRegistered WHERE Username = @Username AND Password = @Password AND Status != 'SUSPENDED'";

            // Create a connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();
                    Console.WriteLine("Database connection opened for IsPasswordValid.");

                    // Create a command to execute the query
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Add parameters to the query
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        // Execute the query and get the result
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // If count > 0, it means there is a match and the account is not suspended
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in IsPasswordValid: {ex.Message}");
                    throw;
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
            // Use the updated connection string pointing to the correct database location
            Console.WriteLine($"Using connection string: {connectionString}");

            // Debug query to verify the existence of the Admin table
            string verifyTableQuery = "SELECT name FROM sqlite_master WHERE type='table' AND name='Admin'";
            string query = "SELECT COUNT(*) FROM Admin WHERE Username = @Username AND Password = @Password";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                try
                {
                    // Open the connection
                    connection.Open();
                    Console.WriteLine("Database connection opened for IsAdminPasswordValid.");

                    // Verify the existence of the Admin table
                    using (SQLiteCommand verifyCommand = new SQLiteCommand(verifyTableQuery, connection))
                    {
                        var result = verifyCommand.ExecuteScalar();
                        if (result == null)
                        {
                            Console.WriteLine("Admin table does not exist. Recreating the table.");
                            CreateTablesIfNotExist();
                        }
                    }

                    // Create a command to execute the query
                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Add parameters to the query
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Password", password);

                        // Execute the query and get the result
                        int count = Convert.ToInt32(command.ExecuteScalar());

                        // If count > 0, it means there is a match
                        return count > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error in IsAdminPasswordValid: {ex.Message}");
                    throw;
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

        private void CreateDatabase()
        {
            // Ensure the directory exists before creating the database file
            string appDataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fighting Feather");
            if (!Directory.Exists(appDataPath))
            {
                Directory.CreateDirectory(appDataPath);
            }
            SQLiteConnection.CreateFile(databaseName);
            Console.WriteLine($"Database created at: {databaseName}");
        }

        private void CreateTablesIfNotExist()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                Console.WriteLine("Database connection opened.");

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
                    Console.WriteLine("AccountRegistered table created.");
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
                    Console.WriteLine("Admin table created.");
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
                    ";
                        SQLiteCommand insertCommand = new SQLiteCommand(insertAdminData, connection);
                        insertCommand.ExecuteNonQuery();
                        Console.WriteLine("Admin table initialized with default data.");
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
                    Console.WriteLine("FLM table created.");
                }
            }
        }

        private void LoginRegisterForm_Load(object sender, EventArgs e)
        {
            if (!File.Exists(databaseName))
            {
                CreateDatabase();
            }
            CreateTablesIfNotExist();

            // Check if FLM data exists before inserting
            if (!FLMDataExists("FREE TRIAL", "FREE TRIAL", DateTime.Now, 30))
            {
                // Insert FLM data
                InsertFLMData("FREE TRIAL", "FREE TRIAL", DateTime.Now, 30);
            }
        }

        private void InsertFLMData(string licenseType, string licenseStatus, DateTime createdDate, int experienceDays)
        {
            // Define the SQL query to insert data into the FLM table
            string query = "INSERT INTO FLM (LicenseType, LicenseStatus, CreatedDate, ExperienceDays, ExpirationDate, LicenseCode, LicenseKey) " +
                           "VALUES (@LicenseType, @LicenseStatus, @CreatedDate, @ExperienceDays, @ExpirationDate, @LicenseCode, @LicenseKey)";

            // Calculate the expiration date based on the created date and experience days
            DateTime expirationDate = createdDate.AddDays(experienceDays);

            // Define the LicenseCode and LicenseKey
            string licenseCode = "Fighting-Feather";
            string licenseKey = "Fighting-Feather";

            // Create a connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Check if the data already exists
                if (FLMDataExists(licenseType, licenseStatus, createdDate, experienceDays))
                {
                    // Data already exists, so return without inserting
                    return;
                }

                // Create a command object with the query and connection
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@LicenseType", licenseType);
                    command.Parameters.AddWithValue("@LicenseStatus", licenseStatus);
                    command.Parameters.AddWithValue("@CreatedDate", createdDate);
                    command.Parameters.AddWithValue("@ExperienceDays", experienceDays);
                    command.Parameters.AddWithValue("@ExpirationDate", expirationDate);
                    command.Parameters.AddWithValue("@LicenseCode", licenseCode);
                    command.Parameters.AddWithValue("@LicenseKey", licenseKey);

                    // Execute the command
                    command.ExecuteNonQuery();
                }
            }
        }

        private bool FLMDataExists(string licenseType, string licenseStatus, DateTime createdDate, int experienceDays)
        {
            // Define the SQL query to check if the data exists
            string query = "SELECT COUNT(*) FROM FLM WHERE LicenseCode = @LicenseCode AND LicenseKey = @LicenseKey";

            // Create a connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                // Open the connection
                connection.Open();

                // Create a command object with the query and connection
                using (SQLiteCommand command = new SQLiteCommand(query, connection))
                {
                    // Add parameters to the command
                    command.Parameters.AddWithValue("@LicenseCode", "Fighting-Feather");
                    command.Parameters.AddWithValue("@LicenseKey", "Fighting-Feather");

                    // Execute the query and get the result
                    int count = Convert.ToInt32(command.ExecuteScalar());

                    // If count > 0, it means the data exists
                    return count > 0;
                }
            }
        }

        private void AppendToJsonFile(string filePath, object data)
        {
            // Serialize JSON data with indentation for better readability
            string jsonData = JsonConvert.SerializeObject(data, Formatting.Indented);

            // Append serialized JSON data to the file followed by a new line
            using (StreamWriter sw = File.AppendText(filePath))
            {
                // Check if the file is empty
                sw.WriteLine(sw.BaseStream.Length == 0 ? $"[{jsonData}]" : $",{jsonData}");
            }
        }


        private void CreateDirectoryFLM()
        {
            // Get the JSON folder path
            string jsonFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fighting Feather", "JSON", "FLM");
            Console.WriteLine($"JSON File Path: {jsonFolderPath}");

            // Check if the directory exists, if not, create it
            if (!Directory.Exists(jsonFolderPath))
            {
                Directory.CreateDirectory(jsonFolderPath); // Create the directory
            }

            // Construct the JSON file path
            string jsonFilePath = Path.Combine(jsonFolderPath, "FLM.json");

            // Check if the JSON file exists
            if (!File.Exists(jsonFilePath))
            {
                // Calculate ExpirationDate based on ExperienceDays
                DateTime launchDate = DateTime.Today; // Change this to your actual launch date
                int experienceDays = 30; // Change this to the desired number of experience days
                DateTime expirationDate = launchDate.AddDays(experienceDays);

                // Create new JSON data
                var jsonData = new
                {
                    ExperienceDays = experienceDays,
                    ExpirationDate = expirationDate.ToString("yyyy-MM-dd"), // Format the date as yyyy-MM-dd
                    LicenseCode = "Fighting-Feather",
                    LicenseKey = "Fighting-Feather",
                    LicenseType = "FREE TRIAL",
                    LicenseStatus = "FULL",
                    CreatedDate = launchDate.ToString("yyyy-MM-dd") // Format the date as yyyy-MM-dd
                };

                // Append JSON data to the file
                AppendToJsonFile(jsonFilePath, jsonData);

                Console.WriteLine("FLM.json created and initialized with data.");
            }
        }

    }
}
