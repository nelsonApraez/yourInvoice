///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Constant;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetExposureQuestionResponse
{
    public class GetExposureQuestionAnswerQueryHandler : IRequestHandler<GetExposureQuestionAnswerQuery, ErrorOr<List<GetExposureQuestionsAnswerResponse>>>
    {
        private readonly IExposureInformationRepository _repository;

        public GetExposureQuestionAnswerQueryHandler(IExposureInformationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<List<GetExposureQuestionsAnswerResponse>>> Handle(GetExposureQuestionAnswerQuery query, CancellationToken cancellationToken)
        {
            var showDetail = new List<Guid>() { CatalogCodeLink_ExposureQuestion.TaxObligations, CatalogCodeLink_ExposureQuestion.PubliclyExposedPersonLink };
            var result = await _repository.GetExposureQuestionAnswerAsync(ExposureCatalog.CatalogName, showDetail);
            return result;
        }
    }
}