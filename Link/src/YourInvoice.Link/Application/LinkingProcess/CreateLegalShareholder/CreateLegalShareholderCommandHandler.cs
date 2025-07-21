///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalShareholder
{
    public sealed class CreateLegalShareholderCommandHandler : IRequestHandler<CreateLegalShareholderCommand, ErrorOr<bool>>
    {
        private readonly ILegalShareholderRepository _repository;
        private readonly IUnitOfWorkLink _unitOfWorkLink;

        public CreateLegalShareholderCommandHandler(ILegalShareholderRepository repository, IUnitOfWorkLink unitOfWorkLink)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWorkLink = unitOfWorkLink ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<ErrorOr<bool>> Handle(CreateLegalShareholderCommand request, CancellationToken cancellationToken)
        {
            LegalShareholder entity = new LegalShareholder(
                Guid.NewGuid(),
                request.id_LegalGeneralInformation,
                request.fullNameCompanyName,
                request.documentTypeId,
                request.documentNumber,
                request.phoneNumber,
                request.completed,
                Guid.Empty,
                null,
                true,
                ExtensionFormat.DateTimeCO(),
                request.id_LegalGeneralInformation,
                null,
                Guid.Empty);

            _repository.AddLegalShareholder(entity);

            await _unitOfWorkLink.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
