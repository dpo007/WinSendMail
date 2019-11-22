# WinSendMail
Basic [Sendmail](https://linux.die.net/man/8/sendmail.sendmail) replacement for Windows, written in C#.

Takes raw email fed via Console/StdIn, and sends it via (authenticated) SMTP.

Initial intended usage is with IIS, PHP and Exchange (all on-prem).

Written because existing "Fake Sendmail" applications for Windows were lacking (mainly in the area of error trapping/logging), and I needed to debug why certain emails were not sending.

It was quicker to just write it than to keep finding/trying various (old-as-dirt) replacements.

Depends on MimeKit and MailKit packages.