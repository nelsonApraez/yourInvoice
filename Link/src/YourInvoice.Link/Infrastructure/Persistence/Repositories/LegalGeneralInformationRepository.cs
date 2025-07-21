///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;

namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class LegalGeneralInformationRepository : ILegalGeneralInformationRepository
    {
        private readonly LinkDbContext _context;

        public LegalGeneralInformationRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<LegalGeneralInformation?> GetByEmailAsync(string email) => await _context.LegalGeneralInformations.SingleOrDefaultAsync(c => c.EmailCorporate == email);
        public LegalGeneralInformation Add(LegalGeneralInformation generalInformation) => _context.LegalGeneralInformations.Add(generalInformation).Entity;

        public async Task<Guid> GetStatusIdLegalGeneralInformationAsync(Guid accountId)
        {
            var exist = await _context.LegalGeneralInformations.FirstOrDefaultAsync(a => a.Id == accountId);
            return exist?.StatusId ?? Guid.Empty;
        }

        public async Task<bool> ExistsAccountLegalAsync(Guid accountId, Guid personTypeId)
        {
            var exist = await _context.Accounts.AnyAsync(a => a.Id == accountId && a.PersonTypeId == personTypeId);
            return exist;
        }

        public async Task<bool> ExistseLegalGeneralInformationAsync(Guid accountId)
        {
            var exist = await _context.LegalGeneralInformations.AnyAsync(a => a.Id == accountId);
            return exist;
        }

        public async Task<bool> CreateLegalGeneralInformationAsync(LegalGeneralInformation legalGeneralInformation)
        {
            await _context.LegalGeneralInformations.AddAsync(legalGeneralInformation);
            return true;
        }

        public async Task<bool> UpdateStatusAsync(Guid id, Guid statusId)
        {
            await _context.LegalGeneralInformations
                .Where(c => c.Id == id)
                .ExecuteUpdateAsync(p => p
                .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                .SetProperty(u => u.ModifiedBy, id)
                .SetProperty(u => u.StatusId, statusId));

            return true;
        }

        public async Task<bool> UpdateLegalGeneralInformationAsync(LegalGeneralInformation legalGeneralInformation)
        {
            await _context.LegalGeneralInformations
                  .Where(c => c.Id == legalGeneralInformation.Id)
                  .ExecuteUpdateAsync(p => p
                  .SetProperty(u => u.Nit, legalGeneralInformation.Nit)
                  .SetProperty(u => u.CheckDigit, legalGeneralInformation.CheckDigit)
                  .SetProperty(u => u.CompanyName, legalGeneralInformation.CompanyName)
                  .SetProperty(u => u.CompanyTypeId, legalGeneralInformation.CompanyTypeId)
                  .SetProperty(u => u.SocietyTypeId, legalGeneralInformation.SocietyTypeId)
                  .SetProperty(u => u.SocietyTypeDetail, legalGeneralInformation.SocietyTypeDetail)
                  .SetProperty(u => u.EconomicActivityId, legalGeneralInformation.EconomicActivityId)
                  .SetProperty(u => u.EconomicActivityDetail, legalGeneralInformation.EconomicActivityDetail)
                  .SetProperty(u => u.CIIUCode, legalGeneralInformation.CIIUCode)
                  .SetProperty(u => u.GreatContributorId, legalGeneralInformation.GreatContributorId)
                  .SetProperty(u => u.IsSelfRetaining, legalGeneralInformation.IsSelfRetaining)
                  .SetProperty(u => u.Fee, legalGeneralInformation.Fee)
                  .SetProperty(u => u.OriginResources, legalGeneralInformation.OriginResources)
                  .SetProperty(u => u.EmailCorporate, legalGeneralInformation.EmailCorporate)
                  .SetProperty(u => u.ElectronicInvoiceEmail, legalGeneralInformation.ElectronicInvoiceEmail)
                  .SetProperty(u => u.PhoneNumber, legalGeneralInformation.PhoneNumber)
                  .SetProperty(u => u.CountryId, legalGeneralInformation.CountryId)
                  .SetProperty(u => u.DepartmentId, legalGeneralInformation.DepartmentId)
                  .SetProperty(u => u.CityId, legalGeneralInformation.CityId)
                  .SetProperty(u => u.Address, legalGeneralInformation.Address)
                  .SetProperty(u => u.BranchAddress, legalGeneralInformation.BranchAddress)
                  .SetProperty(u => u.BranchPhoneNumber, legalGeneralInformation.BranchPhoneNumber)
                  .SetProperty(u => u.BranchDepartmentId, legalGeneralInformation.BranchDepartmentId)
                  .SetProperty(u => u.BranchCityId, legalGeneralInformation.BranchCityId)
                  .SetProperty(u => u.BranchContactName, legalGeneralInformation.BranchContactName)
                  .SetProperty(u => u.BranchDocumentNumberTypeId, legalGeneralInformation.BranchDocumentNumberTypeId)
                  .SetProperty(u => u.BranchDocumentNumber, legalGeneralInformation.BranchDocumentNumber)
                  .SetProperty(u => u.BranchContactPhone, legalGeneralInformation.BranchContactPhone)
                  .SetProperty(u => u.BranchEmailContact, legalGeneralInformation.BranchEmailContact)
                  .SetProperty(u => u.BranchPosition, legalGeneralInformation.BranchPosition)
                  .SetProperty(u => u.StatusDate, ExtensionFormat.DateTimeCO())
                  .SetProperty(u => u.Completed, legalGeneralInformation.Completed)
                  .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                  .SetProperty(u => u.ModifiedBy, legalGeneralInformation.Id));

            return true;
        }

        public async Task<GetLegalGeneralInformationResponse> GetLegalGeneralInformationAsync(Guid accountId)
        {
            var result = await (from A in _context.Accounts
                                from L in _context.LegalGeneralInformations.Where(x => x.Id == A.Id).DefaultIfEmpty()
                                from TC in _context.CatalogItems.Where(x => x.Id == L.CompanyTypeId).DefaultIfEmpty()
                                from TS in _context.CatalogItems.Where(x => x.Id == L.SocietyTypeId).DefaultIfEmpty()
                                from EA in _context.CatalogItems.Where(x => x.Id == L.EconomicActivityId).DefaultIfEmpty()
                                where A.Id == accountId || (L.Id == accountId && L.Status == true)
                                select new GetLegalGeneralInformationResponse
                                {
                                    Id = A.Id,
                                    Nit = string.IsNullOrEmpty(L.Nit) ? A.Nit : L.Nit,
                                    CompanyName = string.IsNullOrEmpty(L.CompanyName) ? A.SocialReason : L.CompanyName,
                                    CheckDigit = string.IsNullOrEmpty(L.CheckDigit) ? A.DigitVerify : L.CheckDigit,
                                    CompanyTypeId = L.CompanyTypeId,
                                    TypeCompanyDescription = TC.Descripton,
                                    SocietyTypeId = L.SocietyTypeId,
                                    TypeSocietyDescription = TS.Descripton,
                                    SocietyTypeDetail = L.SocietyTypeDetail,
                                    EconomicActivityId = L.EconomicActivityId,
                                    EconomicActivityDescription = EA.Descripton,
                                    EconomicActivityDetail = L.EconomicActivityDetail,
                                    CIIUCode = L.CIIUCode,
                                    GreatContributorId = L.GreatContributorId,
                                    IsSelfRetaining = L.IsSelfRetaining,
                                    Fee = L.Fee,
                                    OriginResources = L.OriginResources,
                                    EmailCorporate = L.EmailCorporate,
                                    ElectronicInvoiceEmail = L.ElectronicInvoiceEmail,
                                    PhoneNumber = L.PhoneNumber,
                                    CountryId = L.CountryId,
                                    DepartmentId = L.DepartmentId,
                                    CityId = L.CityId,
                                    Address = L.Address,
                                    BranchAddress = L.BranchAddress,
                                    BranchPhoneNumber = L.BranchPhoneNumber,
                                    BranchDepartmentId = L.BranchDepartmentId,
                                    BranchCityId = L.BranchCityId,
                                    BranchContactName = L.BranchContactName,
                                    BranchDocumentNumberTypeId = L.BranchDocumentNumberTypeId,
                                    BranchDocumentNumber = L.BranchDocumentNumber,
                                    BranchContactPhone = L.BranchContactPhone,
                                    BranchEmailContact = L.BranchEmailContact,
                                    BranchPosition = L.BranchPosition,
                                    Completed = L.Completed,
                                    StatusId = A.StatusId,
                                    StatusDate = L.StatusDate,
                                }).FirstOrDefaultAsync();

            return result ?? new();
        }
    }
}