///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;

namespace yourInvoice.Link.Application.LinkingProcess.DeleteLegalShareholder
{
    public sealed class DeleteLegalShareholderCommandHandler : IRequestHandler<DeleteLegalShareholderCommand, ErrorOr<bool>>
    {
        private readonly ILegalShareholderRepository _repository;

        public DeleteLegalShareholderCommandHandler(ILegalShareholderRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(DeleteLegalShareholderCommand request, CancellationToken cancellationToken)
        {
            if(await _repository.ExistsLegalShareholderById(request.Id, request.Id_LegalGeneralInformation))
            {
                await _repository.DeleteLegalShareholder(request.Id, request.Id_LegalGeneralInformation);
            }

            return true;
        }
    }
}
