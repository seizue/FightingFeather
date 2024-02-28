using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FightingFeather
{
    public partial class UserControl_Shortcut : UserControl
    {

        public UserControl_Shortcut()
        {
            InitializeComponent();
          

            // Attach event handlers to calculate bet difference when bet values change
            textBox_MeronBet.TextChanged += CalculateBetDifference;
            textBox_MeronWala.TextChanged += CalculateBetDifference;

            // Populate the ComboBox with options
            comboBox_Winner.Items.AddRange(new object[] { "M", "W", "Cancel", "Draw" });
            comboBox_Rate.Items.AddRange(new object[] { "8/10", "3/4", "7/10", "N/A" });
        }

        private void CalculateBetDifference(object sender, EventArgs e)
        {

            // Ensure both text boxes have valid integer values
            int meronBet, walaBet;
            if (int.TryParse(textBox_MeronBet.Text, out meronBet) && int.TryParse(textBox_MeronWala.Text, out walaBet))
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

        private void button_Enter_Click(object sender, EventArgs e)
        { 
        
        }
    }
    
}
