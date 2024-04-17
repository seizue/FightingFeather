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
    public partial class UserControl_SettingsLicense : UserControl
    {
        private SQLiteConnection sqliteConnection;
        private string databaseName = "FFkey.db";
        private string connectionString;
        public UserControl_SettingsLicense()
        {
            InitializeComponent();
            connectionString = $"Data Source={databaseName};Version=3;";
         

            // Check if license was previously activated
            if (Properties.Settings.Default.IsLicenseActivated)
            {
                // Show license details and make text boxes read-only
                labelExpiry.Visible = true;
                labelStatus.Visible = true;
                labelDaysLeft.Visible = true;
                labelType.Visible = true;

            }
        }

        private void LoadLicense()
        {
            try
            {
                bool validLicenseFound = false; // Flag to track if a valid license is found

                // Open connection to SQLite database
                using (sqliteConnection = new SQLiteConnection(connectionString))
                {
                    sqliteConnection.Open();

                    // Define SQL command to retrieve data from the table
                    string sql = "SELECT ExpirationDate, LicenseStatus, LicenseCode, LicenseKey, LicenseType FROM FLM";

                    // Create command object
                    using (SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection))
                    {
                        // Execute the command and get the result
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // Check if there are rows returned
                            while (reader.Read())
                            {
                                // Get the values from the columns
                                string expirationDateString = reader["ExpirationDate"].ToString();
                                string licenseStatus = reader["LicenseStatus"].ToString();
                                string licenseCode = reader["LicenseCode"].ToString();
                                string licenseKey = reader["LicenseKey"].ToString();
                                string licenseType = reader["LicenseType"].ToString(); // Get license type

                                // Parse expiration date from string
                                DateTime expirationDate = DateTime.Parse(expirationDateString);

                                // Check if the license is still valid
                                if (expirationDate > DateTime.Now)
                                {
                                    // Calculate remaining days
                                    TimeSpan remainingTime = expirationDate - DateTime.Now;
                                    int remainingDays = (int)Math.Ceiling(remainingTime.TotalDays);

                                    // Update the labels with the retrieved data
                                    labelExpiry.Text = expirationDate.ToShortDateString();
                                    labelStatus.Text = licenseStatus;
                                    labelDaysLeft.Text = $"{remainingDays}";
                                    labelType.Text = licenseType; // Set the license type label

                                    // Save activation status to settings
                                    Properties.Settings.Default.IsLicenseActivated = true;
                                    Properties.Settings.Default.Save(); // Save the settings

                                    validLicenseFound = true; // Set flag indicating a valid license is found

                                    // Exit the loop after loading the first valid license
                                    break;
                                }
                                else
                                {
                                    // If the license is expired, update labelStatus and its forecolor
                                    labelStatus.Text = "Expired";
                                    labelStatus.ForeColor = Color.Salmon;
                                }
                            }
                        }
                    }
                }

                // If no valid license is found, display a message box
                if (!validLicenseFound)
                {
                    MessageBox.Show("License expired."); // Show message box indicating license expiration
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show("Error: " + ex.Message);
            }
                // Change forecolor if license is activated
            if (Properties.Settings.Default.IsLicenseActivated)
            {
                labelStatus.ForeColor = Color.MediumSeaGreen;
            }
        }


        private void UserControl_SettingsLicense_Load(object sender, EventArgs e)
        {
            LoadLicense();

        }
    }
}
