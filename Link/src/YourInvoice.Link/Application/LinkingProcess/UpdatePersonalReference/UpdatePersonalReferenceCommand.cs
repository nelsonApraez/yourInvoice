///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;

namespace yourInvoice.Link.Application.LinkingProcess.UpdatePersonalReference;

public record UpdatePersonalReferenceCommand(PersonalReferences PersonalReferences) : IRequest<ErrorOr<bool>>;