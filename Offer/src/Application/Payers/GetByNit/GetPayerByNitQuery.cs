///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Payers.Common;

namespace yourInvoice.Offer.Application.Payers.GetByNit;

public record GetPayerByNitQuery(string nit) : IRequest<ErrorOr<IReadOnlyList<PayerResponse>>>;