///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Offer.Application.User.Create
{
    public record CreateUserCommand(
    int integrationId,
    string name,
    string job,
    string address,
    string phone,
    string email,
    string city,
    int documentType,
    string documentNumber,
    int? documentDigitVerify,
    string documentExpedition,
    int userType,
    int role,
    string commercialRegistrationNumber, //numero de matricula mercantil
    string commercialRegistrationCity, //ciudad de matricula mercantil
    string chamberOfCommerceCity //ciudad de camara de comercio
    ) : IRequest<ErrorOr<bool>>;
}