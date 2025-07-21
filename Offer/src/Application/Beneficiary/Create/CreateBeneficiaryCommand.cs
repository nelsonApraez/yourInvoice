///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Beneficiary.Create
{
    public record CreateBeneficiaryCommand(
    Guid OfferId,
    string Name,
    Guid DocumentTypeId,
    string DocumentNumber,
    Guid BankId,
    Guid AccountTypeId,
    string AccountNumber,
    decimal Total,
    string DocumentOrRutBase64,
    string BankCertificateBase64,
    Guid PersonTypeId) : IRequest<ErrorOr<Guid>>;
}