///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetExposureQuestionResponse;

public record GetExposureQuestionAnswerQuery() : IRequest<ErrorOr<List<GetExposureQuestionsAnswerResponse>>>;