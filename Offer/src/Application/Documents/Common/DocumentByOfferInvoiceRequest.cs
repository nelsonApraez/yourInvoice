///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Documents.Common;

public record DocumentByOfferInvoiceRequest(Guid OfferId, Guid InvoiceId, string FormatTypeFile);