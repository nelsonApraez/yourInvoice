
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalSAGRILAFT;

public record CreateSagrilaftCommand(IEnumerable<Sagrilaft> CreateSagrilaft) : IRequest<ErrorOr<bool>>;