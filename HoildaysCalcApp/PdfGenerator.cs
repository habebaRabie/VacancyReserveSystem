using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;


namespace HoildaysCalcApp
{
    public class PdfGenerator
    {
        public static void GeneratePdfFromDataSource(DataTable dataTable, string labelContent, string filePath)
        {
            Document doc = new Document();
            try
            {
                PdfWriter writer = PdfWriter.GetInstance(doc, new FileStream(filePath, FileMode.Create));
                doc.Open();

                // Define the font and size for text
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                Font font = new Font(bf, 12, Font.NORMAL);

                // Create a table with the same number of columns as the DataTable
                PdfPTable pdfTable = new PdfPTable(dataTable.Columns.Count);

                // Set table width to 100% of the page width
                pdfTable.WidthPercentage = 100;

                // Create a cell for the label content
                PdfPCell labelCell = new PdfPCell(new Phrase(labelContent, font));
                labelCell.Colspan = dataTable.Columns.Count;
                labelCell.HorizontalAlignment = Element.ALIGN_CENTER;
                pdfTable.AddCell(labelCell);

                // Add table headers
                foreach (DataColumn column in dataTable.Columns)
                {
                    pdfTable.AddCell(new Phrase(column.ColumnName, font));
                }

                // Add table rows
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (object item in row.ItemArray)
                    {
                        pdfTable.AddCell(new Phrase(item.ToString(), font));
                    }
                }

                doc.Add(pdfTable);
            }
            catch (DocumentException ex)
            {
                throw ex;
            }
            catch (IOException ex)
            {
                throw ex;
            }
            finally
            {
                doc.Close();
            }
        }

    }


}