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

    public partial class SettingsForm : MetroFramework.Forms.MetroForm
    {
        private Color defaultColor = Color.FromArgb(0, 0, 42); // Default color 
        private Color clickedColor = Color.FromArgb(193, 84, 55); // Color when the button is clicked

        public SettingsForm()
        {
            InitializeComponent();
            // Populate the ComboBox with options
            comBox_WindowState.Items.AddRange(new object[] { "Normal", "Maximized" });

            metroToggle_Reminder.Checked = Properties.Settings.Default.ReminderEnabled;

            // Set the selected item of the ComboBox based on the saved value
            string savedWindowState = Properties.Settings.Default.MainFormWindowState;
            if (!string.IsNullOrEmpty(savedWindowState))
            {
                comBox_WindowState.SelectedItem = savedWindowState;
            }

        }

        public event EventHandler<bool> ReminderToggleChanged;
        private void metroToggle_Reminder_CheckedChanged(object sender, EventArgs e)
        {
            // Raise the event to notify subscribers about the change in toggle state
            ReminderToggleChanged?.Invoke(this, metroToggle_Reminder.Checked);

            // Save the toggle state to application settings
            Properties.Settings.Default.ReminderEnabled = metroToggle_Reminder.Checked;
            Properties.Settings.Default.Save();

        }


        private void button_Save_Click(object sender, EventArgs e)
        {  
            // Save the selected item of the ComboBox to application settings if it's valid
            if (comBox_WindowState.SelectedItem != null && Enum.IsDefined(typeof(FormWindowState), comBox_WindowState.SelectedItem.ToString()))
            {
                Properties.Settings.Default.MainFormWindowState = comBox_WindowState.SelectedItem.ToString();
                Properties.Settings.Default.Save();

                // Inform the user that changes will reflect after application restart
                MessageBox.Show("Settings saved. Please close and restart the application for the changes to take effect.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // Handle invalid state gracefully, for example, by showing a message to the user
                MessageBox.Show("Invalid window state selected. Please select a valid window state.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Close the settings form or perform any other necessary actions
            this.Close();
        }


        private void button_General_Click(object sender, EventArgs e)
        {
            button_General.ForeColor = clickedColor;
            button_License.ForeColor = defaultColor;
            button_Help.ForeColor = defaultColor;
        }


        private void button_License_Click(object sender, EventArgs e)
        {
            button_License.ForeColor = clickedColor;
            button_General.ForeColor = defaultColor;
            button_Help.ForeColor = defaultColor;
        }
    }
}
