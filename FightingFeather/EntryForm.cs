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
    public partial class EntryForm : MetroFramework.Forms.MetroForm
    {

        private string databasePath;
        private Main mainForm;
        private int fightId;

        public EntryForm(Main mainForm, string databasePath)
        {
            InitializeComponent();
            this.mainForm = mainForm;
            this.databasePath = databasePath;

            // Populate the ComboBox with options
            comboBox_Winner.Items.AddRange(new object[] { "M", "W", "Cancel", "Draw" });
            comboBox_Rate.Items.AddRange(new object[] { "8/10", "3/4", "7/10", "None" });

            // Attach event handlers to calculate bet difference when bet values change
            textBox_MeronBet.TextChanged += CalculateBetDifference;
            textBox_WalaBet.TextChanged += CalculateBetDifference;

        }


        // Define an event to notify about changes
        public event EventHandler DataChanged;

        // Call this method when changes occur
        private void OnDataChanged(EventArgs e)
        {
            DataChanged?.Invoke(this, e);
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
                textBox_BetDiff.Text = "Invalid Input";
            }
        }

        public decimal CalculateFee(decimal meronBet, decimal walaBet, string winner, decimal pago)
        {
            decimal fee = 0;

            if (winner == "M")
            {
                decimal sum = pago + walaBet; // Calculate the sum of pago and meronWala
                fee = sum * 0.10m; // Calculate 10% of the sum
            }

            else if (winner == "W")
            {
                fee = walaBet * 0.10m;
            }
            else if (winner == "Cancel" || winner == "Draw")
            {
                fee = 0;
            }

            return fee;
        }

        // Modify the LoadDataForEditing method in EntryForm class
        public void LoadDataForEditing(int fightId, string meronName, int meronBet, string walaName, int walaBet, int pago, string winner, string rate, int rateamount)
        {
            // Populate the form fields with the provided data for editing
            textBox_MeronName.Text = meronName;
            textBox_MeronBet.Text = meronBet.ToString();
            textBox_WalaName.Text = walaName;
            textBox_WalaBet.Text = walaBet.ToString();
            textBox_Pago.Text = pago.ToString();
            comboBox_Winner.Text = winner.ToString();
            comboBox_Rate.Text = rate.ToString();
            textBox_EnterAmountRate.Text = rateamount.ToString();

            // Check if rateamount is 0, display "-" instead
            textBox_EnterAmountRate.Text = rateamount == 0 ? "-" : rateamount.ToString();


            // Store the fightId for updating
            this.fightId = fightId;
        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            // Check if the required fields are empty
            if (string.IsNullOrWhiteSpace(textBox_MeronName.Text) ||
                string.IsNullOrWhiteSpace(textBox_WalaName.Text) ||
                string.IsNullOrWhiteSpace(textBox_WalaBet.Text) ||
                string.IsNullOrWhiteSpace(textBox_MeronBet.Text))
            {
                MessageBox.Show("Please fill in all required fields!");
                return;
            }

            // Retrieve data from your EntryForm controls
            string meronName = textBox_MeronName.Text;
            int meronBet = string.IsNullOrEmpty(textBox_MeronBet.Text) ? 0 : Convert.ToInt32(textBox_MeronBet.Text);
            string walaName = textBox_WalaName.Text;
            int walaBet = string.IsNullOrEmpty(textBox_WalaBet.Text) ? 0 : Convert.ToInt32(textBox_WalaBet.Text);
            int pago = string.IsNullOrEmpty(textBox_Pago.Text) ? 0 : Convert.ToInt32(textBox_Pago.Text);
            string winner = comboBox_Winner.SelectedItem?.ToString();
            string rateamount = textBox_EnterAmountRate.Text;
            string rate = comboBox_Rate.SelectedItem?.ToString();


            // Convert rateamount to an integer
            int rateAmountValue = 0;
            if (rateamount != "-" && !string.IsNullOrEmpty(rateamount))
            {
                if (!int.TryParse(rateamount, out rateAmountValue))
                {
                    MessageBox.Show("Invalid rate amount.");
                    return;
                }
            }

            // Calculate the initial bet difference
            int initialBetDiff = meronBet - walaBet;

            // Calculate the RATE_EARNINGS based on RATE_AMOUNT and comboBox_Rate if the winner is "M"

            decimal rateResult = 0;

            if (winner == "M" && rate != "None" && rateAmountValue > 0)
            {
                switch (rate)
                {
                    case "8/10":
                        rateResult = rateAmountValue * 8 / 10;
                        break;
                    case "3/4":
                        rateResult = rateAmountValue * 3 / 4;
                        break;
                    case "7/10":
                        rateResult = rateAmountValue * 7 / 10;
                        break;
                    default:
                        MessageBox.Show("Invalid rate selection.");
                        return;
                }
            }
            else
            {
                // If the winner is not "M", set the rateResult to 0
                rateResult = 0;
            }

            // Calculate the fee based on the bets and winner
            decimal fee = CalculateFee(meronBet, walaBet, winner, pago);

            // Determine if fee is 0, then adjust totalPlasada accordingly
            decimal totalPlasada = fee != 0 ? fee + 300 : 0;

            // Calculate the PAREHAS value based on the formula
            int parehas = walaBet + pago;

            // Calculate the winner's earnings based on the provided logic
            decimal winnersEarning = 0;

            if (winner == "M")
            {
                winnersEarning = parehas - totalPlasada + rateResult;
            }
            else if (winner == "W")
            {
                winnersEarning = walaBet - totalPlasada;
            }

            // Save data to the database
            using (var connection = new SQLiteConnection($"Data Source={databasePath}"))
            {
                connection.Open();
                string insertQuery = "INSERT INTO PLASADA (MERON, [BET (M)], WALA, [BET (W)], [INITIAL BET DIFF], PAGO, WINNER, RATE, [RATE AMOUNT], [RATE EARNINGS], [WINNERS EARN])" +
                                     "VALUES (@MeronName, @MeronBet, @WalaName, @WalaBet, @InitialBetDiff, @Pago, @Winner, @Rate, @RateAmount, @RateResult, @WinnersEarning)";

                using (var command = new SQLiteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@MeronName", meronName);
                    command.Parameters.AddWithValue("@MeronBet", meronBet);
                    command.Parameters.AddWithValue("@WalaName", walaName);
                    command.Parameters.AddWithValue("@WalaBet", walaBet);
                    command.Parameters.AddWithValue("@InitialBetDiff", initialBetDiff);
                    command.Parameters.AddWithValue("@Pago", pago);
                    command.Parameters.AddWithValue("@Winner", string.IsNullOrEmpty(winner) ? DBNull.Value : (object)winner); // Handle null or empty values
                    command.Parameters.AddWithValue("@Rate", string.IsNullOrEmpty(rate) ? DBNull.Value : (object)rate); // Handle null or empty values
                    command.Parameters.AddWithValue("@RateAmount", rateAmountValue == 0 ? DBNull.Value : (object)rateAmountValue); // Handle 0 or empty rateAmount
                    command.Parameters.AddWithValue("@RateResult", Math.Round(rateResult, 2)); // Ensure rate result is rounded to two decimal places
                    command.Parameters.AddWithValue("@WinnersEarning", winnersEarning);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Data saved successfully!");

            mainForm.RefreshGrid();
            OnDataChanged(EventArgs.Empty);

            // Optionally, close the EntryForm after saving
            this.Close();
        }

        private void button_Update_Click(object sender, EventArgs e)
        {
            // Check if the required fields are empty
            if (string.IsNullOrWhiteSpace(textBox_MeronName.Text) ||
                string.IsNullOrWhiteSpace(textBox_WalaName.Text) ||
                string.IsNullOrWhiteSpace(textBox_WalaBet.Text) ||
                string.IsNullOrWhiteSpace(textBox_MeronBet.Text))
            {
                MessageBox.Show("Please fill in all required fields.");
                return;
            }

            // Retrieve data from your EntryForm controls
            string meronName = textBox_MeronName.Text;
            int meronBet = string.IsNullOrEmpty(textBox_MeronBet.Text) ? 0 : Convert.ToInt32(textBox_MeronBet.Text);
            string walaName = textBox_WalaName.Text;
            int walaBet = string.IsNullOrEmpty(textBox_WalaBet.Text) ? 0 : Convert.ToInt32(textBox_WalaBet.Text);
            int pago = string.IsNullOrEmpty(textBox_Pago.Text) ? 0 : Convert.ToInt32(textBox_Pago.Text);
            string winner = comboBox_Winner.SelectedItem?.ToString();
            string rateamount = textBox_EnterAmountRate.Text;
            string rate = comboBox_Rate.SelectedItem?.ToString();

            // Convert rateamount to an integer
            int rateAmountValue = 0;
            if (rateamount != "-")
            {
                if (!int.TryParse(rateamount, out rateAmountValue))
                {
                    MessageBox.Show("Invalid rate amount.");
                    return;
                }
            }

            // Calculate the initial bet difference
            int initialBetDiff = meronBet - walaBet;

            // Calculate the RATE_EARNINGS based on RATE_AMOUNT and comboBox_Rate if the winner is "M"
            decimal rateResult = 0;
            if (winner == "M" && rate != "None" && rateAmountValue > 0)
            {
                switch (rate)
                {
                    case "8/10":
                        rateResult = rateAmountValue * 8 / 10;
                        break;
                    case "3/4":
                        rateResult = rateAmountValue * 3 / 4;
                        break;
                    case "7/10":
                        rateResult = rateAmountValue * 7 / 10;
                        break;
                    default:
                        MessageBox.Show("Invalid rate selection.");
                        return;
                }
            }
            else
            {
                // If the winner is not "M", set the rateResult to 0
                rateResult = 0;
            }

            // Calculate the fee based on the bets and winner
            decimal fee = CalculateFee(meronBet, walaBet, winner, pago);

            // Determine if fee is 0, then adjust totalPlasada accordingly
            decimal totalPlasada = fee != 0 ? fee + 300 : 0;

            // Calculate the PAREHAS value based on the formula
            int parehas = walaBet + pago;

            // Calculate the winner's earnings based on the provided logic
            decimal winnersEarning = 0;

            if (winner == "M")
            {
                winnersEarning = parehas - totalPlasada + rateResult;
            }
            else if (winner == "W")
            {
                winnersEarning = walaBet - totalPlasada;
            }


            // Update data in the database
            using (var connection = new SQLiteConnection($"Data Source={databasePath}"))
            {
                connection.Open();
                string updateQuery = "UPDATE PLASADA SET MERON = @MeronName, [BET (M)] = @MeronBet, WALA = @WalaName, [BET (W)] = @WalaBet, [INITIAL BET DIFF] = @InitialBetDiff, PAGO = @Pago, WINNER = @Winner, RATE = @Rate, [RATE AMOUNT] = @RateAmount, [RATE EARNINGS] = @RateResult, [WINNERS EARN] = @WinnersEarning WHERE FIGHT = @FightId";
                using (var command = new SQLiteCommand(updateQuery, connection))
                {
                    command.Parameters.AddWithValue("@MeronName", meronName);
                    command.Parameters.AddWithValue("@MeronBet", meronBet);
                    command.Parameters.AddWithValue("@WalaName", walaName);
                    command.Parameters.AddWithValue("@WalaBet", walaBet);
                    command.Parameters.AddWithValue("@InitialBetDiff", initialBetDiff);
                    command.Parameters.AddWithValue("@FightId", fightId);
                    command.Parameters.AddWithValue("@Pago", pago);
                    command.Parameters.AddWithValue("@Winner", winner);
                    command.Parameters.AddWithValue("@Rate", rate);
                    command.Parameters.AddWithValue("@RateAmount", rateAmountValue);
                    command.Parameters.AddWithValue("@RateResult", Math.Round(rateResult, 2)); // Ensure rate result is rounded to two decimal places
                    command.Parameters.AddWithValue("@WinnersEarning", winnersEarning);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Data updated successfully!");

            mainForm.RefreshGrid();
            OnDataChanged(EventArgs.Empty);

            // Close the EntryForm after updating
            this.Close();
        }

        //Toggle the Visibility of button_Save
        public void ToggleSaveButtonVisibility(bool isVisible)
        {
            button_Save.Visible = isVisible;

        }

        //Toggle the Visibility of button_Update
        public void ToggleUpdateButtonVisibility(bool isVisible)
        {
            button_Update.Visible = isVisible;

        }

    }
}
