///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreatePersonalReference;

public record CreatePersonalReferenceCommand(Guid? id_GeneralInformation, string? namePersonalReference, string? phoneNumber, string? nameBussines, Guid? departmentState, Guid? city, Guid? completed) : IRequest<ErrorOr<bool>>;