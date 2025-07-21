///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Invoice.ValidateInvoicesExcel;

public record ValidateInvoicesExcelCommand(
  Guid OfferId,
  string ExcelBase64
) : IRequest<ErrorOr<InvoiceExcelResponse>>;