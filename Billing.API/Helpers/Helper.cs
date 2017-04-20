using Billing.API.Helpers.PDFGenerator;
using Billing.Database;
using MigraDoc.Rendering;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;

namespace Billing.API.Helpers
{
    public static class Helper
    {
        public static List<Region> Regions { get { return Enum.GetValues(typeof(Region)).Cast<Region>().ToList(); } }
        public static List<Status> Statuses { get { return Enum.GetValues(typeof(Status)).Cast<Status>().ToList(); } }

        public static string FirstLetterLow(string input)
        {
            return Char.ToLowerInvariant(input[0]) + input.Substring(1);
        }

        public static void SendEmail(Invoice invoice, string from, string emailTo)
        {
            string subject = "Invoice - " + invoice.InvoiceNo;
            string body = "Hi," + Environment.NewLine + "Invoice file in attachment.";
            string FromMail =  from + "@billing.com";
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(FromMail);
            mail.To.Add(emailTo);
            mail.Subject = subject;
            mail.Body = body;

            PDFInvoice pdf = new PDFInvoice(invoice);

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer(false);

            pdfRenderer.Document = pdf.CreateDocument();

            pdfRenderer.RenderDocument();


            MemoryStream stream = new MemoryStream();

            pdfRenderer.Save(stream, false);



            mail.Attachments.Add(new Attachment(stream, "Invoice-" + DateTime.UtcNow.ToShortDateString() + ".pdf", MediaTypeNames.Application.Pdf));


            SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["SmtpClient"]);
            SmtpServer.Port = 25;
            SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["EmailPassword"]);
            SmtpServer.EnableSsl = true;
            SmtpServer.Send(mail);
        }
    }
}