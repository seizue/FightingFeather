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
    public partial class UserControl_Summa : UserControl
    {
        public UserControl_Summa()
        {
            InitializeComponent();

            foreach (DataGridViewRow row in GridPlasada_Summary.Rows)
            {
                row.Height = 28;
            }
        }
    }
}
