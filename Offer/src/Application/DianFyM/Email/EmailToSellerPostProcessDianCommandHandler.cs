///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

//using yourInvoice.Common.Business.CatalogModule;
//using yourInvoice.Common.Business.EmailModule;
//using yourInvoice.Common.Business.ExcelModule;
//using yourInvoice.Common.Business.TransformModule;
//using yourInvoice.Common.EF.Data;
//using yourInvoice.Common.EF.Data.Repositories;
//using yourInvoice.Common.Extension;
//using yourInvoice.Offer.Application.Buyer.EmailToAdmin;
//using yourInvoice.Offer.Domain.InvoiceDispersions;
//using yourInvoice.Offer.Domain.Invoices;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.EmailModule;
using yourInvoice.Common.Business.ExcelModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Common.EF.Data.IRepositories;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using System.Data;

///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Offer.Application.DianFyM.Email
{
    public class EmailToSellerPostProcessDianCommandHandler : INotificationHandler<EmailToSellerPostProcessDianCommand>
    {
        private readonly ICatalogBusiness catalogBusiness;
        private readonly IInvoiceRepository invoiceRepository;
        private const string Subject = "Proceso ‎ de ‎espera ‎ validación ‎ con ‎ la ‎ DIAN ‎ finalizado ‎ para‎‎ ‎ ‎oferta ";

        public EmailToSellerPostProcessDianCommandHandler(ICatalogBusiness catalogBusiness, IInvoiceRepository invoiceRepository)
        {
            this.catalogBusiness = catalogBusiness;
            this.invoiceRepository = invoiceRepository;
        }

        public async Task Handle(EmailToSellerPostProcessDianCommand notification, CancellationToken cancellationToken)
        {
            var result = this.invoiceRepository.GetInvoicesProcessed(notification.OfferId, notification.Consecutive);

            #region Generar excel

            var dataTable = new DataTable();
            dataTable.Columns.Add("Factura", typeof(string));
            dataTable.Columns.Add("Estado", typeof(string));

            foreach (var item in result)
            {
                dataTable.Rows.Add(item.InvoiceNumber, item.status);
            }

            var docGenerated = ExcelBusiness.Generate(dataTable);

            #endregion Generar excel

            var template = await this.catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToUserPostProcessDian);

            Dictionary<string, string> replacements = new()
        {
            { "{{offerNumber}}", notification.Consecutive.ToString() },
            { "{{year}}", ExtensionFormat.DateTimeCO().Year.ToString() }
        };

            string templateWithData = TransformModule.ReplaceTokens(template.Descripton, replacements);
            EmainBusiness emainBusiness = new(this.catalogBusiness);
            var email = await invoiceRepository.GetEmailUserByOffer(notification.OfferId, notification.Consecutive);
            await emainBusiness.SendAsync(email, Subject + notification.Consecutive, templateWithData,
                new List<AttachFile>() { new AttachFile() { File = docGenerated, PathFileWithExtension = "FacturasProcesadas.xlsx" } });
        }
    }
}