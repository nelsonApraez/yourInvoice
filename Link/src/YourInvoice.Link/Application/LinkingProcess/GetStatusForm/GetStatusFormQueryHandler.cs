///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.EF.Entity;
using yourInvoice.Common.Persistence.Configuration;
using yourInvoice.Link.Application.LinkingProcess.ChangeLinkStatus;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Domain.LinkingProcesses.Person;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;

namespace yourInvoice.Link.Application.LinkingProcess.GetStatusForm
{
    public class GetStatusFormQueryHandler : IRequestHandler<GetStatusFormQuery, ErrorOr<GetStatusFormResponse>>
    {
        private readonly IPersonRepository _repository;
        private readonly IMediator mediator;
        private readonly ILinkStatusRepository _linkStatusRepository;

        public GetStatusFormQueryHandler(IPersonRepository repository, IMediator mediator, ILinkStatusRepository linkStatusRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            this.mediator = mediator;
            _linkStatusRepository = linkStatusRepository ?? throw new ArgumentNullException(nameof(linkStatusRepository));
        }

        public async Task<ErrorOr<GetStatusFormResponse>> Handle(GetStatusFormQuery query, CancellationToken cancellationToken)
        {
            var statusForm = await GetStatusFormProcessAsync(query.Id_GeneralInformation);
            return new GetStatusFormResponse() { StatusForm = statusForm };
        }

        private async Task<IEnumerable<StatusForm>> GetStatusFormProcessAsync(Guid id_generalInformation)
        {
            var catalogItems = await _repository.GetCatalogItemsAsync(ConstDataBase.StatusForm);
            var statusForm = new List<StatusForm>();
            var banck = await _repository.GetAsync<BankInformation>(c => c.Id_GeneralInformation == id_generalInformation);
            var statusFormBanckItem = GetStatusForm(nameof(BankInformation), banck?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusFormBanckItem);
            var working = await _repository.GetAsync<WorkingInformation>(c => c.Id_GeneralInformation == id_generalInformation);
            var statusFormWorkingItem = GetStatusForm(nameof(WorkingInformation), working?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusFormWorkingItem);
            var financial = await _repository.GetAsync<FinancialInformation>(c => c.Id_GeneralInformation == id_generalInformation);
            var statusFormFinancialItem = GetStatusForm(nameof(FinancialInformation), financial?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusFormFinancialItem);
            var general = await _repository.GetAsync<GeneralInformation>(c => c.Id == id_generalInformation);
            var statusFormGeneralItem = GetStatusForm(nameof(GeneralInformation), general?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusFormGeneralItem);
            var personalReferences = await _repository.GetAsync<PersonalReferences>(c => c.Id_GeneralInformation == id_generalInformation);
            var statusFormPersonalItem = GetStatusForm(nameof(PersonalReferences), personalReferences?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusFormPersonalItem);
            var exposure = await ValidationExposure(id_generalInformation, catalogItems);
            statusForm.Add(exposure);
            var signatureDeclaration = await _repository.GetAsync<SignatureDeclaration>(c => c.Id_GeneralInformation == id_generalInformation);
            var statusSignatureDeclaration = GetStatusForm(nameof(SignatureDeclaration), signatureDeclaration?.Completed ?? Guid.Empty, catalogItems);
            statusForm.Add(statusSignatureDeclaration);
            var statusFormComplete = GetStatusFormCompleted(statusForm, catalogItems);
            var isCompletedNatural = statusFormComplete.FirstOrDefault()?.StatusFormAll ?? false;

            LinkStatus statusId = await _linkStatusRepository.GetLinkStatusAsync(id_generalInformation);
            if (statusId.StatusLinkId != CatalogCodeLink_LinkStatus.PendingApproval && statusId.StatusLinkId != CatalogCodeLink_LinkStatus.Linked && statusId.StatusLinkId != CatalogCodeLink_LinkStatus.Rejected 
                && statusId.StatusLinkId != CatalogCodeLink_LinkStatus.ValidationRejected && statusId.StatusLinkId != CatalogCodeLink_LinkStatus.SignatureUnsuccessful)
            {
                await this.mediator.Publish(new ChangeLinkStatusCommand { IdUserLink = id_generalInformation, StatusLinkId = isCompletedNatural ? CatalogCodeLink_LinkStatus.PendingSignature : CatalogCodeLink_LinkStatus.InProcess });
            }                

            return statusFormComplete;
        }

        private StatusForm GetStatusForm(string nameForm, Guid? statusFormId, IEnumerable<CatalogItemInfo> catalogItems)
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

        private async Task<StatusForm> ValidationExposure(Guid id_generalInformation, IEnumerable<CatalogItemInfo> catalogItems)
        {
            var exposure = await _repository.GetExposureInformationAsync(id_generalInformation);
            var exposureResult = GetStatusForm(nameof(ExposureInformation), exposure?.FirstOrDefault()?.Completed ?? Guid.Empty, catalogItems);
            return exposureResult;
        }

        private static List<StatusForm> GetStatusFormCompleted(List<StatusForm> statusForms, IEnumerable<CatalogItemInfo> catalogItems)
        {
            var statusFormDescription = catalogItems.FirstOrDefault(c => c.Id == CatalogCodeLink_StatusForm.Complete)?.Descripton ?? string.Empty;
            var statusCompleted = !statusForms.Exists(c => c.StatusFormDescription.ToLowerInvariant() != statusFormDescription.ToLowerInvariant());
            statusForms.ToList().ForEach(f => f.StatusFormAll = statusCompleted);
            return statusForms;
        }
    }
}