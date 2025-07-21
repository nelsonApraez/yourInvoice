///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Constant;
using yourInvoice.Offer.Domain.Invoices;
using yourInvoice.Offer.Domain.Primitives;
using static yourInvoice.Common.ErrorHandling.MessageHandler;

namespace yourInvoice.Offer.Application.Offer.Invoice.SaveChangeConfirm
{
    public sealed class SaveChangeConfirmCommandHandler : IRequestHandler<SaveChangeConfirmCommand, ErrorOr<bool>>
    {
        private readonly IInvoiceRepository invoiceRepository;
        private readonly IUnitOfWork _unitOfWork;

        private const string Invoices = "Invoices";

        public SaveChangeConfirmCommandHandler(IInvoiceRepository invoiceRepository, IUnitOfWork unitOfWork)
        {
            this.invoiceRepository = invoiceRepository ?? throw new ArgumentNullException(nameof(invoiceRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<ErrorOr<bool>> Handle(SaveChangeConfirmCommand command, CancellationToken cancellationToken)
        {
            if (command is null || command.request is null ||
                command.request.Invoices is null || !command.request.Invoices.Any()
                )
            {
                return Error.Validation(GetErrorDescription(MessageCodes.ParameterEmpty, Invoices));
            }

            //si el estado de la oferta no es en progreso saca error
            if (!await this.invoiceRepository.OfferIsInProgressAsync(command.request.OfferId))
                return Error.Validation(MessageCodes.MessageOfferIsNotInProgress, GetErrorDescription(MessageCodes.MessageOfferIsNotInProgress));

            var invoicesDataExists = await this.invoiceRepository.GetAllByOffer(command.request.OfferId);
            var messageValidation = HasValuePlusPercentage(command.request.Invoices, invoicesDataExists);
            if (messageValidation)
            {
                return Error.Validation(MessageCodes.PaymentValuePercentagePlus, GetErrorDescription(MessageCodes.PaymentValuePercentagePlus));
            }

            var invoices = command.request.Invoices.Select(s => new Domain.Invoices.Invoice(Guid.Empty, Guid.Empty, s.Numero_Factura, string.Empty, string.Empty,
                Guid.Empty, DateTime.Now, DateTime.Now, 0, 0, Guid.Empty, 0, string.Empty, s.Fecha_de_Pago, s.Valor_de_Pago_Neto)).ToList();
            await this.invoiceRepository.SaveChangeConfirmAsync(command.request.OfferId, invoices);
            var result = await _unitOfWork.SaveChangesAsync(cancellationToken);
            return result >= 0;
        }

        private bool HasValuePlusPercentage(IEnumerable<InvoiceExcelValidated> fileInvoices, IEnumerable<Domain.Invoices.Invoice> invoices)
        {
            var paymentValuePercentagePlus = ConstantCode_InvoicePercentage.InvoiceAllowedPercentage;
            foreach (var fileInvoice in fileInvoices)
            {
                var invoiceTotal = invoices?.FirstOrDefault(c => c.Number == fileInvoice.Numero_Factura)?.Total ?? 0;
                decimal totalValidated = ((invoiceTotal * paymentValuePercentagePlus) / 100) + invoiceTotal;
                if (fileInvoice.Valor_de_Pago_Neto > totalValidated)
                {
                    return true;
                }
            }
            return false;
        }
    }
}