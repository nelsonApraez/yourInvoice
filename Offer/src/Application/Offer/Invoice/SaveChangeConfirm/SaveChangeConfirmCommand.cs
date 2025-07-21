///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Invoice.SaveChangeConfirm;

public record SaveChangeConfirmCommand(InvoiceExcelValidatedRequest request) : IRequest<ErrorOr<bool>>;