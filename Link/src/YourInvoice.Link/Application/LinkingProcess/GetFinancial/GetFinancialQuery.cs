///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetFinancial;

public record GetFinancialQuery(Guid idGeneralInformation) : IRequest<ErrorOr<GetFinancialResponse>>;
