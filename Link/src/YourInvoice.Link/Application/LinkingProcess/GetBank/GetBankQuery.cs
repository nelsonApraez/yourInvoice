
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetBank;

public record GetBankQuery(Guid idGeneralInformation) : IRequest<ErrorOr<GetBankResponse>>;
