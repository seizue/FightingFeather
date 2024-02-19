using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.IO;
using System.Data.SQLite;

namespace FightingFeather
{
    public partial class UserControl_Earnings : UserControl
    {
        private const string DatabaseFileName = "dofox.db";
        private string databasePath;

        public UserControl_Earnings()
        {
            InitializeComponent();

            // Set the database path
            string currentDirectory = Directory.GetCurrentDirectory();
            databasePath = Path.Combine(currentDirectory, DatabaseFileName);

            // Subscribe to the CellFormatting event
            GridPlasada_Earnings.CellFormatting += GridPlasada_Earnings_CellFormatting;

          
        }

        private void RefreshGrid()
        {
            using (var connection = new SQLiteConnection($"Data Source={databasePath}"))
            {
                connection.Open();
                string selectQuery = "SELECT FIGHT, WINNER, MERON, WALA, PAREHAS, [BET (M)], [BET (W)], LOGRO, FEE, [TOTAL PLASADA], [RATE EARNINGS], [WINNERS EARN] FROM PLASADA";

                using (var command = new SQLiteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        GridPlasada_Earnings.Rows.Clear();

                        while (reader.Read())
                        {
                            string winnerName = string.Empty;
                            int winnerBet = 0;
                            string winnerChoice = reader["WINNER"].ToString();

                            switch (winnerChoice)
                            {
                                case "M":
                                    winnerName = reader["MERON"].ToString();
                                    winnerBet = Convert.ToInt32(reader["BET (M)"]);
                                    break;
                                case "W":
                                    winnerName = reader["WALA"].ToString();
                                    winnerBet = Convert.ToInt32(reader["BET (W)"]);
                                    break;
                                case "Cancel":
                                case "None":
                                    winnerName = winnerChoice;
                                    winnerBet = 0;
                                    break;
                            }

                            DataGridViewRow row = new DataGridViewRow();
                            row.CreateCells(GridPlasada_Earnings,
                                reader["FIGHT"], winnerName, winnerBet, reader["PAREHAS"], reader["LOGRO"], reader["FEE"],
                                reader["TOTAL PLASADA"], reader["RATE EARNINGS"], reader["WINNERS EARN"]);

                            // Set the background color based on the winner choice only for specific columns
                            Color cellColor = winnerChoice == "M" ? Color.FromArgb(239, 253, 244) : winnerChoice == "W" ? Color.FromArgb(255, 243, 245) : Color.White;

                            // Set background color for specific columns (winner name, winner bet, and winner earn)
                            row.Cells[1].Style.BackColor = cellColor; // Winner name column
                            row.Cells[8].Style.BackColor = cellColor; // Winner earn column

                            GridPlasada_Earnings.Rows.Add(row);
                        }
                    }
                }
            }
        }

        private void GridPlasada_Earnings_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.Value != null && e.Value.ToString() == "0")
            {
                e.Value = "-";
            }

            // Check if the cell belongs to the "PAREHAS" column and if it's not a header cell
            if (e.ColumnIndex >= 0 && GridPlasada_Earnings.Columns[e.ColumnIndex].Name == "PAREHAS" && e.RowIndex >= 0)
            {
                // Set the font color for cells in the "PAREHASF" column
                e.CellStyle.ForeColor = Color.FromArgb(99, 66, 0);
            }

            // Change the foreground color for the "RATE EARNINGS" column
            if (e.ColumnIndex >= 0 && GridPlasada_Earnings.Columns[e.ColumnIndex].Name == "RATE_EARNINGS" && e.RowIndex >= 0)
            {
                // Set the foreground color for "RATE EARNINGS" cells
                e.CellStyle.ForeColor = Color.FromArgb(153, 105, 28);
            }

            // Change the foreground color for the WINNERS EARN" column
            if (e.ColumnIndex >= 0 && GridPlasada_Earnings.Columns[e.ColumnIndex].Name == "WINNERS_EARN" && e.RowIndex >= 0)
            {
                // Set the foreground color for "WINNERS EARN" cells
                e.CellStyle.ForeColor = Color.Maroon;
            }

        }

        private void GridPlasada_Earnings_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex >= 0 && e.RowIndex >= 0 && GridPlasada_Earnings.Columns[e.ColumnIndex].HeaderText == "RATE EARNINGS")
            {
                e.Paint(e.CellBounds, DataGridViewPaintParts.All & ~DataGridViewPaintParts.Border);

                // Define the custom color for the divider
                Color dividerColor = Color.FromArgb(236, 237, 240); // Change this to your desired color

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
