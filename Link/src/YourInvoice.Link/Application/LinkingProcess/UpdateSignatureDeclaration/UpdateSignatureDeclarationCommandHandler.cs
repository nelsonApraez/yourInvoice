///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateSignatureDeclaration
{
    public class UpdateSignatureDeclarationCommandHandler : IRequestHandler<UpdateSignatureDeclarationCommand, ErrorOr<bool>>
    {
        private readonly ISignatureDeclarationRepository repository;

        public UpdateSignatureDeclarationCommandHandler(ISignatureDeclarationRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(UpdateSignatureDeclarationCommand command, CancellationToken cancellationToken)
        {
            var dec = UtilityBusinessLink.PassDataOriginDestiny(command.UpdateSignatureDeclaration, new SignatureDeclaration());
            await this.repository.UpdateSignatureDeclarationAsync(dec);
            return true;
        }


    }
}