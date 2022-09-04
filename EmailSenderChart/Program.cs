using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace EmailSenderChart
{
    internal class Program
    {
        static void Main()
        {
            // Suppose we got chart as image from html or base64 we will save it in temp folder in real world project
            string path = Path.GetFullPath(Path.Combine(Environment.CurrentDirectory, @"..\..\..\"));
            string imagePth = path + @"EmailSenderChart\UploadedFiles\chart.jpg";
            string htmlMessage = @"<html>
                                  <body>
                                  <img src='cid:chart_1' />
                                  </body>
                                  </html>";

            using (SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SmptClientDomain"], Convert.ToInt32(ConfigurationManager.AppSettings["Email"])))
            {
                client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["Email"], ConfigurationManager.AppSettings["Password"]);
                client.EnableSsl = true;

                MailMessage msg = new MailMessage("anas.jayyusi@outlook.com",
                                                  "anas_jayyusi@outlook.com");
                // Create the HTML view
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                                                             htmlMessage,
                                                             Encoding.UTF8,
                                                             MediaTypeNames.Text.Html);

                // Add Image to html view
                string mediaType = MediaTypeNames.Image.Jpeg;
                LinkedResource img = new LinkedResource(imagePth, mediaType);

                img.ContentId = "chart_1";
                img.ContentType.MediaType = mediaType;
                img.TransferEncoding = TransferEncoding.Base64;
                img.ContentType.Name = img.ContentId;
                img.ContentLink = new Uri("cid:" + img.ContentId);
                htmlView.LinkedResources.Add(img);


                msg.AlternateViews.Add(htmlView);
                msg.IsBodyHtml = true;
                msg.Subject = "Test Chart Image";
                client.Send(msg);
            }
        }
    }
}
