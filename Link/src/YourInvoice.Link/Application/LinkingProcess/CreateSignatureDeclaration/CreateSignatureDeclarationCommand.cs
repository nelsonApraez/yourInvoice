
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************
namespace yourInvoice.Link.Application.LinkingProcess.CreateSignatureDeclaration;

public record CreateSignatureDeclarationCommand(Guid? id, Guid id_GeneralInformation, bool? generalStatement, bool? visitAuthorization, bool? sourceFundsDeclaration,
               Guid? completed, Guid? statusId, DateTime? statusDate, bool? status, DateTime? createOn, Guid? createBy, DateTime? modifiedOn, Guid? modifiedBy) : IRequest<ErrorOr<bool>>;