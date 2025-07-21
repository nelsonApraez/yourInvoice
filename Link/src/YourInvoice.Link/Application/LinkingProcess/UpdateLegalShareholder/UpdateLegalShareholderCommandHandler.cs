///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Link.Application.LinkingProcess.Common;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;

namespace yourInvoice.Link.Application.LinkingProcess.UpdateLegalShareholder
{
    public class UpdateLegalShareholderCommandHandler : IRequestHandler<UpdateLegalShareholderCommand, ErrorOr<bool>>
    {
        private readonly ILegalShareholderRepository _repository;

        public UpdateLegalShareholderCommandHandler(ILegalShareholderRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(UpdateLegalShareholderCommand request, CancellationToken cancellationToken)
        {
            var data = UtilityBusinessLink.PassDataOriginDestiny(request.LegalShareholder, new LegalShareholder());
            await _repository.UpdateLegalShareholder(data);
            return true;
        }
    }
}