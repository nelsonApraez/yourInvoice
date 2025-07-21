///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Offer.Domain.MoneyTransfers.Queries;

namespace yourInvoice.Offer.Application.Beneficiary.List
{
    public record ListBeneficiariesQuery(Guid offerId, SearchInfo pagination) : IRequest<ErrorOr<ListDataInfo<BeneficiariesListResponse>>>;
}