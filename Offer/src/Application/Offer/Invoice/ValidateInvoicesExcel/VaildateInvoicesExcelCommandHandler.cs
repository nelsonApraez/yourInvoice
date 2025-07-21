///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

using Microsoft.Extensions.Logging;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Constant;
using yourInvoice.Common.Integration.Files;
using yourInvoice.Offer.Domain.Invoices;
using System.Collections;
using System.Globalization;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.Invoice.ValidateInvoicesExcel
{
    public class ValidateInvoicesExcelCommandHandler : IRequestHandler<ValidateInvoicesExcelCommand, ErrorOr<InvoiceExcelResponse>>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly IFileOperation _fileOperation;
        private readonly ILogger<ValidateInvoicesExcelCommandHandler> _logger;

        public ValidateInvoicesExcelCommandHandler(IInvoiceRepository invoiceRepository, IFileOperation fileOpertaion, ILogger<ValidateInvoicesExcelCommandHandler> logger)
        {
            _invoiceRepository = invoiceRepository;
            _fileOperation = fileOpertaion;
            _logger = logger;
        }

        public async Task<ErrorOr<InvoiceExcelResponse>> Handle(ValidateInvoicesExcelCommand command, CancellationToken cancellationToken)
        {
            CultureInfo cultura = new CultureInfo("es-CO");
            Thread.CurrentThread.CurrentCulture = cultura;
            Thread.CurrentThread.CurrentUICulture = cultura;

            //si el estado de la oferta no es en progreso saca error
            if (!await this._invoiceRepository.OfferIsInProgressAsync(command.OfferId))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            var excel = Convert.FromBase64String(command.ExcelBase64);

            var rows = _fileOperation.ReadFileExcel<InvoiceExcelModel>(excel);            

            var invoices = await _invoiceRepository.GetAllByOffer(command.OfferId);

            if (rows.Any(x => (x.Valor_neto_de_pago == 0) || x._fecha_de_pago == DateTime.MinValue))
                return Error.Validation(MessageCodes.FieldEmpty,
                               GetErrorDescription(MessageCodes.FieldEmpty));

            if (!HasSameNumberOfRecords(rows, invoices))
                return Error.Validation(MessageCodes.FileRejectByNoContentSameCountRecordThatOffer,
                    GetErrorDescription(MessageCodes.FileRejectByNoContentSameCountRecordThatOffer));

            if (HasNumberInvoiceInvalids(rows, invoices))
                return Error.Validation(MessageCodes.FileRejectByContentInvoiceInvalids,
                    GetErrorDescription(MessageCodes.FileRejectByContentInvoiceInvalids));

            if (HasDateInvoiceInvalids(rows))
                return Error.Validation(MessageCodes.FileRejectByContentDateInvoiceInvalids,
                    GetErrorDescription(MessageCodes.FileRejectByContentDateInvoiceInvalids));

            if (HasValueLessThatZero(rows))
                return Error.Validation(MessageCodes.FileRejectByContentValuesLessThatZero,
                              GetErrorDescription(MessageCodes.FileRejectByContentValuesLessThatZero));
            var messageValidation = HasValuePlusPercentage(rows, invoices);
            if (messageValidation)
            {
                return Error.Validation(MessageCodes.PaymentValuePercentagePlus, GetErrorDescription(MessageCodes.PaymentValuePercentagePlus));
            }

            return new InvoiceExcelResponse(command.OfferId, rows);
        }

        private bool HasValueLessThatZero(IEnumerable<InvoiceExcelModel> rows)
        {
            var query = from item in rows
                        where item.Valor_neto_de_pago <= 0
                        select item;

            return query.Any();
        }

        private bool HasNumberInvoiceInvalids(IEnumerable<InvoiceExcelModel> rows, IEnumerable<Domain.Invoices.Invoice> invoices)
        {
            var query = from item in rows
                        where invoices.FirstOrDefault(invoice => invoice.Number == item.No_factura) == null
                        select item;

            return query.Any();
        }

        private bool HasDateInvoiceInvalids(IEnumerable<InvoiceExcelModel> rows)
        {
           
            var query = from item in rows
                        where item._fecha_de_pago <= DateTime.Now
                        select item;

            return query.Any();
        }

        private bool HasSameNumberOfRecords(ICollection rows, IEnumerable<Domain.Invoices.Invoice> invoices)
        {
            return rows.Count == invoices.Where(x => x.StatusId != CatalogCode_InvoiceStatus.Rejected).Count();
        }

        private bool HasValuePlusPercentage(IEnumerable<InvoiceExcelModel> fileInvoices, IEnumerable<Domain.Invoices.Invoice> invoices)
        {
            var paymentValuePercentagePlus = ConstantCode_InvoicePercentage.InvoiceAllowedPercentage;
            foreach (var fileInvoice in fileInvoices)
            {
                var invoiceTotal = invoices?.FirstOrDefault(c => c.Number == fileInvoice.No_factura)?.Total ?? 0;
                decimal totalValidated = ((invoiceTotal * paymentValuePercentagePlus) / 100) + invoiceTotal;
                if (fileInvoice.Valor_neto_de_pago > totalValidated)
                {
                    return true;
                }
            }
            return false;
        }

    }
}