///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Common.Persistence.Configuration;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetLegalSignatureDeclaration
{
    public class GetLegalSignatureDeclarationQueryHandler : IRequestHandler<GetLegalSignatureDeclarationQuery, ErrorOr<IEnumerable<GetLegalSignatureDeclarationResponse>>>
    {
        private readonly ILegalSignatureDeclarationRepository _repository;
        private readonly string[] nameColumn = { "CommitmentAcceptRiskManagement", "ResponsivilityForInformation", "VisitAuthorization", "Statements" };

        public GetLegalSignatureDeclarationQueryHandler(ILegalSignatureDeclarationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<IEnumerable<GetLegalSignatureDeclarationResponse>>> Handle(GetLegalSignatureDeclarationQuery query, CancellationToken cancellationToken)
        {
            var dataAccont = await _repository.GetAccounLegalGeneralAsync(query.Id_LegalGeneralInformation);
            var signature = await _repository.GetLegalSignatureDeclarationAsync(query.Id_LegalGeneralInformation);
            var paragraphs = await _repository.GeParagraphAsync(ConstDataBase.ParagraphDeclarationLegalSignature);
            var paragraphCompleteData = GetParagrphCompleteData(paragraphs, dataAccont, signature);

            return paragraphCompleteData;
        }

        private List<GetLegalSignatureDeclarationResponse> GetParagrphCompleteData(IEnumerable<string> paragraphs,
            GetAccounLegalGeneralResponse account, LegalSignatureDeclaration signature)
        {
            var register = paragraphs?.Count() ?? 0;
            if (register < 0 || register > 4)
            {
                return new();
            }
            bool?[] status = { signature.CommitmentAcceptRiskManagement, signature.ResponsivilityForInformation, signature.VisitAuthorization, signature.Statements };
            string[] dataAccount =
            {
                account.Name?? string.Empty,
                account.SecondName?? string.Empty,
                account.LastName?? string.Empty,
                account.SecondLastName?? string.Empty,
                account.DocumentTypeDescription?? string.Empty,
                account.DocumentNumber?? string.Empty,
                account.SocialReason?? string.Empty,
                account.Nit?? string.Empty,
                account.CheckDigit?? string.Empty,
                ExtensionFormat.DateTimeCO().Day.ToString(),
                ExtensionFormat.GetNameMonth(),
                ExtensionFormat.DateTimeCO().Year.ToString(),
                account.City?? string.Empty,
            };
            var listParagraph = new List<GetLegalSignatureDeclarationResponse>();
            int cnRegister = 0;
            paragraphs?.ToList().ForEach(x =>
            {
                listParagraph.Add(new GetLegalSignatureDeclarationResponse { Id = nameColumn[cnRegister], Description = string.Format(x, dataAccount), Status = status[cnRegister] });
                cnRegister++;
            });
            return listParagraph;
        }
    }
}