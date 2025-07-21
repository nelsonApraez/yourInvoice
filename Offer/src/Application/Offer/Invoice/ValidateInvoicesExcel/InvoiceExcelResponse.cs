///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** A�o: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Offer.Invoice.ValidateInvoicesExcel;

public record InvoiceExcelResponse(
    Guid OfferId,
    List<InvoiceExcelModel> Invoices
);