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

            while ((line = Console.ReadLine()) != null)
            {
                output += line + Environment.NewLine;
            }

            if (Properties.Settings.Default.SaveEmailsToDisk)
            {
                Console.Write(output);

                string rndFileNamePart = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
                StreamWriter streamWriter = new StreamWriter(@"WinSendMailLog-" + rndFileNamePart + ".txt");

                streamWriter.Write(output);

                streamWriter.Dispose();
            }

            _ = new MimeMessage();
            MimeMessage rawEmail = MimeMessage.Load(GenerateStreamFromString(output));

            using (SmtpClient client = new SmtpClient())
            {
                client.Connect(Properties.Settings.Default.MailServer, Properties.Settings.Default.SMTPPort);
                
                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                client.Authenticate(Properties.Settings.Default.SMTPUserName, Properties.Settings.Default.SMTPPassword);

                client.Send(rawEmail);
                client.Disconnect(true);
            }
        }
    }
}
