
///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration;

namespace yourInvoice.Link.Application.LinkingProcess.GetSignatureDeclaration
{
    public class GetSignatureDeclarationQueryHandler : IRequestHandler<GetSignatureDeclarationQuery, ErrorOr<GetSignatureDeclarationResponse>>
    {
        private readonly ISignatureDeclarationRepository _repository;

        public GetSignatureDeclarationQueryHandler(ISignatureDeclarationRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<GetSignatureDeclarationResponse>> Handle(GetSignatureDeclarationQuery query, CancellationToken cancellationToken)
        {
            var result = await _repository.GetSignatureDeclarationAsync(query.idGeneralInformation);
            return result;
        }
    }
}

