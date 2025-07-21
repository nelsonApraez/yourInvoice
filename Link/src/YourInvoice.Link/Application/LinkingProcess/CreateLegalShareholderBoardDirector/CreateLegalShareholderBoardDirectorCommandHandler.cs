///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholdersBoardDirectors;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;

namespace yourInvoice.Link.Application.LinkingProcess.CreateLegalShareholderBoardDirector
{
    public sealed class CreateLegalShareholderBoardDirectorCommandHandler : IRequestHandler<CreateLegalShareholderBoardDirectorCommand, ErrorOr<bool>>
    {
        private readonly ILegalShareholderBoardDirectorRepository _repository;
        private readonly ILegalShareholderRepository _shareholderRepository;
        private readonly ILegalBoardDirectorRepository _boardRepository;
        private readonly IUnitOfWorkLink _unitOfWorkLink;

        public CreateLegalShareholderBoardDirectorCommandHandler(ILegalShareholderBoardDirectorRepository repository, ILegalShareholderRepository shareholderRepository, ILegalBoardDirectorRepository boardRepository, IUnitOfWorkLink unitOfWorkLink)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _shareholderRepository = shareholderRepository ?? throw new ArgumentNullException(nameof(shareholderRepository));
            _boardRepository = boardRepository ?? throw new ArgumentNullException(nameof(boardRepository));
            _unitOfWorkLink = unitOfWorkLink ?? throw new ArgumentNullException(nameof(unitOfWorkLink));
        }

        public async Task<ErrorOr<bool>> Handle(CreateLegalShareholderBoardDirectorCommand request, CancellationToken cancellationToken)
        {
            if (await _repository.ExistsLegalShareholderBoardDirectorById(request.id_LegalGeneralInformation))
            {
                await _repository.UpdateLegalShareholderBoardDirector(new LegalShareholderBoardDirector(
                    Guid.NewGuid(),
                    request.id_LegalGeneralInformation,
                    request.isSoleProprietorship,
                    await GetStatusForm(request.id_LegalGeneralInformation, request.isSoleProprietorship),
                     null,
                    Guid.Empty,
                    ExtensionFormat.DateTimeCO(),
                    request.id_LegalGeneralInformation,
                    Guid.Empty,
                    null,
                    true
                ));
            }
            else
            {
                _repository.AddLegalShareholderBoardDirector(new LegalShareholderBoardDirector(
                    Guid.NewGuid(),
                    request.id_LegalGeneralInformation,
                    request.isSoleProprietorship,
                    await GetStatusForm(request.id_LegalGeneralInformation, request.isSoleProprietorship),
                    ExtensionFormat.DateTimeCO(),
                    request.id_LegalGeneralInformation,
                    null,
                    Guid.Empty,
                    Guid.Empty,
                null,
                true
                ));

                await _unitOfWorkLink.SaveChangesAsync(cancellationToken);
            }

            return true;
        }

        private async Task<Guid> GetStatusForm(Guid id_LegalGeneralInformation, bool isSoleProprietorship)
        {
            if(isSoleProprietorship)
                return CatalogCodeLink_StatusForm.Complete;

            var existShareholder = await _shareholderRepository.ExistsLegalShareholder(id_LegalGeneralInformation);
            var existBoard = await _boardRepository.ExistsLegalBoardDirector(id_LegalGeneralInformation);

            if(existShareholder && existBoard)
                return CatalogCodeLink_StatusForm.Complete;

            if (!existShareholder && !existBoard)
                return CatalogCodeLink_StatusForm.WithoutStarting;

            return CatalogCodeLink_StatusForm.InProgress;
        }
    }
   
}
