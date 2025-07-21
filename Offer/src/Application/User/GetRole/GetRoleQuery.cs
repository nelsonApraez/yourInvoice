///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Offer.Domain.Users.Queries;

namespace yourInvoice.Offer.Application.User.GetRole;

public record GetRoleQuery() : IRequest<ErrorOr<List<GetRoleResponse>>>;