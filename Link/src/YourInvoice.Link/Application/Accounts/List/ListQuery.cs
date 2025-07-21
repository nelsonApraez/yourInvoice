///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Link.Domain.Accounts.Queries;

namespace yourInvoice.Link.Application.Accounts.List;

public record ListQuery(SearchInfo pagination) : IRequest<ErrorOr<ListDataInfo<ListResponse>>>;