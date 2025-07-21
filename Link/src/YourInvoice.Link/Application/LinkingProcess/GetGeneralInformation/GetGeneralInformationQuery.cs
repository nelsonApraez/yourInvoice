
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;


namespace yourInvoice.Link.Application.LinkingProcess.GetGeneralInformation
{
    public record GetGeneralInformationQuery(Guid Id) : IRequest<ErrorOr<GeneralInformationResponse>>;
}