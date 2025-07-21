
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Constant;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalSAGRILAFTQuestionResponse
{
    public class GetSagrilaftQuestionAnswerQueryHandler : IRequestHandler<GetSagrilaftQuestionAnswerQuery, ErrorOr<List<GetSagrilaftQuestionsAnswerResponse>>>
    {
        private readonly ILegalSAGRILAFTRepository _repository;

        public GetSagrilaftQuestionAnswerQueryHandler(ILegalSAGRILAFTRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<List<GetSagrilaftQuestionsAnswerResponse>>> Handle(GetSagrilaftQuestionAnswerQuery query, CancellationToken cancellationToken)
        {
            var showDetail = new List<Guid>() { CatalogCodeLink_SAGRILAFTQuestion.Q6, CatalogCodeLink_SAGRILAFTQuestion.Q7 };
            var result = await _repository.GetSagrilaftQuestionAnswerAsync(SAGRILAFTCatalog.CatalogName, showDetail);
            return result;
        }
    }
}