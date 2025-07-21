///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.Accounts.CreateAccount;

public record CreateAccountCommand(Guid? personTypeId, string nit, string digitVerify, string socialReason, string name, string secondName, string lastName,
            string secondLastName, Guid? documentTypeId, string documentNumber, string email, string mobileNumber, Guid? mobileCountryId, string phoneNumber,
            Guid? phoneCountryId, Guid? contactById, Guid roleId) : IRequest<ErrorOr<Guid>>;