using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FightingFeather
{
    public partial class UserControl_Shortcut : UserControl
    {
      
        public UserControl_Shortcut()
        {
            InitializeComponent();
            PopulateGridPlasadaShortcut();
            
            // Attach event handlers to calculate bet difference when bet values change
            textBox_MeronBet.TextChanged += CalculateBetDifference;
            textBox_WalaBet.TextChanged += CalculateBetDifference;
            textBox_Pago.TextChanged += CalculateParehas; 


            // Populate the ComboBox with options
            comboBox_Winner.Items.AddRange(new object[] { "M", "W", "Cancel", "Draw" });
            comboBox_Rate.Items.AddRange(new object[] { "8/10", "3/4", "7/10", "N/A" });
        }

        private void CalculateBetDifference(object sender, EventArgs e)
        {

            // Ensure both text boxes have valid integer values
            int meronBet, walaBet;
            if (int.TryParse(textBox_MeronBet.Text, out meronBet) && int.TryParse(textBox_WalaBet.Text, out walaBet))
            {
                // Calculate the bet difference
                int betDiff = meronBet - walaBet;
                textBox_BetDiff.Text = betDiff.ToString();
            }
            else
            {
                // Handle invalid input or non-integer values
                textBox_BetDiff.Text = "-";
            }
        }
        private void CalculateParehas(object sender, EventArgs e)
        {
            // Define variables to store the parsed values
            int walaBet, pago;

            // Parse the values from the text boxes
            if (int.TryParse(textBox_WalaBet.Text, out walaBet) && int.TryParse(textBox_Pago.Text, out pago))
            {
                // Perform the addition
                int parehas = walaBet + pago;

                // Update the textBox_Parehas with the calculated result
                textBox_Parehas.Text = parehas.ToString();
            }
            else
            {
                // Handle cases where parsing fails (e.g., invalid input)
                // Handle invalid input or non-integer values
                textBox_Parehas.Text = "-";
            }
        }





        private bool ValidateInput()
        {
            // Check if the required fields are filled and if the input data is valid
            if (string.IsNullOrWhiteSpace(textBox_MeronBet.Text) ||
                string.IsNullOrWhiteSpace(textBox_WalaBet.Text) ||
                string.IsNullOrWhiteSpace(textBox_MeronName.Text) ||
                string.IsNullOrWhiteSpace(textBox_WalaName.Text))
            {
                return false;
            }

            // Additional validation logic can be added here as per your requirements

            return true;
        }

        private void SaveDataToDatabase()
        {
            try
            {
                // Example connection string for SQLite
                string connectionString = "Data Source=dofox.db;Version=3;";

                // Create an SQLite connection object
                SQLiteConnection connection = new SQLiteConnection(connectionString);
                {
                    connection.Open();

                    // Define the SQL command to insert data into the database
                    string insertQuery = "INSERT INTO PLASADA (MERON, [BET (M)], WALA, [BET (W)], [INITIAL BET DIFF], PAGO, WINNER, RATE)" +
                                         "VALUES (@MeronName, @MeronBet, @WalaName, @WalaBet, @InitialBetDiff, @Pago, @Winner, @Rate)";

                    // Create a SQLiteCommand object
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        // Set parameter values
                        command.Parameters.AddWithValue("@MeronName", textBox_MeronName.Text);
                        command.Parameters.AddWithValue("@MeronBet", textBox_MeronBet.Text);
                        command.Parameters.AddWithValue("@WalaName", textBox_WalaName.Text);
                        command.Parameters.AddWithValue("@WalaBet", textBox_WalaBet.Text);
                        command.Parameters.AddWithValue("@InitialBetDiff", textBox_BetDiff.Text); 
                        command.Parameters.AddWithValue("@Pago", textBox_Pago.Text); 
                        command.Parameters.AddWithValue("@Winner", comboBox_Winner.SelectedItem); 
                        command.Parameters.AddWithValue("@Rate", comboBox_Rate.SelectedItem);

                        // Execute the command
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Data saved successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button_Enter_Click(object sender, EventArgs e)
        {
            // Validate user input
            if (ValidateInput())
            {
                // Save data to the database if input is valid
                SaveDataToDatabase();
            }
            else
            {
                MessageBox.Show("Please fill in all required fields!");
            }
        }

        private void PopulateGridPlasadaShortcut()
        {
          
        }

    }

}
