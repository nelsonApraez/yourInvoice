///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Buyer.GenerateDocs;
public record class GenerateDocsBuyerCommand(int numberOffer) : IRequest<ErrorOr<bool>>;