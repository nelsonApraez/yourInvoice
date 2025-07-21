///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Entities;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.ListEconomicActivity;

public record ListEconomicActivityQuery() : IRequest<ErrorOr<ListDataInfo<ListEconomicActivityResponse>>>;