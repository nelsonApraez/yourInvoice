///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace Payers.Common;

public record PayerResponse(
    Guid Id,
    string Nit,
    string NitDv,
    string Name,
    string Description
    );