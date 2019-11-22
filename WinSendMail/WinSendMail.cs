using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.IO;

namespace WinSendMail
{
    class WinSendMail
    {
        private static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        static void Main(string[] args)
        {
            string output = null;
            string line;

            // Read from console/stdin until "Ctrl-Z"...
            while ((line = Console.ReadLine()) != null)
            {
                output += line + Environment.NewLine;
            }

            if (Properties.Settings.Default.SaveEmailsToDisk)
            {
                // Show raw email in console.
                Console.Write(output);

                // Save the email to a file on disk.
                string rndFileNamePart = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
                StreamWriter streamWriter = new StreamWriter(@"WinSendMailLog-" + rndFileNamePart + ".txt");
                streamWriter.Write(output);
                streamWriter.Dispose();
            }

            // Import raw email into a MimeMessage object.
            _ = new MimeMessage();
            MimeMessage rawEmail = MimeMessage.Load(GenerateStreamFromString(output));

            // Send the email.
            using (SmtpClient mailClient = new SmtpClient())
            {
                mailClient.Connect(Properties.Settings.Default.MailServer, Properties.Settings.Default.SMTPPort);
                
                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                mailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                mailClient.Authenticate(Properties.Settings.Default.SMTPUserName, Properties.Settings.Default.SMTPPassword);

                mailClient.Send(rawEmail);
                mailClient.Disconnect(true);
            }
        }
    }
}
