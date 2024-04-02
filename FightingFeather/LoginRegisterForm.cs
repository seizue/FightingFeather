using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FightingFeather
{
    public partial class LoginRegisterForm :MetroFramework.Forms.MetroForm
    {
        public LoginRegisterForm()
        {
            InitializeComponent();
        }

        private void label_Login_Click(object sender, EventArgs e)
        {
            label_Login.ForeColor = Color.FromArgb(13, 64, 113);
            label_Help.ForeColor = Color.DimGray;
            panel_Indicator.Location = new System.Drawing.Point(28, 50);
        }

        private void label_Help_Click(object sender, EventArgs e)
        {
            label_Help.ForeColor = Color.FromArgb(13, 64, 113);
            label_Login.ForeColor = Color.DimGray;
            panel_Indicator.Location = new System.Drawing.Point(127, 51);
        }

        private void textBox_Search_ButtonClick(object sender, EventArgs e)
        {
            textBox_Username.Text = "";
        }

        private void textBox_Password_ButtonClick(object sender, EventArgs e)
        {
            textBox_Password.Text = "";
        }
    }
}
