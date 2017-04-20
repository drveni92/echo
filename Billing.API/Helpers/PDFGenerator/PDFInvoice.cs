using Billing.Database;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Billing.API.Helpers.PDFGenerator
{
    public class PDFInvoice : PDFBasic
    {
        private Invoice invoice;

        public PDFInvoice(Invoice _invoice)
        {
            invoice = _invoice;
        }

        public override Document CreateDocument()
        {
            // Create a new MigraDoc document
            Document = new Document();
            Document.Info.Title = "Invoice " + invoice.InvoiceNo;
            Document.Info.Subject = "Invoice " + invoice.Date;
            Document.Info.Author = invoice.Agent.Name;

            DefineStyles();

            CreatePage();

            FillContent();

            return Document;

        }

        protected override void CreatePage()
        {
            // Each MigraDoc document needs at least one section.
            Section section = Document.AddSection();

            // Create header
            Table = section.Headers.Primary.AddTable();
            Table.Style = "Table";
            Column column = Table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;
            //2    
            column = Table.AddColumn("6cm");
            column.Format.Alignment = ParagraphAlignment.Left;
            //3
            column = Table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Right;
            //4
            column = Table.AddColumn("6cm");
            column.Format.Alignment = ParagraphAlignment.Left;


            Row row = Table.AddRow();
            row.BottomPadding = 2;
            row.TopPadding = 2;

            row.Cells[0].AddParagraph("Agent:");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[1].AddParagraph(invoice.Agent.Name);
            row.Cells[2].AddParagraph("Customer:");
            row.Cells[2].Format.Font.Bold = true;
            row.Cells[3].AddParagraph(invoice.Customer.Name);

            row = Table.AddRow();
            row.BottomPadding = 2;
            row.TopPadding = 2;
            row.Cells[0].AddParagraph("Shipper:");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[1].AddParagraph(invoice.Shipper.Name);
            row.Cells[2].AddParagraph("Address:");
            row.Cells[2].Format.Font.Bold = true;
            row.Cells[3].AddParagraph(invoice.Customer.Address);

            row = Table.AddRow();
            row.BottomPadding = 2;
            row.TopPadding = 2;
            row.Cells[0].AddParagraph("Destination:");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[1].AddParagraph(invoice.Customer.Town.Name);

            if(invoice.ShippedOn != null)
            {
                row = Table.AddRow();
                row.BottomPadding = 2;
                row.TopPadding = 2;
                row.Cells[0].AddParagraph("Shipped on:");
                row.Cells[0].Format.Font.Bold = true;
                row.Cells[1].AddParagraph(invoice.ShippedOn.Value.ToShortDateString());
            }

            row = Table.AddRow();
            row.BottomPadding = 2;
            row.TopPadding = 2;
            row.Cells[0].AddParagraph("Status:");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[1].AddParagraph(invoice.Status.ToString());


            // Create footer
            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Billing - BiH - " + DateTime.UtcNow.Year);
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;


            // Create the text frame for the address
            AddressFrame = section.AddTextFrame();
            AddressFrame.Height = "3.0cm";
            AddressFrame.Width = "7.0cm";
            AddressFrame.Left = ShapePosition.Left;
            AddressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            AddressFrame.Top = "5.0cm";
            AddressFrame.RelativeVertical = RelativeVertical.Page;
            

            // Add the print date field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "4cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("INVOICE " + invoice.InvoiceNo, TextFormat.Bold);
            paragraph.AddTab();
            paragraph.AddText("Date: ");
            paragraph.AddDateField("dd.MM.yyyy");

            // Create the item table
            Table = section.AddTable();
            Table.Style = "Table";
            Table.Borders.Width = 0.25;
            Table.Borders.Left.Width = 0.5;
            Table.Borders.Right.Width = 0.5;
            Table.Rows.LeftIndent = 0;

            // Before you can add a row, you must define the columns
            //1
            column = Table.AddColumn("1cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            //2    
            column = Table.AddColumn("5cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            //3
            column = Table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            //4
            column = Table.AddColumn("2cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            //5
            column = Table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;
            //6
            column = Table.AddColumn("3cm");
            column.Format.Alignment = ParagraphAlignment.Center;


            // Create the header of the table
            row = Table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            
            row.Cells[0].AddParagraph("#");
            row.Cells[0].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[1].AddParagraph("Name");
            row.Cells[1].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[2].AddParagraph("Unit");
            row.Cells[2].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[3].AddParagraph("Quantity");
            row.Cells[3].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[4].AddParagraph("Price");
            row.Cells[4].Format.Alignment = ParagraphAlignment.Center;
            row.Cells[5].AddParagraph("Total");
            row.Cells[5].Format.Alignment = ParagraphAlignment.Center;
        }

        protected override void FillContent()
        {
            Row row;
            int counter = 1;
            foreach (var item in invoice.Items)
            {
                row = Table.AddRow();
                row.TopPadding = 1.5;
                row.BottomPadding = 1.5;
                row.Cells[0].AddParagraph(counter.ToString());
                row.Cells[1].AddParagraph(item.Product.Name);
                row.Cells[2].AddParagraph(item.Product.Unit);
                row.Cells[3].AddParagraph(item.Quantity.ToString());
                row.Cells[4].AddParagraph(item.Price.ToString());
                row.Cells[5].AddParagraph(item.SubTotal.ToString());

                counter++;
            }

            // Add an invisible row as a space line to the table
            row = Table.AddRow();
            row.Borders.Visible = false;

            // Add the subtotal price row
            row = Table.AddRow();
            row.BottomPadding = 2;
            row.TopPadding = 2;
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].AddParagraph("Subtotal");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            row.Cells[5].AddParagraph(invoice.SubTotal.ToString());

            // Add the VAT row
            row = Table.AddRow();
            row.BottomPadding = 2;
            row.TopPadding = 2;
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].AddParagraph("VAT Rate");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            row.Cells[5].AddParagraph(invoice.Vat.ToString() + " %");

            // Add the VAT Amount row
            row = Table.AddRow();
            row.BottomPadding = 2;
            row.TopPadding = 2;
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].AddParagraph("VAT Amount");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            row.Cells[5].AddParagraph(invoice.VatAmount.ToString());

            // Add the shipping price
            row = Table.AddRow();
            row.BottomPadding = 2;
            row.TopPadding = 2;
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].AddParagraph("Shipping");
            row.Cells[5].AddParagraph(invoice.Shipping.ToString());
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;

            // Add the total price row
            row = Table.AddRow();
            row.BottomPadding = 2;
            row.TopPadding = 2;
            row.Cells[0].Borders.Visible = false;
            row.Cells[0].AddParagraph("Total Price");
            row.Cells[0].Format.Font.Bold = true;
            row.Cells[0].Format.Alignment = ParagraphAlignment.Right;
            row.Cells[0].MergeRight = 4;
            row.Cells[5].AddParagraph(invoice.Total.ToString());

            // Set the borders of the specified cell range
            Table.SetEdge(5, Table.Rows.Count - 5, 1, 5, Edge.Box, BorderStyle.Single, 0.75);

        }
    }
}