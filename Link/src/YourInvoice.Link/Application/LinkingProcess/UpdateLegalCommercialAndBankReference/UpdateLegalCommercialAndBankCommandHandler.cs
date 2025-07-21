using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateLegalCommercialAndBankReference
{
    public class UpdateLegalCommercialAndBankCommandHandler : IRequestHandler<UpdateLegalCommercialAndBankCommand, ErrorOr<bool>>
    {
        private readonly ILegalCommercialAndBankReferenceRepository repository;

        public UpdateLegalCommercialAndBankCommandHandler(ILegalCommercialAndBankReferenceRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task<ErrorOr<bool>> Handle(UpdateLegalCommercialAndBankCommand command, CancellationToken cancellationToken)
        {
            var references = UtilityBusinessLink.PassDataOriginDestiny(command.UpdateCommercialAndBank, new LegalCommercialAndBankReference());
            await this.repository.UpdateLegalCommercialAndBankReferenceAsync(references);
            return true;
        }
    }
}
