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
    public partial class ReminderForm : MetroFramework.Forms.MetroForm
    {
        public ReminderForm(DateTime raDateTimePickerValue)
        {
            InitializeComponent();

            label_Date.Text = raDateTimePickerValue.ToString("yyyy-MM-dd");
            button_Cancel.Click += button_Cancel_Click;
        }

        private void button_Continue_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
