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
    public partial class MuntonPrintForm : MetroFramework.Forms.MetroForm
    {

        private PrintDocument pd = new PrintDocument();
        private PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
        public MuntonPrintForm()
        {
            InitializeComponent();
        }

        private void button_Print_Click(object sender, EventArgs e)
        {
            PrintPreview();
        }

        private void PrintPreview()
        {
            pd.PrintPage += new PrintPageEventHandler(PrintPage);
            pd.QueryPageSettings += new QueryPageSettingsEventHandler(QueryPageSettings);

            printPreviewDialog.Document = pd;
            printPreviewDialog.ShowDialog();
        }

        private void PrintPage(object sender, PrintPageEventArgs e)
        {

            // Calculate the center position for the panel
            int paperWidth = e.PageSettings.PaperSize.Width;
            int paperHeight = e.PageSettings.PaperSize.Height;
            int panelWidth = Panel_PrintReceipt.Width;
            int panelHeight = Panel_PrintReceipt.Height;

            int centerX = (paperWidth - panelWidth) / 2;
            int centerY = (paperHeight - panelHeight) / 2;

            // Draw the panel content at the calculated center position
            using (Bitmap bmp = new Bitmap(panelWidth, panelHeight))
            {
                Panel_PrintReceipt.DrawToBitmap(bmp, new Rectangle(0, 0, panelWidth, panelHeight));
                e.Graphics.DrawImage(bmp, centerX, centerY);
            }
        }

        private void QueryPageSettings(object sender, QueryPageSettingsEventArgs e)
        {
            // Adjust the paper size here
            e.PageSettings.PaperSize = new PaperSize("CustomSize", 510, 510); // Custom size in hundredths of an inch
        }
    }
}
