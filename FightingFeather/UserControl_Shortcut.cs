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
        public event EventHandler SaveButtonClicked;

        public UserControl_Shortcut()
        {
            InitializeComponent();
        }

        private void button_Enter_Click(object sender, EventArgs e)
        {
            // Raise the SaveButtonClicked event
            SaveButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
