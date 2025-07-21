///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.Beneficiary.Delete;

public record class DeleteBeneficiaryCommand(List<Guid> beneficiaryIds) : IRequest<ErrorOr<bool>>;