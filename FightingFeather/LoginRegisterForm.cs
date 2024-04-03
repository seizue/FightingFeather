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
        private Main mainForm;
        public LoginRegisterForm(Main mainForm)
        {
            InitializeComponent();
            this.mainForm = mainForm;
        }

        private void label_Login_Click(object sender, EventArgs e)
        {
            label_Login.ForeColor = Color.FromArgb(13, 64, 113);
            label_Help.ForeColor = Color.DimGray;
            panel_Indicator.Location = new System.Drawing.Point(29, 50);
            panel_Help.Visible = false;
        }

        private void label_Help_Click(object sender, EventArgs e)
        {
            label_Help.ForeColor = Color.FromArgb(13, 64, 113);
            label_Login.ForeColor = Color.DimGray;
            panel_Indicator.Location = new System.Drawing.Point(127, 51);
            panel_Help.Visible = true;
        }

        private void textBox_Search_ButtonClick(object sender, EventArgs e)
        {
            textBox_Username.Text = "";
        }

        private void textBox_Password_ButtonClick(object sender, EventArgs e)
        {
            textBox_Password.Text = "";
        }

        private void button_Login_Click(object sender, EventArgs e)
        {
            // Retrieve username and password input
            string username = textBox_Username.Text;
            string password = textBox_Password.Text;

            // Check if username and password are correct (for demonstration purposes, using hardcoded values)
            if (username == "admin" && password == "888534admin17__!&")
            {
                // Successful login
                MessageBox.Show("Login successful!");

                // Update visibility of Admin button in MainForm
                mainForm.UpdateAdminButtonVisibility(true);

                // Close the login form
                this.Close();
            }
            else
            {
                // Failed login
                MessageBox.Show("Invalid username or password. Please try again.");
                // You can optionally clear the input fields to allow the user to retry.
                textBox_Username.Clear();
                textBox_Password.Clear();
                // Set focus back to the username field for convenience.
                textBox_Username.Focus();
            }
        }
    

        private void fogotPass_Link_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Please contact your system administrator if you forgot the password!");
        }
    }
}
