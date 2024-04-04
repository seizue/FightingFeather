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

namespace FightingFeather
{
    public partial class LoginRegisterForm : MetroFramework.Forms.MetroForm
    {
        private Main mainForm;


        public LoginRegisterForm(Main mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
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
            string query = "SELECT COUNT(*) FROM AccountRegistered WHERE Username = @Username AND Password = @Password";

            // Create a connection to the database
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
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

                // Close the login form and pass user type as NormalUser
                this.Close();
                mainForm.LoginComplete(UserType.NormalUser);
            }
            // Check if username and password are correct for admin (SQLite) or using hardcoded credentials
            else if (IsAdminPasswordValid(username, password) ||
                     (username == "admin" && password == "888534Admin__!&"))
            {
                // Successful login for admin
                MessageBox.Show("Admin login successful!");

                // Close the login form and pass user type as Admin
                this.Close();
                mainForm.LoginComplete(UserType.Admin);
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
    }
}
