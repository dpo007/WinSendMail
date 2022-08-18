using MailKit;
using MailKit.Net.Imap;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Graph;
using Microsoft.Identity.Client;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WinSendMail
{
    class WinSendMailMS365
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

        static async Task Main()
        {
            string rawEmail = null;
            string line;

            // Read from console/stdin until "Ctrl-Z"...
            while ((line = Console.ReadLine()) != null)
            {
                rawEmail += line + Environment.NewLine;
            }

            if (Properties.Settings.Default.SaveEmailsToDisk)
            {
                // Show raw email in console.
                Console.Write(rawEmail);

                // Save the email to a file on disk.
                string rndFileNamePart = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
                StreamWriter streamWriter = new StreamWriter(@"WinSendMailLog-" + rndFileNamePart + ".txt");
                streamWriter.Write(rawEmail);
                streamWriter.Dispose();
            }

            // Import raw email into a MimeMessage object.
            _ = new MimeMessage();
            MimeMessage mimeEmailMsg = MimeMessage.Load(GenerateStreamFromString(rawEmail));

            //PublicClientApplicationOptions options = new PublicClientApplicationOptions
            //{
            //    ClientId = "5a88c283-f790-4a45-998c-d4b5dad4536a",
            //    TenantId = "1d415c3f-174d-4411-b03e-37a96dca2ec3",
            //    RedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient"
            //};

            //IPublicClientApplication publicClientApplication = PublicClientApplicationBuilder
            //    .CreateWithApplicationOptions(options)
            //    .Build();

            var options = new ConfidentialClientApplicationOptions
            {
                ClientId = "5a88c283-f790-4a45-998c-d4b5dad4536a",
                TenantId = "1d415c3f-174d-4411-b03e-37a96dca2ec3",
                ClientSecret = "a8i8Q~cOkCkJdzhMoelUbHw6-olMWjUGeTgDbdgD",
                RedirectUri = "https://login.microsoftonline.com/common/oauth2/nativeclient"
            };

            IConfidentialClientApplication confidentialClientApplication = ConfidentialClientApplicationBuilder
                .CreateWithApplicationOptions(options)
                .Build();

            string[] scopes = new string[] {
                //"email",
                //"offline_access",
                ////"https://outlook.office.com/IMAP.AccessAsUser.All", // Only needed for IMAP
                ////"https://outlook.office.com/POP.AccessAsUser.All",  // Only needed for POP
                //"https://outlook.office.com/SMTP.Send", // Only needed for SMTP
                "https://graph.microsoft.com/.default"
            };

            //AuthenticationResult authToken = await publicClientApplication.AcquireTokenInteractive(scopes).ExecuteAsync();
            AuthenticationResult authResult = await confidentialClientApplication.AcquireTokenForClient(scopes).ExecuteAsync();

            //SaslMechanismOAuth2 oauth2 = new SaslMechanismOAuth2(authResult.Account.Username, authResult.AccessToken);
            ClientCredentialProvider authProvider = new ClientCredentialProvider(confidentialClientApplication);
            GraphServiceClient graphClient = new GraphServiceClient(authProvider);

            //https://techjatinder.medium.com/c-code-to-to-send-emails-using-microsoft-graph-api-2a90da6d648a
            //https://docs.microsoft.com/en-us/graph/sdks/choose-authentication-providers?tabs=CS#client-credentials-provider

            // Send the email.
            //using (var mailClient = new SmtpClient(new ProtocolLogger("WinSendMailMS365_STMP.log")))
            //{
            //    await mailClient.ConnectAsync("smtp.office365.com", 587, SecureSocketOptions.StartTls);
            //    await mailClient.AuthenticateAsync(oauth2);
            //    await mailClient.SendAsync(mimeEmailMsg);
            //    await mailClient.DisconnectAsync(true);
            //}
        }
    }
}
