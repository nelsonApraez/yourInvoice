///*** ProjectCustom Colombia
///*** Proyecto: ProjectCustom
///*** Año: 2024
///*********************************************

using yourInvoice.Common.Business.CatalogModule;
using yourInvoice.Common.Business.TransformModule;
using yourInvoice.Link.Domain.LinkingProcesses.GeneralInformations;
using yourInvoice.Common.Extension;
using yourInvoice.Offer.Domain.Users;
using yourInvoice.Common.Business.EmailModule;

namespace yourInvoice.Link.Application.LinkingProcess.EmailToAdminLinkingCompleted
{
    public class EmailLinkingCompletedCommandHandler : INotificationHandler<EmailLinkingCompletedCommand>
    {
        private readonly IGeneralInformationRepository generalInformationRepository;
        private readonly ICatalogBusiness catalogBusiness;
        private readonly IUserRepository userRepository;
        public Dictionary<string, string> AttachData { get; set; }

        public EmailLinkingCompletedCommandHandler( IGeneralInformationRepository generalInformationRepository, ICatalogBusiness catalogBusiness, IUserRepository userRepository)
        {

            this.generalInformationRepository = generalInformationRepository;
            this.catalogBusiness = catalogBusiness;
            this.userRepository = userRepository;
        }

        public async Task Handle(EmailLinkingCompletedCommand command, CancellationToken cancellationToken)
        {
            var generalInfo = await generalInformationRepository.GetGeneralInformationIdAsync(command.accountId);

            var templateAdmin = await this.catalogBusiness.GetByIdAsync(CatalogCode_Templates.EmailToAdminLinkingCompletedPN);
            var tipoCedula = await this.catalogBusiness.GetByIdAsync(CatalogCode_IdType.Cedula);
            var tipoExt = await this.catalogBusiness.GetByIdAsync(CatalogCode_IdType.CedulaExtrangeria);
            var tipoPasaporte = await this.catalogBusiness.GetByIdAsync(CatalogCode_IdType.Pasaporte);
            string TypeDocument;


            if (AttachData is null)
            {
                AttachData = new();
            }

            if (generalInfo.DocumentTypeId == tipoCedula.Id)
            {
                TypeDocument = tipoCedula.Descripton;
            }
            else if (generalInfo.DocumentTypeId == tipoExt.Id)
            {
                TypeDocument = tipoExt.Descripton;
            }
            else
            {
                TypeDocument = tipoPasaporte.Descripton;
            }

            var urlVinculacionNatural = await this.catalogBusiness.GetByIdAsync(CatalogCode_DatayourInvoice.urlVinculacionCompletedEmailToAdmin);

            string nombreAsusto = $"{generalInfo?.FirstName ?? string.Empty} {generalInfo?.SecondName ?? string.Empty} {generalInfo?.LastName ?? string.Empty} {generalInfo?.SecondLastName ?? string.Empty}";

            var asunto = "Se ha recibido una nueva solicitud de vinculación - " + nombreAsusto;


            AttachData.Add("{{PrimerNombre}}", generalInfo?.FirstName ?? string.Empty);
            AttachData.Add("{{SegundoNombre}}", generalInfo?.SecondName ?? string.Empty);
            AttachData.Add("{{PrimerApellido}}", generalInfo?.LastName ?? string.Empty);
            AttachData.Add("{{SegundoApellido}}", generalInfo?.SecondLastName ?? string.Empty);
            AttachData.Add("{{Documento}}", $"{TypeDocument} {generalInfo?.DocumentNumber.ToString()}");
            AttachData.Add("{{urlVinculacionNatural}}", $"{urlVinculacionNatural.Descripton}/{command.accountId}");
            AttachData.Add("{{year}}", ExtensionFormat.DateTimeCO().Year.ToString());
            string templateAdminWithData = TransformModule.ReplaceTokens(templateAdmin.Descripton, AttachData);
            EmainBusiness emainBusiness = new(this.catalogBusiness);
            var emailAdmin = await this.userRepository.GetEmailRoleAsync(CatalogCode_UserRole.Administrator);
            await emainBusiness.SendAsync(emailAdmin, asunto, templateAdminWithData);

        }
    }

}