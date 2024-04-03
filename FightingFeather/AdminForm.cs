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
    public partial class AdminForm : MetroFramework.Forms.MetroForm
    {
        public AdminForm()
        {
            InitializeComponent();

            // Set raDateTimePicker1 to show the current date
            raDateTimePicker1.Value = DateTime.Today;

            // Populate the ComboBox with options
            comboBox_Status.Items.AddRange(new object[] { "ACTIVE", "SUSPENDED" });
        }


        private void label_RegisterUsers_Click(object sender, EventArgs e)
        {
            label_RegisterUsers.ForeColor = Color.FromArgb(193, 84, 55);
            label_AdminSettings.ForeColor = Color.FromArgb(64, 64, 64);
            panel_Indicator.Location = new Point(260, 66);
            panel_Indicator.Size = new Size(65, 4);
        }

        private void label_AdminSettings_Click(object sender, EventArgs e)
        {
            label_AdminSettings.ForeColor = Color.FromArgb(193, 84, 55);
            label_RegisterUsers.ForeColor = Color.FromArgb(64, 64, 64);
            panel_Indicator.Location = new Point(375, 66);
            panel_Indicator.Size = new Size(121, 4);
        }

        private void button_AddNewUser_Click(object sender, EventArgs e)
        {
            panel_BGAddNewUser.Visible = true;
            label_AdminSettings.Enabled = false;
            button_License.Enabled = false;
        }

        private void button_CloseRegForm_Click(object sender, EventArgs e)
        {
            panel_BGAddNewUser.Visible=false;
            label_AdminSettings.Enabled = true;
            button_License.Enabled = true;
        }

        private void button_UpdateUser_Click(object sender, EventArgs e)
        {
            panel_BGAddNewUser.Visible = true;
            label_AdminSettings.Enabled = false;
            button_License.Enabled = false;
            label_Dis.Text = "UPDATE FORM";
        }

        private void button_SaveNewUser_Click(object sender, EventArgs e)
        {

        }

        private void button_ClearFields_Click(object sender, EventArgs e)
        {

        }
    }
}
