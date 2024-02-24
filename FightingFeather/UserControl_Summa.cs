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
            PopulateAmountColumn();

            GridPlasada_Summary.CellPainting += GridPlasada_Summary_CellPainting;

            foreach (DataGridViewRow row in GridPlasada_Summary.Rows)
            {
                row.Height = 30;
            }
        }


        private void PopulateAmountColumn()
        {
            // Define the values to be added to the "AMOUNT" column
            int[] amounts = { 1, 5, 10, 20, 50, 100, 200, 500, 1000 };

            // Loop through each amount and add it to the "AMOUNT" column
            foreach (int amount in amounts)
            {
                // Add a new row and set the value for the "AMOUNT" column
                int rowIndex = GridPlasada_Summary.Rows.Add();
                GridPlasada_Summary.Rows[rowIndex].Cells["AMOUNT"].Value = amount;
            }
        }

        private void GridPlasada_Summary_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Summary.Columns[e.ColumnIndex].HeaderText == "AMOUNT")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(220, 220, 224); // Change this to your desired color

                // Draw the divider line
                using (Pen dividerPen = new Pen(dividerColor, 3)) // Set the width of the divider
                {
                    // Calculate the position of the divider line
                    int x = e.CellBounds.Right - 1;
                    int y1 = e.CellBounds.Top;
                    int y2 = e.CellBounds.Bottom;

                    e.Graphics.DrawLine(dividerPen, x, y1, x, y2);
                }

                e.Handled = true;
            }


            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Summary.Columns[e.ColumnIndex].HeaderText == "QUANTITY")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(220, 220, 224); // Change this to your desired color

                // Draw the divider line
                using (Pen dividerPen = new Pen(dividerColor, 3)) // Set the width of the divider
                {
                    // Calculate the position of the divider line
                    int x = e.CellBounds.Right - 1;
                    int y1 = e.CellBounds.Top;
                    int y2 = e.CellBounds.Bottom;

                    e.Graphics.DrawLine(dividerPen, x, y1, x, y2);
                }

                e.Handled = true;
            }
        }

    }
}
