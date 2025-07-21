
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalSAGRILAFTQuestionResponse;

public record GetSagrilaftQuestionAnswerQuery() : IRequest<ErrorOr<List<GetSagrilaftQuestionsAnswerResponse>>>;