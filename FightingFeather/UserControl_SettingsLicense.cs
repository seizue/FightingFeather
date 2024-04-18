using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using Newtonsoft.Json;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace FightingFeather
{
    public partial class UserControl_SettingsLicense : UserControl
    {
     
        public UserControl_SettingsLicense()
        {
            InitializeComponent();
            LoadLicense();
        }

        public class LicenseData
        {
            [JsonProperty("ExpirationDate")]
            public DateTime ExpirationDate { get; set; }

            [JsonProperty("LicenseStatus")]
            public string LicenseStatus { get; set; }

            [JsonProperty("LicenseCode")]
            public string LicenseCode { get; set; }

            [JsonProperty("LicenseKey")]
            public string LicenseKey { get; set; }

            [JsonProperty("LicenseType")]
            public string LicenseType { get; set; }
        }
        private void LoadLicense()
        {
            try
            {
                // Get the JSON folder path
                string jsonFolderPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Fighting Feather", "JSON", "FLM");
                Console.WriteLine($"JSON File Path: {jsonFolderPath}");

                // Construct the JSON file path
                string jsonFilePath = Path.Combine(jsonFolderPath, "FLM.json");

                // Check if the JSON file exists
                if (!File.Exists(jsonFilePath))
                {
                    MessageBox.Show("FLM.json cannot be found.");
                    return;
                }

                // Load JSON data from file
                string jsonData = File.ReadAllText(jsonFilePath);

                // Deserialize JSON array into a collection of LicenseData objects
                List<LicenseData> licenseDataList = JsonConvert.DeserializeObject<List<LicenseData>>(jsonData);

                // Find the first non-expired license
                LicenseData activeLicense = licenseDataList.FirstOrDefault(license => license.ExpirationDate >= DateTime.Now);

                if (activeLicense != null)
                {
                    // Calculate the number of days left until expiration
                    TimeSpan timeUntilExpiration = activeLicense.ExpirationDate - DateTime.Now;
                    int daysLeft = (int)timeUntilExpiration.TotalDays;

                    // Display the number of days left
                    labelDaysLeft.Text = $"{daysLeft}";

                    // Display the expiration date
                    labelExpiry.Text = activeLicense.ExpirationDate.ToString("MMMM dd, yyyy");

                    // Display the license type
                    labelType.Text = activeLicense.LicenseType;

                    // Update labelStatus based on the number of days left
                    if (daysLeft > 5)
                    {
                        labelStatus.Text = "ACTIVATED";
                        labelStatus.ForeColor = Color.MediumSeaGreen;
                    }
                    else if (daysLeft >= 0)
                    {
                        labelStatus.Text = "NEAR EXPIRY";
                        labelStatus.ForeColor = Color.DarkGoldenrod;
                    }
                    else
                    {
                        labelStatus.Text = "EXPIRED";
                        labelStatus.ForeColor = Color.Salmon;
                    }
                }
                else if (licenseDataList.Any())
                {
                    // No active license found, display the last expired license
                    LicenseData lastExpiredLicense = licenseDataList.OrderBy(license => license.ExpirationDate).Last();
                    labelExpiry.Text = lastExpiredLicense.ExpirationDate.ToString("MMMM dd, yyyy");
                    labelType.Text = lastExpiredLicense.LicenseType;
                    labelStatus.Text = "EXPIRED";
                    labelStatus.ForeColor = Color.Salmon;
                    labelDaysLeft.Text = "0"; // Assuming the license has expired
                }
                else
                {
                    MessageBox.Show("No license data found.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading license data: {ex.Message}");
            }
        }

    }

}

