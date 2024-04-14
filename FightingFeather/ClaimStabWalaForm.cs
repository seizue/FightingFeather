using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FightingFeather
{
    public partial class ClaimStabWalaForm : MetroFramework.Forms.MetroForm
    {
        public ClaimStabWalaForm(string fight, string wala, string betW)
        {
            InitializeComponent();

            // Set the values to the labels
            labelFight.Text = fight;
            labelWalaName.Text = wala;
            labelWalaBet.Text = betW;
        }

        private void button_PrintReceipt_Click(object sender, EventArgs e)
        {
            PrintPreview();
        }

        private void PrintPreview()
        {
            // Create a new PrintDocument.
            PrintDocument pd = new PrintDocument();
            pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);

            // Set up the paper size.
            pd.DefaultPageSettings.PaperSize = new PaperSize("Custom", Convert.ToInt32(285), Convert.ToInt32(154));

            // Display print preview dialog.
            PrintPreviewDialog printPreviewDialog1 = new PrintPreviewDialog();
            printPreviewDialog1.Document = pd;
            printPreviewDialog1.ShowDialog();
        }

        private void pd_PrintPage(object sender, PrintPageEventArgs e)
        {
            // Capture the contents of the ReceiptPanel as an image.
            Bitmap bm = new Bitmap(panel_Receipt.Width, panel_Receipt.Height);
            panel_Receipt.DrawToBitmap(bm, new Rectangle(0, 0, panel_Receipt.Width, panel_Receipt.Height));

            // Print the captured image.
            e.Graphics.DrawImage(bm, 0, 0);
        }
    }
}
