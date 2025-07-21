///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Constant;
using yourInvoice.Common.Entities;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Common.Business.EmailModule
{
    public class EmainBusiness
    {
        private readonly ICatalogBusiness _catalog;

        public EmainBusiness(ICatalogBusiness catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public async Task SendAsync(List<string> mailto, string subject, string mailbody)
        {
            foreach (var email in mailto)
            {
                await SendAsync(email, subject, mailbody);
            }
        }

        public async Task SendAsync(string mailto, string subject, string mailbody, List<AttachFile> attachFile = default)
        {
            //consultar los catalogos con la info de parametros de conexión
            var catalogs = await _catalog.ListByCatalogAsync(EmailCatalog.CatalogName);

            //consultar valores
            var portCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailPort);
            var hostCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailServer);
            var userCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailUser);
            var passCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailPwd);
            var fromCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailFrom);
            var senderCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailSender);

            Send(mailto, subject, mailbody, hostCatalog?.Descripton, portCatalog?.Descripton, userCatalog?.Descripton, passCatalog?.Descripton, fromCatalog?.Descripton, senderCatalog?.Descripton, attachFile);
        }

        public void SendAsync(List<string> mailto, string subject, string mailbody, string emailHost, string emailPort, string emailUsername, string emailPasProjrd, string emailFrom, string emailSender)
        {
            foreach (var email in mailto)
            {
                Send(email, subject, mailbody, emailHost, emailPort, emailUsername, emailPasProjrd, emailFrom, emailSender);
            }
        }

        public void Send(string mailto, string subject, string mailbody, string emailHost, string emailPort, string emailUsername, string emailPasProjrd, string emailFrom, string emailSender, List<AttachFile> attachFile = default)
        {
            try
            {
                if (!IsValidEmail(mailto))
                    throw new ArgumentException($"{GetErrorDescription(MessageCodes.EmailNotValid)} {mailto}");

                if (!IsValidEmail(emailFrom))
                    throw new ArgumentException($"{GetErrorDescription(MessageCodes.EmailNotValid)} {emailFrom}");

                Validations(emailHost, emailPort, emailUsername, emailPasProjrd, emailFrom, emailSender);

                int port = int.Parse(emailPort);
                string mailTo = mailto;
                string mailTitle = subject;

                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(emailSender, emailFrom));
                message.Sender = new MailboxAddress(emailSender, emailFrom);
                message.To.Add(InternetAddress.Parse(mailTo));
                message.Subject = mailTitle;
                message.Body = new TextPart("html") { Text = mailbody };
                if (attachFile is not null && attachFile.Count > 0)
                {
                    var body = new TextPart("html") { Text = mailbody };
                    var multipart = GetAttachment(attachFile);
                    multipart.Add(body);
                    message.Body = multipart;
                }
                using (var client = new SmtpClient())
                {
                    client.Connect(emailHost, port, SecureSocketOptions.StartTls);
                    client.Authenticate(emailUsername, emailPasProjrd);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task SendWithCCAsync(string mailto, string cc, string subject, string mailbody, List<AttachFile> attachFile = default)
        {
            var catalogs = await _catalog.ListByCatalogAsync(EmailCatalog.CatalogName);

            var portCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailPort);
            var hostCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailServer);
            var userCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailUser);
            var passCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailPwd);
            var fromCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailFrom);
            var senderCatalog = catalogs.FirstOrDefault(x => x.Name == EmailCatalog.EmailSender);

            SendWithCC(mailto, cc, subject, mailbody, hostCatalog?.Descripton, portCatalog?.Descripton, userCatalog?.Descripton, passCatalog?.Descripton, fromCatalog?.Descripton, senderCatalog?.Descripton, attachFile);
        }

        private void SendWithCC(string mailto, string cc, string subject, string mailbody, string emailHost, string emailPort, string emailUsername, string emailPasProjrd, string emailFrom, string emailSender, List<AttachFile> attachFile = default)
        {
            try
            {
                if (!IsValidEmail(mailto))
                    throw new ArgumentException($"{GetErrorDescription(MessageCodes.EmailNotValid)} {mailto}");

                if (!IsValidEmail(emailFrom))
                    throw new ArgumentException($"{GetErrorDescription(MessageCodes.EmailNotValid)} {emailFrom}");

                Validations(emailHost, emailPort, emailUsername, emailPasProjrd, emailFrom, emailSender);

                int port = int.Parse(emailPort);
                string mailTo = mailto;
                string mailTitle = subject;

                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(emailSender, emailFrom));
                message.Sender = new MailboxAddress(emailSender, emailFrom);
                message.To.Add(InternetAddress.Parse(mailTo));

                if(!string.IsNullOrEmpty(cc))
                    message.Cc.Add(InternetAddress.Parse(cc));

                message.Subject = mailTitle;
                message.Body = new TextPart("html") { Text = mailbody };
                
                if (attachFile is not null && attachFile.Count > 0)
                {
                    var body = new TextPart("html") { Text = mailbody };
                    var multipart = GetAttachment(attachFile);
                    multipart.Add(body);
                    message.Body = multipart;
                }
                
                using (var client = new SmtpClient())
                {
                    client.Connect(emailHost, port, SecureSocketOptions.StartTls);
                    client.Authenticate(emailUsername, emailPasProjrd);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static void Validations(string emailHost, string emailPort, string emailUsername, string emailPasProjrd, string emailFrom, string emailSender)
        {
            if (!IsValidEmail(emailSender))
                throw new ArgumentException($"{GetErrorDescription(MessageCodes.EmailNotValid)} {emailSender}");

            if (emailPort is null || emailHost is null || emailUsername is null || emailPasProjrd is null || emailFrom is null || emailSender is null)
                throw new ArgumentException(GetErrorDescription(MessageCodes.EmailNotConfigured));
        }

        private Multipart GetAttachment(List<AttachFile> attachFiles)
        {
            var multipart = new Multipart("mixed");
            foreach (var attachFile in attachFiles)
            {
                var attachment = new MimePart("application", "octet-stream")
                {
                    Content = new MimeContent(attachFile.File),
                    ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                    ContentTransferEncoding = ContentEncoding.Base64,
                    FileName = GetNameFile(attachFile.PathFileWithExtension),
                };
                multipart.Add(attachment);
            }
            return multipart;
        }

        /// <summary>
        /// Valida una direccion de correo
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private static string GetNameFile(string pathUrl)
        {
            if (string.IsNullOrEmpty(pathUrl))
            {
                return string.Empty;
            }
            if (!pathUrl.Contains("/"))
            {
                return pathUrl;
            }
            var nameFile = pathUrl.Substring(pathUrl.LastIndexOf("/") + 1);
            return nameFile;
        }
    }
}