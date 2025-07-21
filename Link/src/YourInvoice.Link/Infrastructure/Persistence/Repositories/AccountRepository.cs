///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************


using Microsoft.EntityFrameworkCore;
using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Entities;
using yourInvoice.Common.Extension;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Accounts.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.EconomicActivities;
using yourInvoice.Link.Domain.LinkingProcesses.Queries;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;


namespace yourInvoice.Link.Infrastructure.Persistence.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly LinkDbContext _context;

        public AccountRepository(LinkDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Account>> GetAllAccountAsync()
        {
            var result = await _context.Accounts.ToListAsync();

            return result;
        }

        public async Task<IEnumerable<EconomicActivity>> GetListEconomicActivityAsync()
        {
            var result = await _context.EconomicActivities.ToListAsync();

            return result;
        }

        public async Task<Account?> GetByEmailAsync(string email) => await _context.Accounts.SingleOrDefaultAsync(c => c.Email == email);

        public async Task<LinkStatus?> GetStatusLinkAsync(Guid IdUserLink) => await _context.LinkStatus.FirstOrDefaultAsync(c => c.IdUserLink == IdUserLink);

        public Account Add(Account account) => _context.Accounts.Add(account).Entity;

        public async Task<bool> ExistsByEmailAsync(string email) =>
            await _context.Accounts.AnyAsync(account => account.Email == email);

        public async Task<bool> ExistsByIdAsync(Guid id) =>
            await _context.Accounts.AnyAsync(account => account.Id == id);

        public async Task<bool> UpdateAsync(Account account)
        {
            if (account != null)
            {
                _context.Accounts.Update(account);

                return true;
            }
            else
            {
                return false; // Cuenta con el ID especificado no encontrada
            }
        }

        public async Task<ListDataInfo<ListResponse>?> GetListAsync(SearchInfo pagination)
        {
            string filter = pagination.filter.Trim().ToLower();

            var query = from A in _context.Accounts
                        join AR in _context.AccountRoles on A.Id equals AR.AccountId
                        join R in _context.Roles on AR.RoleId equals R.Id
                        from P in _context.CatalogItems.Where(x => x.Id == A.PersonTypeId).DefaultIfEmpty()
                        from D in _context.CatalogItems.Where(x => x.Id == A.DocumentTypeId).DefaultIfEmpty()
                        from S in _context.CatalogItems.Where(x => x.Id == A.StatusId).DefaultIfEmpty()
                        where ((string.IsNullOrEmpty(pagination.filter) || (string.Concat("", A.Name.Trim()
                                 , string.IsNullOrEmpty(A.SecondName) ? "" : " " + A.SecondName.Trim()
                                 , string.IsNullOrEmpty(A.LastName) ? "" : " " + A.LastName.Trim()
                               )).Trim().ToLower().Contains(filter))
                            || (string.IsNullOrEmpty(filter) || A.PhoneNumber.Trim().ToLower().Contains(filter))
                            || (string.IsNullOrEmpty(filter) || A.Email.Trim().ToLower().Contains(filter))
                            || (string.IsNullOrEmpty(filter) || R.Name.Trim().ToLower().Contains(filter))
                            || (string.IsNullOrEmpty(filter) || P.Name.Trim().ToLower().Contains(filter))
                            || (string.IsNullOrEmpty(filter) || D.Name.Trim().ToLower().Contains(filter))
                            || (string.IsNullOrEmpty(filter) || A.DocumentNumber.Trim().ToLower().Contains(filter))
                            || (string.IsNullOrEmpty(filter) || S.Name.Trim().ToLower().Contains(filter))

                        ) && AR.RoleId != CatalogCode_UserRole.Administrator
                        select new ListResponse
                        {
                            Id = A.Id,
                            NameOrder = (A.PersonTypeId == CatalogCode_PersonType.Natural) ? A.Name : A.SocialReason,
                            Name = (A.PersonTypeId == CatalogCode_PersonType.Natural) ? $"{A.Name ?? string.Empty} {A.SecondName ?? string.Empty} {A.LastName ?? string.Empty}" : A.SocialReason,
                            PhoneNumber = A.PhoneNumber,
                            Email = A.Email,
                            UserTypeId = R.Id,
                            UserType = R.Name,
                            PersonTypeId = A.PersonTypeId,
                            PersonType = P.Name,
                            DocumentTypeId = A.DocumentTypeId,
                            DocumentType = (A.PersonTypeId == CatalogCode_PersonType.Natural) ? D.Name : "NIT",
                            DocumentNumber = (A.PersonTypeId == CatalogCode_PersonType.Natural) ? A.DocumentNumber : A.Nit,
                            Time = A.StatusId == CatalogCode_StatusPreRegister.Pending ? Convert.ToInt16((ExtensionFormat.DateTimeCO() - Convert.ToDateTime(A.StatusDate)).TotalHours) : 0,
                            StatusId = A.StatusId,
                            Status = S.Name,
                            StatusDate = A.StatusDate,
                            CreatedOn = A.CreatedOn,
                            OrderRegister = A.StatusId == CatalogCode_StatusPreRegister.Pending ? 1 : A.StatusId == CatalogCode_StatusPreRegister.Approved ? 2 : 3,
                        };


            ListDataInfo<ListResponse> result = new();

            if (pagination.ColumnOrder == "time")
            {
                result = new ListDataInfo<ListResponse>
                {
                    Count = await query.CountAsync(),
                    Data = await query.ToListAsync()
                };
            }
            else
            {
                result = new ListDataInfo<ListResponse>
                {
                    Count = await query.CountAsync(),
                    Data = await query.Paginate(pagination).ToListAsync()
                };
            }

            return result;
        }

        public async Task<ListDataInfo<ListLinkingProccessResponse>?> GetListLinkingProcessesAsync(SearchInfo pagination)
        {
            string filter = pagination.filter.Trim().ToLower();
            Guid Nit = Guid.Parse("23A7CE80-A963-47DD-8737-EB3E497F3D5D");
            decimal porcentajeNatural = 100 / 6;
            decimal porcentajeJuridica = 100 / 7;

            var queryPorcentajeNatural = (from n in _context.GeneralInformations
                        join a in _context.Accounts on n.Id equals a.Id
                        join f in _context.SignatureDeclaration on n.Id equals f.Id_GeneralInformation into gj_f
                        from f in gj_f.DefaultIfEmpty()
                        join b in _context.BankInformations on n.Id equals b.Id_GeneralInformation into gj_b
                        from b in gj_b.DefaultIfEmpty()
                        join ex in _context.ExposureInformations on n.Id equals ex.Id_GeneralInformation into gj_ex
                        from ex in gj_ex.DefaultIfEmpty()
                        join w in _context.WorkingInformations on n.Id equals w.Id_GeneralInformation into gj_w
                        from w in gj_w.DefaultIfEmpty()
                        join r in _context.PersonalReference on n.Id equals r.Id_GeneralInformation into gj_r
                        from r in gj_r.DefaultIfEmpty()
                        where a.PersonTypeId == CatalogCode_PersonType.Natural

                        select new
                        {
                            Id = n.Id,
                            General = n.Completed == CatalogCode_FormStatus.Completed ? porcentajeNatural : 0,
                            Bank = b != null && b.Completed == CatalogCode_FormStatus.Completed ? porcentajeNatural : 0,
                            Declaration = f != null && f.Completed == CatalogCode_FormStatus.Completed ? porcentajeNatural : 0,
                            Exposure = ex != null && ex.Completed == CatalogCode_FormStatus.Completed ? porcentajeNatural : 0,
                            Working = w != null && w.Completed == CatalogCode_FormStatus.Completed ? porcentajeNatural : 0,
                            ReferencesP = r != null && r.Completed == CatalogCode_FormStatus.Completed ? porcentajeNatural : 0
                        }).Distinct();


            var queryNatural = from N in _context.GeneralInformations
                        join A in _context.Accounts on N.Id equals A.Id
                        join AR in _context.AccountRoles on A.Id equals AR.AccountId
                        join R in _context.Roles on AR.RoleId equals R.Id
                        join ST in _context.LinkStatus on N.Id equals ST.IdUserLink
                        join PN in queryPorcentajeNatural on N.Id equals PN.Id into PorNatural
                        from PN in PorNatural.DefaultIfEmpty()
                        from P in _context.CatalogItems.Where(x => x.Id == A.PersonTypeId).DefaultIfEmpty()
                        from D in _context.CatalogItems.Where(x => x.Id == A.DocumentTypeId).DefaultIfEmpty()
                        from S in _context.CatalogItems.Where(x => x.Id == ST.StatusLinkId).DefaultIfEmpty()
                        where ((string.IsNullOrEmpty(pagination.filter) || (string.Concat((N.FirstName ?? "").Trim()
                                 , string.IsNullOrEmpty(N.SecondName) ? "" : " " + N.SecondName.Trim()
                                 , string.IsNullOrEmpty(N.LastName) ? "" : " " + N.LastName.Trim()
                                 , string.IsNullOrEmpty(N.SecondLastName) ? "" : " " + N.SecondLastName.Trim()
                               )).Trim().ToLower().Contains(filter))
                              || (string.IsNullOrEmpty(filter) || R.Name.Trim().ToLower().Contains(filter))
                            || (string.IsNullOrEmpty(filter) || P.Name.Trim().ToLower().Contains(filter))
                            || (string.IsNullOrEmpty(filter) || D.Name.Trim().ToLower().Contains(filter))
                            || (string.IsNullOrEmpty(filter) || N.DocumentNumber.Trim().ToLower().Contains(filter))
                            || (string.IsNullOrEmpty(filter) || S.Name.Trim().ToLower().Contains(filter))

                        ) && AR.RoleId != CatalogCode_UserRole.Administrator
                        select new ListLinkingProccessResponse
                        {
                            Id = A.Id,
                            Name = $"{N.FirstName ?? string.Empty} {N.SecondName ?? string.Empty} {N.LastName ?? string.Empty} {N.SecondLastName ?? string.Empty}",
                            UserTypeId = R.Id,
                            UserType = R.Name,
                            PersonTypeId = A.PersonTypeId,
                            PersonType = P.Name,
                            DocumentTypeId = A.DocumentTypeId,
                            DocumentType = D.Name,
                            DocumentNumber = N.DocumentNumber,
                            CompletionPercentage = Convert.ToInt32(PN.General + PN.Bank + PN.Exposure + PN.Declaration + PN.ReferencesP + PN.Working ) ,
                            StatusId = ST.StatusLinkId,
                            Status = S.Name,
                            StatusDate = N.StatusDate,
                            CreatedOn = N.CreatedOn

                        };

            var queryPorcentajeJuridica = (from n in _context.LegalGeneralInformations
                                          join a in _context.Accounts on n.Id equals a.Id
                                          join f in _context.LegalSAGRILAFT on n.Id equals f.Id_LegalGeneralInformation into gj_f
                                          from f in gj_f.DefaultIfEmpty()
                                          join b in _context.LegalRepresentativeTaxAuditors on n.Id equals b.Id_LegalGeneralInformation into gj_b
                                          from b in gj_b.DefaultIfEmpty()
                                          join ex in _context.LegalCommercialAndBankReferences on n.Id equals ex.Id_LegalGeneralInformation into gj_ex
                                          from ex in gj_ex.DefaultIfEmpty()
                                          join w in _context.LegalFinancialInformations on n.Id equals w.Id_LegalGeneralInformation into gj_w
                                          from w in gj_w.DefaultIfEmpty()
                                          join r in _context.LegalShareholdersBoardDirectors on n.Id equals r.Id_LegalGeneralInformation into gj_r
                                          from r in gj_r.DefaultIfEmpty()
                                          join x in _context.LegalSignatureDeclarations on n.Id equals x.Id_LegalGeneralInformation into gj_x
                                          from x in gj_x.DefaultIfEmpty()
                                           
                                           where a.PersonTypeId == CatalogCode_PersonType.Juridica

                                          select new
                                          {
                                              Id = n.Id,
                                              General =  n.Completed == CatalogCode_FormStatus.Completed ? porcentajeJuridica : 0,
                                              Representative = b != null && b.Completed == CatalogCode_FormStatus.Completed ? porcentajeJuridica : 0,
                                              Sagrilaft = f != null && f.Completed == CatalogCode_FormStatus.Completed ? porcentajeJuridica : 0,
                                              Bank = ex != null && ex.Completed == CatalogCode_FormStatus.Completed ? porcentajeJuridica : 0,
                                              Financial = w != null && w.Completed == CatalogCode_FormStatus.Completed ? porcentajeJuridica : 0,
                                              Shareholders = r != null && r.Completed == CatalogCode_FormStatus.Completed ? porcentajeJuridica : 0,
                                              Declaration = x != null && x.Completed == CatalogCode_FormStatus.Completed ? porcentajeJuridica : 0,
                                          }).Distinct();

            var queryjuridica = from L in _context.LegalGeneralInformations
                        join A in _context.Accounts on L.Id equals A.Id
                        join AR in _context.AccountRoles on A.Id equals AR.AccountId
                        join R in _context.Roles on AR.RoleId equals R.Id
                        join ST in _context.LinkStatus on L.Id equals ST.IdUserLink
                        join PN in queryPorcentajeJuridica on L.Id equals PN.Id into Porjuridica
                        from PN in Porjuridica.DefaultIfEmpty()
                        from P in _context.CatalogItems.Where(x => x.Id == A.PersonTypeId).DefaultIfEmpty()
                        from D in _context.CatalogItems.Where(x => x.Id == Nit).DefaultIfEmpty()
                        from S in _context.CatalogItems.Where(x => x.Id == ST.StatusLinkId).DefaultIfEmpty()
                        where  AR.RoleId != CatalogCode_UserRole.Administrator
                        select new ListLinkingProccessResponse
                        {
                            Id = A.Id,
                            Name = L.CompanyName,
                            UserTypeId = R.Id,
                            UserType = R.Name,
                            PersonTypeId = A.PersonTypeId,
                            PersonType = P.Name,
                            DocumentTypeId = Nit,
                            DocumentType = D.Name,
                            DocumentNumber = $"{L.Nit ?? string.Empty}-{L.CheckDigit ?? string.Empty}",
                            CompletionPercentage = Convert.ToInt32(PN.General + PN.Bank + PN.Representative + PN.Declaration + PN.Sagrilaft + PN.Financial + PN.Shareholders),
                            StatusId = ST.StatusLinkId,
                            Status = S.Name,
                            StatusDate = L.StatusDate,
                            CreatedOn = L.CreatedOn
                        };

            ListDataInfo<ListLinkingProccessResponse> result = new();
            
            var dataNatural = await queryNatural.ToListAsync();
            var dataJuridica = await queryjuridica.ToListAsync();
            var countNatural = await queryNatural.CountAsync();
            var countJuridica = await queryjuridica.CountAsync();

            var countTotal = dataNatural.Count + dataJuridica.Count;
            var combinedData = dataNatural.Concat(dataJuridica).ToList();
            result = new ListDataInfo<ListLinkingProccessResponse>
            {
                Count = countTotal,
                Data = combinedData
            };

            return result;
        }

        public async Task<AccountResponse> GetByIdAsync(Guid Id)
        {
            var result = (from u in _context.Accounts
                          from s in _context.AccountRoles.Where(x => x.AccountId == u.Id).DefaultIfEmpty()
                          from c in _context.CatalogItems.Where(x => x.Id == u.StatusId).DefaultIfEmpty()
                          from a in _context.CatalogItems.Where(x => x.Id == u.PersonTypeId).DefaultIfEmpty()
                          from b in _context.CatalogItems.Where(x => x.Id == u.DocumentTypeId).DefaultIfEmpty()
                          from d in _context.CatalogItems.Where(x => x.Id == u.PersonTypeId).DefaultIfEmpty()
                          from e in _context.CatalogItems.Where(x => x.Id == u.PhoneCountryId).DefaultIfEmpty()
                          from f in _context.CatalogItems.Where(x => x.Id == u.ContactById).DefaultIfEmpty()
                          from g in _context.CatalogItems.Where(x => x.Id == u.MobileCountryId).DefaultIfEmpty()
                          from t in _context.CatalogItems.Where(x => x.Id == s.RoleId).DefaultIfEmpty()
                          where
                         u.Id == Id
                          select new AccountResponse
                          {
                              Id = u.Id,
                              PersonTypeId = u.PersonTypeId,
                              PersonType = a.Descripton,
                              RoleId = (Guid)s.RoleId,
                              CustomerType = t.Descripton,
                              Nit = u.Nit,
                              DigitVerify = u.DigitVerify,
                              SocialReason = u.SocialReason,
                              Name = u.Name,
                              SecondName = u.SecondName,
                              LastName = u.LastName,
                              SecondLastName = u.SecondLastName,
                              DocumentTypeId = u.DocumentTypeId,
                              DocumentType = b.Descripton,
                              DocumentNumber = u.DocumentNumber,
                              Email = u.Email,
                              MobileCountryId = u.MobileCountryId,
                              MobileCountry = g.Descripton,
                              MobileNumber = u.MobileNumber,
                              PhoneCountryId = u.PhoneCountryId,
                              PhoneCountry = e.Descripton,
                              PhoneNumber = u.PhoneNumber,
                              ContactById = u.ContactById,
                              ContactBy = f.Descripton,
                              StatusId = u.StatusId,
                              Status = c.Descripton,
                              StatusDate = u.StatusDate,
                              Date = u.StatusDate == null ? string.Empty : Convert.ToDateTime(u.StatusDate).DateddMMMyyyyHHmm(),
                          });

            return await result.SingleOrDefaultAsync();
        }

        public async Task<AccountResponse> GetAccountIdAsync(Guid id)
        {
            var result = await (from A in _context.Accounts
                                join R in _context.AccountRoles on A.Id equals R.AccountId
                                where A.Id == id
                                select new AccountResponse
                                {
                                    RoleId = R.RoleId ?? Guid.Empty,
                                    Id = A.Id,
                                    Name = A.Name ?? string.Empty,
                                    DocumentTypeId = A.DocumentTypeId,
                                    DocumentNumber = A.DocumentNumber,
                                    LastName = A.LastName ?? string.Empty,
                                    SecondLastName = A.SecondLastName ?? string.Empty,
                                    SecondName = A.SecondName ?? string.Empty,
                                    Email = A.Email,
                                    PhoneNumber = A.PhoneNumber,
                                    StatusDate = A.StatusDate ?? ExtensionFormat.DateTimeCO(),
                                    PersonTypeId = A.PersonTypeId,
                                    DigitVerify = A.DigitVerify,
                                    Nit = A.Nit,
                                    SocialReason = A.SocialReason ?? string.Empty,
                                }).FirstOrDefaultAsync();

            return result ?? new AccountResponse();
        }

        public async Task<bool> UpdateStatusAsync(Guid accountId, Guid statusId, Guid userId, int timeHour)
        {
            await _context.Accounts
                .Where(c => c.Id == accountId)
                .ExecuteUpdateAsync(p => p
                .SetProperty(u => u.ModifiedOn, ExtensionFormat.DateTimeCO())
                .SetProperty(u => u.ModifiedBy, userId)
                .SetProperty(u => u.Time, timeHour)
                .SetProperty(u => u.StatusId, statusId));

            return true;
        }
    }
}