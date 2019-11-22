# WinSendMail
Basic Sendmail replacement written in C#

Takes raw email fed via Console/StdIn, and sends it via (authenticated) SMTP.

Initial intended usage is with IIS, PHP and Exchange (all on-prem).

Written becuase existing "Fake Sendmail" applciations for Windows were lacking (mainly in the area of error trapping/logging), and I needed to debug why certain emails were not sending.

It was quicker to just write it than to keep trying various (old as dirt) replacements.

Depends on MimeKit and MailKit packages.
