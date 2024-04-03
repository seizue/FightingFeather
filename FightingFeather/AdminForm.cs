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
        }


        private void label_RegisterUsers_Click(object sender, EventArgs e)
        {
            label_RegisterUsers.ForeColor = Color.FromArgb(193, 84, 55);
            label_AdminSettings.ForeColor = Color.FromArgb(64, 64, 64);
            panel_Indicator.Location = new Point(260, 66);
            panel_Indicator.Size = new Size(158, 4);
        }

        private void label_AdminSettings_Click(object sender, EventArgs e)
        {
            label_AdminSettings.ForeColor = Color.FromArgb(193, 84, 55);
            label_RegisterUsers.ForeColor = Color.FromArgb(64, 64, 64);
            panel_Indicator.Location = new Point(457, 66);
            panel_Indicator.Size = new Size(120, 4);
        }

        private void button_AddNewUser_Click(object sender, EventArgs e)
        {
            panel_BGAddNewUser.Visible = true;
         
        }

        private void button_CloseRegForm_Click(object sender, EventArgs e)
        {
            panel_BGAddNewUser.Visible=false;
        }
    }
}
