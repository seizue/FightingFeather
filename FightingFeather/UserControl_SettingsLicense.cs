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
            LoadLicense();

            // Check if license was previously activated
            if (Properties.Settings.Default.IsLicenseActivated)
            {
                // Show license details and make text boxes read-only
                labelExpiry.Visible = true;
                labelStatus.Visible = true;
                labelDaysLeft.Visible = true;
               
            }
        }


        private void LoadLicense()
        {
            try
            {
                // Open connection to SQLite database
                using (sqliteConnection = new SQLiteConnection(connectionString))
                {
                    sqliteConnection.Open();

                    // Define SQL command to retrieve data from the table
                    string sql = "SELECT ExpirationDate, LicenseStatus, LicenseCode, LicenseKey FROM FLM";

                    // Create command object
                    using (SQLiteCommand command = new SQLiteCommand(sql, sqliteConnection))
                    {
                        // Execute the command and get the result
                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            // Check if there are rows returned
                            if (reader.Read())
                            {
                                // Get the values from the columns
                                string expirationDateString = reader["ExpirationDate"].ToString();
                                string licenseStatus = reader["LicenseStatus"].ToString();
                                string licenseCode = reader["LicenseCode"].ToString();
                                string licenseKey = reader["LicenseKey"].ToString();

                                // Parse expiration date from string
                                DateTime expirationDate = DateTime.Parse(expirationDateString);

                                // Calculate remaining days
                                TimeSpan remainingTime = expirationDate - DateTime.Now;
                                int remainingDays = (int)Math.Ceiling(remainingTime.TotalDays);

                                // Update the labels with the retrieved data
                                labelExpiry.Text = expirationDate.ToShortDateString();
                                labelStatus.Text = licenseStatus;
                                labelDaysLeft.Text = $"{remainingDays}";

                                // Save activation status to settings
                                Properties.Settings.Default.IsLicenseActivated = true;
                                Properties.Settings.Default.Save(); // Save the settings

                            }
                            else
                            {
                                // Handle case where no rows are returned
                                labelExpiry.Text = "No data found";
                                labelStatus.Text = "No data found";
                                labelDaysLeft.Text = "No data found";

                                // Hide Label texts
                                labelExpiry.Visible = false;
                                labelStatus.Visible = false;
                                labelDaysLeft.Visible = false;

                                // Set license activation status to false
                                Properties.Settings.Default.IsLicenseActivated = false;
                                Properties.Settings.Default.Save(); // Save the settings
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show("Error: " + ex.Message);
            }
        }

    }
}
