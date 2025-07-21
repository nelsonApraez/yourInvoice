///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.EF.Entity;
using yourInvoice.Common.Persistence.Configuration;
using yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus;
using yourInvoice.Link.Application.LinkingProcess.GetStatusForm;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholdersBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Domain.LinkingProcesses.Person;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Application.LinkingProcess.GetStatusFormLegal
{
    public class GetStatusFormLegalQueryHandler : IRequestHandler<GetStatusFormLegalQuery, ErrorOr<GetStatusFormLegalResponse>>
    {
        private readonly IPersonRepository _repository;
        private readonly IMediator mediator;
        private readonly ILinkStatusRepository _linkStatusRepository;

        public GetStatusFormLegalQueryHandler(IPersonRepository repository, IMediator mediator, ILinkStatusRepository linkStatusRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mediator = mediator;
            _linkStatusRepository = linkStatusRepository ?? throw new ArgumentNullException(nameof(linkStatusRepository));
        }

        public async Task<ErrorOr<GetStatusFormLegalResponse>> Handle(GetStatusFormLegalQuery query, CancellationToken cancellationToken)
        {
            var statusFormLegal = await GetStatusFormLegalProcessAsync(query.Id_LegalGeneralInformation);
            return new GetStatusFormLegalResponse() { StatusForm = statusFormLegal };
        }

        private async Task<IEnumerable<StatusForm>> GetStatusFormLegalProcessAsync(Guid id_legalGeneralInformation)
        {
            var catalogItems = await _repository.GetCatalogItemsAsync(ConstDataBase.StatusForm);

            var statusForm = new List<StatusForm>();
            var generalInfo = await _repository.GetAsync<LegalGeneralInformation>(c => c.Id == id_legalGeneralInformation);
            var statusGeneralInfo = GetStatusFormLegal(nameof(LegalGeneralInformation), generalInfo?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusGeneralInfo);

            var financial = await _repository.GetAsync<LegalFinancialInformation>(c => c.Id_LegalGeneralInformation == id_legalGeneralInformation);
            var statusFormFinancialItem = GetStatusFormLegal(nameof(LegalFinancialInformation), financial?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusFormFinancialItem);

            var representative = await _repository.GetAsync<LegalRepresentativeTaxAuditor>(c => c.Id_LegalGeneralInformation == id_legalGeneralInformation);
            var statusRepresentativeItem = GetStatusFormLegal(nameof(LegalRepresentativeTaxAuditor), representative?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusRepresentativeItem);

            var solePropiertorship = await _repository.GetAsync<LegalShareholderBoardDirector>(c => c.Id_LegalGeneralInformation == id_legalGeneralInformation);
            var statusShareholdersAndBoardDirectors = GetStatusFormLegal(nameof(LegalShareholderBoardDirector), solePropiertorship?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusShareholdersAndBoardDirectors);

            var comercialBanck = await _repository.GetAsync<LegalCommercialAndBankReference>(c => c.Id_LegalGeneralInformation == id_legalGeneralInformation);
            var statusComercialBanck = GetStatusFormLegal(nameof(LegalCommercialAndBankReference), comercialBanck?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusComercialBanck);

            var sagrilaft = await _repository.GetAsync<LegalSAGRILAFT>(c => c.Id_LegalGeneralInformation == id_legalGeneralInformation);
            var statusSagrilaftItem = GetStatusFormLegal(nameof(LegalSAGRILAFT), sagrilaft?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusSagrilaftItem);

            var signatureDeclaration = await _repository.GetAsync<LegalSignatureDeclaration>(c => c.Id_LegalGeneralInformation == id_legalGeneralInformation);
            var statusSignatureDeclarationItem = GetStatusFormLegal(nameof(LegalSignatureDeclaration), signatureDeclaration?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusSignatureDeclarationItem);

            var statusFormComplete = GetStatusFormLegalCompleted(statusForm, catalogItems);
            var isCompletedLegal = statusFormComplete.FirstOrDefault()?.StatusFormAll ?? false;

            LinkStatus statusId = await _linkStatusRepository.GetLinkStatusAsync(id_legalGeneralInformation);
            if (statusId.StatusLinkId != CatalogCodeLink_LinkStatus.PendingApproval && statusId.StatusLinkId != CatalogCodeLink_LinkStatus.Linked && statusId.StatusLinkId != CatalogCodeLink_LinkStatus.Rejected
                && statusId.StatusLinkId != CatalogCodeLink_LinkStatus.ValidationRejected && statusId.StatusLinkId != CatalogCodeLink_LinkStatus.SignatureUnsuccessful)
            {
                await this.mediator.Publish(new ChangeLinkStatusCommand { IdUserLink = id_legalGeneralInformation, StatusLinkId = isCompletedLegal ? CatalogCodeLink_LinkStatus.PendingSignature : CatalogCodeLink_LinkStatus.InProcess });
            }
            return statusFormComplete;
        }

        private static StatusForm GetStatusFormLegal(string nameForm, Guid? statusFormId, IEnumerable<CatalogItemInfo> catalogItems)
        {
            if (statusFormId is null || statusFormId == Guid.Empty)
            {
                statusFormId = CatalogCodeLink_StatusForm.WithoutStarting;
            }
            var catalogItem = catalogItems.FirstOrDefault(c => c.Id == statusFormId);
            catalogItem = catalogItem is null ? catalogItems.FirstOrDefault(c => c.Id == CatalogCodeLink_StatusForm.WithoutStarting) : catalogItem;
            var statusForm = new StatusForm { NameForm = nameForm, StatusFormName = catalogItem?.Name ?? string.Empty, StatusFormDescription = catalogItem?.Descripton ?? string.Empty };
            return statusForm;
        }

        private static List<StatusForm> GetStatusFormLegalCompleted(List<StatusForm> statusForms, IEnumerable<CatalogItemInfo> catalogItems)
        {
            var statusFormDescription = catalogItems.FirstOrDefault(c => c.Id == CatalogCodeLink_StatusForm.Complete)?.Descripton ?? string.Empty;
            var statusCompleted = !statusForms.Exists(c => c.StatusFormDescription.ToLowerInvariant() != statusFormDescription.ToLowerInvariant());
            statusForms.ToList().ForEach(f => f.StatusFormAll = statusCompleted);
            return statusForms;
        }
    }
}