///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using yourInvoice.Link.Application.Common.Behaviors;
using yourInvoice.Link.Application.Data;
using yourInvoice.Link.Domain.AccountRoles;
using yourInvoice.Link.Domain.Accounts;
using yourInvoice.Link.Domain.Document;
using yourInvoice.Link.Domain.LinkingProcesses.BankInformations;
using yourInvoice.Link.Domain.LinkingProcesses.ExposureInformations;
using yourInvoice.Link.Domain.LinkingProcesses.FinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalCommercialAndBankReference;
using yourInvoice.Link.Domain.LinkingProcesses.LegalFinancialInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalGeneralInformations;
using yourInvoice.Link.Domain.LinkingProcesses.LegalRepresentativeTaxAuditors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSAGRILAFT;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholders;
using yourInvoice.Link.Domain.LinkingProcesses.LegalShareholdersBoardDirectors;
using yourInvoice.Link.Domain.LinkingProcesses.LegalSignatureDeclarations;
using yourInvoice.Link.Domain.LinkingProcesses.LinkStatus;
using yourInvoice.Link.Domain.LinkingProcesses.Person;
using yourInvoice.Link.Domain.LinkingProcesses.PersonalReferences;
using yourInvoice.Link.Domain.LinkingProcesses.SignatureDeclaration;
using yourInvoice.Link.Domain.LinkingProcesses.WorkingInformations;
using yourInvoice.Link.Domain.Roles;
using yourInvoice.Link.Domain.Users;
using yourInvoice.Link.Infrastructure.Persistence;
using yourInvoice.Link.Infrastructure.Persistence.IRepositories;
using yourInvoice.Link.Infrastructure.Persistence.Repositories;

[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Resources.NeutralResourcesLanguage("en")]

namespace yourInvoice.Link
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLink(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssemblyContaining<ApplicationAssemblyReference>();
            });

            services.AddPersistenceLink(configuration);

            services.AddScoped(
                typeof(IPipelineBehavior<,>),
                typeof(ValidationBehavior<,>)
            );
            services.AddValidatorsFromAssemblyContaining<ApplicationAssemblyReference>();
            return services;
        }

        private static IServiceCollection AddPersistenceLink(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<LinkDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("SqlServer")));

            services.AddTransient<ILinkDbContext, LinkDbContext>();

            services.AddTransient<IUnitOfWorkLink, UnitOfWorkLink>();

            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IAccountRepository, AccountRepository>();
            services.AddTransient<IAccountRoleRepository, AccountRoleRepository>();
            services.AddTransient<IUserLinkRepository, UserLinkRepository>();
            services.AddTransient<IGeneralInformationRepository, GeneralInformationRepository>();
            services.AddTransient<IExposureInformationRepository, ExposureInformationRepository>();
            services.AddTransient<IWorkingInformationRepository, WorkingInformationRepository>();
            services.AddTransient<IBankInformationRepository, BankInformationRepository>();
            services.AddTransient<IFinancialInformationRepository, FinancialInformationRepository>();
            services.AddTransient<IPersonalReferenceRepository, PersonalReferenceRepository>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            services.AddTransient<ILegalGeneralInformationRepository, LegalGeneralInformationRepository>();
            services.AddTransient<ILegalRepresentativeTaxAuditorRepository, LegalRepresentativeTaxAuditorRepository>();
            services.AddTransient<ISignatureDeclarationRepository, SignatureDeclarationRepository>();
            services.AddTransient<ILegalShareholderBoardDirectorRepository, LegalShareholderBoardDirectorRepository>();
            services.AddTransient<ILegalShareholderRepository, LegalShareholderRepository>();
            services.AddTransient<ILegalBoardDirectorRepository, LegalBoardDirectorRepository>();
            services.AddTransient<ILegalFinancialInformationRepository, LegalFinancialInformationRepository>();
            services.AddTransient<ILegalCommercialAndBankReferenceRepository, LegalCommercialAndBankReferenceRepository>();
            services.AddTransient<ILegalSAGRILAFTRepository, LegalSAGRILAFTRepository>();
            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddTransient<ILegalSignatureDeclarationRepository, LegalSignatureDeclarationRepository>();
            services.AddTransient<ILinkStatusRepository, LinkStatusRepository>();

            return services;
        }
    }
}